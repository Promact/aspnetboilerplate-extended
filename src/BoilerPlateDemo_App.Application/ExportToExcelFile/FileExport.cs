using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using BoilerPlateDemo_App;

namespace CETAutomation.Export
{
    public class FileExport : IFileExport
    {


        public FileExport()
        {

        }

        /// <summary>
        /// Method for creating excel worksheet
        /// </summary>
        /// <typeparam name="T">Generic Type data</typeparam>
        /// <param name="document">Sheet document</param>
        /// <param name="exportData">List to be exported</param>
        public void CreateWorksheetForExcel<T>(SpreadsheetDocument document, List<T> exportData)
        {
            SheetData partSheetData = GenerateSheetData(exportData);

            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookSheet(workbookPart1);

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData);
        }

        /// <summary>
        /// functions to add exportData into Excel
        /// </summary>
        /// <typeparam name="T">Generic Type data</typeparam>
        /// <param name="exportData">Eport data</param>
        /// <returns>Generated Sheet data</returns>
        private SheetData GenerateSheetData<T>(List<T> exportData)
        {
            SheetData sheetData = new SheetData();
           
            T model = exportData[0];
            sheetData.Append(CreateHeaderForExcel(model));
            foreach (T data in exportData)
            {
                Row partsRows = GenerateRowData(data);
                sheetData.Append(partsRows);
            }

            return sheetData;
        }
          
        
        /// <summary>
        /// Generating workbook Sheet 
        /// </summary>
        /// <param name="workbookPart1">Current work book</param>
        private void GenerateWorkbookSheet(WorkbookPart workbookPart)
        {
            Workbook workbook = new Workbook();
            Sheets sheets = new Sheets();
            Sheet sheet = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
            sheets.Append(sheet);
            workbook.Append(sheets);
            workbookPart.Workbook = workbook;
        }

        /// <summary>
        ///  created for creating Header rows 
        /// </summary>
        /// <typeparam name="T">Generic Param</typeparam>
        /// <param name="model">Model Property</param>
        /// <returns>Generate Header Row</returns>
        private Row CreateHeaderForExcel<T>(T model)
        {
            Row workRow = new Row();
            Row tRow = new Row();
            Type type = model.GetType();
            PropertyInfo[] props = type.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                //chcek property is required in exported excel file
                var prop = props[i].GetCustomAttributes(typeof(ExportAttribute), false);

                //Check display name is available or not
                var atts = props[i].GetCustomAttributes(typeof(DisplayNameAttribute), true);

                if (prop.Length == 0)
                {
                    if (atts.Length != 0)
                    {
                        string displayName = (atts[0] as DisplayNameAttribute).DisplayName;
                        workRow.Append(CreateCell(displayName, 2U));
                    }
                    else
                    {
                        workRow.Append(CreateCell(props[i].Name, 2U));
                    }
                }
            }
            return workRow;
        }

        /// <summary>
        /// generating child rows
        /// </summary>
        /// <typeparam name="T">Generic Datatype</typeparam>
        /// <param name="exportData">Export data </param>
        /// <returns>Create row for exported data</returns>
        private Row GenerateRowData<T>(T exportData)
        {
            Row tRow = new Row();
            Type type = exportData.GetType();
            PropertyInfo[] props = type.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i].GetCustomAttributes(typeof(ExportAttribute), false);
                if (prop.Length == 0)
                {
                    if (props[i].GetValue(exportData) == null)
                    {
                        tRow.Append(CreateCell(""));
                    }
                    else
                    {
                        tRow.Append(CreateCell(props[i].GetValue(exportData).ToString()));
                    }
                }
            }
            return tRow;
        }
        /// <summary>
        /// creating a cell by passing cell exportData 
        /// </summary>
        /// <param name="text">Created cell data type</param>
        /// <returns>Created Cell </returns>
        private Cell CreateCell(string text)
        {
            Cell cell = new Cell();
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(text);
            return cell;
        }

        /// <summary>
        /// creating a cell by passing cell exportData and cell style
        /// </summary>
        /// <param name="text">Created cell data type</param>
        /// <param name="styleIndex">Style of created cell</param>
        /// <returns>Created Cell </returns>
        private Cell CreateCell(string text, uint styleIndex)
        {
            Cell cell = new Cell();
            cell.StyleIndex = styleIndex;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(text);
            return cell;
        }
        /// <summary>
        /// created for resolving the exportData type of numeric value in a cell
        /// </summary>
        /// <param name="text">Current cell data</param>
        /// <returns>Cell value based on data type</returns>
        private EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
        {
            int intVal;
            double doubleVal;
            if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
            {
                return CellValues.Number;
            }
            else
            {
                return CellValues.String;
            }
        }
        /// <summary>
        /// Apply style on current work book section
        /// </summary>
        /// <param name="workbookStylesPart">Current work book part</param>
        private void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart)
        {
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            DocumentFormat.OpenXml.Spreadsheet.FontScheme fontScheme1 = new DocumentFormat.OpenXml.Spreadsheet.FontScheme() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);

            Font font2 = new Font();
            Bold bold1 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = 11D };
            Color color2 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName2 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
            DocumentFormat.OpenXml.Spreadsheet.FontScheme fontScheme2 = new DocumentFormat.OpenXml.Spreadsheet.FontScheme() { Val = FontSchemeValues.Minor };

            font2.Append(bold1);
            font2.Append(fontSize2);
            font2.Append(color2);
            font2.Append(fontName2);
            font2.Append(fontFamilyNumbering2);
            font2.Append(fontScheme2);

            fonts1.Append(font1);
            fonts1.Append(font2);

            Fills fills1 = new Fills() { Count = (UInt32Value)2U };

            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

            fill1.Append(patternFill1);

            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

            fill2.Append(patternFill2);

            fills1.Append(fill1);
            fills1.Append(fill2);

            Borders borders1 = new Borders() { Count = (UInt32Value)2U };

            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            Border border2 = new Border();

            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            Color color3 = new Color() { Indexed = (UInt32Value)64U };

            leftBorder2.Append(color3);

            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            Color color4 = new Color() { Indexed = (UInt32Value)64U };

            rightBorder2.Append(color4);

            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            Color color5 = new Color() { Indexed = (UInt32Value)64U };

            topBorder2.Append(color5);

            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            Color color6 = new Color() { Indexed = (UInt32Value)64U };

            bottomBorder2.Append(color6);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            borders1.Append(border1);
            borders1.Append(border2);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats1.Append(cellFormat1);

            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)3U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };

            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);

            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles1.Append(cellStyle1);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

            stylesheetExtension1.Append(slicerStyles1);

            StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
            X15.TimelineStyles timelineStyles1 = new X15.TimelineStyles() { DefaultTimelineStyle = "TimeSlicerStyleLight1" };

            stylesheetExtension2.Append(timelineStyles1);

            stylesheetExtensionList1.Append(stylesheetExtension1);
            stylesheetExtensionList1.Append(stylesheetExtension2);

            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);

            workbookStylesPart.Stylesheet = stylesheet1;
        }

        /// <summary>
        /// Set work book style
        /// </summary>
        /// <param name="worksheetPart">Work Sheet file</param>
        /// <param name="sheetData">Sheet data</param>
        private void GenerateWorksheetPartContent(WorksheetPart worksheetPart, SheetData sheetData)
        {
            Worksheet worksheet = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            SheetDimension sheetDimension = new SheetDimension() { Reference = "A1" };

            SheetViews sheetViews = new SheetViews();

            SheetView sheetView = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            Selection selection = new Selection() { ActiveCell = "A1", SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1" } };

            sheetView.Append(selection);
            sheetViews.Append(sheetView);
            SheetFormatProperties sheetFormatProperties = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D };

            PageMargins pageMargins = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
            worksheet.Append(sheetDimension);
            worksheet.Append(sheetViews);
            worksheet.Append(sheetFormatProperties);
            worksheet.Append(sheetData);
            worksheet.Append(pageMargins);




            worksheetPart.Worksheet = worksheet;

        }

        /// <summary>
        /// Generate memory stream data for download file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="fileName">file Name</param>
        /// <returns></returns>
        public async Task<MemoryStream> GenerateMemoryStream(string path, string fileName, string fileToken)
        {
            var memoryStream = new MemoryStream();

            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        /// <summary>
        /// Create output file path 
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Path create file </returns>
        public string CreateFilePath(string fileName)
        {  //get temp path of system
            string path = Path.GetTempPath();
            string exportFileName = fileName + AppConsts.ExcelFileExtention;
            return Path.Combine(path, exportFileName);
        }
    }
}

