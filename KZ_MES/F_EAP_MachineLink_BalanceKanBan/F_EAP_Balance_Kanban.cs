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
    public partial class F_EAP_Balance_Kanban : MaterialForm
    {
        string d_dept = "";
        string ovenId = "";
        string pressId = "";
        string freezerId = "";

        //设备状态
        string ovenStatus = "";
        string pressStatus = "";
        string freezerStatus = "";

        //烘箱参数
        string Oven_KWH = "";
        string Oven_SPEED = "";

        string Oven_UPER_SETUP1 = "";
        string Oven_UUCL1 = "";
        string Oven_ULCL1 = "";
        string Oven_UPER_ACTUAL1 = "";
        string Oven_LOWER_SETUP1 = "";
        string Oven_LUCL1 = "";
        string Oven_LLCL1 = "";
        string Oven_LOWER_ACTUAL1 = "";

        string Oven_UPER_SETUP2 = "";
        string Oven_UUCL2 = "";
        string Oven_ULCL2 = "";
        string Oven_UPER_ACTUAL2 = "";
        string Oven_LOWER_SETUP2 = "";
        string Oven_LUCL2 = "";
        string Oven_LLCL2 = "";
        string Oven_LOWER_ACTUAL2 = "";

        string Oven_UPER_SETUP3 = "";
        string Oven_UUCL3 = "";
        string Oven_ULCL3 = "";
        string Oven_UPER_ACTUAL3 = "";
        string Oven_LOWER_SETUP3 = "";
        string Oven_LUCL3 = "";
        string Oven_LLCL3 = "";
        string Oven_LOWER_ACTUAL3 = "";


        //压机参数
        string Press_KWH = "";
        string Press_LP_Average = "";
        string Press_RP_Average = "";


        //冷冻机参数
        string Freezer_KWH = "";
        string Freezer_SPEED = "";
        string Freezer_SETUP = "";
        string Freezer_UCL = "";
        string Freezer_LCL = "";
        string Freezer_ACTUAL = "";

        public F_EAP_Balance_Kanban( )
        {
            GetDept();
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        public F_EAP_Balance_Kanban(string dept)
        {
            d_dept = dept;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void F_EAP_Balance_Kanban_Load(object sender, EventArgs e)
        {
            GetMachineIdByDept();
            SetQty();
            SetEqpStatus();
            SetEqpParm();
            SetEqpAlarm();
        }

        private void SetEqpParm()
        {
            if ((!ovenStatus.Equals("C")) && !string.IsNullOrEmpty(ovenStatus))
            {
                SetOvenParm();
                label8.ForeColor = Color.Green;
            }
            else
            {
                SetBackColor(panelOvenUAct1, textOvenUAct1, Color.WhiteSmoke);
                SetBackColor(panelOvenUAct2, textOvenUAct2, Color.WhiteSmoke);
                SetBackColor(panelOvenUAct3, textOvenUAct3, Color.WhiteSmoke);
                SetBackColor(panelOvenLAct1, textOvenLAct1, Color.WhiteSmoke);
                SetBackColor(panelOvenLAct2, textOvenLAct2, Color.WhiteSmoke);
                SetBackColor(panelOvenLAct3, textOvenLAct3, Color.WhiteSmoke);
                SetOvenParmEmpty();
                label8.ForeColor = Color.Gray;
            }

            if ((!pressStatus.Equals("C")) && !string.IsNullOrEmpty(pressStatus))
            {
                SetPressParm();
                label10.ForeColor = Color.Green;
            }
            else
            {
                SetPressParmEmpty();
                label10.ForeColor = Color.Gray;
            }

            if ((!freezerStatus.Equals("C")) && !string.IsNullOrEmpty(freezerStatus))
            {
                SetFreezerParm();
                label11.ForeColor = Color.Green;
            }
            else
            {
                SetFreezerParmEmpty();
                label11.ForeColor = Color.Gray;
            }
        }

        //获取部门代码、名称
        private void GetDept()
        {
            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetDept", Program.client.UserToken, string.Empty);
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    d_dept = dtJson.Rows[0]["STAFF_DEPARTMENT"].ToString();
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

        private void GetMachineIdByDept()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vDept", d_dept);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "QueryMachineIdByDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0;i< dt.Rows.Count;i++)
                        {
                            if ("OV".Equals(dt.Rows[i]["MACHINE_TYPE"].ToString()))
                            {
                                ovenId = dt.Rows[i]["MACHINE_NO"].ToString();
                            }
                            else if ("FR".Equals(dt.Rows[i]["MACHINE_TYPE"].ToString()))
                            {
                                freezerId = dt.Rows[i]["MACHINE_NO"].ToString();
                            }
                            else if ("PR".Equals(dt.Rows[i]["MACHINE_TYPE"].ToString()))
                            {
                                pressId = dt.Rows[i]["MACHINE_NO"].ToString();
                            }
                            else { }
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

        private void SetQty()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vDept", d_dept);
                p.Add("vOvenId", ovenId);
                p.Add("vPressId", pressId);
                p.Add("vFreezerId", freezerId);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    //textInQty.Text = dt.Rows[0][0].ToString();
                    string pressQty = dt.Rows[1][2].ToString();
                    string pressQty2 = dt.Rows[0][2].ToString();

                    textOvenQty.Text = dt.Rows[1][1].ToString();
                    if (pressQty.Contains("."))
                    {
                        pressQty = pressQty.Substring(0, pressQty.IndexOf(".") + 2);
                    }
                    textPressQty.Text = pressQty;
                    textFreezerQty.Text = dt.Rows[1][3].ToString();
                    textOutQty.Text = dt.Rows[1][4].ToString();

                    textBox1.Text = dt.Rows[0][1].ToString();
                    if (pressQty2.Contains("."))
                    {
                        pressQty2 = pressQty2.Substring(0, pressQty2.IndexOf(".") + 2);
                    }
                    textBox2.Text = pressQty2;
                    textBox3.Text = dt.Rows[0][3].ToString();
                    textBox4.Text = dt.Rows[0][4].ToString();
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

        private void SetEqpStatus()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vDept", d_dept);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryEqpStatusByDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["MACHINE_TYPE"].ToString().Equals("OV"))
                        {
                            textBox7.Text = dt.Rows[i]["STATUS"].ToString();
                            ovenStatus = dt.Rows[i]["TYPE_CODE"].ToString().Substring(0,1);
                        }
                        else if (dt.Rows[i]["MACHINE_TYPE"].ToString().Equals("PR"))
                        {
                            textBox11.Text = dt.Rows[i]["STATUS"].ToString();
                            pressStatus = dt.Rows[i]["TYPE_CODE"].ToString().Substring(0, 1);
                        }
                        else if (dt.Rows[i]["MACHINE_TYPE"].ToString().Equals("FR"))
                        {
                            textBox12.Text = dt.Rows[i]["STATUS"].ToString();
                            freezerStatus = dt.Rows[i]["TYPE_CODE"].ToString().Substring(0, 1);
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

        private void SetOvenParm()
        {
            QueryOvenParm();

            textOvenUSet1.Text = Oven_UPER_SETUP1;
            textOvenUSet2.Text = Oven_UPER_SETUP2;
            textOvenUSet3.Text = Oven_UPER_SETUP3;
            textOvenLSet1.Text = Oven_LOWER_SETUP1;
            textOvenLSet2.Text = Oven_LOWER_SETUP2;
            textOvenLSet3.Text = Oven_LOWER_SETUP3;

            textOvenUAct1.Text = Oven_UPER_ACTUAL1;
            textOvenUAct2.Text = Oven_UPER_ACTUAL2;
            textOvenUAct3.Text = Oven_UPER_ACTUAL3;
            textOvenLAct1.Text = Oven_LOWER_ACTUAL1;
            textOvenLAct2.Text = Oven_LOWER_ACTUAL2;
            textOvenLAct3.Text = Oven_LOWER_ACTUAL3;

            if (decimal.Parse(Oven_UPER_ACTUAL1) < decimal.Parse(Oven_ULCL1) || decimal.Parse(Oven_UPER_ACTUAL1) > decimal.Parse(Oven_UUCL1))
            {
                SetBackColor(panelOvenUAct1, textOvenUAct1, Color.Red);
            }
            else
            {
                SetBackColor(panelOvenUAct1, textOvenUAct1, Color.WhiteSmoke);
            }
            if (decimal.Parse(Oven_UPER_ACTUAL2) < decimal.Parse(Oven_ULCL2) || decimal.Parse(Oven_UPER_ACTUAL2) > decimal.Parse(Oven_UUCL2))
            {
                SetBackColor(panelOvenUAct2, textOvenUAct2, Color.Red);
            }
            else
            {
                SetBackColor(panelOvenUAct2, textOvenUAct2, Color.WhiteSmoke);
            }
            if (decimal.Parse(Oven_UPER_ACTUAL3) < decimal.Parse(Oven_ULCL3) || decimal.Parse(Oven_UPER_ACTUAL3) > decimal.Parse(Oven_UUCL3))
            {
                SetBackColor(panelOvenUAct3, textOvenUAct3, Color.Red);
            }
            else
            {
                SetBackColor(panelOvenUAct3, textOvenUAct3, Color.WhiteSmoke);
            }
            if (decimal.Parse(Oven_LOWER_ACTUAL1) < decimal.Parse(Oven_LLCL1) || decimal.Parse(Oven_LOWER_ACTUAL1) > decimal.Parse(Oven_LUCL1))
            {
                SetBackColor(panelOvenLAct1, textOvenLAct1, Color.Red);
            }
            else
            {
                SetBackColor(panelOvenLAct1, textOvenLAct1, Color.WhiteSmoke);
            }
            if (decimal.Parse(Oven_LOWER_ACTUAL2) < decimal.Parse(Oven_LLCL2) || decimal.Parse(Oven_LOWER_ACTUAL2) > decimal.Parse(Oven_LUCL2))
            {
                SetBackColor(panelOvenLAct2, textOvenLAct2, Color.Red);
            }
            else
            {
                SetBackColor(panelOvenLAct2, textOvenLAct2, Color.WhiteSmoke);
            }
            if (decimal.Parse(Oven_LOWER_ACTUAL3) < decimal.Parse(Oven_LLCL3) || decimal.Parse(Oven_LOWER_ACTUAL3) > decimal.Parse(Oven_LUCL3))
            {
                SetBackColor(panelOvenLAct3, textOvenLAct3, Color.Red);
            }
            else
            {
                SetBackColor(panelOvenLAct3, textOvenLAct3, Color.WhiteSmoke);
            }
        }

        private void SetPressParm()
        {
            QueryPressParm();

            textPressLSetA.Text = Press_LP_Average;
            textPressRSetA.Text = Press_RP_Average;
        }

        private void SetFreezerParm()
        {
            QueryFreezerParm();

            textFreezerSet.Text = Freezer_SETUP;
            textFreezerAct.Text = Freezer_ACTUAL;
            if (decimal.Parse(Freezer_ACTUAL) < decimal.Parse(Freezer_LCL) || decimal.Parse(Freezer_ACTUAL) > decimal.Parse(Freezer_UCL))
            {
                SetBackColor(panelFreezerAct, textFreezerAct, Color.Red);
            }
            else
            {
                SetBackColor(panelFreezerAct, textFreezerAct, Color.WhiteSmoke);
            }
        }

        private void QueryOvenParm()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", ovenId);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryOvenParmById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    Oven_KWH = dt.Rows[0]["KWH"].ToString();
                    Oven_SPEED = dt.Rows[0]["SPEED"].ToString();

                    Oven_UPER_SETUP1 = dt.Rows[0]["UPER_SETUP1"].ToString();
                    Oven_UUCL1 = dt.Rows[0]["UUCL1"].ToString();
                    Oven_ULCL1 = dt.Rows[0]["ULCL1"].ToString();
                    Oven_UPER_ACTUAL1 = dt.Rows[0]["UPER_ACTUAL1"].ToString();
                    Oven_LOWER_SETUP1 = dt.Rows[0]["LOWER_SETUP1"].ToString();
                    Oven_LUCL1 = dt.Rows[0]["LUCL1"].ToString();
                    Oven_LLCL1 = dt.Rows[0]["LLCL1"].ToString();
                    Oven_LOWER_ACTUAL1 = dt.Rows[0]["LOWER_ACTUAL1"].ToString();

                    Oven_UPER_SETUP2 = dt.Rows[0]["UPER_SETUP2"].ToString();
                    Oven_UUCL2 = dt.Rows[0]["UUCL2"].ToString();
                    Oven_ULCL2 = dt.Rows[0]["ULCL2"].ToString();
                    Oven_UPER_ACTUAL2 = dt.Rows[0]["UPER_ACTUAL2"].ToString();
                    Oven_LOWER_SETUP2 = dt.Rows[0]["LOWER_SETUP2"].ToString();
                    Oven_LUCL2 = dt.Rows[0]["LUCL2"].ToString();
                    Oven_LLCL2 = dt.Rows[0]["LLCL2"].ToString();
                    Oven_LOWER_ACTUAL2 = dt.Rows[0]["LOWER_ACTUAL2"].ToString();

                    Oven_UPER_SETUP3 = dt.Rows[0]["UPER_SETUP3"].ToString();
                    Oven_UUCL3 = dt.Rows[0]["UUCL3"].ToString();
                    Oven_ULCL3 = dt.Rows[0]["ULCL3"].ToString();
                    Oven_UPER_ACTUAL3 = dt.Rows[0]["UPER_ACTUAL3"].ToString();
                    Oven_LOWER_SETUP3 = dt.Rows[0]["LOWER_SETUP3"].ToString();
                    Oven_LUCL3 = dt.Rows[0]["LUCL3"].ToString();
                    Oven_LLCL3 = dt.Rows[0]["LLCL3"].ToString();
                    Oven_LOWER_ACTUAL3 = dt.Rows[0]["LOWER_ACTUAL3"].ToString();
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

        private void QueryPressParm()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", pressId);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryPressParmById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    Press_KWH = dt.Rows[0]["KWH"].ToString();

                    Press_LP_Average = dt.Rows[0]["LP_AVERAGE"].ToString();
                    Press_RP_Average = dt.Rows[0]["RP_AVERAGE"].ToString();
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

        private void QueryFreezerParm()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vMachineID", freezerId);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryFreezerParmById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    Freezer_KWH = dt.Rows[0]["KWH"].ToString();
                    Freezer_SPEED = dt.Rows[0]["SPEED"].ToString();

                    Freezer_SETUP = dt.Rows[0]["SETUP"].ToString();
                    Freezer_UCL = dt.Rows[0]["UCL"].ToString();
                    Freezer_LCL = dt.Rows[0]["LCL"].ToString();
                    Freezer_ACTUAL = dt.Rows[0]["ACTUAL"].ToString();
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

        private void SetEqpAlarm()
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vDept", d_dept);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkCollectServer", "QueryEqpAlarmByDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    richTextOvenAlarm.Text = "";
                    richTextPressAlarm.Text = "";
                    richTextFreezerAlarm.Text = "";

                    for (int i = 0;i<dt.Rows.Count;i++)
                    {
                        string time = dt.Rows[i]["TRANSFER_TIME"].ToString();
                        string text = time.Substring(0, 4) + "/" + time.Substring(4, 2) + "/" + time.Substring(6, 2) + " " + time.Substring(8, 2) + ":" + time.Substring(10, 2) + ":" + time.Substring(12, 2) + "---->" + dt.Rows[i]["DESCRIPTION"].ToString();
                        if (dt.Rows[i]["MACHINE_NO"].ToString().Equals(ovenId))
                        {
                            richTextOvenAlarm.AppendText(text + "\n");
                        }
                        else if (dt.Rows[i]["MACHINE_NO"].ToString().Equals(pressId))
                        {
                            richTextPressAlarm.AppendText(text + "\n");
                        }
                        else if (dt.Rows[i]["MACHINE_NO"].ToString().Equals(freezerId))
                        {
                            richTextFreezerAlarm.AppendText(text + "\n");
                        }
                        else { }
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetQty();
            SetEqpStatus();
            SetEqpParm();
        }

        private void SetBackColor(Control c2, Control c3,Color color)
        {
            c2.BackColor = color;
            c3.BackColor = color;
        }

        private void SetOvenParmEmpty()
        {
            textOvenUAct1.Text = "";
            textOvenUAct2.Text = "";
            textOvenUAct3.Text = "";
            textOvenLAct1.Text = "";
            textOvenLAct2.Text = "";
            textOvenLAct3.Text = "";
        }

        private void SetPressParmEmpty()
        {
            textPressLSetA.Text = "";
            textPressRSetA.Text = "";
        }

        private void SetFreezerParmEmpty()
        {
            textFreezerSet.Text = "";
            textFreezerAct.Text = "";
        }

        private void butOvenDetail_Click(object sender, EventArgs e)
        {
            F_EAP_Oven_RealParam frm = new F_EAP_Oven_RealParam(ovenId);
            frm.ShowDialog();
        }

        private void butFreezerDetail_Click(object sender, EventArgs e)
        {
            F_EAP_Freezer_RealParam frm = new F_EAP_Freezer_RealParam(freezerId);
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            F_EAP_Day_HourlyPut frm = new F_EAP_Day_HourlyPut(d_dept);
            frm.ShowDialog();
        }

        private void butPressDetail_Click(object sender, EventArgs e)
        {
            F_EAP_Press_RealParam frm = new F_EAP_Press_RealParam(pressId);
            frm.ShowDialog();
        }

        private void F_EAP_Balance_Kanban_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.Dispose();
        }



        //private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    int index = dataGridView1.CurrentRow.Index;
        //    if (index > -1 && dataGridView1.Rows[index].Cells[0].Value != null)
        //    {
        //        Assembly assembly = null;
        //        string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
        //        assembly = Assembly.LoadFrom(path + @"\" + "Production_Kanban" + ".dll");
        //        Type type = assembly.GetType("Production_Kanban.Interface");
        //        object instance = Activator.CreateInstance(type);
        //        MethodInfo mi = type.GetMethod("RunCustomize");
        //        object[] args = new object[3];
        //        args[0] = Program.Client;
        //        args[1] = this.dataGridView1.Rows[index].Cells["Area"].Value;
        //        args[2] = dateTimePicker1.Text;
        //        object obj = mi.Invoke(instance, args);
        //    }
        //}

    }
}
