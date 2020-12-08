using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TierMeeting.DAL
{
    class FollowTierMeetingDAL
    {
        private enum QueryDeptType : int
        {
            All = 0,
            Plant = 1,
            Section = 2,
            Line = 3
        }
        public DataTable TMS_TIER_Query(DataBase DB, string userId, string CompanyCode, string dept,string date, string dateFormat,string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                type = "0";
            string sql1 = @"select to_char(a.g_created_date,'dd') as g_day, b.DEPARTMENT_CODE as g_deptcode, to_char(a.g_created_date,'" + dateFormat+ "') as g_date,a.g_result,a.g_auditor, a.g_created_date,a.g_created_by,a.g_last_update_date, a.g_updated_by,a.g_1,a.g_2,a.g_3,a.g_4,a.g_5,a.g_6,a.g_7,a.g_8 " +
                          " from TMS_TIER1 a right join BASE005M b on b.DEPARTMENT_CODE=a.G_DEPTCODE " +
                          "where b.udf05 is not null and b.udf07 is not null and b.udf06='Y' ";
            if (!String.IsNullOrWhiteSpace(dept))
            {
                if (int.Parse(type) == (int)QueryDeptType.Plant)
                {
                    sql1 += " and b.udf05='" + dept + "' ";
                }
                else
                if (int.Parse(type) == (int)QueryDeptType.Section)
                {
                    sql1 += " and b.udf07='" + dept + "' ";
                }
                else
                if (int.Parse(type) == (int)QueryDeptType.Line)
                {
                    sql1 += " and b.DEPARTMENT_CODE='" + dept + "' ";
                }
            }
            if (!String.IsNullOrWhiteSpace(date))
                sql1 += " and (TO_CHAR(a.g_created_date, 'yyyy/MM' )= '" + date + "' or a.g_created_date is null) ";
              //  sql1 += " and (a.g_date=to_date('"+DateTime.Parse(date).ToString(dateFormat)+"','"+dateFormat+"') or a.g_date is null )";
            sql1+= " order by b.DEPARTMENT_CODE,a.g_created_date";     
            System.Data.DataTable MES_LABEL_D = DB.GetDataTable(sql1);
            return MES_LABEL_D;
        }
        public DataTable TMS_TIER1_STANDARD_Query(DataBase DB, string userId, string CompanyCode, string dept, string date, string dateFormat, string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                type = "0";//to_char(a.g_date,'" + dateFormat + "') as g_date
            string sql1 = @"select to_char(a.g_created_date,'dd') as g_day, b.DEPARTMENT_CODE as g_deptcode, to_char(a.g_created_date,'" + dateFormat + "') as g_date,G_SUPERVISOR_AUDITOR,G_VSM_AUDITOR,G_THIRD_PARTY_AUDITOR, a.g_created_date,a.g_created_by,a.g_last_update_date, a.g_updated_by," +
                          " G_SUPERVISOR_1,G_SUPERVISOR_2,G_SUPERVISOR_3,G_SUPERVISOR_4,G_SUPERVISOR_5,G_SUPERVISOR_6,G_SUPERVISOR_7,G_SUPERVISOR_8,"+
                          " G_VSM_1,G_VSM_2,G_VSM_3,G_VSM_4,G_VSM_5,G_VSM_6,G_VSM_7,G_VSM_8," +
                          " G_THIRD_PARTY_1,G_THIRD_PARTY_2,G_THIRD_PARTY_3,G_THIRD_PARTY_4,G_THIRD_PARTY_5,G_THIRD_PARTY_6,G_THIRD_PARTY_7,G_THIRD_PARTY_8 " +
           " from TMS_TIER1_STANDARD a right join BASE005M b on b.DEPARTMENT_CODE=a.G_DEPTCODE " +
                          "where b.udf05 is not null and b.udf07 is not null and b.udf06='Y' ";
            if (!String.IsNullOrWhiteSpace(dept))
            {
                if (int.Parse(type) == (int)QueryDeptType.Plant)
                {
                    sql1 += " and b.udf05='" + dept + "' ";
                }
                else
                if (int.Parse(type) == (int)QueryDeptType.Section)
                {
                    sql1 += " and b.udf07='" + dept + "' ";
                }
                else
                if (int.Parse(type) == (int)QueryDeptType.Line)
                {
                    sql1 += " and b.DEPARTMENT_CODE='" + dept + "' ";
                }
            }
            if (!String.IsNullOrWhiteSpace(date))
                sql1 += " and (TO_CHAR(a.g_created_date, 'yyyy/MM' )= '" + date + "' or a.g_created_date is null)";
            sql1 += " order by b.DEPARTMENT_CODE,a.g_created_date";
            System.Data.DataTable MES_LABEL_D = DB.GetDataTable(sql1);
            return MES_LABEL_D;
        }
        public DataTable GetMaturityAssessmentList(DataBase DB, string deptCode,string type, string date,string dateFormat)
        {
            if (string.IsNullOrWhiteSpace(type))
                type = "0";         
            string sql1 = "";
            if (!String.IsNullOrWhiteSpace(deptCode))
            {

               
                if (int.Parse(type) == (int)QueryDeptType.Plant)
                {
                    sql1 += "select distinct udf05 as deptcode from base005m WHERE " +
 "  udf05='" + deptCode + "'"+
 "   AND udf06 = 'Y' " +
 "union " +
 "select distinct udf07 as deptcode from base005m WHERE " +
 "  udf05='" + deptCode + "'" +
 "   AND udf07 IS NOT NULL " +
 "    AND udf06 = 'Y' ";
                }
                else
                if (int.Parse(type) == (int)QueryDeptType.Section)
                {
                    sql1 +=
  "select distinct udf07 as deptcode from base005m WHERE " +
  "  udf07='" + deptCode + "'" +
  "   AND udf05 IS NOT NULL " +
  "    AND udf06 = 'Y' ";
                }                
            }
            else
            {
                sql1 += "select distinct udf05 as deptcode from base005m WHERE " +
 "  udf05 IS NOT NULL" +
 "   AND udf06 = 'Y'" +
 "union " +
 "select distinct udf07 as deptcode from base005m WHERE " +
 "  udf05 IS NOT NULL " +
 "   AND udf07 IS NOT NULL " +
 "    AND udf06 = 'Y' ";
            }              
            string sql = "select  to_char(k.updateddate,'dd') as g_day, h.deptcode,to_char(k.updateddate,'"+dateFormat+"') as updateddate,k.maturitycode,k.status,k.note,k.namecn, k.nameen, k.nameyn,k.code  from ( " +
 sql1+
 "   ) h " +
"left join( " +
"    SELECT " +
 "   a.deptcode, " +
 "   updateddate, " +
  "  a.maturitycode, " +
   " a.status, " +
 "   a.note, " +
 "   b.namecn, " +
 "   b.nameen, " +
 "   b.nameyn, " +
 "   b.code " +
"FROM " +
"    ( " +
 "       SELECT " +
 "           * " +
 "       FROM " +
 "           tms_maturity_assessment_list " +
"        WHERE " +
 "           1 = 1 ";
            if (!String.IsNullOrWhiteSpace(date))
                sql += " and TO_CHAR(updateddate, 'yyyy/MM' )  = '" + date+ "' ";           
            sql+=""+
"    ) a " +
 "   JOIN tms_maturity_list  b ON a.maturitycode = b.code " +
 "   ) k on h.deptcode = k.deptcode order by h.deptcode,updateddate,maturitycode"; 
                      DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetMaturityList(DataBase DB)
        {
            string sql = @"SELECT * FROM  TMS_MATURITY_LIST order by code asc";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
    }
}
