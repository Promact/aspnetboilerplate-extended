using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BoilerPlateDemo_App.EntityFrameworkCore;
using BoilerPlateDemo_App.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace BoilerPlateDemo_App.Web.Tests
{
    [DependsOn(
        typeof(BoilerPlateDemo_AppWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class BoilerPlateDemo_AppWebTestModule : AbpModule
    {
        public BoilerPlateDemo_AppWebTestModule(BoilerPlateDemo_AppEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BoilerPlateDemo_AppWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(BoilerPlateDemo_AppWebMvcModule).Assembly);
        }
    }
}