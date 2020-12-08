using KZ_JMSAPI.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_JMSAPI.Controllers
{
    class JMS_GoalAccomplished_ListServer
    {
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject insert(object OBJ)
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
                JMS_GoalAccomplished_ListBLL bll = new JMS_GoalAccomplished_ListBLL();
                bll.insert(DB, dt,SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken),DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                ret.ErrMsg = "数据导入成功";
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier4_DayQuery(object OBJ)
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
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                JMS_GoalAccomplished_ListBLL bll = new JMS_GoalAccomplished_ListBLL();
                DataTable dataTable = bll.Tier4_DayQuery(DB, date, DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (dataTable.Rows.Count>0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dataTable);
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.RetData = "查无数据";
                }             
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier4_PORateQuery(object OBJ)
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
                DateTime plan_finish_date = DateTime.Parse(jarr["date"].ToString());
                DateTime firstDate = System.DateTime.Now; //定义第一天
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));          
                switch (plan_finish_date.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        firstDate = plan_finish_date;
                        break;
                    case System.DayOfWeek.Tuesday:
                        firstDate = plan_finish_date.AddDays(-1);
                        break;
                    case System.DayOfWeek.Wednesday:
                        firstDate = plan_finish_date.AddDays(-2);
                        break;
                    case System.DayOfWeek.Thursday:
                        firstDate = plan_finish_date.AddDays(-3);
                        break;
                    case System.DayOfWeek.Friday:
                        firstDate = plan_finish_date.AddDays(-4);
                        break;
                    case System.DayOfWeek.Saturday:
                        firstDate = plan_finish_date.AddDays(-5);
                        break;
                    case System.DayOfWeek.Sunday:
                        firstDate = plan_finish_date.AddDays(-6);
                        break;
                }
                string FirstDay = firstDate.ToString(dateFormat);   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat);//星期天  年/月/日
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                JMS_GoalAccomplished_ListBLL bll = new JMS_GoalAccomplished_ListBLL();
                DataTable dataTable = bll.Tier4_PORateQuery(DB, FirstDay, SeventhDay, DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dataTable);
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
