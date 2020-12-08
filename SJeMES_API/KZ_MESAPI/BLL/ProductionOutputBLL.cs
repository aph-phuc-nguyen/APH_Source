using SJeMES_Framework_NETCore.DBHelper;
using KZ_MESAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class ProductionOutputBLL
    {
        public int GetHourQty(DataBase DB, string CompanyCode, string vDDept, string vInOut, string dateFormat, string dateTimeFormat)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetHourQty(DB, CompanyCode, vDDept, vInOut);
        }

        public DataTable GetScanLog(DataBase DB, string CompanyCode, string vDDept, string vInOut, string dateFormat, string dateTimeFormat)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetScanLog(DB, CompanyCode, vDDept, vInOut);
        }

        public int GetSeFinishQty(DataBase DB, string CompanyCode, string vDDept, string vInOut, string vSeId)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetSeFinishQty(DB, CompanyCode, vDDept, vInOut, vSeId);
        }

        public int GetSeQty(DataBase DB, string CompanyCode, string vDDept, string vInOut, string vSeId)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetSeQty(DB, CompanyCode, vDDept, vInOut, vSeId); 
        }

        public DataTable GetOutDetail(DataBase DB, string CompanyCode, string vDDept, string vInOut, string vSeId, string vSizeNo)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetOutDetail(DB, CompanyCode, vDDept, vInOut, vSeId, vSizeNo); 
        }

        public DataTable GetLabelDetail(DataBase DB, string companyCode, string vLabel)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetLabelDetail(DB, companyCode, vLabel);
        }

        public int GetUnfinishQty(DataBase DB, string companyCode, string vSeId, string vSizeNo, string vDDept, string vInOut , string dateFormat, string dateTimeFormat)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetUnfinishQty(DB, companyCode, vSeId, vSizeNo, vDDept, vInOut, dateFormat, dateTimeFormat);
        }

        public void updateOutFinshQty(DataBase DB, string CompanyCode, string vDDept, string vSeId, string vSizeNo, string userId, string vIP, string vLabel, string vScanPZ, string dateFormat, string dateTimeFormat)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            Dal.updateOutFinshQty(DB, CompanyCode, vDDept, vSeId, vSizeNo, userId, vIP, vLabel, vScanPZ, dateFormat, dateTimeFormat);
        }

        public DataTable GetScanDept(DataBase DB)
        {
            ProductionOutputDAL Dal = new ProductionOutputDAL();
            return Dal.GetScanDept(DB);
        }
    }
}



