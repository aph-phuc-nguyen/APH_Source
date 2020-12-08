using KZ_MESAPI.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.Controllers
{
    public class ProductionEfficiencyServer
    {
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject UpLoad(object OBJ)
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
                string data = jarr["data"].ToString();
                DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionEfficiencyBLL bLL = new ProductionEfficiencyBLL();
                int errorCount = bLL.UpLoad(DB,dt,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), 
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (errorCount > 0)
                {
                    ret.ErrMsg = "添加失败！导入资料中存在" + errorCount + "错误资料!";
                    ret.IsSuccess = true;
                }
                else
                {
                    ret.ErrMsg = "添加成功！";
                    ret.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        //查询
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryNumber(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vDept = jarr["vDept"].ToString();
                string vWrokDay = jarr["vWrokDay"].ToString();

                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionEfficiencyBLL bLL = new ProductionEfficiencyBLL();
                System.Data.DataTable dt = bLL.QueryNumber(DB, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), vDept, vWrokDay, DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryWorkDay(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                int numberday = int.Parse(jarr["numberday"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionEfficiencyBLL bLL = new ProductionEfficiencyBLL();
                System.Data.DataTable dt = bLL.QueryWorkDay(DB,date,numberday, DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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
    }
}
