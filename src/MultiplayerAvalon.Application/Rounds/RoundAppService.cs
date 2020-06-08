using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Players.Dto;
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
        public RoundAppService(
           IRepository<Round, Guid> roundRepository,
           IRepository<Game, Guid> gameRepository,
           IRepository<Player, Guid> playerRepository
          )
        {
            _roundRepository = roundRepository;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }
        public async Task AddPlayerToTeam(Guid PlayerId, Guid GameId)
        {
            Player p = await _playerRepository.GetAsync(PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound.CurrentTeam).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            game.CurrentRound.CurrentTeam.Add(p);
            await _gameRepository.UpdateAsync(game);
        }
        public async Task RemovePlayerFromTeam(Guid PlayerId, Guid GameId)
        {
            Player p = await _playerRepository.GetAsync(PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound.CurrentTeam).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            game.CurrentRound.CurrentTeam.Remove(p);
            await _gameRepository.UpdateAsync(game);
        }
        public async Task VoteForTeam(Guid GameId, Guid PlayerId, bool Vote)
        {
            Player p = await _playerRepository.GetAsync(PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            if (Vote) game.CurrentRound.VotesForTeam++;
            await _gameRepository.UpdateAsync(game);
        }
        public async Task ExpeditonVote(Guid GameId, Guid PlayerId, bool Vote)
        {
            Player p = await _playerRepository.GetAsync(PlayerId);
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.CurrentRound).ToListAsync();
            var game = g.Find(item => item.Id == GameId);
            if (Vote) game.CurrentRound.MissionVoteGood++;
            System.Diagnostics.Debug.WriteLine(game.CurrentRound.MissionVoteGood);
            await _gameRepository.UpdateAsync(game);
        }
        public async Task VoteForTeamResults(Guid GameId)
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
        }
        public async Task ExpeditionResults(Guid GameId)
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
        }
    }
}
