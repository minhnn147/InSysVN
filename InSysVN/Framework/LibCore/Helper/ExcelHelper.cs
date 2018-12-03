using System.Xml;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelHelper.Extend
{
    public class ExcelTest
    {
        public int? RowIndex { get; set; }
        [DisplayName("Tên")]
        [Column(Order = 1)]
        public string Name { get; set; }
        [DisplayName("Giá trị")]
        [Column(Order = 1)]
        public string Value { get; set; }
    }
    //dkamphuoc
    /// <summary>
    /// † dkamphuoc †
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        ///Index = 0,
        ///ID = 0,
        ///formatCode = "General"
        ///<para />
        ///Index = 1,
        ///ID = 1,
        ///formatCode = "0"
        ///<para />
        ///Index = 2,
        ///ID = 2,
        ///formatCode = "0.00"
        ///<para />
        ///Index = 3,
        ///ID = 3,
        ///formatCode = "#,##0"
        ///<para />
        ///Index = 4,
        ///ID = 4,
        ///formatCode = "#,##0.00"
        ///<para />
        ///Index = 5,
        ///ID = 9,
        ///formatCode = "0%"
        ///<para />
        ///Index = 6,
        ///ID = 10,
        ///formatCode = "0.00%"
        ///<para />
        ///Index = 7,
        ///ID = 11,
        ///formatCode = "0.00E+00"
        ///<para />
        ///Index = 8,
        ///ID = 12,
        ///formatCode = "#?/?"
        ///<para />
        ///Index = 9,
        ///ID = 13,
        ///formatCode = "#??/??"
        ///<para />
        ///Index = 10,
        ///ID = 14,
        ///formatCode = "mm-dd-yy"
        ///<para />
        ///Index = 11,
        ///ID = 15,
        ///formatCode = "d-mmm-yy"
        ///<para />
        ///Index = 12,
        ///ID = 16,
        ///formatCode = "d-mmm"
        ///<para />
        ///Index = 13,
        ///ID = 17,
        ///formatCode = "mmm-yy"
        ///<para />
        ///Index = 14,
        ///ID = 18,
        ///formatCode = "h:mm AM/PM"
        ///<para />
        ///Index = 15,
        ///ID = 19,
        ///formatCode = "h:mm:ss AM/PM"
        ///<para />
        ///Index = 16,
        ///ID = 20,
        ///formatCode = "h:mm"
        ///<para />
        ///Index = 17,
        ///ID = 21,
        ///formatCode = "h:mm:ss"
        ///<para />
        ///Index = 18,
        ///ID = 22,
        ///formatCode = "m/d/yy h:mm"
        ///<para />
        ///Index = 19,
        ///ID = 37,
        ///formatCode = "#,##0 ;(#,##0)"
        ///<para />
        ///Index = 20,
        ///ID = 38,
        ///formatCode = "#,##0 ;[Red](#,##0)"
        ///<para />
        ///Index = 21,
        ///ID = 39,
        ///formatCode = "#,##0.00;(#,##0.00)"
        ///<para />
        ///Index = 22,
        ///ID = 40,
        ///formatCode = "#,##0.00;[Red](#,##0.00)"
        ///<para />
        ///Index = 23,
        ///ID = 45,
        ///formatCode = "mm:ss"
        ///<para />
        ///Index = 24,
        ///ID = 46,
        ///formatCode = "[h]:mm:ss"
        ///<para />
        ///Index = 25,
        ///ID = 47,
        ///formatCode = "mmss.0"
        ///<para />
        ///Index = 26,
        ///ID = 48,
        ///formatCode = "##0.0E+0"
        ///<para />
        ///Index = 27,
        ///ID = 49,
        ///formatCode = "@"
        /// </summary>
        public static int styleDefault = 165;
        /// <summary>
        /// † dkamphuoc †
        /// return base64
        /// </summary>
        ///public static void ExportExcel<T>(List<T> data, string filePath, ref int totalRow, int maxRow = -1)

        public class ExportTemplateDefine<T>
        {
            public enum TypeCell
            {
                Fix = 1,
                Repeat = 2,
            }

            public TypeCell Type { get; set; }
            public int Row { get; set; }
            public int Col { get; set; }
            public List<T> List { get; set; }
            public T Value { get; set; }
        }
        public static void ExportWithTemplate(string fileTemplate)
        {
            SpreadsheetDocument spreadsheetDocument = null;

            try
            {
                //var FileRead = File.OpenRead(fileTemplate);
                spreadsheetDocument = SpreadsheetDocument.Open(fileTemplate, true, new OpenSettings() { });
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                IEnumerable<SharedStringItem> ListSsi = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>();
                List<SharedStringItem> ListSsi1 = ListSsi.ToList();

                foreach (var worksheetPart in workbookPart.WorksheetParts)
                {
                    OpenXmlReader reader = OpenXmlReader.Create(worksheetPart, true);

                    bool breakWhile = false;
                    int row = 0;
                    int col = 0;
                    int colOrigin = 0;
                    bool objPass = true;
                    List<string> headerExcel = new List<string>();
                    while (reader.Read() && !breakWhile)
                    {
                        if (reader.ElementType == typeof(Row))
                        {
                            row++;
                            col = 0;
                            objPass = true;

                            reader.ReadFirstChild();
                            do
                            {
                                if (reader.ElementType != typeof(Cell))
                                {
                                    continue;
                                }
                                col++;
                                Cell c = (Cell)reader.LoadCurrentElement();
                                if (c.CellReference != null && c.CellReference.HasValue)
                                {
                                    colOrigin = GetColumnIndexFromName(GetColumnName(c.CellReference.Value.ToString()));
                                }
                                else
                                {
                                    colOrigin = col;
                                }

                                c.CellValue.Text = "dkamphuoc";
                            } while (reader.ReadNextSibling() && !breakWhile);
                        }
                    }
                }

                workbookPart.Workbook.Save();

            }
            catch (Exception ex)
            {
                var a = 0;
            }
            finally
            {
                if (spreadsheetDocument != null)
                {
                    spreadsheetDocument.Close();
                    spreadsheetDocument.Dispose();
                }
            }

            //        Sheet sheet;
            //sheet = spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().SingleOrDefault(s => s.Name == "Лист1");// get my sheet
            //var worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(sheet.Id.Value);
            //var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            //Stylesheet stylesheet = spreadsheetDocument.WorkbookPart.WorkbookStylesPart.Stylesheet;

        }
        private static void ExportWithTemplate_Each(WorkbookPart workbookPart)
        {
            foreach (var worksheetPart in workbookPart.WorksheetParts)
            {

                //var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var writer = OpenXmlWriter.Create(worksheetPart);
                OpenXmlReader reader = OpenXmlReader.Create(worksheetPart, true);

                bool breakWhile = false;
                int row = 0;
                int col = 0;
                int colOrigin = 0;
                string text;
                List<string> headerExcel = new List<string>();
                while (reader.Read() && !breakWhile)
                {
                    if (reader.ElementType == typeof(Row))
                    {
                        row++;
                        col = 0;

                        reader.ReadFirstChild();
                        do
                        {
                            if (reader.ElementType != typeof(Cell))
                            {
                                continue;
                            }
                            col++;
                            Cell c = (Cell)reader.LoadCurrentElement();
                            if (c.CellReference != null && c.CellReference.HasValue)
                            {
                                colOrigin = GetColumnIndexFromName(GetColumnName(c.CellReference.Value.ToString()));
                            }
                            else
                            {
                                colOrigin = col;
                            }
                        }
                        while (reader.ReadNextSibling() && !breakWhile);
                    }
                }
            }
        }
        public static string ExportExcelBase64<T>(List<T> data, ref int totalRow, int maxRow = -1, ExcelLayout oLayout = null)
        {
            MemoryStream fileStream = ExportExcel(data, ref totalRow, maxRow, oLayout);
            return Convert.ToBase64String(fileStream.ToArray(), 0, (int)fileStream.Length);
        }
        public static MemoryStream ExportExcel<T>(List<T> data, ref int totalRow, int maxRow = -1, ExcelLayout oLayout = null)
        {
            MemoryStream memoryStream = new MemoryStream();
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            ExportExcel<T>(data, spreadsheetDocument, ref totalRow, maxRow, oLayout);
            return memoryStream;
        }

        // Get the specified worksheet.
        private static Worksheet GetWorksheet(SpreadsheetDocument document, string worksheetName)
        {
            IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == worksheetName);
            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);
            return worksheetPart.Worksheet;
        }

        // Create a spreadsheet cell. 
        private static void CreateSpreadsheetCell(Worksheet worksheet, string cellName)
        {
            string columnName = GetColumnName(cellName);
            uint rowIndex = Convert.ToUInt32(Regex.Replace(cellName, "[^0-9]+", string.Empty));
            IEnumerable<Row> rows = worksheet.Descendants<Row>().Where(r => r.RowIndex.Value == rowIndex);
            Row row = rows.First();
            IEnumerable<Cell> cells = row.Elements<Cell>().Where(c => c.CellReference
                .Value == cellName);
        }

        public static void ExportExcel<T>(List<T> data, string filePath, ref int totalRow, int maxRow = -1)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
            ExportExcel<T>(data, spreadsheetDocument, ref totalRow, maxRow);
        }
        /// <summary>
        /// † dkamphuoc †
        ///<para />
        /// [DisplayName("Order Id")]
        ///<para />
        /// [Column(Order = 0)]
        ///<para />
        /// [Description("{FormatNum: 1, FormatStr: "###", Color: "d15050", Width: 50}")]
        /// </summary>
        //public static void ExportExcel<T>(List<T> data, string filePath, ref int totalRow, int maxRow = -1)
        public static void ExportExcel<T>(List<T> data, SpreadsheetDocument spreadsheetDocument, ref int totalRow, int maxRow = -1, ExcelLayout oLayout = null)
        {
            //get all property info
            //var objDescription = (DescriptionAttribute)(typeof(T).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault());
            var objDescription = (CustomDescriptionAttribute)(typeof(T).GetCustomAttributes(typeof(CustomDescriptionAttribute), false).FirstOrDefault());
            var jsonObjDescription = objDescription == null || string.IsNullOrEmpty(objDescription.Description) ? JObject.Parse("{}") : JObject.Parse(objDescription.Description);
            List<PropertyInfo> property = typeof(T).GetProperties()
                        .Where(p => p.IsDefined(typeof(DisplayNameAttribute), false))
                        .OrderBy(delegate (PropertyInfo p)
                        {
                            ColumnAttribute ca = (ColumnAttribute)(p.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault());
                            if (ca == null)
                            {
                                return 0;
                            }
                            else
                            {
                                return ca.Order;
                            }
                        }).ToList();
            var propertiesType = property
                        .Select(delegate (PropertyInfo p)
                        {
                            if (p.PropertyType.Name == "Nullable`1")
                            {
                                return p.PropertyType.GenericTypeArguments[0].Name;
                            }
                            else
                            {
                                return p.PropertyType.Name;
                            }
                        })
                        .ToList();
            var propertiesDisplay = property
                        .Select(delegate (PropertyInfo p)
                        {
                            DisplayNameAttribute dna = (DisplayNameAttribute)(p.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault());
                            if (dna == null)
                            {
                                return p.Name;
                            }
                            else
                            {
                                return dna.DisplayName;
                            }
                        })
                        .ToList();
            var propertiesDescription = property
                        .Select(delegate (PropertyInfo p)
                        {
                            DescriptionAttribute at = (DescriptionAttribute)(p.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault());
                            if (at == null)
                            {
                                return JObject.Parse("{}");
                            }
                            else
                            {
                                return JObject.Parse(at.Description);
                            }
                        })
                        .ToList();
            List<string> propertiesName = property
                        .Select(p => p.Name)
                        .ToList();

            //set default value
            maxRow = maxRow == -1 ? 1000000 : maxRow;
            //if (File.Exists(filePath))
            //{
            //    File.Delete(filePath);
            //}

            //create excel
            //SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
            //create workbook
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            //create worksheet
            var workbook = workbookPart.Workbook = new Workbook();
            Sheets sheets = workbook.AppendChild<Sheets>(new Sheets());

            var openXmlExportHelper = new OpenXmlWriterHelper();

            //set style columns
            Stylesheet stylesheet = openXmlExportHelper.CreateDefaultStylesheet();
            int indexStyle = 1;
            for (var i = 0; i < propertiesDescription.Count; i++)
            {
                var des = propertiesDescription[i];
                var type = propertiesType[i];

                var cf = new CellFormat();// Date time format is defined as StyleIndex = 1
                cf.NumberFormatId = 0;
                cf.FontId = 0;
                cf.FillId = 0;
                cf.BorderId = 0;
                cf.FormatId = 0;
                cf.ApplyNumberFormat = true;


                //// set align content for cell
                //if (des["Align_Horizontal"] != null)
                //{
                //    var align_str = des["Align_Horizontal"].ToString();
                //    Alignment align = new Alignment();
                //    align.Horizontal = HorizontalAlignmentValues.Justify;
                //    cf.ApplyAlignment = true;
                //    cf.Append(align);
                //}
                if (des["Align_Vertical"] != null)
                {
                    var align_str = des["Align_Vertical"].ToString();
                    Alignment align = new Alignment();

                    align.Vertical = VerticalAlignmentValues.Center;

                    cf.ApplyAlignment = true;
                    cf.Append(align);
                }
                if (des["Color"] != null)
                {
                    var fills = stylesheet.Fills;

                    //header fills background color
                    var fill = new Fill();
                    var patternFill = new PatternFill();
                    patternFill.PatternType = PatternValues.Solid;
                    patternFill.ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString(des["Color"].ToString()) };
                    fill.PatternFill = patternFill;
                    fills.AppendChild(fill);
                    fills.Count = (uint)fills.ChildElements.Count;

                    cf.FillId = fills.Count - 1;
                    cf.ApplyFill = true;
                    cf.BorderId = 0;
                }

                if (des["FormatNum"] != null)
                {
                    cf.NumberFormatId = (uint)(des["FormatNum"]);
                }
                else if (new[] { "Int32", "Int64", "long" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                {
                    var nfs = stylesheet.NumberingFormats;
                    NumberingFormat nf = new NumberingFormat();
                    nf.NumberFormatId = (uint)(styleDefault + indexStyle);
                    nfs.Append(nf);

                    nfs.Count = (uint)nfs.ChildElements.Count;
                    cf.NumberFormatId = nf.NumberFormatId;
                    if (des["FormatStr"] != null)
                    {
                        nf.FormatCode = des["FormatStr"].ToString();
                    }
                    else
                    {
                        nf.FormatCode = "#,##0";
                    }
                }
                else if (new[] { "Decimal", "float", "double" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                {
                    var nfs = stylesheet.NumberingFormats;
                    NumberingFormat nf = new NumberingFormat();
                    nf.NumberFormatId = (uint)(styleDefault + indexStyle);
                    nfs.Append(nf);

                    nfs.Count = (uint)nfs.ChildElements.Count;
                    cf.NumberFormatId = nf.NumberFormatId;
                    if (des["FormatStr"] != null)
                    {
                        nf.FormatCode = des["FormatStr"].ToString();
                    }
                    else
                    {
                        nf.FormatCode = "#,##0.00";
                    }
                }
                else if (new[] { "DateTime" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                {
                    var nfs = stylesheet.NumberingFormats;
                    NumberingFormat nf = new NumberingFormat();
                    nf.NumberFormatId = (uint)(styleDefault + indexStyle);
                    nfs.Append(nf);

                    nfs.Count = (uint)nfs.ChildElements.Count;
                    cf.NumberFormatId = nf.NumberFormatId;
                    if (des["FormatStr"] != null)
                    {
                        nf.FormatCode = des["FormatStr"].ToString();
                    }
                    else
                    {
                        nf.FormatCode = "mm/dd/yyyy;@";
                    }
                }
                else if (new[] { "Bool" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                {

                }
                else if (new[] { "string" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                {

                    var nfs = stylesheet.NumberingFormats;
                    NumberingFormat nf = new NumberingFormat();
                    nf.NumberFormatId = (uint)(styleDefault + indexStyle);
                    nf.FormatCode = "@";
                    nfs.Append(nf);

                    nfs.Count = (uint)nfs.ChildElements.Count;
                    cf.NumberFormatId = nf.NumberFormatId;
                }
                else
                {
                }

                var cfs = stylesheet.CellFormats;
                cfs.Append(cf);
                indexStyle++;
            }
            openXmlExportHelper.SaveCustomStylesheet(workbookPart, stylesheet);

            //write data
            int sheetIndex = 0;
            int index = 0;
            int maxRowInSheet = maxRow;
            int CountItem = data.Count;
            WorksheetPart worksheetPart = null;
            OpenXmlWriter writer = null;

            if (CountItem == 0)
            {
                ExportExcel_CreateStartSheetXML(ref workbookPart, ref worksheetPart, ref writer, 1, ref sheets, propertiesDescription, propertiesDisplay, openXmlExportHelper);
                ExportExcel_CreateEndSheetXML(ref writer, jsonObjDescription, ref worksheetPart);
            }
            else
            {
                for (int row = 0; row < CountItem; row++)
                {
                    var dataRow = data[row];

                    if (index == 0)
                    {
                        sheetIndex++;
                        ExportExcel_CreateStartSheetXML(ref workbookPart, ref worksheetPart, ref writer, sheetIndex, ref sheets, propertiesDescription, propertiesDisplay, openXmlExportHelper);
                        ////create sheet
                        //worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        //writer = OpenXmlWriter.Create(worksheetPart);
                        //Sheet sheet = ExportExcel_CreateSheet(workbookPart, worksheetPart, new UInt32Value(UInt32.Parse("" + sheetIndex)), "Sheet" + sheetIndex);
                        //sheets.Append(sheet);

                        ////start sheet
                        //writer.WriteStartElement(new Worksheet());

                        ////freeze header
                        //writer.WriteStartElement(new SheetViews());
                        //writer.WriteStartElement(new SheetView(), new OpenXmlAttribute[] {
                        //    new OpenXmlAttribute("tabSelected", null, "1"),
                        //    new OpenXmlAttribute("workbookViewId", null, "0"),
                        //}.ToList());

                        //writer.WriteStartElement(new Pane(), new OpenXmlAttribute[] {
                        //    new OpenXmlAttribute("ySplit", null, "1"),
                        //    new OpenXmlAttribute("topLeftCell", null, "A2"),
                        //    new OpenXmlAttribute("activePane", null, "bottomLeft"),
                        //    new OpenXmlAttribute("state", null, "frozen"),
                        //}.ToList());
                        //writer.WriteEndElement();//Pane
                        //writer.WriteStartElement(new Selection(), new OpenXmlAttribute[] {
                        //    new OpenXmlAttribute("pane", null, "bottomLeft"),
                        //    new OpenXmlAttribute("activeCell", null, "A2"),
                        //    new OpenXmlAttribute("sqref", null, "A2"),
                        //}.ToList());
                        //writer.WriteEndElement();//Selection

                        //writer.WriteEndElement();//SheetView
                        //writer.WriteEndElement();//SheetViews

                        ////set width columns
                        //Columns columns = new Columns();
                        //for (var i = 0; i < propertiesDescription.Count; i++)
                        //{
                        //    var des = propertiesDescription[i];
                        //    if (des["Width"] != null)
                        //    {
                        //        Column c1 = new Column() { Min = (UInt32)i + 1, Max = (UInt32)i + 1, Width = (DoubleValue)double.Parse(des["Width"].ToString()), BestFit = true, CustomWidth = true };
                        //        columns.Append(c1);
                        //    }
                        //}
                        //if (columns.Count() != 0)
                        //{
                        //    writer.WriteElement(columns);
                        //}

                        //writer.WriteStartElement(new SheetData());
                        ////fill header
                        //writer.WriteStartElement(new Row());
                        //for (var i = 0; i < propertiesDisplay.Count; i++)
                        //{
                        //    var header = propertiesDisplay[i];
                        //    List<OpenXmlAttribute> attributes = new List<OpenXmlAttribute>();
                        //    attributes.Add(new OpenXmlAttribute("r", null, GetColumnName(i) + "" + 1));
                        //    openXmlExportHelper.WriteCellValueSax(writer, header, CellValues.SharedString, attributes);
                        //}
                        //writer.WriteEndElement();
                    }

                    //fill row
                    if (ExportExcel_FillRow<T>(dataRow, writer, openXmlExportHelper, index, propertiesName, propertiesType, jsonObjDescription))
                    {
                        totalRow++;
                    }
                    index++;

                    if (index == maxRowInSheet || row == CountItem - 1)
                    {
                        index = 0;
                        ExportExcel_CreateEndSheetXML(ref writer, jsonObjDescription, ref worksheetPart);
                        //writer.WriteEndElement();//SheetData
                        //writer.WriteEndElement();//Worksheet
                        //writer.Close();

                        //if (jsonObjDescription["Comment"] != null)
                        //{
                        //    Dictionary<string, string> comments = new Dictionary<string, string>();
                        //    foreach (JObject obj in jsonObjDescription["Comment"] as JArray)
                        //    {
                        //        comments.Add(ExcelHelper.GetColumnName(int.Parse(obj["Order"].ToString()) - 1) + "1", obj["Text"].ToString());
                        //    }
                        //    OpenXmlWriterHelper.InsertComments(worksheetPart, comments);
                        //}
                    }
                }
            }
            openXmlExportHelper.CreateShareStringPart(workbookPart);

            //process merge cell
            if (worksheetPart != null && oLayout != null)
            {
                Worksheet worksheet = worksheetPart.Worksheet;
                //Worksheet worksheet = GetWorksheet(spreadsheetDocument, "Sheet1");
                MergeCells mergeCells;

                if (worksheet.Elements<MergeCells>().Count() > 0)
                    mergeCells = worksheet.Elements<MergeCells>().First();
                else
                {
                    mergeCells = new MergeCells();
                    // Insert a MergeCells object into the specified position.
                    if (worksheet.Elements<CustomSheetView>().Count() > 0)
                        worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                    else
                        worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
                }

                // Create the merged cell and append it to the MergeCells collection.
                if (oLayout.MergeCells != null && oLayout.MergeCells.Count > 0)
                {
                    foreach (var mergeCell in oLayout.MergeCells)
                    {
                        mergeCells.Append(new MergeCell
                        {
                            Reference = new StringValue(mergeCell),
                        });
                    }
                }

                // set style for cells excel
                WorksheetPart wp = workbookPart.WorksheetParts.FirstOrDefault();
                Worksheet ws = wp.Worksheet;
                SheetData sheetData = ws.GetFirstChild<SheetData>();
                var firstRowCells = sheetData.Elements<Row>().ToList()[0].Elements<Cell>();
                for (int i = 0; i < firstRowCells.Count(); i++)
                {
                    ExcelLayoutStyle.AddStyleColumHeader(workbookPart, firstRowCells.ToList()[i], "686868");
                }
                if (oLayout.TotalCells.Count > 0)
                {
                    for (int i = 0; i < oLayout.TotalCells.Count; i++)
                    {
                        ExcelLayoutStyle.AddStyleColumTotal(workbookPart, ws.Descendants<Cell>().FirstOrDefault(o => o.CellReference.Value.Equals(oLayout.TotalCells[i])), "979fa0");
                    }
                }
            }

            workbookPart.Workbook.Save();

            spreadsheetDocument.Close();
            spreadsheetDocument.Dispose();
        }
        private static void ExportExcel_CreateStartSheetXML(ref WorkbookPart workbookPart, ref WorksheetPart worksheetPart, ref OpenXmlWriter writer, int sheetIndex, ref Sheets sheets, List<JObject> propertiesDescription, List<string> propertiesDisplay, OpenXmlWriterHelper openXmlExportHelper)
        {
            worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            writer = OpenXmlWriter.Create(worksheetPart);
            Sheet sheet = ExportExcel_CreateSheet(workbookPart, worksheetPart, new UInt32Value(UInt32.Parse("" + sheetIndex)), "Sheet" + sheetIndex);
            sheets.Append(sheet);

            //start sheet
            writer.WriteStartElement(new Worksheet());

            //freeze header
            writer.WriteStartElement(new SheetViews());
            writer.WriteStartElement(new SheetView(), new OpenXmlAttribute[] {
                        new OpenXmlAttribute("tabSelected", null, "1"),
                        new OpenXmlAttribute("workbookViewId", null, "0"),
                    }.ToList());

            writer.WriteStartElement(new Pane(), new OpenXmlAttribute[] {
                        new OpenXmlAttribute("ySplit", null, "1"),
                        new OpenXmlAttribute("topLeftCell", null, "A2"),
                        new OpenXmlAttribute("activePane", null, "bottomLeft"),
                        new OpenXmlAttribute("state", null, "frozen"),
                    }.ToList());
            writer.WriteEndElement();//Pane
            writer.WriteStartElement(new Selection(), new OpenXmlAttribute[] {
                        new OpenXmlAttribute("pane", null, "bottomLeft"),
                        new OpenXmlAttribute("activeCell", null, "A1"),
                        new OpenXmlAttribute("sqref", null, "A1"),
                    }.ToList());
            writer.WriteEndElement();//Selection

            writer.WriteEndElement();//SheetView
            writer.WriteEndElement();//SheetViews

            //set width columns
            Columns columns = new Columns();
            for (var i = 0; i < propertiesDescription.Count; i++)
            {
                var des = propertiesDescription[i];
                if (des["Width"] != null)
                {
                    Column c1 = new Column() { Min = (UInt32)i + 1, Max = (UInt32)i + 1, Width = (DoubleValue)double.Parse(des["Width"].ToString()), BestFit = true, CustomWidth = true };
                    columns.Append(c1);
                }
            }
            if (columns.Count() != 0)
            {
                writer.WriteElement(columns);
            }

            writer.WriteStartElement(new SheetData());
            //fill header
            writer.WriteStartElement(new Row());
            for (var i = 0; i < propertiesDisplay.Count; i++)
            {
                var header = propertiesDisplay[i];
                List<OpenXmlAttribute> attributes = new List<OpenXmlAttribute>();
                attributes.Add(new OpenXmlAttribute("r", null, GetColumnName(i) + "" + 1));
                openXmlExportHelper.WriteCellValueSax(writer, header, CellValues.SharedString, attributes);
            }
            writer.WriteEndElement();
        }
        private static void ExportExcel_CreateEndSheetXML(ref OpenXmlWriter writer, JObject jsonObjDescription, ref WorksheetPart worksheetPart)
        {
            writer.WriteEndElement();//SheetData
            writer.WriteEndElement();//Worksheet
            writer.Close();

            if (jsonObjDescription["Comment"] != null)
            {
                Dictionary<string, string> comments = new Dictionary<string, string>();
                foreach (JObject obj in jsonObjDescription["Comment"] as JArray)
                {
                    comments.Add(ExcelHelper.GetColumnName(int.Parse(obj["Order"].ToString()) - 1) + "1", obj["Text"].ToString());
                }
                OpenXmlWriterHelper.InsertComments(worksheetPart, comments);
            }
        }
        private static void ExportExcel_CreateComment(OpenXmlWriter writer, string reference, string text)
        {
            writer.WriteElement(new Comment()
            {
                Reference = reference,
                CommentText = new CommentText()
                {
                    Text = new Text()
                    {
                        Text = text,
                        Space = SpaceProcessingModeValues.Preserve
                    },

                }
            });
        }
        private static Sheet ExportExcel_CreateSheet(WorkbookPart wp, WorksheetPart wsp, UInt32Value ID, string Name)
        {
            return new Sheet()
            {
                Id = wp.GetIdOfPart(wsp),
                SheetId = ID,
                Name = Name
            };
        }
        private static bool ExportExcel_FillRow<T>(T data, OpenXmlWriter writer, OpenXmlWriterHelper helper, int indexRow, List<string> propertiesName, List<string> propertiesType, JObject jsonObjDescription)
        {
            object obj = (object)data;
            writer.WriteStartElement(new Row());
            for (var i = 0; i < propertiesName.Count; i++)
            {
                var pName = propertiesName[i];
                var type = propertiesType[i];

                PropertyInfo propInfo = obj.GetType().GetProperty(pName);
                try
                {
                    object valueCell = propInfo.GetValue(obj, null);

                    List<OpenXmlAttribute> attributes = new List<OpenXmlAttribute>();
                    attributes.Add(new OpenXmlAttribute("s", null, "" + (i + 1)));
                    attributes.Add(new OpenXmlAttribute("r", null, GetColumnName(i) + "" + (indexRow + 2)));
                    if (valueCell == null)
                    {
                        helper.WriteCellValueSax(writer, "", CellValues.String, attributes);
                    }
                    else
                    {
                        CellValues tempCellValue;
                        string objValueTemp = "";
                        objValueTemp = valueCell.ToString();
                        if (new[] { "Int32", "Int64", "long", "Decimal", "float", "double" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                        {
                            tempCellValue = CellValues.Number;
                        }
                        else if (new[] { "DateTime" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                        {
                            tempCellValue = CellValues.Date;
                            objValueTemp = DateTime.Parse(valueCell.ToString()).ToOADate().ToString(CultureInfo.InvariantCulture);
                        }
                        else if (new[] { "bool", "Boolean" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                        {
                            tempCellValue = CellValues.Boolean;
                        }
                        else
                        {
                            tempCellValue = CellValues.InlineString;
                            if (!string.IsNullOrEmpty(objValueTemp.ToString()))
                            {
                                if (jsonObjDescription["StrFormat"] != null)
                                {
                                    objValueTemp = objValueTemp.ToString();
                                    byte[] bytes = null;
                                    switch (jsonObjDescription["StrFormat"]["Export"]["Input"].ToString().ToUpper())
                                    {
                                        case "ANSI":
                                            bytes = Encoding.Default.GetBytes(objValueTemp);
                                            break;
                                        case "UTF-8":
                                            bytes = Encoding.UTF8.GetBytes(objValueTemp);
                                            break;
                                        case "ASCII":
                                            bytes = Encoding.ASCII.GetBytes(objValueTemp);
                                            break;
                                        case "BigEndianUnicode":
                                            bytes = Encoding.BigEndianUnicode.GetBytes(objValueTemp);
                                            break;
                                        case "Unicode":
                                            bytes = Encoding.Unicode.GetBytes(objValueTemp);
                                            break;
                                        case "UTF32":
                                            bytes = Encoding.UTF32.GetBytes(objValueTemp);
                                            break;
                                        case "UTF7":
                                            bytes = Encoding.UTF7.GetBytes(objValueTemp);
                                            break;
                                    }
                                    switch (jsonObjDescription["StrFormat"]["Export"]["Output"].ToString().ToUpper())
                                    {
                                        case "ANSI":
                                            objValueTemp = Encoding.Default.GetString(bytes);
                                            break;
                                        case "UTF-8":
                                            objValueTemp = Encoding.UTF8.GetString(bytes);
                                            break;
                                        case "ASCII":
                                            objValueTemp = Encoding.ASCII.GetString(bytes);
                                            break;
                                        case "BigEndianUnicode":
                                            objValueTemp = Encoding.BigEndianUnicode.GetString(bytes);
                                            break;
                                        case "Unicode":
                                            objValueTemp = Encoding.Unicode.GetString(bytes);
                                            break;
                                        case "UTF32":
                                            objValueTemp = Encoding.UTF32.GetString(bytes);
                                            break;
                                        case "UTF7":
                                            objValueTemp = Encoding.UTF7.GetString(bytes);
                                            break;
                                    }
                                }
                            }
                        }
                        helper.WriteCellValueSax(writer, objValueTemp.ToString(), tempCellValue, attributes);
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Row", indexRow);
                    ex.Data.Add("Cell", i + 1);
                    ex.Data.Add("Column Name", pName);
                    writer.WriteEndElement();
                    return false;
                    throw ex;
                }
            }
            writer.WriteEndElement();//Row
            return true;
        }

        /// <summary>
        /// † dkamphuoc †
        /// Import Options
        /// </summary>
        ///checkCellInHeader Validate Header
        ///<para />
        ///readIndex Index Start Object
        public class ImportOptions
        {
            public ImportOptions()
            {
                ignoreHeader = false;
                checkCellInHeader = false;
                continueIfError = true;
                readIndex = 2;
                autoHeaderIndex = false;
                ignoreRowEmpty = false;
                breakIfRowEmpty = false;
            }
            //public ImportOptions(bool checkCellInHeader, int readIndex)
            //{
            //    this.checkCellInHeader = checkCellInHeader;
            //    this.readIndex = readIndex;
            //}
            public bool autoHeaderIndex { get; set; }
            public bool checkCellInHeader { get; set; }
            public bool continueIfError { get; set; }
            public int readIndex { get; set; }
            public bool ignoreHeader { get; set; }
            public bool ignoreRowEmpty { get; set; }
            public bool breakIfRowEmpty { get; set; }
        }
        public static List<T> ImportExcel<T>(string filePath, ref int total, ref int success, ref List<ImportExcelError> errors, bool checkCellInHeader = false, int readIndex = 2) where T : new()
        {
            return ImportExcel<T>(filePath, ref total, ref success, ref errors, new ImportOptions()
            {
                checkCellInHeader = checkCellInHeader,
                readIndex = readIndex
            });
        }
        public static List<T> ImportExcel<T>(string filePath, ref int total, ref int success, ref List<ImportExcelError> errors, ImportOptions options) where T : new()
        {
            filePath = Path.Combine(filePath);
            //var objDescription = (DescriptionAttribute)(typeof(T).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault());
            var objDescription = (CustomDescriptionAttribute)(typeof(T).GetCustomAttributes(typeof(CustomDescriptionAttribute), false).FirstOrDefault());
            var jsonObjDescription = objDescription == null || string.IsNullOrEmpty(objDescription.Description) ? JObject.Parse("{}") : JObject.Parse(objDescription.Description);

            var propertyDicFull = new Dictionary<string, PropertyInfo>();
            #region get property object
            var propertyFullName = typeof(T).GetProperties()
                        .Select(delegate (PropertyInfo p)
                        {
                            var name = p.Name;
                            propertyDicFull.Add(name, p);
                            return name;
                        }).ToList();
            var property = typeof(T).GetProperties()
                        .Where(p => p.IsDefined(typeof(DisplayNameAttribute), false))
                        .OrderBy(delegate (PropertyInfo p)
                        {
                            ColumnAttribute ca = (ColumnAttribute)(p.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault());
                            if (ca == null)
                            {
                                return 0;
                            }
                            else
                            {
                                return ca.Order;
                            }
                        }).ToList();
            var propertiesType = property
                        .Select(delegate (PropertyInfo p)
                        {
                            if (p.PropertyType.Name == "Nullable`1")
                            {
                                return p.PropertyType.GenericTypeArguments[0].Name;
                            }
                            else
                            {
                                return p.PropertyType.Name;
                            }
                        })
                        .ToList();
            var propertiesDisplay = property
                        .Select(delegate (PropertyInfo p)
                        {
                            DisplayNameAttribute dna = (DisplayNameAttribute)(p.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault());
                            if (dna == null)
                            {
                                return p.Name;
                            }
                            else
                            {
                                return dna.DisplayName;
                            }
                        })
                        .ToList();
            var propertiesDescription = property
                        .Select(delegate (PropertyInfo p)
                        {
                            DescriptionAttribute at = (DescriptionAttribute)(p.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault());
                            if (at == null)
                            {
                                return JObject.Parse("{}");
                            }
                            else
                            {
                                return JObject.Parse(at.Description);
                            }
                        })
                        .ToList();
            var propertiesName = property
                        .Select(p => p.Name)
                        .ToList();
            #endregion

            List<T> lists = new List<T>();

            var FileRead = File.OpenRead(filePath);
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(FileRead, false, new OpenSettings() { });
            WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
            IEnumerable<SharedStringItem> ListSsi = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>();
            List<SharedStringItem> ListSsi1 = ListSsi.ToList();

            foreach (var worksheetPart in workbookPart.WorksheetParts)
            {
                OpenXmlReader reader = OpenXmlReader.Create(worksheetPart, true);

                bool breakWhile = false;
                int row = 0;
                int col = 0;
                int colOrigin = 0;
                bool objPass = true;
                bool headerReady = false;
                int headerRow = 0;
                int lastRow = 0;
                string text;
                List<string> headerExcel = new List<string>();
                List<string> rowData = new List<string>();
                while (reader.Read() && !breakWhile)
                {
                    if (reader.ElementType == typeof(Row))
                    {
                        row++;
                        col = 0;
                        objPass = true;
                        T objTemp = new T();

                        reader.ReadFirstChild();
                        do
                        {
                            if (reader.ElementType != typeof(Cell))
                            {
                                continue;
                            }
                            col++;
                            #region read cell value
                            text = reader.ToString();
                            Cell c = (Cell)reader.LoadCurrentElement();
                            string cellValue = "";
                            if (c.DataType == null)
                            {
                                if (c.CellValue != null)
                                {
                                    cellValue = c.CellValue.InnerText;
                                }
                            }
                            else if (c.DataType == CellValues.SharedString)
                            {
                                int indexShare = int.Parse(c.CellValue.InnerText);
                                SharedStringItem ssi = ListSsi1[indexShare];
                                cellValue = ssi.Text.Text;

                            }
                            else if (c.DataType == CellValues.InlineString)
                            {
                                if (c.CellValue != null)
                                {
                                    cellValue = c.CellValue.InnerText;
                                }
                                else if (c.InnerText != null)
                                {
                                    cellValue = c.InnerText;
                                }
                            }
                            else if (c.DataType == CellValues.String)
                            {
                                if (c.CellValue != null)
                                {
                                    cellValue = c.CellValue.InnerText;
                                }
                                else if (c.InnerText != null)
                                {
                                    cellValue = c.InnerText;
                                }
                            }
                            else
                            {
                                if (c.CellValue != null)
                                {
                                    cellValue = c.CellValue.InnerText;
                                }
                                else if (c.InnerText != null)
                                {
                                    cellValue = c.InnerText;
                                }
                            }
                            #endregion

                            if (c.CellReference != null && c.CellReference.HasValue)
                            {
                                colOrigin = GetColumnIndexFromName(GetColumnName(c.CellReference.Value.ToString()));
                            }
                            else
                            {
                                colOrigin = col;
                            }

                            if (!headerReady)
                            {
                                if (!options.autoHeaderIndex)
                                {
                                    if (row == 1)
                                    {
                                        if (!options.ignoreHeader)
                                        {
                                            headerExcel.Add(cellValue);
                                        }
                                    }
                                    else if (row >= options.readIndex)
                                    {
                                        headerReady = true;
                                    }
                                }
                                else
                                {
                                    if (cellValue.Trim().Equals(""))
                                    {
                                        continue;
                                    }
                                    if (headerRow == 0)
                                    {
                                        headerRow = row;
                                    }
                                    if (headerRow != row)
                                    {
                                        headerReady = true;
                                        //continue;
                                    }
                                    headerExcel.Add(cellValue);
                                }
                            }

                            if (!headerReady)
                            {
                                continue;
                            }

                            rowData.Add(cellValue);
                            if (cellValue.Trim().Equals(""))
                            {
                                continue;
                            }



                            if (!options.ignoreHeader)
                            {
                                if (options.checkCellInHeader)
                                {
                                    #region validate excel
                                    string textError = string.Empty;
                                    int rowError = 1;
                                    if (propertiesDisplay.Count != headerExcel.Count)
                                    {
                                        textError = "Invalid header";
                                    }
                                    else if (colOrigin > propertiesDisplay.Count)
                                    {
                                        textError = "Cell not exists header(Row: " + row + ")";
                                    }
                                    //else if (!propertiesDisplay.SequenceEqual(headerExcel))
                                    //{
                                    //    textError = "Text in column header not mapping";
                                    //}
                                    else
                                    {
                                        var listDisficuren = headerExcel.Where(e1 => !propertiesDisplay.Any(e2 => e1.Equals(e2))).ToList();
                                        if (listDisficuren.Count() != 0)
                                        {
                                            textError = "Column header not mapping(" + string.Join(", ", listDisficuren) + ")";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(textError))
                                    {
                                        errors.Add(new ImportExcelError()
                                        {
                                            row = rowError,
                                            Message = "Invalid format excel.",
                                            Text = textError
                                        });

                                        lists = new List<T>();
                                        objPass = false;
                                        if (options.continueIfError)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            breakWhile = true;
                                            break;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (colOrigin >= headerExcel.Count + 1)
                                    {
                                        if (options.continueIfError)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            breakWhile = true;
                                            break;
                                        }
                                    }

                                    var head = headerExcel[colOrigin - 1];
                                    var indexOf = propertiesDisplay.IndexOf(head);
                                    if (indexOf == -1)
                                    {
                                        if (options.continueIfError)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            breakWhile = true;
                                            break;
                                        }
                                    }

                                    colOrigin = indexOf + 1;
                                }
                            }
                            else
                            {
                                if (colOrigin >= propertiesDisplay.Count + 1)
                                {
                                    if (options.continueIfError)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        breakWhile = true;
                                        break;
                                    }
                                }
                            }


                            var colName = propertiesDisplay[colOrigin - 1];
                            var type = propertiesType[colOrigin - 1];
                            var proIn = property[colOrigin - 1];

                            try
                            {
                                var description = propertiesDescription[colOrigin - 1];
                                #region set Data Object
                                if (new[] { "Int32" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    proIn.SetValue(objTemp, Int32.Parse(cellValue));
                                }
                                else if (new[] { "Int64" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    proIn.SetValue(objTemp, Int64.Parse(cellValue));
                                }
                                else if (new[] { "String" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    if (jsonObjDescription["StrFormat"] != null)
                                    {
                                        byte[] bytes = null;
                                        switch (jsonObjDescription["StrFormat"]["Import"]["Input"].ToString().ToUpper())
                                        {
                                            case "ANSI":
                                                bytes = Encoding.Default.GetBytes(cellValue);
                                                break;
                                            case "UTF-8":
                                                bytes = Encoding.UTF8.GetBytes(cellValue);
                                                break;
                                            case "ASCII":
                                                bytes = Encoding.ASCII.GetBytes(cellValue);
                                                break;
                                            case "Unicode":
                                                bytes = Encoding.Unicode.GetBytes(cellValue);
                                                break;
                                            case "BigEndianUnicode":
                                                bytes = Encoding.BigEndianUnicode.GetBytes(cellValue);
                                                break;
                                            case "UTF32":
                                                bytes = Encoding.UTF32.GetBytes(cellValue);
                                                break;
                                            case "UTF7":
                                                bytes = Encoding.UTF7.GetBytes(cellValue);
                                                break;
                                        }
                                        switch (jsonObjDescription["StrFormat"]["Import"]["Output"].ToString().ToUpper())
                                        {
                                            case "ANSI":
                                                cellValue = Encoding.Default.GetString(bytes);
                                                break;
                                            case "UTF-8":
                                                cellValue = Encoding.UTF8.GetString(bytes);
                                                break;
                                            case "ASCII":
                                                cellValue = Encoding.ASCII.GetString(bytes);
                                                break;
                                            case "Unicode":
                                                cellValue = Encoding.Unicode.GetString(bytes);
                                                break;
                                            case "BigEndianUnicode":
                                                cellValue = Encoding.BigEndianUnicode.GetString(bytes);
                                                break;
                                            case "UTF32":
                                                cellValue = Encoding.UTF32.GetString(bytes);
                                                break;
                                            case "UTF7":
                                                cellValue = Encoding.UTF7.GetString(bytes);
                                                break;
                                        }
                                    }
                                    proIn.SetValue(objTemp, cellValue);
                                }
                                else if (new[] { "bool", "Boolean" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    bool tempValue;
                                    int tempValue1;
                                    if (bool.TryParse(cellValue, out tempValue))
                                    {
                                        proIn.SetValue(objTemp, tempValue);
                                    }
                                    else if (int.TryParse(cellValue, out tempValue1))
                                    {
                                        proIn.SetValue(objTemp, tempValue1 == 1 ? true : false);
                                    }
                                    else
                                    {
                                        var temp = "true".ToUpper().Equals(cellValue.ToUpper());
                                        proIn.SetValue(objTemp, temp);
                                    }
                                }
                                else if (new[] { "Float" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    float tempValue;
                                    if (!float.TryParse(cellValue, out tempValue))
                                    {
                                        tempValue = float.Parse(cellValue, System.Globalization.NumberStyles.Float);
                                    }
                                    proIn.SetValue(objTemp, tempValue);
                                }
                                else if (new[] { "Decimal" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    Decimal tempValue;
                                    if (!Decimal.TryParse(cellValue, out tempValue))
                                    {
                                        tempValue = Decimal.Parse(cellValue, System.Globalization.NumberStyles.Float);
                                    }
                                    proIn.SetValue(objTemp, tempValue);
                                }
                                else if (new[] { "double" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    double tempValue;
                                    if (!double.TryParse(cellValue, out tempValue))
                                    {
                                        tempValue = double.Parse(cellValue, System.Globalization.NumberStyles.Float);
                                    }
                                    proIn.SetValue(objTemp, tempValue);
                                }
                                else if (new[] { "DateTime" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                                {
                                    var inputFormat = "MM/dd/yyyy";
                                    if (description["Import"] != null && description["Import"]["DateFormat"] != null)
                                    {
                                        inputFormat = description["Import"]["DateFormat"].ToString();
                                    }
                                    DateTime tempValue;
                                    double tempValue1;
                                    if (DateTime.TryParse(cellValue, out tempValue))
                                    {
                                        proIn.SetValue(objTemp, tempValue);
                                    }
                                    else if (double.TryParse(cellValue, out tempValue1))
                                    {
                                        tempValue = DateTime.FromOADate(tempValue1);
                                        proIn.SetValue(objTemp, tempValue);
                                    }
                                    else
                                    {
                                        tempValue = DateTime.ParseExact(cellValue, inputFormat, CultureInfo.InvariantCulture);
                                        proIn.SetValue(objTemp, tempValue);
                                    }
                                }
                                #endregion
                                else
                                {
                                    errors.Add(new ImportExcelError()
                                    {
                                        row = row,
                                        Column = colName,
                                        Message = "Format column invalid",
                                        Text = cellValue
                                    });
                                    objPass = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                errors.Add(new ImportExcelError()
                                {
                                    row = row,
                                    Column = colName,
                                    Message = ex.Message,
                                    Text = cellValue
                                });
                                objPass = false;
                            }
                        } while (reader.ReadNextSibling() && !breakWhile);

                        if (!headerReady)
                        {
                            continue;
                        }

                        if (rowData.Count > 0 && rowData.Where(e => string.IsNullOrEmpty(e)).Count() == rowData.Count)
                        {
                            if (options.breakIfRowEmpty)
                            {
                                break;
                            }

                            if (options.ignoreRowEmpty)
                            {
                                continue;
                            }
                        }

                        rowData = new List<string>();

                        if (row != 1 && (
                                (!options.autoHeaderIndex && row >= options.readIndex)
                                || (options.autoHeaderIndex && row >= headerRow)
                            )
                        )
                        {
                            total++;
                        }
                        if (row != 1 && objPass && (
                                (!options.autoHeaderIndex && row >= options.readIndex)
                                || (options.autoHeaderIndex && row >= headerRow)
                            )
                        )
                        {
                            ValidateObject<T>(objTemp, ref errors, ref lists, ref success, row);
                        }
                    }
                }

            }
            spreadsheetDocument.Close();
            spreadsheetDocument.Dispose();
            FileRead.Close();
            FileRead.Dispose();
            return lists;
        }
        public class GenObjFromStr_Options<T>
        {
            public GenObjFromStr_Options()
            {
                splitObj = "|";
                splitAttr = ",";
                splitValue = ":";
                row = 0;
                errors = new List<ImportExcelError>();
                lists = new List<T>();
                RefMore = new Dictionary<string, string>();
            }
            public string str { get; set; }
            public List<ImportExcelError> errors { get; set; }
            public List<T> lists { get; set; }
            public int? row { get; set; }
            public string splitObj { get; set; }
            public string splitAttr { get; set; }
            public string splitValue { get; set; }
            public Dictionary<string, string> RefMore { get; set; }
        }
        //public static bool GenObjFromStr<T>(string str, ref List<ImportExcelError> errors, ref List<T> lists, int? row = 0, string splitObj = "|", string splitAttr = ",", string splitValue = ":") where T : new()
        //{
        //    Dictionary<string, string> RefMore = new Dictionary<string, string>();
        //    var check = GenObjFromStr();
        //}
        public static bool GenObjFromStr<T>(string str, ref List<ImportExcelError> errors, ref List<T> lists, int? row = 0, string splitObj = "|", string splitAttr = ",", string splitValue = ":", bool validateObj = true) where T : new()
        {
            List<T> arr = new List<T>();
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            if (errors == null)
            {
                errors = new List<ImportExcelError>();
            }
            if (lists == null)
            {
                lists = new List<T>();
            }

            #region get proerty info
            List<PropertyInfo> property = typeof(T).GetProperties()
                        .Where(p => p.IsDefined(typeof(DisplayNameAttribute), false))
                        .OrderBy(delegate (PropertyInfo p)
                        {
                            ColumnAttribute ca = (ColumnAttribute)(p.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault());
                            if (ca == null)
                            {
                                return 0;
                            }
                            else
                            {
                                return ca.Order;
                            }
                        }).ToList();
            var propertiesType = property
                        .Select(delegate (PropertyInfo p)
                        {
                            if (p.PropertyType.Name == "Nullable`1")
                            {
                                return p.PropertyType.GenericTypeArguments[0].Name;
                            }
                            else
                            {
                                return p.PropertyType.Name;
                            }
                        })
                        .ToList();
            var propertiesDisplay = property
                        .Select(delegate (PropertyInfo p)
                        {
                            DisplayNameAttribute dna = (DisplayNameAttribute)(p.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault());
                            if (dna == null)
                            {
                                return p.Name;
                            }
                            else
                            {
                                return dna.DisplayName;
                            }
                        })
                        .ToList();
            var propertiesDescription = property
                        .Select(delegate (PropertyInfo p)
                        {
                            DescriptionAttribute at = (DescriptionAttribute)(p.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault());
                            if (at == null)
                            {
                                return JObject.Parse("{}");
                            }
                            else
                            {
                                return JObject.Parse(at.Description);
                            }
                        })
                        .ToList();
            List<string> propertiesName = property
                        .Select(p => p.Name)
                        .ToList();
            #endregion

            splitObj = "[" + splitObj + "]";
            splitAttr = "[" + splitAttr + "]";
            splitValue = "[" + splitValue + "]";
            //if (RefMore != null && RefMore.Keys.Count != 0)
            //{
            //    foreach (var item in RefMore)
            //    {
            //        var key = item.Key;
            //        str = Regex.Replace(str, splitObj + key, splitObj + key);
            //        str = Regex.Replace(str, key + splitValue, key + splitValue);

            //        str = Regex.Replace(str, splitAttr + " " + key + splitValue, splitAttr + " " + key + splitValue);
            //    }
            //}
            for (var i = 0; i < propertiesDisplay.Count; i++)
            {
                var display = propertiesDisplay[i];
                if (i == 0)
                {
                    str = Regex.Replace(str, splitObj + display, splitObj + display);
                    str = Regex.Replace(str, display + splitValue, display + splitValue);
                }
                else
                {
                    str = Regex.Replace(str, splitAttr + " " + display + splitValue, splitAttr + " " + display + splitValue);
                }
            }
            var str_arr = str.Split(new string[] { splitObj }, StringSplitOptions.None);
            foreach (var str_obj in str_arr)
            {
                var objPass = true;
                T objTemp = new T();
                var s_att = str_obj.Split(new string[] { splitAttr }, StringSplitOptions.None);

                foreach (var s_value in s_att)
                {
                    var temp = s_value.Split(new string[] { splitValue }, StringSplitOptions.None);
                    if (temp.Length != 2)
                    {
                        continue;
                    }
                    var attr = temp[0] = temp[0].Trim();
                    var value = temp[1] = temp[1].Trim();

                    var indexAttr = propertiesDisplay.IndexOf(attr);
                    if (indexAttr != -1)
                    {
                        try
                        {
                            var type = propertiesType[indexAttr];
                            var proIn = property[indexAttr];
                            var description = propertiesDescription[indexAttr];

                            #region set data
                            if (value.Trim().Equals(""))
                            {
                                continue;
                            }
                            else if (new[] { "Int32" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                proIn.SetValue(objTemp, Int32.Parse(value));
                            }
                            else if (new[] { "Int64" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                proIn.SetValue(objTemp, Int64.Parse(value));
                            }
                            else if (new[] { "String" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                proIn.SetValue(objTemp, value);
                            }
                            else if (new[] { "bool" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                bool tempValue;
                                int tempValue1;
                                if (bool.TryParse(value, out tempValue))
                                {
                                    proIn.SetValue(objTemp, tempValue);
                                }
                                else if (int.TryParse(value, out tempValue1))
                                {
                                    proIn.SetValue(objTemp, tempValue1 == 1 ? true : false);
                                }
                                else
                                {
                                    proIn.SetValue(objTemp, false);
                                }
                            }
                            else if (new[] { "Float" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                float tempValue;
                                if (!float.TryParse(value, out tempValue))
                                {
                                    tempValue = float.Parse(value, System.Globalization.NumberStyles.Float);
                                }
                                proIn.SetValue(objTemp, tempValue);
                            }
                            else if (new[] { "Decimal" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                Decimal tempValue;
                                if (!Decimal.TryParse(value, out tempValue))
                                {
                                    tempValue = Decimal.Parse(value, System.Globalization.NumberStyles.Float);
                                }
                                proIn.SetValue(objTemp, tempValue);
                            }
                            else if (new[] { "double" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                double tempValue;
                                if (!double.TryParse(value, out tempValue))
                                {
                                    tempValue = double.Parse(value, System.Globalization.NumberStyles.Float);
                                }
                                proIn.SetValue(objTemp, tempValue);
                            }
                            else if (new[] { "DateTime" }.Contains(type, StringComparer.OrdinalIgnoreCase))
                            {
                                var inputFormat = "MM/dd/yyyy";
                                if (description["Import"] != null && description["Import"]["DateFormat"] != null)
                                {
                                    inputFormat = description["Import"]["DateFormat"].ToString();
                                }
                                DateTime tempValue;
                                double tempValue1;
                                if (DateTime.TryParse(value, out tempValue))
                                {
                                    proIn.SetValue(objTemp, tempValue);
                                }
                                else if (double.TryParse(value, out tempValue1))
                                {
                                    tempValue = DateTime.FromOADate(tempValue1);
                                    proIn.SetValue(objTemp, tempValue);
                                }
                                else
                                {
                                    tempValue = DateTime.ParseExact(value, inputFormat, CultureInfo.InvariantCulture);
                                    proIn.SetValue(objTemp, tempValue);
                                }
                            }
                            #endregion
                            else
                            {
                                if (errors != null)
                                {
                                    errors.Add(new ImportExcelError()
                                    {
                                        row = row,
                                        Column = attr,
                                        Message = "Format column invalid",
                                        Text = value
                                    });
                                }
                                objPass = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            objPass = false;
                            errors.Add(new ImportExcelError()
                            {
                                row = row,
                                Column = attr,
                                Message = "Format column invalid",
                                Text = value
                            });
                        }
                    }
                    //else if (RefMore != null && RefMore.Keys.Count != 0)
                    //{
                    //    string _out = "";
                    //    RefMore.TryGetValue(attr, out _out);
                    //    if (!string.IsNullOrEmpty(_out))
                    //    {
                    //        RefMore.Add(attr, _out);
                    //    }
                    //}
                }
                if (objPass)
                {
                    int success = 0;
                    if (!validateObj || (validateObj && ValidateObject<T>(objTemp, ref errors, ref lists, ref success, row)))
                    {
                        arr.Add(objTemp);
                    }
                }
            };
            return true;
        }
        public static bool ValidateObject<T>(T obj, ref List<ImportExcelError> errors, ref List<T> lists, ref int success, int? row)
        {
            #region get proerty info
            var property = typeof(T).GetProperties()
                        .Where(p => p.IsDefined(typeof(DisplayNameAttribute), false))
                        .OrderBy(delegate (PropertyInfo p)
                        {
                            ColumnAttribute ca = (ColumnAttribute)(p.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault());
                            if (ca == null)
                            {
                                return 0;
                            }
                            else
                            {
                                return ca.Order;
                            }
                        }).ToList();
            var propertiesType = property
                        .Select(delegate (PropertyInfo p)
                        {
                            if (p.PropertyType.Name == "Nullable`1")
                            {
                                return p.PropertyType.GenericTypeArguments[0].Name;
                            }
                            else
                            {
                                return p.PropertyType.Name;
                            }
                        })
                        .ToList();
            var propertiesDisplay = property
                        .Select(delegate (PropertyInfo p)
                        {
                            DisplayNameAttribute dna = (DisplayNameAttribute)(p.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault());
                            if (dna == null)
                            {
                                return p.Name;
                            }
                            else
                            {
                                return dna.DisplayName;
                            }
                        })
                        .ToList();
            var propertiesName = property
                        .Select(p => p.Name)
                        .ToList();
            var propertyDicFull = new Dictionary<string, PropertyInfo>();
            var propertyFullName = typeof(T).GetProperties()
                        .Select(delegate (PropertyInfo p)
                        {
                            var name = p.Name;
                            propertyDicFull.Add(name, p);
                            return name;
                        }).ToList();

            #endregion

            var errors_val = Validators.Validate(obj);
            if (errors_val.Count != 0)
            {
                foreach (AttributeError e in errors_val)
                {
                    int indexOf = propertiesName.IndexOf(e.AttributeName);
                    var getTemp = obj.GetType().GetProperty(e.AttributeName).GetValue(obj, null);
                    if (errors != null)
                    {
                        errors.Add(new ImportExcelError()
                        {
                            row = row,
                            Column = propertiesDisplay[indexOf],
                            Message = e.MessageError,
                            Text = getTemp != null ? getTemp.ToString() : ""
                        });
                    }
                }
                return false;
            }
            else
            {
                success++;
                var propertyIndex = propertyDicFull["RowIndex"];
                if (propertyIndex != null)
                {
                    propertyIndex.SetValue(obj, row);
                }
                if (lists != null)
                {
                    lists.Add(obj);
                }
                return true;
            }
        }
        public static int GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;

        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }
        public static string GetColumnName(int index)
        {
            //string[] saExcelColumnHeaderNames = new string[16384];
            string[] sa = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string s = string.Empty;
            int i, j, k, l;
            i = j = k = -1;
            for (l = 0; l < 16384; ++l)
            {
                s = string.Empty;
                ++k;
                if (k == 26)
                {
                    k = 0;
                    ++j;
                    if (j == 26)
                    {
                        j = 0;
                        ++i;
                    }
                }
                if (i >= 0) s += sa[i];
                if (j >= 0) s += sa[j];
                if (k >= 0) s += sa[k];
                //saExcelColumnHeaderNames[l] = s;
                if (l == index)
                {
                    return s;
                }
            }
            return "";
        }
        public static int GetRowIndex(string cellReference)
        {
            Regex regex = new Regex("[\\D]");
            cellReference = Regex.Replace(cellReference, "[\\D]", "");
            return int.Parse(cellReference);
        }
        public static uint GetRowIndex1(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }
    }
    public class ImportExcelError
    {
        public int? row { get; set; }
        public string Column { get; set; }
        public string Message { get; set; }
        public string Text { get; set; }
        public string Trace { get; set; }
    }

    /// <summary>
    /// A helper class to keep track of the sharedstringtable string being created in a spreadsheet
    /// </summary>
    public class OpenXmlWriterHelper
    {
        /// <summary>
        /// contains the shared string as the key, and the index as the value.  index is 0 base
        /// </summary>
        private readonly Dictionary<string, int> _shareStringDictionary = new Dictionary<string, int>();
        private int _shareStringMaxIndex = 0;

        /// <summary>
        /// create the default excel formats.  These formats are required for the excel in order for it to render
        /// correctly.
        /// </summary>
        /// <returns></returns>
        public Stylesheet CreateDefaultStylesheet()
        {

            Stylesheet ss = new Stylesheet();

            Fonts fts = new Fonts();
            DocumentFormat.OpenXml.Spreadsheet.Font ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName ftn = new FontName();
            ftn.Val = "Arial";
            FontSize ftsz = new FontSize();
            ftsz.Val = 10;
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);
            fts.Count = (uint)fts.ChildElements.Count;

            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;

            //default fills used by Excel, don't changes these

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.AppendChild(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.AppendChild(fill);



            fills.Count = (uint)fills.ChildElements.Count;

            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);
            borders.Count = (uint)borders.ChildElements.Count;

            CellStyleFormats csfs = new CellStyleFormats();
            CellFormat cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);
            csfs.Count = (uint)csfs.ChildElements.Count;


            CellFormats cfs = new CellFormats();

            cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);



            var nfs = new NumberingFormats();



            nfs.Count = (uint)nfs.ChildElements.Count;
            cfs.Count = (uint)cfs.ChildElements.Count;

            ss.Append(nfs);
            ss.Append(fts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(csfs);
            ss.Append(cfs);

            CellStyles css = new CellStyles(
                new CellStyle()
                {
                    Name = "Normal",
                    FormatId = 0,
                    BuiltinId = 0,
                }
                );

            css.Count = (uint)css.ChildElements.Count;
            ss.Append(css);

            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;
            ss.Append(dfs);

            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = "TableStyleMedium9";
            tss.DefaultPivotStyle = "PivotStyleLight16";
            ss.Append(tss);
            return ss;
        }

        virtual public void SaveCustomStylesheet(WorkbookPart workbookPart, Stylesheet stylesheet1 = null)
        {
            WorkbookStylesPart workbookStylesPart;
            Stylesheet style;

            if (stylesheet1 != null)
            {
                workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                style = workbookStylesPart.Stylesheet = stylesheet1;
                style.Save();
                return;
            }

            //get a copy of the default excel style sheet then add additional styles to it
            Stylesheet stylesheet = CreateDefaultStylesheet();


            // ***************************** Fills *********************************
            var fills = stylesheet.Fills;

            //header fills background color
            var fill = new Fill();
            var patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Solid;
            patternFill.ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString("C8EEFF") };
            //patternFill.BackgroundColor = new BackgroundColor() { Indexed = 64 };
            fill.PatternFill = patternFill;
            fills.AppendChild(fill);
            fills.Count = (uint)fills.ChildElements.Count;

            // *************************** numbering formats ***********************
            var nfs = stylesheet.NumberingFormats;
            //number less than 164 is reserved by excel for default formats
            uint iExcelIndex = 165;
            NumberingFormat nf;
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = @"[$-409]m/d/yy\ h:mm\ AM/PM;@";
            nfs.Append(nf);

            nfs.Count = (uint)nfs.ChildElements.Count;

            //************************** cell formats ***********************************
            var cfs = stylesheet.CellFormats;//this should already contain a default StyleIndex of 0

            var cf = new CellFormat();// Date time format is defined as StyleIndex = 1
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            cf = new CellFormat();// Header format is defined as StyleINdex = 2
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 2;
            cf.ApplyFill = true;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);


            cfs.Count = (uint)cfs.ChildElements.Count;

            workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            style = workbookStylesPart.Stylesheet = stylesheet;
            style.Save();

        }

        /// <summary>
        /// write out the share string xml.  Call this after writing out all shared string values in sheet
        /// </summary>
        /// <param name="workbookPart"></param>
        public void CreateShareStringPart(WorkbookPart workbookPart)
        {
            if (_shareStringMaxIndex > 0)
            {
                var sharedStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
                using (var writer = OpenXmlWriter.Create(sharedStringPart))
                {
                    writer.WriteStartElement(new SharedStringTable());
                    foreach (var item in _shareStringDictionary)
                    {
                        writer.WriteStartElement(new SharedStringItem());
                        writer.WriteElement(new Text(item.Key));
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }

        }

        /// <summary>
        /// CellValues = Boolean -> expects cellValue "True" or "False"
        /// CellValues = InlineString -> stores string within sheet
        /// CellValues = SharedString -> stores index within sheet. If this is called, please call CreateShareStringPart after creating all sheet data to create the shared string part
        /// CellValues = Date -> expects ((DateTime)value).ToOADate().ToString(CultureInfo.InvariantCulture) as cellValue 
        ///              and new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "1") }.ToList() as attributes so that the correct formatting can be applied
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="cellValue"></param>
        /// <param name="dataType"></param>
        /// <param name="attributes"></param>
        public void WriteCellValueSax(OpenXmlWriter writer, string cellValue, CellValues dataType, List<OpenXmlAttribute> attributes = null)
        {
            switch (dataType)
            {
                case CellValues.InlineString:
                    {
                        if (attributes == null)
                        {
                            attributes = new List<OpenXmlAttribute>();
                        }
                        attributes.Add(new OpenXmlAttribute("t", null, "inlineStr"));
                        writer.WriteStartElement(new Cell(), attributes);
                        writer.WriteElement(new InlineString(new Text(cellValue)));
                        writer.WriteEndElement();
                        break;
                    }
                case CellValues.SharedString:
                    {
                        if (attributes == null)
                        {
                            attributes = new List<OpenXmlAttribute>();
                        }
                        attributes.Add(new OpenXmlAttribute("t", null, "s"));//shared string type
                        writer.WriteStartElement(new Cell(), attributes);
                        if (!_shareStringDictionary.ContainsKey(cellValue))
                        {
                            _shareStringDictionary.Add(cellValue, _shareStringMaxIndex);
                            _shareStringMaxIndex++;
                        }

                        //writing the index as the cell value
                        writer.WriteElement(new CellValue(_shareStringDictionary[cellValue].ToString()));


                        writer.WriteEndElement();//cell

                        break;
                    }
                case CellValues.Date:
                    {
                        if (attributes == null)
                        {
                            writer.WriteStartElement(new Cell() { DataType = CellValues.Number });
                        }
                        else
                        {
                            writer.WriteStartElement(new Cell() { DataType = CellValues.Number }, attributes);
                        }

                        writer.WriteElement(new CellValue(cellValue));

                        writer.WriteEndElement();

                        break;
                    }
                case CellValues.Boolean:
                    {
                        if (attributes == null)
                        {
                            attributes = new List<OpenXmlAttribute>();
                        }
                        attributes.Add(new OpenXmlAttribute("t", null, "b"));//boolean type
                        writer.WriteStartElement(new Cell(), attributes);
                        writer.WriteElement(new CellValue(cellValue.Equals("true", StringComparison.OrdinalIgnoreCase) || cellValue.Equals("1", StringComparison.OrdinalIgnoreCase) ? "1" : "0"));
                        writer.WriteEndElement();
                        break;
                    }
                default:
                    {
                        if (attributes == null)
                        {
                            writer.WriteStartElement(new Cell() { DataType = dataType });
                        }
                        else
                        {
                            writer.WriteStartElement(new Cell() { DataType = dataType }, attributes);
                        }
                        writer.WriteElement(new CellValue(cellValue));

                        writer.WriteEndElement();


                        break;
                    }
            }

        }
        /// <summary>
        /// Gets the coordinates for where on the excel spreadsheet to display the VML comment shape
        /// </summary>
        /// <param name="columnName">Column name of where the comment is located (ie. B)</param>
        /// <param name="rowIndex">Row index of where the comment is located (ie. 2)</param>
        /// <returns><see cref="<x:Anchor>"/> coordinates in the form of a comma separated list</returns>
        private static string GetAnchorCoordinatesForVMLCommentShape(string columnName, string rowIndex)
        {
            string coordinates = string.Empty;
            int startingRow = 0;
            //int startingColumn = GetColumnIndexFromName(columnName).Value;
            int startingColumn = ExcelHelper.GetColumnIndexFromName(columnName) - 1;

            // From (upper right coordinate of a rectangle)
            // [0] Left column
            // [1] Left column offset
            // [2] Left row
            // [3] Left row offset
            // To (bottom right coordinate of a rectangle)
            // [4] Right column
            // [5] Right column offset
            // [6] Right row
            // [7] Right row offset
            List<int> coordList = new List<int>(8) { 0, 0, 0, 0, 0, 0, 0, 0 };

            if (int.TryParse(rowIndex, out startingRow))
            {
                // Make the row be a zero based index
                startingRow -= 1;

                coordList[0] = startingColumn + 1; // If starting column is A, display shape in column B
                coordList[1] = 15;
                coordList[2] = startingRow;
                coordList[4] = startingColumn + 3; // If starting column is A, display shape till column D
                coordList[5] = 15;
                coordList[6] = startingRow + 3; // If starting row is 0, display 3 rows down to row 3

                // The row offsets change if the shape is defined in the first row
                if (startingRow == 0)
                {
                    coordList[3] = 2;
                    coordList[7] = 16;
                }
                else
                {
                    coordList[3] = 10;
                    coordList[7] = 4;
                }

                coordinates = string.Join(",", coordList.ConvertAll<string>(x => x.ToString()).ToArray());
            }

            return coordinates;
        }
        private static List<char> Letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public static int? GetColumnIndexFromName(string columnName)
        {
            int? columnIndex = null;

            string[] colLetters = Regex.Split(columnName, "([A-Z]+)");
            colLetters = colLetters.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (colLetters.Count() <= 2)
            {
                int index = 0;
                foreach (string col in colLetters)
                {
                    List<char> col1 = colLetters.ElementAt(index).ToCharArray().ToList();
                    int? indexValue = Letters.IndexOf(col1.ElementAt(index));

                    if (indexValue != -1)
                    {
                        // The first letter of a two digit column needs some extra calculations
                        if (index == 0 && colLetters.Count() == 2)
                        {
                            columnIndex = columnIndex == null ? (indexValue + 1) * 26 : columnIndex + ((indexValue + 1) * 26);
                        }
                        else
                        {
                            columnIndex = columnIndex == null ? indexValue : columnIndex + indexValue;
                        }
                    }

                    index++;
                }
            }

            return columnIndex;
        }

        /// <summary>
        /// Creates the VML Shape XML for a comment. It determines the positioning of the
        /// comment in the excel document based on the column name and row index.
        /// </summary>
        /// <param name="columnName">Column name containing the comment</param>
        /// <param name="rowIndex">Row index containing the comment</param>
        /// <returns>VML Shape XML for a comment</returns>
        private static string GetCommentVMLShapeXML(string columnName, string rowIndex)
        {
            string commentVmlXml = string.Empty;

            // Parse the row index into an int so we can subtract one
            int commentRowIndex;
            if (int.TryParse(rowIndex, out commentRowIndex))
            {
                commentRowIndex -= 1;

                commentVmlXml = "<v:shape id=\"" + Guid.NewGuid().ToString().Replace("-", "") + "\" type=\"#_x0000_t202\" style=\'position:absolute;\r\n  margin-left:59.25pt;margin-top:1.5pt;width:96pt;height:55.5pt;z-index:1;\r\n  visibility:hidden\' fillcolor=\"#ffffe1\" o:insetmode=\"auto\">\r\n  <v:fill color2=\"#ffffe1\"/>\r\n" +
                "<v:shadow on=\"t\" color=\"black\" obscured=\"t\"/>\r\n  <v:path o:connecttype=\"none\"/>\r\n  <v:textbox style=\'mso-fit-shape-to-text:true'>\r\n   <div style=\'text-align:left\'></div>\r\n  </v:textbox>\r\n  <x:ClientData ObjectType=\"Note\">\r\n   <x:MoveWithCells/>\r\n" +
                "<x:SizeWithCells/>\r\n   <x:Anchor>\r\n" + GetAnchorCoordinatesForVMLCommentShape(columnName, rowIndex) + "</x:Anchor>\r\n   <x:AutoFill>False</x:AutoFill>\r\n   <x:Row>" + commentRowIndex + "</x:Row>\r\n   <x:Column>" + (ExcelHelper.GetColumnIndexFromName(columnName) - 1) /*GetColumnIndexFromName(columnName)*/ + "</x:Column>\r\n  </x:ClientData>\r\n </v:shape>";
            }

            return commentVmlXml;
        }
        /// <summary>
        /// Adds all the comments defined in the commentsToAddDict dictionary to the worksheet
        /// </summary>
        /// <param name="worksheetPart">Worksheet Part</param>
        /// <param name="commentsToAddDict">Dictionary of cell references as the key (ie. A1) and the comment text as the value</param>
        public static void InsertComments(WorksheetPart worksheetPart, Dictionary<string, string> commentsToAddDict)
        {
            if (commentsToAddDict.Any())
            {
                string commentsVmlXml = string.Empty;

                // Create all the comment VML Shape XML
                foreach (var commentToAdd in commentsToAddDict)
                {
                    commentsVmlXml += GetCommentVMLShapeXML(ExcelHelper.GetColumnName(commentToAdd.Key), ExcelHelper.GetRowIndex1(commentToAdd.Key).ToString());
                }

                // The VMLDrawingPart should contain all the definitions for how to draw every comment shape for the worksheet
                VmlDrawingPart vmlDrawingPart = worksheetPart.AddNewPart<VmlDrawingPart>();
                using (XmlTextWriter writer = new XmlTextWriter(vmlDrawingPart.GetStream(FileMode.Create), Encoding.UTF8))
                {

                    writer.WriteRaw("<xml xmlns:v=\"urn:schemas-microsoft-com:vml\"\r\n xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n xmlns:x=\"urn:schemas-microsoft-com:office:excel\">\r\n <o:shapelayout v:ext=\"edit\">\r\n  <o:idmap v:ext=\"edit\" data=\"1\"/>\r\n" +
                    "</o:shapelayout><v:shapetype id=\"_x0000_t202\" coordsize=\"21600,21600\" o:spt=\"202\"\r\n  path=\"m,l,21600r21600,l21600,xe\">\r\n  <v:stroke joinstyle=\"miter\"/>\r\n  <v:path gradientshapeok=\"t\" o:connecttype=\"rect\"/>\r\n </v:shapetype>"
                    + commentsVmlXml + "</xml>");
                }

                // Create the comment elements
                foreach (var commentToAdd in commentsToAddDict)
                {
                    WorksheetCommentsPart worksheetCommentsPart = worksheetPart.WorksheetCommentsPart ?? worksheetPart.AddNewPart<WorksheetCommentsPart>();

                    // We only want one legacy drawing element per worksheet for comments
                    if (worksheetPart.Worksheet.Descendants<LegacyDrawing>() == null || worksheetPart.Worksheet.Descendants<LegacyDrawing>().SingleOrDefault() == null)
                    {
                        string vmlPartId = worksheetPart.GetIdOfPart(vmlDrawingPart);
                        LegacyDrawing legacyDrawing = new LegacyDrawing() { Id = vmlPartId };
                        worksheetPart.Worksheet.Append(legacyDrawing);
                    }

                    Comments comments;
                    bool appendComments = false;
                    if (worksheetPart.WorksheetCommentsPart.Comments != null)
                    {
                        comments = worksheetPart.WorksheetCommentsPart.Comments;
                    }
                    else
                    {
                        comments = new Comments();
                        appendComments = true;
                    }

                    // We only want one Author element per Comments element
                    if (worksheetPart.WorksheetCommentsPart.Comments == null)
                    {
                        Authors authors = new Authors();
                        Author author = new Author();
                        author.Text = "Author Name";
                        authors.Append(author);
                        comments.Append(authors);
                    }

                    CommentList commentList;
                    bool appendCommentList = false;
                    if (worksheetPart.WorksheetCommentsPart.Comments != null &&
                        worksheetPart.WorksheetCommentsPart.Comments.Descendants<CommentList>().SingleOrDefault() != null)
                    {
                        commentList = worksheetPart.WorksheetCommentsPart.Comments.Descendants<CommentList>().Single();
                    }
                    else
                    {
                        commentList = new CommentList();
                        appendCommentList = true;
                    }

                    Comment comment = new Comment() { Reference = commentToAdd.Key, AuthorId = (UInt32Value)0U };

                    CommentText commentTextElement = new CommentText();

                    Run run = new Run();

                    RunProperties runProperties = new RunProperties();
                    Bold bold = new Bold();
                    FontSize fontSize = new FontSize() { Val = 8D };
                    Color color = new Color() { Indexed = (UInt32Value)81U };
                    RunFont runFont = new RunFont() { Val = "Tahoma" };
                    RunPropertyCharSet runPropertyCharSet = new RunPropertyCharSet() { Val = 1 };

                    runProperties.Append(bold);
                    runProperties.Append(fontSize);
                    runProperties.Append(color);
                    runProperties.Append(runFont);
                    runProperties.Append(runPropertyCharSet);
                    Text text = new Text();
                    text.Text = commentToAdd.Value;

                    run.Append(runProperties);
                    run.Append(text);

                    commentTextElement.Append(run);
                    comment.Append(commentTextElement);
                    commentList.Append(comment);

                    // Only append the Comment List if this is the first time adding a comment
                    if (appendCommentList)
                    {
                        comments.Append(commentList);
                    }

                    // Only append the Comments if this is the first time adding Comments
                    if (appendComments)
                    {
                        worksheetCommentsPart.Comments = comments;
                    }
                }
            }
        }
    }
    public class Validators
    {
        public static List<AttributeError> Validate(object o)
        {
            List<AttributeError> listErrros = new List<AttributeError>();
            Type type = o.GetType();
            PropertyInfo[] properties = o.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), inherit: true);

                foreach (var customAttribute in customAttributes)
                {
                    var validationAttribute = (ValidationAttribute)customAttribute;

                    bool isValid = validationAttribute.IsValid(propertyInfo.GetValue(o, BindingFlags.GetProperty, null, null, null));

                    if (!isValid)
                    {
                        AttributeError er = new AttributeError();
                        er.AttributeName = propertyInfo.Name;
                        er.MessageError = validationAttribute.ErrorMessage;
                        listErrros.Add(er);
                    }
                }
            }

            return listErrros;
        }
    }
    public class AttributeError
    {
        public string AttributeName { get; set; }
        public string MessageError { get; set; }
    }
    public class ObjExcelHelper
    {

    }
    public class CustomDescriptionAttribute : System.Attribute
    {
        private string target = "IN_CLASS";
        public string Description = "";
        private string tag_message = "";
        public CustomDescriptionAttribute()
        {

        }
        public CustomDescriptionAttribute(string _Description)
        {
            this.Description = _Description;
        }
        public CustomDescriptionAttribute(string _target, string _tag_message)
        {
            this.target = _target;
            this.tag_message = _tag_message;

            var _description = System.Configuration.ConfigurationSettings.AppSettings[this.tag_message];
            this.Description = _description != null ? _description.ToString() : "";
        }
    }

    public class ExcelLayout
    {
        public List<string> MergeCells { get; set; }
        public List<string> TotalCells { get; set; }
        public string Color { get; set; }
        public string Align { get; set; }

    }
    public static class ExcelLayoutStyle
    {
        public static void AddStyleColumHeader(WorkbookPart workPart, Cell c, string color)
        {
            Fills fill = AddFill(workPart.WorkbookStylesPart.Stylesheet.Fills, color);
            Fonts fs = AddFont(workPart.WorkbookStylesPart.Stylesheet.Fonts);
            AddCellFormat(workPart.WorkbookStylesPart.Stylesheet.CellFormats, workPart.WorkbookStylesPart.Stylesheet.Fonts, workPart.WorkbookStylesPart.Stylesheet.Fills, GetAlignment());
            c.StyleIndex = (UInt32)(workPart.WorkbookStylesPart.Stylesheet.CellFormats.Elements<CellFormat>().Count() - 1);

        }
        public static void AddStyleColumTotal(WorkbookPart workPart, Cell c, string color)
        {
            Fills fill = AddFill(workPart.WorkbookStylesPart.Stylesheet.Fills, color);
            Fonts fs = AddFont(workPart.WorkbookStylesPart.Stylesheet.Fonts);
            AddCellFormat(workPart.WorkbookStylesPart.Stylesheet.CellFormats, workPart.WorkbookStylesPart.Stylesheet.Fonts, workPart.WorkbookStylesPart.Stylesheet.Fills, null);
            //c.StyleIndex = (UInt32)(workPart.WorkbookStylesPart.Stylesheet.CellFormats.Elements<CellFormat>().Count()+1);

        }

        public static void AddStyleColumMerging(WorkbookPart workPart, Cell c)
        {
            Fills fill = AddFill(workPart.WorkbookStylesPart.Stylesheet.Fills, "ffffff");
            Fonts fs = AddFontMerging(workPart.WorkbookStylesPart.Stylesheet.Fonts);
            Border border = GenerateBorder();
            AddCellFormat(workPart.WorkbookStylesPart.Stylesheet.CellFormats, workPart.WorkbookStylesPart.Stylesheet.Fonts, workPart.WorkbookStylesPart.Stylesheet.Fills, GetAlignment(), border);
            c.StyleIndex = (UInt32)(workPart.WorkbookStylesPart.Stylesheet.CellFormats.Elements<CellFormat>().Count() - 1);

        }

        public static Fills AddFill(Fills fills, string rbgColor)
        {
            Fill fill = new Fill();

            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor1 = new ForegroundColor() { Rgb = rbgColor };
            BackgroundColor backgroundColor1 = new BackgroundColor() { Indexed = (UInt32Value)64U };

            patternFill1.Append(foregroundColor1);
            patternFill1.Append(backgroundColor1);

            fill.Append(patternFill1);
            fills.Append(fill);
            return fills;
        }
        public static Alignment GetAlignment()
        {
            Alignment alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center };
            return alignment;
        }
        public static Alignment GetAlignmentForTotal()
        {
            Alignment alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Bottom };
            return alignment;
        }
        public static Fonts AddFont(Fonts fs)
        {
            Font font = new Font();
            Bold bold1 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = 11D };
            Color color = new Color() { Theme = (UInt32Value)0U };
            FontName fontName2 = new FontName() { Val = "Arial" };
            FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 1 };
            FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };
            font.Append(bold1);
            font.Append(fontSize2);
            font.Append(color);
            font.Append(fontName2);
            font.Append(fontFamilyNumbering2);
            font.Append(fontScheme2);

            fs.Append(font);
            return fs;
        }
        public static Fonts AddFontMerging(Fonts fs)
        {
            Font font = new Font();
            FontSize fontSize2 = new FontSize() { Val = 11D };
            Color color = new Color() { Theme = (UInt32Value)1U };
            FontName fontName2 = new FontName() { Val = "Arial" };
            FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };
            font.Append(fontSize2);
            font.Append(color);
            font.Append(fontName2);
            font.Append(fontFamilyNumbering2);
            font.Append(fontScheme2);

            fs.Append(font);
            return fs;
        }
        public static void AddCellFormat(CellFormats cf, Fonts font, Fills fill, Alignment alignment, Border border = null)
        {
            CellFormat cellFormat = new CellFormat() { Alignment = alignment, FontId = (UInt32)(font.Elements<Font>().Count() - 1), FillId = (UInt32)(fill.Elements<Fill>().Count() - 1), BorderId = 0, ApplyFill = true };
            cf.Append(cellFormat);
        }

        public static void SetColumnWidth(Worksheet worksheet, uint Index, DoubleValue dwidth)
        {
            Columns cs = worksheet.GetFirstChild<Columns>();
            if (cs != null)
            {
                IEnumerable<Column> ic = cs.Elements<Column>().Where(r => r.Min == Index).Where(r => r.Max == Index);
                if (ic.Count() > 0)
                {
                    Column c = ic.First();
                    c.Width = dwidth;
                }
                else
                {
                    Column c = new Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                    cs.Append(c);
                }
            }
            else
            {
                cs = new Columns();
                Column c = new Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                cs.Append(c);
                worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetFormatProperties>());
            }
        }
        public static int? GetColumnIndex(string cellReference)
        {
            if (string.IsNullOrEmpty(cellReference))
            {
                return null;
            }

            //remove digits
            string columnReference = Regex.Replace(cellReference.ToUpper(), @"[\d]", string.Empty);

            int columnNumber = -1;
            int mulitplier = 1;

            //working from the end of the letters take the ASCII code less 64 (so A = 1, B =2...etc)
            //then multiply that number by our multiplier (which starts at 1)
            //multiply our multiplier by 26 as there are 26 letters
            foreach (char c in columnReference.ToCharArray().Reverse())
            {
                columnNumber += mulitplier * ((int)c - 64);

                mulitplier = mulitplier * 26;
            }

            //the result is zero based so return columnnumber + 1 for a 1 based answer
            //this will match Excel's COLUMN function
            return columnNumber + 1;
        }
        public static Border GenerateBorder()
        {
            Border border2 = new Border();

            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Medium };
            Color color1 = new Color() { Indexed = (UInt32Value)1U };

            leftBorder2.Append(color1);

            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Medium };
            Color color2 = new Color() { Indexed = (UInt32Value)1U };

            rightBorder2.Append(color2);

            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Medium };
            Color color3 = new Color() { Indexed = (UInt32Value)1U };

            topBorder2.Append(color3);

            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Medium };
            Color color4 = new Color() { Indexed = (UInt32Value)1U };

            bottomBorder2.Append(color4);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            return border2;
        }

    }
}
