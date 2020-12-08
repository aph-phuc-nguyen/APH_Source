using KZ_MMSAPI.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MMSAPI.Controllers
{
    class MMS_TrackIn_ListServer
    {
        
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryWareHouse(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            string Data = string.Empty;
            try
            {
                Data = ReqObj.Data.ToString();        
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MMS_TrackIn_ListBLL bLL = new MMS_TrackIn_ListBLL();
                DataTable dateTable=bLL.QueryWareHouse(DB,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dateTable);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Save(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            string Data = string.Empty;
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string se_id = jarr["se_id"].ToString();
                string se_seq = jarr["se_seq"].ToString();
                string size_no = jarr["size_no"].ToString();
                string qty = jarr["qty"].ToString();
                string size_seq = jarr["size_seq"].ToString();
                string art_no = jarr["art_no"].ToString();
                string mate_tieno = jarr["mate_tieno"].ToString();
                string tieno = jarr["tieno"].ToString();
                string stoc_no = jarr["stoc_no"].ToString();
                string rout_no = jarr["rout_no"].ToString();
                string stoc_wh = jarr["stoc_wh"].ToString();
                string stoc_whname = jarr["stoc_whname"].ToString();
                string to_company = jarr["to_company"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MMS_TrackIn_ListBLL bLL = new MMS_TrackIn_ListBLL();
                bLL.Save(DB,se_id,se_seq,size_no,qty,size_seq,art_no,mate_tieno,tieno, stoc_no, rout_no, stoc_wh,stoc_whname, to_company,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryScanInfo(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            string Data = string.Empty;
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string seId = jarr["seId"].ToString();
                string stocWhName = jarr["stocWhName"].ToString();
                string beginDate = jarr["beginDate"].ToString();
                string endDate = jarr["endDate"].ToString();
                string toCompany = jarr["toCompany"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MMS_TrackIn_ListBLL bLL = new MMS_TrackIn_ListBLL();
                DataTable dateTable = bLL.QueryScanInfo(DB,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                   stocWhName, beginDate, endDate, toCompany, seId);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dateTable);

            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryDetailScanInfo(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            string Data = string.Empty;
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string seId = jarr["seId"].ToString();
                string stocWhName = jarr["stocWhName"].ToString();
                string beginDate = jarr["beginDate"].ToString();
                string endDate = jarr["endDate"].ToString();
                string toCompany = jarr["toCompany"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MMS_TrackIn_ListBLL bLL = new MMS_TrackIn_ListBLL();
                DataTable dateTable = bLL.QueryDetailScanInfo(DB,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                   stocWhName, beginDate, endDate, toCompany, seId);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dateTable);

            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QuerySeInfo(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            string Data = string.Empty;
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string seId = jarr["seId"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MMS_TrackIn_ListBLL bLL = new MMS_TrackIn_ListBLL();
                string prodNo = bLL.QuerySeInfo(DB,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                   seId);
                ret.IsSuccess = true;
                ret.RetData = prodNo;

            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QuerySizeInfo(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            string Data = string.Empty;
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string seId = jarr["seId"].ToString();
                string sizeNo = jarr["sizeNo"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MMS_TrackIn_ListBLL bLL = new MMS_TrackIn_ListBLL();
                int sizeSeq = bLL.QuerySizeInfo(DB,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                   seId, sizeNo);
                ret.IsSuccess = true;
                ret.RetData = sizeSeq+"";

            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        

    }
}
