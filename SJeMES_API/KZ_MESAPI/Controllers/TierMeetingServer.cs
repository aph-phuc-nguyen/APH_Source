using KZ_MESAPI;
using KZ_MESAPI.BLL;
using System;
using System.Collections.Generic;
using System.Data;


namespace KZ_MESAPI.Controllers
{
    public class TierMeetingServer
    {

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
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/'); //星期天  年/月/日
        
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekPPH(DB, dateFormat, vLine, FirstDay, SeventhDay);
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier2_DayPPH(object OBJ)
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
                string section = jarr["section"].ToString();//日期格式   2020/09/01  输入的日期
                string date = jarr["date"].ToString().Replace('-', '/');//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                string date2 = dt.AddDays(1).ToString(dateFormat).Replace('-', '/');
                TierMeetingBLL bLL = new TierMeetingBLL();
                if (string.IsNullOrEmpty(date))
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "时间为空";
                }
                else
                {
                    System.Data.DataTable dt1 = bLL.Tier2_DayPPH(DB, date, date2,section,dateFormat);
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
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 某一个厂区一周的PPH
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier2_WeekPPH(object OBJ)
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
                string vSection = jarr["vSection"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/'); //星期天  年/月/日
                int i = DateTime.Parse(firstDate.AddDays(6).ToString(dateFormat)).CompareTo(DateTime.Parse(DateTime.Now.ToString(dateFormat)));
                if (i >= 0)
                {
                    SeventhDay = DateTime.Now.ToString(dateFormat);
                }
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier2_WeekPPH(DB, dateFormat, vSection, FirstDay, SeventhDay);
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

        /// <summary>
        /// 某一个厂区一天PPH
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier3_DayPPH(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try{
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vPlant = jarr["vPlant"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01  输入的日期
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                string date2 = dt.AddDays(1).ToString(dateFormat);    //把日期加1   between 输入的日期  to  加一日期   查出输入的日期       
                TierMeetingBLL bLL = new TierMeetingBLL();
                if (string.IsNullOrEmpty(vPlant) || string.IsNullOrEmpty(date))
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "没有厂区或时间";
                }
                else
                {
                    System.Data.DataTable dt1 = bLL.Tier3_DayPPH(DB, date,date2, vPlant, dateFormat);
                    if(dt1.Rows.Count > 0){
                        ret.IsSuccess = true;
                        ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
                    }
                    else
                    {
                        ret.IsSuccess = false;
                    }
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

        /// <summary>
        /// 某一个厂区一周的PPH
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier3_WeekPPH(object OBJ)
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
                string vPlant = jarr["vPlant"].ToString();//厂区
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;//当前时间
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-', '/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/'); //星期天  年/月/日
                int i = DateTime.Parse(firstDate.AddDays(6).ToString(dateFormat)).CompareTo(DateTime.Parse(DateTime.Now.ToString(dateFormat)));
                if (i >= 0)
                {
                    SeventhDay = DateTime.Now.ToString(dateFormat);
                }
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier3_WeekPPH(DB, dateFormat, vPlant, FirstDay, SeventhDay);
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

        /// <summary>
        /// 公司一天的PPH
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="date"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier4_DayPPH(object OBJ)
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
                string date = jarr["date"].ToString().Replace('-','/');//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                string date2 = dt.AddDays(1).ToString(dateFormat).Replace('-', '/');
                TierMeetingBLL bLL = new TierMeetingBLL();
                if ( string.IsNullOrEmpty(date))
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "时间为空";
                }
                else
                {
                    System.Data.DataTable dt1 = bLL.Tier4_DayPPH(DB, date,date2, dateFormat);
                    if (dt1.Rows.Count>0)
                    {
                        ret.IsSuccess = true;
                        ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt1);
                    }
                    else
                    {
                        ret.IsSuccess = false;
                    }
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
        /// 公司一周的PPH
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier4_WeekPPH(object OBJ)
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
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-','/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/');//星期天  年/月/日
                int i=DateTime.Parse(firstDate.AddDays(6).ToString(dateFormat)).CompareTo(DateTime.Parse(DateTime.Now.ToString(dateFormat)));
                if (i>=0)
                {
                    SeventhDay = DateTime.Now.ToString(dateFormat);
                }
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier4_WeekPPH(DB, dateFormat, FirstDay, SeventhDay);
                if (dt1.Rows.Count>0)
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
    }
}
