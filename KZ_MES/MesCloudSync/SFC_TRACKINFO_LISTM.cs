using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MesCloudSync
{
    public class SFC_TRACKINFO_LISTM
    {
        public static void DoWork()
        {
            string tabName = "SFC_TRACKINFO_LISTM";
            //MES接受数据库信息初始化
            SJeMES_Framework.DBHelper.DataBase mesDB = new SJeMES_Framework.DBHelper.DataBase("oracle", DbConfig.MESSERVER + ":" + DbConfig.MESPORT, DbConfig.MESDBNAME, DbConfig.MESDBUSER, DbConfig.MESDBPWD, string.Empty);
            //云上传数据库信息初始化
            SJeMES_Framework.DBHelper.DataBase CloudDB = new SJeMES_Framework.DBHelper.DataBase("oracle", DbConfig.CLOUDSERVER + ":" + DbConfig.CLOUDPORT, DbConfig.CLOUDDBNAME, DbConfig.CLOUDDBUSER, DbConfig.CLOUDDBPWD, string.Empty);
            
            //MES接受数据库连接串
            string conStr = @"Data Source = (DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = " + DbConfig.MESSERVER + ")(PORT = " + DbConfig.MESPORT + "))(CONNECT_DATA = (SID = " + DbConfig.MESSID + ")));User Id=" + DbConfig.MESDBUSER + "; Password=" + DbConfig.MESDBPWD;
            //Data Source = (DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.2.1.47)(PORT = 1521))(CONNECT_DATA = (SID = APEMES)));User Id=mes00; Password=dbmes00
            try
            {
                DateTime dateStart = DateTime.Now;
                //string LastRunTime = mesDB.GetString(@"select LAST_RUNTIME from SYS_SYNC_001M where TAB_NAME='" + tabName + "'");
                //if (string.IsNullOrEmpty(LastRunTime))
                //{
                //    LastRunTime = " ";
                //}
                DateTime sysDateStart = mesDB.GetDateTime("select sysdate from dual");
                //DateTime thisTime = CloudDB.GetDateTime("select sysdate from dual");
                string sql = "select * from " + tabName + " where issync = 'N'";
                System.Data.DataTable dt = CloudDB.GetDataTable("select * from " + tabName + " where issync = 'N'");

                if (dt.Rows.Count > 0)
                {
                    using (OracleConnection conn = new OracleConnection(conStr))
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand("select * from " + tabName + " where 1 = 2", conn);
                            //OracleCommand cmd = new OracleCommand("select * from SFC_TRACKINFO_LISTM_T where 1 = 2", conn);
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
                                string updataSql = @"update SFC_TRACKINFO_LISTM set issync = 'Y' where issync = 'N'";
                                updataSql += " and barcode ='" + dr["barcode"] + "'";
                                updataSql += " and company_code ='" + dr["company_code"] + "'";
                                updataSql += " and scan_type ='" + dr["scan_type"] + "'";
                                updataSql += " and scan_date = to_date('" + dr["scan_date"] + "','yyyy/mm/dd hh24:mi:ss')";
                                CloudDB.ExecuteNonQueryOffline(updataSql);
                            }

                            MesCloudSyncForm.DvdtHandle("C", sysDateStart.ToString("yyyyMMddHHmmss"), msg, "", tabName);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                mesDB.ExecuteNonQueryOffline(@"update SYS_SYNC_001M set err_msg = '" + ex.Message + "' where tab_name = '" + tabName + "'");
                            }
                            catch { }

                            MesCloudSyncForm.DvdtHandle("C", "", "", ex.Message, tabName);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                try
                {
                    mesDB.ExecuteNonQueryOffline(@"update SYS_SYNC_001M set err_msg = '" + ex.Message + "' where tab_name = '" + tabName + "'");
                }
                catch { }

                MesCloudSyncForm.DvdtHandle("C", "", "", ex.Message, tabName);
            }
        }
    }
}
