using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AspnetBoilerplateExtended.Configuration;

namespace AspnetBoilerplateExtended.Web.Host.Startup
{
    [DependsOn(
       typeof(AspnetBoilerplateExtendedWebCoreModule))]
    public class AspnetBoilerplateExtendedWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AspnetBoilerplateExtendedWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AspnetBoilerplateExtendedWebHostModule).GetAssembly());
        }
    }
}
