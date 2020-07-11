using Abp.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        public async Task<RoundDto> AddPlayerToTeam(ChangeCurrentTeamDto model)
        {
            Player p = await _playerRepository.GetAsync(model.PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound.CurrentTeam).ToListAsync();
            var game = g.Find(item => item.Id == model.GameId);
            game.CurrentRound.CurrentTeam.Add(p);
            await _gameRepository.UpdateAsync(game);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("GameUpdated");
            return ObjectMapper.Map<RoundDto>(game.CurrentRound);
        }
        public async Task<RoundDto> RemovePlayerFromTeam(ChangeCurrentTeamDto model)
        {
            Player p = await _playerRepository.GetAsync(model.PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound.CurrentTeam).ToListAsync();
            var game = g.Find(item => item.Id == model.GameId);
            game.CurrentRound.CurrentTeam.Remove(p);
            await _gameRepository.UpdateAsync(game);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("GameUpdated");
            return ObjectMapper.Map<RoundDto>(game.CurrentRound);
        }
        public async Task<RoundDto> VoteForTeam(Guid GameId, Guid PlayerId, bool Vote)
        {
            Player p = await _playerRepository.GetAsync(PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            if (Vote) game.CurrentRound.VotesForTeam++;
            await _gameRepository.UpdateAsync(game);
            return ObjectMapper.Map<RoundDto>(game.CurrentRound);
        }
        public async Task<RoundDto> ExpeditonVote(Guid GameId, Guid PlayerId, bool Vote)
        {
            Player p = await _playerRepository.GetAsync(PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            if (Vote) game.CurrentRound.MissionVoteGood++;
            System.Diagnostics.Debug.WriteLine(game.CurrentRound.MissionVoteGood);
            await _gameRepository.UpdateAsync(game);
            return ObjectMapper.Map<RoundDto>(game.CurrentRound);
        }
        public async Task<RoundDto> VoteForTeamResults(Guid GameId)
        {
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound).ToListAsync();
            List<Game> GwithPlayers = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            var GPlayers = GwithPlayers.Find(item => item.Id == GameId);
            var Votesfor = game.CurrentRound.VotesForTeam;
            game.CurrentRound.VotesAgainstTeam = game.Players.Count() - game.CurrentRound.VotesForTeam;
            if (Votesfor <= GPlayers.Players.Count() / 2)
            {
                game.CurrentRound.Status = RoundStatus.TeamDenied; // istedenfor dette, bare gå rett på neste spiller. 
                game.CurrentRound.FailedTeams++;
            }
            else game.CurrentRound.Status = RoundStatus.TeamApproved;
            game.CurrentPlayer = GPlayers.Players[game.counter++ % GPlayers.Players.Count()]; // Neste spiller uansett om den går gjennom eller ikke. 
            await _gameRepository.UpdateAsync(game);
            return ObjectMapper.Map<RoundDto>(game.CurrentRound);
        }
        public async Task<RoundDto> ExpeditionResults(Guid GameId)
        {
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound).ToListAsync();
            List<Game> GwithPlayers = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            var GPlayers = GwithPlayers.Find(item => item.Id == GameId);
            var statusen = GPlayers.Players.Count() - game.CurrentRound.MissionVoteGood;
            game.CurrentRound.MissionVoteBad = GPlayers.Players.Count() - game.CurrentRound.MissionVoteGood;
            if (statusen < GPlayers.Players.Count())
            {
                game.CurrentRound.Status = RoundStatus.MissionFailed;
                game.PointsEvil++;
            }
            else
            {
                game.CurrentRound.Status = RoundStatus.MissionSuccess;
                game.PointsInnocent++;
            }
            System.Diagnostics.Debug.WriteLine(game.CurrentRound.Status);
            await _gameRepository.UpdateAsync(game);
            return ObjectMapper.Map<RoundDto>(game.CurrentRound);
        }
        public async Task<HowManyPlayerHelper> HowManyPlayers(int RoundNmr, int TotalPlayers)
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
