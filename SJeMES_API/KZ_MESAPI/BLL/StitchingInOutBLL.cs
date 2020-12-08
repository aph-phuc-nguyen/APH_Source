using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class StitchingInOutBLL
    {
        public DataTable GetOrdM(DataBase DB, string CompanyCode)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            return Dal.GetOrdM(DB, CompanyCode);
        }

        public int GetDayFinishQty(DataBase DB, string CompanyCode, string vDDept)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            return Dal.GetDayFinishQty(DB, CompanyCode, vDDept);
        }

        public DataTable GetSeSize(DataBase DB, string CompanyCode, string vSeId)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            return Dal.GetSeSize(DB, CompanyCode, vSeId);
        }

        public DataTable GetSeSizeDetail(DataBase DB, string CompanyCode, string vSeId, string vSeSeq, string vSizeNo)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            return Dal.GetSeSizeDetail(DB, CompanyCode, vSeId, vSeSeq, vSizeNo);
        }

        public bool ValisStitchingDept(DataBase DB, string vDDept, string vRoutNo)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            return Dal.ValisStitchingDept(DB, vDDept, vRoutNo);
        }

        public void updateInFinshQty(DataBase DB, string CompanyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vSizeSeq, decimal vQty, string userId, string vIP, string vPO, string vArtNo, string vSeDay)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            Dal.updateInFinshQty(DB, CompanyCode, vDDept, vSeId, vSeSeq, vSizeNo, vSizeSeq, vQty, userId, vIP, vPO, vArtNo, vSeDay);
        }

        public int GeSizeFinishQty(DataBase DB, string vOrgId, string vSeId, string vSeSeq, string vSizeNo)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            return Dal.GeSizeFinishQty(DB, vOrgId, vSeId, vSeSeq, vSizeNo);
        }

        public void updateOutFinshQty(DataBase DB, string CompanyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal vQty, string userId, string vIP)
        {
            StitchingInOutDAL Dal = new StitchingInOutDAL();
            Dal.updateOutFinshQty(DB, CompanyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, vIP);
        }
    }
}




