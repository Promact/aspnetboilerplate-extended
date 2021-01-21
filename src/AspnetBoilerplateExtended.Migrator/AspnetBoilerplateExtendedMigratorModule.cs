using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AspnetBoilerplateExtended.Configuration;
using AspnetBoilerplateExtended.EntityFrameworkCore;
using AspnetBoilerplateExtended.Migrator.DependencyInjection;

namespace AspnetBoilerplateExtended.Migrator
{
    [DependsOn(typeof(AspnetBoilerplateExtendedEntityFrameworkModule))]
    public class AspnetBoilerplateExtendedMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AspnetBoilerplateExtendedMigratorModule(AspnetBoilerplateExtendedEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(AspnetBoilerplateExtendedMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                AspnetBoilerplateExtendedConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AspnetBoilerplateExtendedMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
