using Abp.Application.Services;
using MultiplayerAvalon.MultiTenancy.Dto;

namespace MultiplayerAvalon.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

