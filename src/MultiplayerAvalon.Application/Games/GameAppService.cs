using Abp.Domain.Repositories;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using MultiplayerAvalon.Games.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Games
{
    public class GameAppService : MultiplayerAvalonAppServiceBase, IGameAppService
    {
        private readonly IRepository<Game, Guid> _gameRepository;
        public GameAppService(
            IRepository<Game, Guid> gameRepository
            )
        {
            _gameRepository = gameRepository;
        }
        public async Task<GameDto> CreateAsync()
        {
            Game g = new Game();
            await _gameRepository.InsertAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> GetAsync(Guid id)
        {
            Game g = await _gameRepository.GetAsync(id);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> StartGame(Guid id)
        {
            Game g = await _gameRepository.GetAsync(id);
            Round r = await CreateNewRound();
            g.CurrentRound = r;
            g.Status = GameStatus.Playing;
            //g.CurrentPlayer = g.Players[g.counter];
            await _gameRepository.UpdateAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<Round> CreateNewRound()
        {
            Round r = new Round();
            r.CurrentTeam = new List<Player>();
            return r;
        }

        public Task<GameDto> VoteExpedition(Guid GameId, Guid PlayerId, bool Status)
        {
            throw new NotImplementedException();
        }

        public Task<GameDto> VoteExpeditionResult(Guid GameId, Guid PlayerId, bool Status)
        {
            throw new NotImplementedException();
        }
    }
}
