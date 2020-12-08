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
using System.Windows.Forms.DataVisualization.Charting;

namespace F_MMS_Outsourcing_TrackInfo
{
    public partial class F_MMS_Outsourcing_TrackInfo : MaterialForm
    {
        public F_MMS_Outsourcing_TrackInfo()
        {
            InitializeComponent();
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            if (string.IsNullOrEmpty(textBox4.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "起止时间不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBox12.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "起止时间不能为空");
                return;
            }
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
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string vScanType = string.IsNullOrWhiteSpace(cbmScanType.Text.ToString()) ? cbmScanType.Text.ToString() : cbmScanType.Text.ToString().Split('|')[1];
            p.Add("vSeId", txtSeId.Text.ToString());
            p.Add("vCompanyCode", txtCompanyCode.Text.ToString());
            p.Add("vPartName", txtPartName.Text.ToString());
            p.Add("vScanType", vScanType);
            p.Add("vBeginDate", textBox4.Text.ToString());
            p.Add("vEndDate", textBox12.Text.ToString());
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "Query", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            textBox4.Text = dateTimePicker3.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker3_DropDown(object sender, EventArgs e)
        {
            textBox4.Text = string.Empty;

        }

        private void dateTimePicker3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

        }

        private void dateTimePicker6_ValueChanged(object sender, EventArgs e)
        {
            textBox12.Text = dateTimePicker6.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dateTimePicker6_DropDown(object sender, EventArgs e)
        {
            textBox12.Text = string.Empty;
        }

        /// <summary>
        /// 增补查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupplemt_Query_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;
            if (string.IsNullOrEmpty(textBox2.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "起止时间不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "起止时间不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBox5.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "厂商不能为空");
            }
            if (string.IsNullOrEmpty(cbmStatus.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "状态部能为空");
            }
            try
            {
                GetSupplemtData();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void GetSupplemtData()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string vStatus = string.IsNullOrWhiteSpace(cbmStatus.Text.ToString()) ? cbmStatus.Text.ToString() : cbmStatus.Text.ToString().Split('|')[1];
            p.Add("vSeId", textBox6.Text.ToString());
            p.Add("vCompanyCode", textBox5.Text.ToString());
            p.Add("vPartName", textBox7.Text.ToString());
            p.Add("vStatus", vStatus);
            p.Add("vBeginDate", textBox2.Text.ToString());
            p.Add("vEndDate", textBox1.Text.ToString());
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetSupplemtData", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dataGridView2.DataSource = dtJson.DefaultView;
                dataGridView2.Update();
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = dateTimePicker1.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dateTimePicker1_DropDown(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = dateTimePicker2.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dateTimePicker2_DropDown(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            for (int count = 0; count < dataGridView2.Rows.Count; count++)
            {
                dataGridView2.Rows[count].Cells["Check"].Value = "Y";
            }
            dataGridView2.Update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int count = 0; count < dataGridView2.Rows.Count; count++)
            {
                if (dataGridView2.Rows[count].Cells["Check"].Value.Equals("Y"))
                {
                    dataGridView2.Rows[count].Cells["Check"].Value = "N";
                }
                else
                {
                    dataGridView2.Rows[count].Cells["Check"].Value = "Y";
                }
            }
            dataGridView2.Update();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string a = "增补明细.xls";
            ExportExcels(a, dataGridView2);
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

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            DataTable dt = GetDgvToTable(dataGridView2);
            p.Add("dataTable", dt);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "updateSupplemtStatus", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dataGridView2.DataSource = dtJson.DefaultView;
                dataGridView2.Update();
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        public DataTable GetDgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("supplemtBarCode");
            dt.Columns.Add("COMPANY_CODE");
            dt.Columns.Add("SCAN_DATE");
            dt.Columns.Add("SCAN_TYPE");
            // 列强制转换
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                if (dgv.Rows[count].Cells["Check"].Value.ToString().Equals("Y"))
                {
                    dr[0] = dgv.Rows[count].Cells["supplemtBarCode"].Value.ToString();
                    dr[1] = dgv.Rows[count].Cells["dataGridViewTextBoxColumn2"].Value.ToString();
                    dr[2] = dgv.Rows[count].Cells["dataGridViewTextBoxColumn12"].Value.ToString();
                    dr[3] = dgv.Rows[count].Cells["scan_type"].Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCode003A_Query_Click(object sender, EventArgs e)
        {
            this.dataGridView3.DataSource = null;
            this.dataGridView4.DataSource = null;
            this.dataGridView5.DataSource = null;
            this.dataGridView6.DataSource = null;
            if (string.IsNullOrEmpty(textBox9.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "出库起止时间不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBox8.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "出库起止时间不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBox10.Text.ToString()))
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "厂商代码不能为空");
                return;
            }
            try
            {
                GetCode003AData();
                dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }


        private void GetCode003AData()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            string vStatus = string.IsNullOrWhiteSpace(cbmStatus.Text.ToString()) ? cbmStatus.Text.ToString() : cbmStatus.Text.ToString().Split('|')[1];
            p.Add("vSeId", textBox11.Text.ToString());
            p.Add("vCompanyCode", textBox10.Text.ToString());
            p.Add("vPartName", textBox3.Text.ToString());
            p.Add("vSize", textSize.Text.ToString());
            p.Add("vBeginDate", textBox9.Text.ToString());
            p.Add("vEndDate", textBox8.Text.ToString());
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetCode003AData", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dataGridView3.DataSource = dtJson.DefaultView;
                dataGridView3.Update();
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }


        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView4.DataSource = null;
            this.dataGridView4.AutoGenerateColumns = false;
            this.dataGridView5.DataSource = null;

            string packing_barcode = "";
            string se_id = "";
            string company = "";
            if (dataGridView3.CurrentRow != null && dataGridView3.CurrentRow.Index > -1)
            {
                int index = dataGridView3.CurrentRow.Index;
                packing_barcode = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn1"].Value == null ? "" : dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn1"].Value.ToString();
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("vPacking_barcode", packing_barcode);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetDetail", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView4.DataSource = dtJson.DefaultView;
                    dataGridView4.Update();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

                Dictionary<string, Object> p1 = new Dictionary<string, object>();
                se_id = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn5"].Value == null ? "" : dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                company = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn3"].Value == null ? "" : dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn3"].Value.ToString();
                p1.Add("vSeId", se_id);
                p1.Add("vCompany", company);
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetMatchingPart", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p1));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    for (int i = 0; i < dtJson.Rows.Count; i++)
                    {
                        int tempMatchCount = 0;
                        for (int j = 3; j < dtJson.Columns.Count - 1; j++)
                        {
                            string str = dtJson.Rows[i][j].ToString();
                            int tempCount = int.Parse(str);
                            if (j == 3)
                            {
                                tempMatchCount = tempCount;
                            }
                            tempMatchCount = tempMatchCount < tempCount ? tempMatchCount : tempCount;
                        }
                        dtJson.Rows[i][dtJson.Columns.Count - 1] = tempMatchCount;
                    }
                    dataGridView5.DataSource = dtJson.DefaultView;
                    if (dataGridView5.Rows.Count > 0)
                    {
                        dataGridView5.Columns[0].Visible = false;

                    }
                    dataGridView5.Update();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                }

                dataGridView4.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                dataGridView5.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
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
        private void BtnCode003A_Export_Click(object sender, EventArgs e)
        {
            string a = "委外进度查询.xls";
            ExportExcels(a, dataGridView3);
        }



        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            textBox9.Text = dateTimePicker4.Value.ToString("yyyy/MM/dd");
        }

        private void dateTimePicker4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

        }

        private void dateTimePicker4_DropDown(object sender, EventArgs e)
        {
            textBox9.Text = string.Empty;
        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            textBox8.Text = dateTimePicker5.Value.ToString("yyyy/MM/dd");

        }

        private void dateTimePicker5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

        }

        private void dateTimePicker5_DropDown(object sender, EventArgs e)
        {
            textBox8.Text = string.Empty;
        }

        private void dataGridView5_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView6.DataSource = null;
            this.dataGridView6.AutoGenerateColumns = false;
            string se_id = "";
            string company = "";
            string size_no = "";
            if (dataGridView5.CurrentRow != null && dataGridView5.CurrentRow.Index > -1)
            {
                int index = dataGridView5.CurrentRow.Index;
                Dictionary<string, Object> p = new Dictionary<string, object>();
                se_id = dataGridView5.Rows[index].Cells[1].Value == null ? "" : dataGridView5.Rows[index].Cells[1].Value.ToString();
                company = dataGridView5.Rows[index].Cells[0].Value == null ? "" : dataGridView5.Rows[index].Cells[0].Value.ToString();
                size_no = dataGridView5.Rows[index].Cells[2].Value == null ? "" : dataGridView5.Rows[index].Cells[2].Value.ToString();
                p.Add("vSeId", se_id);
                p.Add("vCompany", company);
                p.Add("vSizeNo", size_no);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetOutsourcingDetail", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView6.DataSource = dtJson.DefaultView;
                    dataGridView6.Update();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
                dataGridView6.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);

                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetReceivingDetail", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    dataGridView7.DataSource = dtJson.DefaultView;
                    dataGridView7.Update();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["ErrMsg"].ToString());
                }
                dataGridView7.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null && dataGridView3.CurrentRow.Index > -1)
            {
                //制令号
                int index = dataGridView3.CurrentRow.Index;
                string beginDate = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn13"].Value.ToString();
                string endDate = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn13"].Value.ToString();
                string seId = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                string partName = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn11"].Value.ToString();
                string barId = dataGridView3.Rows[index].Cells["BAR_ID"].Value.ToString();
                //厂商
                Hide();
                using (F_MMS_Outsourcing_TrackInfo_Update form = new F_MMS_Outsourcing_TrackInfo_Update(beginDate, endDate, seId, partName, barId))
                {
                    form.ShowDialog();
                }
                System.Threading.Thread.Sleep(200);
                Show();
            }
            else
            {
                Hide();
                using (F_MMS_Outsourcing_TrackInfo_Update form = new F_MMS_Outsourcing_TrackInfo_Update())
                {
                    form.ShowDialog();
                }
                System.Threading.Thread.Sleep(200);
                Show();
            }

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null && dataGridView3.CurrentRow.Index > -1)
            {
                //制令号
                int index = dataGridView3.CurrentRow.Index;
                string beginDate = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn13"].Value.ToString();
                string endDate = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn13"].Value.ToString();
                string seId = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                string partName = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn11"].Value.ToString();
                string barId = dataGridView3.Rows[index].Cells["BAR_ID"].Value.ToString();
                //厂商
                Hide();
                using (F_MMS_Outsourcing_TrackInfo_Insert form = new F_MMS_Outsourcing_TrackInfo_Insert(seId, partName, barId))
                {
                    form.ShowDialog();
                }
                System.Threading.Thread.Sleep(200);
                Show();
            }
            else
            {
                Hide();
                using (F_MMS_Outsourcing_TrackInfo_Insert form = new F_MMS_Outsourcing_TrackInfo_Insert())
                {
                    form.ShowDialog();
                }
                System.Threading.Thread.Sleep(200);
                Show();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null && dataGridView3.CurrentRow.Index > -1)
            {
                //制令号
                int index = dataGridView3.CurrentRow.Index;
                string beginDate = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn13"].Value.ToString();
                string endDate = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn13"].Value.ToString();
                string seId = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                string partName = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn11"].Value.ToString();
                string barId = dataGridView3.Rows[index].Cells["BAR_ID"].Value.ToString();
                //厂商
                Hide();
                using (F_MMS_Outsourcing_TrackInfo_Delete form = new F_MMS_Outsourcing_TrackInfo_Delete(beginDate, endDate, seId, partName, barId))
                {
                    form.ShowDialog();
                }
                System.Threading.Thread.Sleep(200);
                Show();
            }
            else
            {
                Hide();
                using (F_MMS_Outsourcing_TrackInfo_Delete form = new F_MMS_Outsourcing_TrackInfo_Delete())
                {
                    form.ShowDialog();
                }
                System.Threading.Thread.Sleep(200);
                Show();
            }
        }

        private void F_MMS_Outsourcing_TrackInfo_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView3.AutoGenerateColumns = false;
            //tabPage8.Parent = null;
            //更新多语言
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
            //权限
            String RET = SJeMES_Framework.Common.UIHelper.UIVisiable(this, "委外管理", Program.Client);

            //文件不能为空
            //String message1 = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.Client, "", Program.Client.Language);
            //错误
            // String message2 = SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.Client, "", Program.Client.Language);
            //MessageBox.Show(message1, message2, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public void RefreshData()
        {
            List<string> x1 = new List<string>() {"收料:" +(99349 - 81389), "上线:"+(81389 - 58962), "产出:"+ (69147 - 58962), "出货:"+ 69147 };
            List<int> y1 = new List<int>();
            y1 = new List<int>() { 99349-81389, 81389- 58962, 69147-58962,69147};
            RefreshChart(x1, y1, "chart1");

            List<string> x2 = new List<string>() { "09-21","09-22","09-23","09-24" };
            List<int> y2 = new List<int>();
            y2 = new List<int>() {100,200,300, 400 };
            RefreshChart(x2, y2, "chart2");

        }


        public delegate void RefreshChartDelegate(List<string> x, List<int> y, string type);
        public void RefreshChart(List<string> x, List<int> y, string type)
        {
            if (type == "chart1")
            {
                if (this.chart1.InvokeRequired)
                {
                    RefreshChartDelegate stcb = new RefreshChartDelegate(RefreshChart);
                    this.Invoke(stcb, new object[] { x, y, type });
                }
                else
                {
                    chart1.Series[0].Points.DataBindXY(x, y);
                }
            }
            else if (type == "chart2")
            {
                if (this.chart2.InvokeRequired)
                {
                    RefreshChartDelegate stcb = new RefreshChartDelegate(RefreshChart);
                    this.Invoke(stcb, new object[] { x, y, type });
                }
                else
                {
                    chart2.Series[0].Points.DataBindXY(x, y);
                    chart2.Series[1].Points.DataBindXY(x, y);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            ChartHelper.AddSeries(chart1, "饼状图", SeriesChartType.Pie, Color.White, Color.Red, true);
            ChartHelper.SetStyle(chart1, Color.Transparent, Color.Black);
            ChartHelper.SetLegend(chart1, Docking.Top, StringAlignment.Near, Color.Transparent, Color.Black);



            chart2.Series.Clear();
            ChartHelper.AddSeries(chart2, "曲线图1", SeriesChartType.Line, Color.FromArgb(100, 46, 199,201), Color.Red, true);
            ChartHelper.SetTitle(chart2, "曲线图1", new Font("微软雅黑", 12), Docking.Bottom, Color.FromArgb(46, 199, 201));
            ChartHelper.SetStyle(chart2, Color.Transparent, Color.Black);
            ChartHelper.SetLegend(chart2, Docking.Top, StringAlignment.Center, Color.Transparent, Color.Black);


            ChartHelper.AddSeries(chart2, "曲线图2", SeriesChartType.Line, Color.FromArgb(100, 46, 199, 201), Color.Red, true);
            ChartHelper.SetTitle(chart2, "曲线图2", new Font("微软雅黑", 12), Docking.Bottom, Color.FromArgb(46, 199, 201));
            ChartHelper.SetStyle(chart2, Color.Transparent, Color.Black);
            ChartHelper.SetLegend(chart2, Docking.Top, StringAlignment.Center, Color.Transparent, Color.Black);


            //ChartHelper.SetXY(chart2, "序号", "数值", StringAlignment.Far, Color.Black, Color.Black, AxisArrowStyle.SharpTriangle, 1, 2);
            //ChartHelper.SetMajorGrid(chart2, Color.Gray, 20, 2);

            RefreshData();



        }
    }
}
