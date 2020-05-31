using Abp.Application.Services.Dto;

namespace MultiplayerAvalon.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

