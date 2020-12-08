using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using AutocompleteMenuNS;
using StitchingOutput.Bean;
using Mes.Util;
using MaterialSkin.Controls;

namespace StitchingOutput
{
    public partial class StitchingOutputForm : MaterialForm
    {
        int listSizeSelectIndex = -1;

        Bitmap smile = null;
        Bitmap cry = null;
        Button button_qty;
        IList<VwSjqdmsWorkDaySize> sjqdmsWorkDaySize = null;
        DataTable workDayDt = null;
        bool ByClick = false;

        decimal dayFinishQty = 0;
        int daySizeFinishQty = 0;
        int daySizeWorkQty = 0;
        int unFinishQty = 0;
        private string vPO;
        string d_dept = "";
        string msg01;

        public StitchingOutputForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            btnImage.Visible = false;
            smile = new Bitmap(Properties.Resources.smile);
            cry = new Bitmap(Properties.Resources.cry);
            msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
        }

        private void StitchingOutputForm_Load(object sender, EventArgs e)
        {
            GetDept();
            LoadSeId();
            LoadDayFinishQty();
            SetDayFinishQty();
            TextQuerySeID.Focus();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
        }

        private void LoadSeId()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(300, 300);
            var columnWidth = new int[] { 50, 300 };
            workDayDt = GetSeId();
            int n = 1;
            for (int i = 0; i < workDayDt.Rows.Count; i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(
                    new[] { n + "", workDayDt.Rows[i]["SE_ID"].ToString() + " " + workDayDt.Rows[i]["PO"].ToString() + " " + workDayDt.Rows[i]["ART_NO"].ToString() }, workDayDt.Rows[i]["SE_ID"].ToString() + "|" + workDayDt.Rows[i]["PO"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }

        /// <summary>
        /// 加载当天完成派工的数量
        /// </summary>
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

        private void SetDayFinishQty()
        {
            textFinishQty.Text = dayFinishQty.ToString();
        }

        //获取用户组别
        private void GetDept()
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

        //获取组别当天任务的制令
        private DataTable GetSeId()
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

        //获取组别当天分SIZE的任务列表
        private IList<VwSjqdmsWorkDaySize> GetWorkDaySize(string seId)
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
            return DataConvertUtil<VwSjqdmsWorkDaySize>.ConvertDataTableToList(dt);
        }

        //获取当天完工总数
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

        //获取当天SIZE的排产数量
        private int GetDaySizeWorkQty(string seId, int seSeq, string SizeNo)
        {
            int qty = 0;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetDaySizeWorkQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        //获取当天SIZE的完工数量
        private int GetDaySizeFinishQty(string seId, int seSeq, string SizeNo)
        {
            int qty = 0;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "OUT");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetDaySizeFinishQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        //根据制令号获取po
        private string GetPoBySeid(string seId)
        {
            string po = "";
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetPoBySeid", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                po = json;
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return po;
        }

        private bool updateOutFinshQty(string orgId, string seId, string seSeq, string SizeNo, int qty, string scan_ip)
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vOrgId", orgId);
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vQty", qty);
            p.Add("vIP", scan_ip);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "updateOutFinshQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            bool isOK = Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]);
            if (!isOK)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return isOK;
        }

        private void listSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listSize.SelectedIndex > -1)
            {
                try
                {
                    listSizeSelectIndex = listSize.SelectedIndex;
                    VwSjqdmsWorkDaySize vswdz = sjqdmsWorkDaySize[listSizeSelectIndex];
                    string se_id = vswdz.SE_ID;
                    int se_seq = vswdz.SE_SEQ;
                    string size_no = vswdz.SIZE_NO;
                    //工单size的排产数量
                    int daySizeWorkQty = GetDaySizeWorkQty(se_id, se_seq, size_no);
                    //工单size的完工数量
                    daySizeFinishQty = GetDaySizeFinishQty(se_id, se_seq, size_no);
                    unFinishQty = daySizeWorkQty - daySizeFinishQty;
                    displayQtyButton(unFinishQty);
                    setProductInfoToDefault(daySizeWorkQty, daySizeFinishQty);
                    TextQuerySeID.Text = "";
                }
                catch(Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {
                setProductInfoToDefault(0,0);
            }
        }

        /// <summary>
        /// 通过点击进行报工，相应的需要刷新size列表和button的数量和背景颜色
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="clickButton"></param>
        private void finishQty(string strqty, Button clickButton)
        {
            ByClick = true;
            button_qty = clickButton;
            SetButtonEnable(button_qty);
            VwSjqdmsWorkDaySize obj = sjqdmsWorkDaySize[listSizeSelectIndex];
            string org_id = obj.ORG_ID.ToString();
            string se_id = obj.SE_ID;
            int se_seq = obj.SE_SEQ;
            string size_no = obj.SIZE_NO;
            int qty = int.Parse(strqty);
            string scan_ip = IPUtil.GetIpAddress();
            try
            {
                daySizeWorkQty = GetDaySizeWorkQty(se_id, se_seq, size_no);
                unFinishQty = daySizeWorkQty - daySizeFinishQty;
                if (unFinishQty < qty)
                {
                    btnImage.Visible = true;
                    btnImage.BackgroundImage = cry;
                    this.btnImage.BackColor = System.Drawing.Color.Transparent;
                    this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    textSizeQty.Text = daySizeWorkQty.ToString();
                    displayQtyButton(unFinishQty);
                    if (unFinishQty <= 0)
                    {
                        reflashListSize();
                    }
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("排程变更，请刷新界面后再进行操作!", Program.client, "", Program.client.Language);

                    SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                    return;
                }

                if (updateOutFinshQty(org_id, se_id, se_seq.ToString(), size_no, qty, scan_ip))
                {
                    if (qty == unFinishQty)
                    {
                        reflashListSize();
                    }
                    btnImage.Visible = true;
                    btnImage.BackgroundImage = smile;
                    this.btnImage.BackColor = System.Drawing.Color.Transparent;
                    this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

                    unFinishQty = unFinishQty - qty;
                    daySizeFinishQty += qty;
                    dayFinishQty += qty;
                    SetDayFinishQty();
                }
                else
                {
                    btnImage.Visible = true;
                    btnImage.BackgroundImage = cry;
                    this.btnImage.BackColor = System.Drawing.Color.Transparent;
                    this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                }
            }
            catch (Exception ex)
            {
                ScanFailed();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            textSizeQty.Text = daySizeWorkQty.ToString();
            textSizeFinishQty.Text = daySizeFinishQty.ToString();
            displayQtyButton(unFinishQty);
            textSize.Text = obj.SIZE_NO;
            textQty.Text = strqty;

            TextQuerySeID.Text = "";
            textEnterQty.Text = "";
        }

        /// <summary>
        ///输入制令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextQuerySeID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listSizeSelectIndex = -1;
                string txtQrCode = TextQuerySeID.Text.ToString().ToUpper();
                //B,200,A0A19040071,SM0A190415000049,485,1,9,10,48,EF9370,03996
                //类别,组织,制令,工票唯一码，工票号,订单序,size,数量,size序号,art,模号
                if (!string.IsNullOrWhiteSpace(txtQrCode) && !string.IsNullOrEmpty(txtQrCode) && txtQrCode.Contains(","))
                {
                    string[] str = txtQrCode.Split(',');
                    int length = str.Length;
                    if (ParseQrCode(str, length))
                    {
                        listSize.Items.Clear();
                        setSizeButtonToDefault();
                        string scan_ip = IPUtil.GetIpAddress();
                        string org_id = str[1];
                        string se_id = str[2];
                        int se_seq = 1;
                        string size_no = str[6];
                        int qty = int.Parse(str[7]);
                        string size_seq = str[8];
                        string art_no = str[9];
                        try
                        {
                            daySizeWorkQty = GetDaySizeWorkQty(se_id, se_seq, size_no);
                            if (daySizeWorkQty <= 0)
                            {
                                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("该制令:{0}的{1}码没有投入或已全部扫描", Program.client, "", Program.client.Language);

                                MessageBox.Show(string.Format(msg02, se_id, size_no), msg01);
                                TextQuerySeID.Text = "";
                                ScanFailed();
                                return;
                            }
                            daySizeFinishQty = GetDaySizeFinishQty(se_id, se_seq, size_no);
                            unFinishQty = daySizeWorkQty - daySizeFinishQty;
                            if (unFinishQty < qty)
                            {
                                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("扫描数量大于剩余未扫描数量！", Program.client, "", Program.client.Language);
                                MessageBox.Show(msg02, msg01);
                                TextQuerySeID.Text = "";
                                ScanFailed();
                                return;
                            }

                            vPO = GetPoBySeid(se_id);
                            textPo.Text = vPO;

                            if (updateOutFinshQty(org_id, se_id, se_seq.ToString(), size_no, qty, scan_ip))
                            {
                                textSizeFinishQty.Text = (daySizeFinishQty + qty).ToString();
                                dayFinishQty += qty;
                                SetDayFinishQty();
                                textSizeQty.Text = daySizeWorkQty.ToString();
                                textSize.Text = size_no;
                                textQty.Text = qty.ToString();

                                btnImage.Visible = true;
                                btnImage.BackgroundImage = smile;
                                this.btnImage.BackColor = System.Drawing.Color.Transparent;
                                this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

                                daySizeFinishQty += qty;
                            }
                            else
                            {
                                ScanFailed();
                            }
                        }
                        catch(Exception ex)
                        {
                            ScanFailed();
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                        }
                        TextQuerySeID.Text = "";
                    }
                }
                else
                {
                    string seId = TextQuerySeID.Text.Trim().ToString().ToUpper().Split(new Char[] { '|' })[0];
                    if (!string.IsNullOrEmpty(seId))
                    {
                        listSize.Items.Clear();
                        vPO = "";
                        sjqdmsWorkDaySize = GetWorkDaySize(seId);
                        if (sjqdmsWorkDaySize.Count > 0)
                        {
                            foreach (var o in sjqdmsWorkDaySize)
                            {
                                this.listSize.Items.Add(o.SIZE_NO);
                            }

                            for (int i = 0; i < workDayDt.Rows.Count; i++)
                            {
                                if (seId == workDayDt.Rows[i]["SE_ID"].ToString())
                                {
                                    vPO = workDayDt.Rows[i]["PO"].ToString();
                                    break;
                                }
                            }
                            //vPO = TextQuerySeID.Text.Trim().ToString().ToUpper().Split(new Char[] { '|' })[1];
                            TextQuerySeID.Text = "";
                        }
                        setSizeButtonToDefault();
                        textPo.Text = vPO;
                    }
                    setProductInfoToDefault(0, 0);
                }
            }
        }

        /// <summary>
        /// 清空根据选中size对应的button
        /// </summary>
        /// <param name="uFinishQty">未完工的数量</param>
        private void displayQtyButton(decimal uFinishQty)
        {
            for (int i = 1; i < 10; i++)
            {
                Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                if (uFinishQty >= decimal.Parse(button.Text))
                {
                    button.Visible = true;
                }
                else
                {
                    button.Visible = false;
                }
            }
        }

        /// <summary>
        /// 设置右边栏位信息，size投入数量、工单报工数量
        /// </summary>
        private void setProductInfoToDefault(int daySizeWorkQty, int daySizeFinishQty)
        {
            if (listSizeSelectIndex == -1)
            {
                textSizeQty.Text = 0.ToString();
                textSizeFinishQty.Text = 0.ToString();
            }
            else
            {
                textSizeQty.Text = daySizeWorkQty.ToString();
                textSizeFinishQty.Text = daySizeFinishQty.ToString();
            }
            textSize.Text = "";
            textQty.Text = "";
        }

        /// <summary>
        /// 刷新size列表，移除报工完成的size
        /// </summary>
        /// <param name="listSizeIndex"></param>
        private void reflashListSize()
        {
            listSize.Items.Remove(listSize.SelectedItem);
            sjqdmsWorkDaySize.RemoveAt(listSizeSelectIndex);
            listSizeSelectIndex = -1;
            listSize.SelectedIndex = listSizeSelectIndex;
        }

        //设置按钮不可见
        private void setSizeButtonToDefault()
        {
            for (int i = 1; i < 10; i++)
            {
                Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                button.Visible = false;
            }
        }

        private void btnReflashMesWorkDay_Click(object sender, EventArgs e)
        {
            LoadSeId();
            listSize.Items.Clear();
            setSizeButtonToDefault();
        }

        private Boolean ParseQrCode(string[] str, int length)
        {
            if (length == 11)
            {
                return true;
            }
            else
            {
                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("二维码长度有误，请联系系统管理员！", Program.client, "", Program.client.Language);
                MessageBox.Show(msg02, msg01);
                return false;
            }
        }

        private void ScanFailed()
        {
            btnImage.Visible = true;
            btnImage.BackgroundImage = cry;
            this.btnImage.BackColor = System.Drawing.Color.Transparent;
            this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void SetButtonEnable(Button clickButton)
        {
            if (!ByClick)
            {
                for (int i = 1; i < 10; i++)
                {
                    Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                    button.Enabled = false;
                    button.BackColor = Color.Gray;
                }
            }
            else
            {
                clickButton.BackColor = Color.Gray;
                for (int i = 1; i < 10; i++)
                {
                    Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                    button.Enabled = false;
                }
            }
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            if (!ByClick)
            {
                for (int i = 1; i < 10; i++)
                {
                    Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                    button.Enabled = true;
                    button.BackColor = System.Drawing.Color.CornflowerBlue;
                }
            }
            else
            {
                button_qty.BackColor = System.Drawing.Color.CornflowerBlue;
                for (int i = 1; i < 10; i++)
                {
                    Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                    button.Enabled = true;
                }
            }
            timer1.Stop();
        }

        /// <summary>
        /// 完工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_c1_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c2_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c3_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c4_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c5_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c6_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c7_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c8_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void btn_c9_Click(object sender, EventArgs e)
        {
            finishQty(((Button)sender).Text, (Button)sender);
        }

        private void textEnterQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
        }

        private void butConfirm_Click(object sender, EventArgs e)
        {
            if (listSizeSelectIndex < 0)
            {
                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("请选择制令/PO和SIZE", Program.client, "", Program.client.Language);
                MessageBox.Show(msg02, msg01);
                return;
            }

            if ("".Equals(textEnterQty.Text.ToString()) || decimal.Parse(textEnterQty.Text.ToString()) <= 0)
            {
                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("请输入大于0的转移数量", Program.client, "", Program.client.Language);
                MessageBox.Show(msg02, msg01);
                return;
            }

            int enterQty = int.Parse(textEnterQty.Text.ToString());

            if (enterQty > unFinishQty)
            {
                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("输入的数量大于可转移的数量", Program.client, "", Program.client.Language);
                MessageBox.Show(msg02, msg01);
                return;
            }

            VwSjqdmsWorkDaySize obj = sjqdmsWorkDaySize[listSizeSelectIndex];
            string org_id = obj.ORG_ID.ToString();
            string se_id = obj.SE_ID;
            string se_seq = obj.SE_SEQ.ToString();
            string size_no = obj.SIZE_NO;
            string size_seq = obj.SIZE_SEQ.ToString();
            int oldunFinishQty = unFinishQty;
            string scan_ip = IPUtil.GetIpAddress();

            //string msg11 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
            string msg12 = SJeMES_Framework.Common.UIHelper.UImsg("确认提交数据？", Program.client, "", Program.client.Language);
            DialogResult dr = MessageBox.Show(msg12, msg01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                ByClick = false;
                SetButtonEnable((Button)sender);
                try
                {
                    if (updateOutFinshQty(org_id, se_id, se_seq.ToString(), size_no, enterQty, scan_ip))
                    {
                        if (enterQty == unFinishQty)
                        {
                            reflashListSize();
                        }
                        btnImage.Visible = true;
                        btnImage.BackgroundImage = smile;
                        this.btnImage.BackColor = System.Drawing.Color.Transparent;
                        this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

                        unFinishQty = unFinishQty - enterQty;
                        daySizeFinishQty += enterQty;
                        dayFinishQty += enterQty;
                    }
                    else
                    {
                        btnImage.Visible = true;
                        btnImage.BackgroundImage = cry;
                        this.btnImage.BackColor = System.Drawing.Color.Transparent;
                        this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    }
                }
                catch (Exception ex)
                {
                    //UnFinishQty = oldunFinishQty;
                    btnImage.Visible = true;
                    btnImage.BackgroundImage = cry;
                    this.btnImage.BackColor = System.Drawing.Color.Transparent;
                    this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
                textSizeFinishQty.Text = daySizeFinishQty.ToString();
                displayQtyButton(unFinishQty);
                //textPo.Text = vPO;
                SetDayFinishQty();
                textSize.Text = obj.SIZE_NO;
                textQty.Text = enterQty.ToString();

                TextQuerySeID.Text = "";
                textEnterQty.Text = "";
            }
        }
    }
}
