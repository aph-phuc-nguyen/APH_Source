using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesCloudSync
{
    public class CODE003M
    {
        public static void DoWork()
        {
            string tabName = "CODE003M";
            //MES上传数据库信息初始化
            SJeMES_Framework.DBHelper.DataBase mesDB = new SJeMES_Framework.DBHelper.DataBase("oracle", DbConfig.MESSERVER + ":" + DbConfig.MESPORT, DbConfig.MESDBNAME, DbConfig.MESDBUSER, DbConfig.MESDBPWD, string.Empty);
            //云接收数据库信息初始化
            //GDSJ_Framework.DBHelper.DataBase CloudDB = new GDSJ_Framework.DBHelper.DataBase("oracle", Program.CLOUDSERVER + ":" + Program.CLOUDPORT, Program.CLOUDDBNAME, Program.CLOUDDBUSER, Program.CLOUDDBPWD, string.Empty);
            
            //云接收数据库连接串
            string conStr = @"Data Source = (DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = " + DbConfig.CLOUDSERVER + ")(PORT = " + DbConfig.CLOUDPORT + "))(CONNECT_DATA = (SID = " + DbConfig.CLOUDSID + ")));User Id=" + DbConfig.CLOUDDBUSER + "; Password=" + DbConfig.CLOUDDBPWD;
            try
            {
                DateTime dateStart = DateTime.Now;
                //string LastRunTime = mesDB.GetString(@"select LAST_RUNTIME from SYS_SYNC_001M where TAB_NAME='" + tabName + "'");
                //if (string.IsNullOrEmpty(LastRunTime))
                //{
                //    LastRunTime = " ";
                //}
                DateTime sysDateStart = mesDB.GetDateTime("select sysdate from dual");
                string sql = "select * from " + tabName + " where UDF02 = 'N'";
                System.Data.DataTable dt = mesDB.GetDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    using (OracleConnection conn = new OracleConnection(conStr))
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand("select * from " + tabName + " where 1 = 2", conn);
                            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                            OracleCommandBuilder cb = new OracleCommandBuilder(adapter);
                            DataTable dsNew = new DataTable();
                            int count = adapter.Fill(dsNew);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dsNew.NewRow();
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    dr[dsNew.Columns[j].ColumnName] = dt.Rows[i][j];
                                }
                                dsNew.Rows.Add(dr);
                            }
                            count = adapter.Update(dsNew);
                            adapter.UpdateBatchSize = 500;
                            //adapter.Update(dataTable);

                            DateTime dateEnd = DateTime.Now;
                            string msg = dateStart.ToString("HH:mm:ss") + "->" + dateEnd.ToString("HH:mm:ss") + " : 上传数据" + dt.Rows.Count + @"条,耗时" + dateEnd.Subtract(dateStart).TotalSeconds + "秒";
                            mesDB.ExecuteNonQueryOffline(@"update SYS_SYNC_001M set last_runtime = '" + sysDateStart.ToString("yyyyMMddHHmmss") + @"',msg='" + msg + @"',err_msg='' where tab_name='" + tabName + "'");

                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                string updataSql = @"update CODE003M set UDF02 = 'Y' where UDF02 = 'N'";
                                updataSql += " and PACKING_BARCODE ='" + dr["packing_barcode"] + "'";
                                mesDB.ExecuteNonQueryOffline(updataSql);
                            }

                            MesCloudSyncForm.DvdtHandle("M", sysDateStart.ToString("yyyyMMddHHmmss"), msg, "", tabName);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                mesDB.ExecuteNonQueryOffline(@"update SYS_SYNC_001M set err_msg = '" + ex.Message + "' where tab_name = '" + tabName + "'");
                            }
                            catch { }

                            MesCloudSyncForm.DvdtHandle("M", "", "", ex.Message, tabName);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                try
                {
                    mesDB.ExecuteNonQueryOffline(@"update SYS_SYNC_001M set err_msg = '" + ex.Message + "' where tab_name = '" + tabName + "'");
                }
                catch { }

                MesCloudSyncForm.DvdtHandle("M", "", "", ex.Message, tabName);
            }
        }
    }
}
