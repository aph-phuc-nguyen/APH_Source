using System;
using MaterialSkin.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_EPM_MachineImport
{
    public partial class F_EPM_MachineImport : MaterialForm
    {
        public Boolean isTitle = false;
        IList<object[]> data = null;
        private ExcelProcessor _currentExcelProcessor = null;
        //DataTable dt_MachineInfo = new DataTable();
        public F_EPM_MachineImport()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            var frmtype = this.GetType();
        }
        private void Import_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    LoadMachineInfo();
            //}
            //catch(Exception ex)
            //{
            //}          
        }

        //private DataTable LoadMachineInfo()
        //{         
        //    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EPMAPI", "KZ_EPMAPI.Controllers.MachineImportServer", "GetMachineNO", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(string.Empty));
        //    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
        //    {
        //        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
        //        dt_MachineInfo = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
        //        int count = dt_MachineInfo.Rows.Count;
        //    }
        //    else
        //    {
        //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
        //    }
        //    return dt_MachineInfo;
        //}

        private void btnMachineOpen_Click(object sender, EventArgs e)
        {
            isTitle = true;
            this.dataGridView.AutoGenerateColumns = false;
            if (dataGridView != null)
            {
                this.dataGridView.Rows.Clear();
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
                    foreach (object[] row in data)
                    {
                        if (!isTitle)
                        {
                            try
                            {
                                row[6] = Convert.ToDateTime(row[6]?.ToString()).ToShortDateString();
                                row[8] = Convert.ToDateTime(row[8]?.ToString()).ToShortDateString();
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
        private void btnMaintainOpen_Click(object sender, EventArgs e)
        {
            isTitle = true;
            this.dataGridView3.AutoGenerateColumns = false;
            if (dataGridView3 != null)
            {
                this.dataGridView3.Rows.Clear();
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "EXCEL|*.xls*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (dataGridView2 != null)
                {
                    data = new List<object[]>();
                    this.dataGridView2.Rows.Clear();
                }
                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        this.dataGridView2.Rows.Add(filename);
                        dataGridView2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        this.GetExcelData(Path.GetFullPath(filename));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //dataGridView2.Name = resources.GetString("ImportData");
                dataGridView3.AllowUserToAddRows = false;
                Boolean isTitle = true;
                if (data != null && data.Count > 0)
                {
                    foreach (object[] row in data)
                    {
                        if (!isTitle)
                        {                           
                            dataGridView3.Rows.Add(row);
                        }
                        else
                        {
                            isTitle = false;
                        }
                    }
                }
                dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
        }
        private void btnCorrectionOpen_Click(object sender, EventArgs e)
        {
            isTitle = true;
            this.dataGridView5.AutoGenerateColumns = false;
            if (dataGridView5 != null)
            {
                this.dataGridView5.Rows.Clear();
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "EXCEL|*.xls*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (dataGridView4 != null)
                {
                    data = new List<object[]>();
                    this.dataGridView4.Rows.Clear();
                }
                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        this.dataGridView4.Rows.Add(filename);
                        dataGridView4.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        this.GetExcelData(Path.GetFullPath(filename));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //dataGridView2.Name = resources.GetString("ImportData");
                dataGridView5.AllowUserToAddRows = false;
                Boolean isTitle = true;
                if (data != null && data.Count > 0)
                {
                    foreach (object[] row in data)
                    {
                        if (!isTitle)
                        {
                            dataGridView5.Rows.Add(row);
                        }
                        else
                        {
                            isTitle = false;
                        }
                    }
                }
                dataGridView5.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
        }
        private void btnRepairOpen_Click(object sender, EventArgs e)
        {
            isTitle = true;
            this.dataGridView7.AutoGenerateColumns = false;
            if (dataGridView7 != null)
            {
                this.dataGridView7.Rows.Clear();
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "EXCEL|*.xls*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (dataGridView6 != null)
                {
                    data = new List<object[]>();
                    this.dataGridView6.Rows.Clear();
                }
                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        this.dataGridView6.Rows.Add(filename);
                        dataGridView6.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        this.GetExcelData(Path.GetFullPath(filename));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //dataGridView2.Name = resources.GetString("ImportData");
                dataGridView7.AllowUserToAddRows = false;
                Boolean isTitle = true;
                if (data != null && data.Count > 0)
                {
                    foreach (object[] row in data)
                    {
                        if (!isTitle)
                        {
                            dataGridView7.Rows.Add(row);
                        }
                        else
                        {
                            isTitle = false;
                        }
                    }
                }
                dataGridView7.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
        }

        private void btnMachineAdd_Click(object sender, EventArgs e)
        {
            btnMachineOpen.Enabled = false;
            btnMachineAdd.Enabled = false;
            if (this.dataGridView.Rows.Count >= 1)
            {
                MachineImport();
            }
            else
            {
                //文件不能为空
                String message1 = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client, "", Program.client.Language);
                //错误
                String message2 = SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.client, "", Program.client.Language);
                MessageBox.Show(message1, message2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnMachineOpen.Enabled = true;
            btnMachineAdd.Enabled = true;
        }
        private void btnMaintainAdd_Click(object sender, EventArgs e)
        {
            btnMachineOpen.Enabled = false;
            btnMachineAdd.Enabled = false;
            if (this.dataGridView3.Rows.Count >= 1)
            {
                MaintainImport();
            }
            else
            {
                //文件不能为空
                String message1 = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client, "", Program.client.Language);
                //错误
                String message2 = SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.client, "", Program.client.Language);
                MessageBox.Show(message1, message2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnMachineOpen.Enabled = true;
            btnMachineAdd.Enabled = true;
        }
        private void btnCorrectionAdd_Click(object sender, EventArgs e)
        {
            btnMachineOpen.Enabled = false;
            btnMachineAdd.Enabled = false;
            if (this.dataGridView5.Rows.Count >= 1)
            {
                CorrectionImport();
            }
            else
            {
                //文件不能为空
                String message1 = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client, "", Program.client.Language);
                //错误
                String message2 = SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.client, "", Program.client.Language);
                MessageBox.Show(message1, message2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnMachineOpen.Enabled = true;
            btnMachineAdd.Enabled = true;
        }
        private void btnRepairAdd_Click(object sender, EventArgs e)
        {
            btnMachineOpen.Enabled = false;
            btnMachineAdd.Enabled = false;
            if (this.dataGridView7.Rows.Count >= 1)
            {
                RepairImport();
            }
            else
            {
                //文件不能为空
                String message1 = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client, "", Program.client.Language);
                //错误
                String message2 = SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.client, "", Program.client.Language);
                MessageBox.Show(message1, message2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnMachineOpen.Enabled = true;
            btnMachineAdd.Enabled = true;
        }

        private void MachineImport()
        {
            Boolean isValiDate = true;
            isTitle = true;
            DataTable tab = new DataTable();
            tab.Columns.Add("MACHINE_NO");
            tab.Columns.Add("MACHINE_NAME");
            tab.Columns.Add("Description");
            tab.Columns.Add("Type");
            tab.Columns.Add("BRAND");
            tab.Columns.Add("UDF04");
            tab.Columns.Add("UDF05");
            tab.Columns.Add("PRICE");
            tab.Columns.Add("DATE_BUY");
            tab.Columns.Add("UDF01");
            tab.Columns.Add("ADDRESS");
            tab.Columns.Add("UDF02");
         
            for(int i = 0; i < data.Count; i++)
            {
                if (!isTitle)
                {
                    DataRow dr = tab.NewRow();                  
                    dr[0] = data[i][0].ToString().Trim();
                    dr[1] = data[i][1].ToString().Trim();
                    dr[2] = data[i][2].ToString().Trim();
                    dr[3] = data[i][3].ToString().Trim();
                    dr[4] = data[i][4].ToString().Trim();
                    dr[5] = data[i][5].ToString().Trim();
                    dr[6] = data[i][6].ToString().Trim();
                    dr[7] = data[i][7].ToString().Trim();
                    dr[8] = data[i][8].ToString().Trim();
                    dr[9] = data[i][9].ToString().Trim().ToUpper();
                    dr[10] = data[i][10].ToString().Trim();
                    dr[11] = data[i][11].ToString().Trim();
                    tab.Rows.Add(dr);
                }
                else
                {
                    isTitle = false;
                }
            }
            if (isValiDate)
            {
                Dictionary<string, Object> d = new Dictionary<string, object>();
                d.Add("data", tab);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EPMAPI", "KZ_EPMAPI.Controllers.MachineImportServer", "MachineUpLoad", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
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
                        string msg_m = SJeMES_Framework.Common.UIHelper.UImsg("msg-plan-00005", Program.client, "", Program.client.Language);
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
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }
        private void MaintainImport()
        {
            Boolean isValiDate = true;
            isTitle = true;
            DataTable tab = new DataTable();
            tab.Columns.Add("Item_ID");
            tab.Columns.Add("Item_NAME");
            tab.Columns.Add("Item_DESC");
            for (int i = 0; i < data.Count; i++)
            {
                if (!isTitle)
                {
                    DataRow dr = tab.NewRow();
                    dr[0] = data[i][0].ToString().Trim();
                    dr[1] = data[i][1].ToString().Trim();
                    dr[2] = data[i][2].ToString().Trim();                   
                    tab.Rows.Add(dr);
                }
                else
                {
                    isTitle = false;
                }
            }
            if (isValiDate)
            {
                Dictionary<string, Object> d = new Dictionary<string, object>();
                d.Add("data", tab);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EPMAPI", "KZ_EPMAPI.Controllers.MachineImportServer", "MaintainUpLoad", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
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
                        string msg_m = SJeMES_Framework.Common.UIHelper.UImsg("msg-plan-00005", Program.client, "", Program.client.Language);
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
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }
        private void CorrectionImport()
        {
            Boolean isValiDate = true;
            isTitle = true;
            DataTable tab = new DataTable();
            tab.Columns.Add("Item_ID");
            tab.Columns.Add("Item_NAME");
            tab.Columns.Add("Item_DESC");
            tab.Columns.Add("Tool");
            tab.Columns.Add("InnerOrOut");
            for (int i = 0; i < data.Count; i++)
            {
                if (!isTitle)
                {
                    DataRow dr = tab.NewRow();
                    dr[0] = data[i][0].ToString().Trim();
                    dr[1] = data[i][1].ToString().Trim();
                    dr[2] = data[i][2].ToString().Trim();
                    dr[3] = data[i][3].ToString().Trim();
                    dr[4] = data[i][4].ToString().Trim();
                    tab.Rows.Add(dr);
                }
                else
                {
                    isTitle = false;
                }
            }
            if (isValiDate)
            {
                Dictionary<string, Object> d = new Dictionary<string, object>();
                d.Add("data", tab);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EPMAPI", "KZ_EPMAPI.Controllers.MachineImportServer", "CorrectionUpLoad", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
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
                        string msg_m = SJeMES_Framework.Common.UIHelper.UImsg("msg-plan-00005", Program.client, "", Program.client.Language);
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
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }
        private void RepairImport()
        {

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
    }
}
