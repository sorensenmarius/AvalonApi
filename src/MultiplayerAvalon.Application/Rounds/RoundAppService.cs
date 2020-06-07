using Abp.Domain.Repositories;
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
            (var g, var p) = await GetGameAndPlayerAsync(GameId, PlayerId);
            g.CurrentRound.CurrentTeam.Add(p);
            await _gameRepository.UpdateAsync(g);
        }
        public async Task RemovePlayerFromTeam(Guid PlayerId, Guid GameId)
        {
            (var g, var p) = await GetGameAndPlayerAsync(GameId, PlayerId);
            g.CurrentRound.CurrentTeam.Remove(p);
            await _gameRepository.UpdateAsync(g);
        }
        public async Task VoteForTeam(Guid GameId, Guid PlayerId, bool Vote)
        {
            (var g, var p) = await GetGameAndPlayerAsync(GameId, PlayerId);
            if (Vote) g.CurrentRound.VotesForTeam++;
            await _gameRepository.UpdateAsync(g);
        }
        public async Task ExpeditonVote(Guid GameId, Guid PlayerId, bool Vote)
        {
            (var g, var p) = await GetGameAndPlayerAsync(GameId, PlayerId);
            if (Vote) g.CurrentRound.TeamExpVote++;
            await _gameRepository.UpdateAsync(g);
        }
        public async Task VoteForTeamResults(Guid GameId)
        {
            Game g = await _gameRepository.GetAsync(GameId);
            var Votesfor = g.CurrentRound.VotesForTeam;
            if (Votesfor <= g.Players.Count() / 2)
            {
                g.CurrentRound.Status = RoundStatus.TeamDenied; // istedenfor dette, bare gå rett på neste spiller. 
                g.CurrentPlayer = g.Players[g.counter++ % g.Players.Count()];
            }
            else g.CurrentRound.Status = RoundStatus.TeamApproved;
            await _gameRepository.UpdateAsync(g);
        }
        public async Task ExpeditionResults(Guid GameId)
        {
            Game g = await _gameRepository.GetAsync(GameId);
            var statusen = g.CurrentRound.TeamExpVote - g.Players.Count();
            if (statusen < g.Players.Count() / 2 || statusen == g.Players.Count() / 2)
            {
                g.CurrentRound.Status = RoundStatus.MissionFailed;
            }
            else g.CurrentRound.Status = RoundStatus.MissionSuccess;
            await _gameRepository.UpdateAsync(g);
        }
        public async Task<(Game game, Player player)> GetGameAndPlayerAsync(Guid GameId, Guid PlayerId)
        {
            Game g = await _gameRepository.GetAsync(GameId);
            Player p = await _playerRepository.GetAsync(PlayerId);
            return (g,p);
        }

      
    }
}
