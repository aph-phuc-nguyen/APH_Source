using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    class TierMeetingDAL
    {


        public DataTable Tier1_WeekPPH(DataBase DB, string dateFormat, string vLine,string FirstDay,string SeventhDay)
        {
            string sql = string.Format(@"SELECT to_char(scan_date,'{0}') as WORK_DATE,sum(label_qty) as qty,GF_LINEMANPOWER('{1}',to_char(scan_date,'{2}')) as MANPOWER from  (           
       SELECT  scan_date,label_qty  from mes_label_d,base005m 
  where   
  inout_pz='OUT' 
  and  scan_date between  to_date('{3}','{4}') and to_date('{5}','{6}')
  and department_code=scan_detpt and department_code='{7}')
  group by  to_char(scan_date,'{8}') order by to_char(scan_date,'{9}')", dateFormat, vLine, dateFormat, FirstDay, dateFormat, SeventhDay, dateFormat, vLine, dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }


        public DataTable Tier2_WeekPPH(DataBase DB, string dateFormat, string vSection,string FirstDay, string SeventhDay)
        {
            string sql = string.Format(@"SELECT to_char(scan_date,'{0}') as WORK_DATE,sum(label_qty) as qty,GF_SECTIONMANPOWER('{1}',to_char(scan_date,'{2}')) as MANPOWER from  (           
       SELECT  scan_date,label_qty  from mes_label_d,base005m 
  where   
  inout_pz='OUT' 
  and  scan_date between  to_date('{3}','{4}') and to_date('{5}','{6}')
  and department_code=scan_detpt and udf07='{7}')
  group by  to_char(scan_date,'{8}') order by to_char(scan_date,'{9}')", dateFormat, vSection, dateFormat, FirstDay, dateFormat, SeventhDay, dateFormat, vSection, dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }

        public DataTable Tier2_DayPPH(DataBase DB, string date, string date2,string section, string dateFormat)
        {
            //string tomorrow = DateTime.Parse(date).AddDays(1).ToString(dateFormat);
            string sql = string.Format(@"select 
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
                        left join (select * from mes_dept_manpower where  begin_day<=to_date('{6}','{7}')  and end_day>to_date('{8}','{9}') or end_day is null) c  on  a.scan_detpt=c.d_dept
                        left join (select * from mes_workinghours_01 WHERE WORK_DAY=to_date('{10}','{11}')) d on  a.scan_detpt=d.d_dept
                        where e.udf07='{12}' and e.udf06='Y' order by e.department_code
                        ", date, dateFormat, date2, dateFormat, date, dateFormat, date, dateFormat, date, dateFormat, date, dateFormat, section);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 某一个厂区一天PPH
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="vPlant"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public DataTable Tier3_DayPPH(DataBase DB, string date,string date2, string vPlant, string dateFormat)
        {
            string sql = string.Format(@"SELECT to_char(scan_date,'{0}') AS WORK_DATE,sum(label_qty) as qty,GF_PLANTMANPOWER('{1}',to_char(scan_date,'{2}')) as MANPOWER from  (           
       SELECT  scan_date,label_qty  from mes_label_d,base005m 
  where   
  inout_pz='OUT' 
  and  scan_date between  to_date('{3}','{4}') and to_date('{5}','{6}')
  and department_code=scan_detpt AND UDF05 = '{7}' )
  group by  to_char(scan_date,'{8}')", dateFormat, vPlant, dateFormat,date, dateFormat,date2, dateFormat,vPlant, dateFormat);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 某一个厂一周的PPH
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dateFormat"></param>
        /// <param name="vPlant"></param>
        /// <param name="SeventhDay"></param>
        /// <param name="FirstDay"></param>
        /// <returns></returns>
        public DataTable Tier3_WeekPPH(DataBase DB,string dateFormat, string vPlant, string SeventhDay, string FirstDay)
        {
            string sql = string.Format(@"SELECT to_char(scan_date,'{0}') as WORK_DATE,sum(label_qty) as qty,GF_PLANTMANPOWER('{1}',to_char(scan_date,'{2}')) as MANPOWER from  (           
       SELECT  scan_date,label_qty  from mes_label_d,base005m 
  where   
  inout_pz='OUT' 
  and  scan_date between  to_date('{3}','{4}') and to_date('{5}','{6}')
  and department_code=scan_detpt and udf05='{7}')
  group by  to_char(scan_date,'{8}') order by to_char(scan_date,'{9}')", dateFormat, vPlant, dateFormat, FirstDay, dateFormat,SeventhDay, dateFormat,vPlant,dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 公司一天的PPH
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public DataTable Tier4_DayPPH(DataBase DB, string date,string date2, string dateFormat)
        {
            string sql = string.Format(@"SELECT to_char(scan_date,'{0}') AS WORK_DATE ,sum(label_qty) AS QTY,GF_COMPANYMANPOWER(to_char(scan_date,'{1}'))AS MANPOWER from  (           
       SELECT  scan_date,label_qty  from mes_label_d
  where   
  inout_pz='OUT' 
  and  scan_date between  to_date('{2}','{3}') and to_date('{4}','{5}')
  )
  group by  to_char(scan_date,'{6}') order by to_char(scan_date,'{7}')", dateFormat, dateFormat,date, dateFormat,date2,dateFormat,dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 公司一周的PPH
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public DataTable Tier4_WeekPPH(DataBase DB, string dateFormat, string FirstDay, string SeventhDay)
        {
            string sql = string.Format(@"SELECT to_char(scan_date,'{0}') AS WORK_DATE ,sum(label_qty) AS QTY,
GF_COMPANYMANPOWER(to_char(scan_date,'{1}'))AS MANPOWER from  (           
       SELECT  scan_date,label_qty  from mes_label_d
  where
 scan_detpt in (select department_code from base005m,sjqdms_orginfo where  udf05=code  and mfg='Y' AND UDF06='Y') and
  inout_pz='OUT' 
  and  scan_date between  to_date('{2}','{3}') and to_date('{4}','{5}')
  )
  group by  to_char(scan_date,'{6}') order by to_char(scan_date,'{7}')", dateFormat, dateFormat,FirstDay, 
  dateFormat, SeventhDay, dateFormat, dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }


    }
}
