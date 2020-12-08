using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class ProductionReportBLL
    {
        public DataTable GetWorkDaySizeReport(DataBase DB, string CompanyCode, string vSeId, string vPO, string vDept, string vArt, string vInOut)
        {
            ProductionReportDAL Dal = new ProductionReportDAL();
            return Dal.GetWorkDaySizeReport(DB, CompanyCode, vSeId, vPO, vDept, vArt, vInOut);
        }
        public DataTable GetWorkDayReport(DataBase DB, string CompanyCode, string vSeId, string vWrokDay, string vDept, string vPO, string vArt, string vStatus, string vInOut)
        {
            ProductionReportDAL Dal = new ProductionReportDAL();
            return Dal.GetWorkDayReport(DB, CompanyCode, vSeId, vWrokDay, vDept, vPO, vArt, vStatus, vInOut);
        }
        public DataTable GetMesLableDReport(DataBase DB, string CompanyCode, string vSeId, string vPO, string vWrokDay, string vDept, string vArt)
        {
            ProductionReportDAL Dal = new ProductionReportDAL();
            return Dal.GetMesLableDReport(DB, CompanyCode, vSeId, vPO, vWrokDay, vDept, vArt);
        }
    }
}
