using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using AspnetBoilerplateExtended.EntityFrameworkCore.Seed;

namespace AspnetBoilerplateExtended.EntityFrameworkCore
{
    [DependsOn(
        typeof(AspnetBoilerplateExtendedCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class AspnetBoilerplateExtendedEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<AspnetBoilerplateExtendedDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        AspnetBoilerplateExtendedDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        AspnetBoilerplateExtendedDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AspnetBoilerplateExtendedEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
