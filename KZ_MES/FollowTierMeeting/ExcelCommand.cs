using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml.Drawing;
using System.Diagnostics;
using System.Globalization;
using SJeMES_Control_Library;

namespace FollowTierMeeting
{
    class ExcelCommand
    {
        public static int processindex = 0;
        public const string AllDateFormat = @"m\/d\/yyyy mm\/dd\/yyyy d\/m\/yyyy dd\/mm\/yyyy yyyy\/m\/d yyyy\/mm\/dd m/d/yyyy mm/dd/yyyy d/m/yyyy dd/mm/yyyy yyyy/m/d yyyy/mm/dd m-d-yyyy mm-dd-yyyy d-m-yyyy dd-mm-yyyy yyyy-m-d yyyy-mm-dd m.d.yyyy mm.dd.yyyy d.m.yyyy dd.mm.yyyy yyyy.m.d yyyy.mm.dd m,d,yyyy mm,dd,yyyy d,m,yyyy dd,mm,yyyy yyyy,m,d yyyy,mm,dd m d yyyy mm dd yyyy d m yyyy dd mm yyyy yyyy m d yyyy mm dd";

        public static void SumColDept(ExcelWorksheet oSheet, int rowIndex, int colIndex, int col)
        {
            var cell = oSheet.Cells[rowIndex, colIndex];          
            cell.Formula = "Sum(" + oSheet.Cells[rowIndex, col + 1].Address + ":" + oSheet.Cells[rowIndex, colIndex - 1].Address + ")";
            cell.Style.Numberformat.Format = "#,000#";
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        }
        public static void SumRowDept(ExcelWorksheet oSheet, int rowIndex, int colIndex, int index)
        {
            for (int i = 1; i <= index; i++)
            {
                oSheet.Cells[rowIndex, i + colIndex].Formula = "Sum(" + oSheet.Cells[colIndex, i + colIndex].Address + ":" + oSheet.Cells[rowIndex - 1, i + colIndex].Address + ")";
                oSheet.Cells[rowIndex, i + colIndex].Style.Numberformat.Format = "#,000#";
                oSheet.Cells[rowIndex, i + colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            }
        }
        public static DataTable WorksheetToDataTable(ExcelWorksheet oSheet, int indexRowHeader=1, int maxcolread = 0,int valueindex=2)
        {
            processindex = 0;
            //DateTime date;
            //bool isDate = false;
            int totalRows = 0;           
            int totalCols = 0;
            try
            {
                if(oSheet.Dimension!=null)
                  totalRows = oSheet.Dimension.End.Row;
                if (oSheet.Dimension!=null)
                totalCols = oSheet.Dimension.End.Column;
            }
            catch { }
            if (maxcolread != 0)
            {
                if (totalCols > maxcolread)
                    totalCols = maxcolread;
            }
            DataTable dt = new DataTable(oSheet.Name);
            DataRow dr = null;
            for (int i = indexRowHeader; i <= totalRows; i++)
            {
                if (i >= valueindex) dr = dt.Rows.Add();              
                for (int j = 1; j <= totalCols; j++)
                {
                    if (i == indexRowHeader)
                    {
                        if (oSheet.Cells[i, j].Value != null)
                        {
                            try
                            {
                                dt.Columns.Add(oSheet.Cells[i, j].Value.ToString().Replace("\n", String.Empty));
                            }
                            catch
                            {
                                dt.Columns.Add(oSheet.Cells[i, j].Value.ToString() + "_" + j);
                            }
                        }
                        else
                            dt.Columns.Add("col:" + i + "_" + j);
                    }
                    else
                    {
                        if (i >= valueindex)
                        {                         
                            if (oSheet.Cells[i, j] != null)
                                dr[j - 1] = GetSetDataType(oSheet, i, j);
                            else
                                dr[j - 1] = " ";                        
                        }
                    }
                    processindex = (int)(((double)(i + j) / (double)(totalCols + totalRows)) * 100);
                }

            }
            processindex = 100;
            return dt;
        }
        public static void ExportExcel(DataTable Indt,string filename = "export", string sheetname = "Sheet1", bool auto_open = false, bool col_is_caption = false)
        {
            try
            {

                using (ExcelPackage excelPkg = new ExcelPackage())
                {
                    // 1. Setting Excel Workbook Properties
                    excelPkg.Workbook.Properties.Author = "Create by Apche Bonus Calculation System";
                    excelPkg.Workbook.Properties.Title = "Bonus Calculation System";

                    // Creating Excel Worksheet
                    ExcelWorksheet oSheet = CreateSheet(excelPkg, sheetname);

                    // Sample DataTable
                    //  DataTable dt =  CreateDataTable();
                    DataTable dt = Indt;

                    // 2. Merge Excel Columns: Merging cells and create a center heading for our table
                    //   oSheet.Cells[1, 1].Value = "Danh sach may tinh kiem tra";
                    //   oSheet.Cells[1, 1, 1, dt.Columns.Count].Merge = true;
                    // Setting Font and Alignment for Header
                    //      oSheet.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
                    //     oSheet.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    int rowIndex = 1;

                    // 3. Setting Excel Cell Backgournd Color during Header Creation
                    // 4. Setting Excel Cell Border during Header Creation
                    // Creating Header                  
                    CreateHeader(oSheet, ref rowIndex, dt, col_is_caption);                 

                    // Putting Data into Cells
                    CreateData(oSheet, ref rowIndex, dt);

                    // 5. Setting Excel Formula during Footer Creation
                    // Creating Footer
                    //     CreateFooter(oSheet, ref rowIndex, dt);

                    // 6. Add Comments in Excel Cell
                    // AddComment(oSheet, 5, 5, "Sample Comment", "Debopam Pal");

                    // 7. Add Image in Excel Sheet
                    //   string imagePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.StartupPath)), "debopam.jpg");
                    //  AddImage(oSheet, 1, 10, imagePath);

                    // 8. Add Custom Objects in Excel Sheet
                    //  AddCustomObject(oSheet, 7, 10, eShapeStyle.Ellipse, "Text inside Ellipse");

                    // Writting bytes by bytes in Excel File                  
                    var saveFileDialoge = new SaveFileDialog();
                    saveFileDialoge.FileName = filename;
                    saveFileDialoge.DefaultExt = ".xlsx";
                    if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            Byte[] content = excelPkg.GetAsByteArray();
                            string path_fileName = saveFileDialoge.FileName;
                            File.WriteAllBytes(path_fileName, content);
                            try
                            {
                                // Openning the created excel file using MS Excel Application
                                ProcessStartInfo pi = new ProcessStartInfo(path_fileName);
                                Process.Start(pi);
                            }
                            catch { }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                }

            }
            catch { }          
         
        }
        public static void ExportExcel(DataGridView Indt, string filename = "export", string sheetname = "Sheet1", bool auto_open = false)
        {
            try
            {

                using (ExcelPackage excelPkg = new ExcelPackage())
                {
                    // 1. Setting Excel Workbook Properties
                    excelPkg.Workbook.Properties.Author = "Create by Apche Bonus Calculation System";
                    excelPkg.Workbook.Properties.Title = "Bonus Calculation System";

                    // Creating Excel Worksheet
                    ExcelWorksheet oSheet = CreateSheet(excelPkg, sheetname,Indt.DefaultCellStyle.Font.Size, Indt.DefaultCellStyle.Font.Name);

                    // Sample DataTable
                    //  DataTable dt =  CreateDataTable();                    
                    // 2. Merge Excel Columns: Merging cells and create a center heading for our table
                    //   oSheet.Cells[1, 1].Value = "Danh sach may tinh kiem tra";
                    //   oSheet.Cells[1, 1, 1, dt.Columns.Count].Merge = true;
                    // Setting Font and Alignment for Header
                    //      oSheet.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
                    //     oSheet.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    int rowIndex = 1;

                    // 3. Setting Excel Cell Backgournd Color during Header Creation
                    // 4. Setting Excel Cell Border during Header Creation
                    // Creating Header                  
                    CreateHeader(oSheet, ref rowIndex,Indt);                    
                    // Putting Data into Cells
                    CreateData(oSheet, ref rowIndex, Indt, Indt.DefaultCellStyle.Font.Size);

                    // 5. Setting Excel Formula during Footer Creation
                    // Creating Footer
                    //     CreateFooter(oSheet, ref rowIndex, dt);

                    // 6. Add Comments in Excel Cell
                    // AddComment(oSheet, 5, 5, "Sample Comment", "Debopam Pal");

                    // 7. Add Image in Excel Sheet
                    //   string imagePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.StartupPath)), "debopam.jpg");
                    //  AddImage(oSheet, 1, 10, imagePath);

                    // 8. Add Custom Objects in Excel Sheet
                    //  AddCustomObject(oSheet, 7, 10, eShapeStyle.Ellipse, "Text inside Ellipse");
                    // 9. Set Border all cell
                    FormatBorder(oSheet, 1, 1, oSheet.Dimension.End.Row, oSheet.Dimension.End.Column);                  
                    // Writting bytes by bytes in Excel File                  
                    var saveFileDialoge = new SaveFileDialog();
                    saveFileDialoge.FileName = filename;
                    saveFileDialoge.DefaultExt = ".xlsx";
                    if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            Byte[] content = excelPkg.GetAsByteArray();
                            string path_fileName = saveFileDialoge.FileName;
                            File.WriteAllBytes(path_fileName, content);
                            try
                            {
                                // Openning the created excel file using MS Excel Application
                                ProcessStartInfo pi = new ProcessStartInfo(path_fileName);
                                Process.Start(pi);
                            }
                            catch { }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                }

            }
            catch { }

        }
        public static ExcelWorksheet CreateSheet(ExcelPackage excelPkg, string sheetName,float fontsize=10f,string fontname= "Arial")
        {
            ExcelWorksheet oSheet = excelPkg.Workbook.Worksheets.Add(sheetName);
            // Setting default font for whole sheet
            oSheet.Cells.Style.Font.Name = fontname;
            // Setting font size for whole sheet
            oSheet.Cells.Style.Font.Size = fontsize;
            return oSheet;
        }
        /// <summary>
        /// Creating formatted header of excel sheet
        /// </summary>
        /// <param name="oSheet">The ExcelWorksheet object</param>
        /// <param name="rowIndex">The row number where the header will put</param>
        /// <param name="dt">The DataTable object from where header values will come</param>
        public static void CreateHeader(ExcelWorksheet oSheet, ref int rowIndex, DataTable dt, bool col_is_caption)
        {
            int colIndex = 1;
            foreach (DataColumn dc in dt.Columns)
            {
                var cell = oSheet.Cells[rowIndex, colIndex];
            
                // Setting the background color of header cells to Gray
                //   var fill = cell.Style.Fill;
                //   fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //   fill.BackgroundColor.SetColor(Color.Gray);

                // Setting top/left, right/bottom border of header cells
                //  var border = cell.Style.Border;
                //   border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                if (col_is_caption)
                {
                    // Setting value in cell is caption
                    cell.Value = dc.Caption;
                }
                else
                    cell.Value = dc.ColumnName;

                colIndex++;
            }
        }
        public static void CreateHeader(ExcelWorksheet oSheet, ref int rowIndex, DataGridView dgv)
        {
            int colIndex = 1;         
            for (int i=0;i< dgv.Columns.Count;i++)
            {
                if (dgv.Columns[i].Visible)
                {
                    var cell = oSheet.Cells[rowIndex, colIndex];
                    cell.Style.Font.Size = dgv.ColumnHeadersDefaultCellStyle.Font.Size;
                    oSheet.Column(colIndex).Width = dgv.Columns[i].Width / 6.5f;                   
                    cell.Value = dgv.Columns[i].HeaderText;
                    FormatHeader(oSheet, rowIndex, colIndex, dgv.ColumnHeadersDefaultCellStyle.Font.Size);
                    colIndex++;
                }
            }
        }
        /// <summary>
        /// Putting Data into Excel Cells
        /// </summary>
        /// <param name="oSheet">The ExcelWorksheet object</param>
        /// <param name="rowIndex">The row number from where data will put</param>
        /// <param name="dt">The DataTable object from where data will come</param>
        public static void CreateData(ExcelWorksheet oSheet, ref int rowIndex, DataTable dt,float fontsize=10f)
        {

            int colIndex = 0;
            rowIndex = 1;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    colIndex = 1;
                    rowIndex++;

                    foreach (DataColumn dc in dt.Columns)
                    {

                        var cell = oSheet.Cells[rowIndex, colIndex];
                        cell.Style.Font.Size = fontsize;
                        // Setting value in the cell
                        cell.Value = dr[dc.ColumnName];

                        // Setting border of the cell
                        //   var border = cell.Style.Border;
                        //    border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        colIndex++;
                    }
                }
            }
            catch { }
        }
        public static void CreateData(ExcelWorksheet oSheet, ref int rowIndex, DataGridView dgv, float fontsize = 10f)
        {
            int colIndex = 0;
            rowIndex = 1;
            try
            {
                for (int i=0;i<dgv.Rows.Count;i++)
                {
                    colIndex = 1;
                    rowIndex++;

                    for (int j=0;j< dgv.Columns.Count;j++)
                    {
                        if (dgv.Columns[j].Visible)
                        {
                            var cell = oSheet.Cells[rowIndex, colIndex];
                            cell.Style.Font.Size = fontsize;
                            if (dgv.Rows[i].Cells[j].Style.BackColor.Name != "0")
                            {
                                var fill = cell.Style.Fill;
                                fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(dgv.Rows[i].Cells[j].Style.BackColor);
                            }
                            // Setting value in the cell
                            if (dgv.Columns[j].GetType().Name == "DataGridViewComboBoxColumn")
                                cell.Value = dgv.Rows[i].Cells[j].FormattedValue;
                            else
                                cell.Value = dgv.Rows[i].Cells[j].Value;
                            colIndex++;
                        }
                    }
                }
            }
            catch(Exception err) { }
        }
        /// <summary>
        /// Creating formatted footer in the excel sheet
        /// </summary>
        /// <param name="oSheet">The ExcelWorksheet object</param>
        /// <param name="rowIndex">The row number where the footer will put</param>
        /// <param name="dt">The DataTable object from where footer values will come</param>
        public static void CreateFooter(ExcelWorksheet oSheet, ref int rowIndex, DataTable dt)
        {
            int colIndex = 0;
            // Creating Formula in Footer
            foreach (DataColumn dc in dt.Columns)
            {
                colIndex++;
                var cell = oSheet.Cells[rowIndex, colIndex];

                // Setting Sum Formula for each cell
                // Usage: Sum(From_Addres:To_Address)
                // e.g. - Sum(A3:A6) -> Sums the value of Column 'A' From Row 3 to Row 6
                cell.Formula = "Sum(" + oSheet.Cells[3, colIndex].Address + ":" + oSheet.Cells[rowIndex - 1, colIndex].Address + ")";

                // Setting Background Fill color to Gray
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            }
        }

        /// <summary>
        /// Adding custom comment in specified cell of specified excel sheet
        /// </summary>
        /// <param name="oSheet">The ExcelWorksheet object</param>
        /// <param name="rowIndex">The row number of the cell where comment will put</param>
        /// <param name="colIndex">The column number of the cell where comment will put</param>
        /// <param name="comment">The comment text</param>
        /// <param name="author">The author name</param>
        public static void AddComment(ExcelWorksheet oSheet, int rowIndex, int colIndex, string comment, string author)
        {
            // Adding a comment to a Cell
            oSheet.Cells[rowIndex, colIndex].AddComment(comment, author);
        }

        /// <summary>
        /// Adding custom image in spcified cell of specified excel sheet
        /// </summary>
        /// <param name="oSheet">The ExcelWorksheet object</param>
        /// <param name="rowIndex">The row number of the cell where the image will put</param>
        /// <param name="colIndex">The column number of the cell where the image will put</param>
        /// <param name="imagePath">The path of the image file</param>
        public static void AddImage(ExcelWorksheet oSheet, int rowIndex, int colIndex, string imagePath)
        {
            Bitmap image = new Bitmap(imagePath);
            ExcelPicture excelImage = null;
            if (image != null)
            {
                excelImage = oSheet.Drawings.AddPicture("Debopam Pal", image);
                excelImage.From.Column = colIndex;
                excelImage.From.Row = rowIndex;
                excelImage.SetSize(100, 100);
                // 2x2 px space for better alignment
                excelImage.From.ColumnOff = Pixel2MTU(2);
                excelImage.From.RowOff = Pixel2MTU(2);
            }
        }

        /// <summary>
        /// Adding custom shape or object in specifed cell of specified excel sheet
        /// </summary>
        /// <param name="oSheet">The ExcelWorksheet object</param>
        /// <param name="rowIndex">The row number of the cell where the object will put</param>
        /// <param name="colIndex">The column number of the cell where the object will put</param>
        /// <param name="shapeStyle">The style of the shape of the object</param>
        /// <param name="text">Text inside the object</param>
        public static void AddCustomObject(ExcelWorksheet oSheet, int rowIndex, int colIndex, eShapeStyle shapeStyle, string text)
        {
            ExcelShape excelShape = oSheet.Drawings.AddShape("Custom Object", shapeStyle);
            excelShape.From.Column = colIndex;
            excelShape.From.Row = rowIndex;
            excelShape.SetSize(100, 100);
            // 5x5 px space for better alignment
            excelShape.From.RowOff = Pixel2MTU(5);
            excelShape.From.ColumnOff = Pixel2MTU(5);
            // Adding text into the shape
            excelShape.RichText.Add(text);
        }

        public static int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public static DateTime FromExcelSerialDate(int excelDate)
        {
            if (excelDate < 1)
                throw new ArgumentException("Excel dates cannot be smaller than 0.");

            var dateOfReference = new DateTime(1900, 1, 1);

            if (excelDate > 60d)
                excelDate = excelDate - 2;
            else
                excelDate = excelDate - 1;
            return dateOfReference.AddDays(excelDate);
        }
        public static object GetSetDataType(ExcelWorksheet oSheet, int rowIndex, int colIndex)
        {
            try
            {
                if (oSheet.Cells[rowIndex, colIndex].Value != null)
                {                              
                    if (AllDateFormat.Contains(oSheet.Cells[rowIndex, colIndex].Style.Numberformat.Format.ToLower()))
                    {
                        try
                        {
                            Int64 serialdate;
                            if (Int64.TryParse(oSheet.Cells[rowIndex, colIndex].Value.ToString(), out serialdate))
                            {
                                return FromExcelSerialDate(int.Parse(oSheet.Cells[rowIndex, colIndex].Value.ToString())).ToString(Parameters.dateFormat);
                            }
                            else
                            {                               
                                DateTime parsedDate;
                                if (DateTime.TryParse(oSheet.Cells[rowIndex, colIndex].Value.ToString(), out parsedDate))
                                    return parsedDate.ToString(Parameters.dateFormat);
                            }

                        }
                        catch {

                            return oSheet.Cells[rowIndex, colIndex].Value;
                        }

                    }
                    if (oSheet.Cells[rowIndex, colIndex].Style.Numberformat.Format.Contains("hh:mm") && oSheet.Cells[rowIndex, colIndex].Value.ToString() != "0")
                    {
                        try
                        {
                            DateTime time = new DateTime().AddHours(oSheet.Cells[rowIndex, colIndex].Value.ToString().ToDouble() * 24);
                            return time.ToString("hh:mm:ss tt");

                        }
                        catch { }

                    }
                }
                else
                    return " ";
            }
            catch { }
            return oSheet.Cells[rowIndex, colIndex].Value;
        }
        public static void FormatHeader(ExcelWorksheet oSheet, int rowIndex, int colIndex,float fontsize=12f)
        {
            var cell = oSheet.Cells[rowIndex, colIndex];
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size =fontsize;
            //oSheet.Row(rowIndex).Height = 50;
            cell.Style.WrapText = true;
            // Setting the background color of header cells to Gray
            var fill = cell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);

            // Setting top/left, right/bottom border of header cells
            var border = cell.Style.Border;
            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        }
        public static void FormatHeader(ExcelWorksheet oSheet, int startrowIndex, int startcolIndex, int endrowIndex, int endcolIndex,float fontsize=12f)
        {
            var cell = oSheet.Cells[startrowIndex, startcolIndex, endrowIndex, endcolIndex];
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = fontsize;
            //  oSheet.Row(startrowIndex).Height = 50;
            cell.Style.WrapText = true;
            // Setting the background color of header cells to Gray
            var fill = cell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);

            // Setting top/left, right/bottom border of header cells
            var border = cell.Style.Border;
            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }
        public static void FormatFillColor(ExcelWorksheet oSheet, int startrowIndex, int startcolIndex, int endrowIndex, int endcolIndex,Color c)
        {
            var cell = oSheet.Cells[startrowIndex, startcolIndex, endrowIndex, endcolIndex];
            var fill = cell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(c);
        }
        public static void FormatHeaderEmp(ExcelWorksheet oSheet, int startrowIndex, int startcolIndex, int endrowIndex, int endcolIndex,string title)
        {
            oSheet.Cells[startrowIndex, startcolIndex, endrowIndex, endcolIndex].Merge = true;
            oSheet.Cells[startrowIndex, startcolIndex].Value = title;
            var cell = oSheet.Cells[startrowIndex, startcolIndex, endrowIndex, endcolIndex];
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 15;
            oSheet.Row(startrowIndex).Height = 50;
            cell.Style.WrapText = true;
            // Setting the background color of header cells to Gray
            var fill = cell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);

            // Setting top/left, right/bottom border of header cells
            var border = cell.Style.Border;
            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        }
        public static void FormatHeaderEmpTitle(ExcelWorksheet oSheet, int startrowIndex, int startcolIndex, int endrowIndex, int endcolIndex)
        {
            var cell = oSheet.Cells[startrowIndex, startcolIndex, endrowIndex, endcolIndex];
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            oSheet.Row(startrowIndex).Height = 50;
           
            cell.Style.WrapText = true;
            // Setting the background color of header cells to Gray
            var fill = cell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);

            // Setting top/left, right/bottom border of header cells
            var border = cell.Style.Border;
            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        }
        public static void FormatTitle(ExcelWorksheet oSheet, int rowIndex, int colIndex)
        {
            var cell = oSheet.Cells[rowIndex, colIndex];
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 20;
            oSheet.Row(rowIndex).Height = 50;
            cell.Style.WrapText = true;
            // Setting the background color of header cells to Gray
            var fill = cell.Style.Fill;

            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.Gray);

            // Setting top/left, right/bottom border of header cells
            //var border = cell.Style.Border;
            //border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }
        public static void FormatBorder(ExcelWorksheet oSheet, int startrowIndex, int startcolIndex, int endrowIndex, int endcolIndex)
        {    
            var cell = oSheet.Cells[startrowIndex, startcolIndex, endrowIndex, endcolIndex];    
            // Setting top/left, right/bottom border of header cells
            var border = cell.Style.Border;
            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }
        public static void ExportDept(DataTable dt, DateTime fromdate, DateTime todate)
        {

            ExcelPackage excelPkg = new ExcelPackage();
            // 1. Setting Excel Workbook Properties
            excelPkg.Workbook.Properties.Author = "Create by Tran Tuyen Giap";
            excelPkg.Workbook.Properties.Title = "maytinh";
            ExcelWorksheet oSheet = CreateSheet(excelPkg, "Sheet1");
            Dictionary<DateTime, int> headerIndex = new Dictionary<DateTime, int>();
            string title = "BIỀU THỐNG KÊ CHI TIẾT TIỀN THƯỞNG SẢN LƯỢNG THÁNG {0}/{1} \n {2}/{3}月份生產獎金統計表";
            string[] hearderlist = {"STT\n序號","Mã bộ phận\n部門代號",
                                     "Tên bộ phận(VN) \n部門名稱","Tên bộ phận(EL)\n部門名稱 "};
            title = String.Format(title, fromdate.Year, fromdate.Month, fromdate.Year, fromdate.Month);

            //add header
            for (int i = 0; i < hearderlist.Length; i++)
            {
                oSheet.Cells[2, i + 1].Value = hearderlist[i];
                if (i == 3)
                {
                    oSheet.Column(i + 1).Width = hearderlist[i].Length + 10;
                }
                else
                {
                    oSheet.Column(i + 1).Width = hearderlist[i].Length + 4;
                }
                oSheet.Cells[2, i + 1, 3, i + 1].Merge = true;
                ExcelCommand.FormatHeader(oSheet, 2, i + 1, 3, i + 1);
            }
            int index = 1;
            int countcol = hearderlist.Length;
            //add Month
            for (DateTime dt2 = fromdate; dt2 <= todate; dt2 = dt2.AddMonths(1))
            {
                oSheet.Cells[3, index + countcol].Value = dt2;
                oSheet.Cells[3, index + countcol].Style.Numberformat.Format = "M";
                oSheet.Column(index + countcol).Width =20;
                ExcelCommand.FormatHeader(oSheet, 3, index + countcol);
                headerIndex.Add(dt2, index + countcol);
                index++;
            }
            oSheet.Cells[2, countcol + 1].Value = "Tháng/月份";
            ExcelCommand.FormatHeader(oSheet, 2, countcol + 1, 2, headerIndex.Count + countcol);
            oSheet.Cells[2, countcol + 1, 2, headerIndex.Count + countcol].Merge = true;
            oSheet.Cells[2, headerIndex.Count + countcol + 1].Value = "Tổng\n合計";
            oSheet.Cells[2, headerIndex.Count + countcol + 1, 3, headerIndex.Count + countcol + 1].Merge = true;
            ExcelCommand.FormatHeader(oSheet, 2, headerIndex.Count + countcol + 1);

            //  //magin title
            oSheet.Cells[1, 1].Value = title;
            oSheet.Cells[1, 1, 1, headerIndex.Count + countcol + 1].Merge = true;
            ExcelCommand.FormatTitle(oSheet, 1, 1);

            try
            {
                int stt = 1;
                int currentindex = -1;
                int startrowindex = 4;
                var groups = dt.AsEnumerable().GroupBy(x => x.Field<string>("DEPARTMENT_CODE"));
                int startdateindex = -1;
                int enddateindex = -1;
                foreach (var group in groups)
                {
                    headerIndex.TryGetValue(fromdate, out startdateindex);
                    headerIndex.TryGetValue(todate, out enddateindex);
                    var cell = oSheet.Cells[startrowindex, startdateindex, startrowindex, enddateindex];
                    var fill = cell.Style.Fill;
                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.YellowGreen);
                    foreach (DataRow subrw in group)
                    {
                        currentindex = -1;
                        headerIndex.TryGetValue(new DateTime((int)subrw["g_year"], (int)subrw["g_month"], 1), out currentindex);
                        oSheet.Cells[startrowindex, 1].Value = stt;
                        oSheet.Cells[startrowindex, 2].Value = subrw["DEPARTMENT_CODE"];
                        oSheet.Cells[startrowindex, 3].Value = "";
                        oSheet.Cells[startrowindex, currentindex].Style.Numberformat.Format = String.Format("{0:#,##0}", Double.Parse(subrw["G_MONEY"].ToString()));
                        var cell1 = oSheet.Cells[startrowindex, currentindex];
                        var fill1 = cell1.Style.Fill;
                        fill1.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        fill1.BackgroundColor.SetColor(Color.White);
                    }
                    oSheet.Cells[startrowindex, index + countcol].Formula = "=SUM(RC[-" + (index - 1) + "]:RC[-1])";
                    stt++;
                    startrowindex++;
                }
                oSheet.Cells[startrowindex, 2].Value = "Tổng合計";
                for (int i = 0; i < index; i++)
                {
                    oSheet.Cells[startrowindex, hearderlist.Length + i + 1].Formula = "=SUM(R[-" + (stt - 1) + "]C: R[-1]C)";
                }

            }

            catch (Exception ex)
            {

            }
            if (dt.Rows.Count > 0)
            {
                var saveFileDialoge = new SaveFileDialog();
                saveFileDialoge.FileName = "Report";
                saveFileDialoge.DefaultExt = ".xlsx";
                if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Byte[] content = excelPkg.GetAsByteArray();

                        string fileName = saveFileDialoge.FileName;// "Export_tu_dong.xlsx";
                                                                   // string filePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.StartupPath)), fileName);
                                                                   //  File.WriteAllBytes(fileName, content);
                        File.WriteAllBytes(fileName, content);
                        try
                        {
                            // Openning the created excel file using MS Excel Application
                            ProcessStartInfo pi = new ProcessStartInfo(fileName);
                            Process.Start(pi);
                        }
                        catch { }

                    }
                    catch (Exception ex)
                    {

                        //MessageHelper.ShowErr(this, ex.Message);
                    }


                }

            }
        }

    }
}
