using MaterialSkin.Controls;
using Mes.Util;
using ProductionOutput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionOutput
{
    public partial class ProductionOutputForm:MaterialForm
    {
        delegate void SetTextCallBack(int hour);
        Bitmap smile = null;
        Bitmap cry = null;
        int dayFinishQty = 0;
        int seQty = 0;
        int seFinishQty = 0;
        int sizeUnfinishQty = 0;
        int hourQty = 0;
        Thread hourThread = null;
        DataTable workDayDt = null;
        DataTable workDaySizeDt = null;
        string d_dept = "";
        string d_dept_name = "";
        string size = "";

        Button btnSize;
        bool isScan = true;
        string seId_btn = "";


        private string strValue;
        public string StrValue
        {
            set
            {
                strValue = value;
            }
        }

        public ProductionOutputForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            btnImage.Visible = false;
            smile = new Bitmap(ProductionOutput.Properties.Resources.smile);
            cry = new Bitmap(ProductionOutput.Properties.Resources.cry);
        }

        private void ProductionOutputForm_Load(object sender, EventArgs e)
        {
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            LoadDept();
            LoadDayPo();
            LoadDayFinishQty();
            SetDayFinishQty();
            LoadHourQty();
            LoadScanLog();
            textLabel.LostFocus += new EventHandler(textLabel_LostFocus);
            hourThread = new Thread(new ThreadStart(Go));
            hourThread.Start();
        }

        //线程更新界面当前整点
        public void Go()
        {
            while (true)
            {
                int hour = DateTime.Now.Hour;
                if (!hour.ToString().Equals(labelHour.Text))
                {
                    SetHour(hour);
                }
                Thread.Sleep(1000 * 1);
            }
        }

        private void SetHour(int hour)
        {
            if (this.labelHour.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetHour);
                this.Invoke(stcb, new object[] { hour });
            }
            else
            {
                if (!hour.ToString().Equals(labelHour.Text))
                {
                    labelHour.Text = hour.ToString();
                }
            }
        }

        //加载部门
        private void LoadDept()
        {
            try
            {
                GetDept();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            labelDeptNo.Text = d_dept;
            labelDeptName.Text = d_dept_name;
        }

        //加载当天总完成数量
        private void LoadDayFinishQty()
        {
            try
            {
                GetDayFinishQty();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        //加载PO列表
        private void LoadDayPo()
        {
            this.PoPanel1.Controls.Clear();
            try
            {
                workDayDt = GetSeId_Po();
                for (int i = 0; i < workDayDt.Rows.Count; i++)
                {
                    RadioButton poRB = new RadioButton();
                    poRB = new RadioButton();
                    poRB.Text = workDayDt.Rows[i]["PO"].ToString();
                    poRB.AutoSize = true;
                    poRB.Font = new Font("微软雅黑", 32F);
                    poRB.Margin = new Padding(35, 10, 0, 0);
                    this.PoPanel1.Controls.Add(poRB);
                    poRB.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        //加载当前时产量
        private void LoadHourQty()
        {
            int hour = DateTime.Now.Hour;
            labelHour.Text = hour.ToString();
            try
            {
                GetHourQty();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            textHourQty.Text = hourQty.ToString();
        }

        //加载当天扫描记录
        private void LoadScanLog()
        {
            dataGridView1.Rows.Clear();
            try
            {
                DataTable logDt = GetScanLog();
                for (int i = 0; i < logDt.Rows.Count; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    dr.CreateCells(dataGridView1);
                    dr.Cells[0].Value = logDt.Rows[i]["SE_ID"].ToString();
                    dr.Cells[1].Value = logDt.Rows[i]["PO_NO"].ToString();
                    dr.Cells[2].Value = logDt.Rows[i]["SIZE_NO"].ToString();
                    dr.Cells[3].Value = logDt.Rows[i]["ART_NO"].ToString();
                    dr.Cells[4].Value = Convert.ToDateTime(logDt.Rows[i]["SCAN_DATE"]).ToString();
                    dataGridView1.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        //指定订单扫描总数量
        private void SetSeFinishQty(string seId)
        {
            try
            {
                GetSeFinishQty(seId);
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            textSeFinishQty.Text = seFinishQty.ToString();
        }

        //指定订单包装排产总数量
        private void SetSeUnFinishQty(string seId)
        {
            try
            {
                GetSeQty(seId);
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            textSeUnFinishQty.Text = (seQty - seFinishQty).ToString();
        }

        private void SetDayFinishQty()
        {
            textDayFinishQty.Text = dayFinishQty.ToString();
        }

        //获取部门代码、名称
        private void GetDept()
        {
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetDept", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                d_dept = dtJson.Rows[0]["STAFF_DEPARTMENT"].ToString();
                d_dept_name = dtJson.Rows[0]["DEPARTMENT_NAME"].ToString();
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        
        //获取今天已扫描数量
        private void GetDayFinishQty()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetDayFinishQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                //DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dayFinishQty = int.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        //获取组别当天的任务列表
        private DataTable GetSeId_Po()
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetSeId_Po", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private void GetHourQty()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetHourQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                //DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                hourQty = int.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private DataTable GetScanLog()
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetScanLog", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private void GetSeFinishQty(string seId)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            p.Add("vSeId", seId);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetSeFinishQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                //DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                seFinishQty = int.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void GetSeQty(string seId)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            p.Add("vSeId", seId);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetSeQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                //DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                seQty = int.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        //获取art_name
        private string GetArtName(string artNo)
        {
            string artName = "";
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vArtNo", artNo);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetArtName", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                artName = json;
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return artName;
        }

        //获取组别当天分SIZE的任务列表
        private DataTable GetWorkDaySize(string seId)
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>(); 
            p.Add("vDDept", d_dept);
            p.Add("vSeId", seId);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetWorkDaySize", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        //根据内盒标查询相关订单信息
        private DataTable GetLabelDetail(string label)
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLabel", label);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetLabelDetail", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        //查询指定订单 size 的未完成数量
        private int GetUnfinishQty(string se_id, string size)
        {
            int qty = 0;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", se_id);
            p.Add("vDDept", d_dept);
            p.Add("vSizeNo", size);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetUnfinishQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                //DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                qty = int.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return qty;
        }

        //查询指定订单 size 产出的排产、扫描总数量
        private DataTable GetOutDetail(string se_id, string size)
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", se_id);
            p.Add("vDDept", d_dept);
            p.Add("vSizeNo", size);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetOutDetail", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        /// <summary>
        /// 更新完工数量
        /// </summary>
        /// <param name="seId"></param>
        /// <param name="SizeNo"></param>
        /// <param name="scan_ip"></param>
        /// <param name="label"></param>
        /// <param name="scanPZ">"A"扫描录入，"A"手点录入</param>
        /// <returns></returns>
        private bool updateOutFinshQty(string seId, string SizeNo, string scan_ip, string label, string scanPZ)
        {
            bool isOK = false;
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vIP", scan_ip);
            p.Add("vLabel", label);
            p.Add("vScanPZ", scanPZ);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "updateOutFinshQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            isOK = Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]);
            if (!isOK)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return isOK;
        }

        //切换po
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked)
            {
                return;
            }
            string po = rb.Text;
            string se_id = "";

            for (int i = 0; i < workDayDt.Rows.Count; i++)
            {
                if (po.Equals(workDayDt.Rows[i]["PO"].ToString()))
                {
                    se_id = workDayDt.Rows[i]["SE_ID"].ToString();

                    labelPo.Text = workDayDt.Rows[i]["PO"].ToString();
                    labelArt.Text = workDayDt.Rows[i]["ART_NO"].ToString();
                    labelSeId.Text = se_id;
                    labelArtName.Text = GetArtName(workDayDt.Rows[i]["ART_NO"].ToString());
                    labelSize.Text = "";
                    LoadSizeList(labelSeId.Text);
                    break;
                }
            }
            SetSeFinishQty(se_id);
            SetSeUnFinishQty(se_id);
            SetSizeQty(string.Empty, string.Empty, string.Empty);
        }

        //加载size列表
        private void LoadSizeList(string se_id)
        {
            this.SizePanel.Controls.Clear();
            try
            {
                workDaySizeDt = GetWorkDaySize(se_id);
                for (int i = 0; i < workDaySizeDt.Rows.Count; i++)
                {
                    Button button = new Button();
                    button.Text = workDaySizeDt.Rows[i]["SIZE_NO"].ToString();
                    button.Font = new Font("微软雅黑", 30F);
                    button.Size = new Size(135, 135);
                    button.BackColor = System.Drawing.Color.Gray;
                    button.Click += new EventHandler(buttonSize_Click);
                    this.SizePanel.Controls.Add(button);
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        //PO资料刷新
        private void button1_Click(object sender, EventArgs e)
        {
            LoadDayPo();
            this.SizePanel.Controls.Clear();
            SetUINull();
        }

        //Size列表刷新，若列表为空，则移除相应的 PO
        private void RefreshSize(string se_id)
        {
            LoadSizeList(se_id);
            if (SizePanel.Controls.Count == 0)
            {
                foreach (Control poControl in PoPanel1.Controls)
                {
                    RadioButton rb = (RadioButton)poControl;
                    if (rb.Checked)
                    {
                        PoPanel1.Controls.Remove(rb);
                        SetUINull();
                    }
                }
            }
        }

        //手点式录入数据
        private void buttonSize_Click(object sender, EventArgs e)
        {
            btnSize = (Button)sender;
            size = btnSize.Text;
            string se_id = this.labelSeId.Text;
            string po = labelPo.Text;
            string art = labelArt.Text;
            string scan_ip = IPUtil.GetIpAddress();
            //手动点选，内盒标默认为8888
            string label = "8888";
            isScan = false;
            seId_btn = se_id;
            SetButtonEnable();
            try
            {
                sizeUnfinishQty = GetUnfinishQty(se_id, size);
                DataTable dt = GetOutDetail(se_id, size);
                if (updateOutFinshQty(se_id, size, scan_ip, label, "B"))
                {
                    ScanSucceed();
                    //扫描成功 PO:{0} ART:{1} SIZE:{2}的数据
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-10002", Program.client, "", Program.client.Language);
                    //labelDetail.Text = msg;
                    labelDetail.Text = string.Format(msg, po, art, size);
                    labelSize.Text = size;
                    dayFinishQty += 1;
                    SetDayFinishQty();
                    seFinishQty += 1;
                    textSeFinishQty.Text = seFinishQty.ToString();
                    SetSeUnFinishQty(se_id);
                    hourQty += 1;
                    textHourQty.Text = hourQty.ToString();
                    SetSizeQty(size, Convert.ToInt32(decimal.Parse(dt.Rows[0]["WORK_QTY"].ToString()) + decimal.Parse(dt.Rows[0]["SUPPLEMENT_QTY"].ToString())).ToString(), Convert.ToInt32(decimal.Parse(dt.Rows[0]["FINISH_QTY"].ToString()) + 1).ToString());
                    //RefreshSize(se_id);
                    AddScanLog(se_id, po, size, art);
                }
                else
                {
                    ScanFailed();
                    //扫描失败!
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00001", Program.client, "", Program.client.Language);
                    labelDetail.Text = msg;
                }
            }
            catch (Exception ex)
            {
                ScanFailed();
                //扫描失败!
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00001", Program.client, "", Program.client.Language);
                labelDetail.Text = msg;
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        //扫内盒标
        private void textLabel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string label = textLabel.Text;
                DataTable labelDt = null;
                if (string.IsNullOrEmpty(label))
                {
                    return;
                }
                isScan = true;
                SetButtonEnable();
                try
                {
                    labelDt = GetLabelDetail(label);
                }
                catch (Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }

                if (labelDt.Rows.Count == 0)
                {
                    //扫描失败!没有找到{0}内盒标的信息,请找业务更新
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00002", Program.client, "", Program.client.Language);
                    //labelDetail.Text = msg;
                    labelDetail.Text = string.Format(msg, label);
                    ScanFailed();
                }
                else
                {
                    string po = labelPo.Text;
                    string se_id = labelSeId.Text;
                    size = labelDt.Rows[0]["SIZE_NO"].ToString();
                    string art = labelDt.Rows[0]["ART_NO"].ToString();
                    if (string.IsNullOrEmpty(po) || !art.Equals(labelArt.Text))
                    {
                        //扫描失败!请选择正确的PO
                        string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00003", Program.client, "", Program.client.Language);
                        labelDetail.Text = msg;
                        ScanFailed();
                        textLabel.Text = "";
                        return;
                    }
                    string scan_ip = IPUtil.GetIpAddress();

                    sizeUnfinishQty = GetUnfinishQty(se_id, size);

                    DataTable dt = GetOutDetail(se_id, size);

                    if (dt.Rows.Count <= 0)
                    {
                        //扫描失败!该PO:{0}的{1}码SIZE没有投入
                        string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00004", Program.client, "", Program.client.Language);
                        //labelDetail.Text = msg;
                        labelDetail.Text = string.Format(msg, po, size);
                        SetSizeQty(size, 0.ToString(), 0.ToString());
                        RefreshSize(se_id);
                        ScanFailed();
                    }
                    else if(dt.Rows.Count > 0 && sizeUnfinishQty <= 0)
                    {
                        //扫描失败! PO:{0}的{1}码的SIZE投入数量已全部扫描
                        string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00005", Program.client, "", Program.client.Language);
                        labelDetail.Text = string.Format(msg, po, size);
                        SetSizeQty(size, Convert.ToInt32(decimal.Parse(dt.Rows[0]["WORK_QTY"].ToString()) + decimal.Parse(dt.Rows[0]["SUPPLEMENT_QTY"].ToString())).ToString(), 
                            Convert.ToInt32(decimal.Parse(dt.Rows[0]["FINISH_QTY"].ToString())).ToString());
                        ScanFailed();
                    }
                    else if (dt.Rows.Count > 0 && sizeUnfinishQty > 0)
                    {
                        try
                        {                           
                            if (updateOutFinshQty(se_id, size, scan_ip, label, "A"))
                            {
                                ScanSucceed();
                                //扫描成功!PO:{0} ART:{1} SIZE:{2}的数据                               
                                string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-10002", Program.client, "", Program.client.Language);
                                //labelDetail.Text = msg;
                                labelDetail.Text = string.Format(msg, po, art, size);
                                labelSize.Text = size;
                                dayFinishQty += 1;
                                SetDayFinishQty();
                                seFinishQty += 1;
                                textSeFinishQty.Text = seFinishQty.ToString();
                                SetSeUnFinishQty(se_id);
                                hourQty += 1;
                                textHourQty.Text = hourQty.ToString();
                                SetSizeQty(size, Convert.ToInt32(decimal.Parse(dt.Rows[0]["WORK_QTY"].ToString()) + decimal.Parse(dt.Rows[0]["SUPPLEMENT_QTY"].ToString())).ToString(), Convert.ToInt32(decimal.Parse(dt.Rows[0]["FINISH_QTY"].ToString()) + 1).ToString());
                                RefreshSize(se_id);
                                AddScanLog(se_id, po, size, art);
                            }
                            else
                            {
                                ScanFailed();
                                //labelDetail.Text = "扫描失败";
                                string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00001", Program.client,"", Program.client.Language);
                                labelDetail.Text = msg;
                            }
                        }
                        catch (Exception ex)
                        {
                            ScanFailed();
                            //labelDetail.Text = "扫描失败";
                            string msg = SJeMES_Framework.Common.UIHelper.UImsg("msg-scan-00001", Program.client, "", Program.client.Language);
                            labelDetail.Text = msg;
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                        }
                    }
                }
                textLabel.Text = "";
            }
        }

        //当前整点变化，重置当前小时产量为0
        private void labelHour_TextChanged(object sender, EventArgs e)
        {
            hourQty = 0;
            textHourQty.Text = hourQty.ToString();
        }

        //更新界面的SIZE、投入数量和已扫描数量
        private void SetSizeQty(string size, string inQty, string outFinishQty)
        {
            labelInQty.Text = inQty;
            labelOutQty.Text = outFinishQty;
            labelSize.Text = size;
        }

        //清空界面左边的信息（PO、ART，订单号）
        private void SetUINull()
        {
            labelPo.Text = "";
            labelArt.Text = "";
            labelSeId.Text = "";
            labelArtName.Text = "";
            labelSize.Text = "";
            textSeUnFinishQty.Text = "";
            textSeFinishQty.Text = "";
            SetSizeQty(string.Empty, string.Empty, string.Empty);
        }

        //扫描失败的背景图及颜色
        private void ScanFailed()
        {
            labelDetail.BackColor = Color.Red;
            btnImage.Visible = true;
            btnImage.BackgroundImage = cry;
            //labelSize.Text = "";
        }

        //扫描成功的背景图及颜色
        private void ScanSucceed()
        {
            labelDetail.BackColor = Color.Green;
            btnImage.Visible = true;
            btnImage.BackgroundImage = smile;
        }

        //扫描详情 新增一行
        private void AddScanLog(string se_id, string po, string size, string art)
        {
            DateTime insertDate = DateTime.Now;
            DataGridViewRow dr = new DataGridViewRow();
            dr.CreateCells(dataGridView1);
            dr.Cells[0].Value = se_id;
            dr.Cells[1].Value = po;
            dr.Cells[2].Value = size;
            dr.Cells[3].Value = art;
            dr.Cells[4].Value = insertDate.ToString();
            dataGridView1.Rows.Insert(0, dr);
        }

        private void ProductionOutputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hourThread != null)
            {
                hourThread.Abort();
            }
        }

        //控制内盒标只能输入0-9
        private void textLabel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
        }

        //内盒标输入框失去焦点时 把焦点拉回来
        private void textLabel_LostFocus(object sender, EventArgs e)
        {
            this.textLabel.Focus();
        }

        //定时器，扫描后 一秒内 不可再扫
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1200;
            if (!isScan)
            {
                RefreshSize(seId_btn);
            }

            foreach (Button btn in SizePanel.Controls)
            {
                btn.Enabled = true;
            }
            textLabel.ReadOnly = false;
            //labelSize.Text = "";
            timer1.Stop();
        }

        private void SetButtonEnable()
        {
            if (!isScan)
            {
                btnSize.BackColor = Color.Blue;
            }
            foreach (Button btn in SizePanel.Controls)
            {
                btn.Enabled = false;
            }
            textLabel.ReadOnly = true;
            timer1.Enabled = true;
        }

        private void butQuery_Click(object sender, EventArgs e)
        {
            Hide();
            InOutDetailForm frm = new InOutDetailForm(d_dept);
            frm.ShowDialog();
            System.Threading.Thread.Sleep(200);
            Show();
        }

        public void DataChanged(object sender, DataChangeEventArgs args)
        {
            d_dept = args.name;
            labelDeptNo.Text = d_dept;
            labelDeptName.Text = args.pass;

            SetUINull();
            this.SizePanel.Controls.Clear();
            LoadDayPo();
            LoadDayFinishQty();
            SetDayFinishQty();
            LoadHourQty();
            LoadScanLog(); 
            
        }

        private void butSelectDept_Click(object sender, EventArgs e)
        {
            DepSelectForm frm = new DepSelectForm();
            frm.DataChange += new DepSelectForm.DataChangeHandler(DataChanged);
            frm.ShowDialog();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            textTime.Text = DateTime.Now.ToString();
        }
    }
}
