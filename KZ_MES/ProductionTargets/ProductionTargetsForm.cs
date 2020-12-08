using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AutocompleteMenuNS;
using System.Reflection;
using System.Resources;
using System.Collections;
using MaterialSkin.Controls;
using System.Globalization;

namespace ProductionTargets
{
    public partial class ProductionTargetsForm : MaterialForm
    {

        public Boolean isTitle = false;
        IList<object[]> data = null;
        private ExcelProcessor _currentExcelProcessor = null;
        int checkTime = 0;
        
        public ProductionTargetsForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            var frmtype = this.GetType();    
        }

        private void GetComboBoxUI()
        {
            List<StatusEntry> statusEntries = new List<StatusEntry> { };
            List<InOutEntry> inOutEntries = new List<InOutEntry> { };
            List<RoutEntry> routEntries = new List<RoutEntry> { };
            List<RoutEntry> Column1Entries = new List<RoutEntry> { };
            Dictionary<string, Object> p = new Dictionary<string, object>();  
            p.Add("enmu_type", "InOutStatus");

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ComboBoxUIServer", "GetComboBoxUI", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    inOutEntries.Add(new InOutEntry() { Code = dtJson.Rows[i]["ENUM_CODE"].ToString(), Name = dtJson.Rows[i]["ENUM_VALUE"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

            p.Remove("enmu_type");
            p.Add("enmu_type", "statusEntries");
            string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ComboBoxUIServer", "GetComboBoxUI", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    statusEntries.Add(new StatusEntry() { Code =dtJson.Rows[i]["ENUM_CODE"].ToString(), Name = dtJson.Rows[i]["ENUM_VALUE"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["ErrMsg"].ToString());
            }

            p.Remove("enmu_type");
            p.Add("enmu_type", "Column1Entries");
            string ret3 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ComboBoxUIServer", "GetComboBoxUI", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret3)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret3)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    string str = dtJson.Rows[i]["ENUM_CODE"].ToString();
                    Column1Entries.Add(new RoutEntry() { Code = dtJson.Rows[i]["ENUM_CODE"].ToString(), Name = dtJson.Rows[i]["ENUM_VALUE"].ToString() });
                    
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret3)["ErrMsg"].ToString());
            }
            p.Remove("enmu_type");
            p.Add("enmu_type", "routEntries");
            string ret4 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ComboBoxUIServer", "GetComboBoxUI", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret4)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret4)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    routEntries.Add(new RoutEntry() { Code = dtJson.Rows[i]["ENUM_CODE"].ToString(), Name = dtJson.Rows[i]["ENUM_VALUE"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret4)["ErrMsg"].ToString());
            }

            this.cbWorkDaySizeStatus.DataSource = statusEntries;
            this.cbWorkDaySizeStatus.DisplayMember = "Name";
            this.cbWorkDaySizeStatus.ValueMember = "Code";

            this.comboBoxStatus.DataSource = statusEntries;
            this.comboBoxStatus.DisplayMember = "Name";
            this.comboBoxStatus.ValueMember = "Code";
            
            this.cbStatus.DataSource = statusEntries;
            this.cbStatus.DisplayMember = "Name";
            this.cbStatus.ValueMember = "Code";

            this.comboBox2.DataSource = inOutEntries;
            this.comboBox2.DisplayMember = "Name";
            this.comboBox2.ValueMember = "Code";


            this.INOUT_PZ.DataSource = inOutEntries;
            this.INOUT_PZ.DisplayMember = "Name";
            this.INOUT_PZ.ValueMember = "Code";


            this.comboBox1.DataSource = Column1Entries;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.ValueMember = "Code";


            this.cbWorkDayColumn1.DataSource = Column1Entries;
            this.cbWorkDayColumn1.DisplayMember = "Name";
            this.cbWorkDayColumn1.ValueMember = "Code";

            this.cbWorkDaySizeColumn1.DataSource = Column1Entries;
            this.cbWorkDaySizeColumn1.DisplayMember = "Name";
            this.cbWorkDaySizeColumn1.ValueMember = "Code";


            this.combobox7.DataSource = routEntries;
            this.combobox7.DisplayMember = "Name";
            this.combobox7.ValueMember = "Code";

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            isTitle = true;
            this.dataGridView.AutoGenerateColumns = false;
            if (dataGridView != null)
            {
                this.dataGridView.Columns.Clear();
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
                        //MessageBox.Show(ex.Message, resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //dataGridView2.Name = resources.GetString("ImportData");
                dataGridView.AllowUserToAddRows = false;
                Boolean isTitle = true;
                if (data != null && data.Count > 0)
                {
                    for (int i = 0; i < data[0].Length; i++)
                    {
                        DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
                        acCode.Name = data[0][i].ToString();
                        //acCode.DataPropertyName = "acCode";
                        acCode.HeaderText = data[0][i].ToString();
                        dataGridView.Columns.Add(acCode);
                        if (i <= 3)
                        {
                            acCode.Width = 80;
                        }
                        else
                        {
                            acCode.Width = 43;
                        }
                    }

                    foreach (object[] row in data)
                    {
                        if (!isTitle)
                        {
                            try
                            {
                                //row[3] = Convert.ToDateTime(row[3]?.ToString()).ToShortDateString();
                                row[3] = Convert.ToDateTime(row[3]?.ToString()).ToString(Program.dateFormat);
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show(resources.GetString("DataError"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            dataGridView.Rows.Add(row);
                        }
                        else
                        {
                            isTitle = false;
                        }
                    }
                }
                dataGridView.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
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
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnUpdateWorkDate_Click(object sender, EventArgs e)
        {
            btnOpen.Enabled = false;
            btnAdd.Enabled = false;
            if (this.dataGridView1.Rows.Count >= 1)
            {
                update_db();
            }
            else
            {
                //文件不能为空
                String message1 = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client, "",Program.client.Language);
                //错误
                String message2= SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.client,"", Program.client.Language);
                MessageBox.Show(message1,message2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnOpen.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void update_db()
        {
            string ret = "";
            try
            {
                Boolean isValiDate = true;
                DataTable tab = new DataTable();

                object[] cols = data[0];

                for (int i = 0; i < cols.Length; i++)
                {
                    tab.Columns.Add(cols[i].ToString());
                }

                for (int i = 1; i < data.Count; i++)
                {
                    string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
                    DateTime workDate = DateTime.Parse(data[i][3].ToString());
                    DateTime nowDate = DateTime.Parse(DateTime.Now.ToString(Program.dateFormat));
                    if (workDate < nowDate)
                    {
                        isValiDate = false;
                        break;
                    }

                    DataRow dr = tab.NewRow();
                    for (int j = 0; j < cols.Length; j++)
                    {
                        if (data[i][j] != null)
                        {
                            dr[j] = data[i][j].ToString();
                        }
                        else {
                            dr[j] = "";
                        }
                    }
                    tab.Rows.Add(dr);
                }
                if (isValiDate)
                {
                    Dictionary<string, Object> d = new Dictionary<string, object>();
                    d.Add("data", tab);
                    ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpLoad", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {

                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        if (dtJson.Rows.Count > 0)
                        {
                            string errs = "";
                            for (int i = 0; i < dtJson.Rows.Count; i++)
                            {
                                string msg_d = SJeMES_Framework.Common.UIHelper.UImsg(dtJson.Rows[i][0].ToString(), Program.client, "", Program.client.Language);
                                string er = string.Format(msg_d, dtJson.Rows[i][1], dtJson.Rows[i][2].ToString());
                                errs = errs + er + "\n";
                            }
                            string msg_m = SJeMES_Framework.Common.UIHelper.UImsg("msg-plan-00000", Program.client, "", Program.client.Language);
                            string err_detail = string.Format(msg_m, dtJson.Rows.Count);
                            MessageBox.Show(errs, err_detail, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                        }
                    }
                    else
                    {
                        //string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        //DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        //MessageForm frm=new MessageForm(dtJson);
                        //frm.ShowDialog();
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
                else
                {
                    //资料有误!!
                    String message = SJeMES_Framework.Common.UIHelper.UImsg("err-WorkDay", Program.client, "", Program.client.Language);
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, message);
                }
            }
            catch (Exception ex)
            {
                if (ret != "")
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
                else {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
        }

        //private void update_db()
        //{
        //    string ret = "";
        //    try
        //    {
        //        Boolean isValiDate = true;
        //        isTitle = true;
        //        DataTable tab = new DataTable();
        //        tab.Columns.Add("SE_ID");
        //        tab.Columns.Add("SIZE_no");
        //        tab.Columns.Add("WORK_QTY");
        //        tab.Columns.Add("SUPPLEMENT_QTY");
        //        tab.Columns.Add("D_DEPT");
        //        tab.Columns.Add("WORK_DAY");
        //        tab.Columns.Add("ROUT_NO");
        //        foreach (var o in data)
        //        {
        //            //ORG_ID, D_DEPT, WORK_DAY, SE_ID, SE_SEQ    这个表的主键少了生产制程
        //            //订单号	SIZE	计划生产量	增补数量  生产部门	生产日期	生产制程
        //            if (!isTitle)
        //            {
        //                DateTime workDate = DateTime.Parse(o[5].ToString());
        //                DateTime nowDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        //                if (workDate < nowDate)
        //                {
        //                    isValiDate = false;
        //                    break;
        //                }
        //                DataRow dr = tab.NewRow();
        //                dr[0] = o[0].ToString();
        //                dr[1] = o[1].ToString();
        //                dr[2] = o[2].ToString();
        //                dr[3] = o[3];
        //                dr[4] = o[4].ToString();
        //                dr[5] = o[5].ToString();
        //                dr[6] = o[6];
        //                tab.Rows.Add(dr);
        //            }
        //            else
        //            {
        //                isTitle = false;
        //            }
        //        }
        //        if (isValiDate)
        //        {
        //            Dictionary<string, Object> d = new Dictionary<string, object>();
        //            d.Add("data", tab);
        //            ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpLoad", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
        //            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
        //            {
        //                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
        //            }
        //            else
        //            {
        //                //string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
        //                //DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
        //                //MessageForm frm=new MessageForm(dtJson);
        //                //frm.ShowDialog();
        //                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
        //            }
        //        }
        //        else
        //        {
        //            //资料有误!!
        //            String message = SJeMES_Framework.Common.UIHelper.UImsg("err-00002", Program.client,"" ,Program.client.Language);
        //            SJeMES_Control_Library.MessageHelper.ShowErr(this, message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
        //    }
        //}

        private void btn_query_Click(object sender, EventArgs e)
        {
            checkTime = 0;
            this.dataGridView2.DataSource = null;
            this.dataGridView2.AutoGenerateColumns = false;
            try
            {
                GetData();
                dataGridView2_SelectionChanged(sender, e);
                dataGridView2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

         private void GetData()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", textSeId.Text.ToString());
            p.Add("vDDept", textDDept.Text.ToString());
            p.Add("vArtNo", textArt.Text.ToString());
            p.Add("vWrokDay", dtpWorkDay.Text.ToString());
            p.Add("vEndWrokDay", dateTimePicker1.Text.ToString());
            p.Add("vColumn1", comboBox1.SelectedValue.ToString());
            p.Add("vStatus", comboBoxStatus.SelectedValue.ToString());    
            p.Add("vInOut", comboBox2.SelectedValue.ToString());
            p.Add("title",this.Text);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "Query", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dtJson.Columns.Add("CheckBox");
                foreach (DataRow row in dtJson.Rows)
                {
                    row["CheckBox"] = false;
                }
                dataGridView2.DataSource = dtJson.DefaultView;
                dataGridView2.Update();
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView3.DataSource = null;
            this.dataGridView3.AutoGenerateColumns = false;
            string seId = "";
            string workDate = "";
            string deptNo = "";
            string col1 = "";
            string inOut_PZ = "";
            if (dataGridView2.CurrentRow != null && dataGridView2.CurrentRow.Index > -1)
            {      
                int index = dataGridView2.CurrentRow.Index;
                seId = dataGridView2.Rows[index].Cells[2].Value == null ? "" : dataGridView2.Rows[index].Cells[2].Value.ToString();
                workDate = dataGridView2.Rows[index].Cells["Column4"].Value == null ? "" : dataGridView2.Rows[index].Cells["Column4"].Value.ToString();
                deptNo = dataGridView2.Rows[index].Cells[5].Value == null ? "" : dataGridView2.Rows[index].Cells[5].Value.ToString();
                col1 = dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value == null ? "" : dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value.ToString();
                inOut_PZ = dataGridView2.Rows[index].Cells["INOUT_PZ"].Value == null ? "" : dataGridView2.Rows[index].Cells["INOUT_PZ"].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vSeId", seId);
                p.Add("vDDept", deptNo);
                p.Add("vWrokDay", workDate);
                p.Add("vColumn1", col1);
                p.Add("vInOut", inOut_PZ);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "QuerySize", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView3.DataSource = dtJson.DefaultView;
                    dataGridView3.Update();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
                dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
        }
        public class StatusEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }
        public class RoutEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        public class InOutEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        private void ProductionTargetsForm_Load(object sender, EventArgs e)
        {
            LoadSeDept();
            //下拉框的多语言
            GetComboBoxUI();
            //多语言更新
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            this.dataGridView2.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
            this.dataGridView3.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
            //权限
            String RET=SJeMES_Framework.Common.UIHelper.UIVisiable(this,"日生产计划导入",Program.client);

        }

        private void LoadSeDept()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(350, 350);
            var columnWidth = new int[] { 50, 250 };
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "LoadSeDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject("noParam"));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i=1;i<= dtJson.Rows.Count;i++)
                {
                    autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { i + "", dtJson.Rows[i - 1]["DEPARTMENT_CODE"] + " " + dtJson.Rows[i - 1]["DEPARTMENT_NAME"] }, dtJson.Rows[i - 1]["DEPARTMENT_CODE"].ToString()) { ColumnWidth = columnWidth, ImageIndex = i });
                }
            }

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            string SureClose = SJeMES_Framework.Common.UIHelper.UImsg("SureClose", Program.client,"", Program.client.Language);
            string Tips = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "",Program.client.Language);
            DataTable tab = GetChecedkData();
            if (tab.Rows.Count <= 0)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请选择排程数据！");
                return;
            }
            DialogResult dr = MessageBox.Show(SureClose, Tips, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("data", tab);
                p.Add("status", "99");
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpdateStatus", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    btn_query_Click(sender, e);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

                //int index = dataGridView2.CurrentRow.Index;
                //if (index > -1 && dataGridView2.Rows[index].Cells[0].Value != null)
                //{
                //    string seId = dataGridView2.Rows[index].Cells[1].Value == null ? "" : dataGridView2.Rows[index].Cells[1].Value.ToString();
                //    string workDate = dataGridView2.Rows[index].Cells["Column4"].Value == null ? "" : dataGridView2.Rows[index].Cells["Column4"].Value.ToString();
                //    string deptNo = dataGridView2.Rows[index].Cells[4].Value == null ? "" : dataGridView2.Rows[index].Cells[4].Value.ToString();
                //    string col1 = dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value == null ? "" : dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value.ToString();
                //    string inout_pz = dataGridView2.Rows[index].Cells["INOUT_PZ"].Value == null ? "" : dataGridView2.Rows[index].Cells["INOUT_PZ"].Value.ToString();
                //    Dictionary<string, Object> p = new Dictionary<string, object>();
                //    p.Add("vSeId", seId);
                //    p.Add("vDDept", deptNo);
                //    p.Add("vWrokDay", workDate);
                //    p.Add("vColumn1", col1);
                //    p.Add("vInOut", inout_pz);
                //    p.Add("status", "99");
                //    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpdateStatus", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                //    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                //    {
                //        btn_query_Click(sender, e);
                //    }
                //    else
                //    {
                //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                //    }
                //}
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                int index = dataGridView2.CurrentRow.Index;
                if (index > -1 && dataGridView2.Rows[index].Cells[0].Value != null)
                {
                    string seId = dataGridView2.Rows[index].Cells["Column1"].Value == null ? "" : dataGridView2.Rows[index].Cells["Column1"].Value.ToString();
                    string workDate = dataGridView2.Rows[index].Cells["Column4"].Value == null ? "" : dataGridView2.Rows[index].Cells["Column4"].Value.ToString();
                    string deptNo = dataGridView2.Rows[index].Cells["Column13"].Value == null ? "" : dataGridView2.Rows[index].Cells["Column13"].Value.ToString();
                    string rout_no = dataGridView2.Rows[index].Cells["combobox7"].Value == null ? "" : dataGridView2.Rows[index].Cells["combobox7"].Value.ToString();
                    PlanAdjustmentForm frm = new PlanAdjustmentForm(seId, deptNo, workDate, rout_no);
                    frm.Show();
                }
            }
            catch (Exception ex) {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "Choose row(s) first");
            }
            
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            //确定要删除吗?
            String message1 = SJeMES_Framework.Common.UIHelper.UImsg("msg-00004", Program.client, "",Program.client.Language);
            //错误
            String message2 = SJeMES_Framework.Common.UIHelper.UImsg("msg-00002", Program.client,"", Program.client.Language);
            DataTable tab = GetChecedkData();
            if (tab.Rows.Count <= 0)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请选择排程数据！");
                return;
            }

            DialogResult dr = MessageBox.Show(message1, message2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("data", tab);
                p.Add("status", "99");
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpdateStatus", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    btn_query_Click(sender, e);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

                //int index = dataGridView2.CurrentRow.Index;
                //if (index > -1 && dataGridView2.Rows[index].Cells[0].Value != null)
                //{
                //    string seId = dataGridView2.Rows[index].Cells[1].Value == null ? "" : dataGridView2.Rows[index].Cells[1].Value.ToString();
                //    string workDate = dataGridView2.Rows[index].Cells["Column4"].Value == null ? "" : dataGridView2.Rows[index].Cells["Column4"].Value.ToString();
                //    string deptNo = dataGridView2.Rows[index].Cells[4].Value == null ? "" : dataGridView2.Rows[index].Cells[4].Value.ToString();
                //    string col1 = dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value == null ? "" : dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value.ToString();
                //    string inout_pz = dataGridView2.Rows[index].Cells["INOUT_PZ"].Value == null ? "" : dataGridView2.Rows[index].Cells["INOUT_PZ"].Value.ToString();
                //    Dictionary<string, Object> p = new Dictionary<string, object>();
                //    p.Add("vSeId", seId);
                //    p.Add("vDDept", deptNo);
                //    p.Add("vWrokDay", workDate);
                //    p.Add("vColumn1", col1);
                //    p.Add("vInOut", inout_pz);
                //    p.Add("status", "99");
                //    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpdateStatus", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                //    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                //    {
                //        btn_query_Click(sender, e);
                //    }
                //    else
                //    {
                //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                //    }
                //}
            }
        }

        private void btn_vali_Click(object sender, EventArgs e)
        {
            //是否要生效?
            String message1 = SJeMES_Framework.Common.UIHelper.UImsg("msg-00003", Program.client,"", Program.client.Language);
            //错误
            String message2 = SJeMES_Framework.Common.UIHelper.UImsg("msg-00002", Program.client,"", Program.client.Language);
            DataTable tab = GetChecedkData();
            if (tab.Rows.Count <= 0)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请选择排程数据！");
                return;
            }
            DialogResult dr = MessageBox.Show(message1, message2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("data", tab);
                p.Add("status", "7");
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpdateStatus", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    btn_query_Click(sender, e);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

                //int index = dataGridView2.CurrentRow.Index;
                //if (index > -1 && dataGridView2.Rows[index].Cells[0].Value != null)
                //{
                //    string seId = dataGridView2.Rows[index].Cells[1].Value == null ? "" : dataGridView2.Rows[index].Cells[1].Value.ToString();
                //    string workDate = dataGridView2.Rows[index].Cells["Column4"].Value == null ? "" : dataGridView2.Rows[index].Cells["Column4"].Value.ToString();
                //    string deptNo = dataGridView2.Rows[index].Cells[4].Value == null ? "" : dataGridView2.Rows[index].Cells[4].Value.ToString();
                //    string col1 = dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value == null ? "" : dataGridView2.Rows[index].Cells["cbWorkDayColumn1"].Value.ToString();
                //    string inout_pz = dataGridView2.Rows[index].Cells["INOUT_PZ"].Value == null ? "" : dataGridView2.Rows[index].Cells["INOUT_PZ"].Value.ToString();
                //    Dictionary<string, Object> p = new Dictionary<string, object>();
                //    p.Add("vSeId", seId);
                //    p.Add("vDDept", deptNo);
                //    p.Add("vWrokDay", workDate);
                //    p.Add("vColumn1", col1);
                //    p.Add("vInOut", inout_pz);
                //    p.Add("status", "7");
                //    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionTargetsServer", "UpdateStatus", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                //    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                //    {
                //        btn_query_Click(sender, e);
                //    }
                //    else
                //    {
                //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                //    }
                //}
            }
        }

        private DataTable GetChecedkData()
        {
            DataTable tab = new DataTable();
            tab.Columns.Add("seId");
            tab.Columns.Add("workDate");
            tab.Columns.Add("deptNo");
            tab.Columns.Add("col1");
            tab.Columns.Add("inout_pz");

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue)
                {
                    string seId = row.Cells["Column1"].Value == null ? "" : row.Cells["Column1"].Value.ToString();
                    string workDate = row.Cells["Column4"].Value == null ? "" : row.Cells["Column4"].Value.ToString();
                    string deptNo = row.Cells["Column13"].Value == null ? "" : row.Cells["Column13"].Value.ToString();
                    string col1 = row.Cells["cbWorkDayColumn1"].Value == null ? "" : row.Cells["cbWorkDayColumn1"].Value.ToString();
                    string inout_pz = row.Cells["INOUT_PZ"].Value == null ? "" : row.Cells["INOUT_PZ"].Value.ToString();

                    DataRow dr = tab.NewRow();
                    dr[0] = seId;
                    dr[1] = workDate;
                    dr[2] = deptNo;
                    dr[3] = col1;
                    dr[4] = inout_pz;
                    tab.Rows.Add(dr);
                }
            }

            return tab;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                if ((bool)dataGridView2.Rows[e.RowIndex].Cells[0].EditedFormattedValue)
                {
                    dataGridView2.Rows[e.RowIndex].Cells[0].Value = false;
                }
                else
                {
                    dataGridView2.Rows[e.RowIndex].Cells[0].Value = true;
                }
            }
        }

        private void butCheck_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                checkTime++;
                if (checkTime % 2 == 0)
                {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells[0].Value = false;
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells[0].Value = true;
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
