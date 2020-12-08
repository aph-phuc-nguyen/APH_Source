using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class SYS
    {
        /// <summary>
        /// Help(帮助文档)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string Help(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                IsSuccess = true;

                RetData = @"
 SYS(系统)接口帮助 @END
 方法：Help(帮助),GetOrg(获取账套信息) @END
 @END
====================GetOrg方法调用=================== @END
 Data参数如下
 <Data> @END
 @END
 @END
 </Data> @END
 @END
 返回数据如下 @END
 <Return> @END
 <IsSuccess>true/false</IsSuccess> @END
 <RetData> @END
 如果成功：
 <DataTable> @END
 <Columns>org@;orgname@;dbtype@;dbserver@;dbname@;dbuser@;dbpassword</Columns> @END
 <Row></Row> @END
 </DataTable> @END
 如果失败：失败报错 @END
 </RetData> @END
 </Return> @END
 ================================================== @END
@END
====================GetCode方法调用=================== @END
 Data参数如下
 <Data> @END
 @END
 @END
 </Data> @END
 @END
 返回数据如下 @END
 <Return> @END
 <IsSuccess>true/false</IsSuccess> @END
 <Return> @END
 <IsSuccess>true/false</IsSuccess> @END
 <RetData> @END
 如果成功：<IsOk>True/False</IsOk> @END
 如果失败：失败报错 @END
 </RetData> @END
 </Return> @END
 ================================================== @END
";


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// GetOrg(获取账套)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string GetOrg(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
SELECT
org,
orgname,
dbtype,
dbserver,
dbname,
dbuser,
dbpassword
FROM SJEMSSYS.dbo.SYSORG01M
";

                System.Data.DataTable dt = DB.GetDataTable(sql);

                RetData = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }

        /// <summary>
        /// GetCode(获取Code)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetCode(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数
                string CodeData = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Code>", "</Code>");
                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string[] t = new string[1];
                t[0] = ",";
                string[] a = CodeData.Split(t, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, object> p = new Dictionary<string, object>();


                string sql = @"
SELECT  " + a[1].ToString() + @"  FROM " + a[0].ToString() + @" 
";
             
                IDataReader Row;
                Row = DB.GetDataTableReader(sql, p);
                p.Clear();

                if (Row.Read())
                {
                 
                    sql = @"
SELECT startIndex,endIndex,coderules_type,iscomplement,fixedvalue FROM BASE018A1 where coderules_no =@coderules_no ORDER BY  CAST(startIndex AS INT)
";
                    p.Add("@coderules_no", a[2].ToString());

                    string startIndex = string.Empty;
                    string endIndex = string.Empty;
                    string coderules_type = string.Empty;
                    string iscomplement = string.Empty;
                    string fixedvalue = string.Empty;
                    System.Data.DataTable dtBASE018A1 = DB.GetDataTable(sql, p);
                    p.Clear();

                    if (dtBASE018A1.Rows.Count > 0)
                    {
                        #region
                        foreach (DataRow dr in dtBASE018A1.Rows)
                        {
                            startIndex = dr[0].ToString();
                            endIndex = dr[1].ToString();
                            coderules_type = dr[2].ToString();
                            iscomplement = dr[3].ToString();
                            fixedvalue = dr[4].ToString();

                            if (coderules_type == "1")
                            {
                                Code += fixedvalue;
                                continue;
                            }

                            if (coderules_type == "2")
                            {
                                Code += DateTime.Today.ToString("yyyyMMdd");
                                continue;
                            }

                            if (coderules_type == "3")
                            {
                                  Code += DateTime.Now.ToString("hh:mm:ss");
                                  continue;
                            }

                            if (coderules_type == "4")
                            {
                                sql = @"
select isnull(max(" + a[1].ToString() + @"),0) as storage_plan from " + a[0].ToString() + @" WHERE " + a[1].ToString() + @" LIKE '" + DateTime.Today.ToString("yyyyMMdd") + @"%' 
";
                               
                                System.Data.DataTable dt = DB.GetDataTable(sql);
                             
                                if (dt.Rows[0][0].ToString() != "0")
                                {

                                    string lsh = dt.Rows[0][0].ToString();
                                    lsh = Convert.ToString(Convert.ToInt32(lsh.Substring(lsh.Length - 4,4)) + 1);
                                    if (lsh.Length == 1)
                                    {
                                        Code += "000" + lsh;
                                    }
                                    else if (lsh.Length == 2)
                                    {
                                        Code += "00" + lsh;
                                    }
                                    else if (lsh.Length == 3)
                                    {
                                        Code += "0" + lsh;
                                    }
                                    else if (lsh.Length == 4)
                                    {
                                        Code += lsh;
                                    }


                                }
                                else
                                {
                                        Code += "0001";
                                }
                                continue;
                            }//4

                        }//循环
#endregion
                    }
                    IsSuccess = true;
                    RetData = Code;
                }
                else
                {
                    RetData = "这个表没有该数列名数据";
                }

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }

        /// <summary>
        /// DoSure(确认单据)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string DoSure(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数
                string TableName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<TableName>", "</TableName>");
                string id = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<id>", "</id>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
SELECT dosureby FROM [" + TableName + @"]
WHERE id='" + id + @"'
";


                if (string.IsNullOrEmpty(DB.GetString(sql)))
                {
                    sql = @"
UPDATE [" + TableName + @"] SET status='7',dosureby='" + UserCode + @"',dosuredatetime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
WHERE id='" + id + @"'
";
                    if (DB.ExecuteNonQueryOffline(sql) > 0)
                    {

                        if (TableName == "WMS013M")
                        {
                            sql = @"
SELECT
(SELECT inventory_form FROM WMS013M(NOLOCK) WHERE id=" + id + @") AS 'inventory_form',
warehouse,
location,
WMS012A1.UDF01,
WMS012A1.material_no,
material_name,
material_specifications,
'0' as 'qty',
WMS012A1.qty as 'qty_original',
-WMS012A1.qty as 'qty_difference'
FROM WMS012A1(NOLOCK)
join BASE007M(NOLOCK) ON WMS012A1.material_no=BASE007M.material_no
WHERE location >=(SELECT location1 FROM WMS013M(NOLOCK) WHERE id=" + id + @")
AND location<=(SELECT location2 FROM WMS013M(NOLOCK) WHERE id=" + id + @") AND WMS012A1.qty>0
ORDER BY warehouse,location,material_no,WMS012A1.UDF01
";
                            IDataReader idr = DB.GetDataTableReader(sql);
                            while (idr.Read())
                            {
                                Dictionary<string, object> P = new Dictionary<string, object>();
                                P.Add("inventory_form", idr["inventory_form"].ToString());
                                P.Add("warehouse", idr["warehouse"].ToString());
                                P.Add("location", idr["location"].ToString());
                                P.Add("material_no", idr["material_no"].ToString());
                                P.Add("material_name", idr["material_name"].ToString());
                                P.Add("material_specifications", idr["material_specifications"].ToString());
                                P.Add("qty", idr["qty"].ToString());
                                P.Add("qty_original", idr["qty_original"].ToString());
                                P.Add("qty_difference", idr["qty_difference"].ToString());

                                sql = @"
Insert INTO WMS013A2
(inventory_form,warehouse,location,material_no,material_name,material_specifications,qty,qty_original,qty_difference,UDF01)
VALUES
(@inventory_form,@warehouse,@location,@material_no,@material_name,@material_specifications,@qty,@qty_original,@qty_difference,'"+idr["UDF01"].ToString()+@"')
";
                                DB.ExecuteNonQueryOffline(sql, P);
                            }







                            sql = @"
SELECT
(SELECT inventory_form FROM WMS013M(NOLOCK) WHERE id='" + id + @"') AS 'inventory_form',
BASE011M.warehouse,
CODE002M.location,
CODE002M.UDF01,
CODE002M.lot_barcode,
CODE002M.material_no,
CODE002M.material_name,
CODE002M.material_specifications,
'0' as 'qty',
SUM(qty) as 'qty_original',
-SUM(qty) as 'qty_difference'
FROM
CODE002M(NOLOCK)
JOIN BASE011M(NOLOCK) ON CODE002M.location = BASE011M.location_no
WHERE  CODE002M.status='6' AND location >=(SELECT location1 FROM WMS013M(NOLOCK) WHERE id='" + id + @"')
AND location<=(SELECT location2 FROM WMS013M(NOLOCK) WHERE id='" + id + @"') 
GROUP BY
BASE011M.warehouse,
CODE002M.location,
CODE002M.lot_barcode,
CODE002M.material_no,
CODE002M.material_name,
CODE002M.material_specifications
CODE002M.UDF01
";
                            idr = DB.GetDataTableReader(sql);
                            while (idr.Read())
                            {
                                Dictionary<string, object> P = new Dictionary<string, object>();
                                P.Add("inventory_form", idr["inventory_form"].ToString());
                                P.Add("warehouse", idr["warehouse"].ToString());
                                P.Add("location", idr["location"].ToString());
                                P.Add("material_no", idr["material_no"].ToString());
                                P.Add("material_name", idr["material_name"].ToString());
                                P.Add("material_specifications", idr["material_specifications"].ToString());
                                P.Add("qty", idr["qty"].ToString());
                                P.Add("qty_original", idr["qty_original"].ToString());
                                P.Add("qty_difference", idr["qty_difference"].ToString());
                                P.Add("lot_barcode", idr["lot_barcode"].ToString());

                                

                                    sql = @"
Insert INTO WMS013A5
(inventory_form,warehouse,lot_barcode,location,material_no,material_name,material_specifications,qty,qty_original,qty_difference,UDF01)
VALUES
(@inventory_form,@warehouse,@lot_barcode,@location,@material_no,@material_name,@material_specifications,@qty,@qty_original,@qty_difference,'"+ idr["UDF01"].ToString()+@"')
";
                                    DB.ExecuteNonQueryOffline(sql, P);
                                
                            }


                           


                            IsSuccess = true;


                        }
                        else
                        {
                            RetData = "确认单据失败，没有更新到数据";
                        }
                    }

                    IsSuccess = true;
                }
                else
                {
                    RetData = "单据已确认";
                }

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// NoDoSure(取消确认单据)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string NoDoSure(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数
                string TableName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<TableName>", "</TableName>");
                string id = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<id>", "</id>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
SELECT dosureby FROM [" + TableName + @"]
WHERE id='" + id + @"'
";


                if (!string.IsNullOrEmpty(DB.GetString(sql)))
                {
                    string status = DB.GetString(@"
SELECT status FROM [" + TableName + @"]
WHERE id='" + id + @"'
");
                    if (status == "7" || status == "1")
                    {

                        sql = @"
UPDATE [" + TableName + @"] SET status='8',dosureby='',dosuredatetime=''
WHERE id='" + id + @"'
";
                        if (DB.ExecuteNonQueryOffline(sql) > 0)
                        {

                            IsSuccess = true;

                        }
                        else
                        {
                            RetData = "确认单据失败，没有更新到数据";
                        }
                    }
                }
                else
                {
                    RetData = "取消失败,单据不是已确认状态";
                }

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// CheckSYSSN(检查数据库授权情况)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string CheckSYSSN(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string PCSN = DB.GetString("SELECT SERVER FROM SYSS02M");

                if (GDSJ_Framework.Common.Security.GetPCSN() != PCSN)
                {
                    RetData = "系统授权出错：PCSN";
                }
                else
                { 
                    string APPSN = DB.GetString("SELECT App_SN FROM SYSAPP02M");

                    DataTable dt = DB.GetDataTable("SELECT AppCode FROM SYSAPP03M ORDER BY id asc");

                    string APPSNTmp = string.Empty;

                    foreach (DataRow DR in dt.Rows)
                    {
                        APPSNTmp += DR[0].ToString();
                    }

                    if (APPSN != GDSJ_Framework.Common.Security.MD5(APPSNTmp))
                    {
                        RetData = "系统授权出错：APPSN";
                    }
                    else
                    {
                        string SN = DB.GetString("SELECT SN FROM SYSS01M");

                        if(SN !=GDSJ_Framework.Common.Security.GetSN(PCSN,APPSN))
                        {
                            RetData = "系统授权出错：SN";
                        }
                        else
                        {
                            IsSuccess = true;

                            RetData = "<SYSAPP03M>" + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt) + @"</SYSAPP03M>";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// GetPermissions(获取模块权限)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetPermissions(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");
                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);


                string sql = @"
SELECT * FROM SYSUSER02M WHERE UserCode='"+UserCode+@"'
";
                DataTable dt = DB.GetDataTable(sql);

                if(dt.Rows.Count>0  || UserCode.ToLower()=="admin")
                {
                    IsSuccess = true;

                    RetData = "<SYSUSER02M>" + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt) + @"</SYSUSER02M>";
                }
                else
                {
                    RetData = "账号[" + UserCode + @"]没有系统权限";
                }

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// GetPDAVer(获取PDA版本)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetPDAVer(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

               
                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                List<GDSJ_Framework.Common.IOHelper.File> files = new List<GDSJ_Framework.Common.IOHelper.File>();
                files = GDSJ_Framework.Common.IOHelper.GetAllFile(System.AppDomain.CurrentDomain.BaseDirectory+@"\bin\App", files, 0);

                string Var1 = string.Empty; 
                string Var2 = string.Empty;

                foreach(GDSJ_Framework.Common.IOHelper.File f in files)
                {
                    if(f.Name.StartsWith("pda_wms"))
                    {
                        try
                        {
                            string[] s = f.Name.Replace(".apk", "").Split('_');

                            Var1 = s[2];
                            Var2 = s[3];
                            IsSuccess = true;
                            RetData = "<Var1>" + Var1 + "</Var1><Var2>" + Var2 + "</Var2>";
                        }
                        catch
                        {
                            RetData = "PDA程序命名不规范";
                        }
                    }
                }

                if(string.IsNullOrEmpty(Var1))
                {
                    RetData = "没有PDA程序";
                }

                
            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// SaveMenuLog(记录模块使用)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string SaveMenuLog(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                string MenuName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MenuName>", "</MenuName>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
DELETE FROM SYSUSER03M WHERE AppCode='"+MenuName +@"' AND UserCode='"+UserCode+ @"'
INSERT INTO SYSUSER03M
(UserCode,AppCode,DateTime)
Values
('"+UserCode+@"','"+MenuName+@"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+@"')
";
                DB.ExecuteNonQueryOffline(sql);

                sql = @"
UPDATE SYSUSER04M SET Times=Times+1 WHERE AppCode='" + MenuName + @"' AND UserCode='" + UserCode + @"' 
";
                if(DB.ExecuteNonQueryOffline(sql)==0)
                {
                    sql = @"INSERT INTO SYSUSER04M
(UserCode, AppCode, Times)
Values
('" + UserCode+@"', '"+MenuName+@"', 1)
";
                    DB.ExecuteNonQueryOffline(sql);
                }

                IsSuccess = true;
                


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }

        /// <summary>
        /// SaveMenuFav(收藏)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string SaveMenuFav(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                string AppCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<AppCode>", "</AppCode>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);


                string sql = @"
DELETE FROM SYSUSER05M WHERE AppCode='" + AppCode + @"' AND UserCode='" + UserCode + @"'  
";
                if (DB.ExecuteNonQueryOffline(sql) == 0)
                {
                    sql = @"INSERT INTO SYSUSER05M
(UserCode, AppCode)
Values
('" + UserCode + @"', '" + AppCode + @"')
";
                    DB.ExecuteNonQueryOffline(sql);
                }

                IsSuccess = true;



            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// SaveUILanguage(保存多语言记录)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string SaveUILanguage(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                string MenuName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MenuName>", "</MenuName>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");
                string MenuText = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MenuText>", "</MenuText>");

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);


                string sql = @"
SELECT * FROM SYSLAN01M
WHERE contorl ='" + MenuName + @"'
";
                if (string.IsNullOrEmpty(DB.GetString(sql)))
                {
                    sql = @"INSERT INTO SYSLAN01M
(contorl, cn,hk,en)
Values
('" + MenuName + @"', '" + MenuText + @"','" + MenuText + "','" + MenuText + @"')
";
                    DB.ExecuteNonQueryOffline(sql);
                }

                IsSuccess = true;



            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }



        /// <summary>
        /// GetUILanguage(获取多语言记录)
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetUILanguage(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                string MenuName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MenuName>", "</MenuName>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");
                string MenuText = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MenuText>", "</MenuText>");

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);


                string sql = @"
SELECT * FROM SYSLAN01M
WHERE contorl ='" + MenuName + @"'
";
                if (string.IsNullOrEmpty(DB.GetString(sql)))
                {
                    sql = @"INSERT INTO SYSLAN01M
(contorl, cn,hk,en)
Values
('" + MenuName + @"', '" + MenuText + @"','" + MenuText + "','" + MenuText + @"')
";
                    DB.ExecuteNonQueryOffline(sql);
                }

                IsSuccess = true;



            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }

        /// <summary>
        /// 写入PDA多语言信息
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string SetLanguage(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                string Type = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Type>", "</Type>");
                string Key = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Key>", "</Key>");
                string cn = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<cn>", "</cn>");
                string hk = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<hk>", "</hk>");
                string en = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<en>", "</en>");

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                DataTable dt;
                string sql = string.Empty;
                string sql2 = string.Empty;

                if (Type == "1")
                {
                    sql = @"
SELECT [contorl]  FROM [SYSLAN03M] where contorl = '" + Key + "'";
                    dt = DB.GetDataTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        sql = @"
update SYSLAN03M  set cn='" + cn + "',hk='" + hk + "',en='" + en + "' where contorl='" + Key + "'";
                    }
                    else
                    {
                        sql = @"
insert into SYSLAN03M([contorl],[cn],[hk],[en]) values('" + Key + "','" + cn + "','" + hk + "','" + en + "')";
                    }

                }
                else if (Type == "2")
                {
                    sql = @"
SELECT [msg]  FROM [SYSLAN04M] where msg = '" + Key + "'";
                    dt = DB.GetDataTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        sql = @"
update SYSLAN04M  set cn='" + cn + "',hk='" + hk + "',en='" + en + "' where msg='" + Key + "'";
                    }
                    else
                    {
                        sql = @"
insert into SYSLAN04M([msg],[cn],[hk],[en]) values('" + Key + "','" + cn + "','" + hk + "','" + en + "')";
                    }

                }

                DB.ExecuteNonQueryOffline(sql);
                IsSuccess = true;

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;

        }


        /// <summary>
        /// 获取PDA多语言信息
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetLanguage(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            string Code = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数

                string Type = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Type>", "</Type>");
                string Key = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Key>", "</Key>");
                string Language = "," + GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Language>", "</Language>");
                if (Language == ",")
                {
                    Language = "";
                }


                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);
                System.Data.DataTable dt;

                if (Type == "1")
                {
                    string sql1 = @"
SELECT [contorl] " + Language + " FROM [SYSLAN03M]";
                    dt = DB.GetDataTable(sql1);
                }
                else
                {
                    string sql2 = @"
SELECT [msg]" + Language + " FROM [SYSLAN04M]";
                    dt = DB.GetDataTable(sql2);
                }


                string dtXML1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                if (dt.Rows.Count > 0)
                {
                    if (Type == "1")
                    {
                        RetData = "<SYSLAN03M>" + dtXML1 + @"</SYSLAN03M>";
                    }
                    else if (Type == "2")
                    {
                        RetData = "<SYSLAN04M>" + dtXML1 + @"</SYSLAN04M>";
                    }
                    IsSuccess = true;
                }

                else
                {
                    RetData = "不存在数据";
                }

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;

        }
    }
}
