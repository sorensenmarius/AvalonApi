using Abp.Application.Services;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Players.Dto;
using System;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Games
{
    public interface IGameAppService : IApplicationService
    {

        public Task<GameDto> CreateAsync();
        public Task<GameDto> GetAsync(Guid id);
        public Task<GameDto> VoteExpedition(Guid GameId, Guid PlayerId, Boolean Status);
        public Task<GameDto> VoteExpeditionResult(Guid GameId, Guid PlayerId, Boolean Status);
    }
}
