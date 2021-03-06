﻿using KZ_QCMAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_QCMAPI.BLL
{
    class TierMeetingBLL
    {

        public DataTable Tier1_WeekQuery(DataBase DB, string vDept, string dateFormat, string FirstDay, string SeventhDay)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekQuery(DB, dateFormat, vDept, FirstDay, SeventhDay);
        }

        public DataTable Tier2_WeekQuery(DataBase DB, string vSection, string dateFormat, string FirstDay, string SeventhDay)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier2_WeekQuery(DB, dateFormat, vSection, FirstDay, SeventhDay);
        }

        /// <summary>
        /// 查询当前厂区当天RFT的数据
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="vPlant"></param>
        /// <returns></returns>
        public DataTable Tier3_DayQuery(DataBase DB, string date, string vPlant,string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier3_DayQuery(DB,date,vPlant, dateFormat);
        }

        internal DataTable Tier1_DayQuery(DataBase DB, string date, string line, string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_DayQuery(DB, date, line, dateFormat);
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
        public DataTable Tier3_WeekQuery(DataBase DB,  string vPlant, string dateFormat, string FirstDay,string SeventhDay)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier3_WeekQuery(DB, dateFormat, vPlant, FirstDay, SeventhDay);
        }

        /// <summary>
        /// 查询公司当天的RFT数据
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="factory"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public DataTable Tier4_DayQuery(DataBase DB, string date,string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier4_DayQuery(DB, date,dateFormat);
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
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier4_WeekQuery(DB, dateFormat, FirstDay, SeventhDay);
        }

    }
}
