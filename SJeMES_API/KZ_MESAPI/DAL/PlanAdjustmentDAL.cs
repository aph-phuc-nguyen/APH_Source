using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class PlanAdjustmentDAL
    {

        public DataTable QuerySize(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vInOut, string userId, string companyCode, string dateFormat, string datetimeFormat)
        {
            string sql = "select se_id,size_no,size_seq,nvl(sum(work_qty),0) as work_qty,nvl(sum(SUPPLEMENT_QTY),0) as SUPPLEMENT_QTY," +
                      "sum(nvl(finish_qty,0)) as finish_qty,'' as MOVE_QTY  " +
                      "from sjqdms_work_day_size where 1=1 ";
            sql += " and se_id='" + vSeId + "'";
            sql += " and d_dept='" + vDDept + "'";
            sql += " and work_day=to_date('" + vWrokDay.Substring(0,10) + "','"+dateFormat+"')";
            //sql += " and work_day=to_date('" + vWrokDay.Substring(0, vWrokDay.IndexOf(":") - 2) + "','"+dateFormat+"')";
            sql += " and INOUT_PZ='" + vInOut + "'";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " group by se_id,size_no,size_seq";
            sql += " order by size_seq"; 
            return DB.GetDataTable(sql);
        }

        public  string LoadMoveNo(DataBase DB)
        {
            string sql = "select SEQ_MES_MOVE_M.Nextval from dual";
            return DB.GetString(sql);
        }

        public DataTable LoadSeDept(DataBase DB, string vDDept, string vRoutNo, string userId, string companyCode)
        {
            string sql = "select DEPARTMENT_CODE,DEPARTMENT_NAME from base005m where udf01='"+ vRoutNo+ "' and DEPARTMENT_CODE!='"+ vDDept + "'";
            return DB.GetDataTable(sql);
        }

        public void InsertMesMoveM(DataBase DB, string vMoveNo, string vMoveDate, string vSeId, string vDDept, string vWrokDay, string vInOut, string vNewWorkDay, string vNewDdept, DataTable dataTable, string userCode, string companyCode,string dateFormat, string datetimeFormat)
        {
            string workDay= vWrokDay.Substring(0, 10);
            string sql = "insert into mes_move_m(" +
             "ORG_ID,MOVE_NO,MOVE_DATE,SE_ID,SE_SEQ," +
             "D_DEPT,WORK_DAY,INOUT_PZ,INSERT_USER,OLD_D_DEPT," +
             "NEW_D_DEPT,NEW_WORK_DAY) " +
             "values('" + companyCode + "','" + vMoveNo + "',to_date('" + vMoveDate + "','" + dateFormat + "'),'" + vSeId + "','1'," +
             "'" + vDDept + "',to_date('" + workDay + "','" + dateFormat + "'),'" + vInOut + "','" + userCode + "','" + vDDept + "'," +
             "'" + vNewDdept + "',to_date('" + vNewWorkDay + "','" + dateFormat + "')) ";
            DB.ExecuteNonQuery(sql);
        }

        public  void UpdateSjqmdsWorkDay(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vInOut, string vNewWorkDay, string vNewDdept, DataTable dataTable, string userCode, string companyCode, string dateFormat, string datetimeFormat)
        {
            string workDay = vWrokDay.Substring(0, 10);
            for (int i=0;i<dataTable.Rows.Count;i++)
            {
                
                string sizeNo = dataTable.Rows[i]["size_no"].ToString();
                string sizeSeq = dataTable.Rows[i]["size_seq"].ToString();
                string move_qty =string.IsNullOrWhiteSpace(dataTable.Rows[i]["move_qty"].ToString())?"0":dataTable.Rows[i]["move_qty"].ToString();
                string sql = @"select sum(b.work_qty) as work_qty,sum(b.finish_qty) as finish_qty,sum(b.supplement_qty) as supplement_qty
                           from sjqdms_work_day a, sjqdms_work_day_size  b where 
                           a.org_id=b.org_id and 
                           a.se_id=b.se_id and 
                           a.se_seq=b.se_seq and
                           a.inout_pz=b.inout_pz and
                           a.column1=b.column1 and 
                           a.work_day=b.work_day and 
                           a.org_id ='" + companyCode + "' and a.se_id='" + vSeId + "' and a.d_dept='" + vDDept + "'"+
                          " and a.work_day =to_date('" + workDay + "','" + dateFormat + "') and  a.inout_pz='" + vInOut + "'"+
                          " and size_no='"+sizeNo+"'";
                DataTable valiTable = DB.GetDataTable(sql);
                if (double.Parse(move_qty)==0)
                {
                    continue;
                }
                if (decimal.Parse(move_qty)>(decimal.Parse(valiTable.Rows[0]["work_qty"].ToString())+ decimal.Parse(valiTable.Rows[0]["supplement_qty"].ToString()) - decimal.Parse(valiTable.Rows[0]["finish_qty"].ToString())))
                {
                    throw new Exception("move_qty error") ;
                }
                else
                {
                    #region
                    string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='" + userCode + "'";
                    string grt_dept = DB.GetString(sqlDept);
                    string po = "";
                    string se_day = "";
                    string rout_no = "";
                    string poSql = "select mer_po from mv_se_ord_m " +
                        "where  org_id='" + companyCode + "' and se_id='" + vSeId + "'";
                    System.Data.DataTable seOrdM = DB.GetDataTable(poSql);
                    if (seOrdM.Rows.Count > 0)
                    {
                        po = seOrdM.Rows[0]["MER_PO"].ToString();
                    }

                    string art_no = "";
                    string artSql = @"select * from mv_se_ord_item " +
                       "where  org_id='" + companyCode + "' and se_id='" + vSeId + "' and se_seq='1'";
                    System.Data.DataTable seOrdItem = DB.GetDataTable(artSql);
                    if (seOrdItem.Rows.Count > 0)
                    {
                        art_no = seOrdItem.Rows[0]["PROD_NO"].ToString();
                        se_day = ((DateTime)seOrdItem.Rows[0]["LPD"]).ToString(dateFormat);
                    }
                    string sql1 = @"select * from BASE005M " +
                    "where  department_code='" + vDDept + "'";
                    System.Data.DataTable base005M = DB.GetDataTable(sql1);
                    if (base005M.Rows.Count > 0)
                    {
                        rout_no = base005M.Rows[0]["UDF01"].ToString();
                    }
                    #endregion
                    UpdateSjqmdsWorkDay(DB,vSeId,vDDept,vWrokDay,vInOut,companyCode, move_qty, dateFormat, datetimeFormat);
                    UpdateSjqmdsWorkDaySize(DB, vSeId, vDDept, vWrokDay, vInOut, companyCode, move_qty,sizeNo, dateFormat, datetimeFormat);
                    MoveSjqmdsWorkDay(DB, vSeId, vNewDdept, vNewWorkDay, vInOut, userCode,companyCode, move_qty,po,se_day, art_no, grt_dept,rout_no, dateFormat, datetimeFormat);
                    MoveSjqmdsWorkDaySize(DB, vSeId, vNewDdept, vNewWorkDay, vInOut, userCode,companyCode, move_qty,sizeNo, sizeSeq, grt_dept, dateFormat, datetimeFormat);
                }
            }
        }

        private void MoveSjqmdsWorkDay(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vInOut, string userCode,string companyCode, string move_qty,
            string po,string se_day,string art_no,string grt_dept,string rout_no, string dateFormat, string datetimeFormat)
        {
            string  workDay = DateTime.Parse(vWrokDay).ToString(dateFormat);
            decimal moveQty = decimal.Parse(move_qty);
            string sql = @"select * from sjqdms_work_day a where "+
                         "a.org_id ='" + companyCode + "' and a.se_id='" + vSeId + "' and a.d_dept='" + vDDept + "'"+
                         " and a.work_day =to_date('" + workDay + "','" + dateFormat + "') and  a.inout_pz='" + vInOut + "' and Column1='U' order by column1";
            DataTable dt = DB.GetDataTablebyline(sql);
            if (dt.Rows.Count<1)
            {
                insertSjqdmsWorkDay(DB, companyCode, vSeId, po, se_day, grt_dept, userCode, vDDept, workDay, rout_no, moveQty, art_no,0, vInOut,DateTime.Now.ToString(dateFormat), dateFormat, datetimeFormat);
            }
            else
            {
                string updateSql = "update sjqdms_work_day a set work_qty=work_qty+"+ moveQty+ " where "+
                                   "a.org_id ='" + companyCode + "' and a.se_id='" + vSeId + "' and a.d_dept='" + vDDept + "'"+
                                   " and a.work_day =to_date('" + workDay + "','" + dateFormat + "') and  a.inout_pz='" + vInOut + "' and Column1='U'";
                DB.ExecuteNonQuery(updateSql);
            }
        }

        private void insertSjqdmsWorkDay(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id, string po,
          string se_day, string grt_dept, string grt_user, string d_dept, string workDate, string routNo,
          decimal work_qty, string art_no, decimal supplement_qty, string vInOut, string thisTime, string dateFormat, string datetimeFormat)
        {
            string sql = @"insert into SJQDMS_WORK_DAY(org_id,se_id,se_seq,po,se_day,
                             column1,status,grt_dept,grt_user,last_user,
                             d_dept,work_day,rout_no,work_qty,art_no,
                             supplement_qty,inout_pz,insert_date)
                             values('" + companyCode + "','" + se_id + "','1','" + po + "',to_date('" + se_day.Substring(0, 10) + "','" + dateFormat + "')," +
                             "'U','7','" + grt_dept + "','" + grt_user + "','" + grt_user + "'," +
                             "'" + d_dept + "',to_date('" + workDate + "','" + dateFormat + "'),'" + routNo + "'," + work_qty + ",'" + art_no + "'," +
                             supplement_qty + ",'" + vInOut + "',to_date('" + thisTime + "','" + datetimeFormat + "'))";
            DB.ExecuteNonQuery(sql);
        }

        private void MoveSjqmdsWorkDaySize(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vInOut, 
            string userCode,string companyCode, string move_qty, string sizeNo, string size_seq, string grt_dept, string dateFormat, string datetimeFormat)
        {
            string workDay = DateTime.Parse(vWrokDay).ToString(dateFormat);
            decimal moveQty = decimal.Parse(move_qty);
            string sql = @"select *
                           from sjqdms_work_day_size a where a.org_id ='" + companyCode + "' and a.se_id='" + vSeId + "' and a.d_dept='" + vDDept + "'"+
                         " and a.work_day =to_date('" + workDay + "','" + dateFormat + "') and  a.inout_pz='" + vInOut + "' and Column1='U' and size_no='"+sizeNo+"' order by column1";
            DataTable dt = DB.GetDataTablebyline(sql);
            if (dt.Rows.Count < 1)
            {
                insertSjqdmsWorkDaySize(DB, companyCode,vSeId,sizeNo,size_seq, grt_dept, userCode, vDDept, workDay,moveQty,0,vInOut, DateTime.Now.ToString(dateFormat), dateFormat, datetimeFormat);
            }
            else
            {
                string updateSql = "update sjqdms_work_day_size a set work_qty=work_qty+" + moveQty + " where "+
                         " a.org_id ='" + companyCode + "' and a.se_id='" + vSeId + "' and a.d_dept='" + vDDept + "'"+
                         " and a.work_day =to_date('" + workDay + "','" + dateFormat + "') and  a.inout_pz='" + vInOut + "' and Column1='U' and size_no='"+sizeNo+"'";
                DB.ExecuteNonQuery(updateSql);
            }
        }


       
        
        private void insertSjqdmsWorkDaySize(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id, string size_no,
           string size_seq, string grt_dept, string grt_user, string d_dept, string workDate,
           decimal work_qty, decimal supplement_qty, string vInOut, string thisTime, string dateFormat, string datetimeFormat)
        {
            string sql = @"insert into sjqdms_work_day_size(
                          ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,
                          STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                          WORK_DAY,WORK_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,
                          COLUMN1,COLUMN2,INOUT_PZ) VALUES(
                          '" + companyCode + "','" + se_id + "','1','" + size_no + "','" + size_seq + "'," +
                          "'7','" + grt_dept + "','" + grt_user + "','" + grt_user + "'," + "'" + d_dept +
                          "',to_date('" + workDate + "','" + dateFormat + "')," + work_qty + ",to_date('" + thisTime + "', '" + datetimeFormat + "') ,to_date('" + thisTime + "', '" + datetimeFormat + "')," + supplement_qty + "," +
                          "'U','Y','" + vInOut + "')";
            DB.ExecuteNonQuery(sql);
        }

        

        private void UpdateSjqmdsWorkDaySize(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vInOut, string companyCode, string move_qty, string sizeNo, string dateFormat, string datetimeFormat)
        {
            decimal moveQty=decimal.Parse(move_qty);
            decimal temp_move_qty = 0;
            string workDay = vWrokDay.Substring(0, 10);
            string sql = @"select b.column1,b.work_qty as work_qty,b.finish_qty as finish_qty,b.supplement_qty as supplement_qty
                           from sjqdms_work_day a, sjqdms_work_day_size  b where 
                           a.org_id=b.org_id and 
                           a.se_id=b.se_id and 
                           a.se_seq=b.se_seq and
                           a.inout_pz=b.inout_pz and
                           a.column1=b.column1 and 
                           a.work_day=b.work_day and a.org_id ='" + companyCode + "' and a.se_id='" + vSeId + "' and a.d_dept='" + vDDept + "'"+
                        "  and a.work_day =to_date('" + workDay + "','" + dateFormat + "')  and  a.inout_pz='" + vInOut + "' and b.size_no='"+ sizeNo+"'";
            DataTable dt = DB.GetDataTable(sql);
            for (int i=0;i<dt.Rows.Count;i++)
            {
                string column1 = dt.Rows[i]["column1"].ToString();
                decimal work_qty = decimal.Parse(dt.Rows[i]["work_qty"].ToString());
                decimal finish_qty = decimal.Parse(dt.Rows[i]["finish_qty"].ToString());
                decimal supplement_qty = decimal.Parse(dt.Rows[i]["supplement_qty"].ToString());
                temp_move_qty = (work_qty + supplement_qty - finish_qty) < moveQty ? (work_qty + supplement_qty - finish_qty):moveQty;
                moveQty = moveQty - temp_move_qty;
                string updateSql = "update sjqdms_work_day_size set work_qty=work_qty-"+ temp_move_qty+" where ";
                updateSql += "org_id ='" + companyCode + "' and se_id='" + vSeId + "' and d_dept='" + vDDept + "'";
                updateSql += " and work_day =to_date('" + workDay + "','" + dateFormat + "') and  inout_pz='" + vInOut + "' and column1='"+ column1+ "'and  size_no='" + sizeNo + "'";
                DB.ExecuteNonQuery(updateSql);
            }
        }

        private void UpdateSjqmdsWorkDay(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vInOut, string companyCode, string move_qty, string dateFormat, string datetimeFormat)
        {
            decimal moveQty = decimal.Parse(move_qty);
            decimal temp_move_qty = 0;
            string workDay = vWrokDay.Substring(0, 10);
            string sql = @"select column1,work_qty,finish_qty,supplement_qty
                           from sjqdms_work_day a where a.org_id ='" + companyCode + "' and a.se_id='" + vSeId + "' and a.d_dept='" + vDDept + "'"+
                         " and a.work_day =to_date('" + workDay + "','" + dateFormat + "') and  a.inout_pz='" + vInOut + "' order by column1";
            DataTable dt = DB.GetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string column1 = dt.Rows[i]["column1"].ToString();
                decimal work_qty = decimal.Parse(dt.Rows[i]["work_qty"].ToString());
                decimal finish_qty = decimal.Parse(dt.Rows[i]["finish_qty"].ToString());
                decimal supplement_qty = decimal.Parse(dt.Rows[i]["supplement_qty"].ToString());
                temp_move_qty = (work_qty + supplement_qty - finish_qty) < moveQty ? (work_qty + supplement_qty - finish_qty) : moveQty;
                moveQty = moveQty - temp_move_qty;
                string updateSql = "update sjqdms_work_day set work_qty=work_qty-" + temp_move_qty + " where ";
                updateSql += "org_id ='" + companyCode + "' and se_id='" + vSeId + "' and d_dept='" + vDDept + "'";
                updateSql += " and work_day =to_date('" + workDay + "','" + dateFormat + "') and  inout_pz='" + vInOut + "' and column1='" + column1 + "'";
                DB.ExecuteNonQuery(updateSql);
            }
        }

        public void InsertMesMoveD(DataBase DB, string vMoveNo, string vMoveDate, string vSeId, string vDDept, string vWrokDay, string vInOut, string vNewWorkDay, string vNewDdept, DataTable dataTable, string userCode, string companyCode, string dateFormat, string datetimeFormat)
        {   
            for (int i=0;i<dataTable.Rows.Count;i++)
            {
                string sizeNo = dataTable.Rows[i]["size_no"].ToString();
                string sizeSeq = dataTable.Rows[i]["size_seq"].ToString();
                string old_work_qty= dataTable.Rows[i]["work_qty"].ToString();
                string old_supplement_qty = dataTable.Rows[i]["SUPPLEMENT_QTY"].ToString(); 
                string old_finish_qty= dataTable.Rows[i]["finish_qty"].ToString();
                string move_qty = dataTable.Rows[i]["move_qty"].ToString();
                if (!string.IsNullOrWhiteSpace(move_qty) &&double.Parse(move_qty) >0)
                {
                     string sql = "insert into mes_move_d(" +
                    "ORG_ID,MOVE_NO,SIZE_NO,SIZE_SEQ,OLD_WORK_QTY," +
                    "OLD_SUPPLEMENT_QTY,MOVE_QTY) " +
                    "values('" + companyCode + "','" + vMoveNo + "','" + sizeNo + "','" + sizeSeq + "','" + old_work_qty +
                    "','" + old_supplement_qty + "','" + move_qty + "') ";
                     DB.ExecuteNonQuery(sql);
                }
            }  
        }
    }
}
