﻿using MaterialSkin.Controls;
using Newtonsoft.Json;
using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace F_TMS_TierMeeting2_Main
{
    public partial class MaturityAssessmentForm : MaterialForm
    {
        private string dept = "";
        private string userPlant = "";
        private string userSection = "";
        private int type = 0;
        private DataTable dt;
        public MaturityAssessmentForm(string dept, int type, string userPlant, string userSection)
        {
            this.dept = dept;
            this.type = type;
            this.userPlant = userPlant;
            this.userSection = userSection;
            InitializeComponent();
        }
        private void MaturityAssessmentForm_Load(object sender, EventArgs e)
        {
            InitUI();
            if (!string.IsNullOrEmpty(dept)) {
                GetData();
            }
        }

        private void InitUI()
        {
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            this.WindowState = FormWindowState.Maximized;
            int height = base.Height;
            gridData.ColumnHeadersHeight = Convert.ToInt32(height / 25);
            gridData.RowTemplate.Height = Convert.ToInt32(height / 25);
            gridData.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", (float)height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            gridData.DefaultCellStyle.Font = new Font("宋体", (float)height / 55f, FontStyle.Regular, GraphicsUnit.Point, 134);
            gridData.ColumnHeadersVisible = false;
            gridData.AllowUserToAddRows = false;
            dtpDate.Size = new Size(173, 35);
            dtpDate.Font = new Font("宋体", 18f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = Parameters.dateFormat;
            dtpDate.Text = DateTime.Now.ToShortDateString();
            txtDepartment.Text = dept;
            txtDepartment.Size = new Size(173, 35);
            txtDepartment.Font = new Font("宋体", 18f, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblDepartment.Font = new Font("宋体", (float)height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
        }
        private void GetData() {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("deptCode", dept);
            p.Add("date", dtpDate.Text);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetMaturityAssessmentList",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                gridData.DataSource = dt;
                switch (Program.client.Language)
                {
                    case "en":
                        gridData.Columns["NAMEEN"].Visible = true;
                        gridData.Columns["NAMEEN"].HeaderText = lblNameText.Text;
                        break;
                    case "cn":
                        gridData.Columns["NAMECN"].Visible = true;
                        gridData.Columns["NAMECN"].HeaderText = lblNameText.Text;
                        break;
                    default:
                        gridData.Columns["NAMEYN"].Visible = true;
                        gridData.Columns["NAMEYN"].HeaderText = lblNameText.Text;
                        break;
                }
                gridData.ColumnHeadersVisible = true;
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dept)) {
                MessageHelper.ShowErr(this, SJeMES_Framework.Common.UIHelper.UImsg("tms-ma-err:00001", Program.client, "", Program.client.Language));
                return;
            }
            
            GetData();
            //EnableFilter(false);
            EnableSave(true);
        }
        private void EnableFilter(bool flag) {

            dtpDate.Enabled = flag;
        }
        private void EnableSave(bool flag)
        {
            btnSave.Enabled = flag;
        }

        private void SaveData() {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            List<string> listCode = new List<string>();
            List<string> listStatus = new List<string>();
            List<string> listNote = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                listCode.Add(row["CODE"].ToString());
                listStatus.Add(row["STATUS"].ToString());
                listNote.Add(row["NOTE"].ToString());
            }
            p.Add("date", DateTime.Now);
            p.Add("deptCode", dept);
            p.Add("listCode", listCode);
            p.Add("listStatus", listStatus);
            p.Add("listNote", listNote);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "SaveMaturityAssessment",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                MessageHelper.ShowSuccess(this, SJeMES_Framework.Common.UIHelper.UImsg("tms-ma-suc:00001", Program.client, "", Program.client.Language));
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            EnableFilter(true);
        }
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            EnableSave(false);
        }

        private void txtDepartment_DoubleClick(object sender, EventArgs e)
        {
            SelectDepartmentForm frm = new SelectDepartmentForm((int)Parameters.QueryDeptType.Line, userPlant, userSection);
            frm.DataChange += new SelectDepartmentForm.DataChangeHandler(DeptSelected);
            frm.ShowDialog();
        }
        private void DeptSelected(object sender, SelectDepartmentForm.DataChangeEventArgs args)
        {
            txtDepartment.Text = args.deptCode;
            dept = args.deptCode;
            type = args.deptType;
        }
    }
}
