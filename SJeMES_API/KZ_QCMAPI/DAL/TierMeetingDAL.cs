using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_QCMAPI.DAL
{
    class TierMeetingDAL
    {

        public DataTable Tier1_WeekQuery(DataBase DB, string dateFormat, string vDept, string FirstDay, string SeventhDay)
        {
            string sql = string.Format(@"select NVL(ROUND((SUM(sjqdms_mp.mp010) - SUM(sjqdms_mp.mp011)) / SUM(sjqdms_mp.mp010) * 100, 2), 0) as rft,
                to_char(insert_date, '{0}') as riqi from sjqdms_mp where insert_date between to_date('{1}', '{2}') and  to_date('{3}','{4}')" +
                " and MP003 = '{5}' group by to_char(insert_date, '{6}') order by to_char(insert_date, '{7}')", dateFormat, FirstDay, dateFormat, SeventhDay, dateFormat, vDept, dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 查询当前部门当周RFT的数据
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dateFormat"></param>
        /// <param name="vSection"></param>
        /// <param name="FirstDay"></param>
        /// <param name="SeventhDay"></param>
        /// <returns></returns>
        public DataTable Tier2_WeekQuery(DataBase DB, string dateFormat, string vSection, string FirstDay, string SeventhDay)
        {
            string sql = string.Format(@"select NVL(ROUND((SUM(sjqdms_mp.mp010) - SUM(sjqdms_mp.mp011)) / SUM(sjqdms_mp.mp010) * 100, 2), 0) as rft,
                to_char(insert_date, '{0}') as riqi from sjqdms_mp,base005m  where insert_date between to_date('{1}', '{2}') and  to_date('{3}','{4}')" +
                " and mp003=DEPARTMENT_CODE and udf07 = '{5}' group by to_char(insert_date, '{6}') order by to_char(insert_date, '{7}')", dateFormat, FirstDay, dateFormat, SeventhDay, dateFormat, vSection, dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }

        internal DataTable Tier1_DayQuery(DataBase DB, string date, string line, string dateFormat)
        {
            string sql = "";
            if (date.Equals(DateTime.Now.ToString(dateFormat)))
            {
                string now = DateTime.Now.Hour.ToString();
                if (now == "8")
                {
                    now = "0" + now;
                }
                if (now == "9")
                {
                    now = "0" + now;
                }
                sql = string.Format(@"select ba002,q.time as ba0022,ROUND((ba004 - ba005) * 100 / ba004, 0) as num from QCM_QTA_setting q left join 
                (SELECT mp002 ba002, sum(mp010) ba004, sum(mp011) ba005
                          FROM SJQDMS_Mp
                         WHERE mp007 = 'TQC'
                           and MP003 = '{0}'
                           and mp002 LIKE TO_CHAR(TO_DATE('{1}','{2}'),'YYYYMMDD') || '%'
                         GROUP BY mp002
                         ORDER BY mp002) a
                on q.time = substr(ba002, -2, 2) 
                where q.time<='{2}'
                order by q.time ",line,date, dateFormat,now);
                DataTable dt = DB.GetDataTable(sql);
            }
            else
            {
                sql = string.Format(@"select ba002,q.time as ba0022,ROUND((ba004 - ba005) * 100 / ba004, 0) as num from QCM_QTA_setting q left join 
                (SELECT mp002 ba002, sum(mp010) ba004, sum(mp011) ba005
                          FROM SJQDMS_Mp
                         WHERE mp007 = 'TQC'
                           and MP003 = '{0}'
                           and mp002 LIKE TO_CHAR(TO_DATE('{1}','{2}'),'YYYYMMDD') || '%'
                        
                         GROUP BY mp002
                         ORDER BY mp002) a
                on q.time = substr(ba002, -2, 2) 
                order by q.time ", line, date, dateFormat);
                DataTable dt = DB.GetDataTable(sql);
            }
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 查询当前厂区当天RFT的数据
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="vPlant"></param>
        /// <returns></returns>
        public DataTable Tier3_DayQuery(DataBase DB, string vPlant, string date, string dateFormat)
        {
            string sql = string.Format(@"select NVL(ROUND((SUM(sjqdms_mp.mp010)-SUM(sjqdms_mp.mp011))/SUM(sjqdms_mp.mp010)*100,2),0) as rft
                     from sjqdms_mp where  mp007='TQC' and  to_char(insert_date,'{0}')='{1}' and  MP001 = '{2}'", dateFormat, date, vPlant);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 查询当前厂区当周RFT的数据
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dateFormat"></param>
        /// <param name="vPlant"></param>
        /// <param name="FirstDay"></param>
        /// <param name="SeventhDay"></param>
        /// <returns></returns>
        public DataTable Tier3_WeekQuery(DataBase DB, string dateFormat, string vPlant, string FirstDay, string SeventhDay)
        {
            string sql = string.Format(@"select NVL(ROUND((SUM(sjqdms_mp.mp010) - SUM(sjqdms_mp.mp011)) / SUM(sjqdms_mp.mp010) * 100, 2), 0) as rft,
                to_char(insert_date, '{0}') as riqi from sjqdms_mp where insert_date between to_date('{1}', '{2}') and  to_date('{3}','{4}')" +
                " and MP001 = '{5}' group by to_char(insert_date, '{6}') order by to_char(insert_date, '{7}')", dateFormat,  FirstDay, dateFormat, SeventhDay, dateFormat, vPlant, dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 查询公司当天的RFT数据
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="factory"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public DataTable Tier4_DayQuery(DataBase DB, string date, string dateFormat)
        {
            string sql = string.Format(@"select NVL(ROUND((SUM(sjqdms_mp.mp010)-SUM(sjqdms_mp.mp011))/SUM(sjqdms_mp.mp010)*100,2),0) as rft
                     from sjqdms_mp where mp007 = 'TQC' and to_char(insert_date,'{0}')= '{1}'", dateFormat, date);
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 查询公司当周的RFT数据
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="factory"></param>
        /// <param name="SeventhDay"></param>
        /// <param name="FirstDay"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public DataTable Tier4_WeekQuery(DataBase DB, string SeventhDay, string FirstDay, string dateFormat)
        {
            string sql = string.Format(@"select NVL(ROUND((SUM(sjqdms_mp.mp010) - SUM(sjqdms_mp.mp011)) / SUM(sjqdms_mp.mp010) * 100, 2), 0) as rft,
                to_char(insert_date, '{0}') as riqi from sjqdms_mp where insert_date between to_date('{1}', '{2}') and to_date('{3}','{4}') and mp001 in(select code from sjqdms_orginfo where MFG='Y') " +
                 "group by to_char(insert_date, '{5}')  order by to_char(insert_date, '{6}')", dateFormat, FirstDay,dateFormat,SeventhDay, dateFormat, dateFormat, dateFormat);
            return DB.GetDataTable(sql);
        }
    }
}
