using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CETAutomation.Export
{
  public  interface IFileExport
    {

        /// <summary>
        /// Method for creating excel worksheet
        /// </summary>
        /// <typeparam name="T">Generic Type data</typeparam>
        /// <param name="document">Sheet document</param>
        /// <param name="exportData">List to be exported</param>
        public void CreateWorksheetForExcel<T>(SpreadsheetDocument document, List<T> exportData);

        /// <summary>
        /// Generate memory stream data for download file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="fileName">file Name</param>
        /// <returns></returns>
        public Task<MemoryStream> GenerateMemoryStream(string path, string fileName);

        /// <summary>
        /// Create output file path 
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Path create file </returns>
        public string CreateFilePath(string fileName);
    }
}
