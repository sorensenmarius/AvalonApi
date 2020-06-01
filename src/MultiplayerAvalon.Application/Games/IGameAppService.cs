using Abp.Application.Services;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Players.Dto;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Games
{
    public interface IGameAppService : IApplicationService
    {

        public Task<GameDto> CreateAsync();
    }
}
