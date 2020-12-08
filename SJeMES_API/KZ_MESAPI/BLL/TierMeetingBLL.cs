
using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    class TierMeetingBLL
    {

        public DataTable Tier1_WeekPPH(DataBase DB, string date, string date2, string line, string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekPPH(DB, date, date2, line, dateFormat);
        }

        public DataTable Tier2_WeekPPH(DataBase DB, string date, string date2, string section, string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier2_WeekPPH(DB, date, date2, section, dateFormat);
        }

        /// <summary>
        /// 公司一天的PPH
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public DataTable Tier2_DayPPH(DataBase DB, string date, string date2,string section,string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier2_DayPPH(DB, date, date2, section,dateFormat);
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
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier3_DayPPH(DB, date,date2, vPlant, dateFormat);
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
        public DataTable Tier3_WeekPPH(DataBase DB, string dateFormat, string vPlant, string SeventhDay, string FirstDay)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier3_WeekPPH(DB, dateFormat, vPlant, FirstDay, SeventhDay);
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
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier4_DayPPH(DB, date, date2,dateFormat);
        }

        /// <summary>
        /// 公司一周的PPH
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public DataTable Tier4_WeekPPH(DataBase DB, string dateFormat, string FirstDay, string SeventhDay)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier4_WeekPPH(DB, dateFormat, FirstDay, SeventhDay);
        }
    }
}
