using MaterialSkin.Controls;
using Newtonsoft.Json;
using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FollowTierMeeting
{
    public partial class F_BCS_SelectDepartment_ProcessForm : MaterialForm
    {       
        public int formTypes = (int)Parameters.DeptForm.All;
        public delegate void getCodeDeptTransfer(string dept);
        public delegate void DataChangeHandler(object sender, DataChangeEventArgs args);
        public event DataChangeHandler DataChange;
        public int dgvindex = -1;
        public bool firstenter = true;
        public bool isLoading = false;
        public F_BCS_SelectDepartment_ProcessForm(int type,int formType= (int)Parameters.DeptForm.All)
        {
            InitializeComponent();
            InitUI();
            //GetDataDept("", "");           
            switch (type)
            {
                case (int)Parameters.QueryDeptType.Plant: //plant
                    dgvLine.Visible = false;
                    dgvLine.Visible = false;
                    dvgSection.Visible = false;
                    dvgSection.Visible = false;
                    break;
                case (int)Parameters.QueryDeptType.Section: //section
                    dgvLine.Visible = false;
                    dgvLine.Visible = false;
                    break;
                case (int)Parameters.QueryDeptType.Line: //line
                    break;
                default: //line
                    break;
            }
            formTypes = formType;
          //  this.KeyPreview = true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.KeyCode == Keys.Right)
            {
                if (dgvindex == 0)
                {
                    PlantCellClick();
                    dvgSection.Focus();
                    //   firstenter = false;
                }
                if (dgvindex == 1)
                {
                    SectionCellClick();
                    dgvLine.Focus();
                    // firstenter = false;
                }
                return true;
            }
            if (e.KeyCode == Keys.Left)
            {
                if (dgvindex == 1)
                {
                    // PlantCellClick();
                    dgvPlant.Focus();
                }
                if (dgvindex == 2)
                {
                    // PlantCellClick();
                    dvgSection.Focus();
                }
                return true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dgvLine.Focused == true)
                {
                    LineCellDoubClick();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #region event dept
        public class DataChangeEventArgs : EventArgs
        {
            public string deptCode { get; set; }
            public int deptType { get; set; }
            public string typeLine { get; set; }//19092020 GIAP ADD
            public DataChangeEventArgs(string code, int type,string typeline="")
            {
                deptCode = code;
                deptType = type;
                typeLine = typeline;//19092020 GIAP ADD
            }
        }
        public void OnDataChange(object sender, DataChangeEventArgs args)
        {
            DataChange?.Invoke(this, args);
        }
        #endregion

        #region Getdept
        private void GetDataDept(string plant, string section)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("plant", plant);
            p.Add("section", section);
            string ret =
                SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.client.APIURL,
                    "BonusCalculationSystem",
                    "BonusCalculationSystem.Controllers.BonusCalculationSystemServer",
                                        "GetDept",
                    Program.client.UserToken,
                    JsonConvert.SerializeObject(p));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                isLoading = true;
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                if (plant != "")//Select plant
                {

                    if (dtJson.Rows.Count > 0)//show section and line
                    {
                        //  dvgSection.DataSource = dtJson;
                       
                      Parameters.LoadDataGridView(dtJson, dvgSection);
                       

                    }
                   
                }
                else if (section != "")//when select section
                {
                    if (dtJson.Rows.Count > 0)//show Line
                    {
                        //dgvLine.DataSource = dtJson;
                        Parameters.LoadDataGridView(dtJson, dgvLine);

                    }
                   
                }
                else//select line
                {
                    // dgvPlant.DataSource = dtJson;
                    Parameters.LoadDataGridView(dtJson, dgvPlant);
                    dgvPlant.Focus();
                }
                isLoading = false;
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        #endregion

        #region init
        private void InitUI()
        {
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            int height = base.Height;

            dgvPlant.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", (float)height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dgvPlant.DefaultCellStyle.Font = new Font("宋体", (float)height / 55f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dgvPlant.AllowUserToAddRows = false;
            dgvPlant.ReadOnly = true;
            ///////////////////////////
            dvgSection.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", (float)height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dvgSection.DefaultCellStyle.Font = new Font("宋体", (float)height / 55f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dvgSection.ReadOnly = true;
            dvgSection.AllowUserToAddRows = false;
            ///////////////////////////
            dgvLine.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", (float)height / 60f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dgvLine.DefaultCellStyle.Font = new Font("宋体", (float)height / 55f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dgvLine.ReadOnly = true;
            dgvLine.AllowUserToAddRows = false;
        }
        #endregion

        #region cell Click
        private void dgvPlant_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPlant.CurrentRow != null)
            {
                DataGridViewRow data = dgvPlant.CurrentRow;
                string cellPlant = data.Cells["cellPlant"].Value.ToString();
                RemoveAllRowDataGridView(ref dgvLine);
                RemoveAllRowDataGridView(ref dvgSection);
                GetDataDept(cellPlant, "");
                //  plant = cellPlant;
            }
        }
        public void PlantCellClick()
        {
            if (dgvPlant.CurrentRow != null)
            {
                DataGridViewRow data = dgvPlant.CurrentRow;
                string cellPlant = data.Cells["cellPlant"].Value.ToString();
                RemoveAllRowDataGridView(ref dgvLine);
                RemoveAllRowDataGridView(ref dvgSection);
                GetDataDept(cellPlant, "");
                //  plant = cellPlant;
            }
        }
        private void dvgSection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvgSection.CurrentRow != null)
            {
                DataGridViewRow data = dvgSection.CurrentRow;
                string cellSection = data.Cells["cellSection"].Value.ToString();
                RemoveAllRowDataGridView(ref dgvLine);
                GetDataDept("", cellSection);
                // section = cellSection;
            }
        }
        private void SectionCellClick()
        {
            if (dvgSection.CurrentRow != null)
            {
                DataGridViewRow data = dvgSection.CurrentRow;
                string cellSection = data.Cells["cellSection"].Value.ToString();
                RemoveAllRowDataGridView(ref dgvLine);
                GetDataDept("", cellSection);
                // section = cellSection;
            }
        }
        #endregion

        #region Cell Double Click
        private void dgvLine_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1) {
                string linetype = dgvLine.Rows[e.RowIndex].Cells[1].Value.ToString(); //19092020 GIAP ADD
                string deptCode = dgvLine.Rows[e.RowIndex].Cells[0].Value.ToString();
                OnDataChange(this, new DataChangeEventArgs(deptCode, (int)Parameters.QueryDeptType.Line, linetype));
                this.Close();
            }
           
        }
        private void LineCellDoubClick()
        {
            string linetype = dgvLine.Rows[dgvLine.CurrentRow.Index].Cells[1].Value.ToString(); //19092020 GIAP ADD
            string deptCode = dgvLine.Rows[dgvLine.CurrentRow.Index].Cells[0].Value.ToString();
            OnDataChange(this, new DataChangeEventArgs(deptCode, (int)Parameters.QueryDeptType.Line, linetype));
            this.Close();
        }
       
        private void dvgSection_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (formTypes == (int)Parameters.DeptForm.All && e.RowIndex != -1)
            {
                string deptCode = dvgSection.Rows[e.RowIndex].Cells[0].Value.ToString();
                OnDataChange(this, new DataChangeEventArgs(deptCode, (int)Parameters.QueryDeptType.Section));
                this.Close();
            }

        }

        private void dgvPlant_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (formTypes == (int)Parameters.DeptForm.All && e.RowIndex != -1)
            {
                string deptCode = dgvPlant.Rows[e.RowIndex].Cells[0].Value.ToString();
                OnDataChange(this, new DataChangeEventArgs(deptCode, (int)Parameters.QueryDeptType.Plant));
                this.Close();
            }
        }
        #endregion

        #region Function
        private void RemoveAllRowDataGridView(ref DataGridView dgv)
        {
            //for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            //{
            //    try
            //    {
            //        dgv.Rows.RemoveAt(i);
            //    }
            //    catch { }
            //}
            Parameters.clearDatagridview(dgv);
        }
        #endregion

        #region Regex
        private void dgvPlant_Enter(object sender, EventArgs e)
        {
            dgvindex = 0;
        }

        private void dvgSection_Enter(object sender, EventArgs e)
        {
            dgvindex = 1;
        }

        private void dgvLine_Enter(object sender, EventArgs e)
        {
            dgvindex = 2;
        }
        #endregion

        private void F_BCS_SelectDepartment_ProcessForm_Load(object sender, EventArgs e)
        {
            GetDataDept("", "");
        }
    }
}
