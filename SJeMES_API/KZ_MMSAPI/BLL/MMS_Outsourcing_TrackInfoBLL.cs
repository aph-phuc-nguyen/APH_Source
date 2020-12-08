using KZ_MMSAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MMSAPI.BLL
{
    class MMS_Outsourcing_TrackInfoBLL
    {
        internal DataTable Query(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vScanType,string vBeginDate,string vEndDate)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.Query(DB, userCode, companyCode, vSeId,  vCompanyCode,  vPartName,  vScanType, vBeginDate, vEndDate);
        }

        internal DataTable GetSupplemtData(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vScanType, string vBeginDate, string vEndDate,string dateFormat)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetSupplemtData(DB, userCode, companyCode, vSeId, vCompanyCode, vPartName, vScanType, vBeginDate, vEndDate,dateFormat);
        }

        internal DataTable GetCode003AData(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vBeginDate, string vEndDate,string vSizeNo)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetCode003AData(DB, userCode, companyCode, vSeId, vCompanyCode, vPartName, vBeginDate, vEndDate,vSizeNo);
        }

        internal DataTable GetDetail(DataBase DB, string userCode, string companyCode, string vPacking_barcode)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetDetail(DB, userCode, companyCode, vPacking_barcode);
        }

        internal DataTable GetMatchingPart(DataBase DB, string userCode, string companyCode, string vSeId, string vCompany)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetMatchingPart(DB, userCode, companyCode, vSeId, vCompany);
        }

        internal DataTable GetOutsourcingDetail(DataBase DB, string userCode, string companyCode, string vSeId, string vCompany, string vSizeNo)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetOutsourcingDetail(DB,userCode,companyCode,vSeId, vCompany,vSizeNo);
        }

        internal DataTable GetReceivingDetail(DataBase DB, string userCode, string companyCode, string vSeId, string vCompany, string vSizeNo)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetReceivingDetail(DB, userCode, companyCode, vSeId, vCompany, vSizeNo);
        }

        internal DataTable GetUpateCode003AData(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vBeginDate, string vEndDate, string vSizeNo,string dateFormat)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetUpateCode003AData(DB, userCode, companyCode, vSeId, vCompanyCode, vPartName, vBeginDate, vEndDate, vSizeNo, dateFormat);
        }

        internal DataTable GetUpateCode003ADetailData(DataBase DB, string userCode, string companyCode, string id, string dateFormat)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetUpateCode003ADetailData(DB, userCode, companyCode, id,dateFormat);
        }

        internal void UpateCode003AData(DataBase DB, string userCode, string companyCode, string id, string oldQty, string curQty, string memo,string packing_barcode, string dateFormat, string dateTimeFormat,string dateTimeNowFormat)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            dal.InsertMMS_PROC_Change_List(DB, userCode, companyCode, id, oldQty, curQty, memo, packing_barcode,dateFormat);
            dal.UpateCode003AData(DB, userCode, companyCode, id, oldQty, curQty, memo, dateFormat, dateTimeFormat, dateTimeNowFormat);
        }

        internal void DeleteCode003AData(DataBase DB, string userCode, string companyCode, string id, string oldQty, string curQty, string memo, string packing_barcode, string dateFormat, string dateTimeFormat, string dateTimeNowFormat)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            dal.InsertMMS_PROC_Change_List(DB, userCode, companyCode, id, oldQty, curQty, memo, packing_barcode, dateFormat);
            dal.InserCode003ABak(DB,id);
            dal.DeleteCode003AData(DB,id);
        }

        internal DataTable GetCode003MData(DataBase DB, string userCode, string companyCode, string vSeId, string vfrom, string vSizeNo,string vPartName,string vto,string vOperation)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetCode003MData(DB,userCode,companyCode,vSeId, vfrom, vSizeNo, vPartName,vto,vOperation);
        }

        internal void updateSupplemtStatus(DataBase DB, string userCode, string companyCode, DataTable dt,string dateFormat)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            dal.updateSupplemtStatus(DB,userCode,companyCode,dt,dateFormat);
        }

        internal DataTable GetCode003ADetailData(DataBase DB, string userCode, string companyCode, string packing_barcode)
        {

            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetCode003ADetailData(DB, userCode, companyCode, packing_barcode);
        }

        internal DataTable GetScanDetail(DataBase DB, string vSeId, string vCompany)
        {
            MMS_Outsourcing_TrackInfoDal dal = new MMS_Outsourcing_TrackInfoDal();
            return dal.GetScanDetail(DB,vSeId, vCompany);
        }
    }
}
