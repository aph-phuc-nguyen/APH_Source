using AutocompleteMenuNS;
using DayActualManually;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayActualManually
{
    public partial class DayActualManuallyForm : MaterialForm
    {
        public Boolean isTitle = false;
        IList<object[]> data = null;
        private ExcelProcessor _currentExcelProcessor = null;

        public DayActualManuallyForm()
        {
           
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            // label3.Text = Program.client.Language;
            this.Font = new Font("宋体", (Screen.PrimaryScreen.Bounds.Height / 100f), FontStyle.Regular, GraphicsUnit.Point, 134);
            tabControl1.Font = new Font("宋体", (Screen.PrimaryScreen.Bounds.Height / 90f), FontStyle.Regular, GraphicsUnit.Point, 134);
            panel1.Font = new Font("宋体", (Screen.PrimaryScreen.Bounds.Height / 90f), FontStyle.Regular, GraphicsUnit.Point, 134);
            panel2.Font = new Font("宋体", (Screen.PrimaryScreen.Bounds.Height / 90f), FontStyle.Regular, GraphicsUnit.Point, 134);
            tabPage1.Font = new Font("宋体", (Screen.PrimaryScreen.Bounds.Height / 90f), FontStyle.Regular, GraphicsUnit.Point, 134);
            tabPage2.Font = new Font("宋体", (Screen.PrimaryScreen.Bounds.Height / 90f), FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.RowTemplate.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height / 30);
            string sizedgv = "ColDept:0.07;ColDay:0.09;ColQty:0.11;colart_no:0.13;ColRoutNo:0.13;ColInsertDate:0.15;ColUser:0.12;colNote:0.17";
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.RowTemplate.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height / 30);
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            Parameters.CustomeSizeDataGridView(dataGridView3, sizedgv);
        }

        private void DayActualManuallyForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadSeDept();
                GetComboBoxUI();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
          
            this.dataGridView2.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
            this.dataGridView3.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
        }

        private void butFile_Click(object sender, EventArgs e)
        {
            isTitle = true;
            string errs = "";
            this.dataGridView2.AutoGenerateColumns = false;
            if (dataGridView2 != null)
            {
                this.dataGridView2.Columns.Clear();
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "EXCEL|*.xls*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (dataGridView1 != null)
                {
                    data = new List<object[]>();
                    this.dataGridView1.Rows.Clear();
                }
                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        this.dataGridView1.Rows.Add(filename);
                        dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        this.GetExcelData(Path.GetFullPath(filename));
                    }
                    catch (Exception ex)
                    {
                       MessageBox.Show(this, ex.Message);
                    }
                }
                dataGridView2.AllowUserToAddRows = false;
                if (data != null && data.Count > 0)
                {
                    int colNum = data[0].Length;
                    for (int i = 0; i < colNum; i++)
                    {
                        DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
                        if (i > 0 && i < colNum)
                        {
                            try
                            {
                                //data[0][i] = Convert.ToDateTime(data[0][i].ToString()).ToShortDateString();
                                data[0][i] = Convert.ToDateTime(data[0][i].ToString()).ToString(Parameters.dateFormat);
                                acCode.Width = 68;
                            }
                            catch(Exception ex)
                            {
                                errs += "column:" + (i+1) + "," + ex.Message + "\n";
                            }
                        }
                        acCode.Name = data[0][i].ToString();
                        acCode.HeaderText = data[0][i].ToString();
                        dataGridView2.Columns.Add(acCode);
                    }
                    dataGridView2.Rows.Add(data[1]);
                    DataTable artlist = QueryArtList();
                    string row = Parameters.languageTranslation("Row:", this.Name);
                    string col = Parameters.languageTranslation("Col:", this.Name);
                    string deperr = Parameters.languageTranslation(", Department not exist");
                    for (int i = 2; i < data.Count; i++)
                    {
                        dataGridView2.Rows.Add(data[i]);
                        dataGridView2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        for (int j = 1; j < colNum; j++)
                        {
                            if (data[i][j] != null && data[i][j].ToString().Trim() != "")
                            {
                                if (data[i][j].ToString().Trim().Contains(","))
                                {
                                    string[] cut = data[i][j].ToString().Trim().Split(',');
                                    int num = 0;
                                    if (int.TryParse(cut[0], out num)==false)// || FindText(artlist, "ART_NO", cut[1], "ART_NO") == "")                                   
                                    {
                                        string msg =Parameters.languageTranslation("The model or quantity is not in the correct format", this.Name);
                                        errs += row + i + ","+col + (j + 1) + ", " + msg + "\n";
                                    }
                                }
                                else
                                {
                                    string msg = Parameters.languageTranslation("The model or quantity is not in the correct format", this.Name);
                                    errs += row + i + "," + col + (j + 1) + ", " + msg + "\n";
                                }                               
                            }
                            else
                            {
                                data[i][j] = "";
                            }
                        }
                    }
                }
            }
            if (errs != "")
            {
                MessageBox.Show(errs);
            }
        }

        //导入
        private void butImport_Click(object sender, EventArgs e)
        {
            isTitle = true;
            //btnFolder.Enabled = false;
            butFile.Enabled = false;
            butImport.Enabled = false;
            if (this.dataGridView2.Rows.Count >= 2)
            {
                try
                {
                    update_db();
                }
                catch (Exception ex)
                {                   
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {               
                string msg = Parameters.languageTranslation("Empty data!", this.Name);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
            butFile.Enabled = true;
            butImport.Enabled = true;
        }
        public static string FindText(DataTable indt, string findcolname, string findtext, string getcolname)
        {
            var result = indt.AsEnumerable().Where(myRow => myRow.Field<string>(findcolname).ToLower() == findtext.ToLower()).FirstOrDefault();
            try
            {
                if (result != null)
                    return result[getcolname].ToString();
            }
            catch { }
            return "";
        }
        private void update_db()
        {
            string errs = "";
            string row = Parameters.languageTranslation("Row:", this.Name);
            string col = Parameters.languageTranslation("Col:", this.Name);
            string deperr = Parameters.languageTranslation(", Department not exist");
            DataTable tab = new DataTable();
            DataTable artlist = QueryArtList();
            object[] cols = data[0];
            for (int i = 0; i < cols.Length; i++)
            {
                tab.Columns.Add(cols[i].ToString());
            }
            for (int i = 2; i < data.Count; i++)
            {
                DataRow dr = tab.NewRow();
                try
                {
                    if (!ValiDept(data[i][0].ToString().Trim().ToUpper()))
                    {
                        errs += row + i + deperr + "\n";
                    }
                    else
                    {
                        dr[0] = data[i][0].ToString().Trim().ToUpper();
                    }
                }
                catch (Exception)
                {
                    errs += row+ i + deperr + "\n";
                }
                for (int j = 1; j < cols.Length; j++)
                {
                    if (data[i][j] != null && data[i][j].ToString().Trim() != "")
                    {
                        try
                        {
                            if (data[i][j].ToString().Trim().Contains(","))
                            {
                                string[] cut = data[i][j].ToString().Trim().Split(',');
                                int num = 0;
                             //   if (int.TryParse(cut[0], out num) && FindText(artlist, "ART_NO", cut[1], "ART_NO")!="")
                                    dr[j] = data[i][j].ToString().Trim();
                                //else
                                //{
                                //    string msg = Parameters.languageTranslation("The model or quantity is not in the correct format", this.Name);
                                //    errs += row + i + ","+col + (j + 1) + ", " + msg + "\n";
                                //}
                            }
                            else
                            {
                                string msg = Parameters.languageTranslation("The model or quantity is not in the correct format", this.Name);
                                errs += row + i + "," + col + (j + 1) + ", " + msg + "\n";
                            }    
                           
                        }
                        catch (Exception ex)
                        {
                            errs += row + i + ","+col + (j + 1) + "," + ex.Message + "\n";
                        }
                    }
                    else
                    {
                        dr[j] = "";
                    }
                }
                tab.Rows.Add(dr);
            }
            if (errs == "")
            {
                try
                {
                    UpLoad(tab);
                }
                catch (Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {
                MessageBox.Show(errs);
            }
        }

        private void butquery_Click(object sender, EventArgs e)
        {
        //    string km = Parameters.GetSizeDataGridView(dataGridView3);
            try
            {
                this.dataGridView3.AutoGenerateColumns = false;
                this.dataGridView3.DataSource = null;
                QueryActual();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void UpLoad(DataTable tab)
        {
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("data", tab);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayActualManuallyServer", "UpLoad",
                            Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string msg = Parameters.languageTranslation("Imported successfully!", this.Name);
                SJeMES_Control_Library.MessageHelper.ShowSuccess(this, msg);
            }
            else
            {
                string msg = Parameters.languageTranslation("Import failed!", this.Name);
                string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
            }
        }

        private bool ValiDept(string d_dept)
        {
            bool isOk = false;
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("vDDept", d_dept);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayActualManuallyServer", "ValiDept",
                            Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                isOk = bool.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return isOk;
        }

        private void QueryActual()
        {
            string d_dept = textBox1.Text.ToString().Trim();
            string work_day = dateTimePicker1.Text.ToString();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vWorkDay", work_day);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayActualManuallyServer", "QueryActual", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dataGridView3.Columns["ColQty"].DefaultCellStyle.Format = "0";
                dataGridView3.DataSource = dt.DefaultView;
                dataGridView3.Update();
                dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private DataTable QueryArtList()
        {          
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("Art_No", "");           
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayActualManuallyServer", "GetListArt_No", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
               return SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);               
            }
            else
            {
                return null;
            }
        }

        //获取部门列表
        private DataTable GetAllDepts()
        {
            DataTable dt = new DataTable();
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetAllDepts", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(string.Empty));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            }
            else
            {
                string msg = Parameters.languageTranslation("Server connection failed!", this.Name);
                string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
            }
            return dt;
        }

        //加载部门列表
        private void LoadSeDept()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(350, 350);
            var columnWidth = new int[] { 50, 250 };
            DataTable dt = GetAllDepts();
            int n = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt.Rows[i]["DEPARTMENT_CODE"].ToString() + " " + dt.Rows[i]["DEPARTMENT_NAME"].ToString() }, dt.Rows[i]["DEPARTMENT_CODE"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridView3.CurrentRow.Index;
            if (index > -1 && dataGridView3.Rows[index].Cells["colDEPT"].Value != null)
            {
                string dept = dataGridView3.Rows[index].Cells["colDEPT"].Value == null ? "" : dataGridView3.Rows[index].Cells["colDEPT"].Value.ToString();
                string workDate = dataGridView3.Rows[index].Cells["colDAY"].Value == null ? "" : dataGridView3.Rows[index].Cells["colDAY"].Value.ToString();
                int qty = dataGridView3.Rows[index].Cells["colQTY"].Value == null ? 0 : Convert.ToInt32(decimal.Parse(dataGridView3.Rows[index].Cells["colQTY"].Value.ToString()));
                string art_no =dataGridView3.Rows[index].Cells["colart_no"].Value == null ? "" : dataGridView3.Rows[index].Cells["colart_no"].Value.ToString();
                string workQty = qty.ToString();
                string note = dataGridView3.Rows[index].Cells["colNOTE"].Value == null ? "" : dataGridView3.Rows[index].Cells["colNOTE"].Value.ToString();
                DayActualManuallyModifyForm frm = new DayActualManuallyModifyForm(dept, workDate, workQty, note,art_no);
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    this.butquery.PerformClick();//重新查询 
                }
            }
        }

        private void GetComboBoxUI()
        {
            List<RoutEntry> routEntries = new List<RoutEntry> { };
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("enmu_type", "routEntries");

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ComboBoxUIServer", "GetComboBoxUI", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    routEntries.Add(new RoutEntry() { Code = dtJson.Rows[i]["ENUM_CODE"].ToString(), Name = dtJson.Rows[i]["ENUM_VALUE"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

            this.ColRoutNo.DataSource = routEntries;
            this.ColRoutNo.DisplayMember = "Name";
            this.ColRoutNo.ValueMember = "Code";
        }

        private void GetExcelData(string fileName)
        {
            try
            {
                this._currentExcelProcessor = new ExcelProcessor(fileName);
                IList<object[]> list = this._currentExcelProcessor.GetSheetData(0);
                if (data != null && data.Count > 0)
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        data.Add(list[i]);
                    }
                }
                else
                {
                    data = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            StringFormat centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
       
        public class RoutEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }
    }
}
