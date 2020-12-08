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

namespace F_EAP_MachineLink_DataSet
{
    public partial class F_EAP_Data_Set : MaterialForm
    {
        IList<object[]> data = null;
        private ExcelProcessor _currentExcelProcessor = null;
        string errs = "";

        public F_EAP_Data_Set()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void F_EAP_Data_Set_Load(object sender, EventArgs e)
        {
            GetComboBoxUI();
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView2.AutoGenerateColumns = false;
        }

        private void butQuery_Click(object sender, EventArgs e)
        {
            string factory = comboFactory.SelectedValue.ToString();
            string machine_type = comboType.SelectedValue.ToString();
            string machine_id = textMachineID.Text.Trim();
            string whether_Link = comboWhetherLink.SelectedValue.ToString();
            string d_dept = textDept.Text;

            dataGridView2.DataSource = null;

            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vFactory", factory);
            p.Add("vType", machine_type);
            p.Add("vMachineID", machine_id);
            p.Add("vWhetherLink", whether_Link);
            p.Add("vDept", d_dept);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "QueryMachineList", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                if (dt.Rows.Count > 0)
                {
                    dataGridView2.DataSource = dt;
                    dataGridView2.Rows[0].Selected = false;
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            F_EAP_Contrast_Add frm = new F_EAP_Contrast_Add();
            frm.ShowDialog();
        }
        
        private void GetComboBoxUI()
        {
            List<TypeEntry> typeEntries = new List<TypeEntry> { };
            List<FactoryEntry> factoryEntries = new List<FactoryEntry> { };
            List<YesOrNoEntry> yesOrNoEntry = new List<YesOrNoEntry> { };

            typeEntries.Add(new TypeEntry() { Code = "ALL", Name = "ALL|所有" });
            factoryEntries.Add(new FactoryEntry() { Code = "ALL", Name = "ALL|所有" });

            yesOrNoEntry.Add(new YesOrNoEntry() { Code = "ALL", Name = "ALL|所有" });
            yesOrNoEntry.Add(new YesOrNoEntry() { Code = "Y", Name = "是" });
            yesOrNoEntry.Add(new YesOrNoEntry() { Code = "N", Name = "否" });

            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "GetFactory", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    factoryEntries.Add(new FactoryEntry() { Code = dtJson.Rows[i]["CODE"].ToString(), Name = dtJson.Rows[i]["NAME"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

            string ret2 = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "GetAllMachineType", Program.client.UserToken, string.Empty);
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                for (int i = 0; i < dtJson.Rows.Count; i++)
                {
                    typeEntries.Add(new TypeEntry() { Code = dtJson.Rows[i]["CODE"].ToString(), Name = dtJson.Rows[i]["NAME"].ToString() });
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret2)["ErrMsg"].ToString());
            }

            this.cbFactory.DataSource = factoryEntries;
            this.cbFactory.DisplayMember = "Name";
            this.cbFactory.ValueMember = "Code";

            this.comboFactory.DataSource = factoryEntries;
            this.comboFactory.DisplayMember = "Name";
            this.comboFactory.ValueMember = "Code";

            this.cbType.DataSource = typeEntries;
            this.cbType.DisplayMember = "Name";
            this.cbType.ValueMember = "Code";

            this.comboType.DataSource = typeEntries;
            this.comboType.DisplayMember = "Name";
            this.comboType.ValueMember = "Code";

            this.dataGridViewTextBoxColumn4.DataSource = typeEntries;
            this.dataGridViewTextBoxColumn4.DisplayMember = "Name";
            this.dataGridViewTextBoxColumn4.ValueMember = "Code";

            this.comboWhetherLink.DataSource = yesOrNoEntry;
            this.comboWhetherLink.DisplayMember = "Name";
            this.comboWhetherLink.ValueMember = "Code";

            this.dataGridViewTextBoxColumn2.DataSource = yesOrNoEntry;
            this.dataGridViewTextBoxColumn2.DisplayMember = "Name";
            this.dataGridViewTextBoxColumn2.ValueMember = "Code";
        }
        
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index > -1 && dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("确认要删除此数据吗", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int index = dataGridView1.CurrentRow.Index;
                    string machine_id = dataGridView1.Rows[index].Cells[0].Value == null ? "" : dataGridView1.Rows[index].Cells[0].Value.ToString();
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    p.Add("vMachineID", machine_id);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "DelMachineContrastById", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        SJeMES_Control_Library.MessageHelper.ShowOK(this, "删除成功！");
                        this.btnSelect.PerformClick();//重新查询 
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请选择要删除的项！");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null && dataGridView2.CurrentRow.Index > -1 && dataGridView2.SelectedRows.Count > 0)
            {
                int index = dataGridView2.CurrentRow.Index;
                string machine_id = dataGridView2.Rows[index].Cells[0].Value == null ? "" : dataGridView2.Rows[index].Cells[0].Value.ToString();
                string machine_name = dataGridView2.Rows[index].Cells[1].Value == null ? "" : dataGridView2.Rows[index].Cells[1].Value.ToString();
                string machine_dept = dataGridView2.Rows[index].Cells[3].Value == null ? "" : dataGridView2.Rows[index].Cells[3].Value.ToString();
                string whether_link = dataGridView2.Rows[index].Cells[4].Value == null ? "" : dataGridView2.Rows[index].Cells[4].Value.ToString();

                F_EAP_Edit_LinkStatus frm = new F_EAP_Edit_LinkStatus(machine_id, machine_name, machine_dept, whether_link);
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    this.butQuery.PerformClick();//重新查询 
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "请选择要修改的项！");
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string factory = cbFactory.SelectedValue.ToString();
            string machine_type = cbType.SelectedValue.ToString();
            string machine_id = textID.Text.Trim();
            string machine_no = textNO.Text.Trim();

            dataGridView1.DataSource = null;
            
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vFactory", factory);
            p.Add("vType", machine_type);
            p.Add("vMachineID", machine_id);
            p.Add("vMachineNO", machine_no);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "GetMachineContrastList", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.Rows[0].Selected = false;
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, "未查到相关数据！");
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void UpLoad(DataTable tab)
        {
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("data", tab);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_EAPAPI", "KZ_EAPAPI.Controllers.MachineLinkBasicServer", "updateEapRftAndRate", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                //string msg = SJeMES_Framework.Common.UIHelper.UImsg("导入成功！", Program.client, "", Program.client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "导入成功！");
            }
            else
            {
                //string msg = SJeMES_Framework.Common.UIHelper.UImsg("导入失败！", Program.client, "", Program.client.Language);
                string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "导入失败！" + exceptionMsg);
            }
        }

        private void butImport_Click(object sender, EventArgs e)
        {
            //btnFolder.Enabled = false;
            butFile.Enabled = false;
            butImport.Enabled = false;
            if (this.dataGridView_data.Rows.Count >= 2)
            {
                try
                {
                    updateEapRftAndRate();
                    butFile.Enabled = true;
                }
                catch (Exception ex)
                {
                    butFile.Enabled = true;
                    butImport.Enabled = true;
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {
                butFile.Enabled = true;
                butImport.Enabled = true;
                //string msg = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.client, "", Program.client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, "没有数据");
            }
        }

        private void updateEapRftAndRate()
        {
            if (errs != "")
            {
                MessageBox.Show(errs, "错误！");
                return;
            }
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
                    dr[j] = data[i][j].ToString().Trim();
                }
                tab.Rows.Add(dr);
            }
            UpLoad(tab);
        }

        private void butFile_Click(object sender, EventArgs e)
        {
            errs = "";
            this.dataGridView_data.AutoGenerateColumns = false;
            if (dataGridView_data != null)
            {
                this.dataGridView_data.Columns.Clear();
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "EXCEL|*.xls*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (dataGridView_path != null)
                {
                    data = new List<object[]>();
                    this.dataGridView_path.Rows.Clear();
                }
                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        this.dataGridView_path.Rows.Add(filename);
                        dataGridView_path.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        this.GetExcelData(Path.GetFullPath(filename));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message);
                    }
                }
                dataGridView_data.AllowUserToAddRows = false;
                if (data != null && data.Count > 0)
                {
                    int colNum = data[0].Length;
                    for (int i = 0; i < colNum; i++)
                    {
                        DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
                        acCode.Name = data[0][i].ToString();
                        acCode.HeaderText = data[0][i].ToString();
                        dataGridView_data.Columns.Add(acCode);
                    }
                    for (int i = 1; i < data.Count; i++)
                    {
                        dataGridView_data.Rows.Add(data[i]);
                        dataGridView_data.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        try
                        {
                            data[i][2] = Convert.ToDateTime(data[i][2].ToString()).ToString("yyyyMMdd");
                        }
                        catch (Exception ex)
                        {
                            errs += "row:" + i + ",cloumn3:" + ex.Message + "\n";
                        }
                        for (int j = 3; j < colNum; j++)
                        {
                            if (data[i][j] != null && data[i][j].ToString().Trim() != "")
                            {
                                try
                                {
                                    decimal rate = decimal.Parse(data[i][j].ToString().Trim());
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
                butImport.Enabled = true;
            }
            if (errs != "")
            {
                MessageBox.Show(errs,"错误！");
            }
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

        public class TypeEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        public class FactoryEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }

        public class YesOrNoEntry
        {
            public string Code { get; set; }

            public string Name { get; set; }
        }
    }
}
