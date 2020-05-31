using System.Threading.Tasks;
using MultiplayerAvalon.Configuration.Dto;

namespace MultiplayerAvalon.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
