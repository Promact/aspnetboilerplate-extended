using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using AspnetBoilerplateExtended.Configuration;
using AspnetBoilerplateExtended.Web;

namespace AspnetBoilerplateExtended.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AspnetBoilerplateExtendedDbContextFactory : IDesignTimeDbContextFactory<AspnetBoilerplateExtendedDbContext>
    {
        public AspnetBoilerplateExtendedDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AspnetBoilerplateExtendedDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            AspnetBoilerplateExtendedDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AspnetBoilerplateExtendedConsts.ConnectionStringName));

            return new AspnetBoilerplateExtendedDbContext(builder.Options);
        }
    }
}
