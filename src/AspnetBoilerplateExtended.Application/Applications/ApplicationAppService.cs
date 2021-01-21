using Abp.Application.Services;
using Abp.Domain.Repositories;
using CETAutomation.Application;
using System;
using System.Collections.Generic;
using System.Text;
using CETAutomation.Masters;
using CETAutomation.Applications.Dto;
using System.Threading.Tasks;
using Abp.UI;
using System.Text.RegularExpressions;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.Collections.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.Timing;
using System.IO;
using AspnetBoilerplateExtended;

namespace CETAutomation.Applications
{
    

    public class ApplicationAppService : AspnetBoilerplateExtendedAppServiceBase, IApplicationAppService

    {
        //These members set in constructor using constructor injection.
       
        private readonly IRepository<CETAutomation.Masters.Application,int> _applicationRepository;
        

        public ApplicationAppService(IRepository<CETAutomation.Masters.Application,int> applicationRepository
                                         )
        {
            _applicationRepository = applicationRepository;
          
        }

       

        

        /// <summary>
        /// Method for getting all application data
        /// </summary>
        /// <returns>All application data with paged result</returns>
      
        public async Task<PagedResultDto<GetApplicationForViewDto>> GetAllAsync()
        {

            var ApplicationsList =await _applicationRepository.GetAllListAsync();
               
            var applications = from o in ApplicationsList
                               select new GetApplicationForViewDto()
                               {
                                   Application = new ApplicationDto
                                   {
                                       ApplicationName = o.ApplicationName,
                                       Id = o.Id,
                                       ProjectId=o.ProjectId,
                                       CreationTime = o.CreationTime.ToLocalTime(),
                                   }
                               };

           
            var totalCount = ApplicationsList.Count();

            return new PagedResultDto<GetApplicationForViewDto>(
                totalCount,
                 applications.ToList()
            );
        }

        
    }




}

