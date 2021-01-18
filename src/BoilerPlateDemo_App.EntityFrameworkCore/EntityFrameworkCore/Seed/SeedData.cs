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

        /// <summary>
        /// Method for seeding project data into database
        /// </summary>
        /// <param name="_dbContext">Database context object</param>
        /// <returns>void</returns>
        private static void Initialize(BoilerPlateDemo_AppDbContext _dbContext)
        {



            #region Seed only Project
            List<Project> ProjectList = new List<Project>()
                    {
                  
                
                  new Project{
                  Name="Project1", CreationTime=Clock.Now, CreatorUserId=1,IsDeleted=false },
                  new Project{
                  Name="Project2", CreationTime=Clock.Now, CreatorUserId=1,IsDeleted=false },
                  new Project{
                  Name="Project3", CreationTime=Clock.Now, CreatorUserId=1,IsDeleted=false },
                new Project{
                  Name="Project4", CreationTime=Clock.Now, CreatorUserId=1,IsDeleted=false },


                    };
            var existingProjectCount = _dbContext.Project.Count();
            //if fresh db then add only the HA for the system
            if (existingProjectCount == 0)
            {
                _dbContext.Project.AddRange(ProjectList);
                _dbContext.SaveChanges();
                #endregion
            }


        }
    }
}
