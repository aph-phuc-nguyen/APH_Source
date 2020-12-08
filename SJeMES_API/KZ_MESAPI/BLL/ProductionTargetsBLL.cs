using SJeMES_Framework_NETCore.DBHelper;
using KZ_MESAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    /// <summary>
    /// 用户做生管日计划的业务逻辑控制
    /// </summary>
    public class ProductionTargetsBLL
    {
        /// <summary>
        /// 
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="userId"></param>
        public DataTable UpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB,DataTable dt, string userId,string CompanyCode,string dateFormat,string datetimeFormat)
        {
            ProductionTargetsDAL Dal = new ProductionTargetsDAL();
            //int errorCount = 0;
            DataTable tab = Dal.ValiUpLoad(DB,dt, CompanyCode, dateFormat,  datetimeFormat);
            if (tab.Rows.Count == 0)
            {
               Dal.UpLoad(DB, dt, userId, CompanyCode, dateFormat, datetimeFormat);
            }
            return tab;
        }

        public DataTable Query(DataBase DB, string vSeId, string vDDept, string vArtNo, string vWrokDay,string vEndWrokDay,string vStatus, string vColumn1, string vInOut, string userId, string CompanyCode,string grantUserCode, string dateFormate, string datetimeFormate)
        {
            ProductionTargetsDAL Dal = new ProductionTargetsDAL();
            return Dal.Query(DB, vSeId, vDDept, vArtNo, vWrokDay, vEndWrokDay, vStatus, vColumn1, vInOut,
                    userId, CompanyCode, grantUserCode,  dateFormate,  datetimeFormate);

        }

        public  DataTable QuerySize(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vColumn1, string vInOut, string userId, string CompanyCode, string dateFormate, string datetimeFormate)
        {
            ProductionTargetsDAL Dal = new ProductionTargetsDAL();
            return Dal.QuerySize(DB, vSeId, vDDept, vWrokDay, vColumn1, vInOut,
                 userId, CompanyCode, dateFormate, datetimeFormate);
        }

        public void UpdateStatus(DataBase DB, DataTable dt, string vStatus, string userId, string CompanyCode, string dateFormate, string datetimeFormate)
        {
            ProductionTargetsDAL Dal = new ProductionTargetsDAL();
            Dal.UpdateStatus(DB, dt, vStatus, userId, CompanyCode, dateFormate, datetimeFormate);
        }

        public DataTable LoadSeDept(DataBase DB, string userId, string companyCode)
        {
            ProductionTargetsDAL Dal = new ProductionTargetsDAL();
            return Dal.LoadSeDept(DB,userId, companyCode);
        }
    }
}
