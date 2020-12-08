using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJ_BASEAPI
{
    public class BASE
    {
        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetMaterielInfo(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            string Data = string.Empty;

            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);

                

                string key = "material_no";

                Dictionary<string, object> ReqP = SJeMES_Framework_NETCore.WebAPI.WebAPIHelper.GetWebParameters(key, ReqObj);

                if (!string.IsNullOrEmpty(ReqP["material_no"].ToString()))
                {
                    string selectkey = @"material_no,material_name,material_specifications,material_type,process_no";
                    Dictionary<string, object> selectp = SJeMES_Framework_NETCore.Common.StringHelper.GetDictionaryByString(selectkey);
                    DataTable BASE007M = DB.GetDataTable(SJeMES_Framework_NETCore.Common.StringHelper.GetSelectSqlByDictionary("base007m",
                        selectp, " material_no=@material_no ",ReqObj.UserToken), ReqP);

                    if (BASE007M.Rows.Count > 0)
                    {
                        string json = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(BASE007M);
                        Dictionary<string, object> p = new Dictionary<string, object>();
                        p.Add("BASE007M", json);
                        ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        ret.IsSuccess = true;
                    }
                    else
                    {
                        ret.IsSuccess = false;
                        ret.ErrMsg = "该品号下的物料资料不存在！";
                    }
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "参数为空！";
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
        /// 获取物料信息列表
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetMaterielList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            string Data = string.Empty;

            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();

                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(Data);


                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);//访问企业库



                string key = "Where,OrderBy,Page,PageRow";

                Dictionary<string, object> ReqP = SJeMES_Framework_NETCore.WebAPI.WebAPIHelper.GetWebParameters(key, ReqObj);


                string selectkey = @"material_no,material_name,material_specifications,material_iunit";

                Dictionary<string, object> selectp = SJeMES_Framework_NETCore.Common.StringHelper.GetDictionaryByString(selectkey);

                ReqP["Where"] = SJeMES_Framework_NETCore.Common.StringHelper.GetWhereWithAll(
SJeMES_Framework_NETCore.Common.StringHelper.GetSelectSqlByDictionary(DB.DataBaseType, "base007m",selectp,string.Empty).Replace("WHERE 1=1",""), ReqP["Where"].ToString(), ReqObj.UserToken);

             
                    DataTable BASE007M = DB.GetDataTable(
                        SJeMES_Framework_NETCore.Common.StringHelper.GetSqlCutPage(DB.DataBaseType,
                        SJeMES_Framework_NETCore.Common.StringHelper.GetSelectSqlByDictionary("base007m",
                        selectp, ReqP["Where"].ToString(), ReqObj.UserToken),
                        ReqP["PageRow"].ToString(),
                        ReqP["Page"].ToString(),
                        ReqP["OrderBy"].ToString()));


                int total = DB.GetInt32("select count(1) from base007m where 1=1 " + ReqP["Where"].ToString() + "");

                if (BASE007M.Rows.Count > 0)
                {
                    string headdata = string.Empty;
                    foreach (DataColumn dc in BASE007M.Columns)
                    {
                        headdata += dc.ColumnName + @",";
                    }
                    headdata.Remove(headdata.Length - 1);

                    string headkey = "品号,品名,规格,单位";
                    headdata = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonKeyValue(headkey, headdata);


                    string json = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(BASE007M);
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("headdata", headdata);
                    p.Add("data", json);
                    p.Add("total", total);
                    ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                    ret.IsSuccess = true;
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "";
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
        /// 获取工作中心列表接口
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetWorkcenterList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            string Data = string.Empty;

            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();

            try
            {

                Data = ReqObj.Data.ToString();
                
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);

                
                #region 接口参数
                string Where = jarr["Where"].ToString();//条件
                string OrderBy = jarr["OrderBy"].ToString();//排序
                string Page = jarr["Page"].ToString();//页数
                string PageRow = jarr["PageRow"].ToString();//行数
                #endregion
                #region 逻辑

                Where = SJeMES_Framework_NETCore.Common.StringHelper.GetWhereWithAll(
@" select productionline_no,productionline_name,productionline_description from base016m", Where, ReqObj.UserToken);


                int total = (int.Parse(Page) - 1) * int.Parse(PageRow);
                string sql = @"select * from (
select productionline_no,productionline_name,productionline_description,@n:= @n + 1 as RN from base016m M,(select @n:= 0) d
" + OrderBy + @") tab where  RN >" + total + " " + Where + "  limit " + PageRow + "";



                DataTable dt = DB.GetDataTable(sql);


                total = DB.GetInt32("select count(1) from base016m where 1=1 " + Where + "");

                if (dt.Rows.Count > 0)
                {
                    string headdata = string.Empty;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        headdata += dc.ColumnName + @",";
                    }
                    headdata.Remove(headdata.Length - 1);

                    string headkey = "工作中心,工作中心名称,工作中心描述";
                    headdata = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonKeyValue(headkey, headdata);


                    string json = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("headdata", headdata);
                    p.Add("data", json);
                    p.Add("total", total);
                    ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                    ret.IsSuccess = true;
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "";
                }
                #endregion

            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取供应商列表接口
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetSupplierList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            string Data = string.Empty;

            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();

            try
            {

                Data = ReqObj.Data.ToString();
                
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);

                
                #region 接口参数
                string Where = jarr["Where"].ToString();//条件
                string OrderBy = jarr["OrderBy"].ToString();//排序
                string Page = jarr["Page"].ToString();//页数
                string PageRow = jarr["PageRow"].ToString();//行数
                #endregion
                #region 逻辑
                int total = (int.Parse(Page) - 1) * int.Parse(PageRow);

                Where = SJeMES_Framework_NETCore.Common.StringHelper.GetWhereWithAll(
@" select suppliers_code,suppliers_name from base003m", Where, ReqObj.UserToken);


                string sql = @"select * from (
select suppliers_code,suppliers_name,@n:= @n + 1 as RN from base003m M,(select @n:= 0) d
" + OrderBy + @"
) tab where  RN >" + total + " " + Where + "  limit " + PageRow + "";
                DataTable dt = DB.GetDataTable(sql);
                total  = DB.GetInt32("select count(1) from base003m where 1=1 " + Where + "");

                if (dt.Rows.Count > 0)
                {
                    string headdata = string.Empty;
                    foreach(DataColumn dc in dt.Columns)
                    {
                        headdata += dc.ColumnName + @",";
                    }
                    headdata.Remove(headdata.Length - 1);

                    string headkey = "供应商代号,供应商名称";
                    headdata = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonKeyValue(headkey, headdata);

                    string json = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("headdata", headdata);
                    p.Add("data", json);
                    p.Add("total", total);
                    ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                    ret.IsSuccess = true;
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "";
                }
                #endregion

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
