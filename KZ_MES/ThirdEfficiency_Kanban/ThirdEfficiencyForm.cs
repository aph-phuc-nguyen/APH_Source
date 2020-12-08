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

namespace ThirdEfficiency_Kanban
{
    public partial class ThirdEfficiencyForm : MaterialForm
    {
        DataTable dtJson = new DataTable();


        public ThirdEfficiencyForm()
        {
            InitializeComponent();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
            this.WindowState = FormWindowState.Maximized;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        private void ThirdEfficiencyForm_Load(object sender, EventArgs e)
        {
            SetUI();
            if (!string.IsNullOrEmpty(Interface.plant))
            {
                textBox1.Text = Interface.plant;
                dateTimePicker1.Text = Interface.date;
            }
            else
            {
                try
                {
                    string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Efficiency_KanbanServer", "QueryPlant", Program.Client.UserToken, string.Empty);
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                    {
                        string plant = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                        textBox1.Text = plant;
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                    }
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Efficiency_KanbanServer", "QueryWorkDate", Program.Client.UserToken, string.Empty);
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string date = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        dateTimePicker1.Text = date;
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
            GetData(dateTimePicker1.Text, textBox1.Text);
        }

        private void GetData(string  date,string plant)
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("date",date);
                p.Add("plant", plant);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Efficiency_KanbanServer", "ThirdEfficiency_Query", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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
                string work_qty = string.IsNullOrEmpty(dtJson.Rows[i]["work_qty"].ToString()) ? "0" : dtJson.Rows[i]["work_qty"].ToString();
                string label_qty = string.IsNullOrEmpty(dtJson.Rows[i]["label_qty"].ToString()) ? "0" : dtJson.Rows[i]["label_qty"].ToString();
                decimal result;
                if (!work_qty.Equals("0") && decimal.TryParse(work_qty, out result) && decimal.TryParse(label_qty, out result))
                {
                    acheivementQty = decimal.Parse(label_qty) / decimal.Parse(work_qty) * 100;
                    if (acheivementQty >= 100)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                    }
                    else if (acheivementQty >= 95 && acheivementQty < 100)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;

                    }
                    else if (acheivementQty < 95)
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

        private void button1_Click(object sender, EventArgs e)
        {
            GetData(dateTimePicker1.Text,textBox1.Text);
        }

        /// <summary>
        /// this is call other dll form demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {        
            Assembly assembly = null;
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            assembly = Assembly.LoadFrom(path + @"\" + "ThirdEfficiency_Kanban" + ".dll");
            Type type = assembly.GetType("ThirdEfficiency_Kanban.Interface");
            object instance = Activator.CreateInstance(type);
            MethodInfo mi = type.GetMethod("RunApp");
            object[] args = new object[1];
             args[0] = Program.Client;
            object obj = mi.Invoke(instance, args);
        }


        private void SetUI()
        {
            int fromHeight = this.Height;
            dataGridView1.ColumnHeadersHeight= Convert.ToInt32(fromHeight / 25);
            dataGridView1.RowTemplate.Height = Convert.ToInt32(fromHeight / 25);//datagridview行高
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("宋体", (float)fromHeight / 60, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //datagridview字体高度
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("宋体", (float)fromHeight / 55, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            GetData(dateTimePicker1.Text, textBox1.Text);
        }

        private void materialRaisedButton2_Click_1(object sender, EventArgs e)
        {
            GetData(dateTimePicker1.Text, textBox1.Text);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
            if (index > -1 && dataGridView1.Rows[index].Cells[0].Value != null)
            {
                Assembly assembly = null;
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
                assembly = Assembly.LoadFrom(path + @"\" + "Production_Kanban" + ".dll");
                Type type = assembly.GetType("Production_Kanban.Interface");
                object instance = Activator.CreateInstance(type);
                MethodInfo mi = type.GetMethod("RunCustomize");
                object[] args = new object[3];
                args[0] = Program.Client;
                args[1] = this.dataGridView1.Rows[index].Cells["Area"].Value;
                args[2] = dateTimePicker1.Text;
                object obj = mi.Invoke(instance, args);
            }
        }
    }
}
