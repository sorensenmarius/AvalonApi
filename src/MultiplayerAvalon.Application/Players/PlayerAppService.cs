using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
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
        public async Task<GamePlayerDto> CreateAsync(CreatePlayerDto model)
        {
            Player p = ObjectMapper.Map<Player>(model);
            p.Id = Guid.NewGuid();
            Game g = GameStore.GetGameByJoinCode(model.JoinCode);
            if (g.Players.Select(p => p.Name.ToLower()).Contains(model.Name.ToLower()))
                throw new UserFriendlyException("Name already taken", $"The name '{model.Name}' has already been taken by another player!");
            if(model.Name.Length > 20)
                throw new UserFriendlyException("Name too long", $"Name cannot be longer than 20 characters.");
            g.Players.Add(p);
            GameStore.AddOrUpdateGame(g);
            GamePlayerDto gp = new GamePlayerDto();
            gp.Game = g;
            gp.Player = p;
            return gp;
        }

    }
}
