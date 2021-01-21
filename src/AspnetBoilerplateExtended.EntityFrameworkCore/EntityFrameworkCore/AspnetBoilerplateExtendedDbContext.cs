using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using AspnetBoilerplateExtended.Authorization.Roles;
using AspnetBoilerplateExtended.Authorization.Users;
using AspnetBoilerplateExtended.MultiTenancy;

namespace AspnetBoilerplateExtended.EntityFrameworkCore
{
    public class AspnetBoilerplateExtendedDbContext : AbpZeroDbContext<Tenant, Role, User, AspnetBoilerplateExtendedDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public AspnetBoilerplateExtendedDbContext(DbContextOptions<AspnetBoilerplateExtendedDbContext> options)
            : base(options)
        {
        }
    }
}
