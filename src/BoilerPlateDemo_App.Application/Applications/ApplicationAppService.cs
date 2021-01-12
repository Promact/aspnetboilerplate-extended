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
using BoilerPlateDemo_App;
using Abp.Timing;
using CETAutomation.Export;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using CETAutomation.CacheStorage;

namespace CETAutomation.Applications
{
    

    public class ApplicationAppService : BoilerPlateDemo_AppAppServiceBase, IApplicationAppService

    {
        //These members set in constructor using constructor injection.
       
        private readonly IRepository<CETAutomation.Masters.Application,int> _applicationRepository;
        private readonly IFileExport _fileExportService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public ApplicationAppService(IRepository<CETAutomation.Masters.Application,int> applicationRepository,
                                        IFileExport fileExportService,
                                        ITempFileCacheManager tempFileCacheManager)
        {
            _applicationRepository = applicationRepository;
            _fileExportService = fileExportService;
            _tempFileCacheManager = tempFileCacheManager;
        }

       

        

        /// <summary>
        /// Method for getting all application data
        /// </summary>
        /// <param name="input">GetAllApplicationInput object </param>
        /// <returns>All application data with paged result</returns>
      
        public async Task<PagedResultDto<GetApplicationForViewDto>> GetAllAsync(GetAllApplicationInput input)
        {
           
            var filteredApplications = _applicationRepository.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ApplicationName.Trim().ToLower().Contains(input.Filter.Trim().ToLower()))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => false || e.ApplicationName.Trim().ToLower().Contains(input.NameFilter.Trim().ToLower())).AsQueryable();
            var pagedAndFilteredApplications = filteredApplications.OrderBy(input.Sorting??"id desc")
                .PageBy(input);
            var applications = from o in pagedAndFilteredApplications
                               select new GetApplicationForViewDto()
                               {
                                   Application = new ApplicationDto
                                   {
                                       ApplicationName = o.ApplicationName,
                                       Id = o.Id,
                                       CreationTime = o.CreationTime.ToLocalTime(),
                                   }
                               };

           
            var totalCount = await filteredApplications.CountAsync();

            return new PagedResultDto<GetApplicationForViewDto>(
                totalCount,
                 await applications.ToListAsync()
            );
        }

        /// <summary>
        /// Method for generation excel
        /// </summary>
        /// <param name="projectId">Id of project</param>
        /// <param name="dataSubmissionId">Id of data submission</param>
        /// <param name="offset">Offset of TimeZone</param>
        /// <returns>FileDto</returns>
        public async Task<FileDto> GetUsersToExcel()
        {
            try
            {

                var ApplicationList = await _applicationRepository.GetAll().AsNoTracking().ToListAsync();
                List<ApplicationDto> ApplicationDtoList = new List<ApplicationDto>();
                if (ApplicationDtoList.Count == 0)
                {
                    ApplicationDto reportDto = new ApplicationDto();
                    ApplicationDtoList.Add(reportDto);
                }

                ApplicationDtoList.ForEach((x) =>
                {
                    x.ApplicationName = (x.Id != 0 && x.ApplicationName != null) ? x.ApplicationName : AppConsts.DashSymbol;
                   
                });
                ApplicationDtoList = ObjectMapper.Map<List<ApplicationDto>>(ApplicationList);
                
                var applicationSortedList = ApplicationDtoList.OrderBy(x => x.CreationTime).ToList();




                string fileName = AppConsts.ExportFilename;
                string path = Path.GetTempPath();
                string outputFileName = _fileExportService.CreateFilePath(fileName);
                fileName = fileName + AppConsts.ExcelFileExtention;
                string token = DateTime.Now.Ticks + AppConsts.ExcelFileExtention;

                var file = new FileDto(fileName, AppConsts.ExcelFormat);

                using (SpreadsheetDocument package = SpreadsheetDocument.Create(outputFileName, SpreadsheetDocumentType.Workbook))
                {
                    _fileExportService.CreateWorksheetForExcel(package, applicationSortedList);
                }

                var memory = await _fileExportService.GenerateMemoryStream(path, fileName, token);


                _tempFileCacheManager.SetFile(file.FileToken, memory.ToArray());
                //Delete generate file from server
                File.Delete(Path.Combine(path, fileName));

                return file;

            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Method for getting application data by ID
        /// </summary>
        /// <param name="input">applicationId input</param>
        /// <returns>Data of given id</returns>
        
        public async Task<GetApplicationForEditOutput> GetApplicationAsync(EntityDto<int> input)
        {
            var app = await _applicationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicationForEditOutput { Applications = ObjectMapper.Map<CreateOrEditApplicationDto>(app) };

            return output;
        }

        /// <summary>
        /// Method for updating application
        /// </summary>
        /// <param name="input">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>
        
        public async Task UpdateApplicationAsync(CreateOrEditApplicationDto input)
        {
            if (await _applicationRepository.GetAll().AnyAsync(x => x.Id != input.Id && x.ApplicationName.Trim().ToLower().Equals(input.ApplicationName.Trim().ToLower())))
            {
                throw new UserFriendlyException(L(AppConsts.ApplicationIsAlreadyExist));
            }

            var applicationUpdate = await _applicationRepository.FirstOrDefaultAsync((int)input.Id);
            Regex rg = new Regex(AppConsts.AlphanumericRegex);
            Regex rgVp = new Regex(AppConsts.DecimalRegex);
            var isMatch = rg.IsMatch(input.ApplicationName);
            
            if (isMatch)
            {
                ObjectMapper.Map(input, applicationUpdate);

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
        /// <param name="input">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>
       
        public async Task CreateApplicationAsync(CreateOrEditApplicationDto input)
        {
            if (await _applicationRepository.GetAll().AnyAsync(x => x.ApplicationName.Trim().ToLower().Equals(input.ApplicationName.Trim().ToLower())))
            {
                throw new UserFriendlyException(L(AppConsts.ApplicationIsAlreadyExist));
            }

            var application = ObjectMapper.Map<CETAutomation.Masters.Application>(input);
            Regex rg = new Regex(AppConsts.AlphanumericRegex);
            Regex rgVp = new Regex(AppConsts.DecimalRegex);
            var isMatch = rg.IsMatch(input.ApplicationName);

           

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
        /// <param name="input">id of application</param>
        /// <returns>application data of given id</returns>
        
        public async Task<GetApplicationForEditOutput> GetApplicationForEditAsync(EntityDto input)
        {
            var applicationName = await _applicationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicationForEditOutput { Applications = ObjectMapper.Map<CreateOrEditApplicationDto>(applicationName) };

            return output;
        }

        /// <summary>
        /// Method for create or edit application
        /// </summary>
        /// <param name="input">CreateOrEditApplicationDto object</param>
        /// <returns>Task</returns>
        public async Task CreateOrEditAsync(CreateOrEditApplicationDto input)
        {
            if (input.Id == null)
            {
                await CreateApplicationAsync(input);
            }
            else
            {
                await UpdateApplicationAsync(input);
            }
        }

        /// <summary>
        /// Method for delete application
        /// </summary>
        /// <param name="input">application input</param>
        /// <returns>Task</returns>

        public async Task DeleteApplicationAsync(EntityDto<int> input)
        {
            var app = _applicationRepository.FirstOrDefault(x => x.Id == input.Id);

            if (app != null)
            {
                await _applicationRepository.DeleteAsync(input.Id);

            }
            else
            {
                throw new UserFriendlyException(L(AppConsts.DoesNotExist));

            }
        }
    }




}

