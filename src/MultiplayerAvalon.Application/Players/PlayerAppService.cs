using Abp.Domain.Repositories;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Players.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Players
{
    public class PlayerAppService : MultiplayerAvalonAppServiceBase, IPlayerAppService
    {
        private readonly IRepository<Game, Guid> _gameRepository;
        private readonly IRepository<Player, Guid> _playerRepository;
        public PlayerAppService(
            IRepository<Game, Guid> gameRepository,
            IRepository<Player, Guid> playerRepository
            )
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }
        public async Task<GameDto> CreateAsync(CreatePlayerDto model)
        {
            Player p = ObjectMapper.Map<Player>(model);
            List<Game> games = await _gameRepository.GetAllListAsync(item => item.JoinCode == model.JoinCode && item.Status == GameStatus.WaitingForPlayers);
            Game g = games[0];
            g.Players.Add(p);
            await _gameRepository.UpdateAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
    }
}
