using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MultiplayerAvalon.Authorization;

namespace MultiplayerAvalon
{
    [DependsOn(
        typeof(MultiplayerAvalonCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MultiplayerAvalonApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MultiplayerAvalonAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MultiplayerAvalonApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
