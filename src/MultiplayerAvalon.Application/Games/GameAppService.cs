using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.Games.Dto;
using System;
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
        /// <summary>
        /// Gets a game by ID, populated with players
        /// </summary>
        /// TODO - Refactor this to a stored procedure in the DB
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GameDto> GetAsync(Guid id)
        {
            List<Game> g = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            return ObjectMapper.Map<GameDto>(g.Find(item => item.Id == id));
        }
    }
}
