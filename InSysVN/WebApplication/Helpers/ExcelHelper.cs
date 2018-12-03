using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Framework.Helper.Extensions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using WebGrease.Css.Extensions;

namespace WebApplication.Helpers
{
    public class ExcelHelper
    {
        public List<PropertyInfo> GetProperties(Type type, IEnumerable<string> propertyNames = null)
        {
            var allProperties = type.GetProperties();
            var properties = new List<PropertyInfo>();

            foreach (var property in allProperties)
            {
                var name = property.Name;
                if (propertyNames != null)
                {
                    var field = propertyNames.FirstOrDefault(o => o.Contains(name));
                    if (!string.IsNullOrEmpty(field)) continue;
                }
                properties.Add(property);
            }
            return properties;
        }

        public List<T> ReadData<T>(Stream stream, string fileName, List<PropertyInfo> columns)
        {
            var list = new List<T>();

            if (fileName.EndsWith("xls"))
            {
                var workbook = new HSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(0);

                if (sheet.PhysicalNumberOfRows <= 1)
                    return list;

                for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
                {
                    var item = Activator.CreateInstance<T>();

                    var row = sheet.GetRow(i);

                    var j = 0;

                    foreach (var column in columns)
                    {
                        if (j == row.Cells.Count)
                            break;

                        var val = row.GetCell(j).StringCellValue;

                        if (string.IsNullOrWhiteSpace(val))
                        {
                            continue;
                        }
                        Type t = Nullable.GetUnderlyingType(column.PropertyType) ?? column.PropertyType;
                        column.SetValue(item, Convert.ChangeType(val, t), null);

                        j++;
                    }

                    list.Add(item);
                }
            }
            else if (fileName.EndsWith("xlsx"))
            {
                var workbook = new XSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(0);

                for (int i = 0; i < sheet.PhysicalNumberOfRows; i++)
                {
                    var item = Activator.CreateInstance<T>();

                    var row = sheet.GetRow(i);

                    var j = 0;

                    foreach (var column in columns)
                    {
                        var val = row.GetCell(j).StringCellValue;

                        if (string.IsNullOrWhiteSpace(val))
                        {
                            continue;
                        }
                        Type t = Nullable.GetUnderlyingType(column.PropertyType) ?? column.PropertyType;
                        column.SetValue(item, Convert.ChangeType(val, t), null);

                        j++;
                    }

                    list.Add(item);
                }
            }

            return list;
        }

        public FileContentResult ExportExcel<T>(List<T> items, ConfigExcel configExcel)
        {
            var fileExport = DateTime.Now.ToString("dd-mm-yyyy");
            if (configExcel != null && !string.IsNullOrEmpty(configExcel.FileExport))
                fileExport = configExcel.FileExport;

            var extension = Path.GetExtension(fileExport);
            var templateFile = (configExcel != null && !string.IsNullOrEmpty(configExcel.TemplateFile))
                ? configExcel.TemplateFile
                : "";

            //http://www.c-sharpcorner.com/blogs/export-to-excel-using-npoi-dll-library
            // dll refered NPOI.dll and NPOI.OOXML
            IWorkbook workbook;

            var contentType = "";
            switch (extension)
            {
                case ".xlsx":
                    using (var file = new FileStream(templateFile, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new XSSFWorkbook(file);
                        file.Close();
                    }
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".xls":
                    using (var file = new FileStream(templateFile, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new HSSFWorkbook(file);
                        file.Close();
                    }
                    //workbook = ConvertXLSXToXLS.ConvertWorkbookXSSFToHSSF((XSSFWorkbook)workbook);
                    //workbook = new HSSFWorkbook();
                    contentType = "application/vnd.ms-excel";
                    break;
                default:
                    throw new Exception("This format is not supported");
            }

            ////Style cell
            //ICellStyle styleCell = workbook.CreateCellStyle();

            ////Setting the line of the top border
            //styleCell.BorderTop = BorderStyle.Thick;
            //styleCell.TopBorderColor = 256;

            //styleCell.BorderLeft = BorderStyle.Thick;
            //styleCell.LeftBorderColor = 256;

            //styleCell.BorderRight = BorderStyle.Thick;
            //styleCell.RightBorderColor = 256;

            //styleCell.BorderBottom = BorderStyle.Thick;
            //styleCell.BottomBorderColor = 256;

            ISheet sheet;
            if (configExcel != null && !string.IsNullOrEmpty(configExcel.TemplateFile))
                sheet = workbook.GetSheetAt(0); //get first Excel sheet from workbook  
            else sheet = workbook.CreateSheet();

            //ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            var properties = GetProperties(typeof(T), configExcel != null ? configExcel.IgnoreColumns : null);

            var i = 0;

            // Header row
            if (configExcel != null && configExcel.RenderHeader)
            {
                var j = 0;
                var row = sheet.CreateRow(i);
                foreach (var property in properties)
                {
                    row.CreateCell(j).SetCellValue(property.Name);
                    j++;
                }
                i++;
            }



            //Replace header if have mark value
            if (configExcel != null && configExcel.MarkHeader != null)
            {
                var rowMaxHeader = configExcel.StartRow - 1;
                for (var iHeader = 0; iHeader < rowMaxHeader; iHeader++)
                {
                    var j = 0;
                    var row = sheet.GetRow(iHeader);
                    foreach (var property in properties)
                    {
                        if (j == row.Cells.Count)
                            continue;
                        var val = row.GetCell(j).StringCellValue;

                        if (string.IsNullOrWhiteSpace(val))
                        {
                            j++;
                            continue;
                        }
                        val = val.ReplaceWithStringBuilder(configExcel.MarkHeader);
                        row.Cells[j].SetCellValue(val);
                        j++;
                    }
                    i++;
                }

            }

            if (configExcel != null && !configExcel.RenderHeader && configExcel.StartRow > 0)
            {
                i = configExcel.StartRow - 1;
            }

            //Fill data
            foreach (var item in items)
            {
                var j = configExcel.StartColumn;
                //var row = sheet.CreateRow(i);
                var row = sheet.GetRow(i);
                if (row == null) row = sheet.CreateRow(i);
                //http://npoi.codeplex.com/SourceControl/latest#NPOI.Examples/NumberFormatInXls/Program.cs
                //http://npoi.codeplex.com/SourceControl/latest#NPOI.Examples/MergeCellsInXls/Program.cs
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    if (value != null)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) cell = row.CreateCell(j);
                        cell.SetCellValue(value.ToString());
                        //cell.CellStyle =  styleCell;
                        //row.CreateCell(j).SetCellValue(value.ToString());
                    }
                    j++;
                }
                i++;
            }
            if (configExcel.AutoSizeColumn)
            {
                for (int index = 0; index < properties.Count; index++)
                {
                    sheet.AutoSizeColumn(index);
                }
            }

            if (configExcel.MergeCells != null && configExcel.MergeCells.Any())
            {
                configExcel.MergeCells.ForEach(o =>
                {
                    sheet.AddMergedRegion(new CellRangeAddress(o.FirstRow, o.LastRow, o.FirstCol, o.LastCol));
                });
            }

            //enables gridline
            sheet.DisplayGridlines = true;

            //Force excel to recalculate all the formula while open
            sheet.ForceFormulaRecalculation = true;

            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            return new FileContentResult(memoryStream.ToArray(), contentType)
            {
                FileDownloadName = fileExport
            };
        }

    }

    public class ConfigExcel
    {
        public string FileExport { get; set; }
        public string TemplateFile { get; set; }
        public bool RenderHeader { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public bool AutoSizeColumn { get; set; }
        public Dictionary<string, string> MarkHeader { get; set; }
        public IEnumerable<string> IgnoreColumns { get; set; }
        public IEnumerable<MergeCell> MergeCells { get; set; }
    }

    public class MergeCell
    {
        public int FirstRow { get; set; }
        public int LastRow { get; set; }
        public int FirstCol { get; set; }
        public int LastCol { get; set; }
    }
}