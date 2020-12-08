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

namespace F_EAP_MachineLink_DataSet
{
    public partial class F_EAP_Contrast_Add : MaterialForm
    {
        public F_EAP_Contrast_Add()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string machine_id = textID.Text;
            string machine_no = textNO.Text;

            if (string.IsNullOrEmpty(machine_id))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请输入并确认设备ID");
                return;
            }
            if (string.IsNullOrEmpty(machine_no))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请输入并确认设备编号");
                return;
            }

            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", machine_id);
                p.Add("vMachineNO", machine_no);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "AddMachineContrast",Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    if ("B".Equals(json))
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, string.Format("添加失败！没有查询到ID为{0}的设备信息", machine_id));
                        return;
                    }
                    if ("C".Equals(json))
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, "添加失败！该设备ID已经关联过编号");
                        return;
                    }
                    if ("D".Equals(json))
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, "添加失败！该编号已经被此类设备使用");
                        return;
                    }

                    DialogResult result = MessageBox.Show("添加成功！", "提示");
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            
        }

        private void textMachineID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            textID.Text = "";
            if (string.IsNullOrEmpty(textMachineID.Text.Trim()))
            {
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                textID.Text = textMachineID.Text.Trim();
            }
        }

        private void textMachineNO_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            textNO.Text = "";
            if (string.IsNullOrEmpty(textMachineNO.Text.Trim()))
            {
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                textNO.Text = textMachineNO.Text.Trim();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textMachineID.Text = "";
            textMachineNO.Text = "";
            textID.Text = "";
            textNO.Text = "";
        }
    }
}
