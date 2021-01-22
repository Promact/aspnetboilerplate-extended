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
using CETAutomation.Export;
using CETAutomation.CacheStorage;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using AspnetBoilerplateExtended.Authorization;

namespace CETAutomation.Applications
{


    [AbpAuthorize(PermissionNames.Pages_Applications)]

    public class ApplicationAppService : AspnetBoilerplateExtendedAppServiceBase, IApplicationAppService

    {
        //These members set in constructor using constructor injection.
       
        private readonly IRepository<CETAutomation.Masters.Application,int> _applicationRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IFileExport _fileExportService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public ApplicationAppService(IRepository<CETAutomation.Masters.Application,int> applicationRepository
                                    ,IRepository<Project> projectRepository,
                                     IFileExport fileExportService,
                                     ITempFileCacheManager tempFileCacheManager)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _fileExportService = fileExportService;
            _tempFileCacheManager = tempFileCacheManager;
        }





        /// <summary>
        /// Method for getting all application data
        /// </summary>
        /// <param name="pageFormatData">GetAllApplicationInput object </param>
        /// <returns>All application data with paged result</returns>
        [AbpAuthorize(PermissionNames.Pages_Applications_View)]
        public async Task<PagedResultDto<GetApplicationForViewDto>> GetAllAsync(GetAllApplicationInput pageFormatData)
        {

            var filteredApplications = _applicationRepository.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(pageFormatData.Filter), e => false || e.ApplicationName.Trim().ToLower().Contains(pageFormatData.Filter.Trim().ToLower()))
                .WhereIf(!string.IsNullOrWhiteSpace(pageFormatData.NameFilter), e => false || e.ApplicationName.Trim().ToLower().Contains(pageFormatData.NameFilter.Trim().ToLower())).AsQueryable();
            var pagedAndFilteredApplications = filteredApplications.OrderBy(pageFormatData.Sorting ?? "id desc")
                .PageBy(pageFormatData);
            var applications = from o in pagedAndFilteredApplications
                               select new GetApplicationForViewDto()
                               {
                                   Application = new ApplicationDto
                                   {
                                       ApplicationName = o.ApplicationName,
                                       Id = o.Id,
                                       ProjectId=o.ProjectId,
                                      //ToLocalTime() will convert time to local time zone 
                                       CreationTime = o.CreationTime,
                                   }
                               };


            var totalCount = await filteredApplications.CountAsync();

            return new PagedResultDto<GetApplicationForViewDto>(
                totalCount,
                 await applications.ToListAsync()
            );
        }


        /// <summary>
        /// Method for getting application data by ID
        /// </summary>
        /// <param name="applicationData">applicationData Dto containing Id</param>
        /// <returns>Data of given id</returns>

        [AbpAuthorize(PermissionNames.Pages_Applications_View)]
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

        [AbpAuthorize(PermissionNames.Pages_Applications_Edit)]
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

        [AbpAuthorize(PermissionNames.Pages_Applications_Create)]
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
                application.CreationTime = Clock.Now;
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

        [AbpAuthorize(PermissionNames.Pages_Applications_Delete)]
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

        /// <summary>
        /// Method for generation excel
        /// </summary>
        /// <returns>FileDto</returns>
        [AbpAuthorize(PermissionNames.Pages_Applications_View)]
        public async Task<FileDto> GetUsersToExcel()
        {
            try
            {
                //Below code is will fetch data for export which will change as per requirement

                var ApplicationList = _applicationRepository.GetAll().AsNoTracking().Include(x => x.project).ToList();
                List<ApplicationDto> ApplicationDtoList = new List<ApplicationDto>();
                if (ApplicationList.Count == 0)
                {
                    ApplicationDto reportDto = new ApplicationDto();
                    ApplicationDtoList.Add(reportDto);
                }

                ApplicationDtoList = ObjectMapper.Map<List<ApplicationDto>>(ApplicationList);

                ApplicationDtoList.ForEach((x) =>
                {
                    x.ApplicationName = (x.Id != 0 && x.ApplicationName != null) ? x.ApplicationName : AppConsts.DashSymbol;
                    x.ProjectName = (x.Id != 0 && x.project != null) ? x.project.Name : AppConsts.DashSymbol;
                    x.Time = (x.CreationTime != null) ? x.CreationTime.ToShortDateString() : AppConsts.DashSymbol;

                });


                var applicationSortedList = ApplicationDtoList.OrderBy(x => x.CreationTime).ToList();


                // Below is File Generation code this will remain same for all export you just need to change parameter of createWorksheetForExcel method while calling.

                string fileName = AppConsts.ExportFilename;
                string path = Path.GetTempPath();
                string outputFileName = _fileExportService.CreateFilePath(fileName);
                fileName = fileName + AppConsts.ExcelFileExtention;


                var file = new FileDto(fileName, AppConsts.ExcelFormat);

                using (SpreadsheetDocument package = SpreadsheetDocument.Create(outputFileName, SpreadsheetDocumentType.Workbook))
                {
                    _fileExportService.CreateWorksheetForExcel(package, applicationSortedList);
                }

                var memory = await _fileExportService.GenerateMemoryStream(path, fileName);


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

    }




}

