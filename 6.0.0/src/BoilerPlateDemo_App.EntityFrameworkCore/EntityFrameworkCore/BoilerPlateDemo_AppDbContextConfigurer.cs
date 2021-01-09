using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlateDemo_App.EntityFrameworkCore
{
    public static class BoilerPlateDemo_AppDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BoilerPlateDemo_AppDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BoilerPlateDemo_AppDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}
