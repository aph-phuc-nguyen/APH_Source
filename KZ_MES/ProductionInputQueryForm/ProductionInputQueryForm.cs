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

namespace ProductionInputQuery
{
    public partial class ProductionInputQueryForm : MaterialForm
    {
        public Boolean isTitle = false;
        public class StatusEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        public class RoutEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        public ProductionInputQueryForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void ProductionInputQueryForm_Load(object sender, EventArgs e)
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(350, 350);
            //部门信息提示
            LoadDepts();
            //LoadPOs();
            //下拉框的多语言
            GetComboBoxUI();
            //多语言更新
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            this.dataGridView1.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
            this.dataGridView2.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
            this.dataGridView3.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };

        }

        private void LoadDepts()
        {
            var columnWidth = new int[] { 50, 250 };
            DataTable dt = GetDepts();
            int n = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt.Rows[i]["DEPARTMENT_CODE"].ToString() + " " + dt.Rows[i]["DEPARTMENT_NAME"].ToString() }, dt.Rows[i]["DEPARTMENT_CODE"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }

        private DataTable GetDepts()
        {
            DataTable dt = null;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetAllDepts", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return dt;
        }

        private void GetComboBoxUI()
        {
            List<StatusEntry> statusEntries = new List<StatusEntry> { };
            Dictionary<string, Object> p = new Dictionary<string, object>();
         
            p.Add("enmu_type", "statusEntries");
            string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ComboBoxUIServer", "GetComboBoxUI", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    statusEntries.Add(new StatusEntry() { Code = dtJson.Rows[i]["ENUM_CODE"].ToString(), Name = dtJson.Rows[i]["ENUM_VALUE"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["ErrMsg"].ToString());
            }
       
            this.comboBoxStatus.DataSource = statusEntries;
            this.comboBoxStatus.DisplayMember = "Name";
            this.comboBoxStatus.ValueMember = "Code";

            this.cbStatus.DataSource = statusEntries;
            this.cbStatus.DisplayMember = "Name";
            this.cbStatus.ValueMember = "Code";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView3.DataSource = null;
            this.dataGridView3.AutoGenerateColumns = false;
            Dictionary<string, Object> p = new Dictionary<string, object>();

            string vSeId = textBox5.Text.ToString();
            string vPO = textBox1.Text.ToString();
            string vDept = textBox6.Text.ToString().ToUpper();
            string vArt = textBox4.Text.ToString();

            p.Add("vSeId", vSeId);
            p.Add("vPO", vPO);
            p.Add("vDept", vDept);
            p.Add("vArt", vArt);
            p.Add("vInOut", "IN");
            p.Add("title", this.Text);

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionReportServer", "GetWorkDaySizeReport", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private void btn_query_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;
            this.dataGridView2.AutoGenerateColumns = false;

            Dictionary<string, Object> p = new Dictionary<string, object>();

            //string vSeId = textSeId.Text.ToString();
            //string vWrokDay = dtpWorkDay.Text.ToString();
            //string vDept = textDDept.Text.ToString().ToUpper();
            //string vPO = textPO.Text.ToString();
            //string vArt = textArt.Text.ToString();
            //string vStatus = comboBoxStatus.SelectedValue.ToString();
            //string vStatus = "7";

            p.Add("vSeId", textSeId.Text.ToString());
            p.Add("vWrokDay", dtpWorkDay.Text.ToString());
            p.Add("vDept", textDDept.Text.ToString().ToUpper());
            p.Add("vPO", textPO.Text.ToString());
            p.Add("vArt", textArt.Text.ToString());
            p.Add("vStatus", comboBoxStatus.SelectedValue.ToString());
            p.Add("vInOut", "IN");
            p.Add("title", this.Text);

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionReportServer", "GetWorkDayReport", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dataGridView2.DataSource = dtJson.DefaultView;
                dataGridView2.Update();

            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.AutoGenerateColumns = false;

            Dictionary<string, Object> p = new Dictionary<string, object>();

            string vSeId = textBox3.Text.ToString();
            string vPO = textBox8.Text.ToString();
            string vWrokDay = dateTimePicker1.Text.ToString();
            string vDept = textBox7.Text.ToString().ToUpper();
            string vArt = textBox2.Text.ToString();

            p.Add("vSeId", vSeId);
            p.Add("vPO", vPO);
            p.Add("vWrokDay", vWrokDay);
            p.Add("vDept", vDept);
            p.Add("vArt", vArt);
            p.Add("title", this.Text);

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionReportServer", "GetMesLableDReport", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dataGridView1.DataSource = dtJson.DefaultView;
                dataGridView1.Update();

            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
    }
}
