using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace F_EAP_MachineLink_Collection
{
    public partial class F_EAP_CUT_INFO : MaterialForm
    {
        string cutID = "";
        string cutStatus = "";
        string status_description = "";
        string time_duration = "";

        public F_EAP_CUT_INFO()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        public F_EAP_CUT_INFO(string id)
        {
            cutID = id;
            InitializeComponent();
            textCutID.Text = cutID;
            this.WindowState = FormWindowState.Maximized;
        }

        private void F_EAP_CUT_INFO_Load(object sender, EventArgs e)
        {
            GetComboBoxUI();
            SetDataGridView();
            SetCutParm();
            SetCutRunTime();
            SetCutOutput();
            SetCutStatusTime();
            SetCutOee();
        }

        private void GetComboBoxUI()
        {
            List<ShiftEntry> shiftEntry = new List<ShiftEntry> { };

            shiftEntry.Add(new ShiftEntry() { Code = "", Name = "ALL|所有" });
            shiftEntry.Add(new ShiftEntry() { Code = "A", Name = "白班" });
            shiftEntry.Add(new ShiftEntry() { Code = "B", Name = "夜班" });

            this.comboOutShift.DataSource = shiftEntry;
            this.comboOutShift.DisplayMember = "Name";
            this.comboOutShift.ValueMember = "Code";

            this.Column7.DataSource = shiftEntry;
            this.Column7.DisplayMember = "Name";
            this.Column7.ValueMember = "Code";

            this.dataGridViewTextBoxColumn8.DataSource = shiftEntry;
            this.dataGridViewTextBoxColumn8.DisplayMember = "Name";
            this.dataGridViewTextBoxColumn8.ValueMember = "Code";

            List<ShiftEntry> routRate = new List<ShiftEntry> { };
            shiftEntry.ForEach(i => routRate.Add(i));
            this.comboRateShift.DataSource = routRate;
            this.comboRateShift.DisplayMember = "Name";
            this.comboRateShift.ValueMember = "Code";

            this.Column11.DataSource = routRate;
            this.Column11.DisplayMember = "Name";
            this.Column11.ValueMember = "Code";

            List<ShiftEntry> routRft = new List<ShiftEntry> { };
            shiftEntry.ForEach(i => routRft.Add(i));
            this.comboRftShift.DataSource = routRft;
            this.comboRftShift.DisplayMember = "Name";
            this.comboRftShift.ValueMember = "Code";

            this.dataGridViewTextBoxColumn11.DataSource = routRft;
            this.dataGridViewTextBoxColumn11.DisplayMember = "Name";
            this.dataGridViewTextBoxColumn11.ValueMember = "Code";

            List<ShiftEntry> routOee = new List<ShiftEntry> { };
            shiftEntry.ForEach(i => routOee.Add(i));
            this.comboOeeShift.DataSource = routRft;
            this.comboOeeShift.DisplayMember = "Name";
            this.comboOeeShift.ValueMember = "Code";

            this.dataGridViewComboBoxColumn1.DataSource = routRft;
            this.dataGridViewComboBoxColumn1.DisplayMember = "Name";
            this.dataGridViewComboBoxColumn1.ValueMember = "Code";
        }

        private void SetDataGridView()
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView3.AutoGenerateColumns = false;
            this.dgvOutM.AutoGenerateColumns = false;
            this.dgvOutD.AutoGenerateColumns = false;
            this.dgvRateM.AutoGenerateColumns = false;
            this.dgvRftM.AutoGenerateColumns = false;
            this.dgvOeeM.AutoGenerateColumns = false;
        }

        private void SetCutParm()
        {
            GetCutStatus();
            textCutDuration.Text = time_duration;
            textCutStatus.Text = status_description;
        }

        private void SetCutOutput()
        {
            GetCutHourOutput();
            GetCut7DaysOutput();
        }

        private void SetCutRunTime()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", cutID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "Query7DaysRunTime", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    draw7DaysRunTimeChart(dt);

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

        private void GetCutHourOutput()
        {
            try
            {
                string day = DateTime.Now.ToString("yyyyMMdd");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", cutID);
                p.Add("vDate", day);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutHourOutput", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    drawHoursChart(dt);
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

        private void GetCut7DaysOutput()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", cutID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCut7DaysOutput", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    chart7DaysOutput.Series.Clear();
                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][1] = dt.Rows[i][1].ToString().Substring(4, 2) + " / " + dt.Rows[i][1].ToString().Substring(6, 2); ;
                    }
                    chart7DaysOutput.DataBindTable(dt.DefaultView, "DAYS");
                    chart7DaysOutput.ChartAreas[0].AxisY.Minimum = 0;//设定y轴的最小值
                    chart7DaysOutput.Series[0].Label = "#VAL";
                    chart7DaysOutput.Series[0].Name = "产量";
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

        private void GetCutStatus()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", cutID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryEapStatusById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    if (dt.Rows.Count > 0)
                    {
                        cutStatus = dt.Rows[0]["TYPE_CODE"].ToString().Substring(0, 1);
                        status_description = dt.Rows[0]["STATUS"].ToString();
                        time_duration = dt.Rows[0]["DURATION"].ToString();
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

        private void SetCutStatusTime()
        {
            try
            {
                string day = DateTime.Now.ToString("yyyyMMdd");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", cutID);
                p.Add("vDate", day);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutDayStatusTime", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    drawStatusTimeChat(dt);
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

        private void SetCutOee()
        {
            try
            {
                string day = DateTime.Now.ToString("yyyyMMdd");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", cutID);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCut7DayOee", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    draw7daysOeeChat(dt);
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

        private void draw7daysOeeChat(DataTable dt)
        {
            chart7dayRunRate.Series.Clear();
            if (dt.Rows.Count == 0)
            {
                return;
            }
            Series series1 = new Series("OEE");
            chart7dayRunRate.Series.Add(series1);
            chart7dayRunRate.Series[0].ChartType = SeriesChartType.Line;
            chart7dayRunRate.Series[0].ToolTip = "#VAL" + "%";
            //chart7dayRunRate.Series[0].ToolTip = "#VALX : #VAL";
            chart7dayRunRate.Series[0].MarkerStyle = MarkerStyle.Circle;
            chart7dayRunRate.Series[0].MarkerSize = 8;

            chart7dayRunRate.DataSource = dt;
            chart7dayRunRate.Series[0].XValueMember = "DAYS";
            chart7dayRunRate.Series[0].YValueMembers = "OEE";
        }

        private void draw7DaysRunTimeChart(DataTable dt)
        {
            chartRunTime.Series.Clear();
            if (dt.Rows.Count == 0)
            {
                return;
            }
            chartRunTime.DataBindTable(dt.DefaultView, "DAYS");
            chartRunTime.ChartAreas[0].AxisY.Minimum = 0;//设定y轴的最小值
            chartRunTime.Series[0].Label = "#VAL";
            chartRunTime.Series[0].Name = "运行时间";
        }

        private void drawHoursChart(DataTable dt)
        {
            double cutDayQty = 0;
            double maxQty = 10;
            double minHour = 23;
            double maxHour = 0;
            chartHoursOutput.Series.Clear();
            Series series = new Series("时产量统计");
            chartHoursOutput.ChartAreas[0].AxisX.MajorGrid.Interval = 23;  //设置网格间隔（这里设成0.5，看得更直观一点）

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
                cutDayQty = cutDayQty + double.Parse(dt.Rows[i]["QTY"].ToString());
                series.Points.AddXY(double.Parse(dt.Rows[i]["HOURS"].ToString()), double.Parse(dt.Rows[i]["QTY"].ToString()));
                textCutDayOut.Text = cutDayQty.ToString();
            }

            chartHoursOutput.ChartAreas[0].AxisX.Maximum = maxHour + 0.9;//设定x轴的最大值
            chartHoursOutput.ChartAreas[0].AxisY.Maximum = maxQty;//设定y轴的最大值
            chartHoursOutput.ChartAreas[0].AxisX.Minimum = minHour - 1;//设定x轴的最小值
            chartHoursOutput.ChartAreas[0].AxisY.Minimum = 0;//设定y轴的最小值

            //把series添加到chart上
            chartHoursOutput.Series.Add(series);
            chartHoursOutput.Series[0].Label = "#VAL";
        }

        private void drawStatusTimeChat(DataTable dt)
        {
            chartStatusTime.Series.Clear();
            Series series = new Series("运行率");
            chartStatusTime.Series.Add(series);
            chartStatusTime.Series[0].ChartType = SeriesChartType.Pie;
            if (dt.Rows.Count == 0)
            {
                return;
            }
            List<string> Xvalue = new List<string>();
            List<double> Yvalue = new List<double>();

            double powerOnTime = 0;
            double runTime = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string type = dt.Rows[i]["TYPE"].ToString();
                double time = Math.Round(double.Parse(dt.Rows[i]["DURATION"].ToString()));
                Xvalue.Add(type);
                Yvalue.Add(double.Parse(dt.Rows[i]["DURATION"].ToString()));
                if ("运行".Equals(type))
                {
                    runTime = time;
                    textRunTime.Text = runTime.ToString();
                }
                else if ("空闲".Equals(type))
                {
                    textFreeTime.Text = time.ToString();
                }
                else if ("暂停".Equals(type))
                {
                    textPauseTime.Text = time.ToString();
                }
                else if ("故障".Equals(type))
                {
                    textFaultTime.Text = time.ToString();
                }
                else if ("其它".Equals(type))
                {
                    textOtherTime.Text = time.ToString();
                }
                powerOnTime = powerOnTime + time;
            }
            textPowerOnTime.Text = powerOnTime.ToString();

            series.Points.DataBindXY(Xvalue, Yvalue);
            series["PieLabelStyle"] = "Outside";
            series.Label = "#VALX;#PERCENT{P2}";  //VALX表示X轴的值，设置内容为百分比显示，P2为精确位数为两位小数
            series.LegendText = "#VALX";           

            double thisTime = System.DateTime.Now.TimeOfDay.TotalSeconds;
            double rate = 0;
            if (thisTime < 12.8 * 3600)
            {
                rate = Math.Round(runTime * 60 / (thisTime - 7.5 * 3600), 2);
            }
            else if (thisTime >= 12.8 * 3600 && thisTime < 13.8 * 3600)
            {
                rate = Math.Round(runTime * 60 / (13.8 * 3600 - 7.5 * 3600), 2);
            }
            else if (thisTime >= 13.8 * 3600 && thisTime < 19.5 * 3600)
            {
                rate = Math.Round(runTime * 60 / (thisTime - 7.5 * 3600 - 3600), 2);
            }
            else
            {
                rate = Math.Round(runTime * 60 / (10 * 3600), 2);
            }
            ucMeter1.Value = Convert.ToDecimal(rate);
        }

        private void butQueryRuntime_Click(object sender, EventArgs e)
        {
            string vDate = textRuntimeDate.Text.Replace("/","");
            string vMachineID = textMachineID.Text;
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", vMachineID);
                p.Add("vDate", vDate);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutRunTimeByDay", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    
                    dataGridView1.DataSource = dt.DefaultView;
                    dataGridView1.Update();
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView3.DataSource = null;
            string  vMachineID= "";
            string vDate = "";
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index > -1 && dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.CurrentRow.Index;
                vMachineID = dataGridView1.Rows[index].Cells[0].Value == null ? "" : dataGridView1.Rows[index].Cells[0].Value.ToString();
                vDate = dataGridView1.Rows[index].Cells[1].Value == null ? "" : dataGridView1.Rows[index].Cells[1].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", vMachineID);
                p.Add("vDate", vDate);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutRunTimeByDetail", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            string vDate = textOutDate.Text.Replace("/", "");
            string vMachineID = textIdOut.Text;
            string vShift = comboOutShift.SelectedValue.ToString(); ;
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", vMachineID);
                p.Add("vDate", vDate);
                p.Add("vShift", vShift);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutOutPutByDay", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    dgvOutM.DataSource = dt.DefaultView;
                    dgvOutM.Update();
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

        private void dgvOutM_SelectionChanged(object sender, EventArgs e)
        {
            this.dgvOutD.DataSource = null;
            string vMachineID = "";
            string vDate = "";
            string vShift = "";
            if (dgvOutM.CurrentRow != null && dgvOutM.CurrentRow.Index > -1 && dgvOutM.SelectedRows.Count > 0)
            {
                int index = dgvOutM.CurrentRow.Index;
                vMachineID = dgvOutM.Rows[index].Cells[0].Value == null ? "" : dgvOutM.Rows[index].Cells[0].Value.ToString();
                vDate = dgvOutM.Rows[index].Cells[2].Value == null ? "" : dgvOutM.Rows[index].Cells[2].Value.ToString();
                vShift = dgvOutM.Rows[index].Cells[3].Value == null ? "" : dgvOutM.Rows[index].Cells[3].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", vMachineID);
                p.Add("vDate", vDate);
                p.Add("vShift", vShift);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutOutPutByDetail", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dgvOutD.DataSource = dtJson.DefaultView;
                    dgvOutD.Update();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }

        private void btnRateQuery_Click(object sender, EventArgs e)
        {
            string vDate = textRateDate.Text.Replace("/", "");
            string vMachineID = textIdRate.Text;
            string vShift = comboRateShift.SelectedValue.ToString();
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", vMachineID);
                p.Add("vDate", vDate);
                p.Add("vShift", vShift);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutRateByDay", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    dgvRateM.DataSource = dt.DefaultView;
                    dgvRateM.Update();
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

        private void btnRftQuery_Click(object sender, EventArgs e)
        {
            string vDate = textRftDate.Text.Replace("/", "");
            string vMachineID = textIdRft.Text;
            string vShift = comboRftShift.SelectedValue.ToString();
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", vMachineID);
                p.Add("vDate", vDate);
                p.Add("vShift", vShift);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutRftByDay", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    dgvRftM.DataSource = dt.DefaultView;
                    dgvRftM.Update();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string vDate = textOeeDate.Text.Replace("/", "");
            string vMachineID = textIdOee.Text;
            //string vShift = comboRftShift.SelectedValue.ToString();
            string vShift = "A";
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", vMachineID);
                p.Add("vDate", vDate);
                p.Add("vShift", vShift);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryCutOeeByDay", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    dgvOeeM.DataSource = dt.DefaultView;
                    dgvOeeM.Update();
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textRuntimeDate.Text = dateTimePicker1.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            textOutDate.Text = dateTimePicker2.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            textRateDate.Text = dateTimePicker3.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            textRftDate.Text = dateTimePicker4.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            textOeeDate.Text = dateTimePicker5.Value.ToString("yyyy/MM/dd");
        }

        private void F_EAP_CUT_INFO_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        public class ShiftEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        
    }
}



