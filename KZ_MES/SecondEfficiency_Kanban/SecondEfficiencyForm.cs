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

namespace SecondEfficiency_Kanban
{
    public partial class SecondEfficiencyForm:MaterialForm
    {
        DataTable dtJson = new DataTable();
        public SecondEfficiencyForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
        }

        private void SecondEfficiencyForm_Load(object sender, EventArgs e)
        {
            SetUI();
            if (!string.IsNullOrEmpty(Interface.MyArgs))
            {
                dateTimePicker2.Text = Interface.MyArgs;
            }
            else
            {
                try
                {
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Efficiency_KanbanServer", "QueryWorkDate", Program.Client.UserToken,string.Empty);
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string date = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        dateTimePicker2.Text = date;
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
            GetData(dateTimePicker2.Text);
        }

        private void GetData(string date)
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("date", date);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Efficiency_KanbanServer", "SecondEfficiency_Query", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView1.DataSource = dtJson;
                    dataGridView1.Update();
                    dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView1_RowPostPaint);
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


        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            for (int i = 0; i < dtJson.Rows.Count; i++)
            {
                decimal acheivementQty = 0;
                string work_qty = string.IsNullOrEmpty(dtJson.Rows[i]["work_qty"].ToString())?"0":dtJson.Rows[i]["work_qty"].ToString();
                string label_qty = string.IsNullOrEmpty(dtJson.Rows[i]["label_qty"].ToString()) ?"0":dtJson.Rows[i]["label_qty"].ToString();
                decimal result;
                if (!work_qty.Equals("0")&&decimal.TryParse(work_qty, out result)&& decimal.TryParse(label_qty, out result))
                {
                    acheivementQty =decimal.Parse(label_qty)/decimal.Parse(work_qty)*100;
                    if (acheivementQty >= 100)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                    }
                    else if (acheivementQty >=95 && acheivementQty <100)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;

                    }
                    else if (acheivementQty <95)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                else
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Aqua;
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            GetData(dateTimePicker2.Text);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
            if (index > -1 && dataGridView1.Rows[index].Cells[0].Value != null)
            {
                Assembly assembly = null;
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
                assembly = Assembly.LoadFrom(path + @"\" + "ThirdEfficiency_Kanban" + ".dll");
                Type type = assembly.GetType("ThirdEfficiency_Kanban.Interface");
                object instance = Activator.CreateInstance(type);
                MethodInfo mi = type.GetMethod("RunCustomize");
                object[] args = new object[3];
                args[0] = Program.Client;
                args[1] = this.dataGridView1.Rows[index].Cells["code"].Value;
                args[2] = dateTimePicker2.Text;
                object obj = mi.Invoke(instance, args);
            }
        }

        private void SetUI()
        {
            int fromHeight = this.Height;
            dataGridView1.ColumnHeadersHeight = Convert.ToInt32(fromHeight / 25);
            dataGridView1.RowTemplate.Height = Convert.ToInt32(fromHeight / 25);//datagridview行高
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("宋体", (float)fromHeight / 60, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //datagridview字体高度
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("宋体", (float)fromHeight / 55, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            GetData(dateTimePicker2.Text);
        }
    }
}
