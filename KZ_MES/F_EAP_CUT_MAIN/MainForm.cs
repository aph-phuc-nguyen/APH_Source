using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace F_EAP_CUT_MAIN
{
    public partial class MainForm : MaterialForm
    {
        string query_day = "";
        DataTable contrastDt = null;

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                query_day = dateTimePicker1.Value.ToString("yyyyMMdd");
                BondMachineLinked();
                QueryOeeByDeptShift(query_day);
                QueryDayOutByDept(query_day);
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void BondMachineLinked()
        {
            contrastDt = GetCutContrastList();
            if (contrastDt.Rows.Count > 0)
            {
                foreach (Control label in tableLayoutPanel1.Controls)
                {
                    if (label.GetType().ToString() == "System.Windows.Forms.Label")
                    {
                        for (int i = 0; i < contrastDt.Rows.Count; i++)
                        {
                            if (label.Text.Equals(contrastDt.Rows[i]["machine_code"]))
                            {
                                label.Tag = contrastDt.Rows[i]["machine_id"].ToString();
                                label.BackColor = Color.MediumSeaGreen;
                                label.Click += new EventHandler(machineNo_Click);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void QueryOeeByDeptShift(string Day)
        { 
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDate", Day);
            p.Add("vDept", "FF01F01");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryOeeByDeptShift", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                drawRunTimeChart(dt);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void QueryDayOutByDept(string Day)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDate", Day);
            p.Add("vDept", "FF01F01");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryDayOutByDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                drawDayOutTimeChart(dt);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private DataTable GetCutContrastList()
        {
            DataTable tb = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vType", "CU");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "GetEapContrastListByType", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                tb = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return tb;
        }

        //点击机台编号
        private void machineNo_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string machine_id = label.Tag.ToString();

            Assembly assembly = null;
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            assembly = Assembly.LoadFrom(path + @"\" + "F_EAP_MachineLink_Collection" + ".dll");
            Type type = assembly.GetType("F_EAP_MachineLink_Collection.Interface");
            object instance = Activator.CreateInstance(type);

            MethodInfo mi = type.GetMethod("RunpCutRealParam");
            object[] args = new object[2];
            args[0] = Program.client;
            args[1] = machine_id;
            object obj = mi.Invoke(instance, args);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            query_day = dateTimePicker1.Value.ToString("yyyyMMdd");
            QueryOeeByDeptShift(query_day);
            QueryDayOutByDept(query_day);
        }

        private void drawRunTimeChart(DataTable dt)
        {
            chart1.Series.Clear();
            if (dt.Rows.Count == 0)
            {
                return;
            }
            Series series1 = new Series("A_RunTime");
            Series series2 = new Series("A_Oee");
            series2.ChartType = SeriesChartType.Line;
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            chart1.Series[1].YAxisType = AxisType.Secondary;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[1].MarkerSize = 10;
            chart1.ChartAreas[0].AxisY2.Minimum = 0;//设定y轴的最小值
            chart1.DataSource = dt;
            chart1.Series[0].XValueMember = "MACHINE_CODE";
            chart1.Series[0].YValueMembers = "DURATION";
            chart1.Series[1].YValueMembers = "OEE";
            chart1.Series[0].Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,134);
            chart1.Series[0].CustomProperties = "LabelStyle=Bottom";
            chart1.Series[0].Label = "#VAL";
            chart1.Series[1].ToolTip = "#VAL" + "%";
            //chart1.Series[1].Label = "#VAL";
            chart1.Series[0].Name = "运行时间(白班)";
            chart1.Series[1].Name = "OEE(白班)";
        }

        private void drawDayOutTimeChart(DataTable dt)
        {
            chart2.Series.Clear();
            if (dt.Rows.Count == 0)
            {
                return;
            }
            Series series1 = new Series("A_OUT");
            //Series series2 = new Series("B_OUT");
            chart2.Series.Add(series1);
            //chart2.Series.Add(series2);
            chart2.DataSource = dt;
            chart2.Series[0].XValueMember = "MACHINE_CODE";
            chart2.Series[0].YValueMembers = "A_QTY";
            //chart2.Series[1].YValueMembers = "B_QTY";
            chart2.Series[0].Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            //chart2.Series[1].Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            chart2.Series[0].Label = "#VAL";
            //chart2.Series[1].Label = "#VAL";
            chart2.Series[0].Name = "产量(白班)";
            //chart2.Series[1].Name = "产量(夜班)";
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.HitTestResult Result = new System.Windows.Forms.DataVisualization.Charting.HitTestResult();
            Result = chart1.HitTest((e as MouseEventArgs).X, (e as MouseEventArgs).Y);
            string machine_no = "";
            if (Result.PointIndex >= 0 && Result.Series != null)
                machine_no = Result.Series.Points[Result.PointIndex].AxisLabel;
            if (!string.IsNullOrEmpty(machine_no))
            {
                string machine_id = contrastDt.Select("MACHINE_CODE = '" + machine_no + "'")[0][1].ToString();

                Assembly assembly = null;
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
                assembly = Assembly.LoadFrom(path + @"\" + "F_EAP_MachineLink_Collection" + ".dll");
                Type type = assembly.GetType("F_EAP_MachineLink_Collection.Interface");
                object instance = Activator.CreateInstance(type);

                MethodInfo mi = type.GetMethod("RunpCutRealParam");
                object[] args = new object[2];
                args[0] = Program.client;
                args[1] = machine_id;
                object obj = mi.Invoke(instance, args);
            }
        }
    }
}

