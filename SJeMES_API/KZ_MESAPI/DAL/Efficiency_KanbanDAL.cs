using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class Efficiency_KanbanDAL
    {
        public  DataTable ThirdEfficiency_Query(DataBase DB, string userCode, string compnayCode,string date,string udf05,string dateFormat)
        {
            string tomorrow = DateTime.Parse(date).AddDays(1).ToString(dateFormat);
            string sql =string.Format(@"select 
                        e.department_code as dept,
                        label_qty,
                        work_qty,
                        JOCKEY_QTY,
                        PLURIPOTENT_WORKER,
                        OMNIPOTENT_WORKER,
                        C.UDF01 as Water_Spider,
                        WORK_HOURS,
                        nvl(TRANIN_HOURS,0) as TRANIN_HOURS,
                        nvl(TRANOUT_HOURS,0) as TRANOUT_HOURS,
                        trunc(DECODE((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS,0,0,work_qty/((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS+nvl(TRANIN_HOURS,0)-nvl(TRANOUT_HOURS,0))),2) as TARGETPPH,
                        trunc(DECODE((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS,0,0,LABEL_QTY/((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS+nvl(TRANIN_HOURS,0)-nvl(TRANOUT_HOURS,0))),2) as PPH
                        from base005m e left join (
                        select scan_detpt,sum(label_qty) as label_qty from mes_label_d where   inout_pz='OUT' and scan_date between to_date('{0}','{1}') and to_date('{2}','{3}')
                        group by scan_detpt) a on e.department_code=a.scan_detpt
                        left join (select d_dept,WORK_QTY from sjqdms_worktarget where WORK_DAY=to_date('{4}','{5}')) b   on  a.scan_detpt=b.d_dept
                        left join (select * from mes_dept_manpower where  begin_day<=to_date('{6}','{7}')  and (end_day>to_date('{8}','{9}') or end_day is null)) c  on  a.scan_detpt=c.d_dept
                        left join (select * from mes_workinghours_01 WHERE WORK_DAY=to_date('{10}','{11}')) d on  a.scan_detpt=d.d_dept
                        where e.udf05='{12}' and e.udf06='Y' order by e.department_code
                        ",date,dateFormat,tomorrow,dateFormat,date, dateFormat,date, dateFormat, date, dateFormat, date, dateFormat,udf05);
            return DB.GetDataTable(sql);
        }

        public string QueryDeptNo(DataBase DB, string userCode, string compnayCode)
        {
            string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='" + userCode + "'";
            string deptNo = DB.GetString(sqlDept);
            return deptNo;
        }

        public DataTable SetionEfficiency_Query(DataBase DB, string userCode, string compnayCode, string date, string section,string dateFormat)
        {
            string tomorrow = DateTime.Parse(date).AddDays(1).ToString(dateFormat);
            string sql = string.Format(@"
                        select 
                        e.department_code as dept ,
                        label_qty,
                        work_qty,
                        JOCKEY_QTY,
                        PLURIPOTENT_WORKER,
                        OMNIPOTENT_WORKER,
                        C.UDF01 as Water_Spider,
                        WORK_HOURS,
                        nvl(TRANIN_HOURS,0) as TRANIN_HOURS,
                        nvl(TRANOUT_HOURS,0) as TRANOUT_HOURS,
                        trunc(DECODE((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS,0,0,work_qty/((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS+nvl(TRANIN_HOURS,0)-nvl(TRANOUT_HOURS,0))),2) as TARGETPPH,
                        trunc(DECODE((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS,0,0,LABEL_QTY/((NVL(JOCKEY_QTY,0)+nvl(C.UDF01,0))*WORK_HOURS+nvl(TRANIN_HOURS,0)-nvl(TRANOUT_HOURS,0))),2) as PPH
                        from base005m e left join (
                        select scan_detpt,sum(label_qty) as label_qty from mes_label_d where  inout_pz='OUT' and scan_date between to_date('{0}','{1}') and to_date('{2}','{3}'
                        group by scan_detpt) a on e.department_code=a.scan_detpt
                        left join (select d_dept,WORK_QTY from sjqdms_worktarget where WORK_DAY=to_date('{4}','{5}')) b   on  a.scan_detpt=b.d_dept
                        left join (select * from mes_dept_manpower where  begin_day<=to_date('{6}','{7}')  and end_day>to_date('{8}','{9}') or end_day is null) c  on  a.scan_detpt=c.d_dept                       
                        left join (select * from mes_workinghours_01 WHERE WORK_DAY=to_date('{10}','{11}')) d on  a.scan_detpt=d.d_dept
                        where e.udf07='{12}' and e.udf06='Y' order by e.department_code
                        ", date,dateFormat,tomorrow, dateFormat, date, dateFormat, date, dateFormat, date, dateFormat, date, dateFormat, section);
            return DB.GetDataTable(sql);
        }

        public string QueryPlant(DataBase DB, string userCode, string compnayCode)
        {
            string getPlantSql = @"
                        SELECT UDF05 FROM BASE005M WHERE DEPARTMENT_CODE=(
                        SELECT STAFF_DEPARTMENT FROM HR001M WHERE STAFF_NO='" + userCode + @"')
                          ";
            string udf05 = DB.GetString(getPlantSql);
            return udf05;
        }

        internal decimal TotalQty(DataBase DB, string userCode, string compnayCode, string date,string dateFormat)
        {
            string tomorrow = DateTime.Parse(date).AddDays(1).ToString(dateFormat);
            string sql = string.Format("select  nvl(sum(label_qty),0) from mes_label_d where  inout_pz='OUT' and  scan_date between to_date('{0}','{1}') and to_date('{2}','{3}')",date,dateFormat,tomorrow,dateFormat);
            return DB.GetDecimal(sql);
        }

        internal decimal QueryWorkIngHours(DataBase DB, string userCode, string compnayCode, string date,string dateFormat)
        {
            string tomorrow = DateTime.Parse(date).AddDays(1).ToString(dateFormat);
            decimal hours =0;
            //string sql = string.Format(@"select  distinct scan_detpt,nvl(b.work_hours,0) as work_hours from mes_label_d  a
            //               left join mes_workinghours_01 b  on a.scan_detpt = b.d_dept and a.scan_date between b.work_day and b.work_day+1  
            //               where inout_pz = 'OUT'  and  scan_date between to_date('{0}','{1}') and to_date('{2}','{3}')", date, dateFormat, tomorrow, dateFormat);
            string sql = "select  GF_COMPANYMANPOWER(date) as  work_hours from dual";
            DataTable table = DB.GetDataTable(sql);
            for (int i=0;i<table.Rows.Count;i++)
            {
                if (table.Rows[i]["work_hours"].ToString().Equals("0"))
                {
                    return 0;
                }
                else
                {
                    hours += decimal.Parse(table.Rows[i]["work_hours"].ToString());
                }
            }
            return hours;
        }

            public string QueryWorkDate(DataBase DB, string userCode, string compnayCode)
        {
            string sql = "select max(to_char(work_day,'yyyy/mm/dd')) from mes_workinghours_01   where to_char(work_day,'yyyy/mm/dd')<to_char(SYSDATE,'yyyy/mm/dd')";
            return DB.GetString(sql);
        }

        public DataTable SecondEfficiency_Query(DataBase DB, string userCode, string compnayCode, string date,string dateFormat)
        {
            string tomorrow = DateTime.Parse(date).AddDays(1).ToString(dateFormat);
            string sql = string.Format(@"select 
                        max(f.code)  as code,
                        max(f.org) as area ,
                        sum(nvl(label_qty,0)) as label_qty,
                        sum(nvl(work_qty,0))  as work_qty,
                        sum(nvl(JOCKEY_QTY,0)) as JOCKEY_QTY,
                        sum(nvl(PLURIPOTENT_WORKER,0)) as PLURIPOTENT_WORKER,
                        sum(nvl(OMNIPOTENT_WORKER,0)) as OMNIPOTENT_WORKER,
                        sum(nvl(C.UDF01,0)) as Water_Spider,
                        sum((nvl(c.udf01,0)+nvl(c.JOCKEY_QTY,0))*nvl(WORK_HOURS,0)) as WORK_HOURS,
                        sum(nvl(TRANIN_HOURS,0)) as TRANIN_HOURS,
                        sum(nvl(TRANOUT_HOURS,0)) as TRANOUT_HOURS,
                        trunc(DECODE((sum(nvl(C.UDF01,0))+sum(nvl(JOCKEY_QTY,0)))* sum(nvl(WORK_HOURS,0)),0,0,sum(nvl(work_qty,0))/sum((nvl(c.udf01,0)+nvl(c.JOCKEY_QTY,0))*nvl(WORK_HOURS,0))),2) as TARGETPPH,
                        trunc(DECODE((sum(nvl(C.UDF01,0))+sum(nvl(JOCKEY_QTY,0)))* sum(nvl(WORK_HOURS,0)),0,0,sum(nvl(label_qty,0))/sum((nvl(c.udf01,0)+nvl(c.JOCKEY_QTY,0))*nvl(WORK_HOURS,0))),2) as PPH
                        from sjqdms_orginfo f,base005m e left join (
                        select scan_detpt,sum(label_qty) as label_qty from mes_label_d where  inout_pz='OUT' and scan_date between to_date('{0}','{1}') and to_date('{2}','{3}')
                        group by scan_detpt) a on e.department_code=a.scan_detpt
                        left join (select d_dept,WORK_QTY from sjqdms_worktarget where WORK_DAY=to_date('{4}','{5}')) b   on  a.scan_detpt=b.d_dept
                        left join (select * from mes_dept_manpower where  begin_day<=to_date('{6}','{7}')  and (end_day>to_date('{8}','{9}') or end_day is null)) c on  a.scan_detpt=c.d_dept                
                        left join (select * from mes_workinghours_01 where WORK_DAY=to_date('{10}','{11}')) d on  a.scan_detpt=d.d_dept
                        where e.udf05 is not null and e.udf06='Y'  and e.udf05=f.code and f.mfg='Y'
                        group by e.udf05
                        order by e.udf05
                        ", date,dateFormat,tomorrow,dateFormat,date,dateFormat, date, dateFormat, date, dateFormat,date,dateFormat);
            return DB.GetDataTable(sql);
        }
    }
}
