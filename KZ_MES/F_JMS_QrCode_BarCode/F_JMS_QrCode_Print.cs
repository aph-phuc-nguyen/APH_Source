using AutocompleteMenuNS;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_JMS_QrCode_Print
{
    public partial class F_JMS_QrCode_Print : MaterialForm
    {
        string rout_no = "";
        int print_ver = 0;
        int end_tieno = 0;
        DataTable print_dt = null;
        DataTable dtRout = null;

        public F_JMS_QrCode_Print()
        {
            InitializeComponent();
        }

        private void F_JMS_QrCode_Print_Load(object sender, EventArgs e)
        {
            try
            {
                //LoadPrinter();
                GetComboBoxUI();
                LoadSeId();
            }
            catch(Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void GetComboBoxUI()
        {
            List<UnitEntry> unitEntries = new List<UnitEntry> { };
            unitEntries.Add(new UnitEntry() { Code = "NULL", Name = "" });

            List<RoutEntry> routEntries = new List<RoutEntry> { };
            routEntries.Add(new RoutEntry() { Code = "NULL", Name = "" });

            string retUnit = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetUnitList", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retUnit)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retUnit)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    unitEntries.Add(new UnitEntry() { Code = dtJson.Rows[i]["CODE"].ToString(), Name = dtJson.Rows[i]["ORG"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retUnit)["ErrMsg"].ToString());
            }

            string retRout = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetRoutList", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retRout)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retRout)["RetData"].ToString();
                dtRout = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtRout.Rows.Count; i++)
                {
                    routEntries.Add(new RoutEntry() { Code = dtRout.Rows[i]["ROUT_NO"].ToString(), Name = dtRout.Rows[i]["ROUT_NAME"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retRout)["ErrMsg"].ToString());
            }

            this.cbUnit.DataSource = unitEntries;
            this.cbUnit.DisplayMember = "Name";
            this.cbUnit.ValueMember = "Code";

            this.cbRout.DataSource = routEntries;
            this.cbRout.DisplayMember = "Name";
            this.cbRout.ValueMember = "Code";
            this.cbRout.SelectedIndexChanged += new EventHandler(cbRout_SelectedIndexChanged);

            List<RoutEntry> routT1 = new List<RoutEntry> { };
            routEntries.ForEach(i => routT1.Add(i));
            this.comboRout.DataSource = routT1;
            this.comboRout.DisplayMember = "Name";
            this.comboRout.ValueMember = "Code";

            this.Column6.DataSource = routT1;
            this.Column6.DisplayMember = "Name";
            this.Column6.ValueMember = "Code";
        }

        //查找打印机     Find printers  
        //private void LoadPrinter()
        //{
        //    foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
        //    {
        //        cbPrinter.Items.Add(fPrinterName);
        //        cbPrinter2.Items.Add(fPrinterName);
        //    }
        //    int maxSize = 0;
        //    System.Drawing.Graphics g = CreateGraphics();
        //    for (int i = 0; i < cbPrinter.Items.Count; i++)
        //    {
        //        cbPrinter.SelectedIndex = i;
        //        SizeF size = g.MeasureString(cbPrinter.Text, cbPrinter.Font);
        //        if (maxSize < (int)size.Width)
        //        {
        //            maxSize = (int)size.Width;
        //        }
        //    }
        //    cbPrinter.DropDownWidth = cbPrinter.Width;
        //    if (cbPrinter.DropDownWidth < maxSize)
        //    {
        //        cbPrinter.DropDownWidth = maxSize;
        //        cbPrinter2.DropDownWidth = maxSize;
        //    }
        //    cbPrinter.SelectedIndex = -1;
        //    cbPrinter2.SelectedIndex = -1;
        //}

        //查找制令，PO,ART_NO     Find the order ,PO and ART_NO
        private void LoadSeId()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(280, 350);
            var columnWidth = new int[] { 50, 250 };
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetSeidList", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 1; i <= dtJson.Rows.Count; i++)
                {
                    autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { i + "", dtJson.Rows[i - 1]["se_id"].ToString()}, dtJson.Rows[i - 1]["se_id"].ToString()) { ColumnWidth = columnWidth, ImageIndex = i });
                }
            }
        }

        private void cbRout_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearUI();
            rout_no = cbRout.SelectedValue.ToString();
            if ("NULL".Equals(cbRout.SelectedValue.ToString()))
            {
                textNum.Text = "";
            }
            else
            {
                for (int i = 0; i < dtRout.Rows.Count; i++)
                {
                    if (rout_no.Equals(dtRout.Rows[i]["ROUT_NO"].ToString()))
                    {
                        textNum.Text = dtRout.Rows[i]["PRINT_NUM"].ToString();
                        return;
                    }
                }
            }
        }

        private void textSeid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (string.IsNullOrEmpty(textSeid.Text.Trim()))
            {
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if ("".Equals(rout_no))
                {
                    MessageBox.Show("请先选择制程");
                    return;
                }
                textPrintVer.Text = "";
                textPrintSeid.Text = "";
                print_ver = 0;
                end_tieno = 0;
                dataGridView1.Columns.Clear();
                dataGridView2.Columns.Clear();
                try
                {
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("vSeId", textSeid.Text.Trim());
                    p.Add("vRoutNo", rout_no);
                    string retSizeList = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetSizeListBySeID", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retSizeList)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retSizeList)["RetData"].ToString();
                        DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        if (dt.Rows.Count > 0)
                        {
                            DataTable tb1 = new DataTable();
                            DataTable tb2 = new DataTable();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                tb1.Columns.Add(dt.Rows[i][0].ToString());
                                tb2.Columns.Add(dt.Rows[i][0].ToString());
                            }
                            DataRow tb1_dr1 = tb1.NewRow();
                            DataRow tb1_dr2 = tb1.NewRow();
                            DataRow tb1_dr3 = tb1.NewRow();
                            DataRow tb2_dr1 = tb2.NewRow();
                            DataRow tb2_dr2 = tb2.NewRow();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                tb1_dr1[i] = dt.Rows[i][1].ToString();//订单数量
                                tb1_dr2[i] = dt.Rows[i][2].ToString();//已打印数量
                                tb1_dr3[i] = dt.Rows[i][3].ToString();//未打印数量
                                tb2_dr1[i] = dt.Rows[i][3].ToString();//本次打印数量
                                tb2_dr2[i] = dt.Rows[i][4].ToString();//size_seq
                            }
                            tb1.Rows.Add(tb1_dr1);
                            tb1.Rows.Add(tb1_dr2);
                            tb1.Rows.Add(tb1_dr3);
                            tb2.Rows.Add(tb2_dr1);
                            tb2.Rows.Add(tb2_dr2);

                            dataGridView1.DataSource = tb1;
                            dataGridView2.DataSource = tb2;
                            dataGridView2.Rows[1].Visible = false;

                            print_dt = tb2;

                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                            }

                            Dictionary<string, Object> p2 = new Dictionary<string, object>();
                            p2.Add("vSeId", textSeid.Text.Trim());
                            p2.Add("vRoutNo", rout_no);
                            string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetMaxPrintVer", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p2));
                            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
                            {
                                string json2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                                DataTable dt2 = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json2);
                                if (dt2.Rows.Count > 0)
                                {
                                    print_ver = int.Parse(dt2.Rows[0][0].ToString());
                                    end_tieno = int.Parse(dt2.Rows[0][1].ToString());
                                }
                                print_ver++;
                                textPrintVer.Text = print_ver.ToString();
                                textPrintSeid.Text = textSeid.Text.Trim();
                            }
                        }
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retSizeList)["ErrMsg"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
        }

        private void butPrint_Click(object sender, EventArgs e)
        {
            butPrint.Enabled = false;
            if (string.IsNullOrEmpty(cbRout.Text))
            {
                MessageBox.Show("请填写制程");
                butPrint.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(cbUnit.Text))
            {
                MessageBox.Show("请填写加工单位");
                butPrint.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(textSeid.Text))
            {
                MessageBox.Show("请填写制令号");
                butPrint.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(textStartDate.Text))
            {
                MessageBox.Show("请填写计划开工日期");
                butPrint.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(textFinishDate.Text))
            {
                MessageBox.Show("请填写计划完工日期");
                butPrint.Enabled = true;
                return;
            }
            if (DateTime.Parse(textStartDate.Text) > DateTime.Parse(textFinishDate.Text))
            {
                MessageBox.Show("计划开工日期不能大于计划完工日期");
                butPrint.Enabled = true;
                return;
            }
            //if (string.IsNullOrEmpty(cbType.Text))
            //{
            //    MessageBox.Show("请填写打印类型");
            //    return;
            //}
            //if (string.IsNullOrEmpty(cbPrinter.Text))
            //{
            //    MessageBox.Show("请选择打印机");
            //    butPrint.Enabled = true;
            //    return;
            //}

            try
            {
                for (int i = 0; i < print_dt.Columns.Count; i++)
                {
                    if (string.IsNullOrEmpty(dataGridView2.Rows[0].Cells[i].Value.ToString().Trim()))
                    {
                        print_dt.Rows[0][i] = 0;
                    }
                    int qty_print = int.Parse(dataGridView2.Rows[0].Cells[i].Value.ToString());//本次打印数量
                    int qty_unfinish = int.Parse(dataGridView1.Rows[2].Cells[i].Value.ToString());//剩余未打印数量
                    if (qty_print > qty_unfinish)
                    {
                        MessageBox.Show("输入数量大于未打印数量");
                        butPrint.Enabled = true;
                        return;
                    }
                }

                Dictionary<string, Object> cp = new Dictionary<string, object>();
                cp.Add("data", print_dt);
                cp.Add("vUnit", cbUnit.SelectedValue.ToString());
                cp.Add("vSeId", textSeid.Text.Trim());
                cp.Add("vStartDate", textStartDate.Text);
                cp.Add("vFinishDate", textFinishDate.Text);
                cp.Add("vRoutNo", rout_no);
                cp.Add("vPrintVer", print_ver.ToString());
                cp.Add("vEndTieno", end_tieno.ToString());
                cp.Add("vPrintNum", textNum.Text);
                string retInResule = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "InsertQrCodeData", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(cp));
                if (!Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retInResule)["IsSuccess"]))
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retInResule)["ErrMsg"].ToString());
                    butPrint.Enabled = true;
                    return;
                }
                //else
                //{
                //    SJeMES_Control_Library.MessageHelper.ShowErr(this, "数据写入成功！");
                //}

                Dictionary<string, Object> pintDate = new Dictionary<string, object>();
                //pintDate.Add("vRoutNo", rout_no);
                pintDate.Add("vSeId", textSeid.Text.Trim());
                pintDate.Add("vPrintVer", print_ver.ToString());
                pintDate.Add("vRoutNo", rout_no);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetQrCodeBySeidAndVer", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(pintDate));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    if (dt.Rows.Count <= 0)
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, "没有打印数据！");
                        butPrint.Enabled = true;
                        return;
                    }
                    string path = Application.StartupPath + @"\报表" + "\\二维码打印.frx";
                    QrCodePrint frm = new QrCodePrint(dt, path);
                    frm.Show();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
                butPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                butPrint.Enabled = true;
            }
        }

        //清空输入选项
        private void butClear_Click(object sender, EventArgs e)
        {
            ClearUI();
        }

        private void ClearUI()
        {
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            textSeid.Text = "";
            textStartDate.Text = "";
            textFinishDate.Text = "";
            textPrintVer.Text = "";
            textPrintSeid.Text = "";
            this.cbUnit.SelectedIndex = 0;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textStartDate.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            textFinishDate.Text = dateTimePicker2.Value.ToString("yyyy-MM-dd");
        }

        //历史打印数据查询
        private void butQurey_Click(object sender, EventArgs e)
        {
            this.dataGridView_se.DataSource = null;
            this.dataGridView_se.AutoGenerateColumns = false;
            string seId = text_seid.Text.Trim();
            if (string.IsNullOrEmpty(comboRout.Text))
            {
                MessageBox.Show("请选择制程");
                return;
            }
            if ("".Equals(seId))
            {
                MessageBox.Show("请输入制令号");
                return;
            }

            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vSeId", seId);
            p.Add("vRoutNo", comboRout.SelectedValue.ToString());
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetMListBySeid", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                if (dt.Rows.Count > 0)
                {
                    dataGridView_se.DataSource = dt;
                    //dataGridView_se.CurrentCell = null;
                    dataGridView_se.Rows[0].Selected = false;
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, "没有查询到相关数据");
                }
            }
        }

        private void dataGridView_se_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView_size.DataSource = null;
            this.dataGridView_size.AutoGenerateColumns = false;
            if (!dataGridView_se.Focused)
            {
                return;
            }
            if (dataGridView_se.CurrentRow != null && dataGridView_se.CurrentRow.Index > -1)
            {
                int index = dataGridView_se.CurrentRow.Index;
                string seId = dataGridView_se.Rows[index].Cells[0].Value == null ? "" : dataGridView_se.Rows[index].Cells[0].Value.ToString();
                string ptintVer = dataGridView_se.Rows[index].Cells[2].Value == null ? "" : dataGridView_se.Rows[index].Cells[2].Value.ToString();
                string vRoutNo = dataGridView_se.Rows[index].Cells[1].Value == null ? "" : dataGridView_se.Rows[index].Cells[1].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vSeId", seId);
                p.Add("vPrintVer", ptintVer);
                p.Add("vRoutNo", vRoutNo);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "QueryDListBySeidAndVer", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView_size.DataSource = dtJson.DefaultView;
                    dataGridView_size.Update();
                    //dataGridView_size.CurrentCell = null;
                    dataGridView_size.Rows[0].Selected = false;
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }

        private void dataGridView_size_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView_detail.DataSource = null;
            this.dataGridView_detail.AutoGenerateColumns = false;
            if (!dataGridView_size.Focused)
            {
                return;
            }
            if (dataGridView_size.CurrentRow != null && dataGridView_size.CurrentRow.Index > -1)
            {
                int index = dataGridView_size.CurrentRow.Index;
                string seId = dataGridView_size.Rows[index].Cells[0].Value == null ? "" : dataGridView_size.Rows[index].Cells[0].Value.ToString();
                string ptintVer = dataGridView_size.Rows[index].Cells[1].Value == null ? "" : dataGridView_size.Rows[index].Cells[1].Value.ToString();
                string sizeNo = dataGridView_size.Rows[index].Cells[2].Value == null ? "" : dataGridView_size.Rows[index].Cells[2].Value.ToString();
                string vRoutNo = dataGridView_size.Rows[index].Cells[6].Value == null ? "" : dataGridView_size.Rows[index].Cells[6].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vSeId", seId);
                p.Add("vPrintVer", ptintVer);
                p.Add("vSizeNo", sizeNo);
                p.Add("vRoutNo", vRoutNo);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "QueryDDetialBySize", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView_detail.DataSource = dtJson.DefaultView;
                    dataGridView_detail.Update();
                    //dataGridView_detail.CurrentCell = null;
                    dataGridView_detail.Rows[0].Selected = false;
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }

        private void butPrint2_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(cbPrinter2.Text))
            //{
            //    MessageBox.Show("请选择打印机");
            //    return;
            //}
            DataTable print_result = null;

            if (dataGridView_detail.CurrentRow != null && dataGridView_detail.CurrentRow.Index > -1 && dataGridView_detail.SelectedRows.Count > 0)
            {
                int index = dataGridView_detail.CurrentRow.Index;
                string seId = dataGridView_detail.Rows[index].Cells[0].Value == null ? "" : dataGridView_detail.Rows[index].Cells[0].Value.ToString();
                string tieNo = dataGridView_detail.Rows[index].Cells[2].Value == null ? "" : dataGridView_detail.Rows[index].Cells[2].Value.ToString();
                string vRoutNo = dataGridView_detail.Rows[index].Cells[5].Value == null ? "" : dataGridView_detail.Rows[index].Cells[5].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vSeId", seId);
                p.Add("vTieNo", tieNo);
                p.Add("vRoutNo", vRoutNo);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetQrCodeByTieNo", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    print_result = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
            else if (dataGridView_size.CurrentRow != null && dataGridView_size.CurrentRow.Index > -1 && dataGridView_size.SelectedRows.Count > 0)
            {
                int index = dataGridView_size.CurrentRow.Index;
                string seId = dataGridView_size.Rows[index].Cells[0].Value == null ? "" : dataGridView_size.Rows[index].Cells[0].Value.ToString();
                string ptintVer = dataGridView_size.Rows[index].Cells[1].Value == null ? "" : dataGridView_size.Rows[index].Cells[1].Value.ToString();
                string sizeNo = dataGridView_size.Rows[index].Cells[2].Value == null ? "" : dataGridView_size.Rows[index].Cells[2].Value.ToString();
                string vRoutNo = dataGridView_size.Rows[index].Cells[6].Value == null ? "" : dataGridView_size.Rows[index].Cells[6].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vSeId", seId);
                p.Add("vPrintVer", ptintVer);
                p.Add("vSizeNo", sizeNo);
                p.Add("vRoutNo", vRoutNo);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetQrCodeBySizeAndVer", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    print_result = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            else if (dataGridView_se.CurrentRow != null && dataGridView_se.CurrentRow.Index > -1 && dataGridView_se.SelectedRows.Count > 0)
            {
                int index = dataGridView_se.CurrentRow.Index;
                string seId = dataGridView_se.Rows[index].Cells[0].Value == null ? "" : dataGridView_se.Rows[index].Cells[0].Value.ToString();
                string ptintVer = dataGridView_se.Rows[index].Cells[2].Value == null ? "" : dataGridView_se.Rows[index].Cells[2].Value.ToString();
                string vRoutNo = dataGridView_se.Rows[index].Cells[1].Value == null ? "" : dataGridView_se.Rows[index].Cells[1].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vSeId", seId);
                p.Add("vPrintVer", ptintVer);
                p.Add("vRoutNo", vRoutNo);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_QRCode_Print_Server", "GetQrCodeBySeidAndVer", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    print_result = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请选择打印内容");
                return;
            }
            string path = Application.StartupPath + @"\报表" + "\\二维码打印.frx";
            QrCodePrint frm = new QrCodePrint(print_result, path);
            frm.Show();
        }


        public class UnitEntry
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class RoutEntry
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        
    }
}
