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
        private readonly IRepository<Project> _projectRepository;
        

        public ApplicationAppService(IRepository<CETAutomation.Masters.Application,int> applicationRepository
                                    ,IRepository<Project> projectRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
          
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

        /// <summary>
        /// Method for getting application data by ID
        /// </summary>
        /// <param name="applicationData">applicationData Dto containing Id</param>
        /// <returns>Data of given id</returns>

        public async Task<GetApplicationForEditOutput> GetApplicationAsync(EntityDto<int> applicationData)
        {
            var app = await _applicationRepository.FirstOrDefaultAsync(applicationData.Id);

            var output = new GetApplicationForEditOutput { Applications = ObjectMapper.Map<CreateOrEditApplicationDto>(app) };

            return output;
        }

        /// <summary>
        /// Method for updating application
        /// </summary>
        /// <param name="updatedApplication">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>

        public async Task UpdateApplicationAsync(CreateOrEditApplicationDto updatedApplication)
        {
            if (await _applicationRepository.GetAll().AnyAsync(x => x.Id != updatedApplication.Id && x.ApplicationName.Trim().ToLower().Equals(updatedApplication.ApplicationName.Trim().ToLower())))
            {
                throw new UserFriendlyException(L(AppConsts.ApplicationIsAlreadyExist));
            }

            var applicationUpdate = await _applicationRepository.FirstOrDefaultAsync((int)updatedApplication.Id);
            Regex rg = new Regex(AppConsts.AlphanumericRegex);
            Regex rgVp = new Regex(AppConsts.DecimalRegex);
            var isMatch = rg.IsMatch(updatedApplication.ApplicationName);

            if (isMatch)
            {
                ObjectMapper.Map(updatedApplication, applicationUpdate);

            }
            else
            {
                if (!isMatch)
                    throw new UserFriendlyException(L(AppConsts.UseOnlyAlphaNumericForApplication));

            }
        }

        /// <summary>
        /// Method for creating application
        /// </summary>
        /// <param name="newApplication">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>

        public async Task CreateApplicationAsync(CreateOrEditApplicationDto newApplication)
        {
            if (await _applicationRepository.GetAll().AnyAsync(x => x.ApplicationName.Trim().ToLower().Equals(newApplication.ApplicationName.Trim().ToLower())))
            {
                throw new UserFriendlyException(L(AppConsts.ApplicationIsAlreadyExist));
            }

            var application = ObjectMapper.Map<CETAutomation.Masters.Application>(newApplication);
            Regex rg = new Regex(AppConsts.AlphanumericRegex);
            Regex rgVp = new Regex(AppConsts.DecimalRegex);
            var isMatch = rg.IsMatch(newApplication.ApplicationName);



            if (isMatch)
            {
                await _applicationRepository.InsertAsync(application);
            }
            else
            {
                if (!isMatch)
                    throw new UserFriendlyException(L(AppConsts.UseOnlyAlphaNumericForApplication));


            }
        }



        /// <summary>
        /// Method for getting data of application for edit
        /// </summary>
        /// <param name="applicationData">id of application</param>
        /// <returns>application data of given id</returns>

        public async Task<GetApplicationForEditOutput> GetApplicationForEditAsync(EntityDto applicationData)
        {
            var applicationName = await _applicationRepository.FirstOrDefaultAsync(applicationData.Id);

            var output = new GetApplicationForEditOutput { Applications = ObjectMapper.Map<CreateOrEditApplicationDto>(applicationName) };

            return output;
        }

        /// <summary>
        /// Method for create or edit application
        /// </summary>
        /// <param name="application">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>
        public async Task CreateOrEditAsync(CreateOrEditApplicationDto application)
        {
            if (application.Id == null)
            {
                await CreateApplicationAsync(application);
            }
            else
            {
                await UpdateApplicationAsync(application);
            }
        }

        /// <summary>
        /// Method for delete application
        /// </summary>
        /// <param name="applicationData">application input</param>
        /// <returns>Task</returns>

        public async Task DeleteApplicationAsync(EntityDto<int> applicationData)
        {
            var app = _applicationRepository.FirstOrDefault(x => x.Id == applicationData.Id);

            if (app != null)
            {
                await _applicationRepository.DeleteAsync(applicationData.Id);

            }
            else
            {
                throw new UserFriendlyException(L(AppConsts.DoesNotExist));

            }
        }


        /// <summary>
        /// Method for getting all projects
        /// </summary>
        /// <returns>List of Projects</returns>

        public async Task<List<Project>> GetAllProjects()
        {
            return _projectRepository.GetAllList();
        }

    }




}

