using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AspnetBoilerplateExtended.EntityFrameworkCore;
using AspnetBoilerplateExtended.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace AspnetBoilerplateExtended.Web.Tests
{
    [DependsOn(
        typeof(AspnetBoilerplateExtendedWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class AspnetBoilerplateExtendedWebTestModule : AbpModule
    {
        public AspnetBoilerplateExtendedWebTestModule(AspnetBoilerplateExtendedEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AspnetBoilerplateExtendedWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(AspnetBoilerplateExtendedWebMvcModule).Assembly);
        }
    }
}