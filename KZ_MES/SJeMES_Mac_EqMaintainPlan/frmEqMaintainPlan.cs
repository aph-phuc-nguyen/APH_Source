using GDSJ_Framework.WinForm.CommonForm;
using SJeMES_Control_Library.Forms;
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
using System.Xml;

namespace SJeMES_Mac_EqMaintainPlan
{
    public partial class frmEqMaintainPlan : Form
    {
        public frmEqMaintainPlan(Dictionary<string, object> OBJ = null)
        {
            InitializeComponent();
            if (OBJ != null)
            {
                //Program.WebServiceUrl = (OBJ as Dictionary<string, object>)["WebServiceUrl"] as string;
                //Program.Org = new GDSJ_Framework.Class.OrgClass();
                //Program.Org.Org = (OBJ as Dictionary<string, object>)["Org"] as string;
                //Program.Org.OrgName = (OBJ as Dictionary<string, object>)["OrgName"] as string;
                //Program.Org.DBServer = (OBJ as Dictionary<string, object>)["DBServer"] as string;
                //Program.Org.DBType = (OBJ as Dictionary<string, object>)["DBType"] as string;
                //Program.Org.DBName = (OBJ as Dictionary<string, object>)["DBName"] as string;
                //Program.Org.DBUser = (OBJ as Dictionary<string, object>)["DBUser"] as string;
                //Program.Org.DBPassword = (OBJ as Dictionary<string, object>)["DBPassword"] as string;
                //Program.User = (OBJ as Dictionary<string, object>)["User"] as string;

                Program.WebServiceUrl = (OBJ as Dictionary<string, object>)["WebServiceUrl"] as string;
                Program.Org = new GDSJ_Framework.Class.OrgClass();
                Program.Org.Org = (OBJ as Dictionary<string, object>)["Org"] as string;
                Program.Org.OrgName = (OBJ as Dictionary<string, object>)["OrgName"] as string;
                Program.Org.DBServer = "";
                Program.Org.DBType = "oracle";
                Program.Org.DBName = "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.125)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME =SJWB )))";
                Program.Org.DBUser = "SJWB";
                Program.Org.DBPassword = "123";
                Program.User = (OBJ as Dictionary<string, object>)["User"] as string;
                //Oracle

            }
        }
        GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
        private void frmEqMaintainPlan_Load(object sender, EventArgs e)
        {
            GetLeftData();//加载左侧datagridview数据
            cbo_QueryName.SelectedIndex = 0;
            cbo_State.SelectedIndex = 0;
            cbo_PlanType.SelectedIndex = 0;
            dtp_StartTime.Value = DateTime.Now;
            dtp_CloseTime.Value = DateTime.Now;
            grb_Moth.Visible = false;
            grb_Week.Visible = false;
            grb_EveryYear.Visible = true;
            label1.Text = "";
        }

        private void rbo_EveryMonth_CheckedChanged(object sender, EventArgs e)
        {
            grb_EveryYear.Visible = false;
            grb_Moth.Visible = true;
            grb_Week.Visible = false;
            rbo_FirstYear.Checked = false;
            rbo_LastYear.Checked = false;
            rbo_DesYear.Checked = false;
            rbo_Monday.Checked = false;
            rbo_Tuesday.Checked = false;
            rbo_Thursday.Checked = false;
            rbo_Wednesday.Checked = false;
            rbo_Saturday.Checked = false;
            rbo_Friday.Checked = false;
            rbo_Sunday.Checked = false;
        }

        private void rbo_EveryYear_CheckedChanged(object sender, EventArgs e)
        {
            grb_EveryYear.Visible = true;
            grb_Moth.Visible = false;
            grb_Week.Visible = false;
            rbo_FirstMonth.Checked = false;
            rbo_LastMonth.Checked = false;
            rbo_DisMonth.Checked = false;
            rbo_Monday.Checked = false;
            rbo_Tuesday.Checked = false;
            rbo_Thursday.Checked = false;
            rbo_Wednesday.Checked = false;
            rbo_Saturday.Checked = false;
            rbo_Friday.Checked = false;
            rbo_Sunday.Checked = false;
        }

        private void rbo_EveryWeek_CheckedChanged(object sender, EventArgs e)
        {
            grb_EveryYear.Visible = false;
            grb_Moth.Visible = false;
            grb_Week.Visible = true;
            rbo_FirstMonth.Checked = false;
            rbo_LastMonth.Checked = false;
            rbo_DisMonth.Checked = false;
            rbo_FirstYear.Checked = false;
            rbo_LastYear.Checked = false;
            rbo_DesYear.Checked = false;
        }

        private void rbo_EveryDay_CheckedChanged(object sender, EventArgs e)
        {
            //grb_EveryYear.Enabled = true;
            grb_EveryYear.Visible = false;
            grb_Moth.Visible = false;
            grb_Week.Visible = false;
            rbo_DesYear.Checked = false;
            rbo_FirstMonth.Checked = false;
            rbo_LastMonth.Checked = false;
            rbo_DisMonth.Checked = false;
            rbo_FirstYear.Checked = false;
            rbo_LastYear.Checked = false;
            rbo_DesYear.Checked = false;
            rbo_Monday.Checked = false;
            rbo_Tuesday.Checked = false;
            rbo_Thursday.Checked = false;
            rbo_Wednesday.Checked = false;
            rbo_Saturday.Checked = false;
            rbo_Friday.Checked = false;
            rbo_Sunday.Checked = false;
        }

        private void btn_Plan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_PlanNo.Text.Trim()))
            {
                MessageBox.Show("计划编号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(txt_PlanName.Text.Trim()))
            {
                MessageBox.Show("计划名称不能为空");
                return;
            }
            if (label1.Text.Trim() == "新增")
            {
                //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
                string querystr = @"SELECT * FROM MAC020M WHERE paln_no='" + txt_PlanNo.Text.Trim() + "'";
                //bool b = DB.GetBool(querystr);
                DataTable b = Program.Client.GetDT(querystr); //DB.GetBool(querystr);
                if (b.Rows.Count>0)
                {
                    MessageBox.Show("不能重复添加相同的计划编号");
                    return;
                }
            }
            if (rbo_EveryYear.Checked)
            {
                datelist = EveryYear();
            }
            else if (rbo_EveryMonth.Checked)
            {
                datelist = EveryMoth();
            }
            else if (rbo_EveryWeek.Checked)
            {
                datelist = EveryWeek();
            }else if (rbo_EveryDay.Checked)
            {
                datelist.Clear();
                for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                {
                    datelist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                }
            }
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("设备明细不能为空！");
                return;
            }
            if (datelist.Count == 0)
            {
                MessageBox.Show("所选日期范围内不存在对应的日期!");
                return;
            }
            Dictionary<string, string> p = DataAcquisition();
            string xml = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.Org, Program.Client.WebServiceUrl, "SJEMS_WBAPI", "SJEMS_WBAPI.MAC020", "AddPlan", p);
            bool IsSuccess = Convert.ToBoolean(GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<IsSuccess>", "</IsSuccess>"));
            if (IsSuccess)
            {
               btn_Save_Click(null, null);
                GetLeftData();
          
                return;
            }
            else
                MessageBox.Show("保存失败!");
            label1.Text = "保存失败";
        }

        private List<string> datelist = new List<string>();
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if(label1.Text.Trim() == "删除")
            {
                if (string.IsNullOrEmpty(planNo))
                {
                    MessageBox.Show("请选择数据进行删除！");
                    return;
                }
                //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
                string sql= "DELETE FROM MAC020M WHERE  paln_no='" + planNo + "'";
                //if(DB.ExecuteNonQueryOffline(sql)>0)
                if (Program.Client.ExecuteNonQuery(sql) > 0)
                {
                    sql = " DELETE  FROM MAC020A1 WHERE  paln_no='" + planNo + "'";
                    //if (DB.ExecuteNonQueryOffline(sql) > 0)
                    if (Program.Client.ExecuteNonQuery(sql) > 0)
                    {
                        MessageBox.Show("删除成功！");
                        label1.Text = "删除成功";
                        GetLeftData();
                        QueryData(planNo);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("删除失败！");
                    label1.Text = "删除失败";
                    return;
                }

            }
            else
            {
                if (string.IsNullOrEmpty(txt_PlanNo.Text.Trim()))
                {
                    MessageBox.Show("计划编号不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(txt_PlanName.Text.Trim()))
                {
                    MessageBox.Show("计划名称不能为空");
                    return;
                }
                if (label1.Text.Trim() == "新增")
                {
                    //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
                    string querystr = @"SELECT * FROM MAC020M WHERE paln_no='" + txt_PlanNo.Text.Trim() + "'";
                    //bool b = DB.GetBool(querystr);
                    DataTable b = Program.Client.GetDT(querystr);
                    if (b.Rows.Count>0)
                    {
                        MessageBox.Show("不能重复添加相同的计划编号");
                        return;
                    }
                }

                if (rbo_EveryYear.Checked)
                {
                    if (rbo_FirstYear.Checked != true && rbo_LastYear.Checked != true && rbo_DesYear.Checked != true)
                    {
                        MessageBox.Show("必须选择按每年分配下的状态!");
                        return;
                    }
                }
                else if (rbo_EveryMonth.Checked)
                {
                    if (rbo_LastMonth.Checked != true && rbo_FirstMonth.Checked != true && rbo_DisMonth.Checked != true)
                    {
                        MessageBox.Show("必须选择按每月分配下的状态!");
                        return;
                    }
                }
                else if (rbo_EveryWeek.Checked)
                {
                    if (rbo_Monday.Checked != true && rbo_Tuesday.Checked != true && rbo_Thursday.Checked != true && rbo_Wednesday.Checked != true && rbo_Friday.Checked != true && rbo_Saturday.Checked != true && rbo_Sunday.Checked != true)
                    {
                        MessageBox.Show("必须选择按每周分配下的状态!");
                        return;
                    }
                }
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("设备明细不能为空！");
                    return;
                }
                try
                {
                    Dictionary<string, string> p = DataAcquisition();
                    string xml = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.Org, Program.Client.WebServiceUrl, "SJEMS_WBAPI", "SJEMS_WBAPI.MAC020", "AddDetail", p);
                    string Ret = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<RetData>", "</RetData>");
                    bool IsSuccess = Convert.ToBoolean(GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<IsSuccess>", "</IsSuccess>"));
                    if (IsSuccess)
                    {
                        MessageBox.Show("保存成功！");
                        label1.Text = "保存成功";
                        GetLeftData();
                        return;
                    }
                    else
                        MessageBox.Show(Ret);
                    label1.Text = "保存失败";


                }
                catch (Exception ex)
                {

                }
            }
           
        }
        private Dictionary<string, string> DataAcquisition()
        {
            try
            {
                Dictionary<string, string> p = new Dictionary<string, string>();
                string DataTable1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(DataTableToDataGridView());//设备明细

                //list转string
                string strtolist = string.Join(",", datelist.ToArray());
                p.Add("paln_no", txt_PlanNo.Text.Trim());  //计划编号
                p.Add("paln_name", txt_PlanName.Text.Trim()); //计划名称
                p.Add("plan_type", cbo_PlanType.Text.Trim());
                p.Add("manual_no", txt_Book.Text.Trim());
                p.Add("hours", dtp_Time.Value.ToString("HH:mm:ss"));
                p.Add("date", dtp_StartTime.Value.ToString("yyyy-MM-dd"));
                p.Add("date2", dtp_CloseTime.Value.ToString("yyyy-MM-dd"));
                p.Add("type_radio1", rbo_EveryYear.Checked.ToString());
                p.Add("type_radio2", rbo_EveryMonth.Checked.ToString());
                p.Add("type_radio3", rbo_EveryWeek.Checked.ToString());
                p.Add("type_radio4", rbo_EveryDay.Checked.ToString());
                p.Add("year_radio1", rbo_FirstYear.Checked.ToString());
                p.Add("year_radio2", rbo_LastYear.Checked.ToString());
                p.Add("year_radio3", rbo_DesYear.Checked.ToString());
                string year_md = string.Empty;
                if (rbo_DesYear.Checked)
                    year_md = (dtp_Year.Value.Month).ToString() + "月" + (dtp_Year.Value.Day).ToString() + "日";
                p.Add("year_md", year_md);
                p.Add("month_radio1", rbo_FirstMonth.Checked.ToString());
                p.Add("month_radio2", rbo_LastMonth.Checked.ToString());
                p.Add("month_radio3", rbo_DisMonth.Checked.ToString());
                string month_day = string.Empty;
                if (rbo_DesYear.Checked)
                    month_day = dtp_Month.Value.Day.ToString() + "日";
                p.Add("month_day", month_day);
                p.Add("week1", rbo_Monday.Checked.ToString());
                p.Add("week2", rbo_Tuesday.Checked.ToString());
                p.Add("week3", rbo_Wednesday.Checked.ToString());
                p.Add("week4", rbo_Thursday.Checked.ToString());
                p.Add("week5", rbo_Friday.Checked.ToString());
                p.Add("week6", rbo_Saturday.Checked.ToString());
                p.Add("week7", rbo_Sunday.Checked.ToString());
                p.Add("UserCode", Program.User);
                p.Add("enable", cbo_State.Text == "启用" ? "True" : "False");

                p.Add("DataTable1", DataTable1);
                p.Add("datelist", strtolist);

                return p;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private DataTable DataTableToDataGridView()
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;
            dc = dt.Columns.Add("设备编号");
            dc = dt.Columns.Add("设备名称");
            dc = dt.Columns.Add("存放位置");

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["设备编号"] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                dr["设备名称"] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                dr["存放位置"] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }
        /// <summary>
        /// 每年计算
        /// </summary>
        private List<string> EveryYear()
        {
            try
            {
                //计算时间范围相差多少年
                DateTime dtstart = dtp_StartTime.Value;
                DateTime dtclose = dtp_CloseTime.Value;
                int ys = dtclose.Year - dtstart.Year;
                List<string> YearList = new List<string>();
                if (rbo_FirstYear.Checked)
                {
                    #region 年初
                    for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                    {
                        if (dt.Month == 1 && dt.Day == 1)
                        {
                            YearList.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                        }
                    }
                    #endregion
                }// end if rbo_FirstYear.Checked
                else if (rbo_LastYear.Checked)
                {
                    #region 年末
                    for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                    {
                        if (dt.Month == 12 && dt.Day == 31)
                        {
                            YearList.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                        }
                    }
                    #endregion

                }//end if rbo_LastYear.Checked
                else if (rbo_DesYear.Checked)
                {
                    #region 指定年
                    int DisMonth = dtp_Year.Value.Month;
                    int DisDay = dtp_Year.Value.Day;
                    for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                    {
                        if (dt.Month == DisMonth && dt.Day == DisDay)
                        {
                            YearList.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                        }
                    }
                    #endregion

                }//end if rbo_DesYear.Checked
                return YearList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 每月计算
        /// </summary>
        /// <returns></returns>
        private List<string> EveryMoth()
        {
            try
            {
                int mt = dtp_CloseTime.Value.Month - dtp_StartTime.Value.Month;  //一共有多少个月
                int ys = dtp_CloseTime.Value.Year - dtp_StartTime.Value.Year;  //一共有多少个年
                List<string> MonthList = new List<string>();

                if (rbo_FirstMonth.Checked)
                {
                    #region 月初
                    for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                    {
                        if (dt.Day == 1)
                        {
                            MonthList.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                        }
                    }
                    #endregion


                } // end if rbo_FirstMonth.Checked
                else if (rbo_LastMonth.Checked)
                {
                    #region 月末
                    for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                    {
                        int MaxDay = DateTime.DaysInMonth(dt.Year, dt.Month);
                        if (dt.Day == MaxDay)
                        {
                            MonthList.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                        }
                    }
                    #endregion


                }  // end if rbo_LastMonth.Checked
                else if (rbo_DisMonth.Checked)
                {
                    #region  指定月
                    int DisDay = dtp_Month.Value.Day;
                    for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                    {
                        if (dt.Day == DisDay)
                        {
                            MonthList.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                        }
                    }
                    #endregion
                }

                return MonthList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 每周计算
        /// </summary>
        /// <returns></returns>
        private List<string> EveryWeek()
        {
            List<string> Mondaylist = new List<string>();
            List<string> Tuesdaylist = new List<string>();
            List<string> Wednesdaylist = new List<string>();
            List<string> Thursdaylist = new List<string>();
            List<string> Fridaylist = new List<string>();
            List<string> Saturdaylist = new List<string>();
            List<string> Sundaylist = new List<string>();


            try
            {
                for (DateTime dt = dtp_StartTime.Value; dt <= dtp_CloseTime.Value; dt = dt.AddDays(1))
                {
                    if (dt.DayOfWeek == DayOfWeek.Monday)
                    {
                        Mondaylist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        Tuesdaylist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        Wednesdaylist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Thursday)
                    {
                        Thursdaylist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Friday)
                    {
                        Fridaylist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        Saturdaylist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Sunday)
                    {
                        Sundaylist.Add(dt.ToShortDateString() + " " + dtp_Time.Value.ToString("HH:mm:ss"));
                    }
                }
                if (rbo_Monday.Checked)
                    return Mondaylist;
                else if (rbo_Tuesday.Checked)
                    return Tuesdaylist;
                else if (rbo_Wednesday.Checked)
                    return Wednesdaylist;
                else if (rbo_Thursday.Checked)
                    return Thursdaylist;
                else if (rbo_Friday.Checked)
                    return Fridaylist;
                else if (rbo_Saturday.Checked)
                    return Saturdaylist;
                else
                    return Sundaylist;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void AutoSizeColumn()
        {
            int width = 0;
            //使列自使用宽度
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dataGridView1.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dataGridView1.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dataGridView1.Size.Width)
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            //冻结某列 从左开始 0，1，2
            dataGridView1.Columns[1].Frozen = true;
        }

        private void btn_AddMech_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_PlanNo.Text.Trim()))
            {
                MessageBox.Show("请填写计划编号!");
                return;
            }
            string strwhere = string.Empty;   //设备筛选
            //if (dataGridView1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            strwhere += "WHERE machine_no!='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
            //        }else
            //        {
            //            strwhere += " AND machine_no!='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
            //        }
            //    }
            //}
            //加载设备信息
            //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
            string sql = @"SELECT rownum 行号,machine_no 设备编号,machine_name 设备名称,type 机械型号 FROM MES030M " + strwhere;
            //frmSearchData frmData = new frmSearchData(Program.Org, Program.WebServiceUrl, sql, false, false);
            //frmData.ShowDialog();

            //string ReturnData = frmData.ReturnDataXML;
            ////获取设备编号
           // List<string> strOn = GDSJ_Framework.Common.StringHelper.GetDataFromTag(ReturnData, "<设备编号>", "</设备编号>");
            List<string> strOn = new List<string>();
            FrmSelectData frmData = new FrmSelectData(sql, false, Program.Client);
            frmData.ShowDialog();
            if (frmData.RetData != null)
            {
                if (frmData.RetData.Rows.Count > 0)
                {
                    for (int i = 0; i < frmData.RetData.Rows.Count; i++)
                    {
                        strOn.Add(frmData.RetData.Rows[i]["设备编号"].ToString());

                    }
                    //txt_CopyBook.Text = manual_name;
                }
            }
            if (strOn.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("计划编号");
                dt.Columns.Add("设备编号");
                dt.Columns.Add("设备名称");
                dt.Columns.Add("存放位置");

                DataTable newdt = dt.Clone();
                for (int i = 0; i < strOn.Count; i++)
                {
                    sql = @"SELECT N'" + txt_PlanNo.Text.Trim() + "' as 计划编号 , machine_no 设备编号,machine_name 设备名称,address 存放位置 FROM MES030M WHERE machine_no='"+ strOn [i].ToString()+ "'";
                    DataTable DT_MES030M = Program.Client.GetDT(sql);// DB.GetDataTable(sql);
                    //DT_MES030M.Columns[0].ColumnName = "计划编号";
                    //DT_MES030M.Columns[1].ColumnName = "设备编号";
                    //DT_MES030M.Columns[2].ColumnName = "设备名称";
                    //DT_MES030M.Columns[3].ColumnName = "存放位置";
                    if (DT_MES030M.Rows.Count>0)
                    {
                        newdt.ImportRow(DT_MES030M.Rows[0]);
                    }
                    
                }

                //判断是否存在已有的设备
                //if (dataGridView1.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //    {
                //        for (int j = 0; j < newdt.Rows.Count; j++)
                //        {
                //            if (dataGridView1.Rows[i].Cells[1].Value.ToString() == newdt.Rows[j]["设备编号"].ToString())
                //            {
                //                MessageBox.Show("存在相同设备编号：" + newdt.Rows[j]["设备编号"] + "，不能重复添加！");
                //                return;
                //            }
                //        }
                //    }
                //}
                foreach (DataRow line in newdt.Rows)
                {
                    dataGridView1.Rows.Add(line.ItemArray);
                   
                }
            }
        }

        private void btn_DelMech_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows[0]!=null)
            {
                if (MessageBox.Show("是否删除该条数据", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                }
            }
            else
            {
                MessageBox.Show("请选择需要删除的行");
                return;
            }
            
               
        }


        /// <summary>
        /// 获取左边datagridview的显示数据
        /// </summary>
        private void GetLeftData()
        {
            //清空原有数据
            if (dgv_QueryData.Rows.Count > 0)
            {
                DataTable dt = (DataTable)dgv_QueryData.DataSource;
                dt.Rows.Clear();
                dgv_QueryData.DataSource = dt;
            }
            //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
            string leftsql = @"SELECT paln_no 计划编号,paln_name 计划名称 FROM MAC020M";
            //DataTable leftdt = DB.GetDataTable(leftsql);
            DataTable leftdt = Program.Client.GetDT(leftsql);//.GetDataTable(leftsql);

            if (leftdt.Rows.Count > 0)
            {
                dgv_QueryData.DataSource = leftdt;
                dgv_QueryData.Rows[0].Selected = false;
            }
        }
        string planNo = string.Empty;
        private void dgv_QueryData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex!=-1)
            {
                //获取设备编号
                txt_PlanNo.ReadOnly = true;
                string palnno = dgv_QueryData.Rows[e.RowIndex].Cells[0].Value.ToString();
                planNo = dgv_QueryData.Rows[e.RowIndex].Cells[0].Value.ToString();
                //清空原有数据
                if (dataGridView1.Rows.Count > 0)
                {
                    for (int i = dataGridView1.Rows.Count; i > 0; i--)
                    {
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows[i - 1].Index);
                    }
                }
                QueryData(palnno);
                //label1.Text = "修改";
            }
            
        }

        /// <summary>
        /// 查询数据
        /// <param name="planno">设备编号</param>
        /// </summary>
        private void QueryData(string palnno)
        {
            try
            {
                string querysql = @"SELECT * FROM MAC020M WHERE paln_no='" + palnno + "'";
                //DataTable dt = DB.GetDataTable(querysql);
                DataTable dt = Program.Client.GetDT(querysql); //DB.GetDataTable(querysql);                

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    txt_PlanNo.Text = dt.Rows[i]["paln_no"].ToString();
                    txt_PlanName.Text = dt.Rows[i]["paln_name"].ToString();
                    cbo_PlanType.Text = dt.Rows[i]["plan_type"].ToString();
                    txt_Book.Text = dt.Rows[i]["manual_no"].ToString();

                    dtp_StartTime.Value =!string.IsNullOrEmpty(dt.Rows[i]["date"].ToString())?Convert.ToDateTime(dt.Rows[i]["date"].ToString()): Convert.ToDateTime(dt.Rows[i]["date2"].ToString());
                    dtp_CloseTime.Value = Convert.ToDateTime(dt.Rows[i]["date2"].ToString());

                    rbo_EveryYear.Checked = Convert.ToBoolean(dt.Rows[i]["type_radio1"].ToString());
                    rbo_EveryMonth.Checked = Convert.ToBoolean(dt.Rows[i]["type_radio2"].ToString());
                    rbo_EveryWeek.Checked = Convert.ToBoolean(dt.Rows[i]["type_radio3"].ToString());
                    rbo_EveryDay.Checked = Convert.ToBoolean(dt.Rows[i]["type_radio4"].ToString());
                    rbo_FirstYear.Checked = Convert.ToBoolean(dt.Rows[i]["year_radio1"].ToString());
                    rbo_LastYear.Checked = Convert.ToBoolean(dt.Rows[i]["year_radio2"].ToString());
                    dtp_Year.Value = Convert.ToDateTime(dt.Rows[i]["year_md"].ToString() == "" ? DateTime.Now.ToString() : dt.Rows[i]["year_md"].ToString());
                    rbo_FirstMonth.Checked = Convert.ToBoolean(dt.Rows[i]["month_radio1"].ToString());
                    rbo_LastMonth.Checked = Convert.ToBoolean(dt.Rows[i]["month_radio2"].ToString());
                    rbo_DisMonth.Checked = Convert.ToBoolean(dt.Rows[i]["month_radio3"].ToString());
                    dtp_Month.Value = Convert.ToDateTime(dt.Rows[i]["month_day"].ToString() == "" ? DateTime.Now.ToString() : dt.Rows[i]["month_day"].ToString());
                    rbo_Monday.Checked = Convert.ToBoolean(dt.Rows[i]["week1"].ToString());
                    rbo_Tuesday.Checked = Convert.ToBoolean(dt.Rows[i]["week2"].ToString());
                    rbo_Wednesday.Checked = Convert.ToBoolean(dt.Rows[i]["week3"].ToString());
                    rbo_Thursday.Checked = Convert.ToBoolean(dt.Rows[i]["week4"].ToString());
                    rbo_Friday.Checked = Convert.ToBoolean(dt.Rows[i]["week5"].ToString());
                    rbo_Saturday.Checked = Convert.ToBoolean(dt.Rows[i]["week6"].ToString());
                    rbo_Sunday.Checked = Convert.ToBoolean(dt.Rows[i]["week7"].ToString());
                    dtp_Time.Value = Convert.ToDateTime(dt.Rows[i]["hours"].ToString());
                    if (dt.Rows[i]["enable"].ToString() == "True")
                        cbo_State.Text = "启用";
                    else
                        cbo_State.Text = "停用";


                    //加载设备明细数据
                    querysql = @"SELECT paln_no 计划编号,machine_no 设备编号,machine_name 设备名称,address 存放位置 FROM MAC020A1 WHERE paln_no='" + palnno + "'";
                    //dt = DB.GetDataTable(querysql);
                    dt = Program.Client.GetDT(querysql);
                    foreach (DataRow line in dt.Rows)
                    {
                        dataGridView1.Rows.Add(line.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());

            }

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            txt_PlanNo.ReadOnly = false;
            label1.Text = "";
            foreach (Control item in panel3.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }

            }
            dtp_StartTime.Value = DateTime.Now;
            dtp_CloseTime.Value = DateTime.Now;
            dtp_Year.Value = DateTime.Now;
            dtp_Month.Value = DateTime.Now;
            rbo_EveryYear.Checked = true;
            rbo_FirstYear.Checked = true;
            rbo_FirstMonth.Checked = false;
            rbo_LastMonth.Checked = false;
            rbo_DisMonth.Checked = false;
            rbo_Monday.Checked = false;
            rbo_Wednesday.Checked = false;
            rbo_Tuesday.Checked = false;
            rbo_Thursday.Checked = false;
            rbo_Friday.Checked = false;
            rbo_Saturday.Checked = false;
            rbo_Sunday.Checked = false;
            cbo_PlanType.Text = "";
            cbo_State.Text = "";

            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = dataGridView1.Rows.Count; i > 0; i--)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows[i - 1].Index);
                }
              
            }
            if(dgv_QueryData.Rows.Count>0)
            dgv_QueryData.Rows[0].Selected = false;
            label1.Text = "新增";
        }
        private void txt_PlanNo_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                this.dataGridView1[0, i].Value = txt_PlanNo.Text.Trim();
            }
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            string wherename = string.Empty;
            string strwhere = string.Empty;
            if (cbo_QueryName.Text.Trim() == "计划编号")
                wherename = "paln_no";
            else if (cbo_QueryName.Text.Trim() == "计划名称")
                wherename = "paln_name";
            if (!string.IsNullOrEmpty(cbo_QueryName.Text.Trim()) && !string.IsNullOrEmpty(txt_QueryVal.Text.Trim()))
            {
                strwhere = "WHERE " + wherename + " like'%" + txt_QueryVal.Text.Trim() + "%'";
            }

            //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);

            string sql = @"SELECT paln_no 计划编号,paln_name 计划名称 FROM MAC020M " + strwhere;
            //DataTable dt = DB.GetDataTable(sql);
            DataTable dt = Program.Client.GetDT(sql); //DB.GetDataTable(sql);
            dgv_QueryData.DataSource = dt;
        }

        private void txt_Book_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql = @"select rownum 行号,manual_no 编号,manual_name 手册名称 from MAC030M";
            FrmSelectData frmData = new FrmSelectData(sql, true, Program.Client);
            frmData.ShowDialog();
            if (frmData.RetData != null)
            {
                if (frmData.RetData.Rows.Count > 0)
                {
                    string manual_name = frmData.RetData.Rows[0]["手册名称"].ToString();  
                    txt_Book.Text = manual_name;
                }
            }
            //frmSearchData frmData = new frmSearchData(Program.Org, Program.WebServiceUrl, sql, true, true);
            //frmData.ShowDialog();

            //if (!string.IsNullOrEmpty(frmData.ReturnDataXML))
            //{
            //    string manual_name = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(frmData.ReturnDataXML, "<手册名称>", "</手册名称>");  //设备编号
            //    txt_Book.Text = manual_name;
            //}
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            label1.Text = "删除";
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            label1.Text = "修改";
        }
    }
}
