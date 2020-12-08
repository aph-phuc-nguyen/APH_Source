using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class WMS005
    {
        public static string Audit(object OBJ)
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
                #region 接口参数
                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");
                string DBTYPE = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBTYPE>", "</DBTYPE>");
                string DBSERVER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBSERVER>", "</DBSERVER>");
                string DBNAME = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBNAME>", "</DBNAME>");
                string DBUSER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBUSER>", "</DBUSER>");
                string DBPASSWORD = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBPASSWORD>", "</DBPASSWORD>");

              
                string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                System.Data.DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);

                #endregion

                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS003"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {
                    string where = string.Empty;
                    string[] a;
                    if (allocation_plan.Contains(","))
                    {

                        string[] s = new string[1];
                        s[0] = ",";
                        a = allocation_plan.Split(s, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        a = new string[1];
                        a[0] = allocation_plan;
                    }

                    for (int i = 0; i < a.Length; i++)
                    {
                        bool IsOk = false;
                        string ErrMsg = string.Empty;

                        DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                        guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                        //2.查询WMS003A2看是否有没有审核过的数据，如果没有，报错：该计划没有新的入库资料，不能进行审核。

                        #region 逻辑
                        string sql = @"
SELECT COUNT(*)
FROM WMS005A2
WHERE allocation_plan=@allocation_plan AND status='1'
";
                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        p1.Add("@allocation_plan", a[i].ToString());
                        string rowNumber = DB.GetString(sql, p1);
                        
                        if (rowNumber == "0")  //报错
                        {
                            IsOk = false;
                            ErrMsg = "该计划没有新的入库资料，不能进行审核";
                        }
                        else //未审核
                        {
                            sql = @"
SELECT MAX(allocation_doc) FROM WMS006M
";
                            #region 产生调拨单号
                            string allocation_doc = DB.GetString(sql);

                            if (allocation_doc.IndexOf(DateTime.Now.ToString("yyyyMMdd")) > -1)
                            {
                                allocation_doc = DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(allocation_doc.Replace(DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");
                            }
                            else
                            {
                                allocation_doc = DateTime.Now.ToString("yyyyMMdd") + "0001";
                            }
                            #endregion

                            sql = @"
SELECT allocation_plan,front_document_type,front_document,status,dosureby,dosuredatetime,auditby,auditdatetime from WMS005M where allocation_plan=@allocation_plan 
";
                            Dictionary<string, object> p2 = new Dictionary<string, object>();
                            p2.Add("@allocation_plan", a[i].ToString());
                            IDataReader idr = DB.GetDataTableReader(sql, p2);
                            while (idr.Read())//005M表
                            {
                                string dosureby = idr["dosureby"].ToString();
                                string dosuredatetime = idr["dosuredatetime"].ToString();
                                string auditby = idr["auditby"].ToString();
                                string auditdatetime = idr["auditdatetime"].ToString();

                                //5M-->6M
                                sql = @"
INSERT INTO WMS006M(allocation_doc,front_document_type,front_document,status,createby,createdate,createtime)
values
('" + allocation_doc + "','6','" + allocation_plan + @"','1','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')
";

                                DB.ExecuteNonQueryOffline(sql);

                                sql = @"
SELECT id,allocation_plan,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,warehouse,location,warehouse_target,location_target,qty,qty_int
from WMS005A2 
WHERE allocation_plan=@allocation_plan
and status='1'
";       
                                Dictionary<string, object> p3 = new Dictionary<string, object>();
                                p3.Add("@allocation_plan", a[i].ToString());
                                IDataReader idr2 = DB.GetDataTableReader(sql, p3);
                                while (idr2.Read())//005A2表
                                {
                                    string id = idr2["id"].ToString();
                                    string serial_number = idr2["serial_number"].ToString();
                                    string material_no = idr2["material_no"].ToString();
                                    string material_name = idr2["material_name"].ToString();
                                    string material_var = idr2["material_var"].ToString();
                                    string material_specifications = idr2["material_specifications"].ToString();
                                    string suppliers_lot = idr2["suppliers_lot"].ToString();
                                    string lot_barcode = idr2["lot_barcode"].ToString();

                                    string warehouse = idr2["warehouse"].ToString();
                                    string warehouse_target = idr2["warehouse_target"].ToString();
                                    string location_target = idr2["location_target"].ToString();

                                    decimal qty = Convert.ToDecimal(idr2["qty"]);//拨出
                                    decimal qty2 = Convert.ToDecimal(idr2["qty_int"]);//拨入
                                    string location = idr2["location"].ToString();

                                    //5A2-->6A1
                                    sql = @"
INSERT INTO WMS006A1(allocation_doc,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,warehouse,location,warehouse_target,location_target,qty,createby,createdate,createtime,qty_out)
values
('" + allocation_doc + "','" + serial_number + @"','" + material_no + @"','" + material_name + @"','" + material_var + "','" + material_specifications + @"','" + suppliers_lot + @"','" + lot_barcode + @"','" + warehouse + @"',
'" + location + @"',
'" + warehouse_target + @"',
'" + location_target + @"',
'" + qty2 + @"',
'" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + qty + "')";

                                    DB.ExecuteNonQueryOffline(sql);

                                    //sql += @"UPDATE WMS005A1 SET qty2=qty2+" + qty2 + " WHERE material_no='" + material_no + "' AND location_target='" + location_target + "' AND warehouse_target='" + warehouse_target + "' and allocation_plan='" + allocation_plan + "'";
                                    //DB.ExecuteNonQueryOffline(sql);

                                    sql = @"
 Update WMS005A2 set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "'where id='" + id + @"'";
                                    DB.ExecuteNonQueryOffline(sql);

                                    sql = @"SELECT * FROM WMS005A1 WHERE (qty_plan > qty or qty>qty2) and allocation_plan='" + allocation_plan + @"'";

                                    if (string.IsNullOrEmpty(DB.GetString(sql)))
                                    {
                                        sql = @"
 Update WMS005M set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' where allocation_plan='" + allocation_plan + @"'";
                                        DB.ExecuteNonQueryOffline(sql);
                                    }

                                    //status = "3";
                                    IsOk = true;
                                }
                            }

                            for (int K = 0; K < tmdt.Rows.Count; K++)
                            {

                                string warehouse_target = string.Empty;
                                switch (tmdt.Rows[K]["BarCodeType"].ToString().Trim())
                                {

                                    case "1":
                                        warehouse_target = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[K]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                        DataTable dt3 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org,location,warehouse FROM CODE001M where products_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'");
                                        sql = @"INSERT INTO CODE001A3 (products_barcode,documents_type,documents,serial_number,operation,warehouse,location,warehouse_target,location_target,qty,org) VALUES ('" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "','9','" + allocation_doc + "','','8','" + dt3.Rows[0]["warehouse"].ToString() + "','" + dt3.Rows[0]["location"].ToString() + "','" + warehouse_target + "','" + tmdt.Rows[K]["Location"].ToString().Trim() + "','" + tmdt.Rows[K]["Qty"].ToString().Trim() + "','" + dt3.Rows[0]["org"] + "')";
                                        DB.ExecuteNonQueryOffline(sql);


                                        //5根据条码类型更新单品库存数据
                                    //    sql = @"UPDATE WMS012A4 SET location='" + tmdt.Rows[K]["Locaiton"].ToString().Trim() + "' WHERE material_no='" + tmdt.Rows[K]["Material_no"].ToString().Trim() + "' AND products_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'";
                                        sql += @"UPDATE CODE001M SET location='" + tmdt.Rows[K]["Location"].ToString().Trim() + "',status='6' WHERE material_no='" + tmdt.Rows[K]["Material_no"].ToString().Trim() + "' AND products_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'";
                                        DB.ExecuteNonQueryOffline(sql);

                                        break;
                                    case "2":
                                        warehouse_target = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[K]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                        DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org,location,warehouse FROM CODE002M where lot_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'");
                                        sql = "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse,location,warehouse_target,location_target,qty,org) VALUES('" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "','9','" + allocation_doc + "','','8','" + dt31.Rows[0]["warehouse"].ToString() + "','" + dt31.Rows[0]["location"].ToString() + "','" + warehouse_target + "','" + tmdt.Rows[K]["Location"].ToString().Trim() + "','" + tmdt.Rows[K]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "')";
                                        DB.ExecuteNonQueryOffline(sql);

                                        //5根据条码类型更新批号库存数据
                                  //      sql = @"UPDATE WMS012A2 SET location='" + tmdt.Rows[K]["Locaiton"].ToString().Trim() + "' WHERE material_no='" + tmdt.Rows[K]["Material_no"].ToString().Trim() + "' AND lot_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'";
                                        sql += @"UPDATE CODE002M SET location='" + tmdt.Rows[K]["Location"].ToString().Trim() + "',status='6' WHERE material_no='" + tmdt.Rows[K]["Material_no"].ToString().Trim() + "' AND lot_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'";
                                        DB.ExecuteNonQueryOffline(sql);

                                        break;
                                    case "3":
                                        warehouse_target = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[K]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                        DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org,location,warehouse FROM CODE003M where packing_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'");
                                        sql = @"INSERT INTO CODE003A3 (packing_barcode,documents_type,documents,serial_number,operation,warehouse,location,warehouse_target,location_target,qty,org) VALUES ('" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "','9','" + allocation_doc + "','','8','" + dt32.Rows[0]["warehouse"].ToString() + "','" + dt32.Rows[0]["location"].ToString() + "','" + warehouse_target + "','" + tmdt.Rows[K]["Location"].ToString().Trim() + "','" + tmdt.Rows[K]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["org"] + "')";
                                        DB.ExecuteNonQueryOffline(sql);


                                        //5根据条码类型更新箱号库存数据
                                     //   sql = @"UPDATE WMS012A3 SET location='" + tmdt.Rows[K]["Locaiton"].ToString().Trim() + "' WHERE material_no='" + tmdt.Rows[K]["Material_no"].ToString().Trim() + "' AND packing_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'";
                                        sql += @"UPDATE CODE003M SET location='" + tmdt.Rows[K]["Location"].ToString().Trim() + "',status='6' WHERE material_no='" + tmdt.Rows[K]["Material_no"].ToString().Trim() + "' AND packing_barcode='" + tmdt.Rows[K]["BarCode"].ToString().Trim() + "'";
                                        DB.ExecuteNonQueryOffline(sql);

                                        break;
                                    default: break;
                                }

                            }

                            string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS006DoSure'");
                            if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                            {
                                string s = @"
<WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                    <IP4>" + IP4 + @"</IP4>
                <MAC>" + MAC + @"</MAC>
                
                <DBTYPE>" + DBTYPE + @"</DBTYPE> //数据库类型
                	<DBSERVER>" + DBSERVER + @"</DBSERVER> //数据库IP
                	<DBNAME>" + DBNAME + @"</DBNAME> //数据库名称
                	<DBUSER>" + DBUSER + @"</DBUSER> //数据库用户
                	<DBPASSWORD>" + DBPASSWORD + @"</DBPASSWORD> //数据库密码  
                <Data>
                <UserCode>" + UserCode + @"</UserCode>
                <allocation_doc>" + allocation_doc + @"</allocation_doc>
                </Data>
                
            </WebService>
";
                                ret = WMS006.Audit(OBJ, allocation_doc,1);//计划调拨
                                return ret;
                            }

                        }

                        IsSuccess = true;
                        RetData += "<" + allocation_plan + @">";
                        RetData += "<IsOk>" + IsOk + @"</IsOk>";
                        RetData += "<ErrMsg>" + ErrMsg + @"</ErrMsg>";
                        RetData += "</" + allocation_plan + @">";

                        #endregion
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

        public static string GETDATA(object OBJ)
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

                #region 接口参数
                string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");

                #endregion

                string where = string.Empty;

                if (allocation_plan.Contains(","))
                {
                    string[] tmpStr = allocation_plan.Split(',');
                    foreach (string s in tmpStr)
                    {
                        if (!string.IsNullOrEmpty(where))
                        {
                            where += ",";
                        }

                        where += "'" + s + @"'";
                    }
                }
                else
                {
                    where = "'" + allocation_plan + @"'";
                }



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 逻辑
                string sql = @"
SELECT *
FROM WMS005M(NOLOCK)
WHERE allocation_plan in (" + where + @")
";
                string sql1 = @"
SELECT *
FROM WMS005A1(NOLOCK)
WHERE allocation_plan in (" + where + @")
";
                string sql2 = @"
SELECT *
FROM WMS005A2(NOLOCK)
WHERE allocation_plan in (" + where + @")
";
                Dictionary<string, object> p = new Dictionary<string, object>();


                System.Data.DataTable dt = DB.GetDataTable(sql, p);
                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                System.Data.DataTable dt1 = DB.GetDataTable(sql1, p);
                string dtXML1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1);

                System.Data.DataTable dt2 = DB.GetDataTable(sql2, p);
                string dtXML2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);
                if (dt.Rows.Count > 0)
                {
                    IsSuccess = true;
                    RetData = "<WMS005M>" + dtXML + @"</WMS005M>" + "<WMS005A1>" + dtXML1 + @"</WMS005A1>" + "<WMS005A2>" + dtXML2 + @"</WMS005A2>";
                }

                else
                {
                    RetData = "不存在数据";
                }
                #endregion
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

        public static string DoOut(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string msg = string.Empty;
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

                #region 接口参数
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");
                if (allocation_plan == "null")
                {
                    return WMS006.QuickDoOut(OBJ);
                }
                string DT = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                #endregion

                string where = string.Empty;

                if (allocation_plan.Contains(","))
                {
                    string[] tmpStr = allocation_plan.Split(',');
                    foreach (string s in tmpStr)
                    {
                        if (!string.IsNullOrEmpty(where))
                        {
                            where += ",";
                        }

                        where += "'" + s + @"'";
                    }
                }
                else
                {
                    where = "'" + allocation_plan + @"'";
                }



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 逻辑
                // 1.检查调拨计划是否存在，不存在报错：调拨计划不存在。
                string sql = @"
SELECT *
FROM WMS005M
WHERE allocation_plan in (" + where + @")
";
                Dictionary<string, object> p = new Dictionary<string, object>();
                DataTable dt = DB.GetDataTable(sql, p);
                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DT);
                //2.根据扫描的条码数据进行汇总（按物料）拨出数量，获取WMS005A1和WMS005A2资料，汇可拨出数量（按物料，汇总WMS005A1的计划数量- 汇总WMS005A2实际数量），
                //如果扫描条码的物料拨出数量大于对应物料的可拨出数量，获取物料资料的MoveType,如果是不允许超入的话，
                //报错：拨出数量大于计划拨出数量（物料代号、物料名称、物料规格、计划数量、已拨出数量、当前拨出数量）。

                Dictionary<string, double> Nums = new Dictionary<string, double>();
                Dictionary<string, double> Nums1 = new Dictionary<string, double>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < tmdt.Rows.Count; i++)
                    {
                        if (Nums.Keys.ToString().Contains(tmdt.Rows[i]["Material_no"].ToString().Trim()))
                        {
                            Nums[tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCodeType"].ToString().Trim()] += Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim());
                        }
                        else
                        {
                            Nums.Add(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCodeType"].ToString().Trim(), Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim()));
                        }
                        if (Nums1.ContainsKey(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()))
                        {
                            Nums1[tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()] += Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim());
                        }
                        else
                        {
                            Nums1.Add(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim(), Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim()));
                        }
                    }
                    //kvp.Key;物料代号
                    //kvp.Value;入库数量

                    string MoveType = string.Empty;
                    string serial_number = string.Empty;
                    double qty1 = 0;//汇总WMS005A1的计划数量
                    double  qty2 = 0;//汇总WMS005A2实际数量
                    DataTable dt4;

                    

                    foreach (KeyValuePair<string, double> kvp in Nums)
                    {

                       

                        sql = @"SELECT material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,material_var,warehouse,location,org FROM WMS005A1 WHERE material_no='" + kvp.Key.Split('*')[0] + "'and allocation_plan='" + allocation_plan + "'";
                        DataTable dtA1 = DB.GetDataTable(sql);
                        if (dtA1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtA1.Rows.Count; i++)
                            {
                                qty1 += Convert.ToDouble(dtA1.Rows[i]["qty_plan"].ToString());
                            }
                        }
                        string sql1 = @"SELECT material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,warehouse,location,org FROM WMS005A2 WHERE material_no='" + kvp.Key.Split('*')[0] + "'and allocation_plan='" + allocation_plan + "'";
                        DataTable dtA2 = DB.GetDataTable(sql1);
                        string sql2 = @"SELECT material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,material_var,warehouse,location,location_target,org FROM WMS005A1 WHERE material_no='" + kvp.Key.Split('*')[0] + "'and allocation_plan='" + allocation_plan + "'";
                        DataTable dtA3 = DB.GetDataTable(sql2);
                        if (dtA2.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtA2.Rows.Count; i++)
                            {
                                qty2 += Convert.ToDouble(dtA2.Rows[i]["qty"].ToString());
                            }
                        }
                        if (kvp.Value <= (qty1 - qty2))
                        {
                            // 3.根据扫描条码数据进行汇总（按物料、拨出库位），把数据插入到WMS005A2中去，审核状态为1。
                            foreach (KeyValuePair<string, double > kvp1 in Nums1)
                            {
                                if (kvp.Key.Split('*')[0] == kvp1.Key.Split('*')[0].Trim())
                                {
                                    sql = string.Empty;
                                    Double qtyReal = 0;
                                    Double qtyReal2 = kvp.Value;

                                    DataTable dttmp = DB.GetDataTable(@"SELECT serial_number,qty_plan,qty,lot_barcode FROM WMS005A1(NOLOCK) WHERE allocation_plan='" + allocation_plan + @"' AND material_no='" + kvp.Key.Split('*')[0].Trim() + "' and location='" + kvp.Key.Split('*')[1].Trim() + @"' AND qty<qty_plan ORDER BY serial_number");

                                    for (int i = 0; i < dttmp.Rows.Count; i++)
                                    {
                                        if (qtyReal2 > 0)
                                        {
                                            string sn = dttmp.Rows[i][0].ToString();

                                            double i_qty = Convert.ToDouble(dttmp.Rows[i][2].ToString());
                                            double i_qty_plan = Convert.ToDouble(dttmp.Rows[i][1].ToString());
                                            string lot_barcode = dttmp.Rows[i][3].ToString();

                                            if (i == dttmp.Rows.Count - 1)
                                            {
                                                qtyReal = qtyReal2;
                                            }
                                            else
                                            {
                                                if (qtyReal2 > i_qty_plan - i_qty)
                                                {
                                                    qtyReal = i_qty_plan - i_qty;
                                                    qtyReal2 -= qtyReal;
                                                }
                                                else
                                                {
                                                    qtyReal = qtyReal2;
                                                    qtyReal2 -= qtyReal2;
                                                }
                                            }

                                            sql += @"
UPDATE WMS005A1 SET qty=Convert(decimal(18,2),qty+" + qtyReal + @"),modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
WHERE allocation_plan = '" + allocation_plan + @"' 
and material_no='" + kvp.Key.Split('*')[0] + @"'
and location='" + dtA3.Rows[0]["location"].ToString() + @"'
and location_target='" + dtA3.Rows[0]["location_target"].ToString() + @"'
and serial_number='"+sn+@"'";

                                            #region 产生调拨序号
                                            dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM WMS005A2 where allocation_plan='" + allocation_plan + "'");
                                            if (dt4.Rows[0][0].ToString() == "")
                                            {
                                                serial_number = "001";
                                            }
                                            else
                                            {
                                                serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                                            }
                                            #endregion

                                            string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + kvp1.Key.Split('*')[1] + "'").Rows[0][0].ToString();
                                            sql += @"
INSERT INTO WMS005A2 
(allocation_plan,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,qty_plan,warehouse,location,qty,org,status,createby,createdate,createtime) 
VALUES 
('" + allocation_plan + "','" + serial_number + "','" + kvp.Key.Split('*')[0] + "','" + dtA3.Rows[0]["material_name"].ToString() + "','" + dtA3.Rows[0]["material_var"].ToString() + "','" + dtA3.Rows[0]["material_specifications"].ToString() + "','" + dtA3.Rows[0]["suppliers_lot"].ToString() + "','" + lot_barcode + "','" +Math.Round( i_qty_plan,2) + "','" + dtA3.Rows[0]["warehouse"].ToString() + "','" + dtA3.Rows[0]["location"].ToString() + "','" + kvp.Value + "','" + dtA3.Rows[0]["org"].ToString() + "','1','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                                            DB.ExecuteNonQueryOffline(sql);
                                        }
                                    }


                                    

                                  

                                    sql = @"SELECT warehouse FROM BASE011M where location_no = '" + kvp1.Key.Split('*')[1].Trim() + "'";
                                    DataTable Base011m = DB.GetDataTable(sql);
                                    RetData += WMS012.DataEdit_diaobo(UserCode, kvp.Key.Split('*')[0].Trim(), "", "", "", kvp.Key.Split('*')[1], "", Base011m.Rows[0][0].ToString(), "", -1, Convert.ToDouble(kvp.Value), DB);//修改12A1表的数据
                                    if (kvp.Key.Split('*')[3] != "0")
                                    {
                                        RetData += WMS012.DataEdit_barcode(UserCode, kvp.Key.Split('*')[2], kvp.Key.Split('*')[0].Trim(), kvp.Key.Split('*')[1], "", Base011m.Rows[0][0].ToString(), "", -1, Convert.ToDouble(kvp.Value), DB, kvp.Key.Split('*')[3]);//修改12A2、3、4表数据
                                    }

                                }
                            }
                        }
                        else
                        {
                            //MoveType = "0";
                            MoveType = DB.GetDataTable("SELECT isover_out FROM BASE007M WHERE material_no='" + kvp.Key.Split('*')[0] + "'").Rows[0][0].ToString().Trim();
                            if (MoveType != "TRUE")//如果是不允许超出  报错:拨出数量大于计划拨出数量（物料代号、物料名称、物料规格、计划数量、已拨出数量、当前拨出数量）。
                            {
                                RetData = "拨出数量大于计划拨出数量[" + kvp.Key.Split('*')[0] + "," + dtA1.Rows[0]["material_name"].ToString() + "," + dtA1.Rows[0]["material_specifications"].ToString() + "," + qty1 + "," + qty2 + "," + kvp.Value + "]";
                            }
                            else
                            {
                                // 3.根据扫描条码数据进行汇总（按物料、拨出库位），把数据插入到WMS005A2中去，审核状态为1。
                                foreach (KeyValuePair<string, double> kvp1 in Nums1)
                                {
                                    if (kvp.Key.Split('*')[0] == kvp1.Key.Split('*')[0].Trim())
                                    {
                                        sql = string.Empty;

                                        Double qtyReal = 0;
                                        Double qtyReal2 = kvp.Value;

                                        DataTable dttmp = DB.GetDataTable(@"SELECT serial_number,qty_plan,qty,lot_barcode FROM WMS005A1(NOLOCK) WHERE allocation_plan='" + allocation_plan + @"' AND material_no='" + kvp.Key.Split('*')[0].Trim() + "' and location='" + kvp.Key.Split('*')[1].Trim() + @"' AND qty<qty_plan ORDER BY serial_number");

                                        for (int i = 0; i < dttmp.Rows.Count; i++)
                                        {
                                            if (qtyReal2 > 0)
                                            {
                                                string sn = dttmp.Rows[i][0].ToString();

                                                double i_qty = Convert.ToDouble(dttmp.Rows[i][2].ToString());
                                                double i_qty_plan = Convert.ToDouble(dttmp.Rows[i][1].ToString());
                                                string lot_barcode = dttmp.Rows[i][3].ToString();

                                                if (i == dttmp.Rows.Count - 1)
                                                {
                                                    qtyReal = qtyReal2;
                                                }
                                                else
                                                {
                                                    if (qtyReal2 > i_qty_plan - i_qty)
                                                    {
                                                        qtyReal = i_qty_plan - i_qty;
                                                        qtyReal2 -= qtyReal;
                                                    }
                                                    else
                                                    {
                                                        qtyReal = qtyReal2;
                                                        qtyReal2 -= qtyReal2;
                                                    }
                                                }

                                                sql += @"
UPDATE WMS005A1 SET qty=Convert(decimal(18,2),qty+" + qtyReal + @"),modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
WHERE allocation_plan = '" + allocation_plan + @"' 
and material_no='" + kvp.Key.Split('*')[0] + @"'
and location='" + dtA3.Rows[0]["location"].ToString() + @"'
and location_target='" + dtA3.Rows[0]["location_target"].ToString() + @"'
and serial_number='" + sn + @"'";

                                                #region 产生调拨序号
                                                dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM WMS005A2 where allocation_plan='" + allocation_plan + "'");
                                                if (dt4.Rows[0][0].ToString() == "")
                                                {
                                                    serial_number = "001";
                                                }
                                                else
                                                {
                                                    serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                                                }
                                                #endregion

                                                string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + kvp1.Key.Split('*')[1] + "'").Rows[0][0].ToString();
                                                sql += @"
INSERT INTO WMS005A2 
(allocation_plan,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,qty_plan,warehouse,location,qty,org,status,createby,createdate,createtime) 
VALUES 
('" + allocation_plan + "','" + serial_number + "','" + kvp.Key.Split('*')[0] + "','" + dtA3.Rows[0]["material_name"].ToString() + "','" + dtA3.Rows[0]["material_var"].ToString() + "','" + dtA3.Rows[0]["material_specifications"].ToString() + "','" + dtA3.Rows[0]["suppliers_lot"].ToString() + "','" + lot_barcode + "','" +Math.Round( i_qty_plan,2) + "','" + dtA3.Rows[0]["warehouse"].ToString() + "','" + dtA3.Rows[0]["location"].ToString() + "','" + kvp.Value + "','" + dtA3.Rows[0]["org"].ToString() + "','1','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                                                DB.ExecuteNonQueryOffline(sql);
                                            }
                                        }



                                       

                                        sql = @"SELECT warehouse FROM BASE011M where location_no = '" + kvp.Key.Split('*')[1].Trim() + "'";
                                        DataTable Base011m = DB.GetDataTable(sql);
                                        RetData += WMS012.DataEdit_diaobo(UserCode, kvp.Key.Split('*')[0].Trim(), "", "", "", kvp.Key.Split('*')[1], "", Base011m.Rows[0][0].ToString(), "", -1, Convert.ToDouble(kvp.Value), DB);//修改12A1表的数据
                                        if (kvp.Key.Split('*')[3] != "0")
                                        {
                                            RetData += WMS012.DataEdit_barcode(UserCode, kvp.Key.Split('*')[2], kvp.Key.Split('*')[0].Trim(), kvp.Key.Split('*')[1], "", Base011m.Rows[0][0].ToString(), "", -1, Convert.ToDouble(kvp.Value), DB, kvp.Key.Split('*')[3]);//修改12A2、3、4表数据
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //4.把扫描条码，根据条码的类型，分别插入到 CODE001A1(单品条码操作记录)、CODE001A3(单品条码单据记录)、CODE002A1(批号条码操作记录)、CODE002A3(批号条码单据操作记录)、CODE003A1(包装条码操作记录)、CODE003A3(包装条码单据记录)中。
                    //   0是品号 Base007m，1是单品code001m，2是批号code002m，3是包装code003m
                    for (int i = 0; i < tmdt.Rows.Count; i++)
                    {

                        switch (tmdt.Rows[i]["BarCodeType"].ToString().Trim())
                        {

                            case "1":
                                string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                DataTable dt3 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE001M where products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                                sql = @"INSERT INTO CODE001A1 (products_barcode,operation,warehouse,location,packing_barcode,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','7','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + dt3.Rows[0]["packing_barcode"] + "','" + dt3.Rows[0]["lot_barcode"] + "','" + dt3.Rows[0]["org"] + "')";
                                sql += @"INSERT INTO CODE001A3 (products_barcode,documents_type,documents,serial_number,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + allocation_plan + "','" + serial_number + "','7','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt3.Rows[0]["org"] + "')";
                                sql += @"UPDATE CODE001M SET status='7' WHERE products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                                DB.ExecuteNonQueryOffline(sql);
                                break;
                            case "2":
                                string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                System.Data.DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                                sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','7','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "') ";
                                sql += "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse,location,qty,org) VALUES('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + allocation_plan + "','" + serial_number + "','7','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "')";
                                sql += @"UPDATE CODE002M SET status='7' WHERE lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                                DB.ExecuteNonQueryOffline(sql);
                                break;
                            case "3":
                                string c = "SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'";
                                string warehouse2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                System.Data.DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE003M where packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                                sql = @"INSERT INTO CODE003A1 (packing_barcode,operation,warehouse,location,qty,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','7','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["lot_barcode"] + "','" + dt32.Rows[0]["org"] + "') ";
                                sql += @"INSERT INTO CODE003A3 (packing_barcode	,documents_type,documents,serial_number,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + allocation_plan + "','" + serial_number + "','7','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["org"] + "')";
                                sql += @"UPDATE CODE003M SET status='7' WHERE packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                                DB.ExecuteNonQueryOffline(sql);
                                break;
                            default: break;
                        }

                    }

                    IsSuccess = true;

                    //string Auto = DB.GetDataTable("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS003TOWMS004'").Rows[0][0].ToString();
                    //if (Auto == "Auto")
                    //{
                    //    //DoSure(OBJ);
                    //}
                }

                else
                {
                    RetData = "调拨计划不存在";
                }
                #endregion
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
                    < RetData>" + RetData + "</RetData>" + @"
                </Return>
            </WebService>
            ";

            return ret;
        }

        public static string DoInOLD(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string msg = string.Empty;
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

                #region 接口参数
                string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");

                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                #endregion

                string where = string.Empty;

                if (allocation_plan.Contains(","))
                {
                    string[] tmpStr = allocation_plan.Split(',');
                    foreach (string s in tmpStr)
                    {
                        if (!string.IsNullOrEmpty(where))
                        {
                            where += ",";
                        }

                        where += "'" + s + @"'";
                    }
                }
                else
                {
                    where = "'" + allocation_plan + @"'";
                }

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 逻辑

                System.Data.DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);

                string sql = string.Empty;

                for (int i = 0; i < tmdt.Rows.Count; i++)
                {

                    string warehouse_target = string.Empty;
                    string location = string.Empty;
                    switch (tmdt.Rows[i]["BarCodeType"].ToString().Trim())
                    {

                        case "1":
                            warehouse_target = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + "'").Rows[0][0].ToString();
                            location = DB.GetString("SELECT location from CODE001A3 WHERE documents_type='8' and documents='" + allocation_plan + @"' and products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'");
                            System.Data.DataTable dt3 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org,material_no FROM CODE001M where products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE001A1 (products_barcode,operation,warehouse,location,packing_barcode,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + warehouse_target + "','" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + "','" + dt3.Rows[0]["packing_barcode"] + "','" + dt3.Rows[0]["lot_barcode"] + "','" + dt3.Rows[0]["org"] + "')";
                            sql += @"UPDATE CODE001A3 SET location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"',warehouse_target='" + warehouse_target + @"' WHERE documents_type='8' and documents='" + allocation_plan + @"' and products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            sql += @"UPDATE CODE001M SET status='8' WHERE products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";

                            sql += @"
UPDATE WMS005A2 SET location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"',
warehouse_target='" + warehouse_target + @"'
WHERE material_no = '" + dt3.Rows[0]["material_no"].ToString() + @"' 
AND allocation_plan='" + allocation_plan + @"'
AND location ='" + location + @"'";

                            sql += @"
UPDATE WMS005A1 SET qty2=qty2+" + tmdt.Rows[i]["Qty"].ToString().Trim() + @"
WHERE material_no = '" + dt3.Rows[0]["material_no"].ToString() + @"' 
AND allocation_plan='" + allocation_plan + @"'
AND location ='" + location + @"'
AND location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"'";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        case "2":
                            warehouse_target = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + "'").Rows[0][0].ToString();
                            location = DB.GetString("SELECT location from CODE001A3 WHERE documents_type='8' and documents='" + allocation_plan + @"' and lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'");
                            System.Data.DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org,material_no FROM CODE002M where lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + warehouse_target + "','" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "') ";
                            sql += @"UPDATE CODE002A3 SET location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"',warehouse_target='" + warehouse_target + @"' WHERE documents_type='8' and documents='" + allocation_plan + @"' and lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            sql += @"UPDATE CODE002M SET status='8' WHERE lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            sql += @"
UPDATE WMS005A2 SET location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"',
warehouse_target='" + warehouse_target + @"'
WHERE material_no = '" + dt31.Rows[0]["material_no"].ToString() + @"' 
AND allocation_plan='" + allocation_plan + @"'
AND location ='" + location + @"'";
                            sql += @"
UPDATE WMS005A1 SET qty2=qty2+" + tmdt.Rows[i]["Qty"].ToString().Trim() + @"
WHERE material_no = '" + dt31.Rows[0]["material_no"].ToString() + @"' 
AND allocation_plan='" + allocation_plan + @"'
AND location ='" + location + @"'
AND location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"'";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        case "3":
                            warehouse_target = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + "'").Rows[0][0].ToString();
                            location = DB.GetString("SELECT location from CODE003A3 WHERE documents_type='8' and documents='" + allocation_plan + @"' and packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'");
                            System.Data.DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org,material_no FROM CODE003M where packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE003A1 (packing_barcode,operation,warehouse,location,qty,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + warehouse_target + "','" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["lot_barcode"] + "','" + dt32.Rows[0]["org"] + "') ";
                            sql += @"UPDATE CODE003A3 SET location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"',warehouse_target='" + warehouse_target + @"' WHERE documents_type='8' and documents='" + allocation_plan + @"' and packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            sql += @"UPDATE CODE003M SET status='8' WHERE packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            sql += @"
UPDATE WMS005A2 SET location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"',
warehouse_target='" + warehouse_target + @"',
qty_int='" + tmdt.Rows[i]["Qty"].ToString().Trim() + "' WHERE material_no = '" + dt32.Rows[0]["material_no"].ToString() + @"' 
AND allocation_plan='" + allocation_plan + @"'
AND location ='" + location + @"'";
                            sql += @"
UPDATE WMS005A1 SET qty2=qty2+" + tmdt.Rows[i]["Qty"].ToString().Trim() + @", location_target='" + tmdt.Rows[i]["Locaiton"].ToString().Trim() + @"'
WHERE material_no = '" + dt32.Rows[0]["material_no"].ToString() + @"' 
AND allocation_plan='" + allocation_plan + @"'
AND location ='" + location + @"'";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        default: break;
                    }
                }

                string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS005TOWMS006'");
                if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                {
                    ret = Audit(OBJ);
                    return ret;
                }

                #endregion
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
                    <allocation_doc>" + RetData + "</allocation _doc>" + msg + @"
                </Return>
            </WebService>
            ";

            return ret;
        }

        public static string CheckDoIn(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string msg = string.Empty;
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

                #region 接口参数
                string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");

                string barcode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode>", "</barcode>");
                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                string sql = @"
SELECT * FROM CODE001A3 WHERE products_barcode='" + barcode + @"'
and documents_type='8' and documents='" + allocation_plan + @"'
and ISNULL(location_target,'')='' 
";
                if (!string.IsNullOrEmpty(DB.GetString(sql)))
                {
                    IsSuccess = true;
                }
                else
                {
                    sql = @"
SELECT * FROM CODE002A3 WHERE lot_barcode='" + barcode + @"'
and documents_type='8' and documents='" + allocation_plan + @"'
and ISNULL(location_target,'')='' 
";
                    if (!string.IsNullOrEmpty(DB.GetString(sql)))
                    {
                        IsSuccess = true;
                    }
                    else
                    {
                        sql = @"
SELECT * FROM CODE003A3 WHERE packing_barcode='" + barcode + @"'
and documents_type='8' and documents='" + allocation_plan + @"'
and ISNULL(location_target,'')='' 
";
                        if (!string.IsNullOrEmpty(DB.GetString(sql)))
                        {
                            IsSuccess = true;
                        }
                        else
                        {

                            sql = @"
SELECT * FROM WMS005A2 WHERE material_no='" + barcode + @"'
and allocation_plan='" + allocation_plan + @"'
and ISNULL(location_target,'')='' 
";
                            if (!string.IsNullOrEmpty(DB.GetString(sql)))
                            {
                                IsSuccess = true;
                            }

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
                    < RetData>" + RetData + "</RetData>" + @"
                </Return>
            </WebService>
            ";

            return ret;
        }

        public static string DoIn(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string msg1 = string.Empty;
            string msg2 = string.Empty;
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

                #region 接口参数
                string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");
                if (allocation_plan == "null")//快速入库
                {
                    return WMS006.QuickDoIn(OBJ);
                }
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                #endregion

                string where = string.Empty;

                string sql = string.Empty;

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);

                #region 逻辑

                Dictionary<string, double > Nums = new Dictionary<string, double>();
                for (int i = 0; i < tmdt.Rows.Count; i++)
                {
                    if (Nums.ContainsKey(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()))
                    {
                        Nums[tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()] += Convert.ToDouble (tmdt.Rows[i]["Qty"].ToString().Trim());
                    }
                    else
                    {
                        Nums.Add(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim(), Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim()));
                    }
                }
                foreach (KeyValuePair<string, double> kvp in Nums)
                {
                    //if (kvp.Key.Trim() == kvp.Key.Split('*')[0].Trim())  
                    //{
                    //2更新WMS005A2的字段

                    sql = string.Empty;
                    Double qtyReal = 0;
                    Double qtyReal2 = kvp.Value;

                    DataTable dttmp = DB.GetDataTable(@"SELECT serial_number,qty,qty2,lot_barcode FROM WMS005A1(NOLOCK) WHERE allocation_plan='" + allocation_plan + @"' AND material_no='" + kvp.Key.Split('*')[0].Trim() + "' AND qty2<qty ORDER BY serial_number");

                    for (int i = 0; i < dttmp.Rows.Count; i++)
                    {
                        if (qtyReal2 > 0)
                        {
                            string sn = dttmp.Rows[i][0].ToString();

                            double i_qty = Convert.ToDouble(dttmp.Rows[i][2].ToString());
                            double i_qty_plan = Convert.ToDouble(dttmp.Rows[i][1].ToString());
                            string lot_barcode = dttmp.Rows[i][3].ToString();

                            if (i == dttmp.Rows.Count - 1)
                            {
                                qtyReal = qtyReal2;
                            }
                            else
                            {
                                if (qtyReal2 > i_qty_plan - i_qty)
                                {
                                    qtyReal = i_qty_plan - i_qty;
                                    qtyReal2 -= qtyReal;
                                }
                                else
                                {
                                    qtyReal = qtyReal2;
                                    qtyReal2 -= qtyReal2;
                                }
                            }

                            sql = @"
UPDATE WMS005A1 SET qty2=Convert(decimal(18,2),qty+" + qtyReal + @"),modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
WHERE allocation_plan = '" + allocation_plan + @"' 
and serial_number='" + sn + @"'";
                            DB.ExecuteNonQueryOffline(sql);

                        }
                    }


                    sql = string.Empty;
                    qtyReal = 0;
                    qtyReal2 = kvp.Value;

                    dttmp = DB.GetDataTable(@"SELECT serial_number,qty,isnull(qty_int,0),lot_barcode FROM WMS005A2(NOLOCK) WHERE allocation_plan='" + allocation_plan + @"' AND material_no='" + kvp.Key.Split('*')[0].Trim() + "' AND isnull(qty_int,0)<qty ORDER BY serial_number");

                    for (int i = 0; i < dttmp.Rows.Count; i++)
                    {
                        if (qtyReal2 > 0)
                        {
                            string sn = dttmp.Rows[i][0].ToString();

                            double i_qty = Convert.ToDouble(dttmp.Rows[i][2].ToString());
                            double i_qty_plan = Convert.ToDouble(dttmp.Rows[i][1].ToString());
                            string lot_barcode = dttmp.Rows[i][3].ToString();

                            if (i == dttmp.Rows.Count - 1)
                            {
                                qtyReal = qtyReal2;
                            }
                            else
                            {
                                if (qtyReal2 > i_qty_plan - i_qty)
                                {
                                    qtyReal = i_qty_plan - i_qty;
                                    qtyReal2 -= qtyReal;
                                }
                                else
                                {
                                    qtyReal = qtyReal2;
                                    qtyReal2 -= qtyReal2;
                                }
                            }

                            sql = @"
UPDATE WMS005A2 SET qty_int=Convert(decimal(18,2),isnull(qty_int,0)+" + qtyReal + @"),modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
WHERE allocation_plan = '" + allocation_plan + @"' 
and serial_number='" + sn + @"'";
                            DB.ExecuteNonQueryOffline(sql);

                        }
                    }




                    //                    string warehouse_target = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + kvp.Key.Split('*')[1].Trim() + "'").Rows[0][0].ToString();
                    //                    sql = @"UPDATE WMS005A2 SET qty_int=" + kvp.Value + ", location_target='" + kvp.Key.Split('*')[1].Trim() + "',warehouse_target='" + warehouse_target.ToString().Trim() + "',modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"' where allocation_plan='" + allocation_plan.ToString().Trim() + "' AND material_no='" + kvp.Key.Split('*')[0].Trim() + "' ";

                    //                    sql += @"
                    //UPDATE WMS005A1 SET qty2=isnull(qty2,0)+" + kvp.Value + @", location_target='" + kvp.Key.Split('*')[1].Trim() + @"',modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
                    // WHERE material_no = '" + kvp.Key.Split('*')[0].Trim() + @"' 
                    //AND allocation_plan='" + allocation_plan + @"'";


                }
                
                string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS005TOWMS006'");
                if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                {
                    ret = Audit(OBJ);
                    return ret;
                }
                #endregion
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
                    <allocation_doc>" + RetData + "</allocation _doc>" + msg1 + @"
                </Return>
            </WebService>
            ";

            return ret;
        }
    }
}
