using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace AutoGenerationScheduling
{
    public partial class AutoGenerationSchedulingForm : MaterialForm
    {
        delegate void SetTextCallBack(string text, Color color);

        Thread thread = null;
        Thread thread1 = null;

        private void SetText(string text, Color color)
        {

            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { text, color });
            }
            else
            {
                this.richTextBox1.Focus();
                this.richTextBox1.Select(this.richTextBox1.TextLength, 0);
                this.richTextBox1.SelectionColor = color;
                this.richTextBox1.ScrollToCaret();
                this.richTextBox1.AppendText("\n" + text);
            }
        }

        private void SetTempText(string text, Color color)
        {

            if (this.richTextBox2.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetTempText);
                this.Invoke(stcb, new object[] { text, color });
            }
            else
            {
                this.richTextBox2.Focus();
                this.richTextBox2.Select(this.richTextBox2.TextLength, 0);
                this.richTextBox2.SelectionColor = color;
                this.richTextBox2.ScrollToCaret();
                this.richTextBox2.AppendText("\n" + text);
            }
        }

        public AutoGenerationSchedulingForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            //SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name,this,Program.client,"",Program.client.Language);
        }


        private void btn_start_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = DateTime.Now + "\t 启动线程中---->";
            thread = new Thread(new ThreadStart(Go));
            thread.Start();
        }

        public void Go()
        {
            while (true)
            {
                if (DateTime.Now.Hour.Equals(int.Parse(txtHours.Text.ToString())))
                {
                    SetText(DateTime.Now + "\t 自动生成排程数据开始---->", Color.FromName("Green"));
                    try
                    {
                        DataTable data = new DataTable();
                        string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.AutoGenerationSchedulingServer", "Query", Program.client.UserToken, string.Empty);
                        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                        {
                            string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                            data = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        }
                        else
                        {
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                        }

                        SetText(DateTime.Now + "\t 自动生成排程数据查询完成,---->查询到" + data.Rows.Count + "条数据\n", Color.FromName("Green"));
                        int count = data.Rows.Count;
                        int cishu =count / 10 + 1;
                        for (int i = 0; i < cishu; i++)
                        {
                            DataTable data1 = new DataTable();
                            data1 = data.Clone();
                            DataRow[] dr = data.Select(); // 克隆dt的结构，包括所有dt架构和约束，并无数据
                            int takenum = i * 10 + 10 < count ? 10 : count - i * 10;
                            for (int j= i * 10;j<i*10+ takenum;j++)
                            {
                                data1.ImportRow((DataRow)dr[j]);
                            }
                            Dictionary<string, Object> d1 = new Dictionary<string, object>();
                            if (data1.Rows.Count>0)
                            {
                                d1.Add("data", data1);
                                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.AutoGenerationSchedulingServer", "Insert", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d1));
                                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                                {
                                    SetText(DateTime.Now + "\t 自动生成排程数据结束,---->插入" + Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString() + "条数据\n", Color.FromName("Green"));
                                }
                                else
                                {
                                    SetText(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString() + "\n", Color.FromName("Red"));
                                    SendWX(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                                }
                            }   
                        }
                        SendWX(DateTime.Now + "\t 自动生成排程数据---->自动生成排程数据结束");
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            SetText(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException.InnerException + "\n", Color.FromName("Red"));
                            SendWX(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException.InnerException);
                        }
                        else
                        {
                            SetText(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException + "\n", Color.FromName("Red"));
                            SendWX(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException);
                        }
                        
                    }
                }
                SetText(DateTime.Now + "\t 休眠中----> \n", Color.FromName("Green"));
                Thread.Sleep(1000 * 60 * 60);
            }
        }

        private void SendWX(string mailInfo)
        {
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("data", mailInfo);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.AutoGenerationSchedulingServer", "SendWX", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                SetText(DateTime.Now + "\t 微信推送成功" + "\n", Color.FromName("Green"));
            }
            else
            {
                SetText(DateTime.Now + "\t 微信推送失败" + "\n", Color.FromName("Red"));
            }
        }
        private void btn_stop_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要关闭线程吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (thread != null)
                {
                    SetText(DateTime.Now + "\t 线程关闭---->\n", Color.FromName("Green"));
                    thread.Abort();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = DateTime.Now + "\t 把之前的排程拉过来---->";
            SetText(DateTime.Now + "\t 自动生成排程数据开始---->\n", Color.FromName("Green"));
            try
            {
                DataTable data = new DataTable();
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.AutoGenerationSchedulingServer", "Query_thread1", Program.client.UserToken, string.Empty);
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    data = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
                SetText(DateTime.Now + "\t 自动生成排程数据查询完成,---->查询到" + data.Rows.Count + "条数据\n", Color.FromName("Green"));
               
                int count = data.Rows.Count;
                int cishu = count / 10 + 1;
                for (int i = 0; i < cishu; i++)
                {

                    DataTable data1 = new DataTable();
                    data1 = data.Clone();
                    DataRow[] dr = data.Select(); // 克隆dt的结构，包括所有dt架构和约束，并无数据
                    int takenum = i * 10 + 10 < count ? 10 : count - i * 10;
                    for (int j = i * 10; j < i * 10 + takenum; j++)
                    {
                        data1.ImportRow((DataRow)dr[j]);
                    }
                    Dictionary<string, Object> d1 = new Dictionary<string, object>();
                    if (data1.Rows.Count > 0)
                    {
                        d1.Add("data", data1);
                        string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.AutoGenerationSchedulingServer", "Insert_thread1", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d1));
                        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                        {
                            SetText(DateTime.Now + "\t 自动生成排程数据结束,---->插入" + Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString() + "条数据\n", Color.FromName("Green"));
                        }
                        else
                        {
                            SetText(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString() + "\n", Color.FromName("Red"));
                            //SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                            SendWX(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                        }
                    }
                }
                SendWX(DateTime.Now + "\t 自动生成排程数据---->自动生成排程数据结束");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    SetText(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException.InnerException + "\n", Color.FromName("Red"));
                    SendWX(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException.InnerException);
                }
                else
                {
                    SetText(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException + "\n", Color.FromName("Red"));
                    SendWX(DateTime.Now + "\t 自动生成排程数据---->插入数据发生异常" + ex.InnerException);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
                SetText(DateTime.Now + "\t 线程关闭---->\n", Color.FromName("Green"));
            }

            DialogResult result = MessageBox.Show("确定要删除明天的滚动排程数据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
             if (result == DialogResult.Yes)
            {
                try
                {
                    string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.AutoGenerationSchedulingServer", "Delete", Program.client.UserToken, string.Empty);
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                    {
                        SetText(DateTime.Now + "\t 删除日排程数据完成,------------->", Color.FromName("Green"));
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                    }


                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        SetText(DateTime.Now + "\t 删除数据发生异常" + ex.InnerException.InnerException + "\n", Color.FromName("Red"));
                    }
                    else
                    {
                        SetText(DateTime.Now + "\t 删除数据发生异常" + ex.InnerException + "\n", Color.FromName("Red"));
                    }
                }
            }

            SetText(DateTime.Now + "\t 请重新开启线程！---->\n", Color.FromName("Green"));
        }

        private void AutoGenerationSchedulingForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHours.Text))
            {
                txtHours.Text = "20";
            }
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.richTextBox2.Text = DateTime.Now + "\t 启动线程中---->【这个是将ERP的投入、产出数据同步到MES DB。。。。】";
            thread1 = new Thread(new ThreadStart(TempGo));
            thread1.Start();
        }


        public void TempGo()
        {
            while (true)
            {
                if (DateTime.Now.Hour.Equals(int.Parse(txtHours.Text.ToString())))
                {
                    SetTempText(DateTime.Now + "\t 生产数据同步开始---->", Color.FromName("Green"));
                    try
                    {
                        string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.AutoGenerationSchedulingServer", "TempDataSynchronization", Program.client.UserToken, string.Empty);
                        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                        {
                            SetTempText(DateTime.Now + "\t 生产数据同步结束---->", Color.FromName("Green"));
                        }
                        else
                        {
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                        }
                        SendWX(DateTime.Now + "\t 生产数据同步开始---->生产数据同步结束");
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            SetTempText(DateTime.Now + "\t 生产数据同步--->插入数据发生异常" + ex.InnerException.InnerException + "\n", Color.FromName("Red"));
                            SendWX(DateTime.Now + "\t 生产数据同步---->插入数据发生异常" + ex.InnerException.InnerException);
                        }
                        else
                        {
                            SetTempText(DateTime.Now + "\t 生产数据同步---->插入数据发生异常" + ex.InnerException + "\n", Color.FromName("Red"));
                            SendWX(DateTime.Now + "\t 生产数据同步---->插入数据发生异常" + ex.InnerException);
                        }

                    }
                }
                SetTempText(DateTime.Now + "\t 休眠中----> \n", Color.FromName("Green"));
                Thread.Sleep(1000 * 60 * 60);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要关闭线程吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (thread1 != null)
                {
                    SetTempText(DateTime.Now + "\t 线程关闭---->\n", Color.FromName("Green"));
                    thread1.Abort();
                }
            }
        }

    }
}
 

