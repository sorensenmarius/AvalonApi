using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MultiplayerAvalon.EntityFrameworkCore
{
    public static class MultiplayerAvalonDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MultiplayerAvalonDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MultiplayerAvalonDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
