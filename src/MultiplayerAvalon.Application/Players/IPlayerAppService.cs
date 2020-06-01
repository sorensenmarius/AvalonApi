using Abp.Application.Services;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Players.Dto;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Players
{
    public interface IPlayerAppService : IApplicationService
    {

        public Task<GameDto> CreateAsync(CreatePlayerDto model);
    }
}
