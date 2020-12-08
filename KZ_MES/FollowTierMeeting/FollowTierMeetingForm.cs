using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using SJeMES_Control_Library;
namespace FollowTierMeeting
{
    public partial class FollowTierMeetingForm : MaterialForm
    {
        Waiting wait = new Waiting();
        string keywork = "";
        Dictionary<string, object> msg_query;
        string dept = "";
        string type = "";
        string dept_standa = "";
        string type_standa = "";
        string dept_maturity = "";
        string type_maturity = "";
        DataTable tb_maturity;
        DataTable tb_tier;
        DataTable tb_standa;
        public FollowTierMeetingForm()
        {
            InitializeComponent();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            tabControlExt1.Font= new Font("宋体", (float)Screen.PrimaryScreen.Bounds.Height / 80f, FontStyle.Regular, GraphicsUnit.Point, 134);
            tableLayoutPanel1.Font = new Font("宋体", (float)Screen.PrimaryScreen.Bounds.Height / 80f, FontStyle.Regular, GraphicsUnit.Point, 134);
            tableLayoutPanel2.Font = new Font("宋体", (float)Screen.PrimaryScreen.Bounds.Height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            tableLayoutPanel18.Font = new Font("宋体", (float)Screen.PrimaryScreen.Bounds.Height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            tableLayoutPanel10.Font = new Font("宋体", (float)Screen.PrimaryScreen.Bounds.Height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtDateTier1.Text = DateTime.Now.ToString("yyyy/MM").Replace('-', '/');
            txtDateStanda.Text = txtDateTier1.Text;
            txtDate_Maturity.Text = txtDateTier1.Text;
            CreateColumDay(dataGridView1, "coltier_");
            CreateColumDay(dataGridView2, "colstanda_");
            CreateColumDay(dataGridView3, "colmaturity_");
        }
        private void CreateColumDay(DataGridView dgv,string coldefaultname= "coltier_")
        {
            DataGridViewColumn col;
            int count = dgv.Columns.Count;
            for (int i = 1; i <= 31; i++)
            {
                dgv.Columns.Add(coldefaultname + i, i + "");
                dgv.Columns[count + i - 1].DataPropertyName = String.Format("{0:00}", i);
                dgv.Columns[count + i - 1].Width = 55;
              //  dgv.Columns[count + i - 1].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                dgv.Columns[count + i - 1].DisplayIndex = i;
            }
        }
        private void VisibleColDataGridview(DataGridView dgv, DateTime _workday,int maxcol=31)
        {
            int endday = 0;
            dgv.EnableHeadersVisualStyles = false;
            DateTime workday = new DateTime(_workday.Year, _workday.Month, 1);
            DateTime nowday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (workday >= nowday)
            {
                if (workday == nowday)
                    endday = DateTime.Now.Day;
            }
            else
                endday = 31;
            int coun = dgv.Columns.Count;
            for (int i =1; i <coun; i++)
            {
                try
                {
                    if (i < maxcol)
                        dgv.Columns[i].Visible = true;
                    else
                        dgv.Columns[i].Visible = false;
                    if(i <= endday) 
                        dgv.Columns[i].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                    else
                        dgv.Columns[i].DefaultCellStyle.BackColor = Color.White;

                     //if((new DateTime(workday.Year,workday.Month,i)).DayOfWeek==DayOfWeek.Sunday)
                     //  dgv.Columns[i].HeaderCell.Style.BackColor = Color.Red;
                     //else
                     //   dgv.Columns[i].HeaderCell.Style.BackColor = Color.White;
                }
                catch { }
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void FollowTierMeetingForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            GetMaturity();        
        }

        private void btnQueryTier1_Click(object sender, EventArgs e)
        {
            tb_tier = null;
            RunQuery(btnQueryTier1.Name);
        }
        private void GetTMS_TIER()
        {
            tb_tier = null;
            msg_query = new Dictionary<string, object>();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string ret = "";
            try
            {              
                p.Add("dept",dept);
                p.Add("date",txtDateTier1.Text);
                p.Add("type",type);            
                ret=   SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                        Program.client.APIURL,
                         "TierMeeting", "TierMeeting.Controllers.FollowTierMeetingServer",
                                            "TMS_TIER_Query",
                        Program.client.UserToken,
                        JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);                               
                    msg_query.Add("IsSuccess", "RetData");
                    if (btnQueryTier1.Name == keywork)
                    {
                        tb_tier = dtJson;
                        DataTable tem = Parameters.Pivot(dtJson, dtJson.Columns[1].ColumnName, dtJson.Columns[0].ColumnName);
                        msg_query.Add("RetData", tem);
                    }
                    else
                        msg_query.Add("RetData", dtJson);
                }
                else
                {
                    msg_query.Add("IsSuccess", "ErrMsg");
                    msg_query.Add("ErrMsg", JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            catch (Exception er)
            {
                msg_query.Add("IsSuccess", "ErrMsg");
                msg_query.Add("ErrMsg", er.Message);
            }

        }    
        private void GetTIER1_STANDARD()
        {
            tb_standa = null;
            msg_query = new Dictionary<string, object>();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string ret = "";
            try
            {
                p.Add("dept", dept_standa);
                p.Add("date", txtDateStanda.Text);
                p.Add("type", type_standa);
                ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                        Program.client.APIURL,
                         "TierMeeting", "TierMeeting.Controllers.FollowTierMeetingServer",
                                            "TMS_TIER1_STANDARD_Query",
                        Program.client.UserToken,
                        JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    tb_standa = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    msg_query.Add("IsSuccess", "RetData");
                    msg_query.Add("RetData", Parameters.Pivot(tb_standa, tb_standa.Columns[1].ColumnName, tb_standa.Columns[0].ColumnName));
                }
                else
                {
                    msg_query.Add("IsSuccess", "ErrMsg");
                    msg_query.Add("ErrMsg", JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            catch (Exception er)
            {
                msg_query.Add("IsSuccess", "ErrMsg");
                msg_query.Add("ErrMsg", er.Message);
            }

        }
        private void GetMaturity_assessment()
        {
            tb_maturity = null;
            msg_query = new Dictionary<string, object>();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string ret = "";
            try
            {
                p.Add("dept", txtDept_Maturity.Text);
                p.Add("date", txtDate_Maturity.Text);
                p.Add("type",type_maturity);
                ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                        Program.client.APIURL,
                         "TierMeeting", "TierMeeting.Controllers.FollowTierMeetingServer",
                                            "GetMaturityAssessmentList",
                        Program.client.UserToken,
                        JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    tb_maturity = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    msg_query.Add("IsSuccess", "RetData");
                    msg_query.Add("RetData", Parameters.Pivot(tb_maturity, tb_maturity.Columns[1].ColumnName, tb_maturity.Columns[0].ColumnName));
                }
                else
                {
                    msg_query.Add("IsSuccess", "ErrMsg");
                    msg_query.Add("ErrMsg", JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            catch (Exception er)
            {
                msg_query.Add("IsSuccess", "ErrMsg");
                msg_query.Add("ErrMsg", er.Message);
            }

        }
        private void GetMaturity()
        {
           // msg_query = new Dictionary<string, object>();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string ret = "";
            try
            {
                //p.Add("dept", txtDept_Maturity.Text);
                //p.Add("date", txtDate_Maturity.Text);
                ////   p.Add("type", type);
                ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                        Program.client.APIURL,
                         "TierMeeting", "TierMeeting.Controllers.FollowTierMeetingServer",
                                            "GetMaturityList",
                        Program.client.UserToken,
                        JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    gridData.DataSource = dtJson;
                    switch (Program.client.Language)
                    {
                        case "en":
                            gridData.Columns["NAMEEN"].Visible = true;
                          //  gridData.Columns["NAMEEN"].HeaderText = lblNameText.Text;
                            break;
                        case "cn":
                            gridData.Columns["NAMECN"].Visible = true;
                         //   gridData.Columns["NAMECN"].HeaderText = lblNameText.Text;
                            break;
                        default:
                            gridData.Columns["NAMEYN"].Visible = true;
                         //   gridData.Columns["NAMEYN"].HeaderText = lblNameText.Text;
                            break;
                    }
                    gridData.ColumnHeadersVisible = true;
                }
                else
                {
                    
                }
            }
            catch (Exception er)
            {
               
            }

        }
        private void RunQuery(string key)
        {
           if(!bgw_query.IsBusy)
            {
                keywork = key;
                EnableButton(false);
                bgw_query.RunWorkerAsync();
            }
        }
        private void bgw_query_DoWork(object sender, DoWorkEventArgs e)
        {
            if (keywork == btnQueryTier1.Name)
                GetTMS_TIER();       
            if (keywork == btnQueryStanda.Name)
                GetTIER1_STANDARD();
            if (keywork == btnQueryMauturity.Name)
                GetMaturity_assessment();

        }

        private void bgw_query_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string IsSuccess = msg_query["IsSuccess"].ToString();
            if (IsSuccess == "RetData")
            {
                if (keywork == btnQueryTier1.Name)
                {
                    CleanTierLable();
                    dataGridView1.DataSource = (DataTable)msg_query["RetData"];
                    int endday = 0;
                    DateTime workday = new DateTime(dtpDateTier1.Value.ToDate().Year, dtpDateTier1.Value.ToDate().Month, 1);                   
                    VisibleColDataGridview(dataGridView1,workday,1+ DateTime.DaysInMonth(dtpDateTier1.Value.ToDate().Year, dtpDateTier1.Value.ToDate().Month));
                }
                if (keywork == btnQueryStanda.Name)
                {
                    CleanTierStandaLable();
                    dataGridView2.DataSource = (DataTable)msg_query["RetData"];
                    int endday = 0;
                    DateTime workday = new DateTime(dtpDateStanda.Value.ToDate().Year, dtpDateStanda.Value.ToDate().Month, 1);
                                       VisibleColDataGridview(dataGridView2,workday, 1 + DateTime.DaysInMonth(dtpDateStanda.Value.ToDate().Year, dtpDateStanda.Value.ToDate().Month));
                }
                if (keywork == btnQueryMauturity.Name)
                {
                    ClearMaturity();
                    // string[] TobeDistinct = { "deptcode", "updateddate" };
                    // DataTable dtDistinct =Parameters.GetDistinctRecords(tb_maturity, TobeDistinct);
                    dataGridView3.DataSource = (DataTable)msg_query["RetData"];
                    int endday = 0;
                    DateTime workday = new DateTime(dtpDate_Maturity.Value.ToDate().Year, dtpDate_Maturity.Value.ToDate().Month, 1);
                    DateTime nowday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (workday >= nowday)
                    {
                        if (workday == nowday)
                            endday = DateTime.Now.Day;
                    }
                    else
                        endday = 31;
                    VisibleColDataGridview(dataGridView3, workday, 1 + DateTime.DaysInMonth(dtpDate_Maturity.Value.ToDate().Year, dtpDate_Maturity.Value.ToDate().Month));

                }
            }
            else
            {
                MessageHelper.ShowErr(this, msg_query["ErrMsg"].ToString());
                if (keywork == btnQueryTier1.Name)
                    Parameters.clearDatagridview(dataGridView1);
                if (keywork == btnQueryStanda.Name)
                    Parameters.clearDatagridview(dataGridView2);
                if (keywork == btnQueryMauturity.Name)
                {                   
                    Parameters.clearDatagridview(dataGridView3);
                }
            }
           
            EnableButton(true);
        }
        private void EnableButton(bool flag)
        {
            if (flag)
            {
                wait.Hide();
                this.Cursor = Cursors.Default;
            }
            else
            {

                wait.Show();
                this.Cursor = Cursors.WaitCursor;
            }
            btnQueryTier1.Enabled = flag;
            btnExportStanda.Enabled = flag;
            btnExportStanda.Enabled = flag;
            btnExportTier1.Enabled = flag;
            btnQueryMauturity.Enabled = flag;
            btnExport_Maturity.Enabled = flag;

        }
        private void LoadTier1(int rowindex, int colindex)
        {
            string dept = dataGridView1.Rows[rowindex].Cells["colG_DEPTCODE"].Value.ToString();
            string date = "";
            int num = 0;
            if (int.TryParse(dataGridView1.Columns[colindex].DataPropertyName, out num))
            {
                date = dtpDateTier1.Value.ToString("yyyy/MM").Replace('-', '/') + "/" + string.Format("{0:00}", num);
                DataRow[] row = Parameters.FindText(tb_tier, "G_DEPTCODE='" + dept + "' and g_date='" + date + "'", "") as DataRow[];
                for (int i = 1; i <= 8; i++)
                {
                    try
                    {
                        //clear checkbox Tier
                        CheckBox cbb1 = this.Controls.Find("cbxTier1_" + i, true).FirstOrDefault() as CheckBox;
                        if (row.Length > 0)
                        {
                            Parameters.TextToCheckbox(cbb1, row[0]["G_" + i].ToString());
                        }
                        else
                            Parameters.TextToCheckbox(cbb1, "N");
                    }
                    catch { }
                }
            }
            else
            {
                CleanTierLable();
            }
        }
        private void CleanTierLable()
        {
            for (int i = 1; i <= 8; i++)
            {
                try
                {
                    //clear checkbox Tier
                    CheckBox cbb1 = this.Controls.Find("cbxTier1_" + i, true).FirstOrDefault() as CheckBox;
                    Parameters.TextToCheckbox(cbb1, "N", true);
                }
                catch
                {

                }
            }
        }
        private void LoadTier1_Standa(int rowindex,int colindex)
        {
            string dept = dataGridView2.Rows[rowindex].Cells["col_G_DEPTCODE"].Value.ToString();
            string date = "";
            int num = 0;
            if (int.TryParse(dataGridView2.Columns[colindex].DataPropertyName, out num))
            {
                date = dtpDateStanda.Value.ToString("yyyy/MM").Replace('-', '/') + "/" + string.Format("{0:00}", num);
                DataRow[] row = Parameters.FindText(tb_standa, "G_DEPTCODE='" + dept + "' and g_date='" + date + "'", "") as DataRow[];
                for (int j = 1; j <= 3; j++)
                    for (int i = 1; i <= 8; i++)
                    {
                        try
                        {
                            CheckBox cbb1 = this.Controls.Find("cbxStandard" + j + "_" + i, true).FirstOrDefault() as CheckBox;
                            if (row.Length > 0)
                            {
                                if (j == 1)
                                {
                                    Parameters.TextToCheckbox(cbb1, row[0]["g_supervisor_" + i].ToString());
                                }
                                else
                                    if (j == 2)
                                {
                                    Parameters.TextToCheckbox(cbb1, row[0]["g_vsm_" + i].ToString());
                                }
                                else
                                    if (j == 3)
                                {
                                    Parameters.TextToCheckbox(cbb1, row[0]["g_third_party_" + i].ToString());
                                }
                            }
                            else
                            {
                                Parameters.TextToCheckbox(cbb1, "N");
                            }
                        }
                        catch { }
                    }
                try
                {
                    rtbStandard1.Text = row[0]["g_supervisor_auditor"].ToString();
                    rtbStandard2.Text = row[0]["g_vsm_auditor"].ToString();
                    rtbStandard3.Text = row[0]["g_third_party_auditor"].ToString();
                }
                catch { }
            }
            else
                CleanTierStandaLable();
        }
        private void CleanTierStandaLable()
        {
            for (int j = 1; j <= 3; j++)
                for (int i = 1; i <= 8; i++)
                {
                    try
                    {
                        CheckBox cbb1 = this.Controls.Find("cbxStandard" + j + "_" + i, true).FirstOrDefault() as CheckBox;
                        Parameters.TextToCheckbox(cbb1, "N",true);
                    }
                    catch { }
                }
            try
            {
                rtbStandard1.Text = "";
                rtbStandard2.Text = "";
                rtbStandard3.Text = "";
            }
            catch { }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                LoadTier1(e.RowIndex,e.ColumnIndex);
        }             

        private void dtpDateTier1_ValueChanged(object sender, EventArgs e)
        {         
            txtDateTier1.Text = dtpDateTier1.Value.ToDate().ToString("yyyy/MM").Replace('-','/');
        }

        private void dtpDateTier1_DropDown(object sender, EventArgs e)
        {
         //   txtDateTier1.Text = "";
        }

        private void btnExportTier1_Click(object sender, EventArgs e)
        {
            ExcelCommand.ExportExcel(dataGridView1, "Export Tier meeting 1", "Department_"+txtDeptTier1.Text+"_" + txtDateTier1.Text.Replace('/','-'), true);
        }

        private void txtDeptTier1_Click(object sender, EventArgs e)
        {
            F_BCS_SelectDepartment_ProcessForm dept = new F_BCS_SelectDepartment_ProcessForm((int)Parameters.QueryDeptType.Line, (int)Parameters.DeptForm.All);
            dept.DataChange += new F_BCS_SelectDepartment_ProcessForm.DataChangeHandler(DeptSelected);
            dept.ShowDialog();
        }
       
        private void DeptSelected(object sender, F_BCS_SelectDepartment_ProcessForm.DataChangeEventArgs args)
        {
            txtDeptTier1.Text = args.deptCode;
            dept = args.deptCode;
            type = args.deptType.ToString();
        }
       
        private void DeptSelected_Standa(object sender, F_BCS_SelectDepartment_ProcessForm.DataChangeEventArgs args)
        {
            txtDeptStanda.Text = args.deptCode;
            dept_standa = args.deptCode;
            type_standa = args.deptType.ToString();
        }

        private void DeptSelected_Maturity(object sender, F_BCS_SelectDepartment_ProcessForm.DataChangeEventArgs args)
        {
            txtDept_Maturity.Text = args.deptCode;
            dept_maturity = args.deptCode;
            type_maturity = args.deptType.ToString();
        }
        private void btnQueryStanda_Click(object sender, EventArgs e)
        {
            RunQuery(btnQueryStanda.Name);
        }

        private void dtpDateStanda_DropDown(object sender, EventArgs e)
        {
          //  txtDateStanda.Text = "";
        }

        private void dtpDateStanda_ValueChanged(object sender, EventArgs e)
        {
            txtDateStanda.Text = dtpDateStanda.Value.ToDate().ToString("yyyy/MM").Replace("-","/");
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                LoadTier1_Standa(e.RowIndex,e.ColumnIndex);
        }

        private void txtDeptStanda_Click(object sender, EventArgs e)
        {
            F_BCS_SelectDepartment_ProcessForm dept = new F_BCS_SelectDepartment_ProcessForm((int)Parameters.QueryDeptType.Line, (int)Parameters.DeptForm.All);
            dept.DataChange += new F_BCS_SelectDepartment_ProcessForm.DataChangeHandler(DeptSelected_Standa);
            dept.ShowDialog();
        }

        private void btnExportStanda_Click(object sender, EventArgs e)
        {
            ExcelCommand.ExportExcel(dataGridView2, "Standard Manufacturing Environment","Department_"+ txtDeptStanda.Text+"_"+txtDateStanda.Text, true);
        }

        private void btnQueryMauturity_Click(object sender, EventArgs e)
        {
            RunQuery(btnQueryMauturity.Name);
        }

        private void dtpDate_Maturity_ValueChanged(object sender, EventArgs e)
        {
            txtDate_Maturity.Text = dtpDate_Maturity.Value.ToDate().ToString("yyyy/MM").Replace('-','/');
        }

        private void dtpDate_Maturity_DropDown(object sender, EventArgs e)
        {
          //  txtDate_Maturity.Text = "";
        }
        private void SetValue(int rowindex,int colindex)
        {           
            string dept = dataGridView3.Rows[rowindex].Cells["col_DEPTCODE"].Value.ToString();
            string date = "";
            int num = 0;
            if (int.TryParse(dataGridView3.Columns[colindex].DataPropertyName, out num))
            {
                date = dtpDate_Maturity.Value.ToString("yyyy/MM").Replace('-', '/') + "/" + string.Format("{0:00}", num);
                string fill = "";
                fill = "deptcode='" + dept +
                 "' and updateddate='" + date + "' ";
                DataRow[] row = Parameters.FindText(tb_maturity, fill) as DataRow[];
                if (row.Length>0)
                {
                    try
                    {
                        for (int i = 0; i < row.Length; i++)
                        {
                            if (gridData.Rows[i].Cells["code"].Value.ToString() == row[i]["code"].ToString())
                            {
                                gridData.Rows[i].Cells["Status"].Value = row[i]["Status"];
                                gridData.Rows[i].Cells["Note"].Value = row[i]["Note"];
                            }
                        }
                    }
                    catch
                    {

                    }
                }else
                    ClearMaturity();
            }
            else
                ClearMaturity();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                SetValue(e.RowIndex,e.ColumnIndex);
        }
        private void ClearMaturity()
        {
          for(int i=0;i<gridData.Rows.Count;i++)
            {
                try
                {
                    gridData.Rows[i].Cells["Status"].Value = "";
                    gridData.Rows[i].Cells["Note"].Value = "";
                }
                catch { }
            }
        }

        private void txtDept_Maturity_Click(object sender, EventArgs e)
        {
            F_BCS_SelectDepartment_ProcessForm dept = new F_BCS_SelectDepartment_ProcessForm((int)Parameters.QueryDeptType.Line, (int)Parameters.DeptForm.All);
            dept.DataChange += new F_BCS_SelectDepartment_ProcessForm.DataChangeHandler(DeptSelected_Maturity);
            dept.ShowDialog();
        }

        private void btnExport_Maturity_Click(object sender, EventArgs e)
        {
            ExcelCommand.ExportExcel(dataGridView3, "Maturity Assessment", "Department_"+txtDept_Maturity.Text+"_"+txtDate_Maturity.Text.Replace('/','-'), true);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                string ms = this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName;
                int num = 0;
                if (int.TryParse(ms, out num))
                {
                    try
                    {
                        string stringValue = e.Value.ToString();
                        if (stringValue != "")
                        {
                            e.Value = "";
                            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                        }
                        //else
                        //    this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                    }
                    catch { }
                }

            }

        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                string ms = this.dataGridView2.Columns[e.ColumnIndex].DataPropertyName;
                int num = 0;
                if (int.TryParse(ms, out num))
                {
                    try
                    {
                        string stringValue = e.Value.ToString();
                        if (stringValue != "")
                        {
                            e.Value = "";
                            this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                        }                       
                    }
                    catch { }
                }

            }
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                string ms = this.dataGridView3.Columns[e.ColumnIndex].DataPropertyName;
                int num = 0;
                if (int.TryParse(ms, out num))
                {
                    try
                    {
                        string stringValue = e.Value.ToString();
                        if (stringValue != "")
                        {
                              e.Value = "";
                            this.dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                        }
                    }
                    catch { }
                }

            }
        }
    }
}
