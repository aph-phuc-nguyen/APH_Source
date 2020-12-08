using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SJeMES_Framework_NETCore.DBHelper;

namespace KZ_MESAPI.DAL
{
    public class AutoGenerationSchedulingDAL
    {
        public DataTable Query(DataBase DB)
        {
            string sql = "select * from VW_SJQDMS_WORK_DAY_SIZE_AUTO where work_qty>0";
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 用于把当天未做完的计划转移到第二天
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dt"></param>
        /// <param name="userId"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public int Insert(DataBase DB, DataTable dt, string userId, string companyCode, string dateFormat, string dateTimeFormat)
        {
            //DateTime yesterdayDate = DateTime.Parse(DB.GetString("select sysdate from dual"));
            DateTime todayDay = DateTime.Parse(DB.GetString("select sysdate from dual"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime thisTime = DateTime.Now;
                string po = dt.Rows[i][0].ToString();
                string se_id = dt.Rows[i][1].ToString();
                string art_no = dt.Rows[i][2].ToString();
                string size_no = dt.Rows[i][3].ToString();
                string size_seq = dt.Rows[i][4].ToString();
                decimal v_work_qty = decimal.Parse(dt.Rows[i][5].ToString());
                decimal v_supplement_qty = decimal.Parse(dt.Rows[i][6].ToString());
                string d_dept = dt.Rows[i][7].ToString();
                string column2 = dt.Rows[i][8].ToString();
                DateTime workDate = DateTime.Parse(dt.Rows[i][9].ToString());
                string rout_no = dt.Rows[i][10].ToString();
                string se_day = ((DateTime)dt.Rows[i][11]).ToString(dateFormat);
                string vInOut = dt.Rows[i][12].ToString();


                string sql3 = @"select * from sjqdms_work_day  " +
                       "where org_id='" + companyCode + "' " +
                       "and d_dept='" + d_dept + "' " +
                       "and work_day=to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "') " +
                       "and se_id='" + se_id + "' " +
                       "and SE_SEQ='1' " +
                       "and INOUT_PZ='" + vInOut + "'" +
                       "and column1='S' ";
                System.Data.DataTable sjqdmsWorkDay = DB.GetDataTable(sql3);

                //取之前的用户
                string userSql = @"select grt_user from sjqdms_work_day  " +
                       "where org_id='" + companyCode + "' " +
                       "and d_dept='" + d_dept + "' " +
                       "and work_day=to_date('" + todayDay.ToString(dateFormat) + "','" + dateFormat + "') " +// todayDay.ToShortDateString() + "','yyyy/mm/dd') " +
                       "and se_id='" + se_id + "' " +
                       "and SE_SEQ='1' " +
                       "and INOUT_PZ='" + vInOut + "'" +
                       "order by column1 desc ";
                string grtUser = DB.GetString(userSql);

                if (sjqdmsWorkDay.Rows.Count <= 0)
                {
                    insertSjqdmsWorkDay(DB, companyCode, se_id, po, se_day,
                        d_dept, grtUser, d_dept, workDate, rout_no,
                        v_work_qty, art_no, v_supplement_qty, vInOut, thisTime, dateFormat, dateTimeFormat);
                    insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
                        d_dept, grtUser, d_dept, workDate, v_work_qty,
                        v_supplement_qty, vInOut, thisTime, dateFormat, dateTimeFormat);
                }
                else
                {
                    decimal oldSizeWorkQty = 0;
                    decimal oldSizeSupplementQty = 0;
                    string sql4 = @"select * from sjqdms_work_day_size  " +
                      "where org_id='" + companyCode + "' " +
                      "and d_dept='" + d_dept + "' " +
                      "and work_day=to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "') " +//.ToShortDateString() + "','yyyy/mm/dd') " +
                      "and se_id='" + se_id + "' " +
                      "and size_no='" + size_no + "' " +
                      "and SE_SEQ='1'" +
                      "and INOUT_PZ ='" + vInOut + "'" +
                      "and column1='S' ";

                    System.Data.DataTable sjqdmsWorkDaySize = DB.GetDataTable(sql4);
                    if (sjqdmsWorkDaySize.Rows.Count <= 0)
                    {
                        insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
                        d_dept, grtUser, d_dept, workDate, v_work_qty,
                        v_supplement_qty, vInOut, thisTime, dateFormat, dateTimeFormat);
                    }
                    else
                    {
                        oldSizeWorkQty = decimal.Parse(sjqdmsWorkDaySize.Rows[0]["WORK_QTY"].ToString());
                        oldSizeSupplementQty = decimal.Parse(sjqdmsWorkDaySize.Rows[0]["SUPPLEMENT_QTY"].ToString());
                        updateSjqdmsWorkDaySize(DB, companyCode, se_id, size_no,
                        d_dept, grtUser, d_dept, workDate,
                        v_work_qty, v_supplement_qty, vInOut, thisTime, oldSizeWorkQty, oldSizeSupplementQty, dateFormat, dateTimeFormat);
                    }
                    updateSjqdmsWorkDay(DB, companyCode, se_id, d_dept, grtUser,
                         d_dept, workDate, v_work_qty, v_supplement_qty, vInOut,
                         thisTime, oldSizeWorkQty, oldSizeSupplementQty, dateFormat, dateTimeFormat);
                }
            }
            return dt.Rows.Count;

        }

        public void Delete(DataBase DB, string userId, string companyCode, string dateFormat)
        {
            string tomorrow = DateTime.Now.AddDays(1).ToString(dateFormat);//.ToShortDateString();
            string sql = "delete from SJQDMS_WORK_DAY_SIZE where COLUMN1 = 'S' and WORK_DAY = to_date('" + tomorrow + "','" + dateFormat + "') and ORG_ID='" + companyCode + "'";
            string sql1 = "delete from SJQDMS_WORK_DAY where COLUMN1 = 'S' and WORK_DAY = to_date('" + tomorrow + "','" + dateFormat + "') and ORG_ID='" + companyCode + "'";
            DB.ExecuteNonQueryOffline(sql);
            DB.ExecuteNonQueryOffline(sql1);
        }

        public DataTable Query_thread(DataBase DB, string userId, string companyCode, string dateFormat)
        {
            DateTime tomorrow = DateTime.Parse(DateTime.Now.AddDays(1).ToString(dateFormat));//.ToShortDateString());
            string sql = "select * from SJQDMS_WORK_DAY_SIZE where COLUMN1 = 'S' and WORK_DAY = '" + tomorrow + "'and ORG_ID='" + companyCode + "'";
            return DB.GetDataTable(sql);
        }

        public DataTable Query_thread1(DataBase DB)
        {
            string sql = "select * from VW_SJQDMS_WORK_DAY_SIZE_MEND";
            return DB.GetDataTable(sql);

        }


        public int Insert_thread1(DataBase DB, DataTable dt, string userId, string companyCode, string dateFormat, string dateTimeFormat)
        {
            DateTime yesterdayDate = DateTime.Parse(DB.GetString("select (sysdate-1) from dual"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime thisTime = DateTime.Now;
                string po = dt.Rows[i][0].ToString();
                string se_id = dt.Rows[i][1].ToString();
                string art_no = dt.Rows[i][2].ToString();
                string size_no = dt.Rows[i][3].ToString();
                string size_seq = dt.Rows[i][4].ToString();
                decimal v_work_qty = decimal.Parse(dt.Rows[i][5].ToString());
                decimal v_supplement_qty = decimal.Parse(dt.Rows[i][6].ToString());
                string d_dept = dt.Rows[i][7].ToString();
                string column2 = dt.Rows[i][8].ToString();
                DateTime workDate = DateTime.Parse(dt.Rows[i][9].ToString());
                string rout_no = dt.Rows[i][10].ToString();
                string se_day = ((DateTime)dt.Rows[i][11]).ToString(dateFormat);
                string vInOut = dt.Rows[i][12].ToString();


                string sql3 = @"select * from sjqdms_work_day  " +
                       "where org_id='" + companyCode + "' " +
                       "and d_dept='" + d_dept + "' " +
                       "and work_day=to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "') " +//.ToShortDateString() + "','yyyy/mm/dd') " +
                       "and se_id='" + se_id + "' " +
                       "and INOUT_PZ='" + vInOut + "'" +
                       "and column1='S' ";
                System.Data.DataTable sjqdmsWorkDay = DB.GetDataTable(sql3);

                //取之前的用户
                string userSql = @"select grt_user from sjqdms_work_day  " +
                       "where org_id='" + companyCode + "' " +
                       "and d_dept='" + d_dept + "' " +
                       "and work_day=to_date('" + yesterdayDate.ToString(dateFormat) + "','" + dateFormat + "') " +//.ToShortDateString() + "','yyyy/mm/dd') " +
                       "and se_id='" + se_id + "' " +
                       "and SE_SEQ='1' " +
                       "and INOUT_PZ='" + vInOut + "'" +
                       "order by column1 desc ";
                string grtUser = DB.GetString(userSql);

                if (sjqdmsWorkDay.Rows.Count <= 0)
                {
                    insertSjqdmsWorkDay(DB, companyCode, se_id, po, se_day,
                        d_dept, grtUser, d_dept, workDate, rout_no,
                        v_work_qty, art_no, v_supplement_qty, vInOut, thisTime, dateFormat, dateTimeFormat);
                    insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
                        d_dept, grtUser, d_dept, workDate, v_work_qty,
                        v_supplement_qty, vInOut, thisTime, dateFormat, dateTimeFormat);
                }
                else
                {
                    decimal oldSizeWorkQty = 0;
                    decimal oldSizeSupplementQty = 0;
                    string sql4 = @"select * from sjqdms_work_day_size  " +
                      "where org_id='" + companyCode + "' " +
                      "and d_dept='" + d_dept + "' " +
                      "and work_day=to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "') " +//.ToShortDateString() + "','yyyy/mm/dd') " +
                      "and se_id='" + se_id + "' " +
                      "and size_no='" + size_no + "' " +
                      "and SE_SEQ='1'" +
                      "and INOUT_PZ ='" + vInOut + "'" +
                      "and column1='S' ";

                    System.Data.DataTable sjqdmsWorkDaySize = DB.GetDataTable(sql4);
                    if (sjqdmsWorkDaySize.Rows.Count <= 0)
                    {
                        insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
                        d_dept, grtUser, d_dept, workDate, v_work_qty,
                        v_supplement_qty, vInOut, thisTime, dateFormat, dateTimeFormat);
                    }
                    else
                    {
                        oldSizeWorkQty = decimal.Parse(sjqdmsWorkDaySize.Rows[0]["WORK_QTY"].ToString());
                        oldSizeSupplementQty = decimal.Parse(sjqdmsWorkDaySize.Rows[0]["SUPPLEMENT_QTY"].ToString());
                        updateSjqdmsWorkDaySize(DB, companyCode, se_id, size_no,
                        d_dept, grtUser, d_dept, workDate,
                        v_work_qty, v_supplement_qty, vInOut, thisTime, oldSizeWorkQty, oldSizeSupplementQty, dateFormat, dateTimeFormat);
                    }
                    updateSjqdmsWorkDay(DB, companyCode, se_id, d_dept, grtUser,
                         d_dept, workDate, v_work_qty, v_supplement_qty, vInOut,
                         thisTime, oldSizeWorkQty, oldSizeSupplementQty, dateFormat, dateTimeFormat);
                }
            }
            return dt.Rows.Count;
        }



        private void insertSjqdmsWorkDay(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id, string po,
          string se_day, string grt_dept, string grt_user, string d_dept, DateTime workDate, string routNo,
          decimal work_qty, string art_no, decimal supplement_qty, string vInOut, DateTime thisTime, string dateFormat, string dateTimeFormat)
        {
            string timeformat = " HH:mm:ss";
            string sql = @"insert into SJQDMS_WORK_DAY(org_id,se_id,se_seq,po,se_day,
                             column1,status,grt_dept,grt_user,last_user,
                             d_dept,work_day,rout_no,work_qty,art_no,
                             supplement_qty,inout_pz,insert_date)
                             values('" + companyCode + "','" + se_id + "','1','" + po + "',to_date('" + se_day.Substring(0, 10) + "','" + dateFormat + "')," +// se_day.Substring(0, 10) + "','yyyy/mm/dd')," +
                             "'S','7','" + grt_dept + "','" + grt_user + "','" + grt_user + "'," +
                             "'" + d_dept + "',to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "'),'" + routNo + "'," + work_qty + ",'" + art_no + "'," +
                             supplement_qty + ",'" + vInOut + "',to_date('" + thisTime.ToString(dateFormat + timeformat) + "','" + dateTimeFormat + "'))";
            DB.ExecuteNonQueryOffline(sql);
        }

        private void updateSjqdmsWorkDay(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id,
            string grt_dept, string grt_user, string d_dept, DateTime workDate,
            decimal v_work_qty, decimal v_supplement_qty, string vInOut, DateTime thisTime, decimal oldSizeWorkQty, decimal oldSizeSupplementQty, string dateFormat, string dateTimeFormat)
        {
            string timeformat = " HH:mm:ss";
            string sql = "update sjqdms_work_day set " +
                "WORK_QTY=WORK_QTY+" + (v_work_qty - oldSizeWorkQty) + "," +
                "SUPPLEMENT_QTY=SUPPLEMENT_QTY+" + (v_supplement_qty - oldSizeSupplementQty) + "," +
                "LAST_USER='" + grt_user + "'," +
                "LAST_DATE=to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "') " +
                " WHERE org_id='" + companyCode + "' and " +
                " d_dept='" + d_dept + "' and " +
                " work_day=to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "') and " +
                 " se_id='" + se_id + "' and " +
                " COLUMN1='S' and " +
                " INOUT_PZ='" + vInOut + "'";
            DB.ExecuteNonQueryOffline(sql);


        }

        private void insertSjqdmsWorkDaySize(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id, string size_no,
            string size_seq, string grt_dept, string grt_user, string d_dept, DateTime workDate,
            decimal work_qty, decimal supplement_qty, string vInOut, DateTime thisTime, string dateFormat, string dateTimeFormat)
        {
            string timeformat = " HH:mm:ss";
            string sql = @"insert into sjqdms_work_day_size(
                          ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,
                          STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                          WORK_DAY,WORK_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,
                          COLUMN1,COLUMN2,INOUT_PZ) VALUES(
                          '" + companyCode + "','" + se_id + "','1','" + size_no + "','" + size_seq + "'," +
                          "'7','" + grt_dept + "','" + grt_user + "','" + grt_user + "'," + "'" + d_dept +
                          "',to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')," + work_qty + ",to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "') ,to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "')," + supplement_qty + "," +
                          "'S','Y','" + vInOut + "')";
            DB.ExecuteNonQueryOffline(sql);
        }

        private void updateSjqdmsWorkDaySize(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id, string size_no,
            string grt_dept, string grt_user, string d_dept, DateTime workDate,
            decimal v_work_qty, decimal v_supplement_qty, string vInOut, DateTime thisTime, decimal oldSizeWorkQty, decimal oldSizeSupplementQty, string dateFormat, string dateTimeFormat)
        {
            string timeformat = " HH:mm:ss";
            string sql = "update sjqdms_work_day_size set " +
              "WORK_QTY=WORK_QTY+" + (v_work_qty - oldSizeWorkQty) + "," +
              "SUPPLEMENT_QTY=SUPPLEMENT_QTY+" + (v_supplement_qty - oldSizeSupplementQty) + "," +
              "LAST_USER='" + grt_user + "'," +
              "LAST_DATE=to_date('" + thisTime.ToString(dateFormat + timeformat) + "', '" + dateTimeFormat + "') " +//+ thisTime + "', 'yyyy/mm/dd HH24:MI:SS') " +
              " WHERE org_id='" + companyCode + "' and " +
              " d_dept='" + d_dept + "' and " +
              " work_day=to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "') and " +//.ToShortDateString() + "','yyyy/mm/dd') and " +
              " se_id='" + se_id + "' and " +
              " size_no='" + size_no + "' and " +
              " COLUMN1='S' and " +
              " INOUT_PZ='" + vInOut + "'";
            DB.ExecuteNonQueryOffline(sql);
        }
    }
}
