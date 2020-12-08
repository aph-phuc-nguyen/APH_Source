using KZ_MMSAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MMSAPI.BLL
{
    class MMS_TrackOut_ListBLL
    {
        internal void Save(DataBase DB, string se_id, string se_seq, string size_no, string qty, string size_seq, string art_no, string mate_tieno, string tieno, string stoc_no, string rout_no, string stoc_wh, string stoc_whname,string to_company, string userCode , string companyCode)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                MMS_TrackOut_ListDAL dal = new MMS_TrackOut_ListDAL();
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
            MMS_TrackOut_ListDAL dal = new MMS_TrackOut_ListDAL();
            return dal.QueryWareHouse(DB, userCode, companyCode);
        }

        internal DataTable QueryScanInfo(DataBase DB, string userCode, string companyCode, string stocWhName, string beginDate, string endDate,string toCompnay,string seId)
        {
            MMS_TrackOut_ListDAL dal = new MMS_TrackOut_ListDAL();
            return dal.QueryScanInfo(DB, userCode, companyCode, stocWhName, beginDate, endDate, toCompnay, seId);
        }

        internal DataTable QueryDetailScanInfo(DataBase DB, string userCode, string companyCode, string stocWhName, string beginDate, string endDate, string toCompnay, string seId)
        {
            MMS_TrackOut_ListDAL dal = new MMS_TrackOut_ListDAL();
            return dal.QueryDetailScanInfo(DB, userCode, companyCode, stocWhName, beginDate, endDate, toCompnay, seId);
        }

        
        internal int QuerySizeInfo(DataBase DB, string userCode, string companyCode, string seId, string sizeNo)
        {
            MMS_TrackOut_ListDAL dal = new MMS_TrackOut_ListDAL();
            return dal.QuerySizeInfo(DB, userCode, companyCode, seId, sizeNo);
        }


        internal string QuerySeInfo(DataBase DB, string userCode, string companyCode, string seId)
        {
            MMS_TrackOut_ListDAL dal = new MMS_TrackOut_ListDAL();
            return dal.QuerySeInfo(DB, userCode, companyCode, seId);
        }
    }
}
