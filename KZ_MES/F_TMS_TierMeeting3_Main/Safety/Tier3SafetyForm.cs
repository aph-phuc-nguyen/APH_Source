﻿using MaterialSkin.Controls;
using Newtonsoft.Json;
using SJeMES_Control_Library;
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

namespace TierMeeting
{
    public partial class Tier3SafetyForm : MaterialForm
    {
        private int days;
        private string txtAmount = "Amount";
        private string txtDepartment = "Department";
        private string txtAll = "All";
        private string dept = "";
        private int type = 0;
        private string date= "";
        private string month= "";
        private string plant= "";
        public Tier3SafetyForm(string date, string month, string plant)
        {
            this.date = date;
            this.month = month;
            this.plant = plant;
            InitializeComponent();
        }

        private void DetailSafety_Load(object sender, EventArgs e)
        {
            InitCombobox();
            InitUI();
        }
        private void InitUI() {
            gridData.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            cbxMonth.Size = new Size(173, 35);
            cbxLine.Size = new Size(173, 35);
            cbxPlant.Size = new Size(173, 35);
            cbxSection.Size = new Size(173, 35);
            cbxMonth.Size = new Size(173, 35);
            int height = base.Height;
            gridData.ColumnHeadersHeight = Convert.ToInt32(height / 25);
            gridData.RowTemplate.Height = Convert.ToInt32(height / 25);
            gridData.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", (float)height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            gridData.DefaultCellStyle.Font = new Font("宋体", (float)height / 55f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.WindowState = FormWindowState.Maximized;
        }
        private void GetData() {
            GetTableData();
        }
        private void GetTableData() { 
            GetTypeAndDept();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string tempMonth = (cbxMonth.Text.Length == 1) ? "0" + cbxMonth.Text : cbxMonth.Text;
            p.Add("firstDateOfMonth", date.ToDate().Year.ToString() + "-" + tempMonth + "-01");
            p.Add("dept", dept);
            p.Add("type", type);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetSafetyTable",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                UpdateDataTable(dtJson);
                GetChartData();
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void InitTable() {
            int year = DateTime.Now.Year;
            int month = int.Parse(cbxMonth.Text);
            days = DateTime.DaysInMonth(year, month);
            gridData.Columns.Add(txtDepartment, txtDepartment);
            gridData.Columns.Add(txtAmount, txtAmount);
            gridData.Columns[0].Width = 180;
            gridData.Columns[1].Width = 120;
            for (int day = 1; day <= days; day++)
            {
                DateTime temp = new DateTime(year, month, day);
                gridData.Columns.Add(day.ToString(), day.ToString());
                gridData.Columns[day + 1].Width = 40;
            }
        }
        private DataTable Pivot(DataTable dtJson) {
            string[] uniqueUnits = dtJson.AsEnumerable().Select(x => x.Field<string>("CODE")).Distinct().ToArray();

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("HAPPENEDDATE", typeof(string));
            foreach (string unit in uniqueUnits)
            {
                dt1.Columns.Add(unit, typeof(int));
            }
            var groups = dtJson.AsEnumerable().GroupBy(x => x.Field<string>("HAPPENEDDATE"));

            foreach (var group in groups)
            {
                DataRow newRow = dt1.Rows.Add();
                foreach (DataRow row in group)
                {
                    newRow["HAPPENEDDATE"] = group.Key;
                    newRow[row.Field<string>("CODE")] = row.Field<decimal>("COUNT");
                }
            }
            return dt1;
        }
        private void UpdateDataTable(DataTable dtJson) {
            ClearTable();
            InitTable();
            DataTable dtTemp = Pivot(dtJson);
            for (var i = 1; i < dtTemp.Columns.Count; i++) {
                gridData.Rows.Add();
                gridData.Rows[i - 1].Cells[0].Value = dtTemp.Columns[i].ToString();
                gridData.Rows[i - 1].Cells[1].Value = txtAmount;
                for (var j = 0; j < dtTemp.Rows.Count; j++)
                {
                    for (int day = 1; day <= days; day++)
                    {
                        DateTime dateTemp = DateTime.ParseExact(dtTemp.Rows[j][0].ToString(), "yyyy-MM-dd",
                            CultureInfo.InvariantCulture);
                        if (dateTemp.Day.ToString() == day.ToString())
                        {
                            gridData.Rows[i-1].Cells[day + 1].Value = (dtTemp.Rows[j][i].ToInt()).ToString();
                            if(dtTemp.Rows[j][i].ToInt() != 0) gridData.Rows[i - 1].Cells[day + 1].Style.BackColor = Color.Red;
                            else gridData.Rows[i - 1].Cells[day + 1].Style.BackColor = Color.Green;

                        }
                        else
                        {
                            if (gridData.Rows[i - 1].Cells[day + 1].Value == null)
                            {
                                gridData.Rows[i - 1].Cells[day + 1].Value = "0";
                                gridData.Rows[i - 1].Cells[day + 1].Style.BackColor = Color.Green;
                            }
                        }
                        bool flag = false;
                        DateTime tempYesterday = DateTime.Now.AddDays(-1);
                        flag = int.Parse(cbxMonth.Text) == tempYesterday.Month;
                        if (flag)
                        {
                            flag = day > tempYesterday.Day;
                        }
                        if (flag)
                        {
                            gridData.Rows[i - 1].Cells[day + 1].Value = null;
                            gridData.Rows[i - 1].Cells[day + 1].Style.BackColor = Color.White;
                        }
                    }
                }
            }
        }
        private void InitCombobox() {
            int month = DateTime.Now.AddDays(-1.0).Month;
            for (var i = 0; i < month; i++) {
                cbxMonth.Items.Add(i+1);
                if (i + 1 == date.ToDate().Month) {
                    cbxMonth.Text = (i+1).ToString();
                }
            }
            InitPlantCombobox();
        }
        private void ClearTable() {
            gridData.Rows.Clear();
            gridData.Columns.Clear();
        }
        private void GetChartData()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string tempMonth = (cbxMonth.Text.Length == 1) ? "0" + cbxMonth.Text : cbxMonth.Text;
            p.Add("firstDateOfMonth", date.ToDate().Year.ToString() + "-" + tempMonth + "-01");
            p.Add("dept", dept);
            p.Add("type", type);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetSafetyChart",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                InitChart(dtJson);
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void InitChart(DataTable dtJson)
        {
            //clear chart
            chartData.Series["Number of accidents"].Points.Clear();
            chartData.Titles.Clear();
            chartData.Series["Number of accidents"].IsValueShownAsLabel = false;
            //chartData.Series["Number of accidents"].Legend[0].;
            chartData.Legends.Clear();
            //init chart
            chartData.Titles.Add("Total Number of Accidents Everyday");
            int year = DateTime.Now.Year;
            int month = int.Parse(cbxMonth.Text);
            for (int day = 1; day <= days; day++)
            {
                bool flag = false;
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    DateTime dateTemp = DateTime.ParseExact(dtJson.Rows[i][0].ToString(), "yyyy-MM-dd",
                                CultureInfo.InvariantCulture);
                    if (day.ToString() == dateTemp.Day.ToString())
                    {
                        chartData.Series["Number of accidents"].Points.AddXY(day.ToString(), dtJson.Rows[i][1].ToInt());
                        flag = true;
                    }
                }
                if(!flag) chartData.Series["Number of accidents"].Points.AddXY(day.ToString(), 0);
            }
            var chartSeries = chartData.Series["Number of accidents"];
            var chartArea = chartData.ChartAreas[chartSeries.ChartArea];
            chartArea.CursorX.AutoScroll = true;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Interval = 1;
            chartArea.AxisX.Title = "Days";
            chartArea.AxisY.Title = "Amount";
        }
        private void InitPlantCombobox()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("parent", "is null");
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetDepartmentList",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                cbxPlant.Items.Add(txtAll);
                for (var i = 0; i < dtJson.Rows.Count; i++)
                {
                    cbxPlant.Items.Add(dtJson.Rows[i][0]);
                }
                
                EnableSectionCombobox(false);
                EnableLineCombobox(false);
                cbxPlant.Text = plant;
                GetData();
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void EnableSectionCombobox(bool temp)
        {
            cbxSection.Enabled = temp;
        }
        private void EnableLineCombobox(bool temp)
        {
            cbxLine.Enabled = temp;
        }
        private void FilterSection(string plant)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("parent", "= '" + plant + "'");
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetDepartmentList",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                cbxSection.Items.Clear();
                cbxSection.Items.Add(txtAll);
                for (var i = 0; i < dtJson.Rows.Count; i++)
                {
                    cbxSection.Items.Add(dtJson.Rows[i][0]);
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void FilterLine(string section)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("parent", "= '" + section + "'");
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetDepartmentList",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                cbxLine.Items.Clear();
                cbxLine.Items.Add(txtAll);
                for (var i = 0; i < dtJson.Rows.Count; i++)
                {
                    cbxLine.Items.Add(dtJson.Rows[i][0]);
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void cbxPlant_TextChanged(object sender, EventArgs e)
        {
            if (cbxPlant.Text == txtAll)
            {
                EnableSectionCombobox(false);
                EnableLineCombobox(false);
                cbxSection.Text = txtAll;
                cbxLine.Text = txtAll;
            }
            else
            {
                FilterSection(cbxPlant.Text);
                EnableSectionCombobox(true);
                cbxSection.Text = txtAll;
            }
        }
        private void cbxSection_TextChanged(object sender, EventArgs e)
        {
            if (cbxSection.Text == txtAll)
            {
                EnableLineCombobox(false);
                cbxLine.Text = txtAll;
            }
            else
            {
                FilterLine(cbxSection.Text);
                EnableLineCombobox(true);
                cbxLine.Text = txtAll;
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void GetTypeAndDept()
        {
            type = 0;
            if (cbxPlant.Text != txtAll)
            {
                if (cbxSection.Text != txtAll)
                {
                    if (cbxLine.Text != txtAll)
                    {
                        type = 3; //get by line
                        dept = cbxLine.Text;
                    }
                    else
                    {
                        type = 2; //get by section
                        dept = cbxSection.Text;
                    }
                }
                else
                {
                    type = 1; //get by plant
                    dept = cbxPlant.Text;
                }
            }
        }

        private void gridData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = this.gridData.CurrentRow.Index;
            bool flag = index > -1 && this.gridData.Rows[index].Cells[0].Value != null;
            if (flag)
            {
                string tempMonth = (cbxMonth.Text.Length == 1) ? "0" + cbxMonth.Text : cbxMonth.Text;
                string tempDate = date.ToDate().Year.ToString() + "-" + tempMonth + "-01";
                Tier2SafetyForm frm = new Tier2SafetyForm(tempDate, cbxMonth.Text, cbxPlant.Text, this.gridData.Rows[index].Cells[0].Value.ToString());
                frm.Show();
            }
        }
    }
}