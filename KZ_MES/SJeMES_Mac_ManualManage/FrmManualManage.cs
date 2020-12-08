using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SJeMES_Mac_ManualManage
{
    public partial class FrmManualManage : Form
    {
        GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
        public string Server = string.Empty;
        public string Port = string.Empty;
        string localPath = string.Empty;//本地文件路径
        string guid = string.Empty;
        string operation = string.Empty;

        int dgvIndex = -1;
        public FrmManualManage(Dictionary<string, object> OBJ = null)
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

                //Program.WebServiceUrl = (OBJ as Dictionary<string, object>)["WebServiceUrl"] as string;
                //Program.Org = new GDSJ_Framework.Class.OrgClass();
                //Program.Org.Org = (OBJ as Dictionary<string, object>)["Org"] as string;
                //Program.Org.OrgName = (OBJ as Dictionary<string, object>)["OrgName"] as string;
                //Program.Org.DBServer = "";
                //Program.Org.DBType = "oracle";
                //Program.Org.DBName = "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.125)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME =SJWB )))";
                //Program.Org.DBUser = "SJWB";
                //Program.Org.DBPassword = "123";
                //Program.User = (OBJ as Dictionary<string, object>)["User"] as string;
                //Oracle

            }
            //DB = new GDSJ_Framework.DBHelper.DataBase(Program.Org.DBType, Program.Org.DBServer, Program.Org.DBName, Program.Org.DBUser, Program.Org.DBPassword, string.Empty);
        }

        private void FrmManualManage_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData(string queryContent="")
        {
            try
            {
                //lbl_operation.Text = "查询手册";
                string sql = @"select manual_no 手册编号,manual_name 手册名称,manual_Route,guid,file_state from MAC030M";
                if (!string.IsNullOrEmpty(queryContent))
                {
                    switch (queryContent)
                    {
                        case "手册编号":
                            sql += " where manual_no like'%" + textBox1.Text + "%'";
                            break;
                        case "手册名称":
                            sql += " where manual_name like'%" + textBox1.Text + "%'";
                            break;
                    }
                }
                DataTable dt = Program.Client.GetDT(sql);
                if (dt.Rows.Count>0)
                {
                    dataGridView1.DataSource = Program.Client.GetDT(sql); //DB.GetDataTable(sql);
                    dataGridView1.ClearSelection();
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[4].Visible = false;

                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(SetBackgroundColor));
                    thread.IsBackground = true;
                    thread.Start();
                }
               
                //DataGridViewCheckBoxColumn ChCol = new DataGridViewCheckBoxColumn();
                //ChCol.Name = "操作";
                //ChCol.HeaderText = "操作";
                //ChCol.Width = 50;
                //dataGridView1.Columns.Insert(0, ChCol);
                //dataGridView1.Columns[0].Width = 20;
                //dataGridView1.Columns[1].Width = 40;
                //dataGridView1.Columns[3].Visible = false;
                //dataGridView1.Columns[4].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void SetBackgroundColor()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if(dataGridView1.Rows[i].Cells["file_state"].Value.ToString()=="签出")
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
           
        }
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                localPath= fileDialog.FileName;
                string file = fileDialog.FileName;

                file = file.Remove(0, file.LastIndexOf("\\") + 1);
                file = file.Substring(0, file.LastIndexOf("."));
                textBox3.Text = file;
                textBox8.Text = fileDialog.FileName.Remove(fileDialog.FileName.LastIndexOf("\\"));
            }
        }
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            lbl_operation.Text = "新增手册";
            operation = "insert";
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                lbl_operation.Text = "修改手册";
                operation = "update";
               
              
            }
            catch (Exception ex)
            {

                MessageBox.Show("修改失败,"+ex.Message);
                return;
            }
        }
       /// <summary>
       /// 删除
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                lbl_operation.Text = "删除手册";
                operation = "delete";
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败," + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            string file_path = string.Empty;
            switch (operation)
            {
                #region 添加
                case "insert":
                    try
                    {
                        if (string.IsNullOrEmpty(textBox8.Text))
                        {
                            MessageBox.Show("请选择文件！");
                            return;
                        }
                        if (string.IsNullOrEmpty(comboBox3.Text))
                        {
                            MessageBox.Show("请选择手册状态！");
                            return;
                        }
                        if (string.IsNullOrEmpty(comboBox2.Text))
                        {
                            MessageBox.Show("请选择手册类型！");
                            return;
                        }

                        string newGuid = Guid.NewGuid().ToString();
                        path = "http://" + Server + ":" + Port + "/SJ_WebService.aspx?operation=insert&fileType=manualFile&newid=" + newGuid+ localPath.Substring(localPath.LastIndexOf('.'));

                        file_path = "http://" + Server + ":" + Port + "/Attachment/ManualFile";
                       
                        if (uploadFileByHttp(path, localPath))
                        {
                            string manual_no = string.Empty;//手册编号
                            string sql = "select nvl(MAX(manual_no),to_char(sysdate, 'yyyymmdd')||'000') from MAC030M join dual on 1 = 1 WHERE manual_no LIKE '" + DateTime.Now.ToString("yyMMdd") + "%'";
                            //string sql = "select nvl(MAX(manual_no) from MAC030M";
                            string suffix_name = localPath.Remove(0, localPath.LastIndexOf(".")+1);
                            DataTable dtt = Program.Client.GetDT(sql);
                            if (dtt.Rows.Count > 0)
                            {
                                //dtt.Rows[0][0].ToString(); 
                                manual_no = DateTime.Now.ToString("yyMMdd") + (Convert.ToInt32(dtt.Rows[0][0].ToString().Replace(DateTime.Now.ToString("yyMMdd"), "")) + 1).ToString("000");
                            }
                            string sqlnowID = @"select nvl(max(id),0)+1 from MAC030M";
                            DataTable dt = Program.Client.GetDT(sqlnowID);
                            string nowID = string.Empty;
                            if (dt.Rows.Count > 0)
                            {
                                nowID = dt.Rows[0][0].ToString();
                            }
                            //string nowID = Program.Client.GetDT(sql).Rows[0][0].ToString(); /*DB.GetString(sqlnowID)*/;
                            //sql = @"insert into MAC030M(id,manual_no,manual_name,manual_type,manual_Route,suffix_name,state,memo,guid,createby,createdate,createtime)
                            // values("+ nowID + ",'" + manual_no + "','" + textBox3.Text + "','" + comboBox2.Text + "','" + file_path + "','" + suffix_name + "','" + comboBox3.Text + "','" + textBox9.Text + "','" + newGuid + "','" +
                            //            Program.UserName + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            sql = @"insert into MAC030M(id,manual_no,manual_name,manual_type,manual_Route,suffix_name,state,memo,guid,createby,createdate,createtime)
                             values("+ nowID + ",'" + manual_no + "','" + textBox3.Text + "','" + comboBox2.Text + "','" + file_path + "','" + suffix_name + "','" + comboBox3.Text + "','" + textBox9.Text + "','" + newGuid + "','" +
                                       Program.UserName + "',TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),to_char(sysdate,'hh24:mi:ss'))";
                            //MessageBox.Show(sql);
                            //if (DB.ExecuteNonQueryOffline(sql) > 0)
                            if (Program.Client.ExecuteNonQuery(sql) > 0)
                            {
                                MessageBox.Show("保存成功");
                                LoadData();
                            }
                        }
                        else
                        {
                            MessageBox.Show("保存失败！");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("新增失败," + ex.Message);
                        return;
                    }
                   
                    break;
                #endregion

                #region 修改
                case "update":
                    try
                    {
                        if (string.IsNullOrEmpty(guid))
                        {
                            MessageBox.Show("请选中保养手册！");
                            return;
                        }
                        else
                        {

                            if (MessageBox.Show("确定要修改保养手册吗？", "修改提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                string sql = "select file_state from MAC030M where guid='" + guid + "'";
                                DataTable dt = Program.Client.GetDT(sql);
                                string signoutby = string.Empty;
                                if (dt.Rows.Count>0)
                                {
                                    //if (DB.GetString(sql) == "签出")
                                    if (dt.Rows[0][0].ToString() == "签出")
                                    {
                                        sql = "select signoutby from MAC030M where guid='" + guid + "'";
                                        dt = Program.Client.GetDT(sql);
                                        if (dt.Rows.Count>0)
                                        {
                                            //if (DB.GetString("select signoutby from MAC030M where guid='" + guid + "'").ToLower() == Program.User.ToLower())//判断签出人是否和当前登录人相同
                                            if (dt.Rows[0][0].ToString().ToLower() == Program.User.ToLower())//判断签出人是否和当前登录人相同
                                            {
                                                signoutby = dt.Rows[0][0].ToString();
                                                file_path = "http://" + Server + ":" + Port + "/Attachment/ManualFile";
                                                string suffix_name = localPath.Remove(0, localPath.LastIndexOf(".") + 1);
                                                path = "http://" + Server + ":" + Port + "/SJ_WebService.aspx?operation=update&fileType=manualFile&newid=" + guid + "." + suffix_name;//请求服务器路径
                                                if (string.IsNullOrEmpty(localPath))//当修改时没有上传文件
                                                {
                                                    sql = "select suffix_name from MAC030M where guid='" + guid + "'";
                                                    dt = Program.Client.GetDT(sql);
                                                    if (dt.Rows.Count>0)
                                                    {
                                                        suffix_name = dt.Rows[0][0].ToString();
                                                    }
                                                    path = "http://" + Server + ":" + Port + "/SJ_WebService.aspx?operation=delete&fileType=manualFile&newid=" + guid + "." + suffix_name;

                                                    if (PostFileToService.DeleteFile(path))
                                                    {
                                                        //sql = @"update MAC030M set manual_name='" + textBox3.Text + "',manual_Route='" + path + "',manual_type='" + comboBox2.Text + "',suffix_name='" + suffix_name + "',state='" + comboBox3.Text + "',memo='" + textBox9.Text
                                                        //                        + "',modifyby='" + Program.User + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' where guid='" + guid + "'";
                                                        //sql = @"update MAC030M set manual_name='" + textBox3.Text + "',manual_Route='" + path + "',manual_type='" + comboBox2.Text + "',suffix_name='" + suffix_name + "'," +
                                                        //    "state='" + comboBox3.Text + "',memo='" + textBox9.Text
                                                        //                       + "',modifyby='" + Program.User + "'," +
                                                        //                       "modifydate=TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')," +
                                                        //                       "modifytime=to_char(sysdate,'hh24:mi:ss') where guid='" + guid + "'";
                                                        //if (DB.ExecuteNonQueryOffline(sql) > 0)
                                                        sql = @"update MAC030M set manual_name='" + textBox3.Text + "',manual_Route='" + path + "',manual_type='" + comboBox2.Text + "',suffix_name='" + suffix_name + "'," +
                                                           "state='" + comboBox3.Text + "',memo='" + textBox9.Text
                                                                              + "',modifyby='" + Program.User + "'," +
                                                                              "modifydate=TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')," +
                                                                              "modifytime=to_char(sysdate,'hh24:mi:ss') where guid='" + guid + "'";
                                                        if (Program.Client.ExecuteNonQuery(sql) > 0)
                                                        {
                                                            MessageBox.Show("修改成功！");
                                                            guid = string.Empty;
                                                            textBox8.Text = "";
                                                            LoadData();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("修改失败！");
                                                        return;
                                                    }
                                                }
                                                else//更换上传文件
                                                {
                                                    if (uploadFileByHttp(path, localPath))
                                                    {
                                                        //  sql = @"update MAC030M set manual_name='" + textBox3.Text + "',manual_Route='" + path + "',manual_type='" + comboBox2.Text + "',suffix_name='" + suffix_name + "',state='" + comboBox3.Text + "',memo='" + textBox9.Text
                                                        //+ "',modifyby='" + Program.UserName + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' where guid='" + guid + "'";
                                                          sql = @"update MAC030M set manual_name='" + textBox3.Text + "',manual_Route='" + file_path + "',manual_type='" + comboBox2.Text + "',suffix_name='" + suffix_name + "',state='" + comboBox3.Text + "',memo='" + textBox9.Text
                                                        + "',modifyby='" + Program.UserName + "'," +
                                                        "modifydate=TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')," +
                                                        "modifytime=to_char(sysdate,'hh24:mi:ss') where guid='" + guid + "'";
                                                        //if (DB.ExecuteNonQueryOffline(sql) > 0)
                                                        if (Program.Client.ExecuteNonQuery(sql) > 0)
                                                        {
                                                            MessageBox.Show("修改成功！");
                                                            guid = string.Empty;
                                                            textBox8.Text = "";
                                                            LoadData();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("修改失败！");
                                                        return;
                                                    }
                                                }


                                            }
                                            else
                                            {
                                                //MessageBox.Show("修改失败,手册已被" + DB.GetString("select signoutby from MAC030M where guid='" + guid + "'") + "签出！");
                                                MessageBox.Show("修改失败,手册已被" + signoutby + "签出！");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("修改失败,请先签出手册");
                                        return;
                                    }
                                }
                               
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("修改失败," + ex.Message);
                        return;
                    }
                   
                    break;
                #endregion

                #region 删除
                case "delete":
                    try
                    {
                        if (string.IsNullOrEmpty(guid))
                        {
                            MessageBox.Show("请选中保养手册！");
                            return;
                        }
                        else
                        {
                            if (MessageBox.Show("确定要删除保养手册吗？", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                string sql = "select file_state from MAC030M where guid='" + guid + "'";
                                DataTable dt = Program.Client.GetDT(sql);
                                if (dt.Rows.Count>0)
                                {
                                    string suffix_name = string.Empty;
                                    //if (DB.GetString(sql) != "签出")
                                    if (dt.Rows[0][0].ToString() != "签出")
                                    {
                                        sql = "select suffix_name from MAC030M where guid='" + guid + "'";
                                        dt = Program.Client.GetDT(sql);
                                        if (dt.Rows.Count>0)
                                        {
                                            suffix_name = dt.Rows[0][0].ToString();
                                        }
                                        //string suffix_name = DB.GetString("select suffix_name from MAC030M where guid='" + guid + "'");
                                        path = "http://" + Server + ":" + Port + "/SJ_WebService.aspx?operation=delete&fileType=manualFile&newid=" + guid + "." + suffix_name;
                                        if (PostFileToService.DeleteFile(path))
                                        {
                                            sql = "delete from MAC030M where guid='" + guid + "'";
                                            //if (DB.ExecuteNonQueryOffline(sql) > 0)
                                            if (Program.Client.ExecuteNonQuery(sql) > 0)
                                            {
                                                MessageBox.Show("删除成功！");
                                                ClearTextBox();
                                                guid = string.Empty;
                                                LoadData();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("删除失败！");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        sql = "select signoutby from MAC030M where guid='" + guid + "'";
                                        dt = Program.Client.GetDT(sql);
                                        if (dt.Rows.Count > 0)
                                        {
                                            suffix_name = dt.Rows[0][0].ToString();
                                        }
                                        //MessageBox.Show("删除失败," + "该手册已被" + DB.GetString(sql) + "签出");
                                        MessageBox.Show("删除失败," + "该手册已被" + suffix_name + "签出");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("修改失败," + ex.Message);
                        return; ;
                    }
                   
                    break;
                #endregion
                default:
                    MessageBox.Show("请选择操作！");
                    break;
            }
          
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, EventArgs e)
        {
            FrmImport frm = new FrmImport();
            frm.ShowDialog();
            if (frm.isSuccess)
            {
                LoadData();
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, EventArgs e)
        {
            FrmExport frm = new FrmExport();
            frm.ShowDialog();
        }
        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="webUrl"></param>
        /// <param name="localFileName"></param>
        /// <returns></returns>
        public bool uploadFileByHttp(string webUrl, string localFileName)
        {
            bool b = false;
            try
            {
               b= PostFile(webUrl, localFileName);
                //System.Net.WebClient myWebClient = new System.Net.WebClient();
                //myWebClient.UploadFile(webUrl, "POST", localFileName);

            }
            catch(Exception ex)
            {
                return false;
            }
            return b;
        }
        /// <summary>
        /// Post文件到服务器
        /// </summary>
        /// <param name="tagHost">目标主机地址地址</param>
        /// <param name="filePath">上传的文件，本地路径</param>
        /// <returns></returns>
        public static bool PostFile(string tagHost, string filePath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tagHost);//目标主机ip地址
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] infbytes = new byte[(int)fs.Length];
                fs.Read(infbytes, 0, infbytes.Length);
                fs.Close();

                string cookieheader = string.Empty;
                CookieContainer cookieCon = new CookieContainer();
                request.Method = "POST";
                request.CookieContainer = cookieCon;
                request.ContentType = "text/html";
                request.ContentLength = infbytes.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(infbytes, 0, infbytes.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //获得响应流
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                response.Close();
                receiveStream.Close();
                readStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
        /// <summary>
        /// 签入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_signIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guid))
            {
                MessageBox.Show("请选中保养手册签入！");
                return;
            }
            try
            {
                string sql = "select file_state from MAC030M WHERE guid='" + guid+"'";
                DataTable dt = Program.Client.GetDT(sql);
                if (dt.Rows.Count>0)
                {
                    //if (DB.GetString(sql) == "签入")
                    if (dt.Rows[0][0].ToString() == "签入")
                    {
                        MessageBox.Show("该手册已是签入状态！");
                        return;
                    }
                    else
                    {
                        sql = "select signoutby from MAC030M WHERE guid='" + guid + "'";
                        dt = Program.Client.GetDT(sql);
                        var signoutby = string.Empty;
                        if (dt.Rows.Count>0)
                        {
                           signoutby = dt.Rows[0][0].ToString();
                        }
                        if (string.IsNullOrEmpty(signoutby))
                        {
                            sql = @"update MAC030M set file_state='签入' where guid='" + guid + "'";
                            //if (DB.ExecuteNonQueryOffline(sql) > 0)
                            if (Program.Client.ExecuteNonQuery(sql) > 0)
                            {
                                MessageBox.Show("签入成功！");
                                LoadData();
                            }
                        }
                        else
                        {
                            if (signoutby.ToLower() != Program.User.ToLower())
                            {
                                MessageBox.Show("签入失败,该手册被" + signoutby + "签出");
                                return;
                            }
                            else
                            {
                                sql = @"update MAC030M set file_state='签入' where guid='" + guid + "'";
                                //if (DB.ExecuteNonQueryOffline(sql) > 0)
                                if (Program.Client.ExecuteNonQuery(sql) > 0)
                                {
                                    MessageBox.Show("签入成功！");
                                    LoadData();
                                }
                            }
                        }


                    }
                }
              
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show("签入失败," + ex.Message);
            }
        }
        /// <summary>
        /// 签出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_signOut_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guid))
            {
                MessageBox.Show("请选中保养手册签出！");
                return;
            }
            try
            {
                string sql = "select file_state,signoutby from MAC030M where guid='" + guid + "'";
                var tab = Program.Client.GetDT(sql); 
                if (tab.Rows.Count > 0)
                {
                    if (tab.Rows[0]["file_state"].ToString() == "签出"&& tab.Rows[0]["signoutby"].ToString().ToLower()!=Program.User.ToLower())
                    {
                        MessageBox.Show("该手册已被" + tab.Rows[0]["signoutby"].ToString() + "签出,签出失败！");
                        return;
                    }else
                    {
                        if(tab.Rows[0]["file_state"].ToString() == "签出")
                        {
                            MessageBox.Show("手册已被签出，请直接修改！");
                            return;
                        }
                        sql= "update MAC030M set file_state='签出',signoutby='" + Program.User + "' where guid='" + guid + "'";
                        if (Program.Client.ExecuteNonQuery(sql) > 0) 
                        {
                            MessageBox.Show("签出成功!");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("签出失败!");
                        }
                    }
                }else
                {
                    MessageBox.Show("签出失败!");
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("签出失败," + ex.Message);
                return;
            }
        }
        private void btn_See_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guid))
            {
                MessageBox.Show("请选中保养手册查看！");
                return;
            }
            string sql = "select manual_Route||'/'||guid||'.'||suffix_name as fileName from MAC030M where guid='" + guid + "'";
            string file_path = Program.Client.GetDT(sql).Rows[0][0].ToString();
            FrmPreview frm = new FrmPreview();
            frm.path = file_path;
            frm.ShowDialog();
        }

    
        /// <summary>
        /// 清空内容
        /// </summary>
        private void ClearTextBox()
        {
            try
            {
                foreach (Control ctr in panel4.Controls)
                {
                    if (ctr is TextBox)
                        ctr.Text = "";
                    else if (ctr is ComboBox)
                        ctr.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dgvIndex = e.RowIndex;
                guid = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                string sql = @"select * from MAC030M where guid='"+ guid+"'";
                //var tab = DB.GetDataTable(sql);
                var tab = Program.Client.GetDT(sql);
                if (tab.Rows.Count > 0)
                {
                    textBox2.Text = tab.Rows[0]["manual_no"].ToString();
                    textBox3.Text = tab.Rows[0]["manual_name"].ToString();
                    comboBox2.Text = tab.Rows[0]["manual_type"].ToString(); 
                    comboBox3.Text = tab.Rows[0]["state"].ToString();
                    textBox9.Text = tab.Rows[0]["memo"].ToString();
                }
            }
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("请选择查询条件");
                return;
            }
            LoadData(comboBox1.Text);

        }

    }
}
