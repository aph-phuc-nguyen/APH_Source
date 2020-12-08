﻿using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionOutput
{
    public partial class DepSelectForm : MaterialForm
    {        
        public delegate void DataChangeHandler(object sender, DataChangeEventArgs args);
        public event DataChangeHandler DataChange;
        DataTable dt = new DataTable();

        public DepSelectForm()
        {
            InitializeComponent();
            LoadScanDept();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
        }

        // 调用事件函数
        public void OnDataChange(object sender, DataChangeEventArgs args)
        {
            DataChange?.Invoke(this, args);
        }

        private void LoadScanDept()
        {
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.ProductionOutputServer", "GetScanDept", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(string.Empty));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("查无此数据！", Program.client, "", Program.client.Language);
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                }
            }
            else
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index > -1)
            {
                string dep_no = dataGridView1.Rows[index].Cells[0].Value.ToString();
                string dep_name = dataGridView1.Rows[index].Cells[1].Value.ToString();

                OnDataChange(this, new DataChangeEventArgs(dep_no, dep_name));

                this.Close();
            }
        }

        private void textDepSelect_TextChanged(object sender, EventArgs e)
        {
            string dep = textDepSelect.Text.ToUpper();

            DataView dv = dt.DefaultView;
            dv.RowFilter = "DEPARTMENT_CODE like '%" + dep + "%' or DEPARTMENT_NAME like '%" + dep + "%'";//过滤条件
            dataGridView1.DataSource = dv;
        }
    }

    public class DataChangeEventArgs : EventArgs
    {
        public string name{ get; set; }
        public string pass{ get; set; }

        public DataChangeEventArgs(string s1, string s2)
        {
            name = s1;
            pass = s2;
        }
    }

}
