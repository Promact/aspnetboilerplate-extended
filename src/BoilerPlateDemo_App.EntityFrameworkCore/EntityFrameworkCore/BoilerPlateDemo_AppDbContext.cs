using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BoilerPlateDemo_App.Authorization.Roles;
using BoilerPlateDemo_App.Authorization.Users;
using BoilerPlateDemo_App.MultiTenancy;
using CETAutomation.Masters;

namespace BoilerPlateDemo_App.EntityFrameworkCore
{
    public class BoilerPlateDemo_AppDbContext : AbpZeroDbContext<Tenant, Role, User, BoilerPlateDemo_AppDbContext>
    {
        //masters
        public DbSet<Application> Application { get; set; }
        public DbSet<Project> Project { get; set; }
       


        public BoilerPlateDemo_AppDbContext(DbContextOptions<BoilerPlateDemo_AppDbContext> options)
            : base(options)
        {
       
        }
    }
}
