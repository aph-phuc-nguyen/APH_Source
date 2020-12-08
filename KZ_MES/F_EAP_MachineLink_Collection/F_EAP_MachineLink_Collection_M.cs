using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_EAP_MachineLink_Collection
{
    public partial class F_EAP_MachineLink_Collection_M : MaterialForm
    {
        public F_EAP_MachineLink_Collection_M()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string machine_id = textID.Text;
            string factory = cbFactory.SelectedValue.ToString();
            string d_dept = textDept.Text;
            string machine_type = cbType.SelectedValue.ToString();

            GetMachineList(machine_id, factory, d_dept, machine_type);
        }

        private void GetComboBoxUI()
        {
            List<TypeEntry> typeEntries = new List<TypeEntry> { };
            List<FactoryEntry> factoryEntries = new List<FactoryEntry> { };

            typeEntries.Add(new TypeEntry() { Code = "ALL", Name = "ALL|所有" });
            factoryEntries.Add(new FactoryEntry() { Code = "ALL", Name = "ALL|所有" });

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "GetFactory", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    factoryEntries.Add(new FactoryEntry() { Code = dtJson.Rows[i]["CODE"].ToString(), Name = dtJson.Rows[i]["NAME"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

            string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "GetAllMachineType", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    typeEntries.Add(new TypeEntry() { Code = dtJson.Rows[i]["CODE"].ToString(), Name = dtJson.Rows[i]["NAME"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["ErrMsg"].ToString());
            }

            this.cbFactory.DataSource = factoryEntries;
            this.cbFactory.DisplayMember = "Name";
            this.cbFactory.ValueMember = "Code";

            this.cbFactory2.DataSource = factoryEntries;
            this.cbFactory2.DisplayMember = "Name";
            this.cbFactory2.ValueMember = "Code";

            this.cbType.DataSource = typeEntries;
            this.cbType.DisplayMember = "Name";
            this.cbType.ValueMember = "Code";
        }

        private void GetMachineList(string id, string factory, string dept, string type)
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", id);
                p.Add("vFactory", factory);
                p.Add("vDept", dept);
                p.Add("vType", type);
                p.Add("vWhetherLink", "Y");
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "QueryMachineList", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    dataGridView1.Rows.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        for(int i = 0;i < dt.Rows.Count;i++)
                        {
                            DataGridViewRow dr = new DataGridViewRow();
                            dr.CreateCells(dataGridView1);
                            dr.Cells[0].Value = dt.Rows[i]["MACHINE_NO"];
                            dr.Cells[1].Value = dt.Rows[i]["MACHINE_NAME"];
                            dr.Cells[2].Value = dt.Rows[i]["MACHINE_TYPE"];
                            dr.Cells[3].Value = dt.Rows[i]["ORG_CODE"];
                            dr.Cells[4].Value = dt.Rows[i]["ORG"];
                            dr.Cells[5].Value = dt.Rows[i]["DEPT"];
                            dr.Cells[6].Value = dt.Rows[i]["DEPARTMENT_NAME"];
                            dr.Cells[7].Value = "点击查看";
                            //dr.Cells[8].Value = "点击查看";
                            dataGridView1.Rows.Add(dr);
                        }
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

        private void GetDeptList(string factory, string dept)
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vFactory", factory);
                p.Add("vDept", dept);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "QueryDeptList", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    dataGridView2.Rows.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataGridViewRow dr = new DataGridViewRow();
                            dr.CreateCells(dataGridView2);
                            dr.Cells[0].Value = dt.Rows[i]["ORG_CODE"];
                            dr.Cells[1].Value = dt.Rows[i]["ORG"];
                            dr.Cells[2].Value = dt.Rows[i]["DEPT"];
                            dr.Cells[3].Value = dt.Rows[i]["DEPARTMENT_NAME"];
                            dr.Cells[4].Value = "点击查看线平衡";
                            dataGridView2.Rows.Add(dr);
                        }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (dataGridView1.Columns[e.ColumnIndex].Name == "ButDetail" && index >= 0)
            {
                if ("CU".Equals(dataGridView1.Rows[index].Cells[2].Value.ToString()))
                {
                    F_EAP_CUT_INFO frm = new F_EAP_CUT_INFO(this.dataGridView1.Rows[index].Cells[0].Value.ToString());
                    frm.ShowDialog();
                }
                else
                {
                    Assembly assembly = null;
                    string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
                    assembly = Assembly.LoadFrom(path + @"\" + "F_EAP_MachineLink_BalanceKanBan" + ".dll");
                    Type type = assembly.GetType("F_EAP_MachineLink_BalanceKanBan.Interface");
                    object instance = Activator.CreateInstance(type);

                    if ("OV".Equals(dataGridView1.Rows[index].Cells[2].Value.ToString()))
                    {
                        MethodInfo mi = type.GetMethod("RunOvenRealParam");
                        object[] args = new object[3];
                        args[0] = Program.client;
                        args[1] = this.dataGridView1.Rows[index].Cells[0].Value;
                        args[2] = this.dataGridView1.Rows[index].Cells[2].Value;
                        object obj = mi.Invoke(instance, args);
                    }
                    else if ("FR".Equals(dataGridView1.Rows[index].Cells[2].Value.ToString()))
                    {
                        MethodInfo mi = type.GetMethod("RunFreezerRealParam");
                        object[] args = new object[3];
                        args[0] = Program.client;
                        args[1] = this.dataGridView1.Rows[index].Cells[0].Value;
                        args[2] = this.dataGridView1.Rows[index].Cells[2].Value;
                        object obj = mi.Invoke(instance, args);
                    }
                    else if ("PR".Equals(dataGridView1.Rows[index].Cells[2].Value.ToString()))
                    {
                        MethodInfo mi = type.GetMethod("RunpRressRealParam");
                        object[] args = new object[3];
                        args[0] = Program.client;
                        args[1] = this.dataGridView1.Rows[index].Cells[0].Value;
                        args[2] = this.dataGridView1.Rows[index].Cells[2].Value;
                        object obj = mi.Invoke(instance, args);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string factory = cbFactory2.SelectedValue.ToString();
            string d_dept = textDept2.Text;
            GetDeptList( factory, d_dept);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (dataGridView2.Columns[e.ColumnIndex].Name == "ButBalance" && index >= 0)
            {
                Assembly assembly = null;
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
                assembly = Assembly.LoadFrom(path + @"\" + "F_EAP_MachineLink_BalanceKanBan" + ".dll");
                Type type = assembly.GetType("F_EAP_MachineLink_BalanceKanBan.Interface");
                object instance = Activator.CreateInstance(type);

                MethodInfo mi = type.GetMethod("RunBalanceKanban");
                object[] args = new object[3];
                args[0] = Program.client;
                args[1] = this.dataGridView2.Rows[index].Cells[2].Value;
                args[2] = "";
                object obj = mi.Invoke(instance, args);

            }
        }


        public class TypeEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        public class FactoryEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        private void F_EAP_MachineLink_Collection_M_Load(object sender, EventArgs e)
        {
            GetComboBoxUI();
        }
    }
}
