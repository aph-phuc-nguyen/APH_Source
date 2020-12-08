﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJ_SYSAPI
{
 public   class Role
    {
        /// <summary>
        /// 获取角色权限资料
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetRolePower(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            string Data = string.Empty;

            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB;
            SJeMES_Framework_NETCore.DBHelper.DataBase DBSYS;
            try
            {
                Data = ReqObj.Data.ToString();

                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                DBSYS = new SJeMES_Framework_NETCore.DBHelper.DataBase(string.Empty);

                ret.IsSuccess = true;

                #region 接口参数
                string Role = jarr["Role"].ToString();//条件

                #endregion
                #region 逻辑


                #region 数据获取SQL
                string datasql = @"
select
*
from sysmenu01m

";
                #endregion



                DataTable dt = DBSYS.GetDataTable(datasql);

                #region 数据获取SQL
                datasql = @"
select
*
from rolepower
where RoleName='"+Role+@"'

";
                #endregion

                

                DataTable dtRole = DB.GetDataTable(datasql);

                DataTable dtRole2 = dtRole.Clone();
                dtRole2.Columns.Remove("id");

                foreach(DataRow dr in dt.Rows)
                {
                    DataRow newDR = dtRole2.NewRow();

                    bool isHas = false;
                    foreach (DataRow dr2 in dtRole.Rows)
                    {
                        if (dr2["name"].ToString() == dr["name"].ToString())
                        {
                            isHas = true;

                            foreach (DataColumn dc in dtRole.Columns)
                            {
                                if (dc.ColumnName != "id")
                                {
                                    newDR[dc.ColumnName] = dr2[dc.ColumnName];
                                }
                            }
                        }

                    }

                    
                    newDR["MenuCode"] = dr["name"].ToString();
                    newDR["MenuName"] = dr["meta_title"].ToString();

                    
                    if(!isHas)
                    {
                        newDR["RoleName"] = Role;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            newDR[dc.ColumnName] = dr[dc.ColumnName];
                        }

                    }

                    dtRole2.Rows.Add(newDR);
                }


              

                string json = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dtRole2);
                Dictionary<string, object> p = new Dictionary<string, object>();




                p.Add("data", json);
               
                ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                ret.IsSuccess = true;

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
        /// 获取系统角色列表
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetRoleList(object OBJ)
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
               
                ret.IsSuccess = true;

                #region 接口参数
                string WHERE = jarr["Where"].ToString();//条件
                string OrderBy = jarr["OrderBy"].ToString();//排序
                string Page = jarr["Page"].ToString();//页数
                string PageRow = jarr["PageRow"].ToString();//行数
                #endregion
                #region 逻辑


                #region 数据获取SQL
                string datasql = @"
select
id,RoleName
from roleinfo
";
                #endregion

                DataTable dtColum = DB.GetDataTable(@"
SELECT
*
FROM (" + datasql + @") tmp1 where 1=2");


                WHERE = SJeMES_Framework_NETCore.Common.StringHelper.GetWhereWithAll(DB.DataBaseType, dtColum, WHERE);

                datasql = @"
SELECT
* FROM (" + datasql + @") tmp1 WHERE 1=1 " + WHERE;


                DataTable dt = DB.GetDataTable(SJeMES_Framework_NETCore.Common.StringHelper.GetSqlCutPage(DB.DataBaseType, datasql,
                    PageRow, Page, OrderBy));
                if (dt.Rows.Count > 0)
                {
                    string dtJson = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                    int total = DB.GetInt32(@"
select count(1) 
from(" + datasql + @") counttalbe
");

                    string json = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    string headdata = string.Empty;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        headdata += dc.ColumnName + @",";
                    }
                    headdata.Remove(headdata.Length - 1);
                    string headkey = "id,角色名称";
                    headdata = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonKeyValue(headkey, headdata);


                    p.Add("headdata", headdata);
                    p.Add("data", json);
                    //p.Add("team_no", json);
                    //p.Add("team_name", json);
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
        /// 获取系统角色信息
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetRoleInfo(object OBJ)
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
               
                ret.IsSuccess = true;
                string RoleName = jarr["RoleName"].ToString(); //角色名
                if (!string.IsNullOrEmpty(RoleName))
                {
                    string sql = "select * from rolepower where RoleName='" + RoleName + "'";
                    DataTable sysuser02m = DB.GetDataTable(sql);
                    if (sysuser02m.Rows.Count>0)
                    {
                        string json = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(sysuser02m);
                        Dictionary<string, object> p = new Dictionary<string, object>();
                        p.Add("rolepower", json);
                        ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        ret.IsSuccess = true;
                    }
                    else
                    {
                        ret.IsSuccess = false;
                        ret.ErrMsg = "该角色名下的角色资料不存在！";
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
        /// 删除系统角色
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject DelRole(object OBJ)
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
               
                ret.IsSuccess = true;
                string RoleName = jarr["RoleName"].ToString(); //角色名
                string sql = string.Empty;
                Dictionary<string, object> p = new Dictionary<string, object>();

                bool HasUser = false;

                if(DB.GetInt32(@"select count(1) from userinfo where roles='"+RoleName+@"'")==0)
                { 
             
                    sql = @"
delete from roleinfo where RoleName='" + RoleName + @"'
delete from rolepower where RoleName='" + RoleName + @"'
";
                    DB.ExecuteNonQueryOffline(sql);
                    ret.ErrMsg = "删除成功！";
                    ret.IsSuccess = true;
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "全在该角色下的用户，因此不能删除";
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
        /// 保存系统角色接口
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject SaveRole(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            string Data = string.Empty;

            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB;
            try
            {
                Data = ReqObj.Data.ToString();
                
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(Data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                string sql = string.Empty;

                DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(Data);

                if (dt.Rows.Count > 0)
                {
                    string RoleName = dt.Rows[0]["RoleName"].ToString();

                    if (DB.GetString(@"select 1 from roleinfo where RoleName='" + RoleName + @"'") == "1")
                    {
                        sql = @"Delete from rolepower where RoleName='"+RoleName+@"' ";
                        foreach (DataRow dr in dt.Rows)
                        {
                            sql += @"
Insert into rolepower
(RoleName,MenuCode,Select,Add,Edit,Delete,DoSure,Audit,DoWork,Print,Fun)
Values
('" + RoleName + @"','" + dr["MenuCode"].ToString() + @"','" + dr["Select"].ToString() + @"','" + dr["Add"].ToString() + @"','" + dr["Edit"].ToString() + @"','"
+ dr["Delete"].ToString() + @"','" + dr["DoSure"].ToString() + @"','" + dr["Audit"].ToString() + @"','" + dr["DoWork"].ToString() + @"','" + dr["Print"].ToString() + @"','" + dr["Fun"].ToString() + @"');
";
                        }
                        DB.ExecuteNonQueryOffline(sql);
                    }
                    else
                    {
                        sql = @"Insert into roleinfo (RoleName) values ('" + RoleName + @"') ";
                        foreach (DataRow dr in dt.Rows)
                        {
                            sql += @"
Insert into rolepower
(RoleName,MenuCode,Select,Add,Edit,Delete,DoSure,Audit,DoWork,Print,Fun)
Values
('"+RoleName+@"','"+dr["MenuCode"].ToString()+@"','" + dr["Select"].ToString() + @"','" + dr["Add"].ToString() + @"','" + dr["Edit"].ToString() + @"','"
+ dr["Delete"].ToString() + @"','" + dr["DoSure"].ToString() + @"','" + dr["Audit"].ToString() + @"','" + dr["DoWork"].ToString() + @"','" + dr["Print"].ToString() + @"','" + dr["Fun"].ToString() + @"');
";
                        }

                        DB.ExecuteNonQueryOffline(sql);
                    }

                    

                    ret.ErrMsg = "没有实现逻辑，请联系开发人员！";
                    ret.IsSuccess = true;
                }
                else
                {
                    ret.ErrMsg = "保存失败传入数据为空";
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

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject EditPower(Object OBJ)
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
                string RoleName = jarr["Role"].ToString();
                var o = JObject.Parse(Data);
                string Table = o["table"].ToString();


                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);


                DB.ExecuteNonQueryOffline(@"delete from rolepower where RoleName='" + RoleName + @"'");
                if (!string.IsNullOrEmpty(Table))
                {
                    string sql = string.Empty;
                    DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(Table);

                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<string, object> p = SJeMES_Framework_NETCore.Common.StringHelper.GetDictionaryByDataRow(dr);

                        DB.ExecuteNonQueryOffline(SJeMES_Framework_NETCore.Common.StringHelper.GetInsertSqlByDictionary(DB.DataBaseType, "rolepower", p), p);
                    }

                    ret.IsSuccess = true;
                    ret.ErrMsg = "保存成功！";

                }
                
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "角色不能为空！";
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
