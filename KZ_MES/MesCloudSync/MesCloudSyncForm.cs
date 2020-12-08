using MaterialSkin.Controls;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MesCloudSync
{
    public partial class MesCloudSyncForm : MaterialForm
    {
        delegate void SetTextCallBack(string text, Color color, RichTextBox richText);
        public static DataTable mesToCloud_dt;
        public static DataTable cloudToMes_dt;
        public static ConcurrentDictionary<string, string> syncDic = new ConcurrentDictionary<string, string>();
        private delegate void InvokeHandler();

        public MesCloudSyncForm()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;
            //MES到云  Task的Guid
            syncDic.TryAdd("syncGuid_M01", string.Empty);

            //MES到云 同步是否完成：1完成0同步中
            syncDic.TryAdd("syncFinish_M01", "1");

            //云到MES  Task的Guid
            syncDic.TryAdd("syncGuid_C01", string.Empty);
            syncDic.TryAdd("syncGuid_C02", string.Empty);

            //云到MES 同步是否完成：1完成0同步中
            syncDic.TryAdd("syncFinish_C01", "1");
            syncDic.TryAdd("syncFinish_C02", "1");

            getDBconfig();
            SetDBConfig();
            getIntfaceInfo();
        }

        private void SetText(string text, Color color,RichTextBox richText)
        {
            if (richText.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { text, color, richText });
            }
            else
            {
                //this.richTextBox1.Focus();
                richText.Select(richText.TextLength, 0);
                richText.SelectionColor = color;
                richText.ScrollToCaret();
                richText.AppendText(text + "\n");
            }
        }

        private void getDBconfig()
        {
            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.MesCloudSyncServer", "GetSyncDBConfig", Program.client.UserToken, string.Empty);
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    textServerM.Text = dtJson.Rows[1]["db_server"].ToString();
                    textPortM.Text = dtJson.Rows[1]["db_port"].ToString();
                    textSidM.Text = dtJson.Rows[1]["db_sid"].ToString();
                    textDBNameM.Text = dtJson.Rows[1]["db_name"].ToString();
                    textUserM.Text = dtJson.Rows[1]["db_user"].ToString();
                    textPasswordM.Text = dtJson.Rows[1]["db_password"].ToString();

                    textServerC.Text = dtJson.Rows[0]["db_server"].ToString();
                    textPortC.Text = dtJson.Rows[0]["db_port"].ToString();
                    textSidC.Text = dtJson.Rows[0]["db_sid"].ToString();
                    textDBNameC.Text = dtJson.Rows[0]["db_name"].ToString();
                    textUserC.Text = dtJson.Rows[0]["db_user"].ToString();
                    textPasswordC.Text = dtJson.Rows[0]["db_password"].ToString();
                }
                else
                {
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    SetText(DateTime.Now + "\t 查询同步信息---->发生异常" + ex.InnerException, Color.FromName("Red"), this.richTextBox1);
                    SetText(DateTime.Now + "\t 查询同步信息---->发生异常" + ex.InnerException, Color.FromName("Red"), this.richTextBox2);
                }
                else
                {
                    SetText(DateTime.Now + "\t 查询同步信息---->发生异常" + ex.Message, Color.FromName("Red"), this.richTextBox1);
                    SetText(DateTime.Now + "\t 查询同步信息---->发生异常" + ex.Message, Color.FromName("Red"), this.richTextBox2);
                }
            }
        }

        private void SetDBConfig()
        {
            DbConfig.MESSERVER = textServerM.Text.Trim();
            DbConfig.MESPORT = textPortM.Text.Trim();
            DbConfig.MESSID = textSidM.Text.Trim();
            DbConfig.MESDBNAME = textDBNameM.Text.Trim();
            DbConfig.MESDBUSER = textUserM.Text.Trim();
            DbConfig.MESDBPWD = textPasswordM.Text.Trim();

            DbConfig.CLOUDSERVER = textServerC.Text.Trim();
            DbConfig.CLOUDPORT = textPortC.Text.Trim();
            DbConfig.CLOUDSID = textSidC.Text.Trim();
            DbConfig.CLOUDDBNAME = textDBNameC.Text.Trim();
            DbConfig.CLOUDDBUSER = textUserC.Text.Trim();
            DbConfig.CLOUDDBPWD = textPasswordC.Text.Trim();
        }

        private void getIntfaceInfo()
        {
            try
            {
                LoadingGridView(this.dataGridView1,this.richTextBox1, mesToCloud_dt,"MTC");
                LoadingGridView(this.dataGridView2, this.richTextBox2, cloudToMes_dt,"CTM");
                //LoadingGridView2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 加载GridView1数据
        /// </summary>
        private void LoadingGridView(DataGridView gridView,RichTextBox richTextBox, DataTable dtInfo,string syncType)
        {
            if (dtInfo == null)
            {
                try
                {
                    SJeMES_Framework.DBHelper.DataBase db = new SJeMES_Framework.DBHelper.DataBase("oracle", DbConfig.MESSERVER + ":" + DbConfig.MESPORT, DbConfig.MESDBNAME, DbConfig.MESDBUSER, DbConfig.MESDBPWD, string.Empty);
                    string sql = "select TAB_NAME,LAST_RUNTIME,MSG,ERR_MSG from SYS_SYNC_001M where sync_type = '" + syncType + "'";
                    DataTable dt = db.GetDataTable(sql);
                    dtInfo = dt;
                    gridView.DataSource = dt.DefaultView;
                    gridView.Update();
                }
                catch(Exception ex)
                {
                    SetText(DateTime.Now + "\t 查询同步信息---->发生异常" + ex.Message, Color.FromName("Red"), richTextBox);
                }
            }
            else
            {
                gridView.DataSource = mesToCloud_dt.DefaultView;
                gridView.Update();
            }
        }

        private void butMesToCloud_Click(object sender, EventArgs e)
        {
            LoadingGridView(this.dataGridView1,this.richTextBox1, mesToCloud_dt,"MTC");

            if (this.butMesToCloud.Text == "执行同步")
            {
                //更新同步Guid标识，防止多条线程同时跑
                ConDictionaryAdd("syncGuid_M01", Guid.NewGuid().ToString("N"));
                this.butMesToCloud.Text = "停止同步";
                SetText(DateTime.Now + "\t 执行同步中---->", Color.FromName("Green"), this.richTextBox1);
            }
            else
            {
                //更新同步Guid标识，防止多条线程同时跑
                ConDictionaryAdd("syncGuid_M01", "");
                this.butMesToCloud.Text = "执行同步";
                SetText(DateTime.Now + "\t 已停止同步---->", Color.FromName("Green"), this.richTextBox1);
                return;
            }
            try
            {
                Task t_M01 = new Task(() =>
                {
                    string curSyncGuid = syncDic["syncGuid_M01"];
                    while (!string.IsNullOrEmpty(curSyncGuid) && curSyncGuid == syncDic["syncGuid_M01"])
                    {
                        if (syncDic["syncFinish_M01"] == "0")
                        {
                            Task.Delay(120000).Wait();
                        }
                        else
                        {
                            syncDic["syncFinish_M01"] = "0";
                            CODE003M.DoWork();
                            this.Invoke(new InvokeHandler(delegate () { LoadingGridView(this.dataGridView1,this.richTextBox1, mesToCloud_dt, "MTC"); }));
                            syncDic["syncFinish_M01"] = "1";
                            Task.Delay(120000).Wait();
                        }
                    }
                });
                t_M01.Start();
            }
            catch(Exception ex)
            {
                SetText(DateTime.Now + "\t 自动同步数据---->同步数据发生异常" + ex.Message, Color.FromName("Red"), this.richTextBox1);
            }
        }

        private void butCloudToMes_Click(object sender, EventArgs e)
        {
            LoadingGridView(this.dataGridView2, this.richTextBox2, cloudToMes_dt, "CTM");

            if (this.butCloudToMes.Text == "执行同步")
            {
                //更新同步Guid标识，防止多条线程同时跑
                ConDictionaryAdd("syncGuid_C01", Guid.NewGuid().ToString("N"));
                ConDictionaryAdd("syncGuid_C02", Guid.NewGuid().ToString("N"));
                this.butCloudToMes.Text = "停止同步";
                SetText(DateTime.Now + "\t 执行同步中---->", Color.FromName("Green"),this.richTextBox2);
            }
            else
            {
                //更新同步Guid标识，防止多条线程同时跑
                ConDictionaryAdd("syncGuid_C01", "");
                ConDictionaryAdd("syncGuid_C02", "");
                this.butCloudToMes.Text = "执行同步";
                SetText(DateTime.Now + "\t 已停止同步---->", Color.FromName("Green"), this.richTextBox2);
                return;
            }
            try
            {
                Task t_C01 = new Task(() =>
                {
                    string curSyncGuid = syncDic["syncGuid_C01"];
                    while (!string.IsNullOrEmpty(curSyncGuid) && curSyncGuid == syncDic["syncGuid_C01"])
                    {
                        if (syncDic["syncFinish_C01"] == "0")
                        {
                            Task.Delay(120000).Wait();
                        }
                        else
                        {
                            syncDic["syncFinish_C01"] = "0";
                            SFC_TRACKINFO_LISTM.DoWork();
                            this.Invoke(new InvokeHandler(delegate () { LoadingGridView(this.dataGridView2, this.richTextBox2, cloudToMes_dt, "CTM"); }));
                            syncDic["syncFinish_C01"] = "1";
                            Task.Delay(120000).Wait();
                        }
                    }
                });
                t_C01.Start();

                Task t_C02 = new Task(() =>
                {
                    string curSyncGuid = syncDic["syncGuid_C02"];
                    while (!string.IsNullOrEmpty(curSyncGuid) && curSyncGuid == syncDic["syncGuid_C02"])
                    {
                        if (syncDic["syncFinish_C02"] == "0")
                        {
                            Task.Delay(120000).Wait();
                        }
                        else
                        {
                            syncDic["syncFinish_C02"] = "0";
                            SFC_SUPPLEMENT_LIST.DoWork();
                            this.Invoke(new InvokeHandler(delegate () { LoadingGridView(this.dataGridView2, this.richTextBox2, cloudToMes_dt, "CTM"); }));
                            syncDic["syncFinish_C02"] = "1";
                            Task.Delay(120000).Wait();
                        }
                    }
                });
                t_C02.Start();
            }
            catch (Exception ex)
            {
                SetText(DateTime.Now + "\t 自动同步数据---->同步数据发生异常" + ex.Message, Color.FromName("Red"), this.richTextBox2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">mes到cloud取 M,cloud到mes取 C</param>
        /// <param name="lastRunTime">同步结束时间</param>
        /// <param name="msg">同步信息</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="tabName">同步表名</param>
        public static void DvdtHandle(string type,string lastRunTime, string msg, string errMsg, string tabName)
        {
            DataTable dt = null;
            if ("M".Equals(type))
            {
                dt = MesCloudSyncForm.mesToCloud_dt;
            }
            else if ("C".Equals(type))
            {
                dt = MesCloudSyncForm.cloudToMes_dt;
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    DataRow[] drs = mesToCloud_dt.Select("tab_name = '" + tabName + "'");
                    if (drs.Length > 0)
                    {
                        DataRow dr = drs[0];
                        if (!dr[3].ToString().Equals(errMsg))
                        {

                        }
                        if (!string.IsNullOrEmpty(lastRunTime))
                            dr[1] = lastRunTime;
                        dr[2] = msg;
                        dr[3] = errMsg;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void ConDictionaryAdd(string key, string value)
        {
            if (syncDic.ContainsKey(key))
            {
                string val;
                syncDic.TryRemove(key, out val);
                syncDic.TryAdd(key, value);
            }
            else
            {
                syncDic.TryAdd(key, value);
            }
        }

        private void butEdit_Click(object sender, EventArgs e)
        {
            textServerM.Enabled = true;
            textPortM.Enabled = true;
            textSidM.Enabled = true;
            textDBNameM.Enabled = true;
            textUserM.Enabled = true;
            textPasswordM.Enabled = true;

            textServerC.Enabled = true;
            textPortC.Enabled = true;
            textSidC.Enabled = true;
            textDBNameC.Enabled = true;
            textUserC.Enabled = true;
            textPasswordC.Enabled = true;

            butSave.Enabled = true;
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                SetDBConfig();

                SJeMES_Framework.DBHelper.DataBase db = new SJeMES_Framework.DBHelper.DataBase("oracle", DbConfig.MESSERVER + ":" + DbConfig.MESPORT, DbConfig.MESDBNAME, DbConfig.MESDBUSER, DbConfig.MESDBPWD, string.Empty);

                string sqlMes = "update SYS_CONFIG_CLOUDDB set db_server = '" + textServerM.Text.Trim() + "'," +
                    "db_port = '" + textPortM.Text.Trim() + "'," +
                    "db_sid = '" + textSidM.Text.Trim() + "'," +
                    "db_name = '" + textDBNameM.Text.Trim() + "'," +
                    "db_user = '" + textUserM.Text.Trim() + "'," +
                    "db_password = '" + textPasswordM.Text.Trim() + "'" +
                    " where db_category = 'MES'";

                string sqlCloud = "update SYS_CONFIG_CLOUDDB set db_server = '" + textServerC.Text.Trim() + "'," +
                    "db_port = '" + textPortC.Text.Trim() + "'," +
                    "db_sid = '" + textSidC.Text.Trim() + "'," +
                    "db_name = '" + textDBNameC.Text.Trim() + "'," +
                    "db_user = '" + textUserC.Text.Trim() + "'," +
                    "db_password = '" + textPasswordC.Text.Trim() + "'" +
                    " where db_category = 'CLOUD'";

                db.ExecuteNonQueryOffline(sqlMes);
                db.ExecuteNonQueryOffline(sqlCloud);
                
                textServerM.Enabled = false;
                textPortM.Enabled = false;
                textSidM.Enabled = false;
                textDBNameM.Enabled = false;
                textUserM.Enabled = false;
                textPasswordM.Enabled = false;

                textServerC.Enabled = false;
                textPortC.Enabled = false;
                textSidC.Enabled = false;
                textDBNameC.Enabled = false;
                textUserC.Enabled = false;
                textPasswordC.Enabled = false;

                butSave.Enabled = false;

                MessageBox.Show("修改成功！");
            }
            catch(Exception ex)
            {
                MessageBox.Show("修改失败！" + ex.Message);
            }
        }

        private void butDBTest_MES_Click(object sender, EventArgs e)
        {
            TestDbConn(textServerM.Text.Trim(), textPortM.Text.Trim(), textSidM.Text.Trim(), textUserM.Text.Trim(), textPasswordM.Text.Trim(), textUserM.Text.Trim());
        }

        private void butDBTest_CLOUD_Click(object sender, EventArgs e)
        {
            TestDbConn(textServerC.Text.Trim(), textPortC.Text.Trim(), textSidC.Text.Trim(), textUserC.Text.Trim(), textPasswordC.Text.Trim(), textUserC.Text.Trim());
        }

        private void TestDbConn(string host,string port,string sid,string user,string password,string DBName)
        {
            string connectionString2 = string.Concat(new string[]
                    {
                        "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=",
                        host,
                        ")(PORT=",
                        port,
                        ")))(CONNECT_DATA=(SID = ",
                        sid,
                        ")));User Id=",
                        user,
                        ";Password=",
                        password,
                        ";"
                    });
            OracleConnection oracleConn = new OracleConnection(connectionString2);
            try
            {
                oracleConn.Open();
                oracleConn.Close();
                MessageBox.Show("DB[" + DBName + "] 测试连接 OK!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (oracleConn != null)
                {
                    ((IDisposable)oracleConn).Dispose();
                }
            }
        }
    }
}
