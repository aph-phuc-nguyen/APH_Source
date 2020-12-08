using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionDashBoard
{
    public partial class ProductionDashBoardForm : Form
    {
        public delegate void MyDelegate(string pageName);

        private void MyMehod(string pageName)
        {
            Type type;
            Object obj;
            string className=this.GetType().FullName;
            type = Type.GetType(className);
            obj = System.Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(pageName,new Type[] { });
            object[] parameters = null;
            method.Invoke(obj, parameters);
        }


        public ProductionDashBoardForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void ProductionDashBoardForm_Load(object sender, EventArgs e)
        {
            tabPage8_query();
            if (string.IsNullOrWhiteSpace(txtTimer.Text))
            {
                txtTimer.Text = "10";
            }
            Timer timer1 = new Timer();
            timer1.Interval = int.Parse(txtTimer.Text.ToString()) * 1000;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_Tick);//添加事件

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int index=tabControl1.SelectedIndex;
            if (index==tabControl1.TabCount-2)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            this.tabControl1.SelectedIndex = index;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyDelegate myDelegate = MyMehod;
            string pageName = this.tabControl1.SelectedTab.Name + "_query";
            myDelegate(pageName);
        }

       public void tabPage1_query()
       {
       }

        public void tabPage2_query()
       {
        }

        public void tabPage3_query()
       {
        }

        public void tabPage4_query()
       {
        }

        public void tabPage5_query()
        {
        }

        public void tabPage6_query()
        {
        }

        public void tabPage7_query()
        {

        }

        public void tabPage8_query()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.AutoGenerateColumns = false;
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }


        private void GetData()
        {
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionDashBoardServer", "Query", Program.Client.UserToken,string.Empty);
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
