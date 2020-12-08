using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace F_EAP_MachineLink_BalanceKanBan
{
    public partial class F_EAP_Freezer_RealParam : MaterialForm
    {
        string freezerID = "";
        string freezerStatus = "";
        string status_description = "";

        string Freezer_KWH = "";
        string Freezer_SPEED = "";

        string Freezer_SETUP = "";
        string Freezer_UCL = "";
        string Freezer_LCL = "";
        string Freezer_ACTUAL = "";

        public F_EAP_Freezer_RealParam()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        public F_EAP_Freezer_RealParam(string id)
        {
            freezerID = id;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            textFreezerID.Text = freezerID;
        }

        private void F_EAP_Freezer_RealParam_Load(object sender, EventArgs e)
        {
            SetFreezerParm();
            GetFreezerHourOutput();
            GetFreezerHourParams();
        }

        private void SetFreezerParm()
        {
            GetFreezerStatus();
            GetFreezerParm();
            textFreezerStatus.Text = status_description;

            if (!freezerStatus.Equals("C"))
            {
                textFreezerSet.Text = Freezer_SETUP;
                textFreezerAct.Text = Freezer_ACTUAL;

                textFreezerSpeed.Text = Freezer_SPEED;
                textFreezerKwh.Text = Freezer_KWH;
            }
            else
            {
                textFreezerSet.Text = "";
                textFreezerAct.Text = "";

                textFreezerSpeed.Text = "0";
                textFreezerKwh.Text = "0";
            }
        }

        private void drawOutputChart(DataTable dt)
        {
            double ovenDayQty = 0;
            double maxQty = 10;
            double minHour = 23;
            double maxHour = 0;
            chart1.Series.Clear();
            Series series = new Series("时产量统计");
            series.IsValueShownAsLabel = true; //把值当做标签展示（默认false）
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 23;  //设置网格间隔（这里设成0.5，看得更直观一点）
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Interval = 10; //设置Y轴每个刻度的跨度

            if (dt.Rows.Count == 0)
            {
                return;
            }
            //给系列上的点进行赋值，分别对应横坐标和纵坐标的值
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (double.Parse(dt.Rows[i]["HOURS"].ToString()) < minHour)
                {
                    minHour = double.Parse(dt.Rows[i]["HOURS"].ToString());
                }
                if (double.Parse(dt.Rows[i]["HOURS"].ToString()) > maxHour)
                {
                    maxHour = double.Parse(dt.Rows[i]["HOURS"].ToString());
                }
                if (double.Parse(dt.Rows[i]["QTY"].ToString()) > maxQty)
                {
                    maxQty = Math.Ceiling(double.Parse(dt.Rows[i]["QTY"].ToString()) / 10) * 10;
                }
                ovenDayQty = ovenDayQty + double.Parse(dt.Rows[i]["QTY"].ToString());
                series.Points.AddXY(double.Parse(dt.Rows[i]["HOURS"].ToString()), double.Parse(dt.Rows[i]["QTY"].ToString()));
                textFreezerDayOut.Text = ovenDayQty.ToString();
            }

            chart1.ChartAreas[0].AxisX.Maximum = maxHour + 0.9;//设定x轴的最大值
            chart1.ChartAreas[0].AxisY.Maximum = maxQty;//设定y轴的最大值
            chart1.ChartAreas[0].AxisX.Minimum = minHour - 1;//设定x轴的最小值
            chart1.ChartAreas[0].AxisY.Minimum = 0;//设定y轴的最小值

            //把series添加到chart上
            chart1.Series.Add(series);
        }

        private void drawParamChart(DataTable dt)
        {
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            //chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 23;  //设置网格间隔（这里设成0.5，看得更直观一点）
            chart2.ChartAreas[0].AxisX.Interval = 0.006;
            chart2.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();//设定x轴的最大值
            //chart2.ChartAreas[0].AxisY.Maximum = 110;//设定y轴的最大值
            chart2.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddDays(-0.0417).ToOADate();//设定x轴的最小值
            //chart2.ChartAreas[0].AxisY.Minimum = 50;//设定y轴的最小值
            chart2.ChartAreas[0].AxisY.Interval = 10; //设置Y轴每个刻度的跨度

            chart2.Series.Clear();

            Series series1 = new Series("实际温度");
            Series series2 = new Series("设定温度");
            Series series3 = new Series("温度控制上限");
            Series series4 = new Series("温度控制下限");

            chart2.Series.Add(series1);
            chart2.Series.Add(series2);
            chart2.Series.Add(series3);
            chart2.Series.Add(series4);

            chart2.Series[0].ChartType = SeriesChartType.Line;
            chart2.Series[1].ChartType = SeriesChartType.Line;
            chart2.Series[2].ChartType = SeriesChartType.Line;
            chart2.Series[3].ChartType = SeriesChartType.Line;

            chart2.Series[2].BorderDashStyle = ChartDashStyle.Dash;
            chart2.Series[3].BorderDashStyle = ChartDashStyle.Dash;

            chart2.Series[0].ToolTip = "#VALX : #VAL";
            chart2.Series[0].MarkerStyle = MarkerStyle.Circle;
            chart2.Series[0].MarkerSize = 4;

            if (dt.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chart2.Series[0].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["ACTUAL"].ToString()));
                chart2.Series[1].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["SETUP"].ToString()));
                chart2.Series[2].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["UCL"].ToString()));
                chart2.Series[3].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["LCL"].ToString()));
            }
        }

        private void GetFreezerStatus()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", freezerID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryEapStatusById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    if (dt.Rows.Count > 0)
                    {
                        freezerStatus = dt.Rows[0]["TYPE_CODE"].ToString().Substring(0, 1);
                        status_description = dt.Rows[0]["STATUS"].ToString();
                    }
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void GetFreezerParm()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", freezerID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryFreezerParmById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    if (dt.Rows.Count > 0)
                    {
                        Freezer_KWH = dt.Rows[0]["KWH"].ToString();
                        Freezer_SPEED = dt.Rows[0]["SPEED"].ToString();

                        Freezer_SETUP = dt.Rows[0]["SETUP"].ToString();
                        Freezer_UCL = dt.Rows[0]["UCL"].ToString();
                        Freezer_LCL = dt.Rows[0]["LCL"].ToString();
                        Freezer_ACTUAL = dt.Rows[0]["ACTUAL"].ToString();
                    }
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void GetFreezerHourOutput()
        {
            try
            {
                string day = DateTime.Now.ToString("yyyyMMdd");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", freezerID);
                p.Add("vDate", day);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryFreezerHourOutput", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    drawOutputChart(dt);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void GetFreezerHourParams()
        {
            try
            {
                string max_time = DateTime.Now.ToString("yyyyMMddHHmmss");
                string min_time = DateTime.Now.AddDays(-0.0417).ToString("yyyyMMddHHmmss");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", freezerID);
                p.Add("vMaxTime", max_time);
                p.Add("vMinTime", min_time);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryFreezerHourParams", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    drawParamChart(dt);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetFreezerParm();
            GetFreezerHourOutput();
            GetFreezerHourParams();
        }

        private void F_EAP_Freezer_RealParam_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.Dispose();
        }
    }
}
