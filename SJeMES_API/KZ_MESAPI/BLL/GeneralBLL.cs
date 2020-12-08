using SJeMES_Framework_NETCore.DBHelper;
using KZ_MESAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class GeneralBLL
    {
        public DataTable GetDept(DataBase DB, string userId)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetDept(DB, userId);
        }

        public int GetDayFinishQty(DataBase DB, string CompanyCode, string vDDept, string vInOut, string dateFormat)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetDayFinishQty(DB, CompanyCode, vDDept, vInOut, dateFormat);
        }

        public DataTable GetSeId_Po(DataBase DB, string CompanyCode, string vDDept, string vInOut, string dateFormat)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetSeId_Po(DB, CompanyCode, vDDept, vInOut, dateFormat);
        }

        public string GetArtName(DataBase DB, string companyCode, string vArtNo)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetArtName(DB, companyCode, vArtNo);
        }

        public string GetPoBySeid(DataBase DB, string vSeId)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetPoBySeid(DB, vSeId);
        }

        public DataTable GetAllDepts(DataBase DB)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetAllDepts(DB);
        }

        public DataTable GetWorkDaySize(DataBase DB, string CompanyCode, string vDDept, string vInOut, string vSeId, string dateFormat)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetWorkDaySize(DB, CompanyCode, vDDept, vInOut, vSeId, dateFormat);
        }

        public int GetDaySizeWorkQty(DataBase DB, string CompanyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vInOut, string dateFormat)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetDaySizeWorkQty(DB, CompanyCode, vDDept, vSeId, vSeSeq, vSizeNo, vInOut, dateFormat);
        }

        public int GetDaySizeFinishQty(DataBase DB, string CompanyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vInOut, string dateFormat)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetDaySizeFinishQty(DB, CompanyCode, vDDept, vSeId, vSeSeq, vSizeNo, vInOut, dateFormat);
        }

        public DataTable GetInOutDetail(DataBase DB, string CompanyCode, string vDDept, string vPo, string vSeId)
        {
            GeneralDAL Dal = new GeneralDAL();
            return Dal.GetInOutDetail(DB, CompanyCode, vDDept, vPo, vSeId);
        }
    }  
}


