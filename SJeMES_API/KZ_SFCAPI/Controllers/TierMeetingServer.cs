using KZ_SFCAPI;
using KZ_SFCAPI.BLL;
using System;
using System.Collections.Generic;
using System.Data;


namespace KZ_SFCAPI.Controllers
{
    public class TierMeetingServer
    {
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject TabPage2_Query(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string date = jarr["date"].ToString();
                string line = jarr["line"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt = bLL.TabPage2_Query(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), date, line,DateTimeFormat.getDateFormatValue(DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))));
                if (dt.Rows.Count >= 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else
                {
                    ret.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject TabPage3_Query(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string date = jarr["date"].ToString();
                string line = jarr["line"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt = bLL.TabPage3_Query(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), date, line, DateTimeFormat.getDateFormatValue(DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))));
                if (dt.Rows.Count >= 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else
                {
                    ret.IsSuccess = false;
                }
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
