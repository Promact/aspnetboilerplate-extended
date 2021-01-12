using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BoilerPlateDemo_App.Authorization;

namespace BoilerPlateDemo_App
{
    [DependsOn(
        typeof(BoilerPlateDemo_AppCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class BoilerPlateDemo_AppApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<BoilerPlateDemo_AppAuthorizationProvider>();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

        }

        public override void Initialize()
        {
            var thisAssembly = typeof(BoilerPlateDemo_AppApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );

           
        }
    }
}
