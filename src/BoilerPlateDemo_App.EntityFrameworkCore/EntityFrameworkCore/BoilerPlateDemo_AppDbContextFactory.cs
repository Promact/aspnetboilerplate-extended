using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BoilerPlateDemo_App.Configuration;
using BoilerPlateDemo_App.Web;

namespace BoilerPlateDemo_App.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class BoilerPlateDemo_AppDbContextFactory : IDesignTimeDbContextFactory<BoilerPlateDemo_AppDbContext>
    {
        public BoilerPlateDemo_AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BoilerPlateDemo_AppDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            BoilerPlateDemo_AppDbContextConfigurer.Configure(builder, configuration.GetConnectionString(BoilerPlateDemo_AppConsts.ConnectionStringName));

            return new BoilerPlateDemo_AppDbContext(builder.Options);
        }
    }
}
