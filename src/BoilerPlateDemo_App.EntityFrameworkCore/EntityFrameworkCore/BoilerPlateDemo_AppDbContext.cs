using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BoilerPlateDemo_App.Authorization.Roles;
using BoilerPlateDemo_App.Authorization.Users;
using BoilerPlateDemo_App.MultiTenancy;

namespace BoilerPlateDemo_App.EntityFrameworkCore
{
    public class BoilerPlateDemo_AppDbContext : AbpZeroDbContext<Tenant, Role, User, BoilerPlateDemo_AppDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public BoilerPlateDemo_AppDbContext(DbContextOptions<BoilerPlateDemo_AppDbContext> options)
            : base(options)
        {
        }
    }
}
