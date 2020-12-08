using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class ProductionOutputDAL
    {
        public int GetHourQty(DataBase DB, string companyCode, string vDDept, string vInOut)
        {
            DateTime workDate = DateTime.Now;
            string year = workDate.ToString("yyyy");
            string month = workDate.ToString("MM");
            string day = workDate.ToString("dd");
            string hour = workDate.ToString("HH");

            string sql = "select nvl(sum(LABEL_QTY),0) from MES_LABEL_D where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and scan_detpt = '" + vDDept + "'";
            sql += " and to_char(scan_date,'yyyy') = '" + year + "'";
            sql += " and to_char(scan_date,'mm') = '" + month + "'";
            sql += " and to_char(scan_date,'dd') = '" + day + "'";
            sql += " and to_char(scan_date,'hh24') = '" + hour + "'";
            sql += " and INOUT_PZ = '" + vInOut + "'";

            return DB.GetInt16(sql);
        }

        public DataTable GetScanLog(DataBase DB, string companyCode, string vDDept, string vInOut)
        {
            DateTime workDate = DateTime.Now;
            string year = workDate.ToString("yyyy");
            string month = workDate.ToString("MM");
            string day = workDate.ToString("dd");

            string sql = "select se_id,po_no,size_no,art_no,scan_date from MES_LABEL_D where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and scan_detpt = '" + vDDept + "'";
            sql += " and to_char(scan_date,'yyyy') = '" + year + "'";
            sql += " and to_char(scan_date,'mm') = '" + month + "'";
            sql += " and to_char(scan_date,'dd') = '" + day + "'";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            sql += " order by scan_date desc";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            return dt;
        }

        public int GetSeFinishQty(DataBase DB, string companyCode, string vDDept, string vInOut, string vSeId)
        {
            string sql = "select nvl(sum(FINISH_QTY),0) from SJQDMS_WORK_DAY where 1=1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and INOUT_PZ = '" + vInOut + "'";

            return DB.GetInt32(sql);
        }

        public int GetSeQty(DataBase DB, string companyCode, string vDDept, string vInOut, string vSeId)
        {
            string sql = "select nvl(sum(WORK_QTY),0) from SJQDMS_WORK_DAY where 1=1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            sql += " and COLUMN1 = 'U'";

            return DB.GetInt32(sql);
        }

        public DataTable GetOutDetail(DataBase DB, string companyCode, string vDDept, string vInOut, string vSeId ,string vSizeNo)
        {
            string sql = "select WORK_QTY,SUPPLEMENT_QTY,FINISH_QTY,CY_QTY from VW_SJQDMS_WORK_DAY_SIZE_REPORT where 1=1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            sql += " and size_no = '" + vSizeNo + "'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetLabelDetail(DataBase DB, string companyCode, string vLabel)
        {
            string sql = "select SE_ID,SIZE_NO,ART_NO from MES_LABEL_M where 1=1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and LABEL_ID='" + vLabel + "'";
            
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public int GetUnfinishQty(DataBase DB, string companyCode, string vSeId, string vSizeNo, string vDDept, string vInOut, string dateFormat, string dateTimeFormat)
        {
            int qty = 0;
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            string sql = "select * from VW_SJQDMS_WORK_DAY_SIZE where 1=1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and size_no = '" + vSizeNo + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sql += " and INOUT_PZ = '" + vInOut + "'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                qty = Convert.ToInt32(Convert.ToDecimal(dt.Rows[0]["WORK_QTY"].ToString()) - Convert.ToDecimal(dt.Rows[0]["FINISH_QTY"].ToString()));
            }
            return qty;
        }

        public void updateOutFinshQty_trance(DataBase DB, string companyCode, string vDDept, string vSeId, string vSizeNo, string userId, string vIP, string vLabel, string vScanPZ, string dateFormat, string dateTimeFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql_time = "select sysdate from dual";
            DateTime insertDateTemp = DB.GetDateTime(sql_time);
            string insertDate = insertDateTemp.ToString(dateFormat + "HH:mm:ss");
            //DateTime insertDate = DateTime.Now;

            string sqlDaySizeU = "select * from SJQDMS_WORK_DAY_SIZE where 1 = 1";
            sqlDaySizeU += " and ORG_ID='" + companyCode + "'";
            sqlDaySizeU += " and se_id = '" + vSeId + "'";
            sqlDaySizeU += " and d_dept = '" + vDDept + "'";
            sqlDaySizeU += " and size_no = '" + vSizeNo + "'";
            sqlDaySizeU += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sqlDaySizeU += " and INOUT_PZ = 'OUT'";
            sqlDaySizeU += " and COLUMN1 = 'U'";
            sqlDaySizeU += " and WORK_QTY > FINISH_QTY";
            System.Data.DataTable daySizeU = DB.GetDataTable(sqlDaySizeU);

            if (daySizeU.Rows.Count > 0)
            {
                updateSjqdmsWorkDaySize(DB, companyCode, vDDept, vSeId, vSizeNo, userId, insertDate, workDate, "U", "OUT",dateFormat,dateTimeFormat);
                updateSjqdmsWorkDay( DB, companyCode, vDDept, vSeId, userId, insertDate, workDate, "U", "OUT",  dateFormat,  dateTimeFormat);
            }
            else
            {
                updateSjqdmsWorkDaySize(DB, companyCode, vDDept, vSeId, vSizeNo, userId, insertDate, workDate, "S", "OUT", dateFormat, dateTimeFormat);
                updateSjqdmsWorkDay(DB, companyCode, vDDept, vSeId, userId, insertDate, workDate, "S", "OUT", dateFormat, dateTimeFormat);
            }

            string sqlUser = "select * from HR001M where 1 = 1";
            sqlUser += " and STAFF_NO='" + userId + "'";
            System.Data.DataTable User = DB.GetDataTable(sqlUser);

            string sqlDetail = "select * from SJQDMS_WORK_DAY where 1 = 1";
            sqlDetail += " and ORG_ID='" + companyCode + "'";
            sqlDetail += " and se_id = '" + vSeId + "'";
            sqlDetail += " and d_dept = '" + vDDept + "'";
            sqlDetail += " and INOUT_PZ = 'IN'";
            sqlDetail += " and COLUMN1 = 'U'";
            System.Data.DataTable Detail = DB.GetDataTable(sqlDetail);

            string po = Detail.Rows[0]["PO"].ToString();
            string se_day = Detail.Rows[0]["SE_DAY"].ToString();
            string rout_no = Detail.Rows[0]["ROUT_NO"].ToString();
            string art_no = Detail.Rows[0]["ART_NO"].ToString();

            string scan_name = User.Rows[0]["STAFF_NAME"].ToString();

            string sqlMesD = @"insert into MES_LABEL_D(org_id,se_id,se_seq,po_no,LABEL_TYPE,LABEL_QTY,status,
                            grt_dept,grt_user,last_user,SCAN_DETPT,art_no,SIZE_NO,PROCESS_NO,SCAN_PZ,
                            LABEL_ID,inout_pz,SCAN_IP,SCAN_NAME,SCAN_DATE,insert_date,LAST_DATE)
                             values('" + companyCode + "','" + vSeId + "','1','" + po + "','A','1','7','" + vDDept + "','" + userId + "','" + userId +
                             "'," + "'" + vDDept + "','" + art_no + "','" + vSizeNo + "','" + rout_no + "','" + vScanPZ + "','" + vLabel + "','OUT','" + vIP + "','" + scan_name +
                             "',to_date('" + insertDate + "','"+ dateTimeFormat + "'),to_date('" + insertDate + "','yyyy/mm/dd HH24:MI:SS'),to_date('" + insertDate + "','"+dateTimeFormat +"'))";
            DB.ExecuteNonQuery(sqlMesD);
        }

        public void updateOutFinshQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSizeNo, string userId, string vIP, string vLabel, string vScanPZ, string dateFormat, string dateTimeFormat)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                updateOutFinshQty_trance(DB, companyCode, vDDept, vSeId, vSizeNo, userId, vIP, vLabel, vScanPZ, dateFormat,dateTimeFormat);
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

        public void updateSjqdmsWorkDay(DataBase DB, string companyCode, string vDDept, string vSeId, string userId, string insertDate, DateTime workDate, string column1, string inOut, string dateFormat, string dateTimeFormat)
        {
            string sql = "update sjqdms_work_day set " +
                                 "FINISH_QTY = FINISH_QTY + 1," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', '"+dateTimeFormat+"') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " work_day=to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut +"'";
            DB.ExecuteNonQuery(sql);
        }

        public void updateSjqdmsWorkDaySize(DataBase DB, string companyCode, string vDDept, string vSeId, string vSizeNo, string userId, string insertDate, DateTime workDate, string column1, string inOut, string dateFormat, string dateTimeFormat)
        {
            string sql = "update sjqdms_work_day_size set " +
                                 "FINISH_QTY = FINISH_QTY + 1," +
                                 "LAST_USER='" + userId + "'," +
                                 "LAST_DATE=to_date('" + insertDate + "', '"+ dateTimeFormat + "') " +
                                 " WHERE org_id='" + companyCode + "' and " +
                                 " d_dept='" + vDDept + "' and " +
                                 " size_no='" + vSizeNo + "' and " +
                                 " work_day=to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "') and " +
                                 " se_id='" + vSeId + "' and " +
                                 " COLUMN1= '" + column1 + "' and" +
                                 " INOUT_PZ='" + inOut + "'";
            DB.ExecuteNonQuery(sql);
        }

        public DataTable GetScanDept(DataBase DB)
        {
            string sql = "SELECT DEPARTMENT_CODE,DEPARTMENT_NAME FROM base005m WHERE UDF03 = 'Y' ORDER BY DEPARTMENT_CODE ASC";
            return DB.GetDataTable(sql);
        }
    }
}






