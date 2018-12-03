using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using LibCore.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibCore.Helpers
{
    public class CellRangeTemplate
    {
        private Regex PARAM_PATTERN;
        public string regexVariable = @"\[\%(.*)\%\]";

        public CellRangeTemplate()
        {
            ListVariable = new List<string>();
            AllCellsConfig = new List<dynamic>();
        }
        public CellRangePosition CellRange { get; set; }
        public IEnumerable<string> ListVariable { get; set; }
        public IEnumerable<IGrouping<Row, Cell>> AllCells { get; set; }

        public List<dynamic> AllCellsConfig { get; set; }

        public void Init(SpreadsheetDocument document)
        {
            AllCells = document.FindCellsByRange(CellRange);

            CellRange.SheetTemplate = document.FindSheetByName(CellRange.SheetName);
            CellRange.WorksheetPart = ((WorksheetPart)document.WorkbookPart.GetPartById(CellRange.SheetTemplate.Id));
            CellRange.MergeCells = CellRange.WorksheetPart.Worksheet.GetFirstChild<MergeCells>();

            if (CellRange.MergeCells != null)
            {
                CellRange.MergeCellsDic = CellRange.MergeCells.ToDictionary(e => (e as MergeCell).Reference.Value.ToUpper().Split(':')[0], e => (e as MergeCell));
            }

            foreach (var rowGroup in AllCells)
            {
                foreach (Cell cell in rowGroup)
                {
                    if (cell.DataType != null && cell.CellValue != null)
                    {
                        string stringValue = null;

                        switch (cell.DataType.Value)
                        {
                            case CellValues.SharedString:
                                {
                                    int index = int.Parse(cell.CellValue.Text);
                                    SharedStringItem stringItem = document.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index);

                                    stringValue = stringItem.InnerText;
                                    break;
                                }
                            case CellValues.String:
                                {
                                    stringValue = cell.CellValue.Text;
                                    break;
                                }
                        }

                        if (PARAM_PATTERN == null)
                        {
                            PARAM_PATTERN = new Regex(this.regexVariable);
                        }
                        if (PARAM_PATTERN.IsMatch(stringValue))
                        {
                            var match = PARAM_PATTERN.Match(stringValue);
                            var Func = "";
                            string Variable = "";
                            string VariableFull = Variable = match.Groups[1].Value;


                            string regexFunc = @".\:(.*)";
                            var regexFuncRexgex = new Regex(regexFunc);
                            if (regexFuncRexgex.IsMatch(VariableFull))
                            {
                                var match1 = regexFuncRexgex.Match(VariableFull);

                                Func = match1.Groups[1].Value;
                                Variable = VariableFull.Replace(":" + Func, "");
                            }

                            VariableFull = string.Format("[%{0}%]", VariableFull);
                            AllCellsConfig.Add(new
                            {
                                TextOrigin = stringValue,
                                Cell = cell,
                                VariableFull = VariableFull,
                                Variable = Variable,
                                Func = Func,
                            });
                        }
                    }
                }
            }
        }

        public void FillData<T>(T data)
        {
            for (int i = 0; i < AllCellsConfig.Count; i++)
            {
                var config = AllCellsConfig[i];
                var TextOrigin = config.TextOrigin as string;
                var Variable = config.Variable as string;
                var VariableFull = config.VariableFull as string;
                var Func = config.Func as string;
                var Cell = config.Cell as Cell;

                object value = null;
                if (Variable.IndexOf('.') > 0)
                {
                    var Variables = Variable.Split('.');
                    value = data;
                    for (int j = 0; j < Variables.Length; j++)
                    {
                        value = value.GetDataFromObj(Variables[j]);
                        if (value == null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    value = data.GetDataFromObj(Variable);
                }

                if (!string.IsNullOrEmpty(Func))
                {
                    if (Func.Contains("("))
                    {
                        MethodInfo methodInfo = value.GetType().GetMethod(Func);
                        value = methodInfo.Invoke(this, new object[] { });
                        if (Func.StartsWith("format"))
                        {
                            string regexStr = "format\\(\\\"(.*)\\\"\\)";
                            var regexRexgex = new Regex(regexStr);
                            if (regexRexgex.IsMatch(VariableFull))
                            {
                                var match1 = regexRexgex.Match(Func);
                                var format = match1.Groups[1].Value;
                                //Stylesheet stylesheet = new Stylesheet();

                                //var nfs = stylesheet.NumberFormats;

                                //NumberFormat nf = new NumberFormat();
                                //nf.NumberFormatId = (uint)(styleDefault + indexStyle);
                                //nf.FormatCode = "@";
                                //nfs.Append(nf);

                                //nfs.Count = (uint)nfs.ChildElements.Count;
                                //cf.NumberFormatId = nf.NumberFormatId;
                                //Cell.StyleIndex = 1;
                            }
                        }
                    }
                    else
                    {
                        value = value.GetDataFromObj(Func);
                    }
                }

                value = value == null ? "" : value;
                if (VariableFull == TextOrigin)
                {
                    Cell.SetCellAutoValue(value);
                }
                else
                {
                    TextOrigin = TextOrigin.Replace(VariableFull, value.ToString());
                    Cell.SetCellAutoValue(TextOrigin);
                }
            }
        }
    }

    public class CellRangePosition
    {
        private static Regex PATTERN = new Regex(@"^(?:(.*?)\!)?(?:\$)?([A-Z]+)(?:\$)?([0-9]+)(?:\:(?:\$)?([A-Z]+)(?:\$)?([0-9]+))?$");

        public string SheetName;
        public Sheet SheetTemplate;
        public WorksheetPart WorksheetPart;
        public MergeCells MergeCells;
        public Dictionary<string, MergeCell> MergeCellsDic;
        public CellPosition Start;
        public CellPosition End;

        #region constructors

        public CellRangePosition()
        {
            MergeCellsDic = new Dictionary<string, MergeCell>();
        }

        public CellRangePosition(string value)
        {
            var match = PATTERN.Match(value);
            SheetName = match.Groups[1].Value;
            Start = new CellPosition(match.Groups[3].Value, match.Groups[2].Value);

            string endColumn = match.Groups[4].Value;

            if (endColumn.Length == 0)
            {
                End = new CellPosition(Start);
            }
            else
            {
                End = new CellPosition(match.Groups[5].Value, endColumn);
            }
        }

        public CellRangePosition(string sheetName, CellPosition start, CellPosition end)
        {
            this.SheetName = sheetName;

            this.Start = new CellPosition(start);
            this.End = new CellPosition(end);

            Reorder();
        }
        #endregion

        public int Height
        {
            get { return End.Row - Start.Row; }
        }

        public int Width
        {
            get { return End.Column - Start.Column; }
        }

        private void Reorder()
        {
            if (Start.Row > End.Row)
            {
                int temp = Start.Row;
                Start.Row = End.Row;
                End.Row = temp;
            }

            if (Start.Column > End.Column)
            {
                int temp = Start.Column;
                Start.Column = End.Column;
                End.Column = temp;
            }

        }

        public bool Contains(CellPosition item)
        {
            bool result = Start.Row <= item.Row && item.Row <= End.Row &&
                          Start.Column <= item.Column && item.Column <= End.Column;

            return result;
        }
    }

    public class CellPosition
    {
        private static Regex PATTERN = new Regex(@"^([A-Z]+)([0-9]+)$");

        private int row;
        private int column;
        public CellPosition(string value)
        {
            var match = PATTERN.Match(value);
            column = match.Groups[1].Value.Column2Int();
            row = int.Parse(match.Groups[2].Value);
        }

        public CellPosition(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public CellPosition(string row, string column)
        {
            this.row = int.Parse(row);
            this.column = column.Column2Int();
        }

        public CellPosition(CellPosition right)
        {
            row = right.row;
            column = right.column;
        }
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Column
        {
            get { return column; }
            set { column = value; }
        }



        public CellPosition CalculateOffset(CellPosition value)
        {
            return new CellPosition(row - value.row, column - value.column);
        }

        public void OffsetIt(CellPosition value)
        {
            row += value.row;
            column += value.column;
        }

        static public string OffsetIt(string cellRef, CellPosition offset)
        {
            CellPosition result = new CellPosition(cellRef);
            result.OffsetIt(offset);
            return result.ToString();
        }
        static public string ToString(int row, int column)
        {
            return string.Format("{0}{1}", column.Int2Column(), row);
        }
        public override string ToString()
        {
            return ToString(row, column);
        }
    }

    public class ExcelTemplateHelper : IDisposable
    {
        public enum DirectionType : int
        {
            LEFT_TO_RIGHT
            , TOP_TO_DOWN
        }

        private DirectionType direction;
        private Row currentRow;

        private SheetData currentSheetData;
        private Worksheet currentWorksheet;
        private MergeCells currentMergeCells;
        private Sheet currentSheet;
        private Stylesheet currentStylesheet;
        private string currentSheetName;
        public bool DeleteSheetTemplate { get; set; }

        private SheetData currentTempSheetData;
        private Worksheet currentTempWorksheet;
        private MergeCells currentTempMergeCells;
        private Sheet currentTempSheet;
        private string currentTempSheetName;


        private CellPosition currentPosition;
        public SpreadsheetDocument document;
        //public SpreadsheetDocument documentTemplate;

        public string TempSheetName
        {
            get { return currentTempSheetName; }
            set
            {
                currentTempSheetName = value;
                if (currentTempSheetName != null)
                {
                    currentTempSheet = document.FindSheetByName(currentTempSheetName);
                    currentTempWorksheet = document.FindWorksheetBySheet(currentTempSheet);
                    currentTempSheetData = currentTempWorksheet.GetFirstChild<SheetData>();
                    currentTempMergeCells = currentTempWorksheet.GetFirstChild<MergeCells>();
                }
                else
                {
                    currentTempSheet = null;
                    currentTempWorksheet = null;
                    currentTempSheetData = null;
                    currentTempMergeCells = null;
                }
            }
        }
        public string CurrentSheetName
        {
            get { return currentSheetName; }
            set
            {
                currentSheetName = value;
                if (currentSheetName != null)
                {
                    currentSheet = document.FindSheetByName(currentSheetName);
                    currentWorksheet = document.FindWorksheetBySheet(currentSheet);
                    currentSheetData = currentWorksheet.GetFirstChild<SheetData>();
                    currentMergeCells = currentWorksheet.GetFirstChild<MergeCells>();

                    WorkbookStylesPart workbookStylesPart = document.WorkbookPart.WorkbookStylesPart;
                }
                else
                {
                    currentSheet = null;
                    currentWorksheet = null;
                    currentSheetData = null;
                    currentMergeCells = null;
                }
            }
        }


        public CellPosition CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;

                //reseting this since this is a cached member
                currentRow = null;
            }
        }

        public DirectionType Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        #region contructors
        void ExcelTemplateHelperCreate(Stream fileStream)
        {
            DeleteSheetTemplate = true;
            fileStream.Position = 0;
            document = SpreadsheetDocument.Open(fileStream, true/*isEditable*/);

            //Stream outStream = new MemoryStream();

            //fileStream.Position = 0;
            //byte[] bytes = new byte[fileStream.Length];
            //fileStream.Read(bytes, 0, (int)fileStream.Length);
            //outStream.Write(bytes, 0, bytes.Length);

            //document = SpreadsheetDocument.Open(outStream, true/*isEditable*/);
        }

        void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
        }
        public ExcelTemplateHelper(Stream fileStream, Stream outStream)
        {
            fileStream.Position = 0;
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            outStream.Write(bytes, 0, bytes.Length);

            ExcelTemplateHelperCreate(outStream);
        }
        public ExcelTemplateHelper(string templateFileName, Stream outStream)
        {
            using (FileStream fs = File.OpenRead(templateFileName))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                outStream.Write(bytes, 0, bytes.Length);
            }

            ExcelTemplateHelperCreate(outStream);
        }
        public ExcelTemplateHelper(Stream fileStream, string generatedFileName)
        {

            var outStream = new FileStream(generatedFileName, FileMode.Create, FileAccess.Write);
            ReadWriteStream(fileStream, outStream);
            outStream.Close();
            outStream.Dispose();
            outStream.Close();
            outStream.Dispose();

            Stream streamIn = System.IO.File.Open(generatedFileName, FileMode.Open);
            ExcelTemplateHelperCreate(streamIn);
        }

        public ExcelTemplateHelper(string templateFileName, string generatedFileName)
        {
            File.Copy(templateFileName, generatedFileName, true /*overwrite*/);

            Stream streamIn = System.IO.File.Open(generatedFileName, FileMode.Open);
            ExcelTemplateHelperCreate(streamIn);
        }
        #endregion

        public CellRangePosition GetDefinedName(string name)
        {
            DefinedName definedName = (
                        from item in document.WorkbookPart.Workbook.DefinedNames.Elements<DefinedName>()
                            //from item in documentTemplate.WorkbookPart.Workbook.DefinedNames.Elements<DefinedName>()
                        where item.Name == name
                        select item).Single();
            return new CellRangePosition(definedName.Text);
        }

        public CellRangeTemplate CreateTemplate(string DefinedName)
        {
            var range = new CellRangeTemplate()
            {
                CellRange = GetDefinedName(DefinedName),
            };
            range.Init(document);
            return range;
        }

        public void InsertData<T>(CellRangeTemplate top, T data, double? rowHeight = null)
        {
            InsertDatas(top, new List<object>() { data }, rowHeight);
        }

        public void Insert(CellRangeTemplate top, double? rowHeight = null)
        {
            InsertDatas(top, new List<object>() { new { } }, rowHeight);
        }

        public void InsertDatas<T>(CellRangeTemplate top, IEnumerable<T> datas, double? rowHeight = null)
        {
            datas = datas ?? new List<T>();
            for (int i = 0; i < datas.Count(); i++)
            {
                var data = datas.ElementAt(i);

                top.FillData(data);
                InsertTemplate(top, rowHeight);
            }
        }

        public void CopyWidth()
        {
            CopyWidth(currentTempWorksheet, currentWorksheet);
        }

        private void InsertTemplate(CellRangeTemplate range, double? rowHeight = null)
        {

            CopyRange(ref range, currentSheetData, currentWorksheet, ref currentPosition, rowHeight);

            if (direction == DirectionType.TOP_TO_DOWN)
            {
                currentPosition.Row = currentPosition.Row + range.CellRange.Height + 1;
            }
            else
            {
                currentPosition.Column = currentPosition.Column + range.CellRange.Width + 1;
            }
        }

        private void MoveCurrentRow(int rowIndex)
        {
            if (currentRow == null)
            {
                var rows = currentSheetData.Elements<Row>();
                if (rows.GetEnumerator().MoveNext())
                {
                    //maybe the last row is still before the curretRow, so in this case no need to iterate trough the rows
                    Row lastRow = (Row)currentSheetData.LastChild;
                    if (lastRow.RowIndex > rowIndex)
                    {
                        //we search from the beginnig
                        foreach (Row item in rows)
                        {
                            if (item.RowIndex.Value > rowIndex)
                            {
                                currentRow = item;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                //trying to seach from the last position backward or forward
                if (currentRow.RowIndex > rowIndex)
                {
                    //going backward
                    Row previousRow;
                    while ((previousRow = currentRow.PreviousSibling<Row>()) != null &&
                            previousRow.RowIndex > rowIndex)
                    {
                        currentRow = previousRow;
                    }

                }
                else
                {
                    //going forward
                    while (currentRow != null && currentRow.RowIndex <= rowIndex)
                    {
                        currentRow = currentRow.NextSibling<Row>();
                    }
                }
            }
        }

        private void CopyRange(ref CellRangeTemplate sourceRange, SheetData sheetData, Worksheet worksheet, ref CellPosition target, double? rowHeight = null)
        {
            #region
            Sheet sheetTemplate = sourceRange.CellRange.SheetTemplate;
            var workbookPartTemplate = sourceRange.CellRange.WorksheetPart;
            MergeCells mergeCellsTemplate = sourceRange.CellRange.MergeCells;

            MergeCells mergeCells = worksheet.GetFirstChild<MergeCells>();

            if (false && workbookPartTemplate.DrawingsPart != null && worksheet.WorksheetPart.DrawingsPart == null)
            {
                var drawingsPart = worksheet.WorksheetPart.AddPart<DrawingsPart>(workbookPartTemplate.DrawingsPart);

                drawingsPart = worksheet.WorksheetPart.DrawingsPart;

                if (!worksheet.WorksheetPart.Worksheet.ChildElements.OfType<Drawing>().Any())
                {
                    worksheet.WorksheetPart.Worksheet.Append(new Drawing { Id = worksheet.WorksheetPart.GetIdOfPart(drawingsPart) });
                }
            }

            Dictionary<string, MergeCell> mergeCellTemplateDic = sourceRange.CellRange.MergeCellsDic;
            #endregion

            CellPosition source = sourceRange.CellRange.Start;
            CellPosition offset = target.CalculateOffset(source);

            var cellsToCopy = document.FindCellsByRange(sourceRange.CellRange);
            for (int i = 0; i < cellsToCopy.Count(); i++)
            {
                var rowGroup = cellsToCopy.ElementAt(i);
                Row keyRow = rowGroup.Key;

                Row targetRow = new Row()
                {
                    RowIndex = (UInt32)(keyRow.RowIndex + offset.Row),
                    Height = (short)-1,
                };

                if (rowHeight != null)
                {
                    targetRow.Height = rowHeight;
                    targetRow.CustomHeight = true;
                }

                MoveCurrentRow((int)targetRow.RowIndex.Value);
                sheetData.InsertBefore(targetRow, currentRow);

                foreach (Cell cellToCopy in rowGroup)
                {
                    Cell targetCell = (Cell)cellToCopy.Clone();

                    targetCell.CellReference = CellPosition.OffsetIt(targetCell.CellReference, offset);

                    targetRow.Append(targetCell);

                    MergeCell _findMerge;
                    if (mergeCellTemplateDic != null && mergeCellTemplateDic.TryGetValue(cellToCopy.CellReference.Value.ToUpper(), out _findMerge))
                    {
                        var positionParent = _findMerge.Reference.Value.Split(':');
                        CellPosition offsetStart = new CellPosition(positionParent[0]);
                        CellPosition offsetEnd = new CellPosition(positionParent[1]);

                        var celRefNew = new CellPosition(targetCell.CellReference);


                        if (mergeCells == null)
                        {
                            var a = new MergeCells();
                            worksheet.InsertAfter(a, sheetData);
                            mergeCells = worksheet.GetFirstChild<MergeCells>();
                        }
                        var mergeCell = new MergeCell();
                        mergeCell.Reference = celRefNew.ToString() + ":" + new CellPosition(celRefNew.Row + (offsetEnd.Row - offsetStart.Row), celRefNew.Column + (offsetEnd.Column - offsetStart.Column)).ToString();
                        mergeCells.Append(mergeCell);
                        mergeCells.Count = (mergeCells.Count ?? 0) + 1;
                    }
                }

            }
        }

        private void CopyWidth(Worksheet worksheet, Worksheet worksheetDesc)
        {
            var Columns = worksheet.GetFirstChild<Columns>();
            var ColumnsDesc = worksheetDesc.GetFirstChild<Columns>();

            if (Columns != null)
            {
                if (ColumnsDesc == null)
                {
                    ColumnsDesc = new Columns();
                    var SheetData = worksheetDesc.GetFirstChild<SheetData>();
                    worksheetDesc.InsertBefore(ColumnsDesc, SheetData);
                }
                ColumnsDesc.RemoveAllChildren();
                foreach (Column e in Columns)
                {
                    ColumnsDesc.Append(new Column()
                    {
                        Width = e.Width,
                        Min = e.Min,
                        Max = e.Max,
                        CustomWidth = e.CustomWidth,
                    });
                }
            }
        }

        public void DeleteSheet(string name)
        {
            var sheet = document.FindSheetByName(name);
            var worksheet = sheet.Parent as Worksheet;
            sheet.Remove();
        }

        public void Dispose()
        {
            if (DeleteSheetTemplate)
            {
                DeleteSheet(TempSheetName);
            }

            document.WorkbookPart.Workbook.Save();
            document.Close();
            document = null;
        }
    }
}
