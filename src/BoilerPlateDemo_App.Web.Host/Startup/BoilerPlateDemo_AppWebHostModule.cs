using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BoilerPlateDemo_App.Configuration;

namespace BoilerPlateDemo_App.Web.Host.Startup
{
    [DependsOn(
       typeof(BoilerPlateDemo_AppWebCoreModule))]
    public class BoilerPlateDemo_AppWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public BoilerPlateDemo_AppWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BoilerPlateDemo_AppWebHostModule).GetAssembly());
        }
    }
}
