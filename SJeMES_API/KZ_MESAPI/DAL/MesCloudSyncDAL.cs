using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class MesCloudSyncDAL
    {
        public DataTable GetSyncDBConfig(DataBase DB)
        {
            string sql = "select * from SYS_CONFIG_CLOUDDB order by db_category asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
    }
}
