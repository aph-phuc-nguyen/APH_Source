using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class WorkingHoursBLL
    {

        public DataTable LoadSeDept(DataBase DB, string vDDept, string userId, string CompanyCode)
        {
            WorkingHoursDAL Dal = new WorkingHoursDAL();
            return Dal.LoadSeDept(DB, vDDept, userId, CompanyCode);
        }

        public DataTable Query(DataBase DB, string orgNo, string plantNo, string deptNo, string routNo, string userCode, string companyCode)
        {
            WorkingHoursDAL dal = new WorkingHoursDAL();
            return dal.Query(DB, orgNo, plantNo, deptNo, routNo, userCode, companyCode);
        }
        public DataTable GetWorkingHoursData(DataBase DB, string orgNo, string plantNo, string deptNo, string routNo, string workDate, string userCode, string companyCode, string dateFormat)
        {
            WorkingHoursDAL dal = new WorkingHoursDAL();
            return dal.GetWorkingHoursData(DB, orgNo, plantNo, deptNo, routNo, workDate, userCode, companyCode, dateFormat);
        }

        public void Save(DataBase DB, DataTable dataTable, string vfrom, string vto, string mon_am_from, string mon_am_to, string mon_pm_from, string mon_pm_to, string tue_am_from, string tue_am_to, string tue_pm_from, string tue_pm_to, string wed_am_from, string wed_am_to, string wed_pm_from, string wed_pm_to, string thu_am_from, string thu_am_to, string thu_pm_from, string thu_pm_to, string fri_am_from, string fri_am_to, string fri_pm_from, string fri_pm_to, string sat_am_from, string sat_am_to, string sat_pm_from, string sat_pm_to, string sun_am_from, string sun_am_to, string sun_pm_from, string sun_pm_to, string userCode, string companyCode, string dateFormat)
        {

            WorkingHoursDAL dal = new WorkingHoursDAL();
            try
            {
                DB.Open();
                DB.BeginTransaction();
                dal.Save(DB, dataTable, vfrom, vto,
                mon_am_from, mon_am_to, mon_pm_from, mon_pm_to,
                tue_am_from, tue_am_to, tue_pm_from, tue_pm_to,
                wed_am_from, wed_am_to, wed_pm_from, wed_pm_to,
                thu_am_from, thu_am_to, thu_pm_from, thu_pm_to,
                fri_am_from, fri_am_to, fri_pm_from, fri_pm_to,
                sat_am_from, sat_am_to, sat_pm_from, sat_pm_to,
                sun_am_from, sun_am_to, sun_pm_from, sun_pm_to,
                userCode, companyCode, dateFormat);
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

        public string LoadMoveNo(DataBase DB)
        {
            WorkingHoursDAL Dal = new WorkingHoursDAL();
            return Dal.LoadMoveNo(DB);
        }

        public bool SaveTransTime(DataBase DB, string vMoveNo, string vMoveDate, string vDDept, string vWrokDay, string vTransOutTime, string vTransInTime, string userCode, string companyCode, string dateFormat)
        {
            WorkingHoursDAL dal = new WorkingHoursDAL();
            try
            {
                DB.Open();
                DB.BeginTransaction();
                ///部分线还没上线，没办法做判断
                //if (!dal.QueryWorkingHours(DB, vNewDdept, vWrokDay))
                //{
                //    return false;
                //}
                //dal.InsertMesMoveWorkingHours(DB, vMoveNo, vMoveDate, vDDept, vWrokDay, vNewDdept, vTransTime, userCode, companyCode);
                dal.UpdateWorkingHours(DB, vDDept, vWrokDay, vTransOutTime, vTransInTime, userCode, companyCode, dateFormat);
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
            return true;
        }
    }
}
