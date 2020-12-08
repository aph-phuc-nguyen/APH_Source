using KZ_MESAPI.BLL;
using SJ_BASEHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.Controllers
{
    public class AutoGenerationSchedulingServer
    {
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Query(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                AutoGenerationSchedulingBLL bLL = new AutoGenerationSchedulingBLL();
                System.Data.DataTable dt = bLL.Query(DB);
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
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

        /// <summary>
        /// 用于把当天未做完的计划转移到第二天
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Insert(object OBJ)
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
                AutoGenerationSchedulingBLL bLL = new AutoGenerationSchedulingBLL();
                int count = bLL.Insert(DB, dt, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                     DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                       DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (count > 0)
                {
                    ret.ErrMsg = count + "";
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Query_thread(object OBJ)
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
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                AutoGenerationSchedulingBLL bLL = new AutoGenerationSchedulingBLL();
                System.Data.DataTable dt = bLL.Query(DB);
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Delete(object OBJ)
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
                AutoGenerationSchedulingBLL bLL = new AutoGenerationSchedulingBLL();
                bLL.Delete(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Query_thread1(object OBJ)
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
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                AutoGenerationSchedulingBLL bLL = new AutoGenerationSchedulingBLL();
                System.Data.DataTable dt = bLL.Query_thread1(DB);
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Insert_thread1(object OBJ)
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
                AutoGenerationSchedulingBLL bLL = new AutoGenerationSchedulingBLL();
                int count = bLL.Insert_thread1(DB, dt, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                       DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (count > 0)
                {
                    ret.ErrMsg = count + "";
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


        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SendWX(object OBJ)
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
                string msg = jarr["data"].ToString();
                List<Dictionary<string, string>> listData = new List<Dictionary<string, string>>();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string sql = @"select staff_no,staff_name,udf05 from hr001m where UDF01='BIT01' and udf05 is not null";
                DataTable dataTable = DB.GetDataTable(sql);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Dictionary<string, string> weixinD = new Dictionary<string, string>();
                    weixinD["USERCODE"] = dataTable.Rows[i]["staff_name"].ToString();
                    weixinD["WEIXINCODE"] = dataTable.Rows[i]["udf05"].ToString();
                    weixinD["SENDTEXT"] = msg;
                    listData.Add(weixinD);
                }
                SJeMES_Framework_NETCore.WebAPI.ResultObject weixinResult = SendMessageHelper.SendWEIXIN(listData);
                if (!weixinResult.IsSuccess)
                    throw new Exception(weixinResult.ErrMsg);

                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject TempDataSynchronization(object OBJ)
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
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                string sql1 = @"insert into mes_label_d
                               SELECT * FROM mes_label_d@apherp where to_char(insert_date,'" + dateFormat + "')=TO_CHAR(SYSDATE,'" + dateFormat + "')";
                string sql2 = @"insert into sjqdms_worktarget
                                select * from sjqdms_worktarget@apherp where to_char(work_day,'" + dateFormat + "')=TO_CHAR(SYSDATE,'" + dateFormat + "')";
                string sql3 = @"update mes_label_d set process_no='K' where  process_no='C' and to_char(insert_date,'" + dateFormat + "')=TO_CHAR(SYSDATE,'" + dateFormat + "')";
                DB.ExecuteNonQueryOffline(sql1);
                DB.ExecuteNonQueryOffline(sql2);
                DB.ExecuteNonQueryOffline(sql3);
                ret.IsSuccess = true;
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
