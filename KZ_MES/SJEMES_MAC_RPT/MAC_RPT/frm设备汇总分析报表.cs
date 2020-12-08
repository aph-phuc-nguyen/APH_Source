
using SJEMS_SYS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SJEMES_MAC_RPT.MAC_RPT
{
    public partial class frm设备汇总分析报表 : Form
    {
        //GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

        public frm设备汇总分析报表(Dictionary<string, object> OBJ = null)
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
            Program.DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
        }

        private void btn_Find_Click(object sender, EventArgs e)
        {

            //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowReport));
            //thread.IsBackground = true;
            //thread.Start();
            ShowReport();
            tabControl1.Refresh();
        }
        public void ShowReport()
        {
            try
            {
                //if (this.InvokeRequired)
                //{
                //    this.Invoke(new MethodInvoker(delegate { ShowReport(); }));
                //    return;
                //}
                string path = string.Empty;
                string sql = string.Empty;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                Dictionary<string, string> dicParameter = new Dictionary<string, string>();
                if (tabControl1.SelectedIndex == 0)
                {
                    #region 校正计划
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        sql = @"SELECT
MES030M.machine_no as 设备编号,
MES030M.machine_name as 设备名称,
MES030M.work_state as 工作状态,
MES030M.owner as 负责人,
MES030A5.date_plan as 计划日期,
MES030A5.state as 处理状态,
MES030A5.date_fix as 校正日期,
MES030A5.person_fix as 校正人员,
MES030A5.memo_fix as 处理意见
FROM MES030A5
inner join MES030M
ON MES030A5.machine_no = MES030M.machine_no";
                    }
                    else
                    {
                        sql = @"SELECT 
MES030M.machine_no as 设备编号,
MES030M.machine_name as 设备名称,
MES030M.work_state as 工作状态,
MES030M.owner as 负责人,
MES030A5.date_plan as 计划日期,
MES030A5.state as 处理状态,
MES030A5.date_fix as 校正日期,
MES030A5.person_fix as 校正人员,
MES030A5.memo_fix as 处理意见
FROM MES030A5 
inner join MES030M 
ON MES030A5.machine_no=MES030M.machine_no
where MES030M.machine_no='" + textBox1.Text + "'";
                    }

                    //dic = new Dictionary<string, string>();//数据源键值对
                    //dic.Add("Table", sql);
                    path = Application.StartupPath + @"\FastReport" + "\\设备分析汇总报表-JZ.frx";

                    //dicParameter = new Dictionary<string, string>();//参数键值对
                    //dicParameter.Add("TabulatingPerson", Program.User);
                    //Dictionary<string, string> p = new Dictionary<string, string>();
                    //p.Add("sql", "SELECT ReportPath,ReportXML,ReportString FROM [SJEMSSYS].[dbo].[SYSREPORT01M] where ReportCode = 'Report_MES030A5'");
                    //string XML = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.WebServiceUrl, "SJEMS_API", "SJEMS_API.DataBase", "GetDataTable", p);
                    //string dtXML = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<RetData>", "</RetData>");
                    //var tab = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(dtXML);
                    //string fastReportXml = string.Empty;
                    //string connectionString = string.Empty;
                    //if (tab.Rows.Count > 0)
                    //{

                    //     fastReportXml = tab.Rows[0]["ReportString"].ToString();
                    //    //     connectionString = @"Data Source=" + Program.Org.DBServer + @";AttachDbFilename=;Initial Catalog=" + Program.Org.DBName + @";
                    //    //Integrated Security=False;Persist Security Info=False;User ID=" + Program.Org.DBUser + ";Password=" + Program.Org.DBPassword;
                    //    connectionString = "user id=" + Program.Org.DBUser + ";data source=" + Program.Org.DBName + ";password=" + Program.Org.DBPassword + "";

                    //}
                    //DataTable mainDt = Program.DB.GetDataTable(sql);
                    //DataTable dt = new DataTable();
                    ////if (!string.IsNullOrEmpty(fastReportXml))
                    //    //FastReportHelper.LoadFastReport_laoll(fastReportXml, connectionString, tabPage1, dic, dicParameter);

                    //FastReportHelper.LoadFastReport_laoll(tabPage1, path, dic, mainDt, "Table", dt, "count");

                    DataTable mainDt = Program.Client.GetDT(sql);
                    //MessageBox.Show(mainDt.Rows.Count.ToString());
                    if (mainDt.Rows.Count == 0)
                    {
                        sql = @"SELECT
count(*) aa,'' as 设备编号,''as 设备名称,'' as 工作状态,'' as 负责人,'' as 计划日期,'' as 处理状态,'' as 校正日期,''as 校正人员,'' as 处理意见 FROM MES030A5";
                        mainDt = Program.Client.GetDT(sql);
                    }
                    FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl();//创建报表控件
                    FastReport.Report report = new FastReport.Report();
                    tabPage1.Controls.Add(previewControl);//添加控件
                    previewControl.Size = tabPage1.Size;
                    report.Preview = previewControl;
                    report.Preview.Size = tabPage1.Size;
                    report.Preview.Anchor = tabControl1.Anchor;
                    report.Clear();
                    report.Load(path);
                    report.RegisterData(mainDt, "Table");
                    //FastReport.Data.TableDataSource tds = report.GetDataSource("Table") as FastReport.Data.TableDataSource;//设置表格数据源名称
                    //tds.Table = mainDt;
                    //tds.Init();
                    report.Prepare();
                    report.ShowPrepared(true);
                    //report.PrintSettings.ShowDialog = false;
                    //report.PrintSettings.Printer = comboBox1.Text;

                    //report.Print();
                    //report.Dispose();

                    //bool IsSetPrinterOK = SetDefaultPrinter(comboBox1.Text);

                    #endregion
                }
                else if(tabControl1.SelectedIndex == 1)
                {
                    #region 保养计划
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        sql = @"SELECT 
MES030M.machine_no as 设备编号,
MES030M.machine_name as 设备名称,
MES030M.work_state as 工作状态,
MES030M.owner as 负责人,
MES030A4.date_plan as 计划日期,
MES030A4.state as 处理状态,
MES030A4.date_fix as 校正日期,
MES030A4.person_fix as 校正人员,
MES030A4.memo_fix as 处理意见
FROM MES030A4 
inner join MES030M 
ON MES030A4.machine_no=MES030M.machine_no";
                    }
                    else
                    {
                        sql = @"SELECT 
MES030M.machine_no as 设备编号,
MES030M.machine_name as 设备名称,
MES030M.work_state as 工作状态,
MES030M.owner as 负责人,
MES030A4.date_plan as 计划日期,
MES030A4.state as 处理状态,
MES030A4.date_fix as 校正日期,
MES030A4.person_fix as 校正人员,
MES030A4.memo_fix as 处理意见
FROM MES030A4 
inner join MES030M 
ON MES030A4.machine_no=MES030M.machine_no
where MES030M.machine_no='" + textBox1.Text + "'";
                    }
                      

                    path = Application.StartupPath + @"\FastReport" + "\\设备分析汇总报表-BY.frx";

                    dic = new Dictionary<string, string>();//数据源键值对
                    dic.Add("Table", sql);
                   

                    dicParameter = new Dictionary<string, string>();//参数键值对
                    dicParameter.Add("TabulatingPerson", Program.User);

                    Dictionary<string, string> p = new Dictionary<string, string>();
                    p.Add("sql", "SELECT ReportPath,ReportXML,ReportString FROM [SJEMSSYS].[dbo].[SYSREPORT01M] where ReportCode = 'Report_MEC030A4'");
                    //string XML = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.WebServiceUrl, "SJEMS_API", "SJEMS_API.DataBase", "GetDataTable", p);
                    //string dtXML = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<RetData>", "</RetData>");
                    //var tab = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(dtXML);
                    //string fastReportXml = string.Empty;
                    //string connectionString = string.Empty;
                    //if (tab.Rows.Count > 0)
                    //{

                    //    fastReportXml = tab.Rows[0]["ReportString"].ToString();
                    //    connectionString = "user id=" + Program.Org.DBUser + ";data source=" + Program.Org.DBName + ";password=" + Program.Org.DBPassword + "";

                    //}
                    DataTable mainDt = Program.Client.GetDT(sql);
                    if (mainDt.Rows.Count == 0)
                    {
                        sql = @"SELECT
count(*) aa,'' as 设备编号,''as 设备名称,'' as 工作状态,'' as 负责人,'' as 计划日期,'' as 处理状态,'' as 校正日期,''as 校正人员,'' as 处理意见 FROM MES030A5";
                        mainDt = Program.Client.GetDT(sql);
                    }
                    FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl();//创建报表控件
                    FastReport.Report report = new FastReport.Report();
                    tabPage2.Controls.Add(previewControl);//添加控件
                    previewControl.Size = tabPage2.Size;

                    report.Preview = previewControl;
                    report.Preview.Size = tabPage2.Size;
                    report.Preview.Anchor = tabControl1.Anchor;

                    report.Clear();
                    report.Load(path);
                    report.RegisterData(mainDt, "Table");
                    report.Prepare();
                    report.ShowPrepared(true);
                    //if (!string.IsNullOrEmpty(fastReportXml))
                    //    FastReportHelper.LoadFastReport(fastReportXml, connectionString, tabPage2, dic, dicParameter);
                    #endregion
                }
                else if(tabControl1.SelectedIndex == 2)
                {
                    #region 维修详情
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        sql = @"select
MES030M.machine_name as 设备名称,
MES030M.work_state as 工作状态,
MES030M.owner as 负责人,
MES030A3.date_report AS 报修日期,
MES030A3.person_report AS 报修人员,
MES030A3.trouble AS 故障现象,
MES030A3.reason AS 故障原因,
MES030A3.date_fix as 修复日期,
MES030A3.person_fix as 修复人员
 from MES030A3 
INNER JOIN MES030M 
ON MES030A3.machine_no=MES030M.machine_no";
                    }
                    else
                    {
                        sql = @"select
MES030M.machine_name as 设备名称,
MES030M.work_state as 工作状态,
MES030M.owner as 负责人,
MES030A3.date_report AS 报修日期,
MES030A3.person_report AS 报修人员,
MES030A3.trouble AS 故障现象,
MES030A3.reason AS 故障原因,
MES030A3.date_fix as 修复日期,
MES030A3.person_fix as 修复人员
 from MES030A3 
INNER JOIN MES030M 
ON MES030A3.machine_no=MES030M.machine_no
where MES030M.machine_no='" + textBox1.Text + "'";
                    }
                       

                    path = Application.StartupPath + @"\FastReport" + "\\设备分析汇总报表-WX.frx";

                    dic = new Dictionary<string, string>();//数据源键值对
                    dic.Add("Table", sql);


                    dicParameter = new Dictionary<string, string>();//参数键值对
                    dicParameter.Add("TabulatingPerson", Program.User);

                    Dictionary<string, string> p = new Dictionary<string, string>();
                    p.Add("sql", "SELECT ReportPath,ReportXML,ReportString FROM[SJEMSSYS].[dbo].[SYSREPORT01M] where ReportCode = 'Report_MES030A3'");
                    //string XML = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.WebServiceUrl, "SJEMS_API", "SJEMS_API.DataBase", "GetDataTable", p);
                    //string dtXML = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<RetData>", "</RetData>");
                    //var tab = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(dtXML);
                    //string fastReportXml = string.Empty;
                    //string connectionString = string.Empty;
                    //if (tab.Rows.Count > 0)
                    //{

                    //    fastReportXml = tab.Rows[0]["ReportString"].ToString();
                    ////    connectionString = @"Data Source=" + Program.Org.DBServer + @";AttachDbFilename=;Initial Catalog=" + Program.Org.DBName + @";
                    ////Integrated Security=False;Persist Security Info=False;User ID=" + Program.Org.DBUser + ";Password=" + Program.Org.DBPassword;
                    //    connectionString = "user id=" + Program.Org.DBUser + ";data source=" + Program.Org.DBName + ";password=" + Program.Org.DBPassword + "";

                    //}
                    DataTable mainDt = Program.Client.GetDT(sql);
                    if (mainDt.Rows.Count == 0)
                    {
                        sql = @"SELECT
count(*) aa,'' as 设备名称,''as 工作状态,'' as 负责人,'' as 报修日期,'' as 报修人员,'' as 故障现象,'' as 故障原因,''as 修复日期,'' as 修复人员 FROM MES030A5";
                        mainDt = Program.Client.GetDT(sql);
                    }
                    FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl();//创建报表控件
                    FastReport.Report report = new FastReport.Report();
                    tabPage3.Controls.Add(previewControl);//添加控件
                    previewControl.Size = tabPage3.Size;

                    report.Preview = previewControl;
                    report.Preview.Size = tabPage3.Size;
                    report.Preview.Anchor = tabControl1.Anchor;

                    report.Clear();
                    report.Load(path);
                    report.RegisterData(mainDt, "Table");
                    report.Prepare();
                    report.ShowPrepared(true);
                    //if (!string.IsNullOrEmpty(fastReportXml))
                    //    FastReportHelper.LoadFastReport(fastReportXml, connectionString, tabPage3, dic, dicParameter);
                    #endregion
                }
                else if(tabControl1.SelectedIndex == 3)
                {
                    #region 校正计划图表
                    string[] arry = { "设备名称", "计划日期" };
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(arry);
                    comboBox2.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;

                    #endregion
                }else if(tabControl1.SelectedIndex == 4)
                {
                    string[] arry = { "设备名称", "计划日期" };
                    comboBox3.Items.Clear();
                    comboBox3.Items.AddRange(arry);
                    comboBox4.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                }else if (tabControl1.SelectedIndex == 5)
                {
                    string[] arry = { "设备名称", "故障原因" };
                    comboBox5.Items.Clear();
                    comboBox5.Items.AddRange(arry);
                    comboBox5.SelectedIndex = 0;
                    comboBox6.SelectedIndex = 0;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowReport));
            //thread.IsBackground = true;
            //thread.Start();
          ShowReport();
        }

        private void frm设备汇总分析报表_Load(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void btnShowCharts_Click(object sender, EventArgs e)
        {
            try
            {
                panel4.Controls.Clear();
                string sql = string.Empty;
                if (string.IsNullOrEmpty(textBox1.Text))
                    sql = @"select MES030M.machine_name as 设备名称,date_plan as 计划日期,count(MES030M.machine_name) as 计划数量 from MES030A4 
INNER JOIN MES030M ON MES030A4.machine_no=MES030M.machine_no
group by MES030M.machine_name,date_plan";
                else
                    sql = @"select MES030M.machine_name as 设备名称,date_plan as 计划日期,count(MES030M.machine_name) as 计划数量 from MES030A4  
INNER JOIN MES030M ON MES030A4.machine_no=MES030M.machine_no
where MES030M.machine_no='" + textBox1.Text + @"'
group by MES030M.machine_name,date_plan";
                Microsoft.Reporting.WinForms.ReportViewer reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
                reportViewer.Dock = DockStyle.Fill;
                var tab = Program.Client.GetDT(sql);
                Charts.DynamicChart(reportViewer, Charts.FilterFileds(tab, "计划数量", comboBox2.Text), comboBox1.SelectedIndex+1, comboBox2.Text, "计划数量");
                panel4.Controls.Add(reportViewer);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                panel5.Controls.Clear();
                string sql = string.Empty;
                if (string.IsNullOrEmpty(textBox1.Text))
                    sql = @"select MES030M.machine_name as 设备名称,date_plan as 计划日期,count(MES030M.machine_name) as 计划数量 from MES030A5 
INNER JOIN MES030M ON MES030A5.machine_no=MES030M.machine_no
group by MES030M.machine_name,date_plan";
                else
                    sql = @"select MES030M.machine_name as 设备名称,date_plan as 计划日期,count(MES030M.machine_name) as 计划数量 from MES030A5 
INNER JOIN MES030M ON MES030A5.machine_no=MES030M.machine_no
where MES030M.machine_no='" + textBox1.Text + @"'
group by MES030M.machine_name,date_plan";
                Microsoft.Reporting.WinForms.ReportViewer reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
                reportViewer.Dock = DockStyle.Fill;
                var tab = Program.Client.GetDT(sql);
                Charts.DynamicChart(reportViewer, Charts.FilterFileds(tab, "计划数量", comboBox3.Text), comboBox4.SelectedIndex + 1, comboBox3.Text, "计划数量");
                panel5.Controls.Add(reportViewer);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                panel7.Controls.Clear();
                string sql = string.Empty;
                if (string.IsNullOrEmpty(textBox1.Text))
                    sql = @"select MES030M.machine_name as 设备名称,MES030A3.reason AS 故障原因,count(MES030M.machine_name) as 计划数量 from MES030A3 
INNER JOIN MES030M ON MES030A3.machine_no=MES030M.machine_no
group by MES030M.machine_name,MES030A3.reason";
                else
                    sql = @"select MES030M.machine_name as 设备名称,MES030A3.reason AS 故障原因,count(MES030M.machine_name) as 计划数量 from MES030A3 
INNER JOIN MES030M ON MES030A3.machine_no=MES030M.machine_no
where MES030M.machine_no='" + textBox1.Text + @"'
group by MES030M.machine_name,MES030A3.reason";
                Microsoft.Reporting.WinForms.ReportViewer reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
                reportViewer.Dock = DockStyle.Fill;
                var tab = Program.Client.GetDT(sql);
                Charts.DynamicChart(reportViewer, Charts.FilterFileds(tab, "计划数量", comboBox5.Text), comboBox6.SelectedIndex + 1, comboBox5.Text, "计划数量");
                panel7.Controls.Add(reportViewer);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
