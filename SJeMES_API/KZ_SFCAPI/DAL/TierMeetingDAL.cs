using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_SFCAPI.DAL
{
    class TierMeetingDAL
    {
        public DataTable TabPage2_Query(DataBase DB, string userCode,string date, string line,string dateFormat)
        {
            string sql = string.Format(@"select audit_line,audit_date,audit_type,audit_item,audit_seq,audit_name,audit_keypoint,audit_person,audit_memo,audit_ip,audit_result  
                                         from sfc_execute_audit_list where audit_date=to_date('{0}','{1}') and audit_type='A' and audit_item='A' and audit_line='{2}' ORDER BY audit_seq", date,dateFormat,line);
            return  DB.GetDataTable(sql);
        }


        public DataTable TabPage3_Query(DataBase DB, string userCode, string date, string line,string dateFormat)
        {
            string sql = string.Format(@"select audit_line,audit_date,audit_type,audit_item,audit_seq,audit_name,audit_keypoint,audit_person,audit_memo,audit_ip,audit_result 
                                         from sfc_execute_audit_list where audit_date=to_date('{0}','{1}') and audit_type='B'  and audit_line='{2}' ORDER BY audit_item,audit_seq", date, dateFormat, line);
            return DB.GetDataTable(sql);
        }
    }
}
