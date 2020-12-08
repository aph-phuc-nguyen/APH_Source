using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace F_JMS_Reach_list
{
    public partial class F_JMS_Reach_listForm : MaterialForm
    {
        public Boolean isTitle = false;
        IList<object[]> data = null;
        private ExcelProcessor _currentExcelProcessor = null;
        public F_JMS_Reach_listForm()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, System.EventArgs e)
        {
            isTitle = true;
            this.dataGridView.AutoGenerateColumns = false;
            if (dataGridView != null)
            {
                this.dataGridView.Columns.Clear();
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "EXCEL|*.xls*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (dataGridView1 != null)
                {
                    data = new List<object[]>();
                    this.dataGridView1.Rows.Clear();
                }
                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        this.dataGridView1.Rows.Add(filename);
                        dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        this.GetExcelData(Path.GetFullPath(filename));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                dataGridView.AllowUserToAddRows = false;
                Boolean isTitle = true;
                if (data != null && data.Count > 0)
                {
                    for (int i = 0; i < data[0].Length; i++)
                    {
                        DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
                        acCode.Name = data[0][i].ToString();                      
                        acCode.HeaderText = data[0][i].ToString();
                        dataGridView.Columns.Add(acCode);
                        acCode.Width = 80;
                    }

                    foreach (object[] row in data)
                    {
                        if (!isTitle)
                        {
                            try
                            {
                                row[8] = Convert.ToDateTime(row[8]?.ToString()).ToShortDateString();
                                row[11] = Convert.ToDateTime(row[11]?.ToString()).ToShortDateString();
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show(resources.GetString("DataError"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            dataGridView.Rows.Add(row);
                        }
                        else
                        {
                            isTitle = false;
                        }
                    }
                }
                dataGridView.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
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

        private void GetExcelData(string fileName)
        {
            try
            {
                this._currentExcelProcessor = new ExcelProcessor(fileName);
                IList<object[]> list = this._currentExcelProcessor.GetSheetData(0);
                if (data != null && data.Count > 0)
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        data.Add(list[i]);
                    }
                }
                else
                {
                    data = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnOpen.Enabled = false;
            btnAdd.Enabled = false;
            if (this.dataGridView1.Rows.Count >= 1)
            {
                insert_db();
            }
            else
            {
                //文件不能为空
                String message1 = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client, "", Program.client.Language);
                //错误
                String message2 = SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.client, "", Program.client.Language);
                MessageBox.Show(message1, message2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnOpen.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void insert_db()
        {
            string ret = "";
            try{
                Boolean isValiDate = true;
                DataTable tab = new DataTable();
                object[] cols = data[0];
                for (int i = 0; i < cols.Length; i++)
                {
                    tab.Columns.Add(cols[i].ToString());
                }
                for (int i = 1; i < data.Count; i++)
                {
                    DataRow dr = tab.NewRow();
                    for (int j = 0; j < cols.Length; j++)
                    {
                        if (data[i][j] == null)
                        {
                            dr[j] = "";
                        }
                        else
                        {
                            dr[j] = data[i][j].ToString();
                        }
                    }
                    tab.Rows.Add(dr);
                }
                if (isValiDate)
                {
                    upload(tab);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
           catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            
        }

        private void upload(DataTable tab)
        {           
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("data", tab);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_GoalAccomplished_ListServer", "insert", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                SJeMES_Control_Library.MessageHelper.ShowOK(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, Object> d = new Dictionary<string, object>();
            string wk_id = textBox3.ToString().Trim();
            string plan_finish_date = dateTimePicker1.ToString();
            d.Add("wk_id", wk_id);
            d.Add("plan_finish_date", plan_finish_date);
            if (string.IsNullOrEmpty(wk_id))
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_JMSAPI", "KZ_JMSAPI.Controllers.JMS_GoalAccomplished_ListServer", "efficiencyQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            else
            {

            }
            
        }
    }
}
