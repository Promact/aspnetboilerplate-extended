using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CETAutomation.Applications.Dto;
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
        /// <returns>All application data with paged result</returns>
        Task<PagedResultDto<GetApplicationForViewDto>> GetAllAsync();


       


    }
}
