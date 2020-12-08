﻿using AutocompleteMenuNS;
using DayTargets;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayTargets
{
    public partial class DayTargetsForm : MaterialForm
    {
        public Boolean isTitle = false;
        IList<object[]> data = null;
        private ExcelProcessor _currentExcelProcessor = null;
        public static string dateFormat = "yyyy/MM/dd";
        public DayTargetsForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
          
        }

        private void DayTargetsForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadSeDept();
                GetComboBoxUI();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
          
            this.dataGridView2.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
            this.dataGridView3.DataError += delegate (object obj, DataGridViewDataErrorEventArgs eve) { };
        }

        private void butFile_Click(object sender, EventArgs e)
        {
            isTitle = true;
            string errs = "";
            this.dataGridView2.AutoGenerateColumns = false;
            if (dataGridView2 != null)
            {
                this.dataGridView2.Columns.Clear();
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
                       MessageBox.Show(this, ex.Message);
                    }
                }
                dataGridView2.AllowUserToAddRows = false;
                if (data != null && data.Count > 0)
                {
                    int colNum = data[0].Length;
                    for (int i = 0; i < colNum; i++)
                    {
                        DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
                        if (i > 0 && i < colNum)
                        {
                            try
                            {
                                data[0][i] = Convert.ToDateTime(data[0][i].ToString()).ToString(dateFormat);
                                acCode.Width = 68;
                            }
                            catch(Exception ex)
                            {
                                errs += "column:" + (i+1) + "," + ex.Message + "\n";
                            }
                        }
                        acCode.Name = data[0][i].ToString();
                        acCode.HeaderText = data[0][i].ToString();
                        dataGridView2.Columns.Add(acCode);
                    }
                    dataGridView2.Rows.Add(data[1]);
                    for (int i = 2; i < data.Count; i++)
                    {
                        dataGridView2.Rows.Add(data[i]);
                        dataGridView2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        //try
                        //{
                        //    if (!ValiDept(data[i][0].ToString().Trim().ToUpper()))
                        //    {
                        //        errs += "row:" + i + ",error department" + "\n";
                        //    }
                        //    else
                        //    {
                        //        data[i][0] = data[i][0].ToString().Trim().ToUpper();
                        //    }
                        //}
                        //catch(Exception)
                        //{
                        //    errs += "row:" + i + ",error department" + "\n";
                        //}

                        for (int j = 1; j < colNum; j++)
                        {
                            if (data[i][j] != null && data[i][j].ToString().Trim() != "")
                            {
                                try
                                {
                                    int qty = int.Parse(data[i][j].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    errs += "row:" + i + ",cloumn:" + (j + 1) + "," + ex.Message + "\n";
                                }
                            }
                            else
                            {
                                data[i][j] = "";
                            }
                        }
                    }
                }
            }
            if (errs != "")
            {
                MessageBox.Show(errs);
            }
        }

        //导入
        private void butImport_Click(object sender, EventArgs e)
        {
            isTitle = true;
            //btnFolder.Enabled = false;
            butFile.Enabled = false;
            butImport.Enabled = false;
            if (this.dataGridView2.Rows.Count >= 2)
            {
                try
                {
                    update_db();
                }
                catch (Exception ex)
                {
                    //butFile.Enabled = true;
                    //butImport.Enabled = true;
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {
                //butFile.Enabled = true;
                //butImport.Enabled = true;
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client,"", Program.client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
            butFile.Enabled = true;
            butImport.Enabled = true;
        }

        private void update_db()
        {
            string errs = "";
            DataTable tab = new DataTable();
            object[] cols = data[0];
            for (int i = 0; i < cols.Length; i++)
            {
                tab.Columns.Add(cols[i].ToString());
            }
            for (int i = 2; i < data.Count; i++)
            {
                DataRow dr = tab.NewRow();
                try
                {
                    if (!ValiDept(data[i][0].ToString().Trim().ToUpper()))
                    {
                        errs += "row:" + i + ",error department" + "\n";
                    }
                    else
                    {
                        dr[0] = data[i][0].ToString().Trim().ToUpper();
                    }
                }
                catch (Exception)
                {
                    errs += "row:" + i + ",error department" + "\n";
                }
                for (int j = 1; j < cols.Length; j++)
                {
                    if (data[i][j] != null && data[i][j].ToString().Trim() != "")
                    {
                        try
                        {
                            int qty = int.Parse(data[i][j].ToString().Trim());
                            dr[j] = data[i][j].ToString().Trim();
                        }
                        catch (Exception ex)
                        {
                            errs += "row:" + i + ",cloumn:" + (j + 1) + "," + ex.Message + "\n";
                        }
                    }
                    else
                    {
                        dr[j] = "";
                    }
                }
                tab.Rows.Add(dr);
            }
            if (errs == "")
            {
                try
                {
                    UpLoad(tab);
                }
                catch (Exception ex)
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {
                MessageBox.Show(errs);
            }
        }

        private void butquery_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView3.AutoGenerateColumns = false;
                this.dataGridView3.DataSource = null;
                QueryTargets();
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }

        private void UpLoad(DataTable tab)
        {
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("data", tab);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayTargetsServer", "UpLoad",
                            Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("导入成功！", Program.client, "",Program.client.Language);
                SJeMES_Control_Library.MessageHelper.ShowSuccess(this, msg);
            }
            else
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("导入失败！", Program.client,"", Program.client.Language);
                string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
            }
        }

        private bool ValiDept(string d_dept)
        {
            bool isOk = false;
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("vDDept", d_dept);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayTargetsServer", "ValiDept",
                            Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                isOk = bool.Parse(json);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            return isOk;
        }

        private void QueryTargets()
        {
            string d_dept = textBox1.Text.ToString().Trim();
            string work_day = dateTimePicker1.Text.ToString();
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vDDept", d_dept);
            p.Add("vWorkDay", work_day);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayTargetsServer", "QueryTargets", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                dataGridView3.Columns["ColQty"].DefaultCellStyle.Format = "0";
                dataGridView3.DataSource = dt.DefaultView;
                dataGridView3.Update();
                dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
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
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("err-00003", Program.client, "",Program.client.Language);
                string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
            }
            return dt;
        }

        //加载部门列表
        private void LoadSeDept()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new System.Drawing.Size(350, 350);
            var columnWidth = new int[] { 50, 250 };
            DataTable dt = GetAllDepts();
            int n = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt.Rows[i]["DEPARTMENT_CODE"].ToString() + " " + dt.Rows[i]["DEPARTMENT_NAME"].ToString() }, dt.Rows[i]["DEPARTMENT_CODE"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridView3.CurrentRow.Index;
            if (index > -1 && dataGridView3.Rows[index].Cells[0].Value != null)
            {
                string dept = dataGridView3.Rows[index].Cells[0].Value == null ? "" : dataGridView3.Rows[index].Cells[0].Value.ToString();
                string workDate = dataGridView3.Rows[index].Cells[1].Value == null ? "" : dataGridView3.Rows[index].Cells[1].Value.ToString();
                int qty = dataGridView3.Rows[index].Cells[2].Value == null ? 0 : Convert.ToInt32(decimal.Parse(dataGridView3.Rows[index].Cells[2].Value.ToString()));
                string workQty = qty.ToString();
                string note = dataGridView3.Rows[index].Cells[6].Value == null ? "" : dataGridView3.Rows[index].Cells[6].Value.ToString();
                DayTargetModifyForm frm = new DayTargetModifyForm(dept, workDate, workQty, note);
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    this.butquery.PerformClick();//重新查询 
                }
            }
        }

        private void GetComboBoxUI()
        {
            List<RoutEntry> routEntries = new List<RoutEntry> { };
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("enmu_type", "routEntries");

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ComboBoxUIServer", "GetComboBoxUI", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    routEntries.Add(new RoutEntry() { Code = dtJson.Rows[i]["ENUM_CODE"].ToString(), Name = dtJson.Rows[i]["ENUM_VALUE"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

            this.ColRoutNo.DataSource = routEntries;
            this.ColRoutNo.DisplayMember = "Name";
            this.ColRoutNo.ValueMember = "Code";
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
                MessageBox.Show(this, ex.Message);
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

        public class RoutEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }
    }
}
