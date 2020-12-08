using KZ_SFCAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_SFCAPI.BLL
{
    class TierMeetingBLL
    {
        internal DataTable TabPage2_Query(DataBase DB, string userCode, string date, string line,string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.TabPage2_Query(DB, userCode, date, line, dateFormat);
        }

        internal DataTable TabPage3_Query(DataBase DB, string userCode, string date, string line, string dateFormat)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.TabPage3_Query(DB, userCode, date, line, dateFormat);
        }
    }
}
