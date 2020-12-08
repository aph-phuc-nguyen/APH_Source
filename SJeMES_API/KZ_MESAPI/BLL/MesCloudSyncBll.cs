using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class MesCloudSyncBll
    {
        public DataTable GetSyncDBConfig(DataBase DB)
        {
            MesCloudSyncDAL Dal = new MesCloudSyncDAL();
            return Dal.GetSyncDBConfig(DB);
        }

    }
}
