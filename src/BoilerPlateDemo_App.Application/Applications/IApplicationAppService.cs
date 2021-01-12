using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CETAutomation.Applications.Dto;
using CETAutomation.Export;
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
        /// <param name="input">GetAllApplicationInput object </param>
        /// <returns>All application data with paged result</returns>
        Task<PagedResultDto<GetApplicationForViewDto>> GetAllAsync(GetAllApplicationInput input);


        /// <summary>
        /// Method for delete application
        /// </summary>
        /// <param name="input">application input</param>
        /// <returns>Task</returns>
        public Task DeleteApplicationAsync(EntityDto<int> input);

        /// <summary>
        /// Method for create or edit application
        /// </summary>
        /// <param name="input">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>
        public Task CreateOrEditAsync(CreateOrEditApplicationDto input);

        /// <summary>
        /// Method for getting data of application for edit
        /// </summary>
        /// <param name="input">id of application</param>
        /// <returns>application data of given id</returns>

        public  Task<GetApplicationForEditOutput> GetApplicationForEditAsync(EntityDto input);

        /// <summary>
        /// Method for creating application
        /// </summary>
        /// <param name="input">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>

        public  Task CreateApplicationAsync(CreateOrEditApplicationDto input);

        /// <summary>
        /// Method for updating application
        /// </summary>
        /// <param name="input">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>

        public Task UpdateApplicationAsync(CreateOrEditApplicationDto input);

        /// <summary>
        /// Method for getting application data by ID
        /// </summary>
        /// <param name="input">applicationId input</param>
        /// <returns>Data of given id</returns>

        public  Task<GetApplicationForEditOutput> GetApplicationAsync(EntityDto<int> input);

        /// <summary>
        /// Method for generation excel
        /// </summary>
        /// <param name="projectId">Id of project</param>
        /// <param name="dataSubmissionId">Id of data submission</param>
        /// <param name="offset">Offset of TimeZone</param>
        /// <returns>FileDto</returns>
        public  Task<FileDto> GetUsersToExcel();



    }
}
