using Abp.Domain.Repositories;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.Games.Dto;
using System;
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
    }
}
