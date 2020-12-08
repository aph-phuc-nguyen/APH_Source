using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SJeMES_Framework_NETCore.DBHelper;
using KZ_MESAPI.DAL;

namespace KZ_MESAPI.BLL
{
    public class AutoGenerationSchedulingBLL
    {
        public DataTable Query(DataBase DB)
        {

            AutoGenerationSchedulingDAL Dal = new AutoGenerationSchedulingDAL();
            return Dal.Query(DB);
        }

        /// <summary>
        /// 用于把当天未做完的计划转移到第二天
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dt"></param>
        /// <param name="userId"></param>
        /// <param name="CompanyCode"></param>
        /// <returns></returns>
        public int Insert(DataBase DB, DataTable dt, string userId, string CompanyCode, string dateFormat, string dateTimeFormat)
        {
            AutoGenerationSchedulingDAL Dal = new AutoGenerationSchedulingDAL();
            return Dal.Insert(DB, dt, userId, CompanyCode, dateFormat, dateTimeFormat);
        }


        public DataTable Query_thread(DataBase DB, string userId, string CompanyCode, string dateFormat)
        {
            AutoGenerationSchedulingDAL Dal = new AutoGenerationSchedulingDAL();
            return Dal.Query_thread(DB, userId, CompanyCode, dateFormat);
        }

        public void Delete(DataBase DB, string userId, string CompanyCode, string dateFormat)
        {
            AutoGenerationSchedulingDAL Dal = new AutoGenerationSchedulingDAL();
            Dal.Delete(DB, userId, CompanyCode, dateFormat);
        }

        public DataTable Query_thread1(DataBase DB)
        {
            AutoGenerationSchedulingDAL Dal = new AutoGenerationSchedulingDAL();
            return Dal.Query_thread1(DB);
        }

        public int Insert_thread1(DataBase DB, DataTable dt, string userId, string CompanyCode, string dateFormat, string dateTimeFormat)
        {
            AutoGenerationSchedulingDAL Dal = new AutoGenerationSchedulingDAL();
            return Dal.Insert_thread1(DB, dt, userId, CompanyCode, dateFormat, dateTimeFormat);
        }

    }

}



