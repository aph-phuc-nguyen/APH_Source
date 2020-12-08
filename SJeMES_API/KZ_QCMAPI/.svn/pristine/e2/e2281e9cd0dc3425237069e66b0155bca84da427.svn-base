using KZ_QCMAPI;
using KZ_QCMAPI.BLL;
using System;
using System.Collections.Generic;
using System.Data;


namespace KZ_QCMAPI.Controllers
{
    public class TierMeetingServer
    {

        /// <summary>
        ///查询公司当天的RFT数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_DayQuery(object OBJ)
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
                string line = jarr["line"].ToString();
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt = bLL.Tier1_DayQuery(DB, date,line,dateFormat);
                if (dt.Rows.Count > 0)
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

        /// <summary>
        ///查询当前部门当周RFT的数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier1_WeekQuery(object OBJ)
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
                //Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-', '/'); ;   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/'); ;//星期天  年/月/日
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier1_WeekQuery(DB, vDept, dateFormat, FirstDay, SeventhDay);
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
        ///查询当前部门当周RFT的数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier2_WeekQuery(object OBJ)
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
                string vSection = jarr["vSection"].ToString();
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;
                //Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-', '/'); ;   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/'); ;//星期天  年/月/日
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier2_WeekQuery(DB, vSection, dateFormat, FirstDay, SeventhDay);
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
        /// 查询当前厂区当天RFT的数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier3_DayQuery(object OBJ)
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
                string vPlant = jarr["vPlant"].ToString();
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                TierMeetingBLL bLL = new TierMeetingBLL();                
                if (string.IsNullOrEmpty(vPlant) || vPlant.Equals("ALL"))
                {
                    ret.ErrMsg = "厂区不能空";
                    ret.IsSuccess = false;
                }
                else
                {
                    System.Data.DataTable dt = bLL.Tier3_DayQuery(DB, date, vPlant, dateFormat);
                    if (dt.Rows.Count > 0)
                    {
                        ret.IsSuccess = true;
                        ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                    }
                    else
                    {
                        ret.IsSuccess = false;
                    }
                    
                }
                ret.IsSuccess = true;

            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        /// <summary>
        ///查询当前厂区当周RFT的数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier3_WeekQuery(object OBJ)
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
                string vPlant = jarr["vPlant"].ToString();
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DateTime dt = Convert.ToDateTime(date);//把日期强制转换一下
                DateTime firstDate = System.DateTime.Now;
                //Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken))
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-', '/'); ;   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/'); ;//星期天  年/月/日
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier3_WeekQuery(DB, vPlant, dateFormat, FirstDay, SeventhDay);    
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
        ///查询公司当天的RFT数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
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
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                TierMeetingBLL bLL = new TierMeetingBLL();
                    System.Data.DataTable dt = bLL.Tier4_DayQuery(DB, date, dateFormat);
                    if (dt.Rows.Count > 0)
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

        /// <summary>
        /// 查询公司当周的RFT数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject Tier4_WeekQuery(object OBJ)
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
                string date = jarr["date"].ToString();//日期格式   2020/09/01
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                List<string> sList = new List<string>();
                string dateFormat = DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken));               
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
                string FirstDay = firstDate.ToString(dateFormat).Replace('-','/');   //星期一   年/月/日
                string SeventhDay = firstDate.AddDays(6).ToString(dateFormat).Replace('-', '/'); ;//星期天  年/月/日
                TierMeetingBLL bLL = new TierMeetingBLL();
                System.Data.DataTable dt1 = bLL.Tier4_WeekQuery(DB, dateFormat, FirstDay, SeventhDay);
                if (dt1.Rows.Count >0)
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
