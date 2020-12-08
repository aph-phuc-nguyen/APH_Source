using MaterialSkin.Controls;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_MMS_Outsourcing_TrackInfo
{
    public partial class F_MMS_Outsourcing_TrackInfo_Insert : MaterialForm
    {
        public F_MMS_Outsourcing_TrackInfo_Insert()
        {
            InitializeComponent();
        }

        public F_MMS_Outsourcing_TrackInfo_Insert(string seId, string partName, string barId)
        {
            InitializeComponent();
            textBox11.Text=seId;
            textBox3.Text = partName;
            textSize.Text = barId;
        }


        private void dataGridView5_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbmStatus.Text)||string.IsNullOrEmpty(textBox10.Text)||string.IsNullOrEmpty(textBox11.Text) || string.IsNullOrEmpty(textBox3.Text)|| string.IsNullOrEmpty(textSize.Text))
            {
                MessageBox.Show("*号为必输资料，请输入");
                return;
            }
            try
            {
                GetCode003MData();
                dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            }
            catch (Exception ex)
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
            }
        }


        private void GetCode003MData()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("vStatus", cbmStatus.Text.ToString().Split('|')[0]);
            p.Add("vSeId", textBox11.Text.ToString());
            p.Add("vfrom", textBox10.Text.ToString());
            p.Add("vto", textBox1.Text.ToString());
            p.Add("vPartName", textBox3.Text.ToString());
            p.Add("vSize", textSize.Text.ToString());
            p.Add("vOperation", cbmStatus.Text.Split('|')[0]);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetCode003MData", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
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

        private void F_SFC_Outsourcing_Update_TrackInfo_Load(object sender, EventArgs e)
        {
            this.dataGridView3.AutoGenerateColumns = false;
            this.dataGridView2.AutoGenerateColumns = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dataGridView3.CurrentRow != null && dataGridView3.CurrentRow.Index > -1)
            {
                int index = dataGridView3.CurrentRow.Index;
                string curQty = Interaction.InputBox("请输入正确的收/发料数量", "收/发数量","收/发数量", -1, -1);
                string memo = "";
                if(!string.IsNullOrEmpty(curQty))
                {
                    int result = -1;
                    if (!int.TryParse(curQty,out result))
                    {
                        return;
                    }
                    if (int.Parse(curQty)>int.Parse(dataGridView3.Rows[index].Cells["BarCodeQty"].Value.ToString()))
                    {
                        return;
                    }
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    string id = dataGridView3.Rows[index].Cells["id"].Value == null ? "" : dataGridView3.Rows[index].Cells["id"].Value.ToString();
                    string oldQty = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn4"].Value == null ? "" : dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn4"].Value.ToString();
                    string packing_barcode= dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn1"].Value == null ? "" : dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn1"].Value.ToString(); 
                    p.Add("id",id);
                    p.Add("oldQty", oldQty);
                    p.Add("curQty", curQty);
                    p.Add("memo", memo);
                    p.Add("packing_barcode", packing_barcode);           
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "UpateCode003AData", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        GetCode003MData();
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
            else
            {

            }
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null && dataGridView3.CurrentRow.Index > -1)
            {
                if (!string.IsNullOrEmpty(cbmStatus.Text)&&!string.IsNullOrEmpty(textBox10.Text))
                {
                    string operation = cbmStatus.Text.Split('|')[0];
                    string unit = textBox10.Text.Trim();
                    int index = dataGridView3.CurrentRow.Index;
                    Dictionary<string, Object> p = new Dictionary<string, object>();
                    string packing_barcode = dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn1"].Value == null ? "" : dataGridView3.Rows[index].Cells["dataGridViewTextBoxColumn1"].Value.ToString();
                    p.Add("packing_barcode", packing_barcode);
                    string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_MMSAPI", "KZ_MMSAPI.Controllers.MMS_Outsourcing_TrackInfoServer", "GetCode003ADetailData", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                    {
                        string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                        DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                        dataGridView2.DataSource = dtJson.DefaultView;
                        dataGridView2.Update();
                    }
                    else
                    {
                        dataGridView2.DataSource = null;
                        dataGridView2.Update();
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("查询条件中的操作类型和收发单位不能为空");
                }    
            }
        }
    }
}
