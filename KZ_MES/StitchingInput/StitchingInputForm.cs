using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutocompleteMenuNS;
using System.Configuration;
using System.Threading;
using MaterialSkin.Controls;
using StitchingInput.Bean;
using Mes.Util;

namespace StitchingInput
{
    public partial class StitchingInputForm : MaterialForm
    {
        int ListSizeSelectIndex = -1;
        int UnFinishQty = 0;
        int DayFinishQty = 0;
        //int sizeFinishQty = 0;
        string d_dept = "";

        Bitmap Smile = null;
        Bitmap Cry = null;
        Button button_qty;
        bool ByClick = false;
        string msg01;

        IList<VW_STITCHINPUT_ORD_SIZE>  SeOrdSize= null;
        private string vPO;

        public StitchingInputForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            btnImage.Visible = false;
            Smile = new Bitmap(Properties.Resources.smile);
            Cry = new Bitmap(Properties.Resources.cry);
            msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
        }

        private void StitchingInputForm_Load(object sender, EventArgs e)
        {
            LoadSeId();
            LoadSeDept();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
        }

        //加载订单列表
        private void LoadSeId()
        {
            autocompleteMenu1.Items=null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(350, 350);
            var columnWidth = new int[] { 50, 250 };
            DataTable dt = GetOrdM();
            int n = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(
                    new[] { n+"", dt.Rows[i]["SE_ID"].ToString() + " "+ dt.Rows[i]["MER_PO"].ToString() + " "+ dt.Rows[i]["PROD_NO"].ToString() }, dt.Rows[i]["SE_ID"].ToString() + "|"+ dt.Rows[i]["MER_PO"].ToString() + "|"+ dt.Rows[i]["PROD_NO"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }

        //加载部门列表
        private void LoadSeDept()
        {
            var columnWidth = new int[] { 50, 250 };
            DataTable dt = GetAllDepts();
            int n = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt.Rows[i]["DEPARTMENT_CODE"].ToString() + " " + dt.Rows[i]["DEPARTMENT_NAME"].ToString() }, dt.Rows[i]["DEPARTMENT_CODE"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }

        // 加载当天完成的数量
        private void LoadDayFinishQty()
        {
            DayFinishQty= GetDayFinishQty(textQueryDept.Text.ToString());
        }

        private void SetDayFinishQty()
        {
            textFinishQty.Text = DayFinishQty.ToString();
        }

        //获取订单列表
        private DataTable GetOrdM()
        {
            DataTable dt = new DataTable();
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "GetOrdM", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(string.Empty));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                int count = dt.Rows.Count;
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return dt;
        }

        //获取投入组别报工 的总数量
        private int GetDayFinishQty(string d_dept)
        {
            int qty = 0;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "GetDayFinishQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                qty = int.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return qty;
        }

        //获取部门列表
        private DataTable GetAllDepts()
        {
            DataTable dt = new DataTable();
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.GeneralServer", "GetAllDepts", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(string.Empty));
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

        private IList<VW_STITCHINPUT_ORD_SIZE> GetSeSize(string seId)
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "GetSeSize", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return DataConvertUtil<VW_STITCHINPUT_ORD_SIZE>.ConvertDataTableToList(dt);
        }

        private DataTable GetSeSizeDetail(string seId, string se_seq, string size_no )
        {
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vSeSeq", se_seq);
            p.Add("vSizeNo", size_no);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "GetSeSizeDetail", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        //验证组别的制程是否为 针车（B）
        private bool ValisStitchingDept(string dept, string routNo)
        {
            bool isStitchingDept = false;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", dept);
            p.Add("vRoutNo", routNo);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "ValisStitchingDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                isStitchingDept = Convert.ToBoolean(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return isStitchingDept;
        }

        private bool updateInFinshQty(string orgId, string seId, string seSeq, string SizeNo, string size_seq, int qty, string scan_ip, string po, string artNo, string seDay)
        {
            bool isOK = false;
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vOrgId", orgId);
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vSizeSeq", size_seq);
            p.Add("vQty", qty);
            p.Add("vIP", scan_ip);
            p.Add("vPO", po);
            p.Add("vArtNo", artNo);
            p.Add("vSeDay", seDay);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "updateInFinshQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            isOK = Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]);
            if (!isOK)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return isOK;
        }

        private bool updateInFinshQtyByScan(string orgId, string seId, string seSeq, string SizeNo, string size_seq, int qty, string scan_ip, string artNo)
        {
            bool isOK = false;
            DataTable dt = new DataTable();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vOrgId", orgId);
            p.Add("vSeId", seId);
            p.Add("vSeSeq", seSeq);
            p.Add("vSizeNo", SizeNo);
            p.Add("vDDept", d_dept);
            p.Add("vSizeSeq", size_seq);
            p.Add("vQty", qty);
            p.Add("vIP", scan_ip);
            p.Add("vArtNo", artNo);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "updateInFinshQtyByScan", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            isOK = Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]);
            if (!isOK)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return isOK;
        }

        //获取指定订单 size的完工总数
        private int GeSizeFinishQty(string org_id, string se_id, string se_seq, string size_no)
        {
            int qty = 0;
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vOrgId", org_id);
            p.Add("vSeId", se_id);
            p.Add("vSeSeq", se_seq);
            p.Add("vSizeNo", size_no);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.StitchingInOutServer", "GeSizeFinishQty", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                qty = int.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return qty;
        }

        /// <summary>
        /// 选择制令对应的size，然后更新报工button的数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSize_SelectedIndexChanged(object sender, EventArgs e)
        {   
            if (listSize.SelectedIndex>-1)
            {
                ListSizeSelectIndex = listSize.SelectedIndex;
                //工单的size生产数量
                int qty = Convert.ToInt32(SeOrdSize[ListSizeSelectIndex].SE_QTY);
                //工单的size完工数量
                string dept = textQueryDept.Text.Trim().ToString().ToUpper();
                VW_STITCHINPUT_ORD_SIZE vsos = SeOrdSize[ListSizeSelectIndex];
                int sizeFinishQty = 0;
                string se_id = vsos.SE_ID;
                string org_id = vsos.ORG_ID.ToString();
                string se_seq = vsos.SE_SEQ.ToString();
                string size_no = vsos.SIZE_NO;

                sizeFinishQty = GeSizeFinishQty(org_id, se_id, se_seq, size_no);

                UnFinishQty = qty - sizeFinishQty;
                displayQtyButton(UnFinishQty);
                setProductInfoToDefault(qty, sizeFinishQty);
                //if (!string.IsNullOrEmpty(TextQuerySeID.Text))
                //{
                //    vPO = TextQuerySeID.Text.Trim().ToString().ToUpper().Split(new Char[] { '|' })[1];
                //    textPo.Text = vPO;
                //}
                TextQuerySeID.Text = "";
                textQueryDept.Text = "";
            }
            else
            {
                setProductInfoToDefault(0,0);
            }
        }

        /// <summary>
        /// 清空根据选中size对应的button
        /// </summary>
        /// <param name="uFinishQty">未完工的数量</param>
        private void displayQtyButton(int uFinishQty)
        {
            for (int i=1;i<10;i++)
            {
                Button button = (Button)Controls.Find("btn_c"+i, true)[0];
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

        /// <summary>
        /// 通过点击进行报工，相应的需要刷新size列表和button的数量和背景颜色
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="clickButton"></param>
        private void finishQty(string strqty,Button clickButton)
        {
            ByClick = true;
            if (!string.IsNullOrWhiteSpace(d_dept))
            {
                button_qty = clickButton;
                SetButtonEnable(button_qty);
                VW_STITCHINPUT_ORD_SIZE obj = SeOrdSize[ListSizeSelectIndex];
                int qty = int.Parse(strqty);
                int oldunFinishQty = UnFinishQty;
                string org_id = obj.ORG_ID.ToString();
                string se_id = obj.SE_ID;
                string se_seq = obj.SE_SEQ.ToString();
                string size_no = obj.SIZE_NO;
                string size_seq = obj.SIZE_SEQ.ToString();
                string scan_ip = IPUtil.GetIpAddress();
                string art_no = obj.ART_NO;
                string se_day = obj.SE_DAY.ToString().Substring(0,10);
                try
                {
                    if (updateInFinshQty(org_id, se_id, se_seq, size_no, size_seq, qty, scan_ip, vPO, art_no, se_day))
                    {
                        if (qty == UnFinishQty)
                        {
                            btnImage.Visible = true;
                            btnImage.BackgroundImage = Smile;
                            this.btnImage.BackColor = System.Drawing.Color.Transparent;
                            this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                            reflashListSize();
                        }
                        else if (qty > UnFinishQty)
                        {
                            ScanFailed();
                            MessageBox.Show("something error");
                        }
                        else
                        {
                            btnImage.Visible = true;
                            btnImage.BackgroundImage = Smile;
                            this.btnImage.BackColor = System.Drawing.Color.Transparent;
                            this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                        }
                        UnFinishQty = UnFinishQty - qty;
                        DayFinishQty += qty;
                    }
                    else
                    {
                        ScanFailed();
                    }
                }
                catch (Exception ex)
                {
                    UnFinishQty = oldunFinishQty;
                    ScanFailed();
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
                textSizeFinishQty.Text = (Convert.ToInt32(obj.SE_QTY) - UnFinishQty).ToString();
                displayQtyButton(UnFinishQty);
                //textPo.Text = vPO;
                SetDayFinishQty();
                textSize.Text = obj.SIZE_NO;
                textQty.Text = strqty;

                TextQuerySeID.Text = "";
                textQueryDept.Text = "";
            }
            else
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("请输入投入组别！", Program.client, "", Program.client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg); 
            }
        }

        /// <summary>
        /// 默认生产信息即最右边栏位的信息为空
        /// </summary>
        private void setProductInfoToDefault(int qty, int sizeFinish)
        {
            if (ListSizeSelectIndex == -1)
            {
                textSizeQty.Text = 0.ToString();
                textSizeFinishQty.Text = 0.ToString();
            }
            else
            {
                textSizeQty.Text = qty.ToString();
                textSizeFinishQty.Text = sizeFinish.ToString();
            }            
            textSize.Text = "";
            textQty.Text = "";
        }

        /// <summary>
        /// 刷新size列表
        /// </summary>
        /// <param name="listSizeIndex"></param>
       private void reflashListSize()
       {    
            listSize.Items.Remove(listSize.SelectedItem);
            SeOrdSize.RemoveAt(ListSizeSelectIndex);
            ListSizeSelectIndex = -1;
            listSize.SelectedIndex = ListSizeSelectIndex;
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
                ListSizeSelectIndex = -1;
                string txtQrCode = TextQuerySeID.Text.ToString().ToUpper();
                //B,200,A0A19040071,SM0A190415000049,485,1,9,10,48,EF9370,03996
                //类别,组织,制令,工票唯一码，工票号,订单序,size,数量,size序号,art,模号
                if (!string.IsNullOrWhiteSpace(txtQrCode) && !string.IsNullOrEmpty(txtQrCode) && txtQrCode.Contains(","))
                {
                    string[] str = txtQrCode.Split(',');
                    int length = str.Length;
                    if (ParseQrCode(str, length))
                    {
                        if (string.IsNullOrWhiteSpace(d_dept))
                        {
                            string msg = SJeMES_Framework.Common.UIHelper.UImsg("请输入投入组别！", Program.client, "", Program.client.Language);
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                            TextQuerySeID.Text = "";
                            return;
                        }
                        listSize.Items.Clear();
                        setScanSizeButtonToDefault();
                        string scan_ip = IPUtil.GetIpAddress();
                        string ord_id = str[1];
                        string se_id = str[2];
                        string se_seq = "1";
                        string size_no = str[6];
                        int qty = int.Parse(str[7]);
                        string size_seq = str[8];
                        string art_no = str[9];
                        try
                        {
                            DataTable dt = GetSeSizeDetail(se_id, se_seq, size_no);
                            if (dt.Rows.Count <= 0)
                            {
                                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("查无此数据！", Program.client, "", Program.client.Language);
                                MessageBox.Show(msg02, msg01);
                                TextQuerySeID.Text = "";
                                ScanFailed();
                                return;
                            }
                            vPO = dt.Rows[0]["PO"].ToString();
                            textPo.Text = vPO;
                            string se_day = dt.Rows[0]["SE_DAY"].ToString().Substring(0, 10);
                            int se_qty = (int)decimal.Parse(dt.Rows[0]["SE_QTY"].ToString());
                            int finish_qty = (int)decimal.Parse(dt.Rows[0]["FINISH_QTY"].ToString());
                            if (se_qty <= finish_qty)
                            {
                                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("扫描数量大于订单未投入数量！", Program.client, "", Program.client.Language);
                                MessageBox.Show(msg02, msg01);
                                TextQuerySeID.Text = "";
                                ScanFailed();
                                return;
                            }
                            if (updateInFinshQty(ord_id, se_id, se_seq, size_no, size_seq, qty, scan_ip, vPO, art_no, se_day))
                            {
                                textSizeFinishQty.Text = (finish_qty + qty).ToString();
                                DayFinishQty += qty;
                                SetDayFinishQty();
                                textSizeQty.Text = se_qty.ToString();
                                textSize.Text = size_no;
                                textQty.Text = qty.ToString();
                                //scanToDefault(str[2], str[6], str[7]);
                                btnImage.Visible = true;
                                btnImage.BackgroundImage = Smile;
                                this.btnImage.BackColor = System.Drawing.Color.Transparent;
                                this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                            }
                            else
                            {
                                ScanFailed();
                            }
                        }
                        catch (Exception ex)
                        {
                            //scanToDefault("", "", "");
                            ScanFailed();
                            SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                        }
                        TextQuerySeID.Text = "";
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(d_dept))
                    {
                        string msg = SJeMES_Framework.Common.UIHelper.UImsg("请输入投入组别！", Program.client, "", Program.client.Language);
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                        TextQuerySeID.Text = "";
                        return;
                    }
                    string seId = TextQuerySeID.Text.Trim().ToString().ToUpper().Split(new Char[] { '|' })[0];
                    if (!string.IsNullOrEmpty(seId))
                    {
                        listSize.Items.Clear();
                        vPO = "";
                        SeOrdSize = GetSeSize(seId);
                        if (SeOrdSize.Count > 0)
                        {
                            foreach (var o in SeOrdSize)
                            {
                                this.listSize.Items.Add(o.SIZE_NO);
                            }
                            vPO = SeOrdSize[0].PO;
                            TextQuerySeID.Text = "";
                        }
                        setScanSizeButtonToDefault();
                        textPo.Text = vPO;
                    }
                    setProductInfoToDefault(0, 0);
                }
            }
        }

        //输入部门
        private void textQueryDept_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string dept = textQueryDept.Text.Trim().ToString().ToUpper();
                try
                {
                    if (ValisStitchingDept(dept,"B"))
                    {
                        d_dept = dept;
                        //setProductInfoToDefault();
                        //setSizeButtonBackColorToDefault();
                        LoadDayFinishQty();
                        SetDayFinishQty();
                        textDept.Text = d_dept;
                        textQueryDept.Text = "";
                    }
                    else
                    {
                        string msg = SJeMES_Framework.Common.UIHelper.UImsg("请输入正确的针车组别！", Program.client, "", Program.client.Language);
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                    }
                }
                catch (Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
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

        private void setScanSizeButtonToDefault()
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
            setScanSizeButtonToDefault();
        }

        private void ScanFailed()
        {
            btnImage.Visible = true;
            btnImage.BackgroundImage = Cry;
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
            if (d_dept == "")
            {
                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("请输入投入组别", Program.client, "", Program.client.Language);
                MessageBox.Show(msg02, msg01);
                return;
            }

            if (ListSizeSelectIndex < 0)
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

            if (enterQty > UnFinishQty)
            {
                //string msg01 = SJeMES_Framework.Common.UIHelper.UImsg("Tips", Program.client, "", Program.client.Language);
                string msg02 = SJeMES_Framework.Common.UIHelper.UImsg("输入的数量大于可转移的数量", Program.client, "", Program.client.Language);
                MessageBox.Show(msg02, msg01);
                return;
            }

            VW_STITCHINPUT_ORD_SIZE obj = SeOrdSize[ListSizeSelectIndex];
            string org_id = obj.ORG_ID.ToString();
            string se_id = obj.SE_ID;
            string se_seq = obj.SE_SEQ.ToString();
            string size_no = obj.SIZE_NO;
            string size_seq = obj.SIZE_SEQ.ToString();
            int oldunFinishQty = UnFinishQty;
            string art_no = obj.ART_NO;
            string se_day = obj.SE_DAY.ToString().Substring(0, 10);
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
                    if (updateInFinshQty(org_id, se_id, se_seq, size_no, size_seq, enterQty, scan_ip, vPO, art_no, se_day))
                    {
                        if (enterQty == UnFinishQty)
                        {
                            btnImage.Visible = true;
                            btnImage.BackgroundImage = Smile;
                            this.btnImage.BackColor = System.Drawing.Color.Transparent;
                            this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                            reflashListSize();
                        }
                        else
                        {
                            btnImage.Visible = true;
                            btnImage.BackgroundImage = Smile;
                            this.btnImage.BackColor = System.Drawing.Color.Transparent;
                            this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                        }
                        UnFinishQty = UnFinishQty - enterQty;
                        DayFinishQty += enterQty;
                    }
                    else
                    {
                        btnImage.Visible = true;
                        btnImage.BackgroundImage = Cry;
                        this.btnImage.BackColor = System.Drawing.Color.Transparent;
                        this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    }
                }
                catch (Exception ex)
                {
                    //UnFinishQty = oldunFinishQty;
                    btnImage.Visible = true;
                    btnImage.BackgroundImage = Cry;
                    this.btnImage.BackColor = System.Drawing.Color.Transparent;
                    this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
                textSizeFinishQty.Text = (Convert.ToInt32(obj.SE_QTY) - UnFinishQty).ToString();
                displayQtyButton(UnFinishQty);
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
