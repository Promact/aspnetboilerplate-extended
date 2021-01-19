using Abp.Auditing;
using BoilerPlateDemo_App;
using BoilerPlateDemo_App.Controllers;
using CETAutomation.CacheStorage;
using CETAutomation.Export;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CETAutomation.Controllers
{
  public  class FileController:BoilerPlateDemo_AppControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public FileController(

           ITempFileCacheManager tempFileCacheManager
       ) 
        {
            _tempFileCacheManager = tempFileCacheManager;
           
        }

        /// <summary>
        /// Method for downloading file
        /// </summary>
        /// <param name="file">File to download</param>
        /// <returns>File downloaded</returns>
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L(AppConsts.FileNotExistMessage));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}
