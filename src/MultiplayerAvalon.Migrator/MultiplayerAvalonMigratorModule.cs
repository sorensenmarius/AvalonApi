using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MultiplayerAvalon.Configuration;
using MultiplayerAvalon.EntityFrameworkCore;
using MultiplayerAvalon.Migrator.DependencyInjection;

namespace MultiplayerAvalon.Migrator
{
    [DependsOn(typeof(MultiplayerAvalonEntityFrameworkModule))]
    public class MultiplayerAvalonMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public MultiplayerAvalonMigratorModule(MultiplayerAvalonEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(MultiplayerAvalonMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                MultiplayerAvalonConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MultiplayerAvalonMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
