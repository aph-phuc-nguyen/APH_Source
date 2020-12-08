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

namespace F_MMS_TrackOut_List
{
    public partial class F_MMS_TrackOut_ListForm : MaterialForm
    {
        DataTable ErrorDataTable = new DataTable();
        DataTable ScanDataTable = new DataTable();
        Timer timer1 = new Timer();

        public F_MMS_TrackOut_ListForm()
        {
            InitializeComponent();
        }

        private void textBoxEx1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)+@"\\wav\\fail.wav";
                if (!System.IO.File.Exists(path))
                {
                    return;
                }
                System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(path);
                if (string.IsNullOrEmpty(textBoxEx9.Text))
                {
                    MessageBox.Show("仓库不能为空");
                    sndPlayer.Play();

                    return;
                }
                if(string.IsNullOrEmpty(textBoxEx10.Text))
                {
                    sndPlayer.Play();
                    MessageBox.Show("出货、发料至不能为空");
                    return;
                }
                string txtQrCode = textBoxEx1.Text.ToString();
                //B,200,A0A19040071,SM0A190415000049,485,1,9,10,48,EF9370,03996
                //C,200,F0A20080151,SM0A200725000003,9,1,8,5,46,FX1058,30832
                //类别(B入库，C出库),组织,制令,工票唯一码，工票号,订单序,size,数量,size序号,art,模号
                if (!string.IsNullOrWhiteSpace(txtQrCode) && !string.IsNullOrEmpty(txtQrCode) && txtQrCode.Contains(","))
                {
                    string[] str = txtQrCode.Split(',');
                    int length = str.Length;
                    if (ParseQrCode(str, length))
                    {
                        string ord_id = str[1];
                        string se_id = str[2];
                        string se_seq ="1";
                        string size_no = str[6];
                        int qty = int.Parse(str[7]);
                        string size_seq = str[8];
                        string art_no = str[9];
                        string mate_tieno = str[3];
                        string tieno = str[4];
                        string to_company = textBoxEx10.Text.Trim();

                        textBoxEx2.Text = se_id;
                        textBoxEx7.Text = art_no;
                        textBoxEx6.Text = size_no;
                        textBoxEx4.Text = qty.ToString();
                        textBoxEx3.Text = mate_tieno;
                        textBoxEx5.Text = tieno;
                        SaveQrCode(ord_id,se_id,se_seq,size_no,qty,size_seq,art_no,mate_tieno,tieno, to_company);
                    }
                } 
                textBoxEx1.Text = "";
            }
        }

        private  void SaveQrCode(string ord_id,string  se_id,string se_seq,string size_no,int qty,string size_seq,string art_no,string mate_tieno, string tieno,string to_company)
        {
            try
            {
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("se_id", se_id);
                p.Add("se_seq", se_seq);
                p.Add("size_no", size_no);
                p.Add("qty", qty);
                p.Add("size_seq", size_seq);
                p.Add("art_no", art_no);
                p.Add("mate_tieno", mate_tieno);
                p.Add("tieno", tieno);
                //库位
                p.Add("stoc_no", "ALL");
                //制程
                p.Add("rout_no", "B");
                p.Add("stoc_wh", textBoxEx9.Text);
                p.Add("stoc_whname", textBoxEx8.Text);
                p.Add("to_company", to_company);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_TrackOut_ListServer", "Save", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {

                    DateTime nowDate = DateTime.Parse(DateTime.Now.ToLongTimeString());
                    DataRow dr = ScanDataTable.NewRow();
                    dr[0] = se_id;
                    dr[1] = size_no;
                    dr[2] = qty;
                    dr[3] = art_no;
                    dr[4] = mate_tieno;
                    dr[5] = tieno;
                    dr[6] = nowDate;
                    ScanDataTable.Rows.Add(dr);
                    dataGridView2.DataSource = ScanDataTable;
                    dataGridView2.Update();
                    dataGridView2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                    int NumberOfscans = string.IsNullOrEmpty(ucTextBoxEx2.Text) ? 0 : int.Parse(ucTextBoxEx2.Text);
                    ucTextBoxEx2.Text = (NumberOfscans + 1).ToString();
                    int totalOfScans = string.IsNullOrEmpty(ucTextBoxEx4.Text) ? 0 : int.Parse(ucTextBoxEx4.Text);
                    ucTextBoxEx4.Text = (totalOfScans + qty).ToString();
                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + @"\\wav\\success.wav";
                    if (!System.IO.File.Exists(path))
                    {
                        return;
                    }
                    System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(path);
                    sndPlayer.Play();
                }
                else
                {
                    DateTime nowDate = DateTime.Parse(DateTime.Now.ToLongTimeString());
                    DataRow dr = ErrorDataTable.NewRow();
                    dr[0] = se_id;
                    dr[1] = size_no;
                    dr[2] = qty;
                    dr[3] = art_no;
                    dr[4] = mate_tieno;
                    dr[5] = tieno;
                    dr[6] = nowDate;
                    ErrorDataTable.Rows.Add(dr);
                    dataGridView1.DataSource = ErrorDataTable;
                    dataGridView1.Update();
                    dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                    int NumberOfscans = string.IsNullOrEmpty(ucTextBoxEx1.Text) ? 0 : int.Parse(ucTextBoxEx1.Text);
                    ucTextBoxEx1.Text = (NumberOfscans + 1).ToString();

                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + @"\\wav\\fail.wav";
                    if (!System.IO.File.Exists(path))
                    {
                        return;
                    }
                    System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(path);
                    sndPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
            for (int j = 0; j < dataGridView2.RowCount; j++)
            {
                dataGridView2.Rows[j].DefaultCellStyle.ForeColor = Color.Black;
            }
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();
            StringFormat centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
           

        }

        private Boolean ParseQrCode(string[] str, int length)
        {
            if (length == 11)
            {
                return true;
            }
            else
            {
                MessageBox.Show("二维码长度有误，请联系系统管理员", "错误！");
                return false;
            }
        }

        private void F_MMS_TrackIn_ListForm_Load(object sender, EventArgs e)
        {
            this.textBoxEx1.Focus();
            ErrorDataTable.Columns.Add("SE_ID");
            ErrorDataTable.Columns.Add("SIZE_NO");
            ErrorDataTable.Columns.Add("Qty");
            ErrorDataTable.Columns.Add("Art");
            ErrorDataTable.Columns.Add("mate_tieno");
            ErrorDataTable.Columns.Add("tieno");
            ErrorDataTable.Columns.Add("Scan_Date");
            this.dataGridView1.AutoGenerateColumns = false;

            
            ScanDataTable.Columns.Add("SE_ID");
            ScanDataTable.Columns.Add("SIZE_NO");
            ScanDataTable.Columns.Add("Qty");
            ScanDataTable.Columns.Add("Art");
            ScanDataTable.Columns.Add("mate_tieno");
            ScanDataTable.Columns.Add("tieno");
            ScanDataTable.Columns.Add("Scan_Date");
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView3.AutoGenerateColumns = false;
            this.dataGridView4.AutoGenerateColumns = false;

            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_TrackOut_ListServer", "QueryWareHouse", Program.Client.UserToken, string.Empty);
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    for (int i=0;i<dtJson.Rows.Count;)
                    {
                        textBoxEx9.Text = dtJson.Rows[i]["MANAGEMENTAREA_NO"].ToString();
                        textBoxEx8.Text = dtJson.Rows[i]["MANAGEMENTAREA_MEMO"].ToString();
                        break;
                    }
                  
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
            timer1.Interval = 1 * 1000;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_Tick);//添加事件
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ucledTime1.Value = DateTime.Now;
        }



        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("seId",txtSeId.Text);
                p.Add("stocWhName",txtStocNo.Text);
                p.Add("beginDate",dtpBeginDate.Text);
                p.Add("endDate",dtpEndDate.Text);
                p.Add("toCompany", textBox1.Text);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_TrackOut_ListServer", "QueryScanInfo", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView3.DataSource = dtJson;
                    dataGridView3.Update();
                    dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void tabControlExt1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxEx1.Focus();
            Refresh();
        }

        private void F_MMS_TrackIn_ListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Tick -= new EventHandler(timer1_Tick);//添加事件

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = "鞋面出库资料.xls";
            ExportExcels(a, dataGridView3);
        }

        private void ExportExcels(string fileName, DataGridView myDGV)
        {
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                return;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
                                                                                                                                  //写入标题
            for (int i = 0; i < myDGV.ColumnCount; i++)
            {
                worksheet.Cells[1, i + 1] = "'" + myDGV.Columns[i].HeaderText.ToString();
            }
            //写入数值
            for (int r = 0; r < myDGV.Rows.Count; r++)
            {
                for (int i = 0; i < myDGV.ColumnCount; i++)
                {
                    worksheet.Cells[r + 2, i + 1].NumberFormatLocal = "@";
                    worksheet.Cells[r + 2, i + 1] = myDGV.Rows[r].Cells[i].Value;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            xlApp.Quit();
            GC.Collect();//强行销毁
            MessageBox.Show("保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxEx9.Text))
            {
                MessageBox.Show("仓库不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBoxEx11.Text))
            {
                MessageBox.Show("出货公司不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBoxEx19.Text))
            {
                MessageBox.Show("订单不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBoxEx18.Text))
            {
                MessageBox.Show("鞋款为空");
                return;
            }
            if (string.IsNullOrEmpty(textBoxEx16.Text))
            {
                MessageBox.Show("Size不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBoxEx17.Text))
            {
                MessageBox.Show("数量不能为空");
                return;
            }
            

            string ord_id ="";
            string se_id = textBoxEx19.Text.Trim();
            string se_seq ="1";
            string size_no = textBoxEx16.Text.Trim();
            int qty = int.Parse(textBoxEx17.Text.Trim());
            string size_seq = textBoxEx12.Text.Trim();
            string art_no = textBoxEx18.Text.Trim();
            string mate_tieno = "userManual" +DateTime.Now;
            string tieno ="1";
            string to_company = textBoxEx11.Text.Trim();
            SaveQrCode(ord_id, se_id, se_seq, size_no, qty, size_seq, art_no, mate_tieno, tieno,to_company);
        }

        private void textBoxEx19_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBoxEx19.Text.Trim()))
                {
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("seId", textBoxEx19.Text.Trim());
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_TrackOut_ListServer", "QuerySeInfo", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        textBoxEx18.Text = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        
                    }
                }              
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void textBoxEx16_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBoxEx16.Text.Trim())&& string.IsNullOrEmpty(textBoxEx19.Text.Trim()))
                {
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("seId", textBoxEx19.Text.Trim());
                    p.Add("sizeNo", textBoxEx16.Text.Trim());
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_TrackOut_ListServer", "QuerySizeInfo", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        textBoxEx12.Text = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void btnDetailQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("seId", textBox2.Text);
                p.Add("stocWhName", textBox4.Text);
                p.Add("beginDate", dateTimePicker2.Text);
                p.Add("endDate", dateTimePicker1.Text);
                p.Add("toCompany", textBox3.Text);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_TrackOut_ListServer", "QueryDetailScanInfo", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView4.DataSource = dtJson;
                    dataGridView4.Update();
                    dataGridView4.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                }
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void btnDetailExport_Click(object sender, EventArgs e)
        {
            string a = "鞋面入库明细资料.xls";
            ExportExcels(a, dataGridView4);
        }
    }
}
