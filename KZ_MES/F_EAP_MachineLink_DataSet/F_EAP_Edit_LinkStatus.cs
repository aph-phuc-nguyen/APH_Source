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
    public partial class F_EAP_Edit_LinkStatus : MaterialForm
    {
        string machine_id;
        string machine_name;
        string machine_dept;
        string whether_link;

        public F_EAP_Edit_LinkStatus()
        {
            InitializeComponent();
        }

        public F_EAP_Edit_LinkStatus(string machineId,string machineName,string machineDept,string whetherLink)
        {
            InitializeComponent();
            machine_id = machineId;
            machine_name = machineName;
            machine_dept = machineDept;
            whether_link = whetherLink;
        }

        private void F_EAP_Edit_LinkStatus_Load(object sender, EventArgs e)
        {
            GetComboBoxUI();
            textMachineID.Text = machine_id;
            textMachineName.Text = machine_name;
            textDept.Text = machine_dept;
            cbwhetherLink.SelectedValue = whether_link;
        }

        private void GetComboBoxUI()
        {
            List<WhetherEntry> whetherEntry = new List<WhetherEntry> { };

            whetherEntry.Add(new WhetherEntry() { Code = "Y", Name = "是" });
            whetherEntry.Add(new WhetherEntry() { Code = "N", Name = "否" });

            this.cbwhetherLink.DataSource = whetherEntry;
            this.cbwhetherLink.DisplayMember = "Name";
            this.cbwhetherLink.ValueMember = "Code";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认提交数据吗", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", machine_id);
                p.Add("vWhetherLink", cbwhetherLink.SelectedValue.ToString());
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "UpdateLinkedStatusById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    DialogResult result = MessageBox.Show("修改成功！", "提示");
                    if (result == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }

        public class WhetherEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }
        
    }
}
