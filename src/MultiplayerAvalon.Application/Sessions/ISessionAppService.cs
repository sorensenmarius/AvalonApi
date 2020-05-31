using System.Threading.Tasks;
using Abp.Application.Services;
using MultiplayerAvalon.Sessions.Dto;

namespace MultiplayerAvalon.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
