using KZ_MESAPI.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace KZ_MESAPI.Controllers
{
     public class Efficiency_KanbanServer
    {
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject ThirdEfficiency_Query(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();

                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string date = jarr["date"].ToString();
                string plant = jarr["plant"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                Efficiency_KanbanBLL bLL = new Efficiency_KanbanBLL();
                System.Data.DataTable dt = bLL.ThirdEfficiency_Query(DB,SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), 
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), date, plant,
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                    );
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "查无此数据！";
                }
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SetionEfficiency_Query(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();

                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string date = jarr["date"].ToString();
                string section = jarr["section"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                Efficiency_KanbanBLL bLL = new Efficiency_KanbanBLL();
                System.Data.DataTable dt = bLL.SetionEfficiency_Query(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken),
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), date, section,
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "查无此数据！";
                }
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }


        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SecondEfficiency_Query(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string date = jarr["date"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                Efficiency_KanbanBLL bLL = new Efficiency_KanbanBLL();
                System.Data.DataTable dt = bLL.SecondEfficiency_Query(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken),
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), date,
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                    );
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "查无此数据！";
                }
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }


         public static SJeMES_Framework_NETCore.WebAPI.ResultObject FirstEfficiency_Query1(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string date = jarr["date"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                Efficiency_KanbanBLL bLL = new Efficiency_KanbanBLL();
                System.Data.DataTable dt= bLL.FirstEfficiency_Query(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken),
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),date,
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
                    );
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryWorkDate(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                Efficiency_KanbanBLL bLL = new Efficiency_KanbanBLL();
                string date = bLL.QueryWorkDate(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken),
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                ret.RetData = date;
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryPlant(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                Efficiency_KanbanBLL bLL = new Efficiency_KanbanBLL();
                string date = bLL.QueryPlant(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken),
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                ret.RetData = date;
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryDeptNo(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                Efficiency_KanbanBLL bLL = new Efficiency_KanbanBLL();
                string deptNo = bLL.QueryDeptNo(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken),
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                ret.RetData = deptNo;
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
