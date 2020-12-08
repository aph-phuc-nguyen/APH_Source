using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class DayTargetsBLL
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
            DayTargetsDAL Dal = new DayTargetsDAL();
            Dal.UpLoad(DB, dt, userId, CompanyCode, dateFormat, dateTimeFormat);
        }

        public bool ValiDept(DataBase DB, string vDDept, string CompanyCode)
        {
            DayTargetsDAL Dal = new DayTargetsDAL();
            return Dal.ValiDept(DB, vDDept, CompanyCode);
        }

        public DataTable QueryTargets(DataBase DB, string vDDept, string vWorkDay, string CompanyCode, string dateFormat, string dateTimeFormat)
        {
            DayTargetsDAL Dal = new DayTargetsDAL();
            return Dal.QueryTargets(DB, vDDept, vWorkDay, CompanyCode, dateFormat, dateTimeFormat);
        }

        public void UpdateWorkTarget(DataBase DB, string vDDept, string vWorkDay, decimal vWorkQty, string vNote, string userId, string CompanyCode, string dateFormat, string dateTimeFormat)
        {
            DayTargetsDAL Dal = new DayTargetsDAL();
            Dal.UpdateWorkTarget(DB, vDDept, vWorkDay, vWorkQty, vNote, userId, CompanyCode, dateFormat, dateTimeFormat);
        }

    }
}


