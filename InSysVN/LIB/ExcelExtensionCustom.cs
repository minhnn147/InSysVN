﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;
using System.ComponentModel;

namespace LIB
{
    public class ExcelExtensionCustom
    {
        public static List<T> ReadtoList<T>(string pathFile, ref List<string> listErrors, ref bool ValidateTemplate) where T : new()
        {
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(pathFile, true))
            {
                WorkbookPart workbookPart = spreadSheet.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                var list = new List<T>();
                var PropertiesModel = typeof(T).GetProperties();
                List<PropertyOfModel_ExcelColumn> listProperty = new List<PropertyOfModel_ExcelColumn>();
                foreach (var item in PropertiesModel)
                {
                    var ec = (ExcelColumnCustom)Attribute.GetCustomAttributes(item, typeof(ExcelColumnCustom)).FirstOrDefault();
                    var ep = (DisplayNameAttribute)Attribute.GetCustomAttributes(item, typeof(DisplayNameAttribute)).FirstOrDefault();
                    if (ec != null)
                    {
                        listProperty.Add(new PropertyOfModel_ExcelColumn(item.Name, ec.ColumnName,ep.DisplayName));
                    }
                }
                if (listProperty.Count > 0)
                {
                    foreach (var row in sheetData.Elements<Row>())
                    {
                        if (row.RowIndex > 0)
                        {
                            var obj = new T();
                            bool check = false;
                            foreach (var cell in row.Elements<Cell>())
                            {
                                if (cell.CellValue != null)
                                {
                                    check = true;
                                    Regex regex = new Regex("[A-Za-z]+");
                                    Match match = regex.Match(cell.CellReference);
                                    string columnName = match.Value;

                                    PropertyOfModel_ExcelColumn PE = listProperty.Where(t => t.ExcelColumn == columnName).SingleOrDefault();
                                    if (PE != null)
                                    {
                                        if(row.RowIndex == 1)
                                        {
                                            string cellValue = "";
                                            try
                                            {
                                              
                                                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                                                {
                                                    int id = -1;

                                                    if (Int32.TryParse(cell.InnerText, out id))
                                                    {
                                                        SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                                                        if (item.Text != null)
                                                        {
                                                            cellValue = item.Text.Text;
                                                        }
                                                        else if (item.InnerText != null)
                                                        {
                                                            cellValue = item.InnerText;
                                                        }
                                                        else if (item.InnerXml != null)
                                                        {
                                                            cellValue = item.InnerXml;
                                                        }
                                                    }
                                                }
                                            } catch(Exception ex)
                                            {
                                                ValidateTemplate = false;
                                                return null;
                                            }

                                            if(cellValue != PE.DisplayName)
                                            {
                                                ValidateTemplate = false;
                                                return null;
                                            }
                                        }
                                        else
                                        {
                                            string PropertyName = PE.PropertyOfModel;
                                            try
                                            {
                                                string cellValue = "";
                                                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                                                {
                                                    int id = -1;

                                                    if (Int32.TryParse(cell.InnerText, out id))
                                                    {
                                                        SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                                                        if (item.Text != null)
                                                        {
                                                            cellValue = item.Text.Text;
                                                        }
                                                        else if (item.InnerText != null)
                                                        {
                                                            cellValue = item.InnerText;
                                                        }
                                                        else if (item.InnerXml != null)
                                                        {
                                                            cellValue = item.InnerXml;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    cellValue = cell.CellValue.Text;
                                                }
                                                var nullable = obj.GetType().GetProperty(PropertyName).PropertyType;
                                                //    //check Nullable Column
                                                if (nullable.Name == "Nullable`1")
                                                {
                                                    var type = obj.GetType().GetProperty(PropertyName).PropertyType.GenericTypeArguments[0];
                                                    if (type == typeof(DateTime))
                                                    {
                                                        var value = new DateTime();
                                                        try
                                                        {
                                                            var date = DateTime.FromOADate(double.Parse(cellValue)).ToString("dd/MM/yyyy");
                                                            value = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                            //var value = DateTime.Parse(cellValue.ToString()).ToString("dd/MM/yyyy");
                                                        }
                                                        catch (Exception)
                                                        {
                                                            //value = DateTime.ParseExact(cellValue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                            var strvalue = DateTime.Parse(cellValue.ToString()).ToString("dd/MM/yyyy");
                                                            value = DateTime.ParseExact(strvalue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        }


                                                        obj.GetType().GetProperty(PropertyName).SetValue(obj, value);
                                                    }
                                                    else
                                                    {
                                                        var value = Convert.ChangeType(cellValue, Nullable.GetUnderlyingType(obj.GetType().GetProperty(PropertyName).PropertyType));
                                                        obj.GetType().GetProperty(PropertyName).SetValue(obj, value);

                                                    }
                                                }
                                                else
                                                {
                                                    var value = Convert.ChangeType(cellValue, obj.GetType().GetProperty(PropertyName).PropertyType);
                                                    //(usedrange.Cells[row, col] as Excel.Range).Value;
                                                    obj.GetType().GetProperty(PropertyName).SetValue(obj, value);
                                                }
                                                if (check && obj != null)
                                                {
                                                    list.Add(obj);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                listErrors.Add(string.Format("Lỗi dữ liệu hàng {0} - Cột {1}", row.RowIndex, columnName));
                                                obj = default(T);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
        }
        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
        public class ExcelColumnCustom : Attribute
        {
            public string ColumnName { get; set; }
            public ExcelColumnCustom(string ColumnName)
            {
                this.ColumnName = ColumnName;
            }
        }
        public class PropertyOfModel_ExcelColumn
        {
            public string PropertyOfModel { get; set; }
            public string ExcelColumn { get; set; }
            public string DisplayName { get; set; }
            public PropertyOfModel_ExcelColumn(string _PropertyOfModel, string _ExcelColumn, string _DisplayName)
            {
                this.PropertyOfModel = _PropertyOfModel;
                this.ExcelColumn = _ExcelColumn;
                this.DisplayName = _DisplayName;
            }
        }
    }
}
