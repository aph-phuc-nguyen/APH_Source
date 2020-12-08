using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class DayActualManuallyDAL
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
        public DataTable GetListArt_No(DataBase DB, string vDArt)
        {
            bool isOk = false;
            string sql1 = @"select distinct ART_NO from MES_LABEL_D";
            if (!String.IsNullOrWhiteSpace(vDArt))
                sql1 += " where ART_NO='" + vDArt + "' ";
            System.Data.DataTable MES_LABEL_D = DB.GetDataTable(sql1);            
            return MES_LABEL_D;
        }


        public void UpLoad(DataBase DB, DataTable dt, string userId, string companyCode, string dateFormat,string dateTimeFormat)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rout_no = "";
                string txt = "";
                string d_dept = dt.Rows[i][0].ToString();
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    txt = dt.Rows[i][j].ToString().Trim() == "" ? "-1," : dt.Rows[i][j].ToString().Trim();
                    decimal qty = Decimal.Parse(txt.Split(',')[0]);
                    if (qty >= 0)
                    {
                        DateTime workDate = Convert.ToDateTime(dt.Columns[j].ColumnName.Trim());
                        string art_no = txt.Split(',')[1];
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
                        string sql2 = @"select * from sjqdms_work_day  " +
                               "where org_id='" + companyCode + "' and  SE_ID='manually' " +
                               "and d_dept='" + d_dept + "' " +
                               "and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";//.ToShortDateString() +
                        System.Data.DataTable sjqdmsWorkActual = DB.GetDataTable(sql2);
                        if (sjqdmsWorkActual.Rows.Count <= 0)
                        {
                            insertSjqdmsWorkDay(DB, companyCode, d_dept, workDate, rout_no, qty, note, thisTime, userId,dateFormat,dateTimeFormat,art_no);
                        }
                        else
                        {
                            updateSjqdmsWorkDay(DB, companyCode, userId, qty, d_dept, workDate, thisTime, note,dateFormat, dateTimeFormat,art_no);
                        }
                        string sql3 = @"select * from mes_label_d  " +
                               "where org_id='" + companyCode + "' and  LABEL_ID='manually' " +
                               "and GRT_DEPT='" + d_dept + "' " +
                               "and SCAN_DATE = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";
                        System.Data.DataTable mes_label_d = DB.GetDataTable(sql3);
                        if (mes_label_d.Rows.Count <= 0)
                        {
                            insertmes_label_d(DB, companyCode, d_dept, workDate, rout_no, qty, note, thisTime, userId, dateFormat, dateTimeFormat,art_no);
                        }
                        else
                        {
                            updatemes_label_d(DB, companyCode, userId, qty, d_dept, workDate, thisTime, note, dateFormat, dateTimeFormat,art_no);
                        }
                    }
                }
            }
        }

        private void insertSjqdmsWorkDay(DataBase DB, string companyCode, string d_dept, 
                    DateTime workDate, string routNo, decimal work_qty, string note, DateTime thisTime, string user,string dateFormat,string dateTimeFormat,string art_no)
        {
            string timeformat = " HH:mm:ss";
            string sql = @"insert into sjqdms_work_day (ORG_ID, SE_ID, SE_SEQ, PO, SE_DAY, COLUMN1, COLUMN2, COLUMN3, COLUMN4," +
                " NOTE, STATUS, GRT_DEPT, GRT_USER, LAST_USER, LAST_DATE, COLUMN5, INSERT_DATE, D_DEPT, WORK_DAY, ROUT_NO, WORK_QTY," +
                " ART_NO, FINISH_QTY, SUPPLEMENT_QTY, INOUT_PZ) " +
                " values('" + companyCode + "', 'manually', 1, 'manually',to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "'), 'U', null, null, null, null, 7,'" + d_dept +
                             "', 'manually', 'manually',to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "'), null,to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "'), '" + d_dept +
                             "', to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "'), '" + routNo + "', '" + work_qty + "', '"+ art_no + "', '" + work_qty + "', 0.00, 'OUT')";
            DB.ExecuteNonQueryOffline(sql);
        }

        private void updateSjqdmsWorkDay(DataBase DB, string companyCode, string user, decimal qty,
                    string d_dept, DateTime workDate, DateTime thisTime, string note,string dateFormat,string dateTimeFormat,string art_no)
        {
            string timeformat=" HH:mm:ss";
            string sql = "update sjqdms_work_day set " +
                "WORK_QTY = " + qty + "," +
                "LAST_USER = '" + user + "'," +
                "NOTE = '" + note + "'," +
                "art_no='"+art_no+"',"+
               "LAST_DATE = to_date('" + thisTime.ToString(dateFormat+ timeformat) + "', '"+ dateTimeFormat + "') " +            
                " WHERE org_id='" + companyCode + "' and " +
                " d_dept='" + d_dept + "' and " +
                " work_day = to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"')";
            DB.ExecuteNonQueryOffline(sql);
        }

        public DataTable QueryActual(DataBase DB, string vDDept, string vWorkDay, string companyCode,string dateFormat,string dateTimeFormat)
        {
            DateTime workDate = DateTime.Parse(vWorkDay);
            string sql = @"select d_dept,to_char(work_day,'"+ dateFormat + "') as work_day,work_qty,rout_no," + 
                "to_char(insert_date,'"+dateTimeFormat+ "') as insert_date,last_user,note,art_no  from sjqdms_work_day where SE_ID='manually' "; //yyyy/mm/dd hh24:mi:ss
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"')";//yyyy/mm/dd
            sql += " and d_dept like '%" + vDDept + "%'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public void UpdateWorkActual(DataBase DB, string vDDept, string vWorkDay, decimal vWorkQty, string vNote, string userId, string companyCode,string dateFormat,string dateTimeFormat,string art_no)
        {
            //DateTime thisTime = DateTime.Now;
            string sql_time = "select sysdate from dual";           
            DateTime thisTime = DB.GetDateTime(sql_time);
            DateTime workDate = DateTime.Parse(vWorkDay);

            updateSjqdmsWorkDay(DB, companyCode, userId, vWorkQty, vDDept, workDate, thisTime, vNote,dateFormat,dateTimeFormat, art_no);
            updatemes_label_d(DB, companyCode, userId, vWorkQty, vDDept, workDate, thisTime, vNote, dateFormat, dateTimeFormat, art_no);
        }       
        private void insertmes_label_d(DataBase DB, string companyCode, string d_dept,
                   DateTime workDate, string routNo, decimal work_qty, string note, DateTime thisTime, string user, string dateFormat, string dateTimeFormat,string art_no)
        {
            string timeformat = " HH:mm:ss";
            string sql = @"insert into mes_label_d (ORG_ID, LABEL_TYPE, LABEL_QTY, INSERT_DATE, LAST_DATE, LABEL_ID, LAST_USER, GRT_USER, GRT_DEPT, STATUS, SCAN_DATE, LABEL_COLUMN1, LABEL_COLUMN2, LABEL_COLUMN3, LABEL_COLUMN4, LABEL_COLUMN5, LABEL_COLUMN6, PROCESS_NO, SCAN_DETPT, SCAN_NAME, EQUIPMENT_NO, SCAN_IP, SIZE_NO, ART_NO, SE_ID, SE_SEQ, PO_NO, INOUT_PZ, SCAN_PZ) " +
              "values ('" + companyCode + "', 'A',  '" + work_qty + "',to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "')," +
              "to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "'), 'manually', '" + user + "'," +
              " '" + user + "','" + d_dept + "', 7, to_date('" + workDate.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "'), " +
              "null, null, null, 0, 0, 0, '" + routNo + "', '" + d_dept + "', 'manually', null, 'manually', '7'," +
              " '"+art_no+"', 'manually', 1, 'manually', 'OUT', 'A')";               
            DB.ExecuteNonQueryOffline(sql);
        }
        private void updatemes_label_d(DataBase DB, string companyCode, string user, decimal qty,
                    string d_dept, DateTime workDate, DateTime thisTime, string note, string dateFormat, string dateTimeFormat,string art_no)
        {
            string timeformat = " HH:mm:ss";
            string sql = "update mes_label_d set " +
                "LABEL_QTY = " + qty + "," +
                "LAST_USER = '" + user + "'," +
                "art_no = '" + art_no + "'," +
               "LAST_DATE = to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "') " +
                " WHERE org_id='" + companyCode + "' and LABEL_ID='manually' and " +
                " SCAN_DETPT='" + d_dept + "' and " +
                " SCAN_DATE = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";
            DB.ExecuteNonQueryOffline(sql);
        }
    }
}


