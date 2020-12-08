using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AutocompleteMenuNS;
using MaterialSkin.Controls;

namespace Barcodeprinting
{
    public partial class BarcodeprintingForm : MaterialForm
    {
        string zhiling = string.Empty;
        string BAR_ID = string.Empty;
        string OUT_UNIT = string.Empty;
        string UNIT = string.Empty;
        string xulie = string.Empty;
        DataTable dataTable = new DataTable();

        public BarcodeprintingForm()
        {
            InitializeComponent();
            GetAllInitInfo(this.Controls[0]);
        }

        //打印方法，把刚刚插入的数据，查找出来，打印表格    Print method, take the data you just inserted, look it up, print the table       
        private void LoadReportData(string xuh)
        {
            if (!string.IsNullOrEmpty(xuh))
            {
                string ss = string.Empty;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (i == dataTable.Rows.Count - 1)
                    {
                        if (i>2)
                        {
                            break;
                        }
                        ss += dataTable.Rows[i][0];
                    }
                    else
                    {
                        ss += dataTable.Rows[i][0] + ",";
                    }
                }
                Dictionary<string, Object> p = new Dictionary<string, object>();
                p.Add("ss", ss);
                p.Add("xuh", xuh);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "LoadReportData", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    string path = Application.StartupPath + @"\报表" + "\\条码打印.frx";
                    MidWarehousePrint frm = new MidWarehousePrint(dt, path);
                    frm.Show();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSeId();
            this.dataGridView2.AutoGenerateColumns = false;
            dataTable.Columns.Add("SE_ID");
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                comboBox2.Items.Add(fPrinterName);
            }
        }

        double formWidth;//窗体原始宽度        the forms Original width
        double formHeight;//窗体原始高度       the forms Original height
        Dictionary<string, string> controlInfo = new Dictionary<string, string>();

        //各控件大小自动缩放      
        protected void GetAllInitInfo(Control CrlContainer)
        {
            if (CrlContainer.Parent == this)
            {
                formWidth = Convert.ToDouble(CrlContainer.Width);
                formHeight = Convert.ToDouble(CrlContainer.Height);
            }
            foreach (Control item in CrlContainer.Controls)
            {
                if (item.Name.Trim() != "")
                    controlInfo.Add(item.Name, (item.Left + item.Width / 2) + "," + (item.Top + item.Height / 2) + "," + item.Width + "," + item.Height + "," + item.Font.Size);
                if ((item as UserControl) == null && item.Controls.Count > 0)
                    GetAllInitInfo(item);
            }
        }

        //查找制令，PO,ART_NO     Find the order ,PO and ART_NO
        private void LoadSeId()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(350, 350);
            var columnWidth = new int[] { 50, 250 };
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "SeidQuery", Program.client.UserToken, string.Empty);       
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 1; i <= dtJson.Rows.Count; i++)
                {
                    autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { i + "", dtJson.Rows[i - 1]["se_id"] + "  " + dtJson.Rows[i - 1]["po_no"]+"  "+ dtJson.Rows[i - 1]["art_no"] }, dtJson.Rows[i - 1]["se_id"].ToString()) { ColumnWidth = columnWidth, ImageIndex = i });
                }
            }

        }

        //外发单位查询    query the outgoing unit
        private void comboBox5_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox5.Items.Clear();
            //鼠标指针顺序
            this.comboBox5.SelectionStart = this.comboBox5.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox5.DroppedDown = true;
            string s = comboBox5.Text.Trim();              //获取输入内容

            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "OutgoingQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();//存放数据库查询结果
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["DEPARTMENT_CODE"].ToString() + "   " + dt.Rows[i]["DEPARTMENT_NAME"].ToString());
                }
            }
            if (sList.Count != 0)
            {
                this.comboBox5.Items.AddRange(sList.ToArray());
                this.comboBox5.SelectionStart = this.comboBox5.Text.Length;
            }
            else
            {
                this.comboBox5.Items.Add("");
                this.comboBox5.SelectionStart = this.comboBox5.Text.Length;
            }
        }

        //查询加工单位    query the processing unit
        private void comboBox6_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox6.Items.Clear();
            //鼠标指针顺序
            this.comboBox6.SelectionStart = this.comboBox6.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox6.DroppedDown = true;
            string s = comboBox6.Text.Trim();              //获取输入内容
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "ProcessingQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();//存放数据库查询结果
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["VEND_NO"].ToString() + "   " + dt.Rows[i]["SHORTNM_S"].ToString());
                }

            }
            if (sList.Count != 0)
            {
                this.comboBox6.Items.AddRange(sList.ToArray());
                this.comboBox6.SelectionStart = this.comboBox6.Text.Length;
            }
            else
            {
                this.comboBox6.Items.Add("");
                this.comboBox6.SelectionStart = this.comboBox6.Text.Length;
            }


        }

        //查找部件名称    query the Part name
        private void comboBox8_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除     Clear before to query
            this.comboBox8.Items.Clear();
            //鼠标指针顺序      Mouse pointer sequence
            this.comboBox8.SelectionStart = this.comboBox8.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            //Keep the mouse pointer in its original state. Sometimes the mouse pointer will be overridden by the drop-down box, so do this once.
            Cursor = Cursors.Default;
            this.comboBox8.DroppedDown = true;     //自动出现下拉列表     The drop-down list appears automatically
            string s = comboBox8.Text.Trim();      //获取输入内容,去除空格    Gets input and removes whitespace
            string wkid = "";
            for (int i = 0;i < dataGridView2.Rows.Count; i++)
            {
                string str = dataGridView2.Rows[i].Cells["se_id"].Value.ToString();
                if (i ==dataGridView2.Rows.Count-1)
                {
                    wkid += "'" + str + "'";
                }
                else
                {
                    wkid += "'" + str + "',";
                }
            }          

            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            p.Add("wkid", wkid);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "PartsQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();    //存放数据库查询结果     Store database query results
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["部件名称"].ToString());
                }
            }
            if (sList.Count != 0)
            {
                this.comboBox8.Items.AddRange(sList.ToArray());
                this.comboBox8.SelectionStart = this.comboBox8.Text.Length;
            }
            else
            {
                this.comboBox8.Items.Add("");
                this.comboBox8.SelectionStart = this.comboBox8.Text.Length;
            }
        }


        /// <summary>
        /// 下面是报表的代码
        /// Here is the code  for the report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //  报表查询按钮  this is the query click for the report
        private void button5_Click(object sender, EventArgs e)
        {
            label34.Text = string.Empty;
            label32.Text = string.Empty;
            label30.Text = string.Empty;
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
            }
            if (!string.IsNullOrEmpty(comboBox11.Text))
            {
                string where = string.Empty;
                if (!string.IsNullOrEmpty(comboBox11.Text))
                {
                    where += " and WK_ID like '%" + comboBox11.Text + "%'";
                }
                if (!string.IsNullOrEmpty(textBox5.Text))
                {
                    where += " and PROD_NO= '" + textBox5.Text + "'";
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    where += " and CREATEDATE like TO_DATE('" + textBox3.Text + "','YYYY-MM-DD')";
                }
                if (!string.IsNullOrEmpty(comboBox10.Text))
                {
                    string UNIT = string.Empty;
                    if (!string.IsNullOrEmpty(comboBox10.Text))
                    {
                        UNIT = Regex.Split(comboBox10.Text, "\\s+", RegexOptions.IgnoreCase)[0];
                      
                    }
                    where += " and (UNIT='" + UNIT + "' or OUT_UNIT='" + UNIT + "') ";
                }

                if (!string.IsNullOrEmpty(comboBox9.Text) && comboBox9.Text != "全部")
                {
                    where += " and OPERATION_TYPE like '" + comboBox9.Text.ToString().Substring(0, 1) + "%'";
                }
               
                Dictionary<string, string> p = new Dictionary<string, string>();
                p.Add("where", where);
                string BAR_ID = string.Empty;
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "ReportingOrderQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (string.IsNullOrEmpty(comboBox10.Text))
                            {
                                if (string.IsNullOrEmpty(BAR_ID))
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID = "sum(decode(SHOE_NO, '" + dt.Rows[i]["SHOE_NO"].ToString() + "', QTY, 0)) as " + str + "";
                                }
                                else
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID += ",sum(decode(SHOE_NO, '" + dt.Rows[i]["SHOE_NO"].ToString() + "', QTY, 0)) as " + str + "";
                                }
                            }
                            else
                            {
                                string value = dt.Rows[i]["SHOE_NO"].ToString() + Regex.Split(comboBox10.Text, "\\s+", RegexOptions.IgnoreCase)[0];
                                if (string.IsNullOrEmpty(BAR_ID))
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID = "sum(decode(SHOE_NO||UNIT, '" + value + "', QTY, 0)) as " + str + "";
                                }
                                else
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID += ",sum(decode(SHOE_NO||UNIT, '" + value + "', QTY, 0)) as " + str + "";
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(BAR_ID))
                {
                    string size = "\"SIZE\"";
                    string sum = "\"订单数量\"";
                    string wkid = "\"制令\"";
                    string xiex = "\"ART_NO\"";

                    Dictionary<string, string> a = new Dictionary<string, string>();
                    a.Add("size", size);
                    a.Add("sum", sum);
                    a.Add("wkid", wkid);
                    a.Add("xiex", xiex);
                    a.Add("BAR_ID", BAR_ID);
                    a.Add("where", where);
  
                    string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "ReportingQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(a));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                        DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        BuidGrid(dt);
                        DataGridViewTextBoxColumn acCode1 = new DataGridViewTextBoxColumn();
                        acCode1.Name = "配套数";
                        acCode1.DataPropertyName = "配套数";
                        acCode1.HeaderText = "配套数";
                        dataGridView1.Columns.Add(acCode1);
                        //获取每行的合计以及最小值
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            string qty2 = string.Empty;
                            for (int j = 4; j < dataGridView1.Columns.Count; j++)
                            {
                                if (dataGridView1.Rows[i].Cells[j].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[j].Value.ToString()))
                                {
                                    if (string.IsNullOrEmpty(qty2))
                                    {
                                        qty2 = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                    }
                                    else
                                    {
                                        qty2 += "," + dataGridView1.Rows[i].Cells[j].Value.ToString();
                                    }

                                }
                            }
                            //sql = "";
                            //dataGridView1.Rows[i].Cells[3].Value = qty;
                            if (!string.IsNullOrEmpty(qty2))
                            {
                                string[] str = qty2.Split(','); //将输入分开放入数组中
                                //long min = Convert.ToInt64(str[0]);
                                double min= Convert.ToDouble(str[0]);
                                //定义了最大值,最小值,这里解释一下,long和int64指的是一个长度
                                //min 必须在foreach循环外定义和初始化！！！
                                foreach (string aa in str)
                                {
                                    if (Convert.ToDouble(aa) < min) { min = Convert.ToDouble(aa); }
                                }
                                dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - 1].Value = min;
                            }
                        }
                        GetZhiling(dt);
                        GetSum();
                        GetCount();
                    }
   
                }
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].Frozen = false;
                }
            }
            else
            {
                MessageBox.Show("请选择制令");
            }
        }

        //统计该制令的总数方法       The method for calculating the total amount of the order  
        public void GetSum()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() != "合计")
                {
                    string str = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string sql = string.Empty;
                    Dictionary<string, string> p = new Dictionary<string, string>();
                    p.Add("str", str);                 
                    p.Add("zhiling", zhiling);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "GetSum", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        dataGridView1.Rows[i].Cells[3].Value = json;
                    }
                }
            }
        }

        //增加多一个合计总数的行      Add one more row to the total
        public void GetCount()
        {
            int countIndex = dataGridView1.Rows.Add();
            DataGridViewRow countRow = dataGridView1.Rows[countIndex];
            countRow.Cells[0].Value = "Total:";
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (i == 1 || i == 2)
                {
                    continue;
                }
                else if (i != 0)
                {
                    double qty = 0;
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        if (dataGridView1.Rows[j].Cells[i].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[j].Cells[i].Value.ToString()))
                        {
                            qty += Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value.ToString());
                        }
                    }
                    countRow.Cells[i].Value = qty;
                }
            }
        }

        //查出该ART_NO的鞋型         Find out the ART_NO shoe shape
        public string DelArraySame(string TempArray)
        {
            string nStr = string.Empty;
            for (int i = 0; i < TempArray.Split(',').Length; i++)
            {
                Dictionary<string, string> a = new Dictionary<string, string>();
                a.Add("TempArray", TempArray.Split(',')[i]);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "DelArraySame", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(a));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    if (!nStr.Contains(RetData))
                    {
                        if (string.IsNullOrEmpty(nStr))
                        {
                            nStr = RetData;
                        }
                        else
                        {
                            nStr += "," + RetData;
                        }
                    }
                }       
            }
            return nStr;
        }

        //用逗号分隔查出的制令       use comma to separated the order
        public string DelArraySamezl(string TempArray)
        {
            string nStr = string.Empty;
            for (int i = 0; i < TempArray.Split(',').Length; i++)
            {
                if (!nStr.Contains(TempArray.Split(',')[i]))
                {
                    if (string.IsNullOrEmpty(nStr))
                    {
                        nStr = TempArray.Split(',')[i];
                    }
                    else
                    {
                        nStr += "," + TempArray.Split(',')[i];
                    }
                }             
            }
            return nStr;
        }

        //制令，鞋型，订单总数         oder，shoe          
        public void GetZhiling(DataTable table)
        {
            if (table.Rows.Count > 0)
            {
                zhiling = string.Empty;
                string xiex = string.Empty;
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(zhiling))
                        {
                            zhiling = table.Rows[i]["制令"].ToString();
                        }
                        else
                        {
                            zhiling += "," + table.Rows[i]["制令"].ToString();
                        }
                        if (string.IsNullOrEmpty(xiex))
                        {
                            xiex = table.Rows[i]["ART_NO"].ToString();
                        }
                        else
                        {
                            xiex += "," + table.Rows[i]["ART_NO"].ToString();
                        }
                    }

                }
                
                Dictionary<string, string> a = new Dictionary<string, string>();
                a.Add("zhiling", zhiling); 
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "GetZhiling", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(a));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    label30.Visible = true;
                    label30.Text = RetData;
                }

                label34.Visible = true;
                label34.Text = DelArraySamezl(zhiling);
                label32.Visible = true;
                label32.Text = DelArraySame(xiex);
         
            }
        }

        //表格输出方法        Tabular output method
        private void BuidGrid(DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                DataColumn dcol = table.Columns[i];
                DataGridViewColumn column = new DataGridViewColumn(new DataGridViewTextBoxCell());
                dataGridView1.Columns.Add(column);
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.HeaderText = dcol.ColumnName;
                column.Name = dcol.ColumnName;
            }
            foreach (DataRow row in table.Rows)
            {
                int index = dataGridView1.Rows.Add();
                DataGridViewRow vRow = dataGridView1.Rows[index];
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    vRow.Cells[i].Value = row[i];
                }
            }
        }

        //打印按钮    print button
        private void button2_Click(object sender, EventArgs e)
        {
            #region old
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow dr;
            //设置列表头
            foreach (DataGridViewColumn headerCell in dataGridView1.Columns)
            {
                dt.Columns.Add(headerCell.HeaderText);
            }
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = item.Cells[i].Value.ToString();

                }
                dt.Rows.Add(dr);
            }
          
            #endregion
            new PrintHelper().Print(dt, "中仓报表");
        }

        //预览按钮    
        private void button4_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow dr;
            //设置列表头
            foreach (DataGridViewColumn headerCell in dataGridView1.Columns)
            {
                dt.Columns.Add(headerCell.HeaderText);
            }
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = item.Cells[i].Value.ToString();

                }
                dt.Rows.Add(dr);
            }
            new PrintHelper().PrintPriview(dt, "中仓报表");
        }

        //导出按钮
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "导出Excel文件到";

            saveFileDialog.ShowDialog();
            try
            {
                Stream myStream;
                myStream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("gb2312"));
                //Sheet.RepeatingRows = new CellRangeAddress(0, 5, 0, 5);
                string str = "";

                //写标题  
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dataGridView1.Columns[i].HeaderText;
                }

                sw.WriteLine(str);
                //写内容 
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dataGridView1.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }
                        string cell = dataGridView1.Rows[j].Cells[k].Value.ToString();
                        cell = cell.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\r\n", "");
                        tempStr += cell;
                    }
                    sw.WriteLine(tempStr);
                }
                MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sw.Close();
                myStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //sw.Close();
                //myStream.Close();
            }
        }

        //日期选择
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void dateTimePicker1_DropDown(object sender, EventArgs e)
        {
            textBox3.Text = string.Empty;
        }

        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //制令的输入框
        private void comboBox11_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox11.Items.Clear();
            //鼠标指针顺序
            this.comboBox11.SelectionStart = this.comboBox11.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox11.DroppedDown = true;
            string s = comboBox11.Text.Trim();              //获取输入内容
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox11_TextUpdate", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();    //存放数据库查询结果
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["se_id"].ToString());
                }
                if (sList.Count != 0)
                {
                    this.comboBox11.Items.AddRange(sList.ToArray());
                    this.comboBox11.SelectionStart = this.comboBox11.Text.Length;
                }
                else
                {
                    this.comboBox11.Items.Add("");
                    this.comboBox11.SelectionStart = this.comboBox11.Text.Length;
                }

            }                 
        }

        //送料单位输入框
        private void comboBox10_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox10.Items.Clear();
            //鼠标指针顺序
            this.comboBox10.SelectionStart = this.comboBox10.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox10.DroppedDown = true;
            string s = comboBox10.Text.Trim();              //获取输入内容
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox10_TextUpdate", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();    //存放数据库查询结果
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                if (dt.Rows.Count == 0)
                {                   
                    Dictionary<string, string> pp = new Dictionary<string, string>();
                    pp.Add("s", s);
                    ret= SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox10_TextUpdate1", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(pp));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sList.Add(dt.Rows[i]["WC_NO"].ToString() + "   " + dt.Rows[i]["NAME_T"].ToString());
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sList.Add(dt.Rows[i]["VEND_NO"].ToString() + "   " + dt.Rows[i]["SHORTNM_S"].ToString());
                    }
                }
            }
            if (sList.Count != 0)
            {
                this.comboBox10.Items.AddRange(sList.ToArray());
                this.comboBox10.SelectionStart = this.comboBox10.Text.Length;
            }
            else
            {
                this.comboBox10.Items.Add("");
                this.comboBox10.SelectionStart = this.comboBox10.Text.Length;
            }
        }

        //领料类别
        private void comboBox7_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox7.Items.Clear();
            //鼠标指针顺序
            this.comboBox7.SelectionStart = this.comboBox7.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox7.DroppedDown = true;
            string s = comboBox7.Text.Trim();
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
             string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox7_TextUpdate", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();    //存放数据库查询结果
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["enum_value"].ToString());
                }
                if (sList.Count != 0)
                {
                    this.comboBox7.Items.AddRange(sList.ToArray());
                    this.comboBox7.SelectionStart = this.comboBox7.Text.Length;
                }
                else
                {
                    this.comboBox7.Items.Add("");
                    this.comboBox7.SelectionStart = this.comboBox7.Text.Length;
                }

            }

        }

        /// <summary>
        /// 当前制令对应的鞋型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//如果输入的是回车键
            {
                string whereID = string.Empty;
                string SE_ID = string.Empty;
                if (string.IsNullOrEmpty(textBox11.Text))
                {
                    return;
                }
                if (!string.IsNullOrEmpty(textBox11.Text))
                {
                    SE_ID = textBox11.Text.ToString();
                }

                if (dataTable.Rows.Count==0)
                {
                    whereID = "'" + SE_ID + "'";
                }
                else
                {
                    whereID += "'" + SE_ID + "'";
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        whereID += ",'" + dataTable.Rows[i][0] + "'";
                    }
                }
                Dictionary<string, string> p = new Dictionary<string, string>();
                p.Add("whereID", whereID);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "ShoeNoQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        comboBox1.Items.Add(dt.Rows[i]["SHOE_NO"].ToString());
                    }
                    if (dt.Rows.Count > 0)
                    {
                        comboBox1.Text = dt.Rows[0]["SHOE_NO"].ToString();
                    }
                    SetDataTable(SE_ID);
                    textBox11.Text = "";
                }
            }
        }

        private void SetDataTable(string seId)
        {
            bool isExits = false;
            for (int i=0;i<dataTable.Rows.Count;i++)
            {
                if (dataTable.Rows[i][0].Equals(seId))
                {
                    isExits = true;
                }
            }
            if (!isExits)
            { 
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = seId;
                dataTable.Rows.Add(dataRow);
                dataGridView2.DataSource = dataTable;
                dataGridView2.Update();
            }      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xulie = string.Empty;
            try
            {
                if (button1.Text == "取消打印")
                {
                    this.panel2.Visible = true;
                    button1.Text = "打 印";
                    return;
                }
                if (string.IsNullOrEmpty(textBox6.Text))
                {
                    MessageBox.Show("请填写发料数量");
                    return;
                }
                if (string.IsNullOrEmpty(comboBox2.Text))
                {
                    MessageBox.Show("请选择打印机");
                    return;
                }
                if (string.IsNullOrEmpty(textBox20.Text))
                {
                    MessageBox.Show("请填写分隔符@");
                    return;
                }
                if (string.IsNullOrEmpty(comboBox5.Text))
                {
                    MessageBox.Show("请填写外发单位");
                    return;
                }
                if (string.IsNullOrEmpty(comboBox6.Text))
                {
                    MessageBox.Show("请填写加工单位");
                    return;
                }
                if (string.IsNullOrEmpty(comboBox7.Text))
                {
                    MessageBox.Show("请填写类别");
                    return;
                }
                if (string.IsNullOrEmpty(comboBox8.Text))
                {
                    MessageBox.Show("请填写部件");
                    return;
                }
                if (!string.IsNullOrEmpty(comboBox8.Text))
                {
                    BAR_ID = Regex.Split(comboBox8.Text, "\\s+", RegexOptions.IgnoreCase)[0];
                }
                if (!string.IsNullOrEmpty(comboBox5.Text))
                {
                    OUT_UNIT = Regex.Split(comboBox5.Text, "\\s+", RegexOptions.IgnoreCase)[0];
                }
                if (!string.IsNullOrEmpty(comboBox6.Text))
                {
                    UNIT = Regex.Split(comboBox6.Text, "\\s+", RegexOptions.IgnoreCase)[0];
                }
                for (int bb = 0; bb < Convert.ToDecimal(textBox1.Text); bb++)
                {        
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "GetString", Program.client.UserToken, string.Empty);
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string RetData1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        if (string.IsNullOrEmpty(RetData1))
                        {
                            RetData1 = "1";
                        }
                        else
                        {
                            RetData1 = (int.Parse(RetData1) + 1).ToString();
                        }
                        string xuh = "";
                        string group = "";
                        string ret3 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "Group_method", Program.client.UserToken, string.Empty);
                        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret3)["IsSuccess"]))
                        {
                            string RetData3 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret3)["RetData"].ToString();
                            if (string.IsNullOrEmpty(RetData3))
                            {
                                MessageBox.Show("组别为空");
                                return;
                            }
                            else
                            {
                                group = RetData3;
                            }
                        }
                        else
                        {
                            MessageBox.Show("查找组别失败");
                            return;
                        }
                        string xuhRet = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "GetString1", Program.client.UserToken, string.Empty);
                        if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(xuhRet)["IsSuccess"]))
                        {
                            string RetData2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(xuhRet)["RetData"].ToString();
                            if (string.IsNullOrEmpty(RetData2))
                            {
                                xuh = DateTime.Now.ToString("yyyyMMdd") + "00001";
                            }
                            else
                            {
                                xuh =  DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(RetData2.Replace("" + DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("00000");
                            }
                            string se_id = "";
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (i == dataTable.Rows.Count - 1)
                                {
                                    se_id += dataTable.Rows[i][0];
                                }
                                else
                                {
                                    se_id += dataTable.Rows[i][0] + ",";
                                }
                            }

                            string ID = RetData1;
                            string waifa = OUT_UNIT;
                            string wk_id = se_id;
                            string tB2 = textBox2.Text.Trim();
                            string QTY2 = textBox6.Text.Trim();
                            string CREATEBY = textBox7.Text.Trim();
                            string songliao = UNIT;
                            string PROD_NO = comboBox1.Text.Trim();
                            string SHOE_NO = BAR_ID;
                            string TIME_ROTATION = textBox19.Text.Trim();
                            string AD_DATA = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                            string INSPECTOR = textBox16.Text.Trim();
                            string PACKING_BARCODE = group+xuh;
                            string UDF01 = comboBox7.Text.Trim();
                            string UDF03 = xuh;
                            tB2 = tB2.Replace("。",".");
                            Dictionary<string, Object> a = new Dictionary<string, object>();
                            a.Add("ID", ID);
                            a.Add("OUT_UNIT", waifa);
                            a.Add("wk_id", wk_id);
                            a.Add("BAR_ID", tB2);
                            a.Add("QTY2", QTY2);
                            a.Add("CREATEBY", CREATEBY);
                            a.Add("UNIT", songliao);
                            a.Add("PROD_NO", PROD_NO);
                            a.Add("SHOE_NO", SHOE_NO);
                            a.Add("TIME_ROTATION", TIME_ROTATION);
                            a.Add("AD_DATA", AD_DATA);
                            a.Add("INSPECTOR", INSPECTOR);
                            a.Add("PACKING_BARCODE", PACKING_BARCODE);
                            a.Add("UDF01", UDF01);
                            a.Add("UDF03", UDF03);

                            string[] zhiling = se_id.Split(',');
                            for (int i = 0; i < zhiling.Length; i++)
                            {
                                string MATERIAL_NO = "";
                                string MATERIAL_NAME = "";
                                string MATERIAL_SPECIFICATIONS = "";
                                string user = "";

                                Dictionary<string, Object> aa = new Dictionary<string, object>();
                                aa.Add("ID", ID);
                                aa.Add("PACKING_BARCODE", PACKING_BARCODE);
                                aa.Add("WK_ID", zhiling[i]);
                                aa.Add("QTY", QTY2);
                                aa.Add("UDF01", UDF01);
                                aa.Add("user", user);
                                aa.Add("MATERIAL_NO", MATERIAL_NO);
                                aa.Add("MATERIAL_NAME", MATERIAL_NAME);
                                aa.Add("MATERIAL_SPECIFICATIONS", MATERIAL_SPECIFICATIONS);
                                string sql1Ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "ExecuteNonQuery1", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(aa));
                                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(sql1Ret)["IsSuccess"]))
                                {
                                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(sql1Ret)["ErrMsg"].ToString());
                                }
                                else
                                {
                                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(sql1Ret)["ErrMsg"].ToString());
                                }
                            }

                            string sqLRet = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "ExecuteNonQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(a));
                            if (!Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(sqLRet)["IsSuccess"]))
                            {
                                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(sqLRet)["ErrMsg"].ToString());
                            }
                            else
                            {

                                if (string.IsNullOrEmpty(xulie))
                                {
                                    xulie = "'" + PACKING_BARCODE + "'";
                                }
                                else
                                {
                                    xulie += ",'" + PACKING_BARCODE + "'";
                                }
                            }
                        }
                    }
                }
                LoadReportData(xulie);
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message); 
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count > 0 && string.IsNullOrEmpty(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[0].Value.ToString()))
            {
                int index = dataGridView2.CurrentRow.Index;
                DataRowView drv = dataGridView2.Rows[index].DataBoundItem as DataRowView;
                drv.Delete();
            }
        }



        //报表Test功能
        //制令的输入框
        private void comboBox14_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox14.Items.Clear();
            //鼠标指针顺序
            this.comboBox14.SelectionStart = this.comboBox14.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox14.DroppedDown = true;
            string s = comboBox14.Text.Trim();              //获取输入内容
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox11_TextUpdate", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();    //存放数据库查询结果
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["se_id"].ToString());
                }
                if (sList.Count != 0)
                {
                    this.comboBox14.Items.AddRange(sList.ToArray());
                    this.comboBox14.SelectionStart = this.comboBox14.Text.Length;
                }
                else
                {
                    this.comboBox14.Items.Add("");
                    this.comboBox14.SelectionStart = this.comboBox14.Text.Length;
                }

            }
        }

        //从这里这个输入框，送料单位输入框，判断输入的东西是在加工单位还是外发单位
        private void comboBox13_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox13.Items.Clear();
            //鼠标指针顺序
            this.comboBox13.SelectionStart = this.comboBox13.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox13.DroppedDown = true;
            string s = comboBox13.Text.Trim();              //获取输入内容
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox10_TextUpdate", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            List<string> sList = new List<string>();    //存放数据库查询结果
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["VEND_NO"].ToString());
                }            
            }
            else
            { 
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox10_TextUpdate1", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sList.Add(dt.Rows[i]["DEPARTMENT_CODE"].ToString());
                    }
                }
                else
                {
                    //获取当前仓库信息
                    string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "Getwarehouse", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
                    {
                        RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                        dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sList.Add(dt.Rows[i]["ManagementArea_no"].ToString());
                        }
                    }
                }

            }       

            if (sList.Count != 0)
            {
                this.comboBox13.Items.AddRange(sList.ToArray());
                this.comboBox13.SelectionStart = this.comboBox13.Text.Length;
            }
            else
            {
                this.comboBox13.Items.Add("");
                this.comboBox13.SelectionStart = this.comboBox13.Text.Length;
            }
        }

        //到那里输入框
        private void comboBox15_TextUpdate(object sender, EventArgs e)
        {
            //查询前先清除
            this.comboBox15.Items.Clear();
            //鼠标指针顺序
            this.comboBox15.SelectionStart = this.comboBox15.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动出现下拉列表
            this.comboBox15.DroppedDown = true;
            string s = comboBox15.Text.Trim();              //获取输入内容         
            List<string> sList = new List<string>();    //存放数据库查询结果
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("s", s);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox10_TextUpdate", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sList.Add(dt.Rows[i]["VEND_NO"].ToString());
                }
            }
            else
            {
                string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "comboBox10_TextUpdate1", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                {
                    RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                    dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sList.Add(dt.Rows[i]["DEPARTMENT_CODE"].ToString());
                    }
                }
                else
                {
                    //获取当前仓库信息
                    string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "Getwarehouse", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
                    {
                        RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                        dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(RetData);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sList.Add(dt.Rows[i]["ManagementArea_no"].ToString());
                        }
                    }
                }

            }

            if (sList.Count != 0)
            {
                this.comboBox15.Items.AddRange(sList.ToArray());
                this.comboBox15.SelectionStart = this.comboBox15.Text.Length;
            }
            else
            {
                this.comboBox15.Items.Add("");
                this.comboBox15.SelectionStart = this.comboBox15.Text.Length;
            }
        }

        //扫描日期选择
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            textBox4.Text = dateTimePicker3.Value.ToString("yyyy-MM-dd");
        }

        private void dateTimePicker3_DropDown(object sender, EventArgs e)
        {
            textBox4.Text = string.Empty;
        }

        private void dateTimePicker3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //交货日期选择
        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            textBox9.Text = dateTimePicker4.Value.ToString("yyyy-MM-dd");
        }

        private void dateTimePicker4_DropDown(object sender, EventArgs e)
        {
            textBox9.Text = string.Empty;
        }

        private void dateTimePicker4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //查询按钮
        private void button9_Click(object sender, EventArgs e)
        {
            label20.Text = string.Empty;
            label11.Text = string.Empty;
            label9.Text = string.Empty;
            if (dataGridView3.Rows.Count > 0)
            {
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();
            }
            if (!string.IsNullOrEmpty(comboBox14.Text))
            {
                string where = string.Empty;
                if (!string.IsNullOrEmpty(comboBox14.Text))
                {
                    where += " and A.WK_ID like '%" + comboBox14.Text + "%'";
                }
                if (!string.IsNullOrEmpty(textBox8.Text))
                {
                    where += " and A.PROD_NO= '" + textBox8.Text + "'";
                }

                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    where += "and to_char(A.CREATEDATE,'YYYY-MM-DD') >= '"+ textBox4.Text+ "'";
                }
                if (!string.IsNullOrEmpty(textBox12.Text))
                {
                    where += "and to_char(A.CREATEDATE,'YYYY-MM-DD') <= '" + textBox12.Text + "'";
                }
                if(!string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrEmpty(textBox12.Text))
                {
                    where += " and A.CREATEDATE between TO_DATE('" + textBox4.Text + "','YYYY-MM-DD') and TO_DATE('" + textBox12.Text + "','YYYY-MM-DD')";
                }

                if (!string.IsNullOrEmpty(textBox9.Text))
                {
                    where += "and B.AD_DATA >= '" + textBox9.Text + "'";
                }
                if (!string.IsNullOrEmpty(textBox10.Text))
                {
                    where += "and B.AD_DATA <= '" + textBox10.Text + "'";
                }
                if(!string.IsNullOrEmpty(textBox9.Text) && !string.IsNullOrEmpty(textBox12.Text))
                {
                    where += " and B.AD_DATA between TO_DATE('" + textBox9.Text + "','YYYY-MM-DD') and TO_DATE('" + textBox10.Text + "','YYYY-MM-DD')";
                }
                if (!string.IsNullOrEmpty(comboBox13.Text))
                {
                    string UNIT = string.Empty;
                    if (!string.IsNullOrEmpty(comboBox13.Text))
                    {
                        UNIT = Regex.Split(comboBox13.Text, "\\s+", RegexOptions.IgnoreCase)[0];
                    }
                    where += " and (A.UNIT like'" + UNIT + "%' or A.OUT_UNIT like'" + UNIT + "%' or A.UDF02 like'"+UNIT+"%' ) ";
                }
                if (!string.IsNullOrEmpty(comboBox15.Text))
                {
                    string UDF02 = string.Empty;
                    if (!string.IsNullOrEmpty(comboBox15.Text))
                    {
                        UDF02 = Regex.Split(comboBox15.Text, "\\s+", RegexOptions.IgnoreCase)[0];
                    }
                    where += " and (A.UNIT like'" + UDF02 + "%' or A.OUT_UNIT like'" + UDF02 + "%' or A.UDF02 like'" + UDF02 + "%' ) ";
                }
                if (!string.IsNullOrEmpty(comboBox12.Text) && comboBox12.Text != "全部")
                {
                    where += " and A.OPERATION_TYPE like '" + comboBox12.Text.ToString().Substring(0, 1) + "%'";
                }

                Dictionary<string, string> p = new Dictionary<string, string>();
                p.Add("where", where);
                string BAR_ID = string.Empty;
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "TestReportingOrderQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (string.IsNullOrEmpty(comboBox13.Text))
                            {
                                if (string.IsNullOrEmpty(BAR_ID))
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID = "sum(decode(A.SHOE_NO, '" + dt.Rows[i]["SHOE_NO"].ToString() + "', A.QTY, 0)) as " + str + "";
                                }
                                else
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID += ",sum(decode(A.SHOE_NO, '" + dt.Rows[i]["SHOE_NO"].ToString() + "', A.QTY, 0)) as " + str + "";
                                }
                            }
                            else
                            {
                                string value = dt.Rows[i]["SHOE_NO"].ToString();
                                if (string.IsNullOrEmpty(BAR_ID))
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID = "sum(decode(A.SHOE_NO, '" + value + "', A.QTY, 0)) as " + str + "";
                                }
                                else
                                {
                                    string str = "" + "\"" + dt.Rows[i]["SHOE_NO"].ToString() + "\"";
                                    BAR_ID += ",sum(decode(A.SHOE_NO, '" + value + "', A.QTY, 0)) as " + str + "";
                                }
                            }

                        }
                    }
                }



                //BAR_ID = "123";
                //string str1 = ""+ "\"" + BAR_ID + "\"";
                if (!string.IsNullOrEmpty(BAR_ID))
                {

                    string size = "\"SIZE\"";
 //                 string sum = "\"订单数量\"";
                    string wkid = "\"制令\"";
                    string art = "\"ART_NO\"";
                    string xiex = "\"鞋型\"";
                    string unit = "\"来自\"";
                    string there = "\"去向\"";
                    string operation_type= "\"操作类型\"";
                    string scantime = "\"扫描日期\"";
                    string Deliverytime = "\"需交货日期\"";
                    Dictionary<string, string> a = new Dictionary<string, string>();
                    a.Add("size", size);
  //                a.Add("sum", sum);
                    a.Add("wkid", wkid);
                    a.Add("art", art);
                    a.Add("xiex", xiex);
                    a.Add("BAR_ID", BAR_ID);
                    a.Add("unit", unit);
                    a.Add("there", there);
                    a.Add("operation_type", operation_type);
                    a.Add("scantime", scantime);
                    a.Add("Deliverytime", Deliverytime);
                    a.Add("where", where);


                    string ret1 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "TestReportingQuery", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(a));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["IsSuccess"]))
                    {
                        //string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                        //DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json); //json有返回值了

                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret1)["RetData"].ToString();
                        DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        TestBuidGrid(dt);
                        DataGridViewTextBoxColumn acCode1 = new DataGridViewTextBoxColumn();
                        acCode1.Name = "配套数";
                        acCode1.DataPropertyName = "配套数";
                        acCode1.HeaderText = "配套数";
                        dataGridView3.Columns.Add(acCode1);
                        //获取每行的合计以及最小值
                        for (int i = 0; i < dataGridView3.Rows.Count; i++)
                        {
                            string qty2 = string.Empty;
                            for (int j = 9; j < dataGridView3.Columns.Count; j++)
                            {
                                if (dataGridView3.Rows[i].Cells[j].Value != null && !string.IsNullOrEmpty(dataGridView3.Rows[i].Cells[j].Value.ToString()))
                                {
                                    if (string.IsNullOrEmpty(qty2))
                                    {
                                        qty2 = dataGridView3.Rows[i].Cells[j].Value.ToString();
                                    }
                                    else
                                    {
                                        qty2 += "," + dataGridView3.Rows[i].Cells[j].Value.ToString();
                                    }

                                }
                            }

                            if (!string.IsNullOrEmpty(qty2))
                            {
                                string[] str = qty2.Split(','); //将输入分开放入数组中
                                long min = Convert.ToInt64(str[0]);
                                //double min = Convert.ToDouble(str[0]);
                                //定义了最大值,最小值,这里解释一下,long和int64指的是一个长度
                                //min 必须在foreach循环外定义和初始化！！！
                                foreach (string aa in str)
                                {
                                    if (Convert.ToInt64(aa) < min) { min = Convert.ToInt64(aa); }
                                }
                                dataGridView3.Rows[i].Cells[dataGridView3.Columns.Count - 1].Value = min;
                            }
                        }
                        TestGetZhiling(dt);
                       // TestGetSum();
                        TestGetCount();
                    }

                }
                for (int i = 0; i < dataGridView3.Columns.Count; i++)
                {
                    dataGridView3.Columns[i].Frozen = false;
                }
            }
            else
            {
                MessageBox.Show("请选择制令");
            }
        }

        //打印按钮
        private void button8_Click(object sender, EventArgs e)
        {
            #region old
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow dr;
            //设置列表头
            foreach (DataGridViewColumn headerCell in dataGridView3.Columns)
            {
                dt.Columns.Add(headerCell.HeaderText);
            }
            foreach (DataGridViewRow item in dataGridView3.Rows)
            {
                dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = item.Cells[i].Value.ToString();

                }
                dt.Rows.Add(dr);
            }

            #endregion
            new PrintHelper().Print(dt, "中仓报表");
        }

        //预览按钮
        private void button6_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow dr;
            //设置列表头
            foreach (DataGridViewColumn headerCell in dataGridView3.Columns)
            {
                dt.Columns.Add(headerCell.HeaderText);
            }
            foreach (DataGridViewRow item in dataGridView3.Rows)
            {
                dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = item.Cells[i].Value.ToString();

                }
                dt.Rows.Add(dr);
            }
            new PrintHelper().PrintPriview(dt, "中仓报表");
        }

        //导出按钮
        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "导出Excel文件到";

            saveFileDialog.ShowDialog();
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("gb2312"));
            try
            {  
                //Sheet.RepeatingRows = new CellRangeAddress(0, 5, 0, 5);
                string str = "";

                //写标题  
                for (int i = 0; i < dataGridView3.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dataGridView3.Columns[i].HeaderText;
                }

                sw.WriteLine(str);
                //写内容 
                for (int j = 0; j < dataGridView3.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dataGridView3.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }
                        if (dataGridView3.Rows[j].Cells[k].Value != null)
                        {
                            string cell = dataGridView3.Rows[j].Cells[k].Value.ToString();
                            cell = cell.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\r\n", "");
                            tempStr += cell;
                        }                                  
                    }
                    sw.WriteLine(tempStr);
                }
                MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sw.Close();
                myStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());               
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
        }

        //表格输出方法
        private void TestBuidGrid(DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                DataColumn dcol = table.Columns[i];
                DataGridViewColumn column = new DataGridViewColumn(new DataGridViewTextBoxCell());
                dataGridView3.Columns.Add(column);
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.HeaderText = dcol.ColumnName;
                column.Name = dcol.ColumnName;
            }
            foreach (DataRow row in table.Rows)
            {
                int index = dataGridView3.Rows.Add();
                DataGridViewRow vRow = dataGridView3.Rows[index];
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    vRow.Cells[i].Value = row[i];
                }
            }
        }

        //增加多一个合计的行
        public void TestGetCount()
        {
            int countIndex = dataGridView3.Rows.Add();
            DataGridViewRow countRow = dataGridView3.Rows[countIndex];
            countRow.Cells[0].Value = "Total:";
            for (int i = 0; i < dataGridView3.Columns.Count; i++)
            {
                if (i < 9)
                {
                    continue;
                }
                else if (i != 0)
                {
                    double qty = 0;
                    for (int j = 0; j < dataGridView3.Rows.Count; j++)
                    {

                        if (dataGridView3.Rows[j].Cells[i].Value != null && !string.IsNullOrEmpty(dataGridView3.Rows[j].Cells[i].Value.ToString()))
                        {
                            qty += Convert.ToDouble(dataGridView3.Rows[j].Cells[i].Value.ToString());
                        }
                    }
                    countRow.Cells[i].Value = qty;
                }

            }
        }

        //获取制令或鞋型
        public void TestGetZhiling(DataTable table)
        {
            if (table.Rows.Count > 0)
            {
                zhiling = string.Empty;
                string xiex = string.Empty;
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(zhiling))
                        {
                            zhiling = table.Rows[i]["制令"].ToString();
                        }
                        else
                        {
                            zhiling =  table.Rows[0]["制令"].ToString();
                        }
                        if (string.IsNullOrEmpty(xiex))
                        {
                            xiex = table.Rows[i]["ART_NO"].ToString();
                        }
                        else
                        {
                            xiex =  table.Rows[0]["ART_NO"].ToString();
                        }
                    }

                }

                Dictionary<string, string> a = new Dictionary<string, string>();
                a.Add("zhiling", zhiling);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.BarcodeprintingServer", "GetZhiling", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(a));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string RetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    label9.Visible = true;
                    label9.Text = RetData;
                }
                label20.Visible = true;
                label20.Text = DelArraySamezl(zhiling);
                label11.Visible = true;
                label11.Text = DelArraySame(xiex);
            }
        }

        private void dateTimePicker5_DropDown(object sender, EventArgs e)
        {
            textBox10.Text = string.Empty;

        }

        private void dateTimePicker5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            textBox10.Text = dateTimePicker5.Value.ToString("yyyy-MM-dd");

        }

        private void dateTimePicker6_ValueChanged(object sender, EventArgs e)
        {
            textBox12.Text = dateTimePicker6.Value.ToString("yyyy-MM-dd");

        }

        private void dateTimePicker6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dateTimePicker6_DropDown(object sender, EventArgs e)
        {
            textBox12.Text = string.Empty;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <='9')
            {
                e.Handled = false;
            }

            if (e.KeyChar == '.' || e.KeyChar == '。'|| e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.K || e.KeyChar == 'k')
            {
                e.Handled = false;
            }
           
           
        }
    }
}

