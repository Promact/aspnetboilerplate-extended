using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AspnetBoilerplateExtended.Authorization;

namespace AspnetBoilerplateExtended
{
    [DependsOn(
        typeof(AspnetBoilerplateExtendedCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class AspnetBoilerplateExtendedApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AspnetBoilerplateExtendedAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AspnetBoilerplateExtendedApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
