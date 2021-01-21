using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using AspnetBoilerplateExtended.Authorization.Roles;
using AspnetBoilerplateExtended.Authorization.Users;
using AspnetBoilerplateExtended.MultiTenancy;
using CETAutomation.Masters;

namespace AspnetBoilerplateExtended.EntityFrameworkCore
{
    public class AspnetBoilerplateExtendedDbContext : AbpZeroDbContext<Tenant, Role, User, AspnetBoilerplateExtendedDbContext>
    {
        /* Define a DbSet for each entity of the application */
         //masters
        public DbSet<Application> Application { get; set; }
        public DbSet<Project> Project { get; set; }
        
        public AspnetBoilerplateExtendedDbContext(DbContextOptions<AspnetBoilerplateExtendedDbContext> options)
            : base(options)
        {
        }
    }
}
