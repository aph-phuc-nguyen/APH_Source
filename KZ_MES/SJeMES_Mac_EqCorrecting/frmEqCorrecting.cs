using GDSJ_Framework.WinForm.CommonForm;
using SJeMES_Control_Library.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SJeMES_Mac_EqCorrecting
{
    public partial class frmEqCorrecting : Form
    {
        //GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
        GDSJ_Framework.DBHelper.Oracle DB = new GDSJ_Framework.DBHelper.Oracle();

        private string machine_no;
        public delegate void TransfDelegate(Boolean value);//定义委托

        public frmEqCorrecting(string machine_no, Dictionary<string, object> OBJ=null)
        {
            this.machine_no = machine_no;
          
            InitializeComponent();
            if (OBJ != null)
            {
                //Program.WebServiceUrl = (OBJ as Dictionary<string, object>)["WebServiceUrl"] as string;
                //Program.Org = new GDSJ_Framework.Class.OrgClass();
                //Program.Org.Org = (OBJ as Dictionary<string, object>)["Org"] as string;
                //Program.Org.OrgName = (OBJ as Dictionary<string, object>)["OrgName"] as string;
                //Program.Org.DBServer = (OBJ as Dictionary<string, object>)["DBServer"] as string;
                //Program.Org.DBType = (OBJ as Dictionary<string, object>)["DBType"] as string;
                //Program.Org.DBName = (OBJ as Dictionary<string, object>)["DBName"] as string;
                //Program.Org.DBUser = (OBJ as Dictionary<string, object>)["DBUser"] as string;
                //Program.Org.DBPassword = (OBJ as Dictionary<string, object>)["DBPassword"] as string;
                //Program.User = (OBJ as Dictionary<string, object>)["User"] as string;

                Program.WebServiceUrl = (OBJ as Dictionary<string, object>)["WebServiceUrl"] as string;
                Program.Org = new GDSJ_Framework.Class.OrgClass();
                Program.Org.Org = (OBJ as Dictionary<string, object>)["Org"] as string;
                Program.Org.OrgName = (OBJ as Dictionary<string, object>)["OrgName"] as string;
                Program.Org.DBServer = "";
                Program.Org.DBType = "oracle";
                Program.Org.DBName = "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.125)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME =SJWB )))";
                Program.Org.DBUser = "SJWB";
                Program.Org.DBPassword = "123";
                Program.User = (OBJ as Dictionary<string, object>)["User"] as string;
                //Oracle

            }
            if (!string.IsNullOrEmpty(this.machine_no))
                SelectData(this.machine_no, "2");
        }

        private void txt_EqCode_KeyDown(object sender, KeyEventArgs e)
        {
            //查询是否存在该条码的设备信息
            if (e.KeyCode == Keys.Enter)
            {

                SelectData(txt_EqCode.Text.Trim(),"1");
                txt_EqCode.SelectAll();//扫描枪全选
            }
        }
        private void SelectData(string machine_no,string flat)
        {
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("machine_no", machine_no);
            p.Add("UserName", Program.UserName);
            p.Add("org", Program.Org.Org);
            try
            {
                
                string xml = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.Org, Program.Client.WebServiceUrl, "SJEMS_WBAPI", "SJEMS_WBAPI.MES030", "GetData_JiaoZheng_PC", p);//
                bool IsSuccess = Convert.ToBoolean(GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<IsSuccess>", "</IsSuccess>"));
                string RetData = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<RetData>", "</RetData>");

                if (!IsSuccess)
                {
                    btn_Save.Enabled = false;
                    return;
                }
                else
                {
                    btn_Save.Enabled = true;
                    string strdt = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<MES030A5>", "</MES030A5>");
                    string strold = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<oldMES030A5>", "</oldMES030A5>");
                    string strmac001 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<MAC002>", "</MAC002>");

                    string mname = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<machine_name>", "</machine_name>");
                    string no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<machine_no>", "</machine_no>");
                    string item_fix = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<date_plan>", "</date_plan>");
                    string state = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<state>", "</state>");
                    string work_state = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<work_state>", "</work_state>");
                    string owner = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<owner>", "</owner>");



                    DataTable palndt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(strdt);
                    DataTable palnolddt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(strold);
                    DataTable dtmac001 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(strmac001);

                    txt_EqNo.Text = no;
                    txt_EqName.Text = mname;
                    txt_State.Text = work_state;
                    txt_People.Text = owner;
                    if (flat == "1")
                    {
                        LoadMachineData();
                    }
                    
                    //是否存在计划
                    if (palndt.Rows.Count > 0)
                    {
                        
                        dgv_OldData.DataSource = palndt;
                        dgv_OldData.Columns["计划编号"].Visible = false;
                        //dgv_HistoryData.DataSource = palnolddt;

                        if (dtmac001.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("计划编号");
                            dt.Columns.Add("计划日期");
                            dt.Columns.Add("内容编号");
                            dt.Columns.Add("内容名称");
                            dt.Columns.Add("标准值");
                            dt.Columns.Add("维护人员");
                            dt.Columns.Add("机械状态");
                            dt.Columns.Add("保养说明");

                            for (int i = 0; i < dtmac001.Rows.Count; i++)
                            {
                                DataRow newRow = dt.NewRow();
                                newRow["计划编号"] = palndt.Rows[0]["计划编号"];
                                newRow["计划日期"] = palndt.Rows[0]["计划日期"];
                                newRow["内容编号"] = dtmac001.Rows[i]["内容项编号"];
                                newRow["内容名称"] = dtmac001.Rows[i]["内容项名称"];
                                newRow["标准值"] = string.Empty;
                                newRow["维护人员"] = txt_People.Text.Trim();
                                newRow["机械状态"] = string.Empty;
                                newRow["保养说明"] = string.Empty;
                                dt.Rows.Add(newRow);
                            }

                            
                            dgv_MaintainData.DataSource = dt;
                            dgv_MaintainData.Columns["计划编号"].Visible = false;
                            dgv_MaintainData.Columns["计划日期"].Visible = false;
                        }
                        else
                        {
                            
                            MessageBox.Show("不存在校正内容");
                            btn_Save.Enabled = false;
                        }



                    }
                    else
                    {
                        dgv_HistoryData.DataSource = palnolddt;
                        MessageBox.Show("该设备不存在校正计划");
                        btn_Save.Enabled = false;
                        return;
                    }
                    dgv_HistoryData.DataSource = palnolddt;
                }
                
               
                
            }
            catch (Exception ex)
            {

            }

        }

        private void dgv_MaintainData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_MaintainData.Rows[e.RowIndex].Cells[4].Selected)
            {
                //string sql = @"select MAC002A2.item_id as '设备编号',value as '标准值' from MAC002A1(NOLOCK) join MAC002A2 on MAC002A1.item_id=MAC002A2.item_id where machine_no='" + txt_EqNo.Text.Trim() + "' AND MAC002A2.item_id='" + dgv_MaintainData.Rows[e.RowIndex].Cells[2].Value.ToString() + "'";
                string sql = @"select rownum 行号,MAC002A2.item_id 设备编号,value 标准值 from MAC002A1 join MAC002A2 on MAC002A1.item_id=MAC002A2.item_id ";
                FrmSelectData frmData = new FrmSelectData(sql, true, Program.Client);
                frmData.ShowDialog();
                if (frmData.RetData != null)
                {
                    if (frmData.RetData.Rows.Count > 0)
                    {
                        string value = frmData.RetData.Rows[0]["标准值"].ToString();
                        dgv_MaintainData.Rows[e.RowIndex].Cells[4].Value = value;
                        dgv_MaintainData.CurrentCell = null;
                    }
                }

                //SJeMES_Control_Library.Forms.FrmSelectData frmData = new SJeMES_Control_Library.Forms.FrmSelectData(sql, true, Program.Client);

                //frmSearchData frm = new frmSearchData(Program.Org, Program.WebServiceUrl, sql, true, true);
                //frm.ShowDialog();

                //if (!string.IsNullOrEmpty(frm.ReturnDataXML))
                //{
                //    string value = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(frm.ReturnDataXML, "<标准值>", "</标准值>");
                //    dgv_MaintainData.Rows[e.RowIndex].Cells[4].Value = value;
                //    dgv_MaintainData.CurrentCell = null;
                //}
                //frmData.ShowDialog();
                //if (frmData.RetData.Rows.Count > 0)
                //{
                //    string value = frmData.RetData.Rows[0]["标准值"].ToString();
                //    dgv_MaintainData.Rows[e.RowIndex].Cells[4].Value = value;
                //    dgv_MaintainData.CurrentCell = null;
                //}
            }
        }

        public event TransfDelegate TransfEvent;


        /// <summary>
        /// 需要修改webservice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_MaintainData.Rows.Count > 0)
                {
                    DataTable palndt = dgv_MaintainData.DataSource as DataTable;
                    DataTable dt = dgv_OldData.DataSource as DataTable;

                    string EqNO = txt_EqNo.Text.Trim();
                    string EqName = txt_EqName.Text.Trim();
                    string People = txt_People.Text.Trim();
                    string state = txt_State.Text.Trim();
                    string book = txt_CopyBook.Text.Trim();


                    string strpalndt = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(palndt);
                    string strdt = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                    //string strpalndt = "", strdt="";
                    Dictionary<string, string> p = new Dictionary<string, string>();
                    p.Add("strMES030A5", strpalndt);
                    p.Add("stroldMES030A5", strdt);
                    p.Add("machine_no", EqNO);
                    p.Add("machine_name", EqName);
                    p.Add("People", People);
                    p.Add("state", state);
                    p.Add("book", book);
                    p.Add("username", Program.UserName);

                    string xml = GDSJ_Framework.Common.WebServiceHelper.RunService(Program.Org, Program.Client.WebServiceUrl, "SJEMS_WBAPI", "SJEMS_WBAPI.MAC040", "Confirm_JiaoZheng_PC", p);//

                    bool IsSuccess = Convert.ToBoolean(GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(xml, "<IsSuccess>", "</IsSuccess>"));
                    if (!IsSuccess)
                    {
                        if (string.IsNullOrEmpty(machine_no))
                        {
                            MessageBox.Show("校正失败！");
                            return;
                        }else
                        {
                            MessageBox.Show("处理失败！");
                            TransfEvent(false);
                            this.Close();
                            this.Dispose();
                            return;
                        }
                       
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(machine_no))
                        {
                            MessageBox.Show("校正成功!");
                            dgv_OldData.DataSource = null;
                            dgv_MaintainData.DataSource = null;
                            return;
                        }else
                        {
                            MessageBox.Show("处理成功!");
                            TransfEvent(true);
                            this.Close();
                            this.Dispose();
                            return;
                        }
                            
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void frmEqCorrecting_Load(object sender, EventArgs e)
        {
            LoadMachineData();
        }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        private void LoadMachineData()
        {
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(txt_EqCode.Text.Trim()))
            {
                strwhere += "where machine_no='" + txt_EqCode.Text.Trim() + "'";
            }
            ////首次进入加载所有设备信息
            //string sql = "select machine_no as 设备编号,machine_name as 设备名称 from MES030M" + strwhere;
            //DataTable dt = Program.Client.GetDT(sql);

            //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
            //DB = new GDSJ_Framework.DBHelper.Oracle(Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword);
            //首次进入加载所有设备信息
            string sql = "select machine_no  ,machine_name   from MES030M " + strwhere;
            //DataTable dt = DB.GetDataTable(sql);
            DataTable dt = Program.Client.GetDT(sql); //DB.GetDataTable(sql);
            dt.Columns[0].ColumnName = "设备编号";
            dt.Columns[1].ColumnName = "设备名称";

            dgv_QueryPaln.DataSource = dt;
            if (dgv_QueryPaln.Rows.Count > 0)
            {
                dgv_QueryPaln.Rows[0].Selected = false;
            }

        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            string strwhere = string.Empty;
            string name = string.Empty;
            if (!string.IsNullOrEmpty(cbo_QueryName.Text) && !string.IsNullOrEmpty(txt_QueryVal.Text.Trim()))
            {
                if (cbo_QueryName.Text == "设备编号")
                    name = "machine_no";
                else
                    name = "machine_name";
                strwhere += "where " + name + " like '%" + txt_QueryVal.Text.Trim() + "%'";
            }

            //首次进入加载所有设备信息
            string sql = "select machine_no,machine_name   from MES030M " + strwhere;

            //DataTable dt = DB.GetDataTable(sql);
            DataTable dt = Program.Client.GetDT(sql);
            dt.Columns[0].ColumnName = "设备编号";
            dt.Columns[1].ColumnName = "设备名称";

            dgv_QueryPaln.DataSource = dt;
        }

        private void txt_EqNo_DoubleClick(object sender, EventArgs e)
        {
            string sql = @"select rownum 行号,machine_no 设备编号,machine_name 设备名称 from MES030M";
            FrmSelectData frmData = new FrmSelectData(sql, true, Program.Client);
            frmData.ShowDialog();
            if (frmData.RetData!=null)
            {
                if (frmData.RetData.Rows.Count > 0)
                {
                    string machine_no = frmData.RetData.Rows[0]["设备编号"].ToString();
                    SelectData(machine_no, "1");
                }
            }
           
            //frmSearchData frmData = new frmSearchData(Program.Org, Program.WebServiceUrl, sql, true, true);
            //frmData.ShowDialog();

            //if (!string.IsNullOrEmpty(frmData.ReturnDataXML))
            //{
            //    string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(frmData.ReturnDataXML, "<设备编号>", "</设备编号>");  //设备编号
            //    SelectData(machine_no, "1");
            //}
        }

        private void dgv_QueryPaln_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dgv_MaintainData.Rows.Count > 0)
                {
                    DataTable dt1 = (DataTable)dgv_MaintainData.DataSource;
                    dt1.Rows.Clear();
                    dgv_MaintainData.DataSource = dt1;
                    dgv_MaintainData.Columns.Clear();
                }
                if (dgv_OldData.Rows.Count > 0)
                {
                    DataTable dt1 = (DataTable)dgv_OldData.DataSource;
                    dt1.Rows.Clear();
                    dgv_OldData.DataSource = dt1;
                    dgv_OldData.Columns.Clear();
                }
                if (dgv_HistoryData.Rows.Count > 0)
                {
                    DataTable dt1 = (DataTable)dgv_HistoryData.DataSource;
                    dt1.Rows.Clear();
                    dgv_HistoryData.DataSource = dt1;
                    dgv_HistoryData.Columns.Clear();
                }
                //获取设备编号
                string palnno = dgv_QueryPaln.Rows[e.RowIndex].Cells[0].Value.ToString();
                SelectData(palnno,"2");
            }
        }

        private void txt_CopyBook_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //string sql = @"select manual_no as '编号',manual_name as 手册名称 from MAC030M(NOLOCK) where manual_type='校正计划'";
            string sql = @"select rownum 行号,manual_no 编号,manual_name 手册名称 from MAC030M ";
            FrmSelectData frmData = new FrmSelectData(sql, true, Program.Client);
            frmData.ShowDialog();
            if (frmData.RetData != null)
            {
                if (frmData.RetData.Rows.Count > 0)
                {
                    string manual_name = frmData.RetData.Rows[0]["手册名称"].ToString();
                    txt_CopyBook.Text = manual_name;
                }
            }
            //frmSearchData frmData = new frmSearchData(Program.Org, Program.WebServiceUrl, sql, true, true);

            //frmData.ShowDialog();

            //if (!string.IsNullOrEmpty(frmData.ReturnDataXML))
            //{
            //    string manual_name = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(frmData.ReturnDataXML, "<手册名称>", "</手册名称>");
            //    txt_CopyBook.Text = manual_name;
            //}


        }

        private void dgv_OldData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string date_plan = this.dgv_OldData.Rows[e.RowIndex].Cells[1].Value.ToString(); //计划日期
            string plan_no = this.dgv_OldData.Rows[e.RowIndex].Cells[2].Value.ToString(); //计划单号
            for (int i = 0; i < dgv_MaintainData.Rows.Count; i++)
            {
                dgv_MaintainData.Rows[i].Cells[0].Value = plan_no;
                dgv_MaintainData.Rows[i].Cells[1].Value = date_plan;
            }
        }
    }
}
