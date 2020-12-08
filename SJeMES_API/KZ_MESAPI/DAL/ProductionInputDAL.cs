using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class ProductionInputDAL
    {        
        public DataTable GetSeSize(DataBase DB, string companyCode, string vDDept, string vSeId, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql = "select * from VW_SJQDMS_WORK_DAY_SIZE where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sql += " and INOUT_PZ = 'IN'";
            sql += " and WORK_QTY + SUPPLEMENT_QTY > 0";

            sql += " order by size_seq asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                string size_no = dt.Rows[i]["SIZE_NO"].ToString();
                if (!(getSeSizeInputQty(DB, companyCode, vDDept, vSeId, size_no, dateFormat) < getWorkDaySizeQty(DB, companyCode, vDDept, vSeId, size_no, dateFormat)))
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            return dt;
        }

        public int getSeSizeInputQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSizeNo, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql = "select sum(finish_qty) from SJQDMS_WORK_DAY_SIZE where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and size_no = '" + vSizeNo + "'";
            sql += " and work_day <= to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sql += " and INOUT_PZ = 'IN'";
            return DB.GetInt32(sql);
        }

        public int getWorkDaySizeQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSizeNo, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql = "select sum(WORK_QTY) + sum(SUPPLEMENT_QTY) from SJQDMS_WORK_DAY_SIZE where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and size_no = '" + vSizeNo + "'";
            sql += " and work_day <= to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sql += " and INOUT_PZ = 'IN'";
            sql += " and COLUMN1 = 'U'";
            return DB.GetInt32(sql);
        }

        public void updateFinshQty_trance(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal vQty, string userId, string vIP, string dateFormat, string dateTimeFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql_time = "select sysdate from dual";
            DateTime insertDatetemp = DB.GetDateTime(sql_time);
            string insertDate = insertDatetemp.ToString(dateFormat + "HH:mm:ss");
            string sqlDaySizeU = "select * from SJQDMS_WORK_DAY_SIZE where 1 = 1";
            sqlDaySizeU += " and ORG_ID='" + companyCode + "'";
            sqlDaySizeU += " and se_id = '" + vSeId + "'";
            sqlDaySizeU += " and se_seq = '" + vSeSeq + "'";
            sqlDaySizeU += " and d_dept = '" + vDDept + "'";
            sqlDaySizeU += " and size_no = '" + vSizeNo + "'";
            sqlDaySizeU += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sqlDaySizeU += " and INOUT_PZ = 'IN'";
            sqlDaySizeU += " and COLUMN1 = 'U'";
            System.Data.DataTable daySizeU = DB.GetDataTable(sqlDaySizeU);
            if (daySizeU.Rows.Count > 0)
            {
                decimal? uQty = decimal.Parse(daySizeU.Rows[0]["WORK_QTY"].ToString()) + decimal.Parse(daySizeU.Rows[0]["SUPPLEMENT_QTY"].ToString())
                    - decimal.Parse(daySizeU.Rows[0]["FINISH_QTY"].ToString());
                if (uQty > 0)
                {
                    //如果还可以报,可报数量--uQty大于报的数量vQty,则用vQty否则uQty
                    uQty = (uQty >= vQty ? vQty : uQty.GetValueOrDefault());

                    updateWorkDayFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, uQty, userId, insertDate, workDate, "U", "IN",dateFormat, dateTimeFormat);
                    updateWorkDaySizeFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, uQty, userId, insertDate, workDate, "U", "IN",dateFormat,dateTimeFormat);
                }

                decimal? sQty = vQty - uQty;
                if (sQty > 0)
                {
                    string sqlDaySizeS = "select * from SJQDMS_WORK_DAY_SIZE where 1 = 1";
                    sqlDaySizeU += " and ORG_ID='" + companyCode + "'";
                    sqlDaySizeU += " and se_id = '" + vSeId + "'";
                    sqlDaySizeU += " and se_seq = '" + vSeSeq + "'";
                    sqlDaySizeU += " and d_dept = '" + vDDept + "'";
                    sqlDaySizeU += " and size_no = '" + vSizeNo + "'";
                    sqlDaySizeU += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"')";
                    sqlDaySizeU += " and INOUT_PZ = 'IN'";
                    sqlDaySizeU += " and COLUMN1 = 'S'";
                    System.Data.DataTable daySizeS = DB.GetDataTable(sqlDaySizeS);
                    if (daySizeS.Rows.Count > 0)
                    {
                        updateWorkDayFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, sQty, userId, insertDate, workDate, "S", "IN",dateFormat, dateTimeFormat);
                        updateWorkDaySizeFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, sQty, userId, insertDate, workDate, "S", "IN", dateFormat,dateTimeFormat);
                    }
                    else
                    {
                        throw new Exception();//如果大于0，但是没有可以塞资料的
                    }
                }
            }
            else
            {
                updateWorkDayFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vQty, userId, insertDate, workDate, "S", "IN", dateFormat, dateTimeFormat);
                updateWorkDaySizeFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, insertDate, workDate, "S", "IN",dateFormat, dateTimeFormat);
            }

            string sqlUser = "select * from HR001M where 1 = 1";
            sqlUser += " and STAFF_NO='" + userId + "'";
            System.Data.DataTable User = DB.GetDataTable(sqlUser);

            string sqlDetail = "select * from SJQDMS_WORK_DAY where 1 = 1";
            sqlDetail += " and ORG_ID='" + companyCode + "'";
            sqlDetail += " and se_id = '" + vSeId + "'";
            sqlDetail += " and se_seq = '" + vSeSeq + "'";
            sqlDetail += " and d_dept = '" + vDDept + "'";
            sqlDetail += " and INOUT_PZ = 'IN'";
            sqlDetail += " and COLUMN1 = 'U'";
            System.Data.DataTable Detail = DB.GetDataTable(sqlDetail);

            string po = Detail.Rows[0]["PO"].ToString();
            string se_day = ((DateTime)Detail.Rows[0]["SE_DAY"]).ToString(dateFormat);
            string rout_no = Detail.Rows[0]["ROUT_NO"].ToString();
            string art_no = Detail.Rows[0]["ART_NO"].ToString();
            string scan_name = User.Rows[0]["STAFF_NAME"].ToString();

            string sqlMesD = @"insert into MES_LABEL_D(org_id,se_id,se_seq,po_no,LABEL_TYPE,LABEL_QTY,status,grt_dept,grt_user,
                                    last_user,SCAN_DETPT,art_no,SIZE_NO,PROCESS_NO,SCAN_PZ,LABEL_ID,inout_pz,SCAN_IP,SCAN_NAME,
                                    SCAN_DATE,insert_date,LAST_DATE)values(
                                    '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + po + "','B','" + vQty + "','7','" + vDDept + "','" + userId + 
                                    "','" + userId + "'," + "'" + vDDept + "','" + art_no + "','" + vSizeNo + "','" + rout_no + "','B','8888','IN','" + vIP + 
                                    "','" + scan_name + "',to_date('" + insertDate + "','"+dateTimeFormat+"'),to_date('" + insertDate +
                                    "','" + dateTimeFormat + "'),to_date('" + insertDate + "','" + dateTimeFormat + "'))";

            DB.ExecuteNonQuery(sqlMesD);

            //投入时更新当天产出排程
            string sql = @"select * from BASE005M " +
                    "where  department_code='" + vDDept + "'";

            System.Data.DataTable base005M = DB.GetDataTable(sql);
            if (base005M.Rows.Count > 0)
            {
                if (base005M.Rows[0]["UDF03"].ToString() == "Y")
                {
                    createOutPlan(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, insertDate, workDate, po, se_day, rout_no, art_no, dateFormat, dateTimeFormat);
                }
            }
        }

        public void createOutPlan(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal vQty, string userId, 
            string insertDate, DateTime workDate, string po, string se_day, string rout_no, string art_no, string dateFormat, string dateTimeFormat)
        {
            string size_seq = "0";
            string sqlOrdSize = @"select * from mv_SE_ORD_size " +
                "where  org_id='" + companyCode + "' and se_id='" + vSeId + "' and se_seq='1' and size_no='" + vSizeNo + "'";
            System.Data.DataTable seOrdSize = DB.GetDataTable(sqlOrdSize);

            if (seOrdSize.Rows.Count > 0)
            {
                size_seq = seOrdSize.Rows[0]["SIZE_SEQ"].ToString();
            }

            string sqlDay_Out = "select * from SJQDMS_WORK_DAY where 1 = 1";
            sqlDay_Out += " and ORG_ID='" + companyCode + "'";
            sqlDay_Out += " and se_id = '" + vSeId + "'";
            sqlDay_Out += " and se_seq = '" + vSeSeq + "'";
            sqlDay_Out += " and d_dept = '" + vDDept + "'";
            sqlDay_Out += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sqlDay_Out += " and INOUT_PZ = 'OUT'";
            sqlDay_Out += " and COLUMN1 = 'U'";
            System.Data.DataTable Day_Out = DB.GetDataTable(sqlDay_Out);

            if (Day_Out.Rows.Count > 0)
            {
                string sqlDaySize_Out = "select * from SJQDMS_WORK_DAY_SIZE where 1 = 1";
                sqlDaySize_Out += " and ORG_ID='" + companyCode + "'";
                sqlDaySize_Out += " and se_id = '" + vSeId + "'";
                sqlDaySize_Out += " and se_seq = '" + vSeSeq + "'";
                sqlDaySize_Out += " and d_dept = '" + vDDept + "'";
                sqlDaySize_Out += " and size_no = '" + vSizeNo + "'";
                sqlDaySize_Out += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"')";
                sqlDaySize_Out += " and INOUT_PZ = 'OUT'";
                sqlDaySize_Out += " and COLUMN1 = 'U'";
                System.Data.DataTable DaySize_Out = DB.GetDataTable(sqlDaySize_Out);

                if (DaySize_Out.Rows.Count > 0)
                {
                    string sql1 = "update sjqdms_work_day set " +
                                        "WORK_QTY = WORK_QTY + " + vQty + 
                                        //"," +
                                        //"LAST_USER = '" + userId + "'," +
                                        //"LAST_DATE = to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                        " WHERE org_id = '" + companyCode + "' and " +
                                        " d_dept='" + vDDept + "' and " +
                                        " work_day = to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"') and " +
                                        " se_id='" + vSeId + "' and " +
                                        " se_seq='" + vSeSeq + "' and " +
                                        " COLUMN1 = 'U' and" +
                                        " INOUT_PZ = 'OUT'";

                    string sql2 = "update sjqdms_work_day_size set " +
                                        "WORK_QTY = WORK_QTY + " + vQty + "," +
                                        "LAST_USER = '" + userId + "'," +
                                        "LAST_DATE = to_date('" + insertDate + "', '"+ dateTimeFormat + "') " +
                                        " WHERE org_id = '" + companyCode + "' and " +
                                        " d_dept = '" + vDDept + "' and " +
                                        " size_no = '" + vSizeNo + "' and " +
                                        " work_day = to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"') and " +
                                        " se_id = '" + vSeId + "' and " +
                                        " se_seq = '" + vSeSeq + "' and " +
                                        " COLUMN1 = 'U' and" +
                                        " INOUT_PZ = 'OUT'";

                    DB.ExecuteNonQuery(sql1);
                    DB.ExecuteNonQuery(sql2);
                }
                else
                {
                    string sql3 = @"insert into sjqdms_work_day_size(
                                        ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                                        WORK_DAY,WORK_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,COLUMN1,COLUMN2,
                                        INOUT_PZ) VALUES(
                                        '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vSizeNo + "','" + size_seq + "'," +"'7','" + vDDept + 
                                        "','" + userId + "','" + userId + "'," + "'" + vDDept + "',to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"')," + 
                                        vQty + ",to_date('" + insertDate + "', '"+ dateTimeFormat + "') ,to_date('" + insertDate + "', '"+ dateTimeFormat + "')," +
                                        "'0','U','Y','OUT')";

                    string sql4 = "update sjqdms_work_day set " +
                                        "WORK_QTY = WORK_QTY + " + vQty + 
                                        //"," +
                                        //"LAST_USER = '" + userId + "'," +
                                        //"LAST_DATE = to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                        " WHERE org_id = '" + companyCode + "' and " +
                                        " d_dept='" + vDDept + "' and " +
                                        " work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "') and " +
                                        " se_id='" + vSeId + "' and " +
                                        " se_seq='" + vSeSeq + "' and " +
                                        " COLUMN1 = 'U' and" +
                                        " INOUT_PZ = 'OUT'";
                    DB.ExecuteNonQuery(sql3);
                    DB.ExecuteNonQuery(sql4);
                }
            }
            else
            {
                string sql5 = @"insert into SJQDMS_WORK_DAY(
                                    org_id,se_id,se_seq,po,se_day,column1,status,grt_dept,grt_user,last_user,d_dept,work_day,
                                    rout_no,work_qty,art_no,supplement_qty,inout_pz,insert_date)values(
                                    '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + po + "',to_date('" + se_day.Substring(0, 10) + "','" + dateFormat + "')," + 
                                    "'U','7','" + vDDept + "','" + userId + "','" + userId + "'," + "'" + vDDept + "',to_date('" + workDate.ToString(dateFormat) +
                                    "','" + dateFormat + "'),'" + rout_no + "'," + vQty + ",'" + art_no + "'," + "'0','OUT'," + "to_date('" + insertDate + "','" + dateTimeFormat + "'))";

                string sql6 = @"insert into sjqdms_work_day_size(
                                    ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                                    WORK_DAY,WORK_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,COLUMN1,COLUMN2,
                                    INOUT_PZ) VALUES(
                                    '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vSizeNo + "','" + size_seq + "'," + "'7','" + vDDept +
                                    "','" + userId + "','" + userId + "'," + "'" + vDDept + "',to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"')," +
                                    vQty + ",to_date('" + insertDate + "', '"+ dateTimeFormat + "') ,to_date('" + insertDate + "', '"+dateTimeFormat+"')," +
                                    "'0','U','Y','OUT')";
                DB.ExecuteNonQuery(sql5);
                DB.ExecuteNonQuery(sql6);
            }
        }

        public void updateFinshQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal vQty, string userId, string vIP, string dateFormat, string dateTimeFormat)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                updateFinshQty_trance(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, vIP, dateFormat, dateTimeFormat);
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

        public void updateWorkDayFinishQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, decimal? qty, string userId, string insertDate, DateTime workDate, string column1, string inOut, string dateFormat, string dateTimeFormat)
        {
            string sql = "update sjqdms_work_day set " +
                                 "FINISH_QTY = FINISH_QTY + " + qty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', '"+dateTimeFormat+"') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " work_day=to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut + "'";
            DB.ExecuteNonQuery(sql);
        }

        public void updateWorkDaySizeFinishQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal? qty, string userId, string insertDate, DateTime workDate, string column1, string inOut, string dateFormat, string dateTimeFormat)
        {
            string sql = "update sjqdms_work_day_size set " +
                                 "FINISH_QTY = FINISH_QTY +" + qty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', '"+ dateTimeFormat + "') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " size_no='" + vSizeNo + "' and " +
                                 " work_day=to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut + "'";
            DB.ExecuteNonQuery(sql);
        }
    }
}




