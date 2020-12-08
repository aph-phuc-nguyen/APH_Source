using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class ProductionReportDAL
    {
        public DataTable GetWorkDaySizeReport(DataBase DB, string CompanyCode, string vSeId, string vPO, string vDept, string vArt, string vInOut)
        {
            string sql_seid = string.IsNullOrEmpty(vSeId) ? "" : " and SE_ID like '" + vSeId + "%'";
            string sql_dept = string.IsNullOrEmpty(vDept) ? "" : " and D_DEPT like '" + vDept + "%'";
            string sql_PO = string.IsNullOrEmpty(vPO) ? "" : " and MER_PO like '" + vPO + "%'";
            string sql_Art = string.IsNullOrEmpty(vArt) ? "" : " and PROD_NO like '" + vArt + "%'";

            string sql = "select * from VW_SJQDMS_WORK_DAY_REPORT_M where 1=1 " +
            " and ORG_ID = '" + CompanyCode + "'" +
            sql_seid +
            sql_dept +
            sql_PO +
            sql_Art +
            " and INOUT_PZ = '" + vInOut + "'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetWorkDayReport(DataBase DB, string CompanyCode, string vSeId, string vWrokDay, string vDept, string vPO, string vArt, string vStatus, string vInOut)
        {
            //DateTime workDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            //string sql = "select * from MES00.VW_SJQDMS_WORK_DAY_SIZE where 1=1 ";
            //sql += " and ORG_ID='" + companyCode + "'";
            //sql += " and status = '7'";
            //sql += " and se_id = '" + vSeId + "'";
            //sql += " and d_dept = '" + vDDept + "'";
            //sql += " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')";
            //sql += " and INOUT_PZ = 'IN'";
            //sql += " and WORK_QTY + SUPPLEMENT_QTY > 0";
            //sql += " order by size_seq asc";

            DateTime workDate = DateTime.Parse(vWrokDay);
            string sql_seid = string.IsNullOrEmpty(vSeId) ? "" : " and SE_ID like '" + vSeId + "%'";
            string sql_dept = string.IsNullOrEmpty(vDept) ? "" : " and D_DEPT like '" + vDept + "%'";
            string sql_PO = string.IsNullOrEmpty(vPO) ? "" : " and PO like '" + vPO + "%'";
            string sql_Art = string.IsNullOrEmpty(vArt) ? "" : " and ART_NO like '" + vArt + "%'";
            string sql_status = vStatus.Equals("107") ? "" : " and STATUS = '" + vStatus + "'";

            string sql = "select * from VW_SJQDMS_WORK_DAY_REPORT where 1=1 " +
            " and ORG_ID = '" + CompanyCode + "'" +
            sql_seid +
            " and work_day = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')" +
            sql_dept +
            sql_PO +
            sql_Art +
            sql_status +
            " and INOUT_PZ = '" + vInOut + "'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetMesLableDReport(DataBase DB, string CompanyCode, string vSeId, string vPO, string vWrokDay, string vDept, string vArt)
        {
            string workDate = DateTime.Parse(vWrokDay).ToString("yyyy/MM/dd");
            string sql_seid = string.IsNullOrEmpty(vSeId) ? "" : " and SE_ID like '" + vSeId + "%'";
            string sql_PO = string.IsNullOrEmpty(vPO) ? "" : " and PO_NO like '" + vPO + "%'";
            string sql_workDay = " and to_char(SCAN_DATE,'yyyy/mm/dd') = '" + workDate + "'";
            string sql_dept = string.IsNullOrEmpty(vDept) ? "" : " and SCAN_DETPT like '" + vDept + "%'";
            string sql_Art = string.IsNullOrEmpty(vArt) ? "" : " and ART_NO like '" + vArt + "%'";

            string sql = "select * from VW_MES_LABEL_D_REPORT where 1=1 " +
            " and ORG_ID = '" + CompanyCode + "'" +
            sql_seid +
            sql_PO +
            sql_workDay +
            //" and SCAN_DATE = to_date('" + workDate.ToShortDateString() + "','yyyy/mm/dd')" +
            sql_dept +
            sql_Art;

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
    }
}
