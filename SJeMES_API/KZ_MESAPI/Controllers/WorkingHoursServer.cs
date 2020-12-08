using KZ_MESAPI.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.Controllers
{
    public class WorkingHoursServer
    {

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject LoadSeDept(object OBJ)
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
                string vDDept = jarr["vDDept"].ToString(); DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                WorkingHoursBLL bLL = new WorkingHoursBLL();
                System.Data.DataTable dt = bLL.LoadSeDept(DB, vDDept,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Query(object OBJ)
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
                string orgNo = jarr["vOrgNo"].ToString();
                string plantNo = jarr["vPlantNo"].ToString();
                string deptNo = jarr["vDeptNo"].ToString();
                string routNo = jarr["vRoutNo"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                WorkingHoursBLL bLL = new WorkingHoursBLL();
                System.Data.DataTable dt = bLL.Query(DB, orgNo, plantNo, deptNo, routNo,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetWorkingHoursData(object OBJ)
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
                string orgNo = jarr["vOrgNo"].ToString();
                string plantNo = jarr["vPlantNo"].ToString();
                string deptNo = jarr["vDeptNo"].ToString();
                string routNo = jarr["vRoutNo"].ToString();
                string workDate = jarr["vDate"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                WorkingHoursBLL bLL = new WorkingHoursBLL();
                System.Data.DataTable dt = bLL.GetWorkingHoursData(DB, orgNo, plantNo, deptNo, routNo, workDate,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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


        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Save(object OBJ)
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
                string vfrom = jarr["from"].ToString();
                string vto = jarr["to"].ToString();

                string mon_am_from = jarr["mon_am_from"].ToString();
                string mon_am_to = jarr["mon_am_to"].ToString();
                string mon_pm_from = jarr["mon_pm_from"].ToString();
                string mon_pm_to = jarr["mon_pm_to"].ToString();

                string tue_am_from = jarr["tue_am_from"].ToString();
                string tue_am_to = jarr["tue_am_to"].ToString();
                string tue_pm_from = jarr["tue_pm_from"].ToString();
                string tue_pm_to = jarr["tue_pm_to"].ToString();

                string wed_am_from = jarr["wed_am_from"].ToString();
                string wed_am_to = jarr["wed_am_to"].ToString();
                string wed_pm_from = jarr["wed_pm_from"].ToString();
                string wed_pm_to = jarr["wed_pm_to"].ToString();

                string thu_am_from = jarr["thu_am_from"].ToString();
                string thu_am_to = jarr["thu_am_to"].ToString();
                string thu_pm_from = jarr["thu_pm_from"].ToString();
                string thu_pm_to = jarr["thu_pm_to"].ToString();

                string fri_am_from = jarr["fri_am_from"].ToString();
                string fri_am_to = jarr["fri_am_to"].ToString();
                string fri_pm_from = jarr["fri_pm_from"].ToString();
                string fri_pm_to = jarr["fri_pm_to"].ToString();

                string sat_am_from = jarr["sat_am_from"].ToString();
                string sat_am_to = jarr["sat_am_to"].ToString();
                string sat_pm_from = jarr["sat_pm_from"].ToString();
                string sat_pm_to = jarr["sat_pm_to"].ToString();

                string sun_am_from = jarr["sun_am_from"].ToString();
                string sun_am_to = jarr["sun_am_to"].ToString();
                string sun_pm_from = jarr["sun_pm_from"].ToString();
                string sun_pm_to = jarr["sun_pm_to"].ToString();

                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DataTable dataTable = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(data);
                WorkingHoursBLL bLL = new WorkingHoursBLL();
                bLL.Save(DB, dataTable, vfrom, vto,
                    mon_am_from, mon_am_to, mon_pm_from, mon_pm_to,
                    tue_am_from, tue_am_to, tue_pm_from, tue_pm_to,
                    wed_am_from, wed_am_to, wed_pm_from, wed_pm_to,
                    thu_am_from, thu_am_to, thu_pm_from, thu_pm_to,
                    fri_am_from, fri_am_to, fri_pm_from, fri_pm_to,
                    sat_am_from, sat_am_to, sat_pm_from, sat_pm_to,
                    sun_am_from, sun_am_to, sun_pm_from, sun_pm_to,
                   SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;

            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject LoadMoveNo(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                WorkingHoursBLL bLL = new WorkingHoursBLL();
                ret.ErrMsg = bLL.LoadMoveNo(DB);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveTransTime(object OBJ)
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
                string vMoveNo = jarr["vMoveNo"].ToString();
                string vMoveDate = jarr["vMoveDate"].ToString();
                string vDDept = jarr["vDDept"].ToString();
                string vWrokDay = jarr["vWrokDay"].ToString();
                string vTransInTime = jarr["vTransInTime"].ToString();
                string vTransOutTime = jarr["vTransOutTime"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                WorkingHoursBLL bLL = new WorkingHoursBLL();
                if (bLL.SaveTransTime(DB, vMoveNo, vMoveDate, vDDept, vWrokDay, vTransOutTime, vTransInTime,
                SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                 DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))))
                {
                    ret.IsSuccess = true;
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
