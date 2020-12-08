using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    public class Efficiency_KanbanBLL
    {
        public  DataTable ThirdEfficiency_Query(DataBase DB,string userCode, string compnayCode,string date,string plant,string dateFormat)
        {
            Efficiency_KanbanDAL Dal = new Efficiency_KanbanDAL();
            return Dal.ThirdEfficiency_Query(DB, userCode, compnayCode,date,plant,dateFormat);
        }

        public DataTable SecondEfficiency_Query(DataBase DB, string userCode, string compnayCode, string date, string dateFormat)
        {
            Efficiency_KanbanDAL Dal = new Efficiency_KanbanDAL();
            return Dal.SecondEfficiency_Query(DB, userCode, compnayCode, date,dateFormat);
        }

        public DataTable FirstEfficiency_Query(DataBase DB, string userCode, string compnayCode,string date, string dateFormat)
        {
            DataTable dataTable = new DataTable();
            Efficiency_KanbanDAL Dal = new Efficiency_KanbanDAL();
            //date = Dal.QueryWorkDate(DB, userCode, compnayCode);
            decimal workingHours = Dal.QueryWorkIngHours(DB, userCode, compnayCode,date,dateFormat);
            decimal total = Dal.TotalQty(DB, userCode, compnayCode, date,dateFormat);
            decimal pph = workingHours == 0 ? 0 : (total / workingHours);
            dataTable.Columns.Add("Total Produced");
            dataTable.Columns.Add("pph");
            DataRow dr = dataTable.NewRow();
            dr[0] = total;
            dr[1] = pph;
            dataTable.Rows.Add(dr);
            return dataTable;
        }

        public string QueryWorkDate(DataBase DB, string userCode, string compnayCode)
        {
            Efficiency_KanbanDAL Dal = new Efficiency_KanbanDAL();
            string date = Dal.QueryWorkDate(DB, userCode, compnayCode);
            return date;
        }

        public string QueryPlant(DataBase DB, string userCode, string compnayCode)
        {
            Efficiency_KanbanDAL Dal = new Efficiency_KanbanDAL();
            string plant = Dal.QueryPlant(DB, userCode, compnayCode);
            return plant;
        }

        public string QueryDeptNo(DataBase DB, string userCode, string compnayCode)
        {
            Efficiency_KanbanDAL Dal = new Efficiency_KanbanDAL();
            string plant = Dal.QueryDeptNo(DB, userCode, compnayCode);
            return plant;
        }

        public DataTable SetionEfficiency_Query(DataBase DB, string userCode, string compnayCode, string date, string section,string dateFormat)
        {
            Efficiency_KanbanDAL Dal = new Efficiency_KanbanDAL();
            return Dal.SetionEfficiency_Query(DB, userCode, compnayCode, date, section, dateFormat);
        }
    }
}
