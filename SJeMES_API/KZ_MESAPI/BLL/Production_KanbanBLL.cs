using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    class Production_KanbanBLL
    {
        public DataTable TabPage8_Query(DataBase DB, string userCode, string companyCode, string date, string line, string dateFormat)
        {
            Production_KanbanDAL Dal = new Production_KanbanDAL();
            return Dal.TabPage8_Query(DB, userCode, companyCode, date, line, dateFormat);
        }


        public DataTable TabPage1_Query(DataBase DB, string userCode, string companyCode, string date, string line, string dateFormat)
        {
            Production_KanbanDAL Dal = new Production_KanbanDAL();
            return Dal.TabPage1_Query(DB, userCode, companyCode, date, line, dateFormat);
        }

        public DataTable TabPage2_Query(DataBase DB, string userCode, string companyCode, string date, string line, string dateFormat)
        {
            Production_KanbanDAL Dal = new Production_KanbanDAL();
            return Dal.TabPage2_Query(DB, userCode, companyCode, date, line);
        }

        public DataTable TabPage6_Query(DataBase DB, string userCode, string companyCode, string date, string line, DataTable clinetTimeParams, string dateFormat, string dateTimeFormat)
        {
            Production_KanbanDAL Dal = new Production_KanbanDAL();
            return Dal.TabPage6_Query(DB, userCode, companyCode, date, line, clinetTimeParams, dateFormat, dateTimeFormat);
        }



        public DataTable TabPage6_Query_ScanDetail(DataBase DB, string userCode, string companyCode, string date, string line, string dateFormat)
        {
            Production_KanbanDAL Dal = new Production_KanbanDAL();
            return Dal.TabPage6_Query_ScanDetail(DB, userCode, companyCode, date, line, dateFormat);
        }


        public DataTable TabPage6_Query_OtherDetail(DataBase DB, string userCode, string companyCode, string date, string line, string dateFormat)
        {
            Production_KanbanDAL Dal = new Production_KanbanDAL();
            return Dal.TabPage6_Query_OtherDetail(DB, userCode, companyCode, date, line, dateFormat);
        }

    }
}
