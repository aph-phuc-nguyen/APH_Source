using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class PlanAdjustmentBLL
    {
        public DataTable QuerySize(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vInOut, string userId, string CompanyCode, string dateFormat, string datetimeFormat)
        {
            PlanAdjustmentDAL Dal = new PlanAdjustmentDAL();
            return Dal.QuerySize(DB, vSeId, vDDept, vWrokDay, vInOut,
                 userId, CompanyCode, dateFormat, datetimeFormat);
        }

        public DataTable LoadSeDept(DataBase DB, string vDDept, string vRoutNo, string userId, string CompanyCode)
        {
            PlanAdjustmentDAL Dal = new PlanAdjustmentDAL();
            return Dal.LoadSeDept(DB,vDDept,vRoutNo,userId, CompanyCode);
        }

        public string LoadMoveNo(DataBase DB)
        {
            PlanAdjustmentDAL Dal = new PlanAdjustmentDAL();
            return Dal.LoadMoveNo(DB);
        }

        public void Save(DataBase DB, string vMoveNo,string  vMoveDate, string vSeId, string vDDept, string vWrokDay, string vInOut, string vNewWorkDay, string vNewDdept, DataTable dataTable, string userCode, string companyCode, string dateFormat, string datetimeFormat)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                PlanAdjustmentDAL Dal = new PlanAdjustmentDAL();
                Dal.InsertMesMoveM(DB, vMoveNo, vMoveDate,vSeId, vDDept, vWrokDay, vInOut, vNewWorkDay, vNewDdept, dataTable, userCode, companyCode, dateFormat, datetimeFormat);
                Dal.InsertMesMoveD(DB, vMoveNo, vMoveDate, vSeId, vDDept, vWrokDay, vInOut, vNewWorkDay, vNewDdept, dataTable, userCode, companyCode, dateFormat, datetimeFormat);
                Dal.UpdateSjqmdsWorkDay(DB, vSeId, vDDept, vWrokDay, vInOut, vNewWorkDay, vNewDdept, dataTable, userCode, companyCode, dateFormat, datetimeFormat);
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }  
        }
    }
}
