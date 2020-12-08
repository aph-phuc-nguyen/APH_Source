using KZ_JMSAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_JMSAPI.BLL
{
    class JMS_GoalAccomplished_ListBLL
    {
        public void insert(DataBase DB, DataTable dt, string userId, string dateFormat)        
        {
            JMS_GoalAccomplished_ListDAL Dal = new JMS_GoalAccomplished_ListDAL();
            Dal.insert(DB,dt, userId, dateFormat);          
        }

        public DataTable Tier4_PORateQuery(DataBase DB, string FirstDay, string SeventhDay, string dateFormat)
        {
            JMS_GoalAccomplished_ListDAL Dal = new JMS_GoalAccomplished_ListDAL();
            return Dal.Tier4_PORateQuery(DB,FirstDay,SeventhDay,dateFormat);
        }

        public DataTable Tier4_DayQuery(DataBase DB, string date, string dateFormat)
        {
            JMS_GoalAccomplished_ListDAL Dal = new JMS_GoalAccomplished_ListDAL();
            return Dal.Tier4_DayQuery(DB, date, dateFormat);
        }
    }
}
