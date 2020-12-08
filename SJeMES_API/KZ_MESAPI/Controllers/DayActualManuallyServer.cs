using System;
using System.Collections.Generic;
using System.Text;
using KZ_MESAPI.BLL;

namespace KZ_MESAPI.Controllers
{
    public class DayActualManuallyServer
    {
        /// <summary>
        /// 用于导入日产量目标的接口
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject UpLoad(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string data = jarr["data"].ToString();
                System.Data.DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(data);

                int count = dt.Rows.Count;

                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DayActualManuallyBLL bLL = new DayActualManuallyBLL();
                bLL.UpLoad(DB, dt, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                    , DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                    , DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject ValiDept(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vDDept = jarr["vDDept"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DayActualManuallyBLL bLL = new DayActualManuallyBLL();
                bool isOk = bLL.ValiDept(DB, vDDept, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(isOk);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetListArt_No(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vDDept = jarr["Art_No"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DayActualManuallyBLL bLL = new DayActualManuallyBLL();
                System.Data.DataTable dt = bLL.GetListArt_No(DB, vDDept);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryActual(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vDDept = jarr["vDDept"].ToString();
                string vWorkDay = jarr["vWorkDay"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DayActualManuallyBLL bLL = new DayActualManuallyBLL();
                System.Data.DataTable dt = bLL.QueryActual(DB, vDDept, vWorkDay, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                    , DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                     , DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                //ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject UpdateWorkActual(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vDDept = jarr["vDDept"].ToString();
                string vWorkDay = jarr["vWorkDay"].ToString();
                decimal vWorkQty = decimal.Parse(jarr["vWorkQty"].ToString());
                string vNote = jarr["vNote"].ToString();
                string vArtNo = jarr["vArtNo"].ToString();
                System.Data.DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DayActualManuallyBLL bLL = new DayActualManuallyBLL();
                bLL.UpdateWorkActual(DB, vDDept, vWorkDay, vWorkQty, vNote, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                     , DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), vArtNo);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }
    }
}


