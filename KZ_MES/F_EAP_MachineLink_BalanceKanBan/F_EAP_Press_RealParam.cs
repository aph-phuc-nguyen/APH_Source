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
    public partial class F_EAP_Press_RealParam : MaterialForm
    {
        string pressID = "";
        string pressStatus = "";
        string status_description = "";

        string press_KWH = "";



        public F_EAP_Press_RealParam()
        {
            InitializeComponent();
        }

        public F_EAP_Press_RealParam(string id)
        {
            pressID = id;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            textPressID.Text = pressID;
        }


        private void F_EAP_Press_RealParam_Load(object sender, EventArgs e)
        {
            SetPressParm();
            GetPressHourOutput();
            //GetPressHourParams();
        }

        private void SetPressParm()
        {
            GetPressStatus();
            //GetPressParm();
            textPressStatus.Text = status_description;

        }


        private void GetPressStatus()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", pressID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryEapStatusById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    if (dt.Rows.Count > 0)
                    {
                        pressStatus = dt.Rows[0]["TYPE_CODE"].ToString().Substring(0, 1);
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

        private void GetPressParm()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", pressStatus);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryFreezerParmById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    if (dt.Rows.Count > 0)
                    {
                        press_KWH = dt.Rows[0]["KWH"].ToString();
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

        private void GetPressHourOutput()
        {
            try
            {
                string day = DateTime.Now.ToString("yyyyMMdd");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", pressID);
                p.Add("vDate", day);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryPressHourOutput", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private void drawOutputChart(DataTable dt)
        {
            double pressDayQty = 0;
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
                pressDayQty = pressDayQty + double.Parse(dt.Rows[i]["QTY"].ToString());
                series.Points.AddXY(double.Parse(dt.Rows[i]["HOURS"].ToString()), double.Parse(dt.Rows[i]["QTY"].ToString()));
                textPressDayOut.Text = pressDayQty.ToString();
            }

            chart1.ChartAreas[0].AxisX.Maximum = maxHour + 0.9;//设定x轴的最大值
            chart1.ChartAreas[0].AxisY.Maximum = maxQty;//设定y轴的最大值
            chart1.ChartAreas[0].AxisX.Minimum = minHour - 1;//设定x轴的最小值
            chart1.ChartAreas[0].AxisY.Minimum = 0;//设定y轴的最小值

            //把series添加到chart上
            chart1.Series.Add(series);
        }

        private void F_EAP_Press_RealParam_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
