using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    class F_MMS_TrackIn_ListBLL
    {
        internal void Save(DataBase DB, string se_id, string se_seq, string size_no, string qty, string size_seq, string art_no, string mate_tieno, string tieno, string stoc_no, string rout_no, string stoc_wh, string stoc_whname,string to_company, string userCode , string companyCode)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                F_MMS_TrackIn_ListDAL dal = new F_MMS_TrackIn_ListDAL();
                dal.save(DB, se_id, se_seq, size_no, qty, size_seq, art_no, mate_tieno, tieno, stoc_no, rout_no, stoc_wh, stoc_whname,to_company, userCode, companyCode);
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }

           
        }

        internal DataTable QueryWareHouse(DataBase DB, string userCode, string companyCode)
        {
            F_MMS_TrackIn_ListDAL dal = new F_MMS_TrackIn_ListDAL();
            return dal.QueryWareHouse(DB, userCode, companyCode);
        }

        internal DataTable QueryScanInfo(DataBase DB, string userCode, string companyCode, string stocWhName, string beginDate, string endDate,string toCompnay,string seId)
        {
            F_MMS_TrackIn_ListDAL dal = new F_MMS_TrackIn_ListDAL();
            return dal.QueryScanInfo(DB, userCode, companyCode, stocWhName, beginDate, endDate, toCompnay, seId);
        }

        internal DataTable QueryDetailScanInfo(DataBase DB, string userCode, string companyCode, string stocWhName, string beginDate, string endDate, string toCompnay, string seId)
        {
            F_MMS_TrackIn_ListDAL dal = new F_MMS_TrackIn_ListDAL();
            return dal.QueryDetailScanInfo(DB, userCode, companyCode, stocWhName, beginDate, endDate, toCompnay, seId);
        }

        
        internal int QuerySizeInfo(DataBase DB, string userCode, string companyCode, string seId, string sizeNo)
        {
            F_MMS_TrackIn_ListDAL dal = new F_MMS_TrackIn_ListDAL();
            return dal.QuerySizeInfo(DB, userCode, companyCode, seId, sizeNo);
        }


        internal string QuerySeInfo(DataBase DB, string userCode, string companyCode, string seId)
        {
            F_MMS_TrackIn_ListDAL dal = new F_MMS_TrackIn_ListDAL();
            return dal.QuerySeInfo(DB, userCode, companyCode, seId);
        }
    }
}
