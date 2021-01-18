using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CETAutomation.Applications.Dto;
using CETAutomation.Export;
using CETAutomation.Masters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CETAutomation.Application
{

    public interface IApplicationAppService : IApplicationService
    
    {
        /// <summary>
        /// Method for getting all application data
        /// </summary>
        /// <param name="pageFormatData">GetAllApplicationInput object </param>
        /// <returns>All application data with paged result</returns>
        Task<PagedResultDto<GetApplicationForViewDto>> GetAllAsync(GetAllApplicationInput pageFormatData);


        /// <summary>
        /// Method for delete application
        /// </summary>
        /// <param name="applicationData">application input</param>
        /// <returns>Task</returns>
        public Task DeleteApplicationAsync(EntityDto<int> applicationData);

        /// <summary>
        /// Method for create or edit application
        /// </summary>
        /// <param name="application">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>
        public Task CreateOrEditAsync(CreateOrEditApplicationDto application);

        /// <summary>
        /// Method for getting data of application for edit
        /// </summary>
        /// <param name="applicationData">id of application</param>
        /// <returns>application data of given id</returns>

        public Task<GetApplicationForEditOutput> GetApplicationForEditAsync(EntityDto applicationData);

        /// <summary>
        /// Method for creating application
        /// </summary>
        /// <param name="newApplication">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>

        public Task CreateApplicationAsync(CreateOrEditApplicationDto newApplication);

        /// <summary>
        /// Method for updating application
        /// </summary>
        /// <param name="updatedApplication">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>

        public Task UpdateApplicationAsync(CreateOrEditApplicationDto updatedApplication);

        /// <summary>
        /// Method for getting application data by ID
        /// </summary>
        /// <param name="applicationData">object containing applicationId</param>
        /// <returns>Data of given id</returns>

        public  Task<GetApplicationForEditOutput> GetApplicationAsync(EntityDto<int> applicationData);

        /// <summary>
        /// Method for generation excel
        /// </summary>
        /// <returns>FileDto</returns>
        public  Task<FileDto> GetUsersToExcel();



        /// <summary>
        /// Method for getting all projects
        /// </summary>
        /// <returns>List of Projects</returns>

        public Task<List<Project>> GetAllProjects();



    }
}
