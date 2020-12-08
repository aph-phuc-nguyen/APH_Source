using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class ProductionDashBoardBLL
    {
        public DataTable Query(DataBase DB, string userCode, string companyCode,string vCompany, string vPlant,string  vDept, string vRountNo)
        {
            ProductionDashBoardDAL Dal = new ProductionDashBoardDAL();
            return Dal.Query(DB, userCode, companyCode, vCompany, vPlant, vDept, vRountNo);
        }

        public DataTable LoadOrg(DataBase DB, string userCode, string companyCode)
        {
            ProductionDashBoardDAL Dal = new ProductionDashBoardDAL();
            return Dal.LoadOrg(DB, userCode, companyCode);
        }

        public DataTable LoadPlant(DataBase DB, string userCode, string companyCode)
        {
            ProductionDashBoardDAL Dal = new ProductionDashBoardDAL();
            return Dal.LoadPlant(DB, userCode, companyCode);
        }

        public DataTable LoadRoutNo(DataBase DB, string userCode, string companyCode)
        {
            ProductionDashBoardDAL Dal = new ProductionDashBoardDAL();
            return Dal.LoadRoutNo(DB, userCode, companyCode);
        }
    }
}
