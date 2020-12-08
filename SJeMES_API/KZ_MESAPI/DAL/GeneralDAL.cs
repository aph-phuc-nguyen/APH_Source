using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class GeneralDAL
    {
        public DataTable GetDept(DataBase DB, string userId)
        {
            string sql = "select a.STAFF_DEPARTMENT,b.Department_Name FROM hr001m a left join base005m b on a.STAFF_DEPARTMENT = b.department_code where 1=1 ";
            sql += " and a.staff_no='" + userId + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public int GetDayFinishQty(DataBase DB, string companyCode, string vDDept, string vInOut, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql = "select nvl(sum(finish_qty),0) from sjqdms_work_day where 1=1 ";
            sql += " and status = '7'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            sql += " and ORG_ID='" + companyCode + "'";

            return DB.GetInt32(sql);
        }

        public DataTable GetSeId_Po(DataBase DB, string companyCode, string vDDept, string vInOut, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            string sql = "select SE_ID,PO,ART_NO,WORK_QTY,FINISH_QTY from vw_sjqdms_work_day where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            sql += " and (WORK_QTY + SUPPLEMENT_QTY) > FINISH_QTY";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public string GetArtName(DataBase DB, string companyCode, string vArtNo)
        {
            string sql = "select GF_PROD_NAME_MG('" + companyCode + "','" + vArtNo + "','T') as NAME_T from dual";
            return DB.GetString(sql);
        }

        public string GetPoBySeid(DataBase DB, string vSeId)
        {
            string sql = "SELECT MER_PO FROM mv_se_ord_m WHERE 1=1";
            sql += " and SE_ID='" + vSeId + "'";
            return DB.GetString(sql);
        }

        public DataTable GetAllDepts(DataBase DB)
        {
            string sql = "SELECT DEPARTMENT_CODE,DEPARTMENT_NAME FROM base005m";
            return DB.GetDataTable(sql);
        }

        public DataTable GetWorkDaySize(DataBase DB, string companyCode, string vDDept, string vInOut, string vSeId, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            string sql = "select * from VW_SJQDMS_WORK_DAY_SIZE where 1=1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            sql += " and WORK_QTY > FINISH_QTY";

            sql += " order by size_seq asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            return dt;
        }

        public int GetDaySizeWorkQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vInOut, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            int qty = 0;
            string sql = "select nvl(sum(WORK_QTY),0) + nvl(sum(SUPPLEMENT_QTY),0) from sjqdms_work_day_size where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and se_seq = '" + vSeSeq + "'";
            sql += " and size_no = '" + vSizeNo + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','"+ dateFormat + "')";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            qty = DB.GetInt32(sql);
            return qty;
        }

        public int GetDaySizeFinishQty(DataBase DB, string companyCode, string vDDept, string vSeId, string vSeSeq, string vSizeNo, string vInOut, string dateFormat)
        {
            DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            int qty = 0;
            string sql = "select nvl(sum(finish_qty),0) from sjqdms_work_day_size where 1=1 ";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and status = '7'";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and se_seq = '" + vSeSeq + "'";
            sql += " and size_no = '" + vSizeNo + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and work_day = to_date('" + workDate.ToString(dateFormat) + "','" + dateFormat + "')";
            sql += " and INOUT_PZ = '" + vInOut + "'";
            qty = DB.GetInt32(sql);
            return qty;
        }

        public DataTable GetInOutDetail(DataBase DB, string companyCode, string vDDept, string vPo, string vSeId)
        {
            string sql = "select SE_ID,PO,SIZE_NO,SIZE_SEQ,D_DEPT,PLAN_QTY,IN_QTY,OUT_QTY from VW_SJQDMS_SIZE_INOUT_DETAIL where 1=1";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += " and d_dept = '" + vDDept + "'";
            sql += " and se_id like '%" + vSeId + "%'";
            sql += " and po like '%" + vPo + "%'";

            sql += " order by size_seq asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);

            return dt;
        }

    }
}

