using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using MultiplayerAvalon.EntityFrameworkCore.Seed;

namespace MultiplayerAvalon.EntityFrameworkCore
{
    [DependsOn(
        typeof(MultiplayerAvalonCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class MultiplayerAvalonEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<MultiplayerAvalonDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        MultiplayerAvalonDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        MultiplayerAvalonDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MultiplayerAvalonEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
