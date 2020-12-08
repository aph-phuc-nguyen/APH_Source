using TierMeeting.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TierMeeting.BLL
{
    class FollowTierMeetingBLL
    {
        public DataTable TMS_TIER_Query(DataBase DB, string userId, string CompanyCode,string dept,string date, string dateFormat,string type)
        {
            FollowTierMeetingDAL Dal = new FollowTierMeetingDAL();
            return Dal.TMS_TIER_Query(DB, userId, CompanyCode, dept, date, dateFormat,type);            
        }
        public DataTable TMS_TIER1_STANDARD_Query(DataBase DB, string userId, string CompanyCode, string dept, string date, string dateFormat, string type)
        {
            FollowTierMeetingDAL Dal = new FollowTierMeetingDAL();
            return Dal.TMS_TIER1_STANDARD_Query(DB, userId, CompanyCode, dept, date, dateFormat, type);
        }
        public DataTable GetMaturityAssessmentList(DataBase DB, string deptCode,string type, string date, string dateFormat)
        {
            FollowTierMeetingDAL Dal = new FollowTierMeetingDAL();
            return Dal.GetMaturityAssessmentList(DB, deptCode,type, date, dateFormat);
        }
        public DataTable GetMaturityList(DataBase DB)
        {
            FollowTierMeetingDAL DAL = new FollowTierMeetingDAL();
            return DAL.GetMaturityList(DB);
        }

    }
}
