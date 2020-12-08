using AutocompleteMenuNS;
using MaterialSkin.Controls;
using Mes.Util;
using ProductionInput.Bean;
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

namespace ProductionInput
{
    public partial class ProductionInputForm : MaterialForm
    {
        Bitmap smile = null;
        Bitmap cry = null;
        int listSizeSelectIndex = -1;
        private string vPO;

        int dayFinishQty = 0;
        int daySizeFinishQty = 0;
        int unFinishQty = 0;
        int daySizeWorkQty = 0;
        string d_dept = "";

        IList<VwSjqdmsWorkDaySize> sjqdmsWorkDaySize = null;
        DataTable workDayDt = null;
        Button button_qty;
        bool ByClick = true;

        public ProductionInputForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            btnImage.Visible = false;
            smile = new Bitmap(Properties.Resources.smile);
            cry = new Bitmap(Properties.Resources.cry);
        }

        private void ProductionInputForm_Load(object sender, EventArgs e)
        {
            try
            {
                GetDept();
                LoadSeId();
            }
            catch(Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            LoadDayFinishQty();
            SetDayFinishQty();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
        }

        private void LoadSeId()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(300, 300);
            var columnWidth = new int[] { 50, 300 };
            workDayDt = GetSeId();
            int n = 1;
            for (int i = 0; i < workDayDt.Rows.Count;i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(
                    new[] { n + "", workDayDt.Rows[i]["SE_ID"].ToString() + " " + workDayDt.Rows[i]["PO"].ToString() +" "+ workDayDt.Rows[i]["ART_NO"].ToString() }, workDayDt.Rows[i]["SE_ID"].ToString() + "|" + workDayDt.Rows[i]["PO"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
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
            textDayFinishQty.Text = dayFinishQty.ToString();
        }

        private void GetDayFinishQty()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "IN");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetDayFinishQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                //DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dayFinishQty = int.Parse(json.ToString());
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
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

        //获取组别当天的制令
        private DataTable GetSeId()
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "IN");
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

        private IList<VwSjqdmsWorkDaySize> GetSeSize(string seId)
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vDDept", d_dept);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionInputServer", "GetSeSize", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private int GetDaySizeWorkQty(string seId, int seSeq, string SizeNo)
        {
            int qty = 0;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "IN");
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

        private int GetDaySizeFinishQty(string seId, int seSeq, string SizeNo)
        {
            int qty = 0;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vInOut", "IN");
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

        private bool updateFinshQty(string orgId, string seId, string seSeq, string SizeNo, int qty, string scan_ip)
        {
            bool isOK = false;
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vOrgId", orgId);
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vQty", qty);
            p.Add("vIP", scan_ip);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionInputServer", "updateFinshQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            isOK = Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]);
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
                    daySizeWorkQty = GetDaySizeWorkQty(se_id, se_seq, size_no);
                    //工单size的完工数量
                    daySizeFinishQty = GetDaySizeFinishQty(se_id, se_seq, size_no);
                    unFinishQty = daySizeWorkQty - daySizeFinishQty;
                    displayQtyButton(unFinishQty);
                    setProductInfoToDefault(daySizeWorkQty, daySizeFinishQty);
                    //setSizeButtonBackColorToDefault();
                    TextQuerySeID.Text = "";
                }
                catch (Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {
                setProductInfoToDefault(0, 0);
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
            string se_seq = obj.SE_SEQ.ToString();
            string size_no = obj.SIZE_NO;
            int qty = int.Parse(strqty);
            int oldunFinishQty = unFinishQty;
            string scan_ip = IPUtil.GetIpAddress();
            try
            {
                daySizeWorkQty = GetDaySizeWorkQty(se_id, int.Parse(se_seq), size_no);
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
                if (updateFinshQty(org_id, se_id, se_seq, size_no, qty, scan_ip))
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
                    //unFinishQty = oldunFinishQty;
                    btnImage.Visible = true;
                    btnImage.BackgroundImage = cry;
                    this.btnImage.BackColor = System.Drawing.Color.Transparent;
                    this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                }
            }
            catch (Exception ex)
            {
                //unFinishQty = oldunFinishQty;
                btnImage.Visible = true;
                btnImage.BackgroundImage = cry;
                this.btnImage.BackColor = System.Drawing.Color.Transparent;
                this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            textSizeQty.Text = daySizeWorkQty.ToString();
            textSizeFinishQty.Text = daySizeFinishQty.ToString();
            displayQtyButton(unFinishQty);
            //setSizeButtonBackColorToDefault();
            //clickButton.BackColor = Color.Blue;
            //textPo.Text = vPO;
            textSize.Text = obj.SIZE_NO;
            textQty.Text = strqty;
            TextQuerySeID.Text = "";
        }
        
        /// <summary>
        /// 设置size的按钮为蓝色
        /// </summary>
        private void setSizeButtonBackColorToDefault()
        {
            for (int i = 1; i < 10; i++)
            {
                Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                button.BackColor = System.Drawing.Color.CornflowerBlue;
            }
        }

        /// <summary>
        /// 清空根据选中size对应的button
        /// </summary>
        /// <param name="uFinishQty">未完工的数量</param>
        private void displayQtyButton(int uFinishQty)
        {
            for (int i = 1; i < 10; i++)
            {
                Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                if (uFinishQty >= int.Parse(button.Text))
                {
                    button.Visible = true;
                }
                else
                {
                    button.Visible = false;
                }
            }
        }

        private void reflashListSize()
        {
            listSize.Items.Remove(listSize.SelectedItem);
            sjqdmsWorkDaySize.RemoveAt(listSizeSelectIndex);
            listSizeSelectIndex = -1;
            listSize.SelectedIndex = listSizeSelectIndex;
        }

        private void btnReflash_Click(object sender, EventArgs e)
        {
            LoadSeId();
            listSize.Items.Clear();
            setSizeButtonToDefault();
        }

        private void setSizeButtonToDefault()
        {
            for (int i = 1; i < 10; i++)
            {
                Button button = (Button)Controls.Find("btn_c" + i, true)[0];
                //button.BackColor = Color.Green;
                button.Visible = false;
            }
        }

        private void TextQuerySeID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listSizeSelectIndex = -1;
                string seId = TextQuerySeID.Text.Trim().ToString().ToUpper().Split(new Char[] { '|' })[0];
                if (!string.IsNullOrEmpty(seId))
                {
                    listSize.Items.Clear();
                    vPO = "";
                    sjqdmsWorkDaySize = GetSeSize(seId);
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

        private void setProductInfoToDefault(int qty, int daySizeFinishQty)
        {
            if (listSizeSelectIndex == -1)
            {
                textSizeQty.Text = 0.ToString();
                textSizeFinishQty.Text = 0.ToString();
            }
            else
            {
                textSizeQty.Text = qty.ToString();
                textSizeFinishQty.Text = daySizeFinishQty.ToString();
            }
            textSize.Text = "";
            textQty.Text = "";
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

        private void butQuery_Click(object sender, EventArgs e)
        {
            Hide();
            InOutDetailForm frm = new InOutDetailForm(d_dept);
            frm.ShowDialog();
            System.Threading.Thread.Sleep(200);
            Show();
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
                MessageBox.Show("请选择制令/PO和SIZE", "错误！");
                return;
            }

            if ("".Equals(textEnterQty.Text.ToString()) || decimal.Parse(textEnterQty.Text.ToString()) <= 0)
            {
                MessageBox.Show("请输入大于0的转移数量", "错误！");
                return;
            }

            int enterQty = int.Parse(textEnterQty.Text.ToString());

            if (enterQty > unFinishQty)
            {
                MessageBox.Show("输入的数量大于可转移的数量", "错误！");
                return;
            }

            VwSjqdmsWorkDaySize obj = sjqdmsWorkDaySize[listSizeSelectIndex];
            string org_id = obj.ORG_ID.ToString();
            string se_id = obj.SE_ID;
            string se_seq = obj.SE_SEQ.ToString();
            string size_no = obj.SIZE_NO;
            int oldunFinishQty = unFinishQty;
            string scan_ip = IPUtil.GetIpAddress();

            DialogResult dr = MessageBox.Show("确认提交数据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                ByClick = false;
                SetButtonEnable((Button)sender);
                try
                {
                    daySizeWorkQty = GetDaySizeWorkQty(se_id, int.Parse(se_seq), size_no);
                    unFinishQty = daySizeWorkQty - daySizeFinishQty;
                    if (unFinishQty < enterQty)
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
                    if (updateFinshQty(org_id, se_id, se_seq, size_no, enterQty, scan_ip))
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
                        SetDayFinishQty();
                    }
                    else
                    {
                        //unFinishQty = oldunFinishQty;
                        btnImage.Visible = true;
                        btnImage.BackgroundImage = cry;
                        this.btnImage.BackColor = System.Drawing.Color.Transparent;
                        this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    }
                }
                catch (Exception ex)
                {
                    //unFinishQty = oldunFinishQty;
                    btnImage.Visible = true;
                    btnImage.BackgroundImage = cry;
                    this.btnImage.BackColor = System.Drawing.Color.Transparent;
                    this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
                textSizeQty.Text = daySizeWorkQty.ToString();
                textSizeFinishQty.Text = daySizeFinishQty.ToString();
                displayQtyButton(unFinishQty);
                //setSizeButtonBackColorToDefault();
                //clickButton.BackColor = Color.Blue;
                //textPo.Text = vPO;
                textSize.Text = obj.SIZE_NO;
                textQty.Text = enterQty.ToString();
                TextQuerySeID.Text = "";
                textEnterQty.Text = "";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            textTime.Text = DateTime.Now.ToString();
        }
    }
}
