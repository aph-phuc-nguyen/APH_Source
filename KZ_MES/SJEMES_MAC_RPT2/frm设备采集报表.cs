using GDSJ_Framework.WinForm.CommonForm;
using SJeMES_Control_Library.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SJEMES_MAC_RPT2
{
    public partial class frm设备采集报表 : Form
    {
        //GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

        public frm设备采集报表(Dictionary<string, object> OBJ = null)
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
            //Program.DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
        }
        private void LoadReportData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { LoadReportData(); }));
                return;
            }

            string path = string.Empty;
            string sql = string.Empty;
            string sql1 = string.Empty;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Dictionary<string, string> dicParameter = new Dictionary<string, string>();
            sql = @"select machine_no as 设备编号,machine_name as 设备名称,brand as 设备品牌,
    type as 设备类型,work_center as 工作中心,work_state as 工作状态,address as 存放位置,
    connect_state as 连接状态,owner  as 管理人员,pro_type as 属性类型 from MES030M where 1=1";
            sql1 = @"select date_input as 采集时间,MAC003A1.item_no as 内容项,field_id as 采集项,field_name as 采集名称,
    value as 采集内容,MES030A6.memo as 备注 from MES030A6 INNER JOIN MES030M ON MES030A6.machine_no = MES030M.machine_no
     INNER JOIN MAC003A1 ON MES030M.pro_type = MAC003A1.item_no";
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                sql += " AND machine_no = '" + textBox1.Text + "'";
            }
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                sql += " AND machine_name = '" + textBox2.Text + "'";
            }
            //dic = new Dictionary<string, string>();//数据源键值对
            dic.Add("Table", sql);
            dic.Add("Table2", sql1);
            //dicParameter = new Dictionary<string, string>();//参数键值对
            dicParameter.Add("TabulatingPerson", Program.User);
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("sql", "SELECT ReportPath,ReportXML,ReportString FROM[SJEMSSYS].[dbo].[SYSREPORT01M] where ReportCode = 'Report_MES030M'");
            //string XML = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.WebServiceUrl, "SJEMS_API", "SJEMS_API.DataBase", "GetDataTable", p);
            //string dtXML = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<RetData>", "</RetData>");
            //var tab = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(dtXML);
            //string fastReportXml = string.Empty;
            //string connectionString = string.Empty;
            //if (tab.Rows.Count > 0)
            //{

            //    fastReportXml = tab.Rows[0]["ReportString"].ToString();
            //    //connectionString = @"Data Source=" + Program.Org.DBServer + @";AttachDbFilename=;Initial Catalog=" + Program.Org.DBName + @";
            //    //    Integrated Security=False;Persist Security Info=False;User ID=" + Program.Org.DBUser + ";Password=" + Program.Org.DBPassword;
            //    connectionString = "user id=" + Program.Org.DBUser + ";data source=" + Program.Org.DBName + ";password=" + Program.Org.DBPassword + "";
            //}
            path = Application.StartupPath + @"\FastReport" + "\\设备数据采集报表.frx";


            DataTable mainDt = Program.Client.GetDT(sql);
            DataTable mainDt1 = Program.Client.GetDT(sql1);
            //DataTable mainDt = Program.DB.GetDataTable(sql);
            //DataTable mainDt1 = Program.DB.GetDataTable(sql1);
            if (mainDt1.Rows.Count==0)
                sql1 = @"select count(*) aa,' ' as 采集时间,' ' as 内容项,' ' as 采集项,'' as 采集名称,' ' as 采集内容,' ' as 备注 from MES030A6";
            mainDt1 = Program.Client.GetDT(sql1);
            //mainDt1 = Program.DB.GetDataTable(sql1);
            FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl();//创建报表控件
            FastReport.Report report = new FastReport.Report();
            panel2.Controls.Add(previewControl);//添加控件
            previewControl.Size = panel2.Size;
            report.Preview = previewControl;
            report.Preview.Size = panel2.Size;
            report.Preview.Anchor = panel2.Anchor;
            report.Clear();
            report.Load(path);
            report.RegisterData(mainDt, "Table");
            report.RegisterData(mainDt1, "Table2");           
            report.Prepare();
            report.ShowPrepared(true);
            //if (!string.IsNullOrEmpty(fastReportXml))
            //    FastReportHelper.LoadFastReport(fastReportXml, connectionString, panel2, dic, dicParameter);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadReportData();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            string sql = "select rownum 行号,machine_no 设备编号,machine_name 设备名称 from MES030M";
            //frmSearchData frmData = new frmSearchData(Program.Org, Program.WebServiceUrl, sql, true, true);
            //frmData.ShowDialog();
            //if (!string.IsNullOrEmpty(frmData.ReturnDataXML))
            //{
            //    string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(frmData.ReturnDataXML, "<设备编号>", "</设备编号>");  
            //    string machine_name = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(frmData.ReturnDataXML, "<设备名称>", "</设备名称>");
            //    textBox1.Text = machine_no;
            //    textBox2.Text = machine_name;
            //}
            FrmSelectData frmData = new FrmSelectData(sql, true, Program.Client);
            frmData.ShowDialog();
            if (frmData.RetData != null)
            {
                if (frmData.RetData.Rows.Count > 0)
                {
                    string machine_no = frmData.RetData.Rows[0]["设备编号"].ToString();
                    string machine_name = frmData.RetData.Rows[0]["设备名称"].ToString();
                    textBox1.Text = machine_no;
                    textBox2.Text = machine_name;

                }
            }
        }

    }
}
