using AspnetBoilerplateExtended.EntityFrameworkCore;
using CETAutomation.Masters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CETAutomation.EntityFrameworkCore.Seed
{
    public class SeedData
    {
        private readonly AspnetBoilerplateExtendedDbContext _context;

        public SeedData(AspnetBoilerplateExtendedDbContext context)
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
        private static void Initialize(AspnetBoilerplateExtendedDbContext _dbContext)
        {



            #region Seed only Project
            List<Project> ProjectList = new List<Project>()
                    {
                  
                
                  new Project{
                  Name="Project1", CreationTime=DateTime.Now, CreatorUserId=1,IsDeleted=false },
                  new Project{
                  Name="Project2", CreationTime=DateTime.Now, CreatorUserId=1,IsDeleted=false },
                  new Project{
                  Name="Project3", CreationTime=DateTime.Now, CreatorUserId=1,IsDeleted=false },
                new Project{
                  Name="Project4", CreationTime=DateTime.Now, CreatorUserId=1,IsDeleted=false },


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
