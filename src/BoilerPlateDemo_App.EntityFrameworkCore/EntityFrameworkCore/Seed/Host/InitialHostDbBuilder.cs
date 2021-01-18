using CETAutomation.EntityFrameworkCore.Seed;

namespace BoilerPlateDemo_App.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly BoilerPlateDemo_AppDbContext _context;

        public InitialHostDbBuilder(BoilerPlateDemo_AppDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new SeedData(_context).Create();

            _context.SaveChanges();
        }
    }
}
