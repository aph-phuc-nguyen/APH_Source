using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class DayActualManuallyBLL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dt"></param>
        /// <param name="userId"></param>
        /// <param name="CompanyCode"></param>
        /// <returns></returns>
        public void UpLoad(DataBase DB, DataTable dt, string userId, string CompanyCode, string dateFormat, string dateTimeFormat)
        {
            DayActualManuallyDAL Dal = new DayActualManuallyDAL();
            Dal.UpLoad(DB, dt, userId, CompanyCode, dateFormat, dateTimeFormat);
        }

        public bool ValiDept(DataBase DB, string vDDept, string CompanyCode)
        {
            DayActualManuallyDAL Dal = new DayActualManuallyDAL();
            return Dal.ValiDept(DB, vDDept, CompanyCode);
        }

        public DataTable GetListArt_No(DataBase DB, string vDDept)
        {
            DayActualManuallyDAL Dal = new DayActualManuallyDAL();
            return Dal.GetListArt_No(DB, vDDept);
        }
        public DataTable QueryActual(DataBase DB, string vDDept, string vWorkDay, string CompanyCode, string dateFormat, string dateTimeFormat)
        {
            DayActualManuallyDAL Dal = new DayActualManuallyDAL();
            return Dal.QueryActual(DB, vDDept, vWorkDay, CompanyCode, dateFormat, dateTimeFormat);
        }

        public void UpdateWorkActual(DataBase DB, string vDDept, string vWorkDay, decimal vWorkQty, string vNote, string userId, string CompanyCode, string dateFormat, string dateTimeFormat, string art_no)
        {
            DayActualManuallyDAL Dal = new DayActualManuallyDAL();
            Dal.UpdateWorkActual(DB, vDDept, vWorkDay, vWorkQty, vNote, userId, CompanyCode, dateFormat, dateTimeFormat, art_no);
        }

    }
}


