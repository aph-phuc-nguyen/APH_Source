using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class DayTargetsDAL
    {
        /// <summary>
        /// -------该功能主要执行验证上传的日产量目标-------
        /// 1:验证组别资料
        /// 2:
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dt"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public bool ValiDept(DataBase DB, string vDDept, string companyCode)
        {
            bool isOk = false;
            string sql1 = @"select * from BASE005M " +
                "where  department_code='" + vDDept + "'";
            System.Data.DataTable base005M = DB.GetDataTable(sql1);
            if (base005M.Rows.Count > 0)
            {
                isOk = true;
            }
            return isOk;
        }

        public void UpLoad(DataBase DB, DataTable dt, string userId, string companyCode, string dateFormat, string dateTimeFormat)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rout_no = "";
                string d_dept = dt.Rows[i][0].ToString();
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    decimal qty = dt.Rows[i][j].ToString().Trim() == "" ? -1 : Decimal.Parse(dt.Rows[i][j].ToString().Trim());
                    if (qty >= 0)
                    {
                        DateTime workDate = Convert.ToDateTime(dt.Columns[j].ColumnName.Trim());
                        string note = "";
                        string sql_time = "select sysdate from dual";
                        DateTime thisTime = DB.GetDateTime(sql_time);
                        string sql1 = @"select * from BASE005M " +
                            "where  department_code='" + d_dept + "'";
                        System.Data.DataTable base005M = DB.GetDataTable(sql1);
                        if (base005M.Rows.Count > 0)
                        {
                            rout_no = base005M.Rows[0]["UDF01"].ToString();
                        }
                        string sql2 = @"select * from SJQDMS_WORKTARGET  " +
                               "where org_id='" + companyCode + "' " +
                               "and d_dept='" + d_dept + "' " +
                               "and work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";//.ToShortDateString() +
                        System.Data.DataTable sjqdmsWorkTarget = DB.GetDataTable(sql2);
                        if (sjqdmsWorkTarget.Rows.Count <= 0)
                        {
                            insertSjqdmsWorkTarget(DB, companyCode, d_dept, workDate, rout_no, qty, note, thisTime, userId, dateFormat, dateTimeFormat);
                        }
                        else
                        {
                            updateSjqdmsWorkDay(DB, companyCode, userId, qty, d_dept, workDate, thisTime, note, dateFormat, dateTimeFormat);
                        }
                    }
                }
            }
        }

        private void insertSjqdmsWorkTarget(DataBase DB, string companyCode, string d_dept,
                    DateTime workDate, string routNo, decimal work_qty, string note, DateTime thisTime, string user, string dateFormat, string dateTimeFormat)
        {
            string timeformat = " HH:mm:ss";
            string sql = @"insert into SJQDMS_WORKTARGET(org_id,d_dept,work_day,rout_no,work_qty,note,status,
                             insert_date,grt_user,grt_dept,last_user,last_date) values('" + companyCode + "','" + d_dept +
                             "',to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "'),'" + routNo + "','" + work_qty + "','" +
                             note + "','7',to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "'),'" + user + "','" + d_dept + "','" + user + "',to_date('" + thisTime.ToString(dateFormat + timeformat) + "','" + dateTimeFormat + "'))";//yyyy/mm/dd HH24:MI:SS

            DB.ExecuteNonQueryOffline(sql);
        }

        private void updateSjqdmsWorkDay(DataBase DB, string companyCode, string user, decimal qty,
                    string d_dept, DateTime workDate, DateTime thisTime, string note, string dateFormat, string dateTimeFormat)
        {
            string timeformat = " HH:mm:ss";
            string sql = "update SJQDMS_WORKTARGET set " +
                "WORK_QTY = " + qty + "," +
                "LAST_USER = '" + user + "'," +
                "NOTE = '" + note + "'," +
               "LAST_DATE = to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "') " +//yyyy/mm/dd HH24:MI:SS             
                " WHERE org_id='" + companyCode + "' and " +
                " d_dept='" + d_dept + "' and " +
                " work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";//yyyy/mm/dd
            DB.ExecuteNonQueryOffline(sql);
        }

        public DataTable QueryTargets(DataBase DB, string vDDept, string vWorkDay, string companyCode, string dateFormat, string dateTimeFormat)
        {
            DateTime workDate = DateTime.Parse(vWorkDay);
            string sql = @"select d_dept,to_char(work_day,'" + dateFormat + "') as work_day,work_qty,rout_no," +
                "to_char(insert_date,'" + dateTimeFormat + "') as insert_date,last_user,note from SJQDMS_WORKTARGET where 1 = 1"; //yyyy/mm/dd hh24:mi:ss
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";//yyyy/mm/dd
            sql += " and d_dept like '%" + vDDept + "%'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public void UpdateWorkTarget(DataBase DB, string vDDept, string vWorkDay, decimal vWorkQty, string vNote, string userId, string companyCode, string dateFormat, string dateTimeFormat)
        {
            //DateTime thisTime = DateTime.Now;
            string sql_time = "select sysdate from dual";
            DateTime thisTime = DB.GetDateTime(sql_time);
            DateTime workDate = DateTime.Parse(vWorkDay);
            updateSjqdmsWorkDay(DB, companyCode, userId, vWorkQty, vDDept, workDate, thisTime, vNote, dateFormat, dateTimeFormat);
        }
    }
}


