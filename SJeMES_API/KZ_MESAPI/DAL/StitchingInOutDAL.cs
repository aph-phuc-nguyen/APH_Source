using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class StitchingInOutDAL
    {
        public DataTable GetOrdM(DataBase DB, string companyCode)
        {
            string sql = "select SE_ID,MER_PO,SE_DAY,prod_no from vw_se_ord_item where 1 = 1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            return dt;
        }

        public int GetDayFinishQty(DataBase DB, string companyCode, string vDDept)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql = "select nvl(sum(finish_qty),0) from SJQDMS_WORK_DAY where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
            sql += " and INOUT_PZ = 'IN'";
            sql += " and COLUMN1 = 'U'";

            return DB.GetInt32(sql);
        }

        public DataTable GetSeSize(DataBase DB, string companyCode, string vSeId)
        {
            string sql = "select * from VW_STITCHINPUT_ORD_SIZE where 1 = 1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and SE_ID='" + vSeId + "'";
            sql += " and SE_QTY > FINISH_QTY";

            sql += " order by size_seq asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            return dt;
        }

        public DataTable GetSeSizeDetail(DataBase DB, string companyCode, string vSeId, string vSeSeq, string vSizeNo)
        {
            string sql = "select * from VW_STITCHINPUT_ORD_SIZE where 1 = 1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and SE_ID='" + vSeId + "'";
            sql += " and SE_SEQ = '" + vSeSeq + "'";
            sql += " and SIZE_NO = '" + vSizeNo + "'";

            sql += " order by size_seq asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            return dt;
        }

        public bool ValisStitchingDept(DataBase DB, string vDDept, string vRoutNo)
        {
            bool isStitchingDept = false;
            string sql = "SELECT DEPARTMENT_CODE,DEPARTMENT_NAME FROM base005m where 1 = 1";
            sql += " and DEPARTMENT_CODE='" + vDDept + "'";
            sql += " and UDF01 = '" + vRoutNo + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                isStitchingDept = true;
            }
            return isStitchingDept;
        }

        public void updateInFinshQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vSizeSeq, 
            decimal vQty, string userId, string vIP, string vPO, string vArtNo, string vSeDay)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                updateInFinshQty_trance(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vSizeSeq, vQty, userId, vIP, vPO, vArtNo, vSeDay);
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

        public void updateInFinshQty_trance(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vSizeSeq, 
            decimal vQty, string userId, string vIP, string vPO, string vArtNo, string vSeDay)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql_time = "select sysdate from dual";
            DateTime insertDate = DB.GetDateTime(sql_time);
            //DateTime insertDate = DateTime.Now;

            string sql_hr = "select STAFF_DEPARTMENT,staff_name from hr001m where 1=1 ";
            sql_hr += " and staff_no='" + userId + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql_hr);
            string userDept = dt.Rows[0]["STAFF_DEPARTMENT"].ToString();
            string userName = dt.Rows[0]["staff_name"].ToString();

            string sql_dept = @"select * from BASE005M " +
                    "where  department_code='" + vDDept + "'";

            System.Data.DataTable base005M = DB.GetDataTable(sql_dept);
            if (base005M.Rows.Count > 0 && base005M.Rows[0]["UDF02"].ToString() == "Y")
            {
                string rout_no = base005M.Rows[0]["UDF01"].ToString();

                string sqlDayIN = "select * from SJQDMS_WORK_DAY where 1 = 1";
                sqlDayIN += " and ORG_ID='" + companyCode + "'";
                sqlDayIN += " and d_dept = '" + vDDept + "'";
                sqlDayIN += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
                sqlDayIN += " and se_id = '" + vSeId + "'";
                sqlDayIN += " and se_seq = '" + vSeSeq + "'";
                sqlDayIN += " and INOUT_PZ = 'IN'";
                sqlDayIN += " and COLUMN1 = 'U'";
                System.Data.DataTable dayIN = DB.GetDataTable(sqlDayIN);

                if (dayIN.Rows.Count > 0)
                {
                    string sqlDaySizeIN = "select * from SJQDMS_WORK_DAY_SIZE where 1 = 1";
                    sqlDaySizeIN += " and ORG_ID='" + companyCode + "'";
                    sqlDaySizeIN += " and d_dept = '" + vDDept + "'";
                    sqlDaySizeIN += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
                    sqlDaySizeIN += " and se_id = '" + vSeId + "'";
                    sqlDaySizeIN += " and se_seq = '" + vSeSeq + "'";
                    sqlDaySizeIN += " and size_no = '" + vSizeNo + "'";
                    sqlDaySizeIN += " and INOUT_PZ = 'IN'";
                    sqlDaySizeIN += " and COLUMN1 = 'U'";
                    System.Data.DataTable daySizeIN = DB.GetDataTable(sqlDaySizeIN);

                    if (daySizeIN.Rows.Count > 0)
                    {
                        updateWorkDayQtyIN(DB, companyCode, vDDept, vSeId, vSeSeq, vQty, userId, insertDate, workDate, "U", "IN");
                        updateWorkDaySizeQtyIN(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, insertDate, workDate, "U", "IN");
                    }
                    else
                    {
                        string sql1 = @"insert into sjqdms_work_day_size(
                                            ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                                            WORK_DAY,WORK_QTY,FINISH_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,COLUMN1,COLUMN2,
                                            INOUT_PZ) VALUES(
                                            '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vSizeNo + "','" + vSizeSeq + "'," + "'7','" + userDept + 
                                            "','" + userId + "','" + userId + "'," + "'" + vDDept + "',to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')," + 
                                            vQty + "," + vQty + ",to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') ,to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS')," +
                                            "'0','U','Y','IN')";

                        DB.ExecuteNonQuery(sql1);
                        updateWorkDayQtyIN(DB, companyCode, vDDept, vSeId, vSeSeq, vQty, userId, insertDate, workDate, "U", "IN");
                    }
                }
                else
                {
                    string sql2 = @"insert into SJQDMS_WORK_DAY(
                                        org_id,se_id,se_seq,po,se_day,column1,status,grt_dept,grt_user,last_user,
                                        d_dept,work_day,rout_no,work_qty,finish_qty,art_no,supplement_qty,inout_pz,
                                        insert_date) values(
                                        '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vPO + "',to_date('" + vSeDay + 
                                        "','yyyy/mm/dd')," +"'U','7','" + userDept + "','" + userId + "','" + userId + "'," +"'" + vDDept + 
                                        "',to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd'),'" + rout_no + "'," + vQty + "," + 
                                        vQty + ",'" + vArtNo + "'," +"'0','IN'," + "to_date('" + insertDate + "','yyyy/mm/dd HH24:MI:SS'))";

                    string sql3 = @"insert into sjqdms_work_day_size(
                                        ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                                        WORK_DAY,WORK_QTY,FINISH_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,COLUMN1,COLUMN2,
                                        INOUT_PZ) VALUES(
                                        '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vSizeNo + "','" + vSizeSeq + "'," + "'7','" + 
                                        userDept + "','" + userId + "','" + userId + "'," + "'" + vDDept + "',to_date('" + workDate.ToShortDateString() + 
                                        "','yyyy/mm/dd')," + vQty + "," + vQty + ",to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') ,to_date('" + 
                                        insertDate + "', 'yyyy/mm/dd HH24:MI:SS')," + "'0','U','Y','IN')";

                    DB.ExecuteNonQuery(sql2);
                    DB.ExecuteNonQuery(sql3);
                }

                string sqlMesD = @"insert into MES_LABEL_D(
                                        org_id,se_id,se_seq,po_no,LABEL_TYPE,LABEL_QTY,status,grt_dept,grt_user,last_user,
                                        SCAN_DETPT,art_no,SIZE_NO,PROCESS_NO,SCAN_PZ,LABEL_ID,inout_pz,SCAN_IP,SCAN_NAME,
                                        SCAN_DATE,insert_date,LAST_DATE)values(
                                        '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vPO + "','B','" + vQty + "','7','" + userDept + "','" + userId + 
                                        "','" + userId + "'," + "'" + vDDept + "','" + vArtNo + "','" + vSizeNo + "','" + rout_no + "','B','8888','IN','" + vIP + 
                                        "','" + userName + "',to_date('" + insertDate + "','yyyy/mm/dd HH24:MI:SS'),to_date('" + insertDate + 
                                        "','yyyy/mm/dd HH24:MI:SS'),to_date('" + insertDate + "','yyyy/mm/dd HH24:MI:SS'))";

                DB.ExecuteNonQuery(sqlMesD);

                //投入时更新当天产出排程
                if (base005M.Rows.Count > 0 && base005M.Rows[0]["UDF03"].ToString() == "Y")
                {
                    createStitchingOutPlan( DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vSizeSeq, vQty, userId, rout_no, vPO, vArtNo, vSeDay, workDate, insertDate, userDept);
                }
            }
        }

        public void updateWorkDayQtyIN(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, decimal qty, string userId, 
            DateTime insertDate, DateTime workDate, string column1, string inOut)
        {
            string sql = "update sjqdms_work_day set " +
                                 "WORK_QTY = WORK_QTY + " + qty + "," +
                                 "FINISH_QTY = FINISH_QTY + " + qty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " work_day=to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut + "'";
            DB.ExecuteNonQuery(sql);
        }

        public void updateWorkDaySizeQtyIN(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal qty, 
            string userId, DateTime insertDate, DateTime workDate, string column1, string inOut)
        {
            string sql = "update sjqdms_work_day_size set " +
                                 "WORK_QTY = WORK_QTY +" + qty + "," +
                                 "FINISH_QTY = FINISH_QTY +" + qty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " size_no='" + vSizeNo + "' and " +
                                 " work_day=to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut + "'";
            DB.ExecuteNonQuery(sql);
        }

        public void createStitchingOutPlan(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vSizeSeq, 
            decimal vQty, string userId, string rout_no, string vPO, string vArtNo, string vSeDay, DateTime workDate, DateTime insertDate, string userDept)
        {
            string sqlDayOUT = "select * from SJQDMS_WORK_DAY where 1 = 1";
            sqlDayOUT += " and ORG_ID='" + companyCode + "'";
            sqlDayOUT += " and d_dept = '" + vDDept + "'";
            sqlDayOUT += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
            sqlDayOUT += " and se_id = '" + vSeId + "'";
            sqlDayOUT += " and se_seq = '" + vSeSeq + "'";
            sqlDayOUT += " and INOUT_PZ = 'OUT'";
            sqlDayOUT += " and COLUMN1 = 'U'";
            System.Data.DataTable dayOut = DB.GetDataTable(sqlDayOUT);

            if (dayOut.Rows.Count > 0)
            {
                string sqlDaySizeOut = "select * from SJQDMS_WORK_DAY_SIZE where 1 = 1";
                sqlDaySizeOut += " and ORG_ID='" + companyCode + "'";
                sqlDaySizeOut += " and d_dept = '" + vDDept + "'";
                sqlDaySizeOut += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
                sqlDaySizeOut += " and se_id = '" + vSeId + "'";
                sqlDaySizeOut += " and se_seq = '" + vSeSeq + "'";
                sqlDaySizeOut += " and size_no = '" + vSizeNo + "'";
                sqlDaySizeOut += " and INOUT_PZ = 'OUT'";
                sqlDaySizeOut += " and COLUMN1 = 'U'";
                System.Data.DataTable daySizeOut = DB.GetDataTable(sqlDaySizeOut);

                if (daySizeOut.Rows.Count > 0)
                {
                    string sql1 = "update sjqdms_work_day set " +
                                 "WORK_QTY = WORK_QTY + " + vQty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " work_day=to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= 'U' and" +
                                 " INOUT_PZ='OUT'";

                    string sql2 = "update sjqdms_work_day_size set " +
                                 "WORK_QTY = WORK_QTY +" + vQty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " size_no='" + vSizeNo + "' and " +
                                 " work_day=to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= 'U' and" +
                                 " INOUT_PZ='OUT'";

                    DB.ExecuteNonQuery(sql1);
                    DB.ExecuteNonQuery(sql2);
                }
                else
                {
                    string sql3 = @"insert into sjqdms_work_day_size(
                                        ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                                        WORK_DAY,WORK_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,COLUMN1,
                                        COLUMN2,INOUT_PZ) VALUES(
                                        '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vSizeNo + "','" + vSizeSeq + "'," + "'7','" + userDept + 
                                        "','" + userId + "','" + userId + "'," + "'" + vDDept + "',to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')," +
                                        vQty + ",to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') ,to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS')," + 
                                        "'0','U','Y','OUT')";

                    string sql4 = "update sjqdms_work_day set " +
                                 "WORK_QTY = WORK_QTY + " + vQty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " work_day=to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= 'U' and" +
                                 " INOUT_PZ='OUT'";

                    DB.ExecuteNonQuery(sql3);
                    DB.ExecuteNonQuery(sql4);
                }
            }
            else
            {
                string sql5 = @"insert into SJQDMS_WORK_DAY(org_id,se_id,se_seq,po,se_day,column1,status,grt_dept,
                                    grt_user,last_user,d_dept,work_day,rout_no,work_qty,art_no,supplement_qty,
                                    inout_pz,insert_date) values(
                                    '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vPO + "',to_date('" + vSeDay.Substring(0, 10) + "','yyyy/mm/dd')," + 
                                    "'U','7','" + userDept + "','" + userId + "','" + userId + "'," +"'" + vDDept + "',to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd'),'" + 
                                    rout_no + "'," + vQty + ",'" + vArtNo + "'," +"'0','OUT'," + "to_date('" + insertDate + "','yyyy/mm/dd HH24:MI:SS'))";

                string sql6 = @"insert into sjqdms_work_day_size(
                                    ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                                    WORK_DAY,WORK_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,COLUMN1,COLUMN2,
                                    INOUT_PZ) VALUES(
                                    '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + vSizeNo + "','" + vSizeSeq + "'," + "'7','" + userDept + "','" + 
                                    userId + "','" + userId + "'," + "'" + vDDept + "',to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')," + vQty + 
                                    ",to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') ,to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS')," +
                                    "'0','U','Y','OUT')";

                DB.ExecuteNonQuery(sql5);
                DB.ExecuteNonQuery(sql6);
            }
        }

        public int GeSizeFinishQty(DataBase DB, string companyCode, string vSeId, string vSeSeq, string vSizeNo)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            int qty = 0;
            string sql = "select * from vw_stitchinput_size_finish where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and se_seq = '" + vSeSeq + "'";
            sql += " and size_no = '" + vSizeNo + "'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                qty = Convert.ToInt32(dt.Rows[0]["FINISH_QTY"].ToString());
            }
            return qty;
        }

        public void updateOutFinshQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal vQty, string userId, string vIP)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                updateOutFinshQty_trance(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, vIP);
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

        public void updateOutFinshQty_trance(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal vQty, string userId, string vIP)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql_time = "select sysdate from dual";
            DateTime insertDate = DB.GetDateTime(sql_time);
            //DateTime insertDate = DateTime.Now;
            string sqlDaySizeU = "select * from SJQDMS_WORK_DAY_SIZE where 1 = 1";
            sqlDaySizeU += " and ORG_ID='" + companyCode + "'";
            sqlDaySizeU += " and se_id = '" + vSeId + "'";
            sqlDaySizeU += " and se_seq = '" + vSeSeq + "'";
            sqlDaySizeU += " and d_dept = '" + vDDept + "'";
            sqlDaySizeU += " and size_no = '" + vSizeNo + "'";
            sqlDaySizeU += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
            sqlDaySizeU += " and INOUT_PZ = 'OUT'";
            sqlDaySizeU += " and COLUMN1 = 'U'";
            System.Data.DataTable daySizeU = DB.GetDataTable(sqlDaySizeU);
            if (daySizeU.Rows.Count > 0)
            {
                decimal? uQty = decimal.Parse(daySizeU.Rows[0]["WORK_QTY"].ToString()) - decimal.Parse(daySizeU.Rows[0]["FINISH_QTY"].ToString());
                if (uQty > 0)
                {
                    //如果还可以报,可报数量--uQty大于报的数量vQty,则用vQty否则uQty
                    uQty = (uQty >= vQty ? vQty : uQty.GetValueOrDefault());

                    updateWorkDayFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, uQty, userId, insertDate, workDate, "U", "OUT");
                    updateWorkDaySizeFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, uQty, userId, insertDate, workDate, "U", "OUT");
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
                    sqlDaySizeU += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
                    sqlDaySizeU += " and INOUT_PZ = 'OUT'";
                    sqlDaySizeU += " and COLUMN1 = 'S'";
                    System.Data.DataTable daySizeS = DB.GetDataTable(sqlDaySizeS);
                    if (daySizeS.Rows.Count > 0)
                    {
                        updateWorkDayFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, sQty, userId, insertDate, workDate, "S", "OUT");
                        updateWorkDaySizeFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, sQty, userId, insertDate, workDate, "S", "OUT");
                    }
                    else
                    {
                        throw new Exception();//如果大于0，但是没有可以塞资料的
                    }
                }
            }
            else
            {
                updateWorkDayFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vQty, userId, insertDate, workDate, "S", "OUT");
                updateWorkDaySizeFinishQty(DB, companyCode, vDDept, vSeId, vSeSeq, vSizeNo, vQty, userId, insertDate, workDate, "S", "OUT");
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
            string se_day = Detail.Rows[0]["SE_DAY"].ToString();
            string rout_no = Detail.Rows[0]["ROUT_NO"].ToString();
            string art_no = Detail.Rows[0]["ART_NO"].ToString();
            string scan_name = User.Rows[0]["STAFF_NAME"].ToString();

            string sqlMesD = @"insert into MES_LABEL_D(org_id,se_id,se_seq,po_no,LABEL_TYPE,LABEL_QTY,status,grt_dept,grt_user,
                                    last_user,SCAN_DETPT,art_no,SIZE_NO,PROCESS_NO,SCAN_PZ,LABEL_ID,inout_pz,SCAN_IP,SCAN_NAME,
                                    SCAN_DATE,insert_date,LAST_DATE)values(
                                    '" + companyCode + "','" + vSeId + "','" + vSeSeq + "','" + po + "','B','" + vQty + "','7','" + vDDept + "','" + userId +
                                    "','" + userId + "'," + "'" + vDDept + "','" + art_no + "','" + vSizeNo + "','" + rout_no + "','B','8888','OUT','" + vIP +
                                    "','" + scan_name + "',to_date('" + insertDate + "','yyyy/mm/dd HH24:MI:SS'),to_date('" + insertDate +
                                    "','yyyy/mm/dd HH24:MI:SS'),to_date('" + insertDate + "','yyyy/mm/dd HH24:MI:SS'))";

            DB.ExecuteNonQuery(sqlMesD);
        }

        public void updateWorkDayFinishQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, decimal? qty, string userId, DateTime insertDate, DateTime workDate, string column1, string inOut)
        {
            string sql = "update sjqdms_work_day set " +
                                 "FINISH_QTY = FINISH_QTY + " + qty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " work_day=to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut + "'";
            DB.ExecuteNonQuery(sql);
        }

        public void updateWorkDaySizeFinishQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, decimal? qty, string userId, DateTime insertDate, DateTime workDate, string column1, string inOut)
        {
            string sql = "update sjqdms_work_day_size set " +
                                 "FINISH_QTY = FINISH_QTY +" + qty + "," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', 'yyyy/mm/dd HH24:MI:SS') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " size_no='" + vSizeNo + "' and " +
                                 " work_day=to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " se_seq='" + vSeSeq + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut + "'";
            DB.ExecuteNonQuery(sql);
        }
    }
}

