using Abp.Application.Services;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Players.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Games
{
    public interface IGameAppService : IApplicationService
    {

        public Task<GameDto> CreateAsync();
        public Task<GameDto> StartGame(Guid id, List<string> roles,int Minions);
        public Task<GameDto> GetAsync(Guid id);
    }
}
