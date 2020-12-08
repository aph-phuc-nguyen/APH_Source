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
    public partial class F_EAP_Oven_RealParam : MaterialForm
    {
        string ovenID = "";
        string ovenStatus = "";
        string status_description = "";

        string Oven_KWH = "";
        string Oven_SPEED = "";

        string Oven_UPER_SETUP1 = "";
        string Oven_UUCL1 = "";
        string Oven_ULCL1 = "";
        string Oven_UPER_ACTUAL1 = "";
        string Oven_LOWER_SETUP1 = "";
        string Oven_LUCL1 = "";
        string Oven_LLCL1 = "";
        string Oven_LOWER_ACTUAL1 = "";

        string Oven_UPER_SETUP2 = "";
        string Oven_UUCL2 = "";
        string Oven_ULCL2 = "";
        string Oven_UPER_ACTUAL2 = "";
        string Oven_LOWER_SETUP2 = "";
        string Oven_LUCL2 = "";
        string Oven_LLCL2 = "";
        string Oven_LOWER_ACTUAL2 = "";

        string Oven_UPER_SETUP3 = "";
        string Oven_UUCL3 = "";
        string Oven_ULCL3 = "";
        string Oven_UPER_ACTUAL3 = "";
        string Oven_LOWER_SETUP3 = "";
        string Oven_LUCL3 = "";
        string Oven_LLCL3 = "";
        string Oven_LOWER_ACTUAL3 = "";

        public F_EAP_Oven_RealParam()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        public F_EAP_Oven_RealParam(string id)
        {
            ovenID = id;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            textOvenID.Text = ovenID;
        }

        private void F_EAP_Oven_RealParam_Load(object sender, EventArgs e)
        {
            SetOvenParm();
            GetOvenHourOutput();
            GetOvenHourParams();
        }

        private void SetOvenParm()
        {
            GetOvenStatus();
            GetOvenParm();

            textOvenStatus.Text = status_description;

            if (!ovenStatus.Equals("C"))
            {
                textOvenUSet1.Text = Oven_UPER_SETUP1;
                textOvenUSet2.Text = Oven_UPER_SETUP2;
                textOvenUSet3.Text = Oven_UPER_SETUP3;
                textOvenLSet1.Text = Oven_LOWER_SETUP1;
                textOvenLSet2.Text = Oven_LOWER_SETUP2;
                textOvenLSet3.Text = Oven_LOWER_SETUP3;

                textOvenUAct1.Text = Oven_UPER_ACTUAL1;
                textOvenUAct2.Text = Oven_UPER_ACTUAL2;
                textOvenUAct3.Text = Oven_UPER_ACTUAL3;
                textOvenLAct1.Text = Oven_LOWER_ACTUAL1;
                textOvenLAct2.Text = Oven_LOWER_ACTUAL2;
                textOvenLAct3.Text = Oven_LOWER_ACTUAL3;

                textOvenSpeed.Text = Oven_SPEED;
                textOvenKwh.Text = Oven_KWH;
            }
            else
            {
                textOvenUSet1.Text = "";
                textOvenUSet2.Text = "";
                textOvenUSet3.Text = "";
                textOvenLSet1.Text = "";
                textOvenLSet2.Text = "";
                textOvenLSet3.Text = "";

                textOvenUAct1.Text = "";
                textOvenUAct2.Text = "";
                textOvenUAct3.Text = "";
                textOvenLAct1.Text = "";
                textOvenLAct2.Text = "";
                textOvenLAct3.Text = "";

                textOvenSpeed.Text = "0";
                textOvenKwh.Text = "0";
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
            //chart1.ChartAreas[0].AxisX.Maximum = 23;//设定x轴的最大值
            //chart1.ChartAreas[0].AxisY.Maximum = 120;//设定y轴的最大值
            //chart1.ChartAreas[0].AxisX.Minimum = 0;//设定x轴的最小值
            //chart1.ChartAreas[0].AxisY.Minimum = 0;//设定y轴的最小值
            chart1.ChartAreas[0].AxisY.Interval = 10; //设置Y轴每个刻度的跨度
            
            if (dt.Rows.Count == 0)
            {
                return;
            }
            //给系列上的点进行赋值，分别对应横坐标和纵坐标的值
            for (int i = 0; i <dt.Rows.Count; i++)
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
                    maxQty = Math.Ceiling(double.Parse(dt.Rows[i]["QTY"].ToString()) / 10)*10;
                }
                ovenDayQty = ovenDayQty + double.Parse(dt.Rows[i]["QTY"].ToString());
                series.Points.AddXY(double.Parse(dt.Rows[i]["HOURS"].ToString()), double.Parse(dt.Rows[i]["QTY"].ToString()));
                textOvenDayOut.Text = ovenDayQty.ToString();
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
            chart2.ChartAreas[0].AxisY.IsStartedFromZero = false;//Y轴坐标是否从0开始

            chart2.Series.Clear();

            Series series1 = new Series("1节上层温度");
            Series series2 = new Series("1节下层温度");
            Series series3 = new Series("2节上层温度");
            Series series4 = new Series("2节下层温度");
            Series series5 = new Series("3节上层温度");
            Series series6 = new Series("3节下层温度");
            chart2.Series.Add(series1);
            chart2.Series.Add(series2);
            chart2.Series.Add(series3);
            chart2.Series.Add(series4);
            chart2.Series.Add(series5);
            chart2.Series.Add(series6);

            for (int i = 0;i<6;i++)
            {
                chart2.Series[i].ChartType = SeriesChartType.Line;
                chart2.Series[i].ToolTip = "#VALX : #VAL";
                chart2.Series[i].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[i].MarkerSize = 4;
            }

            if (dt.Rows.Count == 0)
            {
                return;
            }

            ////给系列上的点进行赋值，分别对应横坐标和纵坐标的值
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chart2.Series[0].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["UPER_ACTUAL1"].ToString()));
                chart2.Series[1].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["LOWER_ACTUAL1"].ToString()));
                chart2.Series[2].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["UPER_ACTUAL2"].ToString()));
                chart2.Series[3].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["LOWER_ACTUAL2"].ToString()));
                chart2.Series[4].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["UPER_ACTUAL3"].ToString()));
                chart2.Series[5].Points.AddXY(DateTime.ParseExact(dt.Rows[i]["TIMES"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture), double.Parse(dt.Rows[i]["LOWER_ACTUAL3"].ToString()));
            }
        }

        private void GetOvenStatus()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", ovenID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryEapStatusById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    if (dt.Rows.Count > 0)
                    {
                        ovenStatus = dt.Rows[0]["TYPE_CODE"].ToString().Substring(0, 1);
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

        private void GetOvenParm()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", ovenID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryOvenParmById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    if (dt.Rows.Count > 0)
                    {
                        Oven_KWH = dt.Rows[0]["KWH"].ToString();
                        Oven_SPEED = dt.Rows[0]["SPEED"].ToString();

                        Oven_UPER_SETUP1 = dt.Rows[0]["UPER_SETUP1"].ToString();
                        Oven_UUCL1 = dt.Rows[0]["UUCL1"].ToString();
                        Oven_ULCL1 = dt.Rows[0]["ULCL1"].ToString();
                        Oven_UPER_ACTUAL1 = dt.Rows[0]["UPER_ACTUAL1"].ToString();
                        Oven_LOWER_SETUP1 = dt.Rows[0]["LOWER_SETUP1"].ToString();
                        Oven_LUCL1 = dt.Rows[0]["LUCL1"].ToString();
                        Oven_LLCL1 = dt.Rows[0]["LLCL1"].ToString();
                        Oven_LOWER_ACTUAL1 = dt.Rows[0]["LOWER_ACTUAL1"].ToString();

                        Oven_UPER_SETUP2 = dt.Rows[0]["UPER_SETUP2"].ToString();
                        Oven_UUCL2 = dt.Rows[0]["UUCL2"].ToString();
                        Oven_ULCL2 = dt.Rows[0]["ULCL2"].ToString();
                        Oven_UPER_ACTUAL2 = dt.Rows[0]["UPER_ACTUAL2"].ToString();
                        Oven_LOWER_SETUP2 = dt.Rows[0]["LOWER_SETUP2"].ToString();
                        Oven_LUCL2 = dt.Rows[0]["LUCL2"].ToString();
                        Oven_LLCL2 = dt.Rows[0]["LLCL2"].ToString();
                        Oven_LOWER_ACTUAL2 = dt.Rows[0]["LOWER_ACTUAL2"].ToString();

                        Oven_UPER_SETUP3 = dt.Rows[0]["UPER_SETUP3"].ToString();
                        Oven_UUCL3 = dt.Rows[0]["UUCL3"].ToString();
                        Oven_ULCL3 = dt.Rows[0]["ULCL3"].ToString();
                        Oven_UPER_ACTUAL3 = dt.Rows[0]["UPER_ACTUAL3"].ToString();
                        Oven_LOWER_SETUP3 = dt.Rows[0]["LOWER_SETUP3"].ToString();
                        Oven_LUCL3 = dt.Rows[0]["LUCL3"].ToString();
                        Oven_LLCL3 = dt.Rows[0]["LLCL3"].ToString();
                        Oven_LOWER_ACTUAL3 = dt.Rows[0]["LOWER_ACTUAL3"].ToString();
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

        private void GetOvenHourOutput()
        {
            try
            {
                string day = DateTime.Now.ToString("yyyyMMdd");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", ovenID);
                p.Add("vDate", day);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryOvenHourOutput", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private void GetOvenHourParams()
        {
            try
            {
                string max_time = DateTime.Now.ToString("yyyyMMddHHmmss");
                string min_time = DateTime.Now.AddDays(-0.0417).ToString("yyyyMMddHHmmss");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", ovenID);
                p.Add("vMaxTime", max_time);
                p.Add("vMinTime", min_time);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryOvenHourParams", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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
            SetOvenParm();
            GetOvenHourOutput();
            GetOvenHourParams();
        }

        private void F_EAP_Oven_RealParam_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.Dispose();
        }
    }
}
