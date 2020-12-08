using SJeMES_Framework_NETCore.DBHelper;
using KZ_MESAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    class ImportInnerBoxBLL
    {
        public void UpLoad(DataBase DB, DataTable dt, string userCode, string companyCode, string dateFormat, string dateTimeFormat)
        {
            ImportInnerBoxDAL Dal = new ImportInnerBoxDAL();
            Dal.UpLoad(DB, dt, userCode, Organization.getValue(companyCode), dateFormat,dateTimeFormat);
        }

        public DataTable Query(DataBase DB, string date, string userCode, string companyCode, string dateFormat, string dateTimeFormat)
        {
            ImportInnerBoxDAL Dal = new ImportInnerBoxDAL();
            return Dal.Query(DB,date, userCode, Organization.getValue(companyCode), dateFormat, dateTimeFormat);
        }
    }
}
