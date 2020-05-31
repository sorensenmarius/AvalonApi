using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MultiplayerAvalon.Configuration.Dto;

namespace MultiplayerAvalon.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MultiplayerAvalonAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
