using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MultiplayerAvalon.Configuration;

namespace MultiplayerAvalon.Web.Host.Startup
{
    [DependsOn(
       typeof(MultiplayerAvalonWebCoreModule))]
    public class MultiplayerAvalonWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MultiplayerAvalonWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MultiplayerAvalonWebHostModule).GetAssembly());
        }
    }
}
