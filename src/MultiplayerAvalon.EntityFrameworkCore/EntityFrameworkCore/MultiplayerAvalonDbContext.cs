using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MultiplayerAvalon.Authorization.Roles;
using MultiplayerAvalon.Authorization.Users;
using MultiplayerAvalon.MultiTenancy;

namespace MultiplayerAvalon.EntityFrameworkCore
{
    public class MultiplayerAvalonDbContext : AbpZeroDbContext<Tenant, Role, User, MultiplayerAvalonDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public MultiplayerAvalonDbContext(DbContextOptions<MultiplayerAvalonDbContext> options)
            : base(options)
        {
        }
    }
}
