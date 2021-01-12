using Abp.Timing;
using BoilerPlateDemo_App.EntityFrameworkCore;
using CETAutomation.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CETAutomation.EntityFrameworkCore.Seed
{
    public class SeedData
    {
        private readonly BoilerPlateDemo_AppDbContext _context;

        public SeedData(BoilerPlateDemo_AppDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            Initialize(_context);
        }


        private static void Initialize(BoilerPlateDemo_AppDbContext _dbContext)
        {



            #region Seed only Application
            List<Application> applicationList = new List<Application>()
                    {
                        new Application{
                        ApplicationName="ANPR", CreationTime = Clock.Now, CreatorUserId = 1, IsDeleted = false },
                        new Application{
                        ApplicationName="VMS", CreationTime = Clock.Now, CreatorUserId = 1, IsDeleted = false },
                         new Application{
                        ApplicationName="VA", CreationTime = Clock.Now, CreatorUserId = 1, IsDeleted = false },
                          new Application{
                        ApplicationName="ICCC", CreationTime = Clock.Now, CreatorUserId = 1, IsDeleted = false },

                    };
            var existingApplicationCount = _dbContext.Application.Count();
            //if fresh db then add only the HA for the system
            if (existingApplicationCount == 0)
            {
                _dbContext.Application.AddRange(applicationList);
                _dbContext.SaveChanges();
                #endregion
            }


        }
    }
}
