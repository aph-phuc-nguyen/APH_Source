using AutocompleteMenuNS;
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

namespace ProductionTargets
{
    public partial class PlanAdjustmentForm:MaterialForm
    {
        DataTable dataTable;
        DataTable workQtyDatatable;
        DataTable deptDatatable;
        private string seId;
        private string deptNo;
        private string workDate;
        private string routNo;

        public PlanAdjustmentForm()
        {
            InitializeComponent();
        }

        public PlanAdjustmentForm(string seId, string deptNo, string workDate,string rout_no)
        {
            InitializeComponent();
            this.seId = seId;
            this.deptNo = deptNo;
            this.workDate = workDate;
            this.routNo = rout_no;
            this.tet_se_id.Text = seId;
            this.text_d_dept.Text = deptNo;
            this.txt_work_day.Text = workDate;
            this.txtMoveDate.Text = DateTime.Now.ToString(Program.dateFormat);
            LoadMoveNo();
            LoadSeDept();
            Query();
        }


        public void LoadMoveNo()
        {
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.PlanAdjustmentServer", "LoadMoveNo", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                this.txt_move_no.Text = "MV_"+ deptNo+"_"+Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        public void LoadSeDept()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(350, 350);
            var columnWidth = new int[] { 50, 250 };
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", deptNo);
            p.Add("vRoutNo", routNo);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.PlanAdjustmentServer", "LoadSeDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                deptDatatable = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 1; i <= deptDatatable.Rows.Count; i++)
                {
                    autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { i + "", deptDatatable.Rows[i - 1]["DEPARTMENT_CODE"] + " " + deptDatatable.Rows[i - 1]["DEPARTMENT_NAME"] }, deptDatatable.Rows[i - 1]["DEPARTMENT_CODE"].ToString()) { ColumnWidth = columnWidth, ImageIndex = i });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        public void Query()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vDDept", deptNo);
            p.Add("vWrokDay", workDate);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.PlanAdjustmentServer", "QuerySize", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dataTable = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                this.dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dataTable.DefaultView;
                dataGridView1.Update();

            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            //没有输入组别
            if (string.IsNullOrWhiteSpace(text_new_dept_no.Text))
            {
                return;
            }
            //不能调整之前的
            if (DateTime.Parse(DateTime.Now.ToShortDateString())> DateTime.Parse(dt_new_work_day.Text.ToString()))
            {
                return;
            }
            //调拨数量不能大于计划数量-完工数量
            for (int i=0;i< dataTable.Rows.Count;i++)
            {
                decimal work_qty = decimal.Parse(dataTable.Rows[i]["work_qty"].ToString());
                decimal supplement_qty = decimal.Parse(dataTable.Rows[i]["supplement_qty"].ToString());
                decimal finish_qty = decimal.Parse(dataTable.Rows[i]["finish_qty"].ToString());
                if (!string.IsNullOrWhiteSpace(dataTable.Rows[i]["move_qty"].ToString()))
                {
                    try
                    {
                        decimal move_qty = decimal.Parse(dataTable.Rows[i]["move_qty"].ToString());
                        if (move_qty > (work_qty + supplement_qty - finish_qty))
                        {
                            return;
                        }
                    }
                    catch (Exception ex) {
                        return;
                    }
                }
        
            }
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vMoveNo",txt_move_no.Text);
            p.Add("vMoveDate",txtMoveDate.Text);
            p.Add("vSeId", seId);
            p.Add("vDDept", deptNo);
            p.Add("vWrokDay", workDate);
            p.Add("vInOut", "OUT");
            p.Add("vNewWorkDay", dt_new_work_day.Text);
            p.Add("vNewDdept", text_new_dept_no.Text);
            p.Add("dataTable", dataTable);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.PlanAdjustmentServer", "Save", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                this.Close();
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PlanAdjustmentForm_Load(object sender, EventArgs e)
        {
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
        }
    }
}
