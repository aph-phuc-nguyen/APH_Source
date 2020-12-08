using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TierMeeting.BLL;

namespace TierMeeting.Controllers
{
    class TierMeetingServer
    {

        #region Tier4Form
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetDepartmentByUser(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetDepartmentByUser(DB, 
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken));
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetSafetyDataByDate(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetSafetyDataByDate(DB, date, dept, type);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetSafetyDataUntilDate(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DateTime firstDateOfYear = DateTime.Parse(jarr["firstDateOfYear"].ToString());
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetSafetyDataUntilDate(DB, date, firstDateOfYear, dept, type);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetSafetyDays(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DateTime firstDateOfYear = DateTime.Parse(jarr["firstDateOfYear"].ToString());
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetSafetyDays(DB, date, firstDateOfYear, dept, type);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetKZAPDataTable(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                int process = int.Parse(jarr["process"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetKZAPDataTable(DB, dept,type, process, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetKZAPDataBarChart(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                int process = int.Parse(jarr["process"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetKZAPDataBarChart(DB, dept, type, process);
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetKZAPDataPieChart(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                int process = int.Parse(jarr["process"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetKZAPDataPieChart(DB, dept, type,process, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetKaizenUntilDate(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DateTime firstDate = DateTime.Parse(jarr["firstDate"].ToString());
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetKaizenUntilDate(DB, firstDate, date, dept, type);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetManpower(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetManpower(DB, dept, type, date);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetMainKaizenChart(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetMainKaizenChart(DB, date,  dept, type);
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
        #endregion
        #region DetailSafetyForm
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetSafetyTable(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime firstDateOfMonth = DateTime.Parse(jarr["firstDateOfMonth"].ToString());
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetSafetyTable(DB, firstDateOfMonth, dept, type);
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else if (dt.Columns.Count > 0)
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetSafetyChart(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string firstDateOfMonth = DateTime.Parse(jarr["firstDateOfMonth"].ToString()).ToString(Parameters.dateFormat);
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetSafetyChart(DB, firstDateOfMonth, dept, type);
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else if (dt.Columns.Count > 0)
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
        #endregion
        #region DetailKaizenForm
        
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetKaizenData(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                string dept = jarr["dept"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetKaizenData(DB, date, dept, type, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else if (dt.Columns.Count > 0)
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
        #endregion
   
        #region Kaizen Action
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetKaizenAction(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string from = jarr["from"].ToString();
                string to = jarr["to"].ToString();
                int type = int.Parse(jarr["type"].ToString());
                int process = int.Parse(jarr["process"].ToString());
                string dept = jarr["dept"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetKaizenAction(DB, from, to, type, dept, process, Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    // SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt); when dt have null column ,it will have a bug
                    //i do not how to write by english
                    ret.RetData =
                    SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject EditKaizenAction(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string id = jarr["id"].ToString();
                string department = jarr["department"].ToString();
                string finder = jarr["finder"].ToString();
                string problem = jarr["problem"].ToString();
                string measure = jarr["measure"].ToString();
                string principal = jarr["principal"].ToString();
                string remark = jarr["remark"].ToString();
                string createdDate = jarr["createdDate"].ToString();
                string planDate = jarr["planDate"].ToString();
                string finishDate = jarr["finishDate"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.EditKaizenAction(DB, id, department, finder, problem, measure, principal, remark, createdDate, planDate, finishDate);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject CreateKaizenAction(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string department = jarr["department"].ToString();
                string finder = jarr["finder"].ToString();
                string problem = jarr["problem"].ToString();
                string measure = jarr["measure"].ToString();
                string principal = jarr["principal"].ToString();
                string remark = jarr["remark"].ToString();
                string createdDate = jarr["createdDate"].ToString();
                string planDate = jarr["planDate"].ToString();
                string finishDate = jarr["finishDate"].ToString();
                string T1 = jarr["T1"].ToString();
                string T2 = jarr["T2"].ToString();
                string T3 = jarr["T3"].ToString();
                string T4 = jarr["T4"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.CreateKaizenAction(DB, department, finder, problem, measure, principal, remark, createdDate, planDate, finishDate, T1, T2, T3, T4);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject UpdateStatus(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                int id = int.Parse(jarr["id"].ToString());
                string T1 = jarr["T1"].ToString();
                string T2 = jarr["T2"].ToString();
                string T3 = jarr["T3"].ToString();
                string T4 = jarr["T4"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.UpdateStatus(DB, id, T1, T2, T3, T4);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        #endregion
        #region Maturity Assessment
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetMaturityList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetMaturityList(DB);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetMaturityAssessmentList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string deptCode = jarr["deptCode"].ToString();
                DateTime date = Convert.ToDateTime(jarr["date"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetMaturityAssessmentList(DB, deptCode, date);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveMaturityAssessment(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string deptCode = jarr["deptCode"].ToString();
                DateTime date = Convert.ToDateTime(jarr["date"].ToString());
                var listCode = jarr["listCode"];
                var listStatus = jarr["listStatus"];
                var listNote = jarr["listNote"];
                string[] arrCode = ((IEnumerable)listCode).Cast<object>()
                            .Select(x => x.ToString())
                            .ToArray();
                string[] arrStatus = ((IEnumerable)listStatus).Cast<object>()
                            .Select(x => x.ToString())
                            .ToArray(); 
                string[] arrNote = ((IEnumerable)listNote).Cast<object>()
                            .Select(x => x.ToString())
                            .ToArray();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
               // BLL.DeleteMaturityAssessment(DB, deptCode, date);
                BLL.SaveMaturityAssessment(DB, deptCode, date, arrCode, arrStatus, arrNote);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        #endregion
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetDeptList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string plant = jarr["plant"].ToString();
                string section = jarr["section"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetDeptList(DB, plant, section);
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else if (dt.Columns.Count > 0)
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
        #region Tier 1
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetTier1(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetTier1(DB, dept, date);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetTier1Standard(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetTier1Standard(DB, dept, date);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveTier1(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string G_DEPTCODE = jarr["G_DEPTCODE"].ToString();
                DateTime G_DATE = DateTime.Parse(jarr["G_DATE"].ToString());
                string G_1 = jarr["G_1"].ToString();
                string G_2 = jarr["G_2"].ToString();
                string G_3 = jarr["G_3"].ToString();
                string G_4 = jarr["G_4"].ToString();
                string G_5 = jarr["G_5"].ToString();
                string G_6 = jarr["G_6"].ToString();
                string G_7 = jarr["G_7"].ToString();
                string G_8 = jarr["G_8"].ToString();
                string G_RESULT = jarr["G_RESULT"].ToString();
                string G_AUDITOR = jarr["G_AUDITOR"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.SaveTier1(DB, G_DEPTCODE, G_DATE, G_1, G_2, G_3, G_4, G_5, G_6, G_7, G_8, G_RESULT, G_AUDITOR,
                         SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken));
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveTier1Standard(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string G_DEPTCODE = jarr["G_DEPTCODE"].ToString();
                DateTime G_DATE = DateTime.Parse(jarr["G_DATE"].ToString());
                string G_SUPERVISOR_1 = jarr["G_SUPERVISOR_1"].ToString();
                string G_SUPERVISOR_2 = jarr["G_SUPERVISOR_2"].ToString();
                string G_SUPERVISOR_3 = jarr["G_SUPERVISOR_3"].ToString();
                string G_SUPERVISOR_4 = jarr["G_SUPERVISOR_4"].ToString();
                string G_SUPERVISOR_5 = jarr["G_SUPERVISOR_5"].ToString();
                string G_SUPERVISOR_6 = jarr["G_SUPERVISOR_6"].ToString();
                string G_SUPERVISOR_7 = jarr["G_SUPERVISOR_7"].ToString();
                string G_SUPERVISOR_8 = jarr["G_SUPERVISOR_8"].ToString();
                string G_SUPERVISOR_AUDITOR = jarr["G_SUPERVISOR_AUDITOR"].ToString();
                string G_VSM_1 = jarr["G_VSM_1"].ToString();
                string G_VSM_2 = jarr["G_VSM_2"].ToString();
                string G_VSM_3 = jarr["G_VSM_3"].ToString();
                string G_VSM_4 = jarr["G_VSM_4"].ToString();
                string G_VSM_5 = jarr["G_VSM_5"].ToString();
                string G_VSM_6 = jarr["G_VSM_6"].ToString();
                string G_VSM_7 = jarr["G_VSM_7"].ToString();
                string G_VSM_8 = jarr["G_VSM_8"].ToString();
                string G_VSM_AUDITOR = jarr["G_VSM_AUDITOR"].ToString();
                string G_THIRD_PARTY_1 = jarr["G_THIRD_PARTY_1"].ToString();
                string G_THIRD_PARTY_2 = jarr["G_THIRD_PARTY_2"].ToString();
                string G_THIRD_PARTY_3 = jarr["G_THIRD_PARTY_3"].ToString();
                string G_THIRD_PARTY_4 = jarr["G_THIRD_PARTY_4"].ToString();
                string G_THIRD_PARTY_5 = jarr["G_THIRD_PARTY_5"].ToString();
                string G_THIRD_PARTY_6 = jarr["G_THIRD_PARTY_6"].ToString();
                string G_THIRD_PARTY_7 = jarr["G_THIRD_PARTY_7"].ToString();
                string G_THIRD_PARTY_8 = jarr["G_THIRD_PARTY_8"].ToString();
                string G_THIRD_PARTY_AUDITOR = jarr["G_THIRD_PARTY_AUDITOR"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.SaveTier1Standard(DB, G_DEPTCODE, G_DATE,
                        G_SUPERVISOR_1, G_SUPERVISOR_2, G_SUPERVISOR_3, G_SUPERVISOR_4, G_SUPERVISOR_5, G_SUPERVISOR_6, G_SUPERVISOR_7, G_SUPERVISOR_8, G_SUPERVISOR_AUDITOR,
                        G_VSM_1, G_VSM_2, G_VSM_3, G_VSM_4, G_VSM_5, G_VSM_6, G_VSM_7, G_VSM_8, G_VSM_AUDITOR,
                        G_THIRD_PARTY_1, G_THIRD_PARTY_2, G_THIRD_PARTY_3, G_THIRD_PARTY_4, G_THIRD_PARTY_5, G_THIRD_PARTY_6, G_THIRD_PARTY_7, G_THIRD_PARTY_8, G_THIRD_PARTY_AUDITOR,
                         SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken));
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetTHTByART(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string ART = jarr["ART"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetTHTByART(DB, ART);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveTHT(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string ART = jarr["ART"].ToString();
                string THT = jarr["THT"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.SaveTHT(DB, ART,THT);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekSafety(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekSafety(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetDeptType(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.GetDeptType(DB,dept);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekOutput(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekOutput(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekPPHTarget(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekPPHTarget(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekPPH(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekPPH(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_Kaizen(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                System.Data.DataTable dt = BLL.Tier1_Kaizen(DB, dept,date);
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekLLER(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekLLER(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekMulti(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekMulti(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveTier1_Downtime(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                string downtime = jarr["downtime"].ToString();
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.SaveTier1_Downtime(DB, dept, downtime,date);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekDowntime(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekDowntime(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveTier1_COT(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj =
                (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret =
                new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string dept = jarr["dept"].ToString();
                string target = jarr["target"].ToString();
                string actual = jarr["actual"].ToString();
                DateTime date = DateTime.Parse(jarr["date"].ToString());
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL BLL = new TierMeetingBLL();
                BLL.SaveTier1_COT(DB, dept, date, target, actual);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekCOT(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekCOT(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekHourlyOutput(object OBJ)
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
                string vLine = jarr["vLine"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); //星期天  年/月/日

                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekHourlyOutput(DB, vLine, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekRFT(object OBJ)
        {
            //SUN周日  MON周一   TUE 周二     WED  周三  THU 周四    FRI 周五   SAT  周六
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vDept = jarr["vDept"].ToString();
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;
                switch (dt.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = dt;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = dt.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = dt.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = dt.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = dt.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = dt.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = dt.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(Parameters.dateFormat).Replace('-', '/'); ;   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(Parameters.dateFormat).Replace('-', '/'); ;//星期天  年/月/日
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekRFT(DB, vDept, FirstDay, SeventhDay);
                if (dt1.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
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
        #endregion
    }
}
