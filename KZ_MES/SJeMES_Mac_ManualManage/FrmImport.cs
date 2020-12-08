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
    public partial class FrmImport : Form
    {
        GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
        public string Server = string.Empty;
        public string Port = string.Empty;
        public bool isSuccess = false;
        public FrmImport(Dictionary<string, object> OBJ = null)
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
        List<string> lstFilePath = new List<string>();
        private void btn_Select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Director(dialog.SelectedPath);
                DataTable tab = new DataTable();
                DataColumn dc = null;
                dc = tab.Columns.Add("序号", typeof(string));
                dc = tab.Columns.Add("文件路径", typeof(string));
                dc = tab.Columns.Add("文件名", typeof(string));
                dc = tab.Columns.Add("类型", typeof(string));
                int i = 0;
                DataRow dr ;
                foreach (var item in lstFilePath)
                {
                    try
                    {
                        i++;
                        dr = tab.NewRow();
                        string path = item.Substring(0, item.LastIndexOf("\\") + 1).ToString();
                        string fileName = item.Remove(0, item.LastIndexOf("\\") + 1);
                        fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                        string fileType = item.Remove(0, item.LastIndexOf(".") + 1);
                        dr["序号"] = i.ToString();
                        dr["文件路径"] = path;
                        dr["文件名"] = fileName;
                        dr["类型"] = fileType;
                        tab.Rows.Add(dr);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                   
                }
                dr = tab.NewRow();
                dr["序号"] = "合计";
                dr["文件路径"] = i.ToString()+"文件";
                dr["文件名"] = "";
                dr["类型"] = "";
                tab.Rows.Add(dr);
                lstFilePath = new List<string>();
                dataGridView1.DataSource = tab;
               
                dataGridView1.Columns[2].Width = 80;
                dataGridView1.Columns[3].Width = 55;
                dataGridView1.Columns[0].Width = 55;

            }
           
        }
      
        /// <summary>
        /// 获取目录下所有文件
        /// </summary>
        /// <param name="dir"></param>
        public void Director(string dir)
        {
            List<string> lstPath = new List<string>();
            try
            {

                DirectoryInfo d = new DirectoryInfo(dir);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos)
                {
                    if (fsinfo is DirectoryInfo)     //判断是否为文件夹
                    {
                        Director(fsinfo.FullName);//递归调用
                    }
                    else
                    {
                        lstPath.Add(fsinfo.FullName);

                    }
                }
                lstFilePath.AddRange(lstPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="tagHost">目标主机地址</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool PostData(string tagHost,string filePath)
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

        private void btn_Import_Click(object sender, EventArgs e)
        {
            try
            {
                string save_path = "http://" + Server + ":" + Port + "/Attachment/ManualFile";
                
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    string newGuid = Guid.NewGuid().ToString();
                    string filePath = dataGridView1.Rows[i].Cells[1].Value.ToString() + dataGridView1.Rows[i].Cells[2].Value.ToString() + "." + dataGridView1.Rows[i].Cells[3].Value.ToString();
                    
                    //服务器文件名加路径
                    string fileName = "http://" + Server + ":" + Port + "/SJ_WebService.aspx?fileType=manualFile&operation=import&newid=" + newGuid
                       + "." + dataGridView1.Rows[i].Cells[3].Value.ToString();

                   if (PostFileToService.PostFile(fileName, filePath))
                    {
                        string sql = "select nvl(MAX(manual_no),to_char(sysdate, 'yyyymmdd')||'000') from MAC030M join dual on 1 = 1 WHERE manual_no LIKE '" + DateTime.Now.ToString("yyMMdd") + "%'";
                        IDataReader dr = DB.GetDataTableReader(sql);
                        if (dr.Read())
                        {
                            string manual_no = string.Empty;//手册编号
                            manual_no = DateTime.Now.ToString("yyMMdd") + (Convert.ToInt32(dr[0].ToString().Replace(DateTime.Now.ToString("yyMMdd"), "")) + 1).ToString("000");

                            string sqlnowID = @"select nvl(max(id),0)+1 from MAC030M";
                            string nowID = DB.GetString(sqlnowID);                            
                            sql = @"insert into MAC030M(id,manual_no,manual_name,manual_type,manual_Route,suffix_name,state,guid,createby,createdate,createtime)
                             values(" + nowID + ",'" + manual_no + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','" + "保养计划" + "','" + save_path + "','" 
                             + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + "正常" + "','" + newGuid + "','" +
                               Program.UserName + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            DB.ExecuteNonQueryOffline(sql);                           
                        }
                    }
                    else
                    {
                        MessageBox.Show("导入到服务器失败！");
                        return;
                    }
                }
                MessageBox.Show("导入成功！");
                isSuccess = true;
                this.Close();
               
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
