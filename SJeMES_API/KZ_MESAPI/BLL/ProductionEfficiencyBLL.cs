using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class ProductionEfficiencyBLL
    {
        public int UpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string CompanyCode,string dateFormat)
        {
            ProductionEfficiencyDAL Dal = new ProductionEfficiencyDAL();
            int errorCount = 0;
            errorCount = Dal.ValiUpLoad(DB, dt, CompanyCode);
            if (errorCount == 0)
            {
                Dal.UpLoad(DB, dt, userId, CompanyCode, dateFormat);
            }
            return errorCount;
        }

        public DataTable QueryNumber(DataBase DB, string CompanyCode, string vDept, string vWrokDay, string dateFormat)
        {
            ProductionEfficiencyDAL Dal = new ProductionEfficiencyDAL();
            return Dal.QueryNumber(DB, CompanyCode, vDept, vWrokDay, dateFormat);
        }       
        public DataTable QueryWorkDay(DataBase DB, DateTime date, int numberday, string dateFormat)
        {
            ProductionEfficiencyDAL Dal = new ProductionEfficiencyDAL();
            return Dal.QueryWorkDay(DB, date, numberday,dateFormat);
        }
    }
}
