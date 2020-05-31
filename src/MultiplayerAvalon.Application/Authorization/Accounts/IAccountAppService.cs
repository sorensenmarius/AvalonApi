using System.Threading.Tasks;
using Abp.Application.Services;
using MultiplayerAvalon.Authorization.Accounts.Dto;

namespace MultiplayerAvalon.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
