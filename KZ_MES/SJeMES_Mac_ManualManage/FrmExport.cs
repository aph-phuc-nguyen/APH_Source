using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SJeMES_Mac_ManualManage
{
    public partial class FrmExport : Form
    {
        GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
         string Server = string.Empty;
         string Port = string.Empty;
        public FrmExport(Dictionary<string, object> OBJ = null)
        {
            Server = GDSJ_Framework.Common.ConfigHelper.getSetting("Server");
            Port = GDSJ_Framework.Common.ConfigHelper.getSetting("Port");
            //DB = new GDSJ_Framework.DBHelper.DataBase("SqlServer", Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
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
            DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
        }

      
        private void FrmExport_Load(object sender, EventArgs e)
        {
            string sql = @"select manual_no,manual_name,manual_Route,guid||'.'||suffix_name as fileName from MAC030M";

            dataGridView1.DataSource = DB.GetDataTable(sql);
            DataGridViewCheckBoxColumn ChCol = new DataGridViewCheckBoxColumn();
            ChCol.Name = "操作";
            ChCol.HeaderText = "操作";
            ChCol.Width = 50;
            dataGridView1.Columns.Insert(0, ChCol);
            dataGridView1.Columns[0].Width = 20;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;

        }
       

        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择保存文件路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var savePath = dialog.SelectedPath;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                        {
                            string file= dataGridView1.Rows[i].Cells["文件"].Value.ToString();
                            string fileName = dataGridView1.Rows[i].Cells["手册名称"].Value.ToString() + file.Substring(file.LastIndexOf("."));
                            if (! PostFileToService.UpLoadFile("http://" + Server + ":" + Port + "/Attachment/ManualFile/"+ file, savePath+"\\" + fileName))
                            {
                                MessageBox.Show("导出失败！");
                                return;
                            }
                        }

                    }
                    MessageBox.Show("导出成功！");
                    this.Close();
                }
               
               
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
