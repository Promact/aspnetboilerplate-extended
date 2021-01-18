using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using BoilerPlateDemo_App.EntityFrameworkCore.Seed;
using Microsoft.EntityFrameworkCore;
using Abp.MultiTenancy;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;

namespace BoilerPlateDemo_App.EntityFrameworkCore
{
    [DependsOn(
        typeof(BoilerPlateDemo_AppCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class BoilerPlateDemo_AppEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<BoilerPlateDemo_AppDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        BoilerPlateDemo_AppDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        BoilerPlateDemo_AppDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BoilerPlateDemo_AppEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var dbContextProvider = IocManager.Resolve<IDbContextProvider<BoilerPlateDemo_AppDbContext>>();
            var unitOfWorkManager = IocManager.Resolve<IUnitOfWorkManager>();

            using (var unitOfWork = unitOfWorkManager.Begin())
            {
                var context = dbContextProvider.GetDbContext(MultiTenancySides.Host);

                //Removes actual connection as it has been enlisted in a non needed transaction for migration
                context.Database.CloseConnection();
                context.Database.Migrate();
            }

            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
