using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MultiplayerAvalon.Authorization.Roles;
using MultiplayerAvalon.Authorization.Users;
using MultiplayerAvalon.MultiTenancy;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using MultiplayerAvalon.AppDomain.GameRoles;

namespace MultiplayerAvalon.EntityFrameworkCore
{
    public class MultiplayerAvalonDbContext : AbpZeroDbContext<Tenant, Role, User, MultiplayerAvalonDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public MultiplayerAvalonDbContext(DbContextOptions<MultiplayerAvalonDbContext> options)
            : base(options)
        {
        }
    }
}
