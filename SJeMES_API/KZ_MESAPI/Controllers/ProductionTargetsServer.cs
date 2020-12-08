using KZ_MESAPI.BLL;
using SJ_BASEHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace KZ_MESAPI.Controllers
{
    public class ProductionTargetsServer
    {

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject TestUpLoad(object OBJ)
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
                int errorCount = 0;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("订单号");
                dataTable.Columns.Add("SIZE");
                dataTable.Columns.Add("计划数量");
                dataTable.Columns.Add("部门");
                dataTable.Columns.Add("生产日期");
                dataTable.Columns.Add("报错明细说明");
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    Random random = new Random();
                    string str = "";
                    errorCount++;
                    DataRow dr = dataTable.NewRow();
                    dr[0] = dt.Rows[i][0].ToString();
                    dr[1] = dt.Rows[i][1].ToString();
                    dr[2] = dt.Rows[i][2] == null ? 0 : Decimal.Parse(dt.Rows[i][2].ToString());
                    dr[3] = dt.Rows[i][4].ToString();
                    dr[4] = DateTime.Parse(dt.Rows[i][5].ToString());
                    if(random.Next(1, 100)>10)
                    {
                        str += "中文？？英文？？越南文??---订单号" + dt.Rows[i][0].ToString();
                    }
                    if (random.Next(1, 100) > 10)
                    {
                        str += "中文？？英文？？越南文??---SIZE" + dt.Rows[i][1].ToString();
                    }
                    if (random.Next(1, 100) > 10)
                    {
                        str += "中文？？英文？？越南文??---计划数量" + dt.Rows[i][2].ToString();
                    }
                    if (random.Next(1, 100) > 10)
                    {
                        str += "中文？？英文？？越南文??---部门" + dt.Rows[i][4].ToString();
                    }
                    if (random.Next(1, 100) > 10)
                    {
                        str += "中文？？英文？？越南文??---生产日期" + dt.Rows[i][5].ToString();
                    }
                    dr[5] =str;
                    dataTable.Rows.Add(dr);
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dataTable);
                    ret.ErrMsg = "添加失败！导入资料中存在" + errorCount + "错误资料!";
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
        ///用于生管上传日生产指示的接口
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject UpLoad(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data= ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,Object>>(Data);
                string data = jarr["data"].ToString();
                DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionTargetsBLL bLL = new ProductionTargetsBLL();
                DataTable tab = bLL.UpLoad(DB,dt, 
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), 
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                if (tab.Rows.Count > 0)
                {
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(tab);
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

        /// <summary>
        /// 用于日生产指示查询
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
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
                string vSeId = jarr["vSeId"].ToString();
                string vDDept = jarr["vDDept"].ToString();
                string vArtNo = jarr["vArtNo"].ToString();
                string vWrokDay = jarr["vWrokDay"].ToString(); 
                string vEndWrokDay = jarr["vEndWrokDay"].ToString();
                string vStatus = jarr["vStatus"].ToString();
                string vColumn1 = jarr["vColumn1"].ToString();
                string vInOut = jarr["vInOut"].ToString();
                string grantUserCode = SJeMES_Framework_NETCore.Web.System.GetUserCode(ReqObj);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionTargetsBLL bLL = new ProductionTargetsBLL();
                System.Data.DataTable  dt=bLL.Query(DB, vSeId, vDDept, vArtNo, vWrokDay, vEndWrokDay,vStatus, vColumn1, vInOut,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), 
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    grantUserCode,
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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


        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QuerySize(object OBJ)
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
                string vSeId = jarr["vSeId"].ToString();
                string vDDept = jarr["vDDept"].ToString();
                string vWrokDay = jarr["vWrokDay"].ToString();
                string vColumn1 = jarr["vColumn1"].ToString();
                string vInOut = jarr["vInOut"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionTargetsBLL bLL = new ProductionTargetsBLL();
                System.Data.DataTable dt = bLL.QuerySize(DB, vSeId, vDDept,vWrokDay,vColumn1, vInOut,
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), 
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject UpdateStatus(object OBJ)
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
                string vStatus = jarr["status"].ToString();
                DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(data);
                //Data = ReqObj.Data.ToString();
                //var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                //string vSeId = jarr["vSeId"].ToString();
                //string vDDept = jarr["vDDept"].ToString();
                //string vWrokDay = jarr["vWrokDay"].ToString();
                //string vColumn1 = jarr["vColumn1"].ToString();
                //string vInOut = jarr["vInOut"].ToString();
                //string vStatus = jarr["status"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionTargetsBLL bLL = new ProductionTargetsBLL();
                bLL.UpdateStatus(DB, dt, vStatus, 
                    SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), 
                    Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)),
                    DateTimeFormat.getDateTimeFormatValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

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
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                ProductionTargetsBLL bLL = new ProductionTargetsBLL();
                System.Data.DataTable dt = bLL.LoadSeDept(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject PlanAdjustQuery(object OBJ)
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
                ProductionTargetsBLL bLL = new ProductionTargetsBLL();
                //System.Data.DataTable dt = bLL.PlanAdjustQuery(DB, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken), Organization.getValue(SJeMES_Framework_NETCore.Web.System.GetCompanyCodeByToken(ReqObj.UserToken)));
                ret.IsSuccess = true;
                //ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject MailTest(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();

                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                Dictionary<string, string> d = new Dictionary<string, string>();
                d["USERCODE"] = "谢灿能";
                d["EMAIL"] = "sunwy-xie@apachefootwear.com";
                d["SENDTEXT"] = "这是一封测试邮件";
                list.Add(d);
                SJeMES_Framework_NETCore.WebAPI.ResultObject result = SendMessageHelper.SendEMAIL(list);

                if (!result.IsSuccess)
                    throw new Exception(result.ErrMsg);

                List<Dictionary<string, string>> listData = new List<Dictionary<string, string>>();

                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string sql = @"select staff_no,staff_name,udf05 from hr001m where UDF01='BIT01' and udf05 is not null";
                DataTable dataTable = DB.GetDataTable(sql);
                for (int i=0;i<dataTable.Rows.Count;i++)
                {
                    Dictionary<string, string> weixinD = new Dictionary<string, string>();
                    weixinD["USERCODE"] = dataTable.Rows[i]["staff_name"].ToString();
                    weixinD["WEIXINCODE"] = dataTable.Rows[i]["udf05"].ToString();
                    weixinD["SENDTEXT"] = "这是一条微信推送消息【20200417】";
                    listData.Add(weixinD);
                }
                SJeMES_Framework_NETCore.WebAPI.ResultObject weixinResult = SendMessageHelper.SendWEIXIN(listData);
                if (!weixinResult.IsSuccess)
                    throw new Exception(weixinResult.ErrMsg);

                ret.IsSuccess = true;
                ///哪里报错了？断点
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
