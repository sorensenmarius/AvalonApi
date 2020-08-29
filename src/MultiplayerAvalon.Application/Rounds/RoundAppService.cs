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
        private readonly IRepository<Round, Guid> _roundRepository;
        private readonly IRepository<Game, Guid> _gameRepository;
        private readonly IRepository<Player, Guid> _playerRepository;
        private readonly IHubContext<GameHub> _gameHub;
        public RoundAppService(
            IRepository<Round, Guid> roundRepository,
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
            Game g = _gameRepository.GetAll().Include("Players").Include("CurrentRound").FirstOrDefault(game => game.Id == model.GameId);
            if (model.Vote)
            {
                g.CurrentRound.VotesForTeam++;
            } else
            {
                g.CurrentRound.VotesAgainstTeam++;
            }
            await _gameRepository.UpdateAsync(g);
            if (g.CurrentRound.VotesForTeam + g.CurrentRound.VotesAgainstTeam >= g.Players.Count)
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
            if (model.Vote)
            {
                g.CurrentRound.MissionVoteGood++;
            }
            else
            {
                g.CurrentRound.MissionVoteBad++;
            }
            if (g.CurrentRound.MissionVoteGood + g.CurrentRound.MissionVoteBad >= g.CurrentRound.CurrentTeam.Count)
            {
                await ExpeditionResults(g);
            }
            else
            {
                await _gameRepository.UpdateAsync(g);
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
            g.CurrentRound.TeamString = String.Join(' ', g.CurrentRound.CurrentTeam.Select(p => p.Name));
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
