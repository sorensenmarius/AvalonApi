using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MultiplayerAvalon.Controllers
{
    public abstract class MultiplayerAvalonControllerBase: AbpController
    {
        protected MultiplayerAvalonControllerBase()
        {
            LocalizationSourceName = MultiplayerAvalonConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
