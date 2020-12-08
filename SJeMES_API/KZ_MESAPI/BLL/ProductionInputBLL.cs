using SJeMES_Framework_NETCore.DBHelper;
using KZ_MESAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class ProductionInputBLL
    {                       
        public DataTable GetSeSize(DataBase DB, string CompanyCode, string vDDept, string vSeId, string dateFormat)
        {
            ProductionInputDAL Dal = new ProductionInputDAL();
            return Dal.GetSeSize(DB, CompanyCode, vDDept, vSeId, dateFormat);
        }

        public void updateFinshQty(DataBase DB, string CompanyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal vQty, string userId, string vIP, string dateFormat, string dateTimeFormat)
        {
            ProductionInputDAL Dal = new ProductionInputDAL();
            Dal.updateFinshQty(DB, CompanyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, vIP,dateFormat, dateTimeFormat);
        }
    }
}
