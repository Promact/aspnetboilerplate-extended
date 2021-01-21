using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AspnetBoilerplateExtended.EntityFrameworkCore
{
    public static class AspnetBoilerplateExtendedDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AspnetBoilerplateExtendedDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AspnetBoilerplateExtendedDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}
