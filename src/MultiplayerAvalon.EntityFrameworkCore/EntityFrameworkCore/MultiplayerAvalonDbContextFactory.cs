using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiplayerAvalon.Configuration;
using MultiplayerAvalon.Web;

namespace MultiplayerAvalon.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class MultiplayerAvalonDbContextFactory : IDesignTimeDbContextFactory<MultiplayerAvalonDbContext>
    {
        public MultiplayerAvalonDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MultiplayerAvalonDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            MultiplayerAvalonDbContextConfigurer.Configure(builder, configuration.GetConnectionString(MultiplayerAvalonConsts.ConnectionStringName));

            return new MultiplayerAvalonDbContext(builder.Options);
        }
    }
}
