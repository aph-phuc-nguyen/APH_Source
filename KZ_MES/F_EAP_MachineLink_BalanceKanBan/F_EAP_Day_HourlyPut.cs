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

namespace F_EAP_MachineLink_BalanceKanBan
{
    public partial class F_EAP_Day_HourlyPut : MaterialForm
    {
        string d_dept = "";
        public F_EAP_Day_HourlyPut()
        {
            InitializeComponent();
        }

        public F_EAP_Day_HourlyPut(string dept)
        {
            d_dept = dept;
            InitializeComponent();
        }

        private void F_EAP_Day_HourlyPut_Load(object sender, EventArgs e)
        {
            GetEQPHourOutput();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = null;
            GetEQPHourOutput();
        }

        private void GetEQPHourOutput()
        {
            try
            {
                string day = dateTimePicker1.Value.ToString("yyyyMMdd");
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vDate", day);
                p.Add("vDept", d_dept);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryEqpHourOutput", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        
    }
}
