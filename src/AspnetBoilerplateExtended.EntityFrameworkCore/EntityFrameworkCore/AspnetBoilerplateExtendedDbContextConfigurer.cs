using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AspnetBoilerplateExtended.EntityFrameworkCore
{
    public static class AspnetBoilerplateExtendedDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AspnetBoilerplateExtendedDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AspnetBoilerplateExtendedDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
