using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OfficeHelper.ExcelHelper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.ComponentModel.DataAnnotations;
using Common.Configuration;

namespace ExcelHelper.Extend
{
    public class ExcelHelperExtend
    {
        public class ExportTemplateOptions
        {
            public class DataRender
            {
                public DataRender()
                {
                    CellStart = "";
                    DirectionType = DirectionType.TOP_TO_DOWN;
                }
                public string Name { get; set; }
                public string CellStart { get; set; }
                public dynamic Data { get; set; }
                public DirectionType DirectionType { get; set; }
            }
            public ExportTemplateOptions()
            {
                SheetName = "Sheet1";
                SheetTemplateName = "Template";
                CellStart = "A1";
                DirectionType = DirectionType.TOP_TO_DOWN;
                isDeleteTemplate = true;
            }
            public string SheetName { get; set; }
            public string SheetTemplateName { get; set; }
            public string CellStart { get; set; }
            public DirectionType DirectionType { get; set; }
            public IEnumerable<DataRender> Datas { get; set; }
            public bool isDeleteTemplate { get; set; }
        }
        static dynamic Gendata<T>(IEnumerable<T> Data)
        {
            Type type = null;
            if (Data.FirstOrDefault() != null)
            {
                type = Data.FirstOrDefault().GetType();
            }
            else
            {
                type = typeof(T);
            }

            var listName = type.GetProperties().Select(e => e.Name).ToList();
            var listData = new List<List<object>>();

            for (int i = 0; i < Data.Count(); i++)
            {
                var e = Data.ElementAt(i);

                var row = new List<object>();
                for (int j = 0; j < listName.Count; j++)
                {
                    var e1 = listName[j];
                    var value = e.GetType().GetProperty(e1).GetValue(e, null);
                    row.Add(value);
                }
                listData.Add(row);
            }

            return new
            {
                Columns = listName,
                Data = listData
            };
        }
        public static void ExportTemplateOpenXML(string file, Stream outStream, ExportTemplateOptions options = null)
        {
            options = options ?? new ExportTemplateOptions
            {

            };
            using (OfficeHelper.ExcelHelper helper = new OfficeHelper.ExcelHelper(file, outStream))
            {
                helper.Direction = options.DirectionType;

                helper.CurrentSheetName = options.SheetName;

                helper.CurrentPosition = new OfficeHelper.CellRef(options.CellStart);

                for (int i = 0; i < options.Datas.Count(); i++)
                {
                    var config = options.Datas.ElementAt(i);

                    if (!string.IsNullOrEmpty(config.CellStart))
                    {
                        helper.CurrentPosition = new OfficeHelper.CellRef(config.CellStart);
                        helper.Direction = config.DirectionType;
                    }

                    var data = new List<dynamic>();
                    if (config.Data != null)
                    {
                        if (config.Data is IEnumerable<dynamic>)
                        {
                            data = (config.Data as IEnumerable<dynamic>).ToList();
                        }
                        else
                        {
                            data.Add(config.Data);
                        }
                        var dataGen = Gendata(data);
                        if (data.Count != 0)
                        {
                            OfficeHelper.CellRangeTemplate block = helper.CreateCellRangeTemplate(config.Name, dataGen.Columns);
                            helper.InsertRange(block, dataGen.Data);
                        }
                        else
                        {
                            helper.InsertRange(config.Name);
                        }
                    }
                    else
                    {
                        helper.InsertRange(config.Name);
                    }
                }
                if (options.isDeleteTemplate)
                {
                    helper.DeleteSheet(options.SheetTemplateName);
                }
            }
        }

        public List<object> ReadData<T>(Stream stream, ExcelConfig excelConfig, ref List<RowError> errors, ref List<dynamic> listDataTable, Func<T, bool> callback = null, Func<Dictionary<string, string>, T, object> setMasterData = null, Func<List<T>, T, List<string>> validateRow = null)
        {
            var list = new List<T>();
            IWorkbook workbook;
            if (excelConfig.FileImport.EndsWith("xls"))
                workbook = new HSSFWorkbook(stream);
            else if (excelConfig.FileImport.EndsWith("xlsx"))
                workbook = new XSSFWorkbook(stream);
            else
                throw new Exception("Tệp tin sai định dạng!");

            var sheet = workbook.GetSheetAt(0);
            if (sheet.PhysicalNumberOfRows <= 1)
                return list.Cast<object>().ToList();

            //Get colums
            var clsType = typeof(T);
            var columns = clsType.GetProperties().Where(prop =>
            {
                if (excelConfig.IgnoreColumns != null)
                {
                    var field = excelConfig.IgnoreColumns.FirstOrDefault(o => o.Contains(prop.Name));
                    if (!string.IsNullOrEmpty(field)) return false;
                }
                return Attribute.IsDefined(prop, typeof(ExcelReaderAttribute));
            }).OrderBy(p =>
            {
                ExcelReaderAttribute ca = (ExcelReaderAttribute)(p.GetCustomAttributes(typeof(ExcelReaderAttribute), false).FirstOrDefault());
                if (ca == null) return 0;
                else return ca.Index;
            }).ToList();

            var startRow = 0;
            if (!excelConfig.RenderHeader && excelConfig.StartRow > 0)
            {
                startRow = excelConfig.StartRow - 1;
            }

            var totalColumn = columns.Count() + 1;
            var rowDataTest = false;
            var evaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();
            var indexRow = 1;
            for (var i = startRow; i < sheet.PhysicalNumberOfRows; i++)
            {
                var item = Activator.CreateInstance<T>();
                dynamic itemTable = new System.Dynamic.ExpandoObject();
                var row = sheet.GetRow(i);
                var lineNumber = i + 1;
                var errorLine = errors.FirstOrDefault(x => x.Line == lineNumber);
                var j = CellReference.ConvertColStringToIndex(excelConfig.StartColumnLetter);
                foreach (var column in columns)
                {
                    var columnLetter = CellReference.ConvertNumToColString(j);
                    try
                    {
                        var indexOfColumns = columns.IndexOf(column);
                        rowDataTest = false;
                        var attrs = (ExcelReaderAttribute)Attribute.GetCustomAttribute(column, typeof(ExcelReaderAttribute));
                        if (j > columns.Count)
                            break;

                        var t = Nullable.GetUnderlyingType(column.PropertyType) != null ? Nullable.GetUnderlyingType(column.PropertyType) : column.PropertyType;

                        var cell = row.GetCell(j);
                        j++;
                        if (cell == null)
                        {
                            if (attrs != null && attrs.Required)
                            {
                                var msg = string.Format(Config.GetConfigByKey("MessageErrorImportRequired"), columnLetter);
                                if (errorLine == null)
                                {
                                    errorLine = new RowError()
                                    {
                                        Line = lineNumber,
                                        ErrorColumn = new List<string>() { msg }
                                    };
                                    errors.Add(errorLine);
                                }
                                else errorLine.ErrorColumn.Add(msg);
                            }
                            ((IDictionary<string, object>)itemTable)["column" + j] = "";
                            continue;
                        }
                        object val = null;
                        var cellValue = evaluator.Evaluate(cell);
                        if (cellValue == null) val = "";
                        else
                        {
                            switch (cellValue.CellType)
                            {
                                case CellType.Numeric:
                                    if (column.PropertyType == typeof(DateTime) || column.PropertyType == typeof(DateTime?))
                                    {
                                        val = DateTime.FromOADate(cellValue.NumberValue);
                                    }
                                    else
                                    {
                                        val = cellValue.NumberValue;
                                    }
                                    break;
                                case CellType.Blank:
                                case CellType.String:
                                    val = cellValue.StringValue;
                                    break;
                                case CellType.Boolean:
                                    val = cellValue.BooleanValue;
                                    break;
                                case CellType.Error:
                                    val = cellValue.ErrorValue;
                                    break;
                                case CellType.Unknown:
                                    break;
                                case CellType.Formula:
                                    break;
                            }
                        }

                        if (cell.CellType == CellType.String)
                        {
                            if (cell.CellType == CellType.Blank && t == typeof(Int32))
                                val = 0; 
                            
                            var sArrayDataTest = Config.GetConfigByKey("ArrayDataTest") ?? string.Empty;
                            if (sArrayDataTest.Length > 0)
                            {
                                var arrayDataTest = sArrayDataTest.Split(',');
                                if (arrayDataTest.Any(x => x == (string)val))
                                {
                                    rowDataTest = true;
                                    break;
                                }
                            }
                        }

                        if (cell.CellType == CellType.Blank && t == typeof(Int32))
                        {
                            val = 0;
                        }

                        var oldValue = val;
                        if (setMasterData != null)
                        {
                            var dictSet = new Dictionary<string, string>();
                            dictSet["Val"] = val.ToString();
                            dictSet["Attr"] = column.Name;
                            var newVal = setMasterData.Invoke(dictSet, item);

                            if (newVal != null)
                            {
                                val = newVal;
                            }
                        }

                        if (val != null && !val.GetType().Equals(typeof(RowError)) && val.ToString().ToLower() != "error")
                        {
                            if(column.PropertyType.Name == "Nullable`1")
                            {
                                if(val.ToString() == "")
                                    column.SetValue(item, null, null);
                                else
                                    column.SetValue(item, Convert.ChangeType(val, t), null);
                            }
                            else
                            {
                                column.SetValue(item, Convert.ChangeType(val, t), null);
                            }
                            ((IDictionary<string, object>)itemTable)["column" + j] = oldValue.ToString();
                        }
                        else
                        {
                            if (val != null && val.GetType().Equals(typeof(RowError)))
                            {
                                var oRef = (RowError)val;
                                var msg = string.Format(oRef.ErrorColumn[0], columnLetter);
                                if (errorLine == null)
                                {
                                    errorLine = new RowError()
                                    {
                                        Line = lineNumber,
                                        ErrorColumn = new List<string>() { msg }
                                    };
                                    errors.Add(errorLine);
                                }
                                else
                                    errorLine.ErrorColumn.Add(msg);
                            }
                            else
                            {
                                var msg = string.Format(Config.GetConfigByKey("MessageErrorImportInCorrect"), columnLetter);
                                if (errorLine == null)
                                {
                                    errorLine = new RowError()
                                    {
                                        Line = lineNumber,
                                        ErrorColumn = new List<string>() { msg }
                                    };
                                    errors.Add(errorLine);
                                }
                                else
                                    errorLine.ErrorColumn.Add(msg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var sError = string.Format("Dữ liệu cột {0}: {1}", columnLetter, ex.Message);
                        errorLine = errors.FirstOrDefault(x => x.Line == lineNumber);
                        if (errorLine == null)
                        {
                            errorLine = new RowError()
                            {
                                Line = lineNumber,
                                ErrorColumn = new List<string>() { sError }
                            };
                            errors.Add(errorLine);
                        }
                        else
                            errorLine.ErrorColumn.Add(sError);
                    }
                }
                if (!rowDataTest && errorLine == null)
                {

                    var error = ValidateItem(item, lineNumber, list, validateRow);
                    if (error != null) errors.Add(error);
                    else
                    {
                        list.Add(item);
                        ((IDictionary<string, object>)itemTable)["column1"] = indexRow;
                        if (callback != null)
                        {
                            var statusRow = callback.Invoke(item);
                            var columnLast = totalColumn + 1;
                            ((IDictionary<string, object>)itemTable)["column" + columnLast] = statusRow ? "Mới" : "Thay thế";
                        }

                        listDataTable.Add(itemTable);
                        indexRow++;
                    }
                }
            }

            
            workbook.Close();
            stream.Close();

            return list.Cast<object>().ToList();
        }

        private RowError ValidateItem<T>(T item, int lineNumber, List<T> list, Func<List<T>, T, List<string>> validateRow = null)
        {
            RowError errorRow = null;
            var validationContext = new ValidationContext(item, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(item, validationContext, results, true))
            {
                if (results != null && results.Count > 0)
                {
                    errorRow = new RowError
                    {
                        Line = lineNumber,
                        ErrorColumn = new List<string>()
                    };

                    errorRow.ErrorColumn.AddRange(results.Select(e => e.ErrorMessage));
                }
            }

            if (validateRow != null)
            {
                var listErrors = validateRow.Invoke(list, item);
                if (listErrors.Count > 0)
                {
                    if (errorRow == null)
                    {
                        errorRow = new RowError
                        {
                            Line = lineNumber,
                            ErrorColumn = new List<string>()
                        };
                    }
                    errorRow.ErrorColumn.AddRange(listErrors);
                }
            }
            return errorRow;
        }

    }
    public class RowError
    {
        public RowError()
        {
            ErrorColumn = new List<string>();
        }

        public int Line { get; set; }
        public List<string> ErrorColumn { get; set; }
    }
    public class ExcelReaderAttribute : Attribute
    {
        public int Index { get; set; }
        public string Column { get; set; }
        public string AliAsColumn { get; set; }
        public bool IsTimeType { get; set; }
        public bool IsYear { get; set; }
        public bool IsCity { get; set; }
        public int Row { get; set; }
        public bool Required { get; set; }

        public int EnumGroupId { get; set; }
        public bool isEnumArray { get; set; }

        public bool CityID { get; set; }
        public bool isCityArray { get; set; }

        public ExcelReaderAttribute(string column = "", int Index = 0, int EnumGroupId = -1, bool CityID = false, bool isCityArray = false, bool isEnumArray = false)
        {
            this.Index = Index;
            Column = column;
            Required = false;

            this.EnumGroupId = EnumGroupId;
            this.CityID = CityID;
            this.isCityArray = isCityArray;
            this.isEnumArray = isEnumArray;
        }
    }

    public class IgnoreAttribute : Attribute
    {
        public bool Status { get; set; }

        public IgnoreAttribute(bool status = true)
        {
            this.Status = status;
        }
    }

    public class ExcelConfig
    {
        public ExcelConfig()
        {
            AutoSizeColumn = false;
        }
        public string FileExport { get; set; }
        public string FileImport { get; set; }
        public string TemplateFile { get; set; }
        public bool RenderHeader { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public string StartColumnLetter { get; set; }
        public string SheetName { get; set; }
        public string ImageData { get; set; }
        public List<string> lstImageData { get; set; }
        public string ImageData1 { get; set; }
        public string ImageData2 { get; set; }
        public string ImageData3 { get; set; }
        public bool AutoSizeColumn { get; set; }
        public Dictionary<string, string> MarkHeader { get; set; }
        public IEnumerable<string> IgnoreColumns { get; set; }
        public IEnumerable<CellMerge> CellMerges { get; set; }
        public IEnumerable<string> ListChart { get; set; }
        public List<int> lstHideColumns { get; set; }
    }

    public class CellMerge
    {
        public int FirstRow { get; set; }
        public int LastRow { get; set; }
        public int FirstCol { get; set; }
        public int LastCol { get; set; }
    }

}
