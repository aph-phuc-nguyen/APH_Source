using KZ_MESAPI.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace KZ_MESAPI.Controllers
{
    public class ProductionReportServer
    {
        //计划产量差异查询
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetWorkDaySizeReport(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vSeId = jarr["vSeId"].ToString();
                string vPO = jarr["vPO"].ToString();
                string vDept = jarr["vDept"].ToString();
                string vArt = jarr["vArt"].ToString();
                string vInOut = jarr["vInOut"].ToString();

                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionReportBLL bLL = new ProductionReportBLL();
                System.Data.DataTable dt = bLL.GetWorkDaySizeReport(DB, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), vSeId, vPO, vDept, vArt, vInOut);
                ret.IsSuccess = true;
                //ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        //按日生产指示查询
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetWorkDayReport(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vSeId = jarr["vSeId"].ToString();
                string vWrokDay = jarr["vWrokDay"].ToString();
                string vDept = jarr["vDept"].ToString();
                string vPO = jarr["vPO"].ToString();
                string vArt = jarr["vArt"].ToString();
                string vStatus = jarr["vStatus"].ToString();
                string vInOut = jarr["vInOut"].ToString();

                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionReportBLL bLL = new ProductionReportBLL();
                System.Data.DataTable dt = bLL.GetWorkDayReport(DB, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), vSeId, vWrokDay, vDept, vPO, vArt, vStatus, vInOut);
                ret.IsSuccess = true;
                //ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        //按插入时间查询
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetMesLableDReport(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vSeId = jarr["vSeId"].ToString();
                string vPO = jarr["vPO"].ToString();
                string vWrokDay = jarr["vWrokDay"].ToString();
                string vDept = jarr["vDept"].ToString();
                string vArt = jarr["vArt"].ToString();

                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionReportBLL bLL = new ProductionReportBLL();
                System.Data.DataTable dt = bLL.GetMesLableDReport(DB, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)), vSeId, vPO, vWrokDay, vDept, vArt);
                ret.IsSuccess = true;
                //ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
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
