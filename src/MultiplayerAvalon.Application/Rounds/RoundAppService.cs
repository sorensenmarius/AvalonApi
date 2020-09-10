using Abp.Domain.Repositories;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using MultiplayerAvalon.Games;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Players.Dto;
using MultiplayerAvalon.Rounds.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Rounds
{
    public class RoundAppService : MultiplayerAvalonAppServiceBase, IRoundAppService
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IRepository<Game, Guid> _gameRepository;
        private readonly IRepository<Player, Guid> _playerRepository;
        private readonly IHubContext<GameHub> _gameHub;
        public RoundAppService(
            IRoundRepository roundRepository,
            IRepository<Game, Guid> gameRepository,
            IRepository<Player, Guid> playerRepository,
            IHubContext<GameHub> gameHub

          )
        {
            _roundRepository = roundRepository;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _gameHub = gameHub;
        }

        public async Task SetTeam(SetTeamDto model)
        {
            Game g = _gameRepository.GetAll().Include("CurrentRound.CurrentTeam").Include("Players").FirstOrDefault(g => g.Id == model.GameId);
            List<Player> team = new List<Player>();
            g.CurrentRound.CurrentTeam.Clear();
            foreach (Guid playerId in model.CurrentTeam)
            {
                team.Add(await _playerRepository.GetAsync(playerId));
            }
            g.CurrentRound.CurrentTeam = team;
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateHost");
        }
        public async Task<Round> VoteForTeam(VoteDto model)
        {
            Player p = _playerRepository.Get(model.PlayerId);
            System.Diagnostics.Debug.WriteLine($"{p.Name} voted {model.Vote}");
            Game g = _gameRepository.GetAll().Include("CurrentRound").Include("Players").FirstOrDefault(game => game.Id == model.GameId);
            var totalVotes = 0;
            if (model.Vote)
            {
                totalVotes = await _roundRepository.VoteForTeam(g.CurrentRound.Id);
            } else
            {
                totalVotes = await _roundRepository.VoteAgainstTeam(g.CurrentRound.Id);
            }
            System.Diagnostics.Debug.WriteLine($"{p.Name} updated game in DB");
            if (totalVotes >= g.Players.Count)
            {
                await VoteForTeamResults(g);
            } else
            {
                await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateHost");
            }
            return g.CurrentRound;
        }
        public async Task<Round> ExpeditonVote(VoteDto model)
        {
            Player p = await _playerRepository.GetAsync(model.PlayerId);
            Game g = _gameRepository.GetAll().Include("CurrentRound.CurrentTeam").FirstOrDefault(game => game.Id == model.GameId);
            var totalVotes = 0;
            if (model.Vote)
            {
                totalVotes = await _roundRepository.VoteForExpedition(g.CurrentRound.Id);
            }
            else
            {
                totalVotes = await _roundRepository.VoteAgainstExpedition(g.CurrentRound.Id);
            }
            if (totalVotes >= g.CurrentRound.CurrentTeam.Count)
            {
                await ExpeditionResults(g);
            }
            else
            {
                await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateHost");
            }
            return g.CurrentRound;
        }

        public async Task SetRoundStatus(SetRoundStatusDto model)
        {
            var g = _gameRepository.GetAll().Include("CurrentRound").FirstOrDefault(game => game.Id == model.GameId);
            g.CurrentRound.Status = model.Status;
            if (model.Status == RoundStatus.SelectingTeam)
            {
                g.CurrentRound.VotesForTeam = 0;
                g.CurrentRound.VotesAgainstTeam = 0;
            }
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateAll");
        }
        
        private async Task VoteForTeamResults(Game g)
        {
            var Votesfor = g.CurrentRound.VotesForTeam;
            if (Votesfor <= g.Players.Count() / 2)
            {
                g.CurrentRound.Status = RoundStatus.TeamDenied;
                g.CurrentRound.FailedTeams++;
            } else
            {
                g.CurrentRound.Status = RoundStatus.TeamApproved;
            }
            g.Counter++;
            g.CurrentPlayer = g.Players[g.Counter % g.Players.Count()];
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
        }
        private async Task ExpeditionResults(Game g)
        {
            if (g.CurrentRound.MissionVoteBad > 0)
            {
                g.CurrentRound.Status = RoundStatus.MissionFailed;
                g.PointsEvil++;
            }
            else
            {
                g.CurrentRound.Status = RoundStatus.MissionSuccess;
                g.PointsInnocent++;
            }
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
        }
        public async Task SubmitTeam(GameAndPlayerIdDto model)
        {
            // TODO - Handle team size. Throw error if incorrect team size
            Game g = await _gameRepository.GetAll()
                                        .Include("CurrentRound")
                                        .FirstOrDefaultAsync(game => game.Id == model.GameId);
            var round = await _roundRepository.GetAsync(g.CurrentRound.Id);
            round.Status = RoundStatus.VotingForTeam;
            await _roundRepository.UpdateAsync(round);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateAll");
        }
        public HowManyPlayerHelper HowManyPlayers(int RoundNmr, int TotalPlayers)
        {
            bool DoubleRound = false;
            int[,] NmrPlayerBasedOnRound = new int[5,6] { { 2, 2, 2, 3, 3, 3 }, { 3, 3, 3, 4, 4, 4 }, { 2, 4, 3, 4, 4, 4 }, { 3, 3, 4, 5, 5, 5 }, { 3, 4, 4, 5, 5, 5 } };
            if(RoundNmr == 4 && TotalPlayers >= 7)
            {
                DoubleRound = true;
            }
            int HowMany = NmrPlayerBasedOnRound[RoundNmr-1, TotalPlayers-5];
            return new HowManyPlayerHelper(HowMany,DoubleRound);
        }
        public class HowManyPlayerHelper
        {
            public int HowMany;
            public bool DoubleRound;
            public HowManyPlayerHelper(int howMany, bool doubleRound)
            {
                HowMany = howMany;
                DoubleRound = doubleRound;
            }
        }

    }
}
