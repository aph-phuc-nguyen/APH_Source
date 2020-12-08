using MaterialSkin.Controls;
using Newtonsoft.Json;
using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


namespace Production_Kanban
{
    public partial class Production_KanbanForm : MaterialForm
    {

        Timer timer1 = new Timer();
        private string ART = "";
        private delegate void GetDataCallback();
        public delegate void SetTextCallBack(string setTextMethod, DataTable dtJson);
        private void MySetTxetMehod(string setTextMethod, DataTable dtJson)
        {
            Type type;
            Object obj;
            string className = this.GetType().FullName;
            type = Type.GetType(className);
            obj = System.Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(setTextMethod);
            object[] args = new object[1];
            args[0] = dtJson;
            method.Invoke(obj, args);
        }

        public delegate void MyDelegate(string pageName);

        private void MyMehod(string pageName)
        {
            Type type;
            Object obj;
            string className = this.GetType().FullName;
            type = Type.GetType(className);
            obj = System.Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(pageName, new Type[] { });
            object[] parameters = null;
            method.Invoke(obj, parameters);
        }


        public Production_KanbanForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            tabPage8.Parent = null;
            //tabPage2.Parent = null;
            //tabPage3.Parent = null;
            //tabPage4.Parent = null;
            //tabPage5.Parent = null;
            //tabPage7.Parent = null;
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
        }

        private void Production_KanbanForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Interface.line) && !string.IsNullOrEmpty(Interface.date))
            {
                txtLine.Text = Interface.line;
                dateTimePicker1.Text = Interface.date;
            }
            else if (!string.IsNullOrEmpty(Interface.date))
            {
                getLine();
            }
            else
            {
                try
                {
                    getLine();
                    getWorkDate();
                }
                catch (Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
         //   SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
            //tabPage8_query();
            tabControl1_SelectedIndexChanged(sender,e);
            if (string.IsNullOrWhiteSpace(txtTimer.Text))
            {
                txtTimer.Text = "600";
            }

            timer1.Interval = int.Parse(txtTimer.Text.ToString()) * 1000;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_Tick);//添加事件

        }

        private void getLine()
        {
            string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Efficiency_KanbanServer", "QueryDeptNo", Program.Client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
            {
                string line = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                txtLine.Text = line;
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
            }
        }
        private void getWorkDate()
        {
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



        private void timer1_Tick(object sender, EventArgs e)
        {
            int index = tabControl1.SelectedIndex;
            if (index == tabControl1.TabCount - 2)
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
            //myDelegate(pageName);
            switch (pageName)
            {
                case "tabPage1_query":
                    tabPage1_query();
                    break;
                case "tabPage2_query":
                    //tabPage2_query();
                    GetTier1();
                    break;
                case "tabPage3_query":
                    //tabPage3_query();
                    GetTier1Standard();
                    break;
                case "tabPage4_query":
                    GetTier1_WeekSafety();
                    GetTier1Data();
                    tabPage4_query();
                    break;
                case "tabPage5_query":
                    tabPage5_query();
                    break;
                case "tabPage6_query":
                    tabPage6_fast_query();
                    break;
                case "tabPage7_query":
                    tabPage7_query();
                    break;
                case "tabPage8_query":
                    tabPage8_query();
                    break;
                case "tabPage9_query":
                    tabPage9_query();
                    break;
            }
        }
        private void GetTier1Data() {
            GetWeekRFT();
            GetWeekPPHTarget();
            GetWeekPPH();
            GetWeekOutput();
            GetKaizen();
            GetWeekLLER();
            GetWeekMulti();
            GetWeekDT();
            GetWeekCOT();
            GetWeekWIP();
        }
        private void GetWeekRFT()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekRFT);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekRFT();
            }
        }
        private void GetWeekPPHTarget()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekPPHTarget);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekPPHTarget();
            }
        }
        private void GetWeekPPH()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekPPH);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekPPH();
            }
        }
        private void GetWeekOutput()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekOutput);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekOutput();
            }
        }
        private void GetKaizen()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetKaizen);
                this.Invoke(d);
            }
            else
            {
                GetTier1_Kaizen();
            }
        }
        private void GetWeekLLER()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekLLER);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekLLER();
            }
        }
        private void GetWeekMulti()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekMulti);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekMulti();
            }
        }
        private void GetWeekDT()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekDT);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekDT();
            }
        }
        private void GetWeekCOT()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekCOT);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekCOT();
            }
        }
        private void GetWeekWIP()
        {
            if (this.InvokeRequired)
            {
                GetDataCallback d = new GetDataCallback(GetWeekWIP);
                this.Invoke(d);
            }
            else
            {
                GetTier1_WeekWIP();
            }
        }
        /// <summary>
        /// 基础资料
        /// </summary>
        public void tabPage1_query()
        {
            try
            {
                if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString()) && !string.IsNullOrEmpty(txtLine.Text))
                {
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("date", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    p.Add("line", txtLine.Text);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage1_Query", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        //不能使用委托更新UI
                        //SetTextCallBack mySetTxetMehod = MySetTxetMehod;
                        //mySetTxetMehod("Set_Page1Text", dtJson);
                        //可以启动另外一个线程来更新UI
                        //this.Invoke(new Action(() =>
                        //{
                        //    label3.Text = dtJson.Rows[0]["ModelName"].ToString();
                        //    label53.Text = dtJson.Rows[0]["Date"].ToString();
                        //    label54.Text = dtJson.Rows[0]["Target"].ToString();
                        //    label55.Text = dtJson.Rows[0]["TaktTime"].ToString();
                        //    label56.Text = dtJson.Rows[0]["WaterSpiderNo"].ToString();
                        //    label58.Text = dtJson.Rows[0]["ModelTHT"].ToString();
                        //    label52.Text = dtJson.Rows[0]["OperatorNo"].ToString();
                        //}));
                        SetText(label3, dtJson.Rows[0]["ModelName"].ToString());
                        SetText(label53, dtJson.Rows[0]["Date"].ToString());
                        SetText(label54, dtJson.Rows[0]["Target"].ToString());
                        if(dtJson.Rows[0]["Target"].ToString()!="0")
                        SetText(label55, (3600 /(decimal.Parse(dtJson.Rows[0]["Target"].ToString())/10)).ToString("0.00"));
                        else
                            SetText(label55,"");                      
                        SetText(label56, dtJson.Rows[0]["WaterSpiderNo"].ToString());
                        //SetText(label58, dtJson.Rows[0]["ModelTHT"].ToString());
                        SetText(label52, dtJson.Rows[0]["OperatorNo"].ToString());
                        ART = dtJson.Rows[0]["ART"].ToString();
                        Dictionary<string, Object> pTHT = new Dictionary<string, object>();
                        pTHT.Add("ART", ART);
                        string retTHT = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL,
                            "TierMeeting","TierMeeting.Controllers.TierMeetingServer",
                                        "GetTHTByART", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(pTHT));
                        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retTHT)["IsSuccess"])) 
                        {
                            string jsonTHT = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retTHT)["RetData"].ToString();
                            DataTable dtJsonTHT = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(jsonTHT);
                            rtbTHT.Text = dtJsonTHT.Rows[0][0].ToString();
                        }
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 一层层级会议
        /// </summary>
        public void tabPage2_query()
        {
            try
            {
                if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString()) && !string.IsNullOrEmpty(txtLine.Text))
                {
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("date", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    p.Add("line", txtLine.Text);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_SFCAPI", "KZ_SFCAPI.Controllers.TierMeetingServer", "TabPage2_Query", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        this.Invoke(new Action(() =>
                        {
                            lblTier1Time.Text = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                            if (dtJson.Rows.Count==0)
                            {
                                //label32.BackColor = Color.FromArgb(255, 255, 255);
                                //label33.BackColor = Color.FromArgb(255, 255, 255);
                                //label34.BackColor = Color.FromArgb(255, 255, 255);
                                //label35.BackColor = Color.FromArgb(255, 255, 255);
                                //label36.BackColor = Color.FromArgb(255, 255, 255);
                                //label37.BackColor = Color.FromArgb(255, 255, 255);
                                //label38.BackColor = Color.FromArgb(255, 255, 255);
                                //label39.BackColor = Color.FromArgb(255, 255, 255);
                                //label122.BackColor = Color.FromArgb(255, 255, 255);
                                //label32.Text = "";
                                //label33.Text= "";
                                //label34 .Text= "";
                                //label35 .Text= "";
                                //label36 .Text= "";
                                //label37 .Text= "";
                                //label38.Text = "";
                                //label39 .Text= "";
                                //label122.Text= "";
                                //label51.Text = "";
                            }
                            for (int i = 0; i < dtJson.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label32.Text = "√";
                                //        label32.BackColor = Color.FromArgb(255, 255, 255);
                                //    }
                                //    else
                                //    {
                                //        label32.Text = "×";
                                //        label32.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                //    label51.Text = dtJson.Rows[i]["audit_person"].ToString();
                                //}
                                //if (i == 1)
                                //{
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label33.Text = "√";
                                //        label33.BackColor = Color.FromArgb(255, 255, 255);

                                //    }
                                //    else
                                //    {
                                //        label33.Text = "×";
                                //        label33.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                //}
                                //if (i == 2)
                                //{
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label34.Text = "√";
                                //        label34.BackColor = Color.FromArgb(255, 255, 255);

                                //    }
                                //    else
                                //    {
                                //        label34.Text = "×";
                                //        label34.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                //}
                                //if (i == 3)
                                //{
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label35.Text = "√";
                                //        label35.BackColor = Color.FromArgb(255, 255, 255);

                                //    }
                                //    else
                                //    {
                                //        label35.Text = "×";
                                //        label35.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                //}
                                //if (i == 4)
                                //{
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label36.Text = "√";
                                //        label36.BackColor = Color.FromArgb(255, 255, 255);

                                //    }
                                //    else
                                //    {
                                //        label36.Text = "×";
                                //        label36.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                //}
                                //if (i == 5)
                                //{
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label37.Text = "√";
                                //        label37.BackColor = Color.FromArgb(255, 255, 255);

                                //    }
                                //    else
                                //    {
                                //        label37.Text = "×";
                                //        label37.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                //}
                                //if (i == 6)
                                //{
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label38.Text = "√";
                                //        label38.BackColor = Color.FromArgb(255, 255, 255);

                                //    }
                                //    else
                                //    {
                                //        label38.Text = "×";
                                //        label38.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                //}
                                //if (i == 7)
                                //{
                                //    if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                //    {
                                //        label39.Text = "√";
                                //        label39.BackColor = Color.FromArgb(255, 255, 255);

                                //    }
                                //    else
                                //    {
                                //        label39.Text = "×";
                                //        label39.BackColor = Color.FromArgb(254, 205, 208);

                                //    }
                                }
                                if (i == 8)
                                {
                                    //if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                                    //{
                                    //    label122.Text = "√";
                                    //    label122.BackColor = Color.FromArgb(255, 255, 255);

                                    //}
                                    //else
                                    //{
                                    //    label122.Text = "×";
                                    //    label122.BackColor = Color.FromArgb(254, 205, 208);

                                    //}
                                }
                            }
                            
                        }));
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 标准制造环境
        /// </summary>
        public void tabPage3_query()
        {
            try
            {
                if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString()) && !string.IsNullOrEmpty(txtLine.Text))
                {
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("date", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    p.Add("line", txtLine.Text);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_SFCAPI", "KZ_SFCAPI.Controllers.TierMeetingServer", "TabPage3_Query", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        this.Invoke(new Action(() =>
                        {
                            //label62.Text = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                            //if (dtJson.Rows.Count==0)
                            //{
                            //    setManagerToDefault();
                            //    setVsmLableToDefault();
                            //    setThirdPartySpotCheckLableToDefault();
                            //    setNonImpentationReasonsToDefalut();
                            //}
                            //else
                            //{
                            //    if (dtJson.Select("AUDIT_ITEM='A'").Count() == 0)
                            //    {
                            //        setManagerToDefault();
                            //    }
                            //    if (dtJson.Select("AUDIT_ITEM='B'").Count() == 0)
                            //    {
                            //        setVsmLableToDefault();
                            //    }
                            //    if (dtJson.Select("AUDIT_ITEM='C'").Count() == 0)
                            //    {
                            //        setThirdPartySpotCheckLableToDefault();
                            //    }
                            //    //没有执行的原因
                            //    if (dtJson.Select("AUDIT_SEQ='1'").Count()==0)
                            //    {
                            //        label74.Text = "";
                            //    }
                            //    else 
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='1'");
                            //        for (int j=0;j<foundRows.Count();j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label74.Text = tempStr;
                            //    }

                            //    if (dtJson.Select("AUDIT_SEQ='2'").Count() == 0)
                            //    {
                            //        label80.Text = "";
                            //    }
                            //    else
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='2'");
                            //        for (int j = 0; j < foundRows.Count(); j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label80.Text = tempStr;
                            //    }

                            //    if (dtJson.Select("AUDIT_SEQ='3'").Count() == 0)
                            //    {
                            //        label86.Text = "";
                            //    }
                            //    else
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='3'");
                            //        for (int j = 0; j < foundRows.Count(); j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label86.Text = tempStr;
                            //    }


                            //    if (dtJson.Select("AUDIT_SEQ='4'").Count() == 0)
                            //    {
                            //        label92.Text = "";
                            //    }
                            //    else
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='4'");
                            //        for (int j = 0; j < foundRows.Count(); j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label92.Text = tempStr;
                            //    }


                            //    if (dtJson.Select("AUDIT_SEQ='5'").Count() == 0)
                            //    {
                            //        label98.Text = "";
                            //    }
                            //    else
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='5'");
                            //        for (int j = 0; j < foundRows.Count(); j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label98.Text = tempStr;
                            //    }


                            //    if (dtJson.Select("AUDIT_SEQ='6'").Count() == 0)
                            //    {
                            //        label104.Text = "";
                            //    }
                            //    else
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='6'");
                            //        for (int j = 0; j < foundRows.Count(); j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label104.Text = tempStr;
                            //    }

                            //    if (dtJson.Select("AUDIT_SEQ='7'").Count() == 0)
                            //    {
                            //        label110.Text = "";
                            //    }
                            //    else
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='7'");
                            //        for (int j = 0; j < foundRows.Count(); j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label110.Text = tempStr;
                            //    }

                            //    if (dtJson.Select("AUDIT_SEQ='8'").Count() == 0)
                            //    {
                            //        label116.Text = "";
                            //    }
                            //    else
                            //    {
                            //        string tempStr = "";
                            //        DataRow[] foundRows;
                            //        foundRows = dtJson.Select("AUDIT_SEQ='8'");
                            //        for (int j = 0; j < foundRows.Count(); j++)
                            //        {
                            //            tempStr += foundRows[j]["audit_memo"];
                            //        }
                            //        label116.Text = tempStr;
                            //    }

                            //}                        
                            //for (int i = 0; i < dtJson.Rows.Count; i++)
                            //{
                            //    #region 主管自评
                            //    if (dtJson.Rows[i]["audit_item"].ToString().Equals("A"))
                            //    {
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("1"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label71.Text = "√";
                            //                label71.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label71.Text = "×";
                            //                label71.BackColor = Color.FromArgb(254, 205, 208);
                            //            }
                            //            label117.Text = dtJson.Rows[i]["audit_person"].ToString();
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("2"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label77.Text = "√";
                            //                label77.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label77.Text = "×";
                            //                label77.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("3"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label83.Text = "√";
                            //                label83.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label83.Text = "×";
                            //                label83.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("4"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label89.Text = "√";
                            //                label89.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label89.Text = "×";
                            //                label89.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("5"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label95.Text = "√";
                            //                label95.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label95.Text = "×";
                            //                label95.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("6"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label101.Text = "√";
                            //                label101.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label101.Text = "×";
                            //                label101.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("7"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label107.Text = "√";
                            //                label107.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label107.Text = "×";
                            //                label107.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("8"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label113.Text = "√";
                            //                label113.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label113.Text = "×";
                            //                label113.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //    }
                            //    #endregion 主管自评

                            //    #region VSM复核
                            //    if (dtJson.Rows[i]["audit_item"].ToString().Equals("B"))
                            //    {
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("1"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label72.Text = "√";
                            //                label72.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label72.Text = "×";
                            //                label72.BackColor = Color.FromArgb(254, 205, 208);
                            //            }
                            //            label119.Text = dtJson.Rows[i]["audit_person"].ToString();
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("2"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label78.Text = "√";
                            //                label78.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label78.Text = "×";
                            //                label78.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("3"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label84.Text = "√";
                            //                label84.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label84.Text = "×";
                            //                label84.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("4"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label90.Text = "√";
                            //                label90.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label90.Text = "×";
                            //                label90.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("5"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label96.Text = "√";
                            //                label96.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label96.Text = "×";
                            //                label96.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("6"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label102.Text = "√";
                            //                label102.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label102.Text = "×";
                            //                label102.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("7"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label108.Text = "√";
                            //                label108.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label108.Text = "×";
                            //                label108.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("8"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label114.Text = "√";
                            //                label114.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label114.Text = "×";
                            //                label114.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //    }
                            //    #endregion VSM复核

                            //    #region 第三方抽查
                            //    if (dtJson.Rows[i]["audit_item"].ToString().Equals("C"))
                            //    {
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("1"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label73.Text = "√";
                            //                label73.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label73.Text = "×";
                            //                label73.BackColor = Color.FromArgb(254, 205, 208);
                            //            }
                            //            label121.Text = dtJson.Rows[i]["audit_person"].ToString();
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("2"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label79.Text = "√";
                            //                label79.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label79.Text = "×";
                            //                label79.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("3"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label85.Text = "√";
                            //                label85.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label85.Text = "×";
                            //                label85.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("4"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label91.Text = "√";
                            //                label91.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label91.Text = "×";
                            //                label91.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("5"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label97.Text = "√";
                            //                label97.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label97.Text = "×";
                            //                label97.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("6"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label103.Text = "√";
                            //                label103.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label103.Text = "×";
                            //                label103.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("7"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label109.Text = "√";
                            //                label109.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label109.Text = "×";
                            //                label109.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //        if (dtJson.Rows[i]["audit_seq"].ToString().Equals("8"))
                            //        {
                            //            if (dtJson.Rows[i]["audit_result"].ToString().Equals("Y"))
                            //            {
                            //                label115.Text = "√";
                            //                label115.BackColor = Color.FromArgb(255, 255, 255);
                            //            }
                            //            else
                            //            {
                            //                label115.Text = "×";
                            //                label115.BackColor = Color.FromArgb(254, 205, 208);

                            //            }
                            //        }
                            //    }
                            //    #endregion 第三方抽查
                            //}

                        }));
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setManagerToDefault()
        {
            #region 主管自评
            //label71.Text = "";
            //label71.BackColor = Color.FromArgb(255, 255, 255);
            //label77.Text = "";
            //label77.BackColor = Color.FromArgb(255, 255, 255);
            //label83.Text = "";
            //label83.BackColor = Color.FromArgb(255, 255, 255);
            //label89.Text = "";
            //label89.BackColor = Color.FromArgb(255, 255, 255);
            //label95.Text = "";
            //label95.BackColor = Color.FromArgb(255, 255, 255);
            //label101.Text = "";
            //label101.BackColor = Color.FromArgb(255, 255, 255);
            //label107.Text = "";
            //label107.BackColor = Color.FromArgb(255, 255, 255);
            //label113.Text = "";
            //label113.BackColor = Color.FromArgb(255, 255, 255);
            //label117.Text = "";
            //label117.BackColor = Color.FromArgb(255, 255, 255);
            #endregion 主管自评
        }
        private void setVsmLableToDefault()
        {

            #region vsm复合
            //label72.Text = "";
            //label72.BackColor = Color.FromArgb(255, 255, 255);
            //label78.Text = "";
            //label78.BackColor = Color.FromArgb(255, 255, 255);
            //label84.Text = "";
            //label84.BackColor = Color.FromArgb(255, 255, 255);
            //label90.Text = "";
            //label90.BackColor = Color.FromArgb(255, 255, 255);
            //label96.Text = "";
            //label96.BackColor = Color.FromArgb(255, 255, 255);
            //label102.Text = "";
            //label102.BackColor = Color.FromArgb(255, 255, 255);
            //label108.Text = "";
            //label108.BackColor = Color.FromArgb(255, 255, 255);
            //label114.Text = "";
            //label114.BackColor = Color.FromArgb(255, 255, 255);
            //label119.Text = "";
            //label119.BackColor = Color.FromArgb(255, 255, 255);
            #endregion vsm复合
        }

        private void setNonImpentationReasonsToDefalut()
        {
            //label74.Text = "";
            //label80.Text = "";
            //label86.Text = "";
            //label92.Text = "";
            //label98.Text = "";
            //label104.Text = "";
            //label110.Text = "";
            //label116.Text = "";
        }

        private void setThirdPartySpotCheckLableToDefault()
        {
            #region 第三方抽查
            //label73.Text = "";
            //label73.BackColor = Color.FromArgb(255, 255, 255);
            //label79.Text = "";
            //label79.BackColor = Color.FromArgb(255, 255, 255);
            //label85.Text = "";
            //label85.BackColor = Color.FromArgb(255, 255, 255);
            //label91.Text = "";
            //label91.BackColor = Color.FromArgb(255, 255, 255);
            //label97.Text = "";
            //label97.BackColor = Color.FromArgb(255, 255, 255);
            //label103.Text = "";
            //label103.BackColor = Color.FromArgb(255, 255, 255);
            //label109.Text = "";
            //label109.BackColor = Color.FromArgb(255, 255, 255);
            //label115.Text = "";
            //label115.BackColor = Color.FromArgb(255, 255, 255);
            //label121.Text = "";
            //label121.BackColor = Color.FromArgb(255, 255, 255);
            #endregion 第三方抽查
        }
        /// <summary>
        /// 层级会议 & 标准小线绩效
        /// </summary>
        public void tabPage4_query()
        {
            //try
            //{
            //    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage4_Query", Program.client.UserToken, string.Empty);
            //    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            //    {
            //        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
            //        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            //        this.Invoke(new Action(() =>
            //        {

            //        }));
            //    }
            //    else
            //    {
            //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        /// <summary>
        /// KPI定义
        /// </summary>
        public void tabPage5_query()
        {
            //try
            //{
            //    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage5_Query", Program.client.UserToken, string.Empty);
            //    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            //    {
            //        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
            //        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            //        this.Invoke(new Action(() =>
            //        {

            //        }));
            //    }
            //    else
            //    {
            //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        public void tabPage6_fast_query()
        {
            try
            {
                if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString()) && !string.IsNullOrEmpty(txtLine.Text))
                {
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("date", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    p.Add("line", txtLine.Text);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage6_Query_ScanDetail", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                        string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage6_Query_OtherDetail", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                        {
                            string otherJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                            DataTable otherDtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(otherJson);
                            set_tabPage6_info(dtJson, otherDtJson);
                        }
                        else
                        {
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                        }
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }


                    //string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCMAPI", "KZ_QCMAPI.Controllers.TierMeetingServer", "Tier1_DayQuery", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    //if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
                    //{
                    //    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                    //    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    //    set_tabPage6_rft_info(dtJson);
                    //}
                    //else
                    //{
                    //    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["ErrMsg"].ToString());
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void set_tabPage6_rft_info(DataTable dtJson)
        {
            setRftToDefault(dtJson);
            for (int i=0;i<dtJson.Rows.Count;i++)
            {
                if (i==0)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString())&&decimal.Parse(dtJson.Rows[i]["num"].ToString())<85)
                    {
                        label248.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label248.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    label248.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString())?"":decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 1)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) && decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label255.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label255.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label255.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label255.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 2)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString())&&decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label262.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label262.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label262.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label262.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 3)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString())&&decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label269.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label269.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label269.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label269.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 4)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) && decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label276.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label276.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label276.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label276.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");

                }
                if (i == 5)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) && decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label283.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label283.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label283.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label283.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 6)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) && decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label290.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label290.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label290.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label290.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 7)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) && decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label297.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label297.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label297.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label297.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 8)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) && decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label304.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label304.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label304.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label304.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == 9)
                {
                    if (!string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) && decimal.Parse(dtJson.Rows[i]["num"].ToString()) < 85)
                    {
                        label311.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label311.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    //label311.Text = decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                    label311.Text = string.IsNullOrEmpty(dtJson.Rows[i]["num"].ToString()) ? "" : decimal.Parse(dtJson.Rows[i]["num"].ToString()).ToString("0.0");
                }
                if (i == dtJson.Rows.Count-1)
                {
                    decimal sum = 0;
                    decimal rft = 0;
                    int cishu = 0;
                    for (int s=0;s<dtJson.Rows.Count;s++)
                    {
                        if (!string.IsNullOrEmpty(dtJson.Rows[s]["num"].ToString()))
                        {
                            sum += decimal.Parse(dtJson.Rows[s]["num"].ToString());
                            cishu++;
                        }
                    }
                    rft= cishu == 0?0:(sum / cishu);
                    if (rft< 85)
                    {
                        label318.BackColor = Color.FromArgb(254, 205, 208);
                    }
                    else
                    {
                        label318.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    label318.Text = cishu==0?"":rft.ToString("0.0");

                }
            }
        }

        private void setRftToDefault(DataTable dataTable)
        {
            if (dataTable.Rows.Count==0) 
            {
                label248.BackColor = Color.FromArgb(255, 255, 255);
                label248.Text = "";
                label255.BackColor = Color.FromArgb(255, 255, 255);
                label255.Text = "";
                label262.BackColor = Color.FromArgb(255, 255, 255);
                label262.Text = "";
                label269.BackColor = Color.FromArgb(255, 255, 255);
                label269.Text = "";
                label276.BackColor = Color.FromArgb(255, 255, 255);
                label276.Text = "";
                label283.BackColor = Color.FromArgb(255, 255, 255);
                label283.Text = "";
                label290.BackColor = Color.FromArgb(255, 255, 255);
                label290.Text = "";
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
                label318.BackColor = Color.FromArgb(255, 255, 255);
                label318.Text = "";
            }
            if (dataTable.Rows.Count == 1)
            {
                label255.BackColor = Color.FromArgb(255, 255, 255);
                label255.Text = "";
                label262.BackColor = Color.FromArgb(255, 255, 255);
                label262.Text = "";
                label269.BackColor = Color.FromArgb(255, 255, 255);
                label269.Text = "";
                label276.BackColor = Color.FromArgb(255, 255, 255);
                label276.Text = "";
                label283.BackColor = Color.FromArgb(255, 255, 255);
                label283.Text = "";
                label290.BackColor = Color.FromArgb(255, 255, 255);
                label290.Text = "";
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 2)
            {
                label262.BackColor = Color.FromArgb(255, 255, 255);
                label262.Text = "";
                label269.BackColor = Color.FromArgb(255, 255, 255);
                label269.Text = "";
                label276.BackColor = Color.FromArgb(255, 255, 255);
                label276.Text = "";
                label283.BackColor = Color.FromArgb(255, 255, 255);
                label283.Text = "";
                label290.BackColor = Color.FromArgb(255, 255, 255);
                label290.Text = "";
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 3)
            {
                label269.BackColor = Color.FromArgb(255, 255, 255);
                label269.Text = "";
                label276.BackColor = Color.FromArgb(255, 255, 255);
                label276.Text = "";
                label283.BackColor = Color.FromArgb(255, 255, 255);
                label283.Text = "";
                label290.BackColor = Color.FromArgb(255, 255, 255);
                label290.Text = "";
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 4)
            {
                label276.BackColor = Color.FromArgb(255, 255, 255);
                label276.Text = "";
                label283.BackColor = Color.FromArgb(255, 255, 255);
                label283.Text = "";
                label290.BackColor = Color.FromArgb(255, 255, 255);
                label290.Text = "";
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 5)
            {
                label283.BackColor = Color.FromArgb(255, 255, 255);
                label283.Text = "";
                label290.BackColor = Color.FromArgb(255, 255, 255);
                label290.Text = "";
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 6)
            {
                label290.BackColor = Color.FromArgb(255, 255, 255);
                label290.Text = "";
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 7)
            {
                label297.BackColor = Color.FromArgb(255, 255, 255);
                label297.Text = "";
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 8)
            { 
                label304.BackColor = Color.FromArgb(255, 255, 255);
                label304.Text = "";
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
            if (dataTable.Rows.Count == 9)
            {
                label311.BackColor = Color.FromArgb(255, 255, 255);
                label311.Text = "";
            }
        }
    
        private void set_tabPage6_info(DataTable pDtJson, DataTable pOtherDtJson)
        {
            if (pDtJson.Rows.Count==0)
                return;
            DataTable timeParams = new DataTable();
            timeParams.Columns.Add("time");
            DataRow dr1 = timeParams.NewRow();
            dr1[0] = label193.Text;
            DataRow dr2 = timeParams.NewRow();
            dr2[0] = label194.Text;
            DataRow dr3 = timeParams.NewRow();
            dr3[0] = label195.Text;
            DataRow dr4 = timeParams.NewRow();
            dr4[0] = label196.Text;
            DataRow dr5 = timeParams.NewRow();
            dr5[0] = label197.Text;
            DataRow dr6 = timeParams.NewRow();
            dr6[0] = label198.Text;
            DataRow dr7 = timeParams.NewRow();
            dr7[0] = label199.Text;
            DataRow dr8 = timeParams.NewRow();
            dr8[0] = label200.Text;
            DataRow dr9 = timeParams.NewRow();
            dr9[0] = label201.Text;
            DataRow dr10 = timeParams.NewRow();
            dr10[0] = label202.Text;
            timeParams.Rows.Add(dr1);
            timeParams.Rows.Add(dr2);
            timeParams.Rows.Add(dr3);
            timeParams.Rows.Add(dr4);
            timeParams.Rows.Add(dr5);
            timeParams.Rows.Add(dr6);
            timeParams.Rows.Add(dr7);
            timeParams.Rows.Add(dr8);
            timeParams.Rows.Add(dr9);
            timeParams.Rows.Add(dr10);

            DataTable dtJson = new DataTable();
            dtJson.Columns.Add("moldelName");
            dtJson.Columns.Add("workQty");
            dtJson.Columns.Add("Target");
            dtJson.Columns.Add("OperatorNo");
            dtJson.Columns.Add("WaterSpiderNo");
            dtJson.Columns.Add("amfrom");
            dtJson.Columns.Add("amto");
            dtJson.Columns.Add("pmfrom");
            dtJson.Columns.Add("pmto");
            dtJson.Columns.Add("totalHours");

            for (int i = 0; i < timeParams.Rows.Count; i++)
            {
                string from =timeParams.Rows[i][0].ToString().Split('~')[0].ToString()+":"+"00";
                string to = timeParams.Rows[i][0].ToString().Split('~')[1].ToString()+":"+"00";
                string expression;
                expression = string.Format("INSERT_TIME>='{0}'  and  INSERT_TIME<'{1}'", from,to);
                DataRow[] foundRows;
                foundRows = pDtJson.Select(expression);
                Console.WriteLine("从{0}到{1}有{2}行",from,to,foundRows.Count());

                DataRow dr = dtJson.NewRow();
                if (foundRows.Count()>=1)
                {
                    dr[0] = foundRows[0][2];
                }    
                dr[1] = foundRows.Count();
                if (pOtherDtJson.Rows.Count > 0)
                {
                    dr[2] = pOtherDtJson.Rows[0][0];
                    dr[3] = pOtherDtJson.Rows[0][1];
                    dr[4] = pOtherDtJson.Rows[0][2];
                    dr[9] = pOtherDtJson.Rows[0][3];

                    dr[5] = pOtherDtJson.Rows[0][4];
                    dr[6] = pOtherDtJson.Rows[0][5];
                    dr[7] = pOtherDtJson.Rows[0][6];
                    dr[8] = pOtherDtJson.Rows[0][7];

                }
                dtJson.Rows.Add(dr);
            }


            this.Invoke(new Action(() =>
            {
                int totalFinishQty = 0;
                int totalOperatorHours = 0;
                string target = "0";
                double hourQty = 0;
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    target = dtJson.Rows[i][2].ToString();
                    hourQty = double.Parse(dtJson.Rows[i][9].ToString()) == 0 ? 0 : double.Parse(dtJson.Rows[i][2].ToString()) / double.Parse(dtJson.Rows[i][9].ToString());
                    totalFinishQty += int.Parse(dtJson.Rows[i][1].ToString());

                    if (i == 0)
                    {
                        double time1 = Convert.ToDateTime(label193.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label193.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label244.Text = dtJson.Rows[0][0].ToString();
                        label245.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label246.Text = dtJson.Rows[0][1].ToString();
                        label247.Text = (decimal.Parse(label246.Text.ToString()) - decimal.Parse(label245.Text.ToString())).ToString();
                        if (decimal.Parse(label246.Text.ToString()) - decimal.Parse(label245.Text.ToString()) < 0)
                        {
                            label247.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label247.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 1)
                    {
                        double time1 = Convert.ToDateTime(label194.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label194.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label243.Text = dtJson.Rows[1][0].ToString();
                        label252.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label253.Text = dtJson.Rows[1][1].ToString();
                        label254.Text = (decimal.Parse(label253.Text.ToString()) - decimal.Parse(label252.Text.ToString())).ToString();
                        if (decimal.Parse(label253.Text.ToString()) - decimal.Parse(label252.Text.ToString()) < 0)
                        {
                            label254.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label254.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 2)
                    {
                        double time1 = Convert.ToDateTime(label195.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label195.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label242.Text = dtJson.Rows[2][0].ToString();
                        label259.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label260.Text = dtJson.Rows[2][1].ToString();
                        label261.Text = (decimal.Parse(label260.Text.ToString()) - decimal.Parse(label259.Text.ToString())).ToString();
                        if (decimal.Parse(label260.Text.ToString()) - decimal.Parse(label259.Text.ToString()) < 0)
                        {
                            label261.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label261.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 3)
                    {
                        double time1 = Convert.ToDateTime(label196.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label196.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label241.Text = dtJson.Rows[3][0].ToString();
                        label266.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label267.Text = dtJson.Rows[3][1].ToString();
                        label268.Text = (decimal.Parse(label267.Text.ToString()) - decimal.Parse(label266.Text.ToString())).ToString();
                        if (decimal.Parse(label267.Text.ToString()) - decimal.Parse(label266.Text.ToString()) < 0)
                        {
                            label268.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label268.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 4)
                    {
                        double time1 = Convert.ToDateTime(label197.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label197.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        double time3 = dtJson.Rows[i][6] == null || string.IsNullOrEmpty(dtJson.Rows[i][6].ToString()) ? 0 : Convert.ToDateTime(dtJson.Rows[i][6].ToString()).TimeOfDay.TotalSeconds;
                        double time4 = dtJson.Rows[i][7] == null || string.IsNullOrEmpty(dtJson.Rows[i][7].ToString()) ? 0 : Convert.ToDateTime(dtJson.Rows[i][7].ToString()).TimeOfDay.TotalSeconds;
                        label240.Text = dtJson.Rows[4][0].ToString();
                        label273.Text = (hourQty * (time2 - time1 - (time4 - time3)) / 3600).ToString("0");
                        label274.Text = dtJson.Rows[4][1].ToString();
                        label275.Text = (decimal.Parse(label274.Text.ToString()) - decimal.Parse(label273.Text.ToString())).ToString();
                        if (decimal.Parse(label274.Text.ToString()) - decimal.Parse(label273.Text.ToString()) < 0)
                        {
                            label275.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label275.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 5)
                    {
                        double time1 = Convert.ToDateTime(label198.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label198.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label239.Text = dtJson.Rows[5][0].ToString();
                        label280.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label281.Text = dtJson.Rows[5][1].ToString();
                        label282.Text = (decimal.Parse(label281.Text.ToString()) - decimal.Parse(label280.Text.ToString())).ToString();
                        if (decimal.Parse(label281.Text.ToString()) - decimal.Parse(label280.Text.ToString()) < 0)
                        {
                            label282.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label282.BackColor = Color.FromArgb(255, 255, 255);
                        }

                    }
                    else if (i == 6)
                    {
                        double time1 = Convert.ToDateTime(label199.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label199.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label238.Text = dtJson.Rows[6][0].ToString();
                        label287.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label288.Text = dtJson.Rows[6][1].ToString();
                        label289.Text = (decimal.Parse(label288.Text.ToString()) - decimal.Parse(label287.Text.ToString())).ToString();
                        if (decimal.Parse(label288.Text.ToString()) - decimal.Parse(label287.Text.ToString()) < 0)
                        {
                            label289.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label289.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 7)
                    {
                        double time1 = Convert.ToDateTime(label200.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label200.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label237.Text = dtJson.Rows[7][0].ToString();
                        label294.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label295.Text = dtJson.Rows[7][1].ToString();
                        label296.Text = (decimal.Parse(label295.Text.ToString()) - decimal.Parse(label294.Text.ToString())).ToString();
                        if (decimal.Parse(label295.Text.ToString()) - decimal.Parse(label294.Text.ToString()) < 0)
                        {
                            label296.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label296.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 8)
                    {
                        double time1 = Convert.ToDateTime(label201.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label201.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label236.Text = dtJson.Rows[8][0].ToString();
                        label301.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label302.Text = dtJson.Rows[8][1].ToString();
                        label303.Text = (decimal.Parse(label302.Text.ToString()) - decimal.Parse(label301.Text.ToString())).ToString();
                        if (decimal.Parse(label302.Text.ToString()) - decimal.Parse(label301.Text.ToString()) < 0)
                        {
                            label303.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label303.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    else if (i == 9)
                    {
                        double time1 = Convert.ToDateTime(label202.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                        double time2 = Convert.ToDateTime(label202.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                        label235.Text = dtJson.Rows[9][0].ToString();
                        label308.Text = (hourQty * (time2 - time1) / 3600).ToString("0");
                        label309.Text = dtJson.Rows[9][1].ToString();
                        label310.Text = (decimal.Parse(label309.Text.ToString()) - decimal.Parse(label308.Text.ToString())).ToString();
                        if (decimal.Parse(label309.Text.ToString()) - decimal.Parse(label308.Text.ToString()) < 0)
                        {
                            label310.BackColor = Color.FromArgb(254, 205, 208);
                        }
                        else
                        {
                            label310.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }

                label316.Text = totalFinishQty.ToString();
                label315.Text = target;
                label317.Text = (totalFinishQty - int.Parse(target)).ToString("0");
                if (totalFinishQty - int.Parse(target) < 0)
                {
                    label317.BackColor = Color.FromArgb(254, 205, 208);
                }
                else
                {
                    label317.BackColor = Color.FromArgb(255, 255, 255);
                }
                //label32.Text = "√";
                //label33.Text = "√";
                //label34.Text = "√";
                //label35.Text = "√";
                //label36.Text = "√";
                //label37.Text = "√";
                //label38.Text = "√";
                //label39.Text = "√";
                //label51.Text = "";
                //label122.Text = "√";
            }));
        }

        /// <summary>
        /// 每小时生产管理表
        /// </summary>
        public void tabPage6_query()
        {
            try
            {
                if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString()) && !string.IsNullOrEmpty(txtLine.Text))
                {
                    DataTable timeParams = new DataTable();
                    timeParams.Columns.Add("time");
                    DataRow dr1 = timeParams.NewRow();
                    dr1[0] = label193.Text;
                    DataRow dr2 = timeParams.NewRow();
                    dr2[0] = label194.Text ;
                    DataRow dr3= timeParams.NewRow();
                    dr3[0] = label195.Text;
                    DataRow dr4 = timeParams.NewRow();
                    dr4[0] = label196.Text;
                    DataRow dr5 = timeParams.NewRow();
                    dr5[0] = label197.Text;
                    DataRow dr6 = timeParams.NewRow();
                    dr6[0] = label198.Text;
                    DataRow dr7= timeParams.NewRow();
                    dr7[0] = label199.Text;
                    DataRow dr8 = timeParams.NewRow();
                    dr8[0] = label200.Text;
                    DataRow dr9 = timeParams.NewRow();
                    dr9[0] = label201.Text;
                    DataRow dr10 = timeParams.NewRow();
                    dr10[0] = label202.Text;
                    timeParams.Rows.Add(dr1);
                    timeParams.Rows.Add(dr2);
                    timeParams.Rows.Add(dr3);
                    timeParams.Rows.Add(dr4);
                    timeParams.Rows.Add(dr5);
                    timeParams.Rows.Add(dr6);
                    timeParams.Rows.Add(dr7);
                    timeParams.Rows.Add(dr8);
                    timeParams.Rows.Add(dr9);
                    timeParams.Rows.Add(dr10);

                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("date", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    p.Add("line", txtLine.Text);
                    p.Add("clinetTimeParams", timeParams);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage6_Query", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();

                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        this.Invoke(new Action(() =>
                        {
                            int totalFinishQty = 0;
                            int totalOperatorHours = 0;
                            string target = "0";
                            double hourQty = 0;
                            for (int i=0;i< dtJson.Rows.Count;i++)
                            {
                                target = dtJson.Rows[i][2].ToString();
                                hourQty = double.Parse(dtJson.Rows[i][9].ToString())==0?0: double.Parse(dtJson.Rows[i][2].ToString())/ double.Parse(dtJson.Rows[i][9].ToString());
                                totalFinishQty += int.Parse(dtJson.Rows[i][1].ToString());

                                if (i==0)
                                {
                                    double time1 = Convert.ToDateTime(label193.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label193.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label244.Text = dtJson.Rows[0][0].ToString();
                                    label245.Text = (hourQty*(time2-time1)/3600).ToString();
                                    label246.Text = dtJson.Rows[0][1].ToString();
                                    label247.Text = (decimal.Parse(label246.Text.ToString())-decimal.Parse(label245.Text.ToString())).ToString();
                                    if (decimal.Parse(label246.Text.ToString()) - decimal.Parse(label245.Text.ToString())<0)
                                    {
                                        label247.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                                else if (i==1)
                                {
                                    double time1 = Convert.ToDateTime(label194.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label194.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label243.Text = dtJson.Rows[1][0].ToString();
                                    label252.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label253.Text = dtJson.Rows[1][1].ToString();
                                    label254.Text = (decimal.Parse(label253.Text.ToString())-decimal.Parse(label252.Text.ToString())).ToString();
                                    if (decimal.Parse(label253.Text.ToString()) - decimal.Parse(label252.Text.ToString()) < 0)
                                    {
                                        label254.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                                else if (i ==2)
                                {
                                    double time1 = Convert.ToDateTime(label195.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label195.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label242.Text = dtJson.Rows[2][0].ToString();
                                    label259.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label260.Text = dtJson.Rows[2][1].ToString();
                                    label261.Text = (decimal.Parse(label260.Text.ToString())-decimal.Parse(label259.Text.ToString())).ToString();
                                    if (decimal.Parse(label260.Text.ToString()) - decimal.Parse(label259.Text.ToString()) < 0)
                                    {
                                        label261.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                                else if (i ==3)
                                {
                                    double time1 = Convert.ToDateTime(label196.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label196.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label241.Text = dtJson.Rows[3][0].ToString();
                                    label266.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label267.Text = dtJson.Rows[3][1].ToString();
                                    label268.Text = (decimal.Parse(label267.Text.ToString())-decimal.Parse(label266.Text.ToString())).ToString();
                                    if (decimal.Parse(label267.Text.ToString()) - decimal.Parse(label266.Text.ToString()) < 0)
                                    {
                                        label268.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                                else if (i ==4)
                                {
                                    double time1 = Convert.ToDateTime(label197.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label197.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    double time3= dtJson.Rows[i][6]==null||string.IsNullOrEmpty(dtJson.Rows[i][6].ToString())?0:Convert.ToDateTime(dtJson.Rows[i][6].ToString()).TimeOfDay.TotalSeconds;
                                    double time4= dtJson.Rows[i][7] == null || string.IsNullOrEmpty(dtJson.Rows[i][7].ToString()) ? 0 : Convert.ToDateTime(dtJson.Rows[i][7].ToString()).TimeOfDay.TotalSeconds;
                                    label240.Text = dtJson.Rows[4][0].ToString();
                                    label273.Text = (hourQty * (time2 - time1-(time4-time3)) / 3600).ToString();
                                    label274.Text = dtJson.Rows[4][1].ToString();
                                    label275.Text = (decimal.Parse(label274.Text.ToString())-decimal.Parse(label273.Text.ToString())).ToString();
                                    if (decimal.Parse(label274.Text.ToString()) - decimal.Parse(label273.Text.ToString()) < 0)
                                    {
                                        label275.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                    else
                                    {
                                        label275.BackColor = Color.FromArgb(255, 255, 255);
                                    }
                                }
                                else if (i ==5)
                                {
                                    double time1 = Convert.ToDateTime(label198.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label198.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label239.Text = dtJson.Rows[5][0].ToString();
                                    label280.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label281.Text = dtJson.Rows[5][1].ToString();
                                    label282.Text = (decimal.Parse(label281.Text.ToString())-decimal.Parse(label280.Text.ToString())).ToString();
                                    if (decimal.Parse(label281.Text.ToString()) - decimal.Parse(label280.Text.ToString()) < 0)
                                    {
                                        label282.BackColor = Color.FromArgb(254, 205, 208);
                                    }

                                }
                                else if (i ==6)
                                {
                                    double time1 = Convert.ToDateTime(label199.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label199.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label238.Text = dtJson.Rows[6][0].ToString();
                                    label287.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label288.Text = dtJson.Rows[6][1].ToString();
                                    label289.Text = (decimal.Parse(label288.Text.ToString())-decimal.Parse(label287.Text.ToString())).ToString();
                                    if (decimal.Parse(label288.Text.ToString()) - decimal.Parse(label287.Text.ToString()) < 0)
                                    {
                                        label289.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                                else if (i ==7)
                                {
                                    double time1 = Convert.ToDateTime(label200.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label200.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label237.Text = dtJson.Rows[7][0].ToString();
                                    label294.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label295.Text = dtJson.Rows[7][1].ToString();
                                    label296.Text = (decimal.Parse(label295.Text.ToString())-decimal.Parse(label294.Text.ToString())).ToString();
                                    if (decimal.Parse(label295.Text.ToString()) - decimal.Parse(label294.Text.ToString()) < 0)
                                    {
                                        label296.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                                else if (i == 8)
                                {
                                    double time1 = Convert.ToDateTime(label201.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label201.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label236.Text = dtJson.Rows[8][0].ToString();
                                    label301.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label302.Text = dtJson.Rows[8][1].ToString();
                                    label303.Text = (decimal.Parse(label302.Text.ToString())-decimal.Parse(label301.Text.ToString())).ToString();
                                    if (decimal.Parse(label302.Text.ToString()) - decimal.Parse(label301.Text.ToString()) < 0)
                                    {
                                        label303.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                                else if (i == 9)
                                {
                                    double time1 = Convert.ToDateTime(label202.Text.Split('~')[0].ToString()).TimeOfDay.TotalSeconds;
                                    double time2 = Convert.ToDateTime(label202.Text.Split('~')[1].ToString()).TimeOfDay.TotalSeconds;
                                    label235.Text = dtJson.Rows[9][0].ToString();
                                    label308.Text = (hourQty * (time2 - time1) / 3600).ToString();
                                    label309.Text = dtJson.Rows[9][1].ToString();
                                    label310.Text = (decimal.Parse(label309.Text.ToString())-decimal.Parse(label308.Text.ToString())).ToString();
                                    if (decimal.Parse(label309.Text.ToString()) - decimal.Parse(label308.Text.ToString()) < 0)
                                    {
                                        label310.BackColor = Color.FromArgb(254, 205, 208);
                                    }
                                }
                            }

                            label316.Text = totalFinishQty.ToString();
                            label315.Text = target;
                            label317.Text = (totalFinishQty - int.Parse(target)).ToString();
                            if (totalFinishQty -int.Parse(target) < 0)
                            {
                            label317.BackColor = Color.FromArgb(254, 205, 208);
                            }
                            //label32.Text = "√";
                            //label33.Text = "√";
                            //label34.Text = "√";
                            //label35.Text = "√";
                            //label36.Text = "√";
                            //label37.Text = "√";
                            //label38.Text = "√";
                            //label39.Text = "√";
                            //label51.Text = "";
                            //label122.Text = "√";
                        }));
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void tabPage7_query()
        {
            //try
            //{
            //    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage7_Query", Program.client.UserToken, string.Empty);
            //    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            //    {
            //        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
            //        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            //        this.Invoke(new Action(() =>
            //        {

            //        }));
            //    }
            //    else
            //    {
            //        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        /// <summary>
        /// 日小时产量表
        /// </summary>
        public void tabPage8_query()
        {
            try
            {
                if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString())&&!string.IsNullOrEmpty(txtLine.Text))
                {
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("date", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    p.Add("line", txtLine.Text);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage8_Query", Program.Client.UserToken,Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        this.Invoke(new Action(() =>
                        {
                            label226.Text = Convert.ToInt32(dtJson.Rows[0]["PresentQty"].ToString()).ToString();
                            label208.Text = dtJson.Rows[0]["SumQty"].ToString();
                            label207.Text = Convert.ToInt32(dtJson.Rows[0]["HourTargetQty"].ToString()).ToString();
                            label206.Text = dtJson.Rows[0]["HourQty"].ToString();
                            label204.Text = Convert.ToDouble(dtJson.Rows[0]["Ration"].ToString()).ToString("0.0");
                            label228.Text = dtJson.Rows[0]["Title"].ToString();
                            label230.Text = Convert.ToInt32(dtJson.Rows[0]["Target"].ToString()).ToString();
                            label205.Text = Convert.ToDouble(dtJson.Rows[0]["BA"].ToString()).ToString("0.0");
                        }));
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void tabPage9_query()
        {
            //try
            //{
            //    if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString()) && !string.IsNullOrEmpty(txtLine.Text))
            //    {
            //        Dictionary<string, Object> p = new Dictionary<string, object>();
            //        p.Add("date", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
            //        p.Add("line", txtLine.Text);
            //        string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.Production_KanbanServer", "TabPage8_Query", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            //        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            //        {
            //            string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
            //            DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            //            this.Invoke(new Action(() =>
            //            {
            //                label226.Text = Convert.ToInt32(dtJson.Rows[0]["PresentQty"].ToString()).ToString();
            //                label208.Text = dtJson.Rows[0]["SumQty"].ToString();
            //                label207.Text = dtJson.Rows[0]["HourTargetQty"].ToString();
            //                label206.Text = dtJson.Rows[0]["HourQty"].ToString();
            //                label204.Text = Convert.ToDouble(dtJson.Rows[0]["Ration"].ToString()).ToString("0.0");
            //                label228.Text = dtJson.Rows[0]["Title"].ToString();
            //                label230.Text = Convert.ToInt32(dtJson.Rows[0]["Target"].ToString()).ToString();
            //                label205.Text = Convert.ToDouble(dtJson.Rows[0]["BA"].ToString()).ToString("0.0");
            //            }));
            //        }
            //        else
            //        {
            //            SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        public void Set_Page1Text(DataTable dtJson)
        {
            label3.Text = dtJson.Rows[0]["ModelName"].ToString();
            label53.Text = dtJson.Rows[0]["Date"].ToString();
            label54.Text = dtJson.Rows[0]["Target"].ToString();
            label55.Text = dtJson.Rows[0]["TaktTime"].ToString();
            label56.Text = dtJson.Rows[0]["WaterSpiderNo"].ToString();
            //label58.Text = dtJson.Rows[0]["ModelTHT"].ToString();
            label52.Text = dtJson.Rows[0]["OperatorNo"].ToString();
        }

        public void Set_Page8Text(DataTable dtJson)
        {
            label226.Text = Convert.ToInt32(dtJson.Rows[0]["PresentQty"].ToString()).ToString();
            label208.Text = dtJson.Rows[0]["SumQty"].ToString();
            label207.Text = dtJson.Rows[0]["HourTargetQty"].ToString();
            label206.Text = dtJson.Rows[0]["HourQty"].ToString();
            label204.Text = Convert.ToDouble(dtJson.Rows[0]["Ration"].ToString()).ToString("0.0");
            label228.Text = dtJson.Rows[0]["Title"].ToString();
            label230.Text = Convert.ToInt32(dtJson.Rows[0]["Target"].ToString()).ToString();
            label205.Text = Convert.ToDouble(dtJson.Rows[0]["BA"].ToString()).ToString("0.0");
        }

        delegate void SetTextCallback(Label lable,string text);

        private void SetText(Label label, string text)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    SetTextCallback stcb = new SetTextCallback(SetText);
                    label.Invoke(stcb, new object[] { label, text });
                }
                else
                {
                    label.Text = text;
                }
            }
        }

        private void label32_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void label33_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void label34_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void label35_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void label36_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void label37_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void label38_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void label39_Click(object sender, EventArgs e)
        {
            //TierMettingMaintain frm = new TierMettingMaintain(label49.Text);
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Tick -= new EventHandler(timer1_Tick);//添加事件
            try
            {
                timer1.Interval = int.Parse(txtTimer.Text)*1000;
                MessageBox.Show("设置成功！", "提示：");
            }
            catch
            {
                MessageBox.Show("请输入正确的整数！", "提示：");
            }
            timer1.Tick += new EventHandler(timer1_Tick);//添加事件
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Tick -= new EventHandler(timer1_Tick);//添加事件
        }

        private void Production_KanbanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            timer1.Tick -= new EventHandler(timer1_Tick);//添加事件
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearData();
        }
        #region APH changed
        private void GetTier1() {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("dept", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetTier1",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                TextToCheckbox(cbxTier1_1, dtJson.Rows[0]["G_1"].ToString());
                TextToCheckbox(cbxTier1_2, dtJson.Rows[0]["G_2"].ToString());
                TextToCheckbox(cbxTier1_3, dtJson.Rows[0]["G_3"].ToString());
                TextToCheckbox(cbxTier1_4, dtJson.Rows[0]["G_4"].ToString());
                TextToCheckbox(cbxTier1_5, dtJson.Rows[0]["G_5"].ToString());
                TextToCheckbox(cbxTier1_6, dtJson.Rows[0]["G_6"].ToString());
                TextToCheckbox(cbxTier1_7, dtJson.Rows[0]["G_7"].ToString());
                TextToCheckbox(cbxTier1_8, dtJson.Rows[0]["G_8"].ToString());
                rtbTier1_1.Text = dtJson.Rows[0]["G_RESULT"].ToString();
                rtbTier1_2.Text = dtJson.Rows[0]["G_AUDITOR"].ToString();
                if (!string.IsNullOrEmpty(dtJson.Rows[0]["G_LAST_UPDATE_DATE"].ToString())) {
                    lblTier1Time.Text = dtJson.Rows[0]["G_LAST_UPDATE_DATE"].ToString();
                } else {
                    lblTier1Time.Text = dtJson.Rows[0]["G_CREATED_DATE"].ToString();
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void GetTier1Standard()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("dept", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetTier1Standard",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                TextToCheckbox(cbxStandard1_1, dtJson.Rows[0]["G_SUPERVISOR_1"].ToString());
                TextToCheckbox(cbxStandard1_2, dtJson.Rows[0]["G_SUPERVISOR_2"].ToString());
                TextToCheckbox(cbxStandard1_3, dtJson.Rows[0]["G_SUPERVISOR_3"].ToString());
                TextToCheckbox(cbxStandard1_4, dtJson.Rows[0]["G_SUPERVISOR_4"].ToString());
                TextToCheckbox(cbxStandard1_5, dtJson.Rows[0]["G_SUPERVISOR_5"].ToString());
                TextToCheckbox(cbxStandard1_6, dtJson.Rows[0]["G_SUPERVISOR_6"].ToString());
                TextToCheckbox(cbxStandard1_7, dtJson.Rows[0]["G_SUPERVISOR_7"].ToString());
                TextToCheckbox(cbxStandard1_8, dtJson.Rows[0]["G_SUPERVISOR_8"].ToString());
                rtbStandard1.Text = dtJson.Rows[0]["G_SUPERVISOR_AUDITOR"].ToString();
                TextToCheckbox(cbxStandard2_1, dtJson.Rows[0]["G_VSM_1"].ToString());
                TextToCheckbox(cbxStandard2_2, dtJson.Rows[0]["G_VSM_2"].ToString());
                TextToCheckbox(cbxStandard2_3, dtJson.Rows[0]["G_VSM_3"].ToString());
                TextToCheckbox(cbxStandard2_4, dtJson.Rows[0]["G_VSM_4"].ToString());
                TextToCheckbox(cbxStandard2_5, dtJson.Rows[0]["G_VSM_5"].ToString());
                TextToCheckbox(cbxStandard2_6, dtJson.Rows[0]["G_VSM_6"].ToString());
                TextToCheckbox(cbxStandard2_7, dtJson.Rows[0]["G_VSM_7"].ToString());
                TextToCheckbox(cbxStandard2_8, dtJson.Rows[0]["G_VSM_8"].ToString());
                rtbStandard2.Text = dtJson.Rows[0]["G_VSM_AUDITOR"].ToString();
                TextToCheckbox(cbxStandard3_1, dtJson.Rows[0]["G_THIRD_PARTY_1"].ToString());
                TextToCheckbox(cbxStandard3_2, dtJson.Rows[0]["G_THIRD_PARTY_2"].ToString());
                TextToCheckbox(cbxStandard3_3, dtJson.Rows[0]["G_THIRD_PARTY_3"].ToString());
                TextToCheckbox(cbxStandard3_4, dtJson.Rows[0]["G_THIRD_PARTY_4"].ToString());
                TextToCheckbox(cbxStandard3_5, dtJson.Rows[0]["G_THIRD_PARTY_5"].ToString());
                TextToCheckbox(cbxStandard3_6, dtJson.Rows[0]["G_THIRD_PARTY_6"].ToString());
                TextToCheckbox(cbxStandard3_7, dtJson.Rows[0]["G_THIRD_PARTY_7"].ToString());
                TextToCheckbox(cbxStandard3_8, dtJson.Rows[0]["G_THIRD_PARTY_8"].ToString());
                rtbStandard3.Text = dtJson.Rows[0]["G_THIRD_PARTY_AUDITOR"].ToString();
                if (!string.IsNullOrEmpty(dtJson.Rows[0]["G_LAST_UPDATE_DATE"].ToString()))
                {
                    lblStandardTime.Text = dtJson.Rows[0]["G_LAST_UPDATE_DATE"].ToString();
                }
                else
                {
                    lblStandardTime.Text = dtJson.Rows[0]["G_CREATED_DATE"].ToString();
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void btnTier1Save_Click(object sender, EventArgs e)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("G_DEPTCODE", txtLine.Text);
            p.Add("G_DATE", dateTimePicker1.Value);
            p.Add("G_1", CheckboxToText(cbxTier1_1));
            p.Add("G_2", CheckboxToText(cbxTier1_2));
            p.Add("G_3", CheckboxToText(cbxTier1_3));
            p.Add("G_4", CheckboxToText(cbxTier1_4));
            p.Add("G_5", CheckboxToText(cbxTier1_5));
            p.Add("G_6", CheckboxToText(cbxTier1_6));
            p.Add("G_7", CheckboxToText(cbxTier1_7));
            p.Add("G_8", CheckboxToText(cbxTier1_8));
            p.Add("G_RESULT", rtbTier1_1.Text);
            p.Add("G_AUDITOR", rtbTier1_2.Text);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "SaveTier1",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void btnStandardSave_Click(object sender, EventArgs e)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("G_DEPTCODE", txtLine.Text);
            p.Add("G_DATE", dateTimePicker1.Value);
            p.Add("G_SUPERVISOR_1", CheckboxToText(cbxStandard1_1));
            p.Add("G_SUPERVISOR_2", CheckboxToText(cbxStandard1_2));
            p.Add("G_SUPERVISOR_3", CheckboxToText(cbxStandard1_3));
            p.Add("G_SUPERVISOR_4", CheckboxToText(cbxStandard1_4));
            p.Add("G_SUPERVISOR_5", CheckboxToText(cbxStandard1_5));
            p.Add("G_SUPERVISOR_6", CheckboxToText(cbxStandard1_6));
            p.Add("G_SUPERVISOR_7", CheckboxToText(cbxStandard1_7));
            p.Add("G_SUPERVISOR_8", CheckboxToText(cbxStandard1_8));
            p.Add("G_SUPERVISOR_AUDITOR", rtbStandard1.Text);
            p.Add("G_VSM_1", CheckboxToText(cbxStandard2_1));
            p.Add("G_VSM_2", CheckboxToText(cbxStandard2_2));
            p.Add("G_VSM_3", CheckboxToText(cbxStandard2_3));
            p.Add("G_VSM_4", CheckboxToText(cbxStandard2_4));
            p.Add("G_VSM_5", CheckboxToText(cbxStandard2_5));
            p.Add("G_VSM_6", CheckboxToText(cbxStandard2_6));
            p.Add("G_VSM_7", CheckboxToText(cbxStandard2_7));
            p.Add("G_VSM_8", CheckboxToText(cbxStandard2_8));
            p.Add("G_VSM_AUDITOR", rtbStandard2.Text);
            p.Add("G_THIRD_PARTY_1", CheckboxToText(cbxStandard3_1));
            p.Add("G_THIRD_PARTY_2", CheckboxToText(cbxStandard3_2));
            p.Add("G_THIRD_PARTY_3", CheckboxToText(cbxStandard3_3));
            p.Add("G_THIRD_PARTY_4", CheckboxToText(cbxStandard3_4));
            p.Add("G_THIRD_PARTY_5", CheckboxToText(cbxStandard3_5));
            p.Add("G_THIRD_PARTY_6", CheckboxToText(cbxStandard3_6));
            p.Add("G_THIRD_PARTY_7", CheckboxToText(cbxStandard3_7));
            p.Add("G_THIRD_PARTY_8", CheckboxToText(cbxStandard3_8));
            p.Add("G_THIRD_PARTY_AUDITOR", rtbStandard3.Text);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "SaveTier1Standard",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {

            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private string CheckboxToText(CheckBox cbxName)
        {
            string str = "";
            str = cbxName.Checked ? "Y" : "N";
            return str;
        }
        private string TextToCheckbox(CheckBox cbxName, string val)
        {
            if (val.Equals("Y"))
            {
                cbxName.Checked = true;
            }
            else
            {
                cbxName.Checked = false;
            }
            return "";
        }

        #endregion

        private void rtbTHT_TextChanged(object sender, EventArgs e)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("ART", ART);
            p.Add("THT", rtbTHT.Text);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "SaveTHT",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {

            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void GetTier1_WeekSafety() {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekSafety",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count - 1; i++) {
                    string lblDateName = "lblDate" + (i + 1);
                    Label lblDate = this.Controls.Find(lblDateName, true).FirstOrDefault() as Label;
                    DateTime tempDate = Convert.ToDateTime(dtJson.Rows[i]["day"].ToString());
                    SetText(lblDate, tempDate.ToString("yyyy/MM/dd"));
                    if (tempDate <= dateTimePicker1.Value) {
                        string lblSafetyTargetName = "lbl" + (i + 1) + "_1";
                        string lblSafetyActualName = "lbl" + (i + 1) + "_2";
                        Label lblSafetyTarget = this.Controls.Find(lblSafetyTargetName, true).FirstOrDefault() as Label;
                        Label lblSafetyActual = this.Controls.Find(lblSafetyActualName, true).FirstOrDefault() as Label;
                        SetText(lblSafetyTarget, "0");
                        SetText(lblSafetyActual, dtJson.Rows[i]["count"].ToString());
                    }
                }
                DisableFuture();
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void GetTier1_WeekPPHTarget()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekPPHTarget",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for(int i = 0; i<dtJson.Rows.Count;i++)
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["WORK_day"].ToString()))
                    {
                        string lblPPHTargetName = "lbl" + (i + 1) + "_7";
                        Label lblPPHTarget = this.Controls.Find(lblPPHTargetName, true).FirstOrDefault() as Label;
                        SetText(lblPPHTarget, dtJson.Rows[i]["PPHTarget"].ToString());
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void GetTier1_WeekPPH()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekPPH",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                double pph = 0;
                DataTable dtPPH = new DataTable();
                dtPPH.Columns.Add("PPH");
                dtPPH.Columns.Add("DATE");
                foreach (DataRow r in dtJson.Rows)
                {
                    double tempPPH = 0;
                    DataRow dr = dtPPH.NewRow();
                    if (r["MANPOWER"] != null)
                    {
                        if (!(string.IsNullOrEmpty(r["MANPOWER"].ToString()) || r["MANPOWER"].ToString() == "0"))
                        {
                            tempPPH = r["QTY"].ToDouble() / r["MANPOWER"].ToDouble();
                            tempPPH = Math.Round(tempPPH, 2);                           
                        }
                        else
                        {
                            tempPPH = 0;
                        }
                    }
                    if (dateTimePicker1.Value >= DateTime.Parse(r["WORK_DATE"].ToString()))
                    {
                        pph = tempPPH;
                        dr["PPH"] = tempPPH;
                        dr["DATE"] = DateTime.Parse(r["WORK_DATE"].ToString()).ToString("MM/dd");
                        dtPPH.Rows.Add(dr);
                    }
                }
                for (int i = 0; i < dtPPH.Rows.Count; i++)
                {
                    string lblPPHActualName = "lbl" + (i + 1) + "_8";
                    Label lblPPHActual = this.Controls.Find(lblPPHActualName, true).FirstOrDefault() as Label;
                    SetText(lblPPHActual, dtPPH.Rows[i]["PPH"].ToString());
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void GetTier1_WeekRFT()
        {
            string RFTTarget = GetDeptType();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDept", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekRFT",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for(int i =0; i<dtJson.Rows.Count;i++) 
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["RIQI"].ToString()))
                    {
                        string lblRFTTargetName = "lbl" + (i + 1) + "_3";
                        string lblRFTActualName = "lbl" + (i + 1) + "_4";
                        Label lblRFTTarget = this.Controls.Find(lblRFTTargetName, true).FirstOrDefault() as Label;
                        Label lblRFTActual = this.Controls.Find(lblRFTActualName, true).FirstOrDefault() as Label;
                        SetText(lblRFTTarget, RFTTarget);
                        SetText(lblRFTActual, dtJson.Rows[i]["RFT"].ToString());
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private string GetDeptType()
        {
            string RFT = "";
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("dept", txtLine.Text);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "GetDeptType",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                switch (dtJson.Rows[0][0]) {
                    case "B":
                        RFT = "97";
                        break;
                    case "C":
                        RFT = "90";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return RFT;
        }
        private void GetTier1_WeekOutput()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekOutput",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["Work_day"].ToString()))
                    {
                        string lblOutputTargetName = "lbl" + (i + 1) + "_5";
                        string lblOutputActualName = "lbl" + (i + 1) + "_6";
                        Label lblOutputTarget = this.Controls.Find(lblOutputTargetName, true).FirstOrDefault() as Label;
                        Label lblOutputActual = this.Controls.Find(lblOutputActualName, true).FirstOrDefault() as Label;
                        SetText(lblOutputTarget, dtJson.Rows[i]["WORK_QTY"].ToString());
                        SetText(lblOutputActual, dtJson.Rows[i]["QTY"].ToString());
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void GetTier1_Kaizen()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("dept", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_Kaizen",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                SetText(lblKaizen1, dtJson.Rows[0]["MONTH"].ToString());
                SetText(lblKaizen2, dtJson.Rows[0]["YEAR"].ToString());
                SetText(lblKaizen3, dtJson.Rows[0]["PERPEOPLE"].ToString());
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void GetTier1_WeekLLER()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekLLER",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for(int i = 0; i <dtJson.Rows.Count;i++)
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["work_day"].ToString()))
                    {
                        string lblLLERTargetName = "lbl" + (i + 1) + "_9";
                        string lblLLERActualName = "lbl" + (i + 1) + "_10";
                        Label lblLLERTarget = this.Controls.Find(lblLLERTargetName, true).FirstOrDefault() as Label;
                        Label lblLLERActual = this.Controls.Find(lblLLERActualName, true).FirstOrDefault() as Label;
                        SetText(lblLLERTarget, "85");
                        SetText(lblLLERActual, dtJson.Rows[i]["LLER"].ToString());
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void GetTier1_WeekMulti()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekMulti",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["work_day"].ToString()))
                    {
                        string lblMultiTargetName = "lbl" + (i + 1) + "_11";
                        string lblMultiActualName = "lbl" + (i + 1) + "_12";
                        Label lblMultiTarget = this.Controls.Find(lblMultiTargetName, true).FirstOrDefault() as Label;
                        Label lblMultiActual = this.Controls.Find(lblMultiActualName, true).FirstOrDefault() as Label;
                        SetText(lblMultiTarget, "40");
                        SetText(lblMultiActual, dtJson.Rows[i]["Multi"].ToString());
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void SaveDownTime(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)sender;
            if (rtb != null)
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("dept", txtLine.Text);
                p.Add("downtime", rtb.Text);
                switch (rtb.Name)
                {
                    case "rtb1_16":
                        p.Add("date", lblDate1.Text);
                        break;
                    case "rtb2_16":
                        p.Add("date", lblDate2.Text);
                        break;
                    case "rtb3_16":
                        p.Add("date", lblDate3.Text);
                        break;
                    case "rtb4_16":
                        p.Add("date", lblDate4.Text);
                        break;
                    case "rtb5_16":
                        p.Add("date", lblDate5.Text);
                        break;
                    case "rtb6_16":
                        p.Add("date", lblDate6.Text);
                        break;
                    default:
                        break;
                }
                string ret =
                    SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                        Program.Client.APIURL,
                        "TierMeeting",
                        "TierMeeting.Controllers.TierMeetingServer",
                                            "SaveTier1_Downtime",
                        Program.Client.UserToken,
                        JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {

                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }

        private void DisableFuture() {
            for (int i = 1; i < 7; i++) {
                string lblDateName = "lblDate" + i;
                Label lblDate = this.Controls.Find(lblDateName, true).FirstOrDefault() as Label;
                DateTime temp = Convert.ToDateTime(lblDate.Text);
                if (temp > dateTimePicker1.Value) {
                    string lblDTTargetName = "lbl"+i+"_15";
                    Label lblDTTarget = this.Controls.Find(lblDTTargetName, true).FirstOrDefault() as Label;
                    lblDTTarget.Text = "";
                    string rtbDTActualName = "rtb"+i+"_16";
                    RichTextBox rtbDTActual = this.Controls.Find(rtbDTActualName, true).FirstOrDefault() as RichTextBox;
                    rtbDTActual.Enabled = false;
                    string rtbCOTTargetName = "rtb" + i + "_17";
                    RichTextBox rtbCOTTarget = this.Controls.Find(rtbCOTTargetName, true).FirstOrDefault() as RichTextBox;
                    rtbCOTTarget.Enabled = false;
                    string rtbCOTActualName = "rtb" + i + "_18";
                    RichTextBox rtbCOTActual = this.Controls.Find(rtbCOTActualName, true).FirstOrDefault() as RichTextBox;
                    rtbCOTActual.Enabled = false;
                }
            }
        }
        private void GetTier1_WeekDT()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekDowntime",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["G_date"].ToString()))
                    {
                        string rtbDTActualName = "rtb" + (i + 1) + "_16";
                        RichTextBox rtbDTActual = this.Controls.Find(rtbDTActualName, true).FirstOrDefault() as RichTextBox;
                        rtbDTActual.Text=dtJson.Rows[i]["G_DOWNTIME"].ToString();
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void SaveCOTTarget(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)sender;
            if (rtb != null)
            {
                if (rtb.Text == "")
                    return;
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("dept", txtLine.Text);
                p.Add("target", rtb.Text);
                p.Add("actual", "");
                switch (rtb.Name)
                {
                    case "rtb1_17":
                        p.Add("date", lblDate1.Text);
                        break;
                    case "rtb2_17":
                        p.Add("date", lblDate2.Text);
                        break;
                    case "rtb3_17":
                        p.Add("date", lblDate3.Text);
                        break;
                    case "rtb4_17":
                        p.Add("date", lblDate4.Text);
                        break;
                    case "rtb5_17":
                        p.Add("date", lblDate5.Text);
                        break;
                    case "rtb6_17":
                        p.Add("date", lblDate6.Text);
                        break;
                    default:
                        break;
                }
                string ret =
                    SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                        Program.Client.APIURL,
                        "TierMeeting",
                        "TierMeeting.Controllers.TierMeetingServer",
                                            "SaveTier1_COT",
                        Program.Client.UserToken,
                        JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {

                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }
        private void SaveCOTActual(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)sender;
            if (rtb != null)
            {
                if (rtb.Text == "")
                    return;
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("dept", txtLine.Text);
                p.Add("target", "");
                p.Add("actual", rtb.Text);
                switch (rtb.Name)
                {
                    case "rtb1_18":
                        p.Add("date", lblDate1.Text);
                        break;
                    case "rtb2_18":
                        p.Add("date", lblDate2.Text);
                        break;
                    case "rtb3_18":
                        p.Add("date", lblDate3.Text);
                        break;
                    case "rtb4_18":
                        p.Add("date", lblDate4.Text);
                        break;
                    case "rtb5_18":
                        p.Add("date", lblDate5.Text);
                        break;
                    case "rtb6_18":
                        p.Add("date", lblDate6.Text);
                        break;
                    default:
                        break;
                }
                string ret =
                    SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                        Program.Client.APIURL,
                        "TierMeeting",
                        "TierMeeting.Controllers.TierMeetingServer",
                                            "SaveTier1_COT",
                        Program.Client.UserToken,
                        JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {

                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }
        private void GetTier1_WeekCOT()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekCOT",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["G_date"].ToString()))
                    {
                        string rtbCOTTargetName = "rtb" + (i + 1) + "_17";
                        string rtbCOTActualName = "rtb" + (i + 1) + "_18";
                        RichTextBox rtbCOTTarget = this.Controls.Find(rtbCOTTargetName, true).FirstOrDefault() as RichTextBox;
                        RichTextBox rtbCOTActual = this.Controls.Find(rtbCOTActualName, true).FirstOrDefault() as RichTextBox;
                        rtbCOTTarget.Text = dtJson.Rows[i]["G_TARGET_COT"].ToString();
                        rtbCOTActual.Text = dtJson.Rows[i]["G_ACTUAL_COT"].ToString();
                    }
                }
              //  SetTextChanged();
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            SetTextChanged();
        }
        private void SetTextChanged()
        {
            rtb1_16.TextChanged += new EventHandler(SaveDownTime);
            rtb2_16.TextChanged += new EventHandler(SaveDownTime);
            rtb3_16.TextChanged += new EventHandler(SaveDownTime);
            rtb4_16.TextChanged += new EventHandler(SaveDownTime);
            rtb5_16.TextChanged += new EventHandler(SaveDownTime);
            rtb6_16.TextChanged += new EventHandler(SaveDownTime);
            rtb1_17.TextChanged += new EventHandler(SaveCOTTarget);
            rtb2_17.TextChanged += new EventHandler(SaveCOTTarget);
            rtb3_17.TextChanged += new EventHandler(SaveCOTTarget);
            rtb4_17.TextChanged += new EventHandler(SaveCOTTarget);
            rtb5_17.TextChanged += new EventHandler(SaveCOTTarget);
            rtb6_17.TextChanged += new EventHandler(SaveCOTTarget);
            rtb1_18.TextChanged += new EventHandler(SaveCOTActual);
            rtb2_18.TextChanged += new EventHandler(SaveCOTActual);
            rtb3_18.TextChanged += new EventHandler(SaveCOTActual);
            rtb4_18.TextChanged += new EventHandler(SaveCOTActual);
            rtb5_18.TextChanged += new EventHandler(SaveCOTActual);
            rtb6_18.TextChanged += new EventHandler(SaveCOTActual);
        }
        private void GetTier1_WeekWIP()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vLine", txtLine.Text);
            p.Add("date", dateTimePicker1.Value);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "TierMeeting",
                    "TierMeeting.Controllers.TierMeetingServer",
                                        "Tier1_WeekHourlyOutput",
                    Program.Client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    if (dateTimePicker1.Value >= DateTime.Parse(dtJson.Rows[i]["WORK_day"].ToString()))
                    {
                        string lblWIPTargetName = "lbl" + (i + 1) + "_13";
                        string lblWIPActualName = "lbl" + (i + 1) + "_14";
                        Label lblWIPTarget = this.Controls.Find(lblWIPTargetName, true).FirstOrDefault() as Label;
                        Label lblWIPActual = this.Controls.Find(lblWIPActualName, true).FirstOrDefault() as Label;
                        int target = 0;
                        int actual = 0;
                        if (dtJson.Rows[i]["work_hours"].ToInt() > 0)
                        {
                            target  = dtJson.Rows[i]["work_qty"].ToInt() / dtJson.Rows[i]["work_hours"].ToInt();
                            target = target > 60 ? 240 : target * 4;
                            actual = dtJson.Rows[i]["qty"].ToInt() / dtJson.Rows[i]["work_hours"].ToInt();
                        }                        
                        SetText(lblWIPTarget, target.ToString());
                        SetText(lblWIPActual, actual.ToString());
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void ClearData()
        {
            //clear Richtextbox
           for(int i=16;i<=18;i++)
                for(int j=1;j<=6;j++)
                {
                    RichTextBox rtb = this.Controls.Find("rtb"+j+"_"+i, true).FirstOrDefault() as RichTextBox;
                    rtb.Text = "";
                }
            //clear label
            for (int j = 1; j <= 6; j++)
             for (int i = 1; i <= 15; i++)               
             {
                    Label lbl = this.Controls.Find("lbl" + j + "_"+i, true).FirstOrDefault() as Label;
                    lbl.Text = "";
             }
            for (int i = 1; i <= 8; i++)
            {
                //clear checkbox Tier
                CheckBox cbb1 = this.Controls.Find("cbxTier1_" + i, true).FirstOrDefault() as CheckBox;
                cbb1.Checked = false;
                for (int j = 1; j <= 3; j++)
                {
                    //clear cbxStandard
                    CheckBox cbb = this.Controls.Find("cbxStandard" + j + "_" + i, true).FirstOrDefault() as CheckBox;
                    cbb.Checked = false;
                }
            }
            for (int i = 234; i <= 321; i++)
            {
                Label lbl_mangesheet = this.Controls.Find("label" + i, true).FirstOrDefault() as Label;
                lbl_mangesheet.Text = "";
            }
            for(int i=1;i<=3;i++)
            {
                RichTextBox rtbStandard = this.Controls.Find("rtbStandard" + i, true).FirstOrDefault() as RichTextBox;
                rtbStandard.Text = "";
                Label lblKaizen = this.Controls.Find("lblKaizen" + i, true).FirstOrDefault() as Label;
                lblKaizen.Text = "";              
            }
            rtbTier1_1.Text = "";
            rtbTier1_2.Text = "";

        }
    }
}
