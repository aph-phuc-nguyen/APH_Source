using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class WMS013
    {
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
 WMS013(盘点单)接口帮助 @END
 方法：Help(帮助),Audit(审核) @END
 @END
====================Audit方法调用=================== @END
 Data参数如下
 <Data> @END
 @END
 
 <moveout_doc></moveout_doc>调拨单 @END
 @END
 </Data> @END
 @END
 返回数据如下 @END
 <Return> @END
 <IsSuccess>true/false</IsSuccess> @END
 <RetData> @END
 如果成功：<IsOk>True/False</IsOk> @END
 如果失败：失败报错 @END
 </RetData> @END
 </Return> @END
 ================================================== @END
@END
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
                string inventory_form = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<inventory_form>", "</inventory_form>");

                #endregion

                string where = string.Empty;

                if (inventory_form.Contains(","))
                {
                    string[] tmpStr = inventory_form.Split(',');
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
                    where = "'" + inventory_form + @"'";
                }



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                #region 逻辑
                string sql = @"
SELECT *
FROM WMS013M
WHERE inventory_form in (" + where + @")
";
                string sql1 = @"
SELECT *
FROM WMS013A1
WHERE inventory_form in (" + where + @")
";
                string sql2 = @"
SELECT *
FROM WMS013A2
WHERE inventory_form in (" + where + @")
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
                    RetData = "<WMS013M>" + dtXML + @"</WMS013M>" + "<WMS013A1>" + dtXML1 + @"</WMS013A1>" + "<WMS013A2>" + dtXML2 + @"</WMS013A2>";
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

        public static string GETDATAByCode(object OBJ)
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
                string inventory_form = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<inventory_form>", "</inventory_form>");
                string code = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<code>", "</code>");

                #endregion

                string where = string.Empty;




                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                #region 逻辑
                string sql = @"
SELECT *
FROM WMS013A3
WHERE inventory_form='" + inventory_form + @"' AND code='" + code + @"'
";
                Dictionary<string, object> p = new Dictionary<string, object>();


                System.Data.DataTable dt = DB.GetDataTable(sql, p);
                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);


                if (dt.Rows.Count > 0)
                {
                    IsSuccess = true;
                    RetData = "<WMS013A3>" + dtXML + @"</WMS013A3>";
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

        public static string GetBarCode(object OBJ)
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
                string inventory_form = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<inventory_form>", "</inventory_form>");
                string BarCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");

                #endregion

                string where = string.Empty;



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                #region 逻辑

                string sql = @"
SELECT *
FROM WMS013A4
WHERE inventory_form ='" + inventory_form + @"' and code='" + BarCode + @"'
";
                Dictionary<string, object> p = new Dictionary<string, object>();



                System.Data.DataTable dt = DB.GetDataTable(sql, p);
                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                if (dt.Rows.Count > 0)
                {
                    IsSuccess = true;
                    RetData = "<WMS013A3>" + dtXML + @"</WMS013A3>";
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
        public static string DoWork(object OBJ)
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
                string inventory_form = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<inventory_form>", "</inventory_form>");
                string location = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<location>", "</location>");
                string material_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<material_no>", "</material_no>");
                string barcode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode>", "</barcode>");
                string barcodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcodeType>", "</barcodeType>");
                string qty = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<qty>", "</qty>");
                string size= GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Size>", "</Size>");

                #endregion


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);


                string sql = string.Empty;

                string lot_barcode = string.Empty;


                lot_barcode = DB.GetString("Select lot_barcode from CODE002M WHERE UDF01='" + size + @"' AND UDF02='" + barcode + @"'");
                

                #region 逻辑
                //1.查询WMS013A3看条码是否之前已经有进行扫描，如果已有记录，报错：条码已经在（库位）进行了盘点，不能重复盘点
                sql = @"
SELECT location 
FROM WMS013A3
WHERE code ='" + barcode + "' and inventory_form='" + inventory_form + "' and UDF01='"+size+@"'";

                string material_name = DB.GetDataTable("SELECT material_name FROM BASE007M where material_no='" + material_no + "'").Rows[0][0].ToString();
                string material_specifications = DB.GetDataTable("SELECT material_specifications FROM BASE007M where material_no='" + material_no + "'").Rows[0][0].ToString();
                string material_var = DB.GetDataTable("SELECT material_ver FROM BASE007M where material_no='" + material_no + "'").Rows[0][0].ToString();

                string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + location + "'").Rows[0][0].ToString();

                Dictionary<string, object> p = new Dictionary<string, object>();
                System.Data.DataTable dt = DB.GetDataTable(sql, p);

                //没有盘点过或者是品号、批号
                if (dt.Rows.Count == 0 || barcodeType == "0" || barcodeType == "2")
                {

                    //2.判断传入的库位资料是否在盘点单的盘点范围内（WMS013M）的起始库位和结束库位范围内，如果不是，报错：盘点单中不需要盘点（库位），请重新扫描A-01-01-01，A-21-04-10
                    string location1 = DB.GetString("SELECT location1 FROM WMS013M WHERE inventory_form='" + inventory_form + "'");
                    string location2 = DB.GetString("SELECT location2 FROM WMS013M WHERE inventory_form='" + inventory_form + "'");


                    if (!string.IsNullOrEmpty(DB.GetString("select location_no from BASE011M where location_no='" + location + "' and location_no between '" + location1 + "' and '" + location2 + "'")))
                    {



                        //3.把条码信息插入WMS013A3中，barcodeType=0时不执行
                        if (barcodeType != "0")
                        {
                            sql = @"INSERT INTO WMS013A3 
                             (inventory_form,code,codetype,material_name,material_specifications,material_no,material_var,qty,warehouse,location,UDF01) VALUES
                             ('" + inventory_form + "','" + barcode + "','" + barcodeType + "','" + material_name + "','" + material_specifications + "','" + material_no + "','" + material_var + "','" + qty + "','" + warehouse + "','" + location + "','"+size+@"')";
                            DB.ExecuteNonQueryOffline(sql);
                        }

                        //4.更新WMS013A1的盘点记录，（如果已有同一库位同一物料的记录，更新加上新扫描的数量。如果没有记录，新增一条记录）


                        sql = "UPDATE WMS013A1 SET qty =qty + '" + qty + "'  WHERE material_no = '" + material_no + "' AND location = '" + location + "' and inventory_form = '" + inventory_form + "' AND UDF01='"+size+@"'";
                        if (DB.ExecuteNonQueryOffline(sql) == 0)
                        {
                            sql = "INSERT INTO WMS013A1 (warehouse,location,material_no,material_name,material_specifications,qty,inventory_form,UDF01) VALUES ('" + warehouse + "','" + location + "','" + material_no + "','" + material_name + "','" + material_specifications + "','" + qty + "','" + inventory_form + "','"+size+@"')";
                            DB.ExecuteNonQueryOffline(sql);
                        }

                        sql = "UPDATE WMS013A4 SET qty =qty + '" + qty + "'  WHERE material_no = '" + material_no + "' AND location = '" + location + "' and inventory_form = '" + inventory_form + "' and lot_barcode='" + lot_barcode + @"' and UDF01='"+size+@"'";
                        if (DB.ExecuteNonQueryOffline(sql) == 0)
                        {
                            sql = "INSERT INTO WMS013A4 (warehouse,location,material_no,material_name,material_specifications,qty,inventory_form,lot_barcode,UDF01) VALUES ('" + warehouse + "','" + location + "','" + material_no + "','" + material_name + "','" + material_specifications + "','" + qty + "','" + inventory_form + "','" + lot_barcode + @"','"+size+@"')";
                            DB.ExecuteNonQueryOffline(sql);
                        }



                        //5.更新WMS013A2的盘点差异，（如果已有同一库位同一物料的记录，盘点数量更新加上新扫描的数量。如果没有记录，新增一条记录，并计算差异数量）

                        //更新原库位差异
                        double qty_difference = 0;
                        double qty_original = 0;

                        #region 更新原库位差异
                        //                        if (barcodeType != "0" & barcodeType != "2")
                        //                        {
                        //                            string barcode_location = string.Empty;
                        //                            if (barcodeType == "1")
                        //                            {
                        //                                barcode_location = DB.GetString("SELECT location FROM CODE001M(NOLOCK) where products_barcode='" + barcode + @"'");
                        //                            }
                        //                            else if (barcodeType == "3")
                        //                            {
                        //                                barcode_location = DB.GetString("SELECT location FROM CODE003M(NOLOCK) where packing_barcode='" + barcode + @"'");
                        //                            }

                        //                            string barcode_warehouse = DB.GetString("SELECT warehouse FROM BASE011M(NOLOCK) where location_no='" + barcode_location + @"'");

                        //                            if (barcode_location != location)
                        //                            {

                        //                                if (string.IsNullOrEmpty(DB.GetString("SELECT * FROM WMS013A2(NOLOCK) WHERE material_no='" + material_no + @"' AND location='" + barcode_location + @"' AND inventory_form='"+ inventory_form+@"'")))
                        //                                {
                        //                                    sql = "SELECT ISNULL(qty,0) FROM WMS012A1 WHERE material_no ='" + material_no + "' AND location='" + barcode_location + "'";

                        //                                    qty_original = DB.GetDouble(sql);

                        //                                    sql = @"
                        //INSERT INTO WMS013A2 
                        //(warehouse,location,material_no,material_name,material_specifications,qty,qty_original,qty_difference,inventory_form) 
                        //values 
                        //('" + barcode_warehouse + "','" + barcode_location + "','" + material_no + "','" + material_name + "','" + material_specifications + @"',
                        //'0','" + qty_original + "','-" + qty_original + "','" + inventory_form + "')";
                        //                                    DB.ExecuteNonQueryOffline(sql);
                        //                                }
                        //                            }
                        //                        } 
                        #endregion

                        ///更新目标库位差异
                        sql = "SELECT ISNULL(qty,0) FROM WMS012A1 WHERE material_no ='" + material_no + "' AND location='" + location + "' AND UDF01='"+size+@"'";

                        qty_original = DB.GetDouble(sql);

                        sql = "SELECT * FROM WMS013A2 WHERE location='" + location + "' and material_no='" + material_no + "' and inventory_form='" + inventory_form + "' AND UDF01='"+size+@"'";
                        System.Data.DataTable dt3 = DB.GetDataTable(sql, p);
                        if (dt3.Rows.Count > 0)
                        {
                            if (barcodeType == "0")
                            {
                                qty_difference = Convert.ToDouble(qty) - qty_original;

                                sql = "UPDATE WMS013A2 SET qty = '" + qty + "',qty_difference='" + qty_difference + "' WHERE material_no='" + material_no + "' AND location='" + location + "' and inventory_form='" + inventory_form + "' AND UDF01='"+size+@"'";

                                DB.ExecuteNonQueryOffline(sql);

                            }
                            else
                            {
                                sql = "select ISNULL(SUM(qty),0) qty from WMS013A3 where material_no='" + material_no + "' AND location='" + location + "' and inventory_form='" + inventory_form + "' AND UDF01='"+size+@"' group by location";
                                double WMS013A1qty = DB.GetDouble(sql);

                                qty_difference = WMS013A1qty - qty_original;

                                sql = "UPDATE WMS013A2 SET qty = '" + WMS013A1qty + "',qty_difference='" + qty_difference + "' WHERE material_no='" + material_no + "' AND location='" + location + "' and inventory_form='" + inventory_form + "' AND UDF01='"+size+@"'";

                                DB.ExecuteNonQueryOffline(sql);


                                sql = "select ISNULL(qty_original,0) FROM WMS013A5 where material_no='" + material_no + "' AND location='" + location + "' and inventory_form='" + inventory_form + "' and lot_barcode='" + lot_barcode + @"' AND UDF01='"+size+@"'";

                                qty_original = DB.GetDouble(sql);

                                sql = @"
select 
ISNULL(SUM(qty),0) qty
from WMS013A3 
where material_no='" + material_no + "' AND location='" + location + "' and inventory_form='" + inventory_form + @"' AND UDF01='"+size+@"'
AND
(CASE WHEN codetype = '2' THEN code WHEN codetype = '3' THEN(SELECT lot_barcode FROM CODE003M where packing_barcode = code) WHEN codetype = '1' THEN(select lot_barcode FROM CODE001M WHERE products_barcode = code) ELSE '' END) = '" + lot_barcode + @"'
";
                                double WMS013A4qty = DB.GetDouble(sql);

                                qty_difference = WMS013A4qty - qty_original;

                                sql = "UPDATE WMS013A5 SET qty =  '" + WMS013A4qty + "',qty_difference='" + qty_difference + "' WHERE material_no='" + material_no + "' AND location='" + location + "' and inventory_form='" + inventory_form + "' and lot_barcode='" + lot_barcode + @"' AND UDF01='"+size+@"'";

                                if (DB.ExecuteNonQueryOffline(sql) == 0)
                                {
                                    sql = " INSERT INTO WMS013A5 (warehouse,location,material_no,material_name,material_specifications,qty,qty_original,qty_difference,inventory_form,lot_barcode,UDF01) values ('" + warehouse + "','" + location + "','" + material_no + "','" + material_name + "','" + material_specifications + "','" + qty + "','" + qty_original + "','" + qty_difference + "','" + inventory_form + "','" + lot_barcode + @"','"+size+@"')";
                                    DB.ExecuteNonQueryOffline(sql);
                                }
                            }


                        }
                        else
                        {
                            qty_difference = double.Parse(qty) - qty_original;

                            sql = " INSERT INTO WMS013A2 (warehouse,location,material_no,material_name,material_specifications,qty,qty_original,qty_difference,inventory_form,UDF01) values ('" + warehouse + "','" + location + "','" + material_no + "','" + material_name + "','" + material_specifications + "','" + qty + "','" + qty_original + "','" + qty_difference + "','" + inventory_form + "','"+size+@"')";
                            DB.ExecuteNonQueryOffline(sql);


                            sql = "select ISNULL(qty_original,0) FROM WMS013A5 where material_no='" + material_no + "' AND location='" + location + "' and inventory_form='" + inventory_form + "' and lot_barcode='" + lot_barcode + @"' and inventory_form='" + inventory_form + "' AND UDF01='"+size+@"'";

                            qty_original = DB.GetDouble(sql);
                            qty_difference = double.Parse(qty) - qty_original;

                            sql = " INSERT INTO WMS013A5 (warehouse,location,material_no,material_name,material_specifications,qty,qty_original,qty_difference,inventory_form,lot_barcode,UDF01) values ('" + warehouse + "','" + location + "','" + material_no + "','" + material_name + "','" + material_specifications + "','" + qty + "','" + qty_original + "','" + qty_difference + "','" + inventory_form + "','" + lot_barcode + @"','"+size+@"')";
                            DB.ExecuteNonQueryOffline(sql);
                        }
                        IsSuccess = true;
                    }
                    else
                    {
                        RetData = "盘点单中不需要盘点（" + location + @"），请重新扫描";
                    }


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
                <IsSuccess>" + IsSuccess + @"</IsSuccess> 
                <Return> 
                    " + RetData + @"
                </Return>
            </WebService>
            ";

            return ret;
        }


        /// <summary>
        /// Audit(审核)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
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

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string inventory_form = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<inventory_form>", "</inventory_form>");//入库单
                #endregion

                #region 逻辑
                // 1.	请求User.DoSureCheck(User,WMS004)方法检查用户是否有权限进行审核，如果没有权限，报错该用户没有权限进行审核
                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS004"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {
                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                    string[] t = new string[1];
                    t[0] = ",";
                    string[] a = inventory_form.Split(t, StringSplitOptions.RemoveEmptyEntries);
                    
                    if(a.Length ==0)
                    {
                        a = new string[1];
                        a[0] = inventory_form;
                    }

                    Dictionary<string, object> p = new Dictionary<string, object>();
                    for (int i = 0; i < a.Length; i++)
                    {
                        bool IsOk = false;
                        string ErrMsg = string.Empty;

                        //2.	查询WMS013M是否存在该单号以及状态不为已审核，不存在报错：单号不存在，已审核报错：单号状态为已审核，不能重复审核
                        string sql = @"
SELECT status,warehouse1,warehouse2,location1,location2 FROM WMS013M WHERE inventory_form=@inventory_form 
";

                        p.Add("@inventory_form", a[i].ToString());
                        IDataReader Row1;
                        //string status = DB.GetString(sql, p);
                        Row1 = DB.GetDataTableReader(sql, p);
                        if (Row1.Read())
                        {
                            string status = Row1[0].ToString();
                            string warehouse1 = Row1[1].ToString();
                            string warehouse2 = Row1[2].ToString();
                            string location1 = Row1[3].ToString();
                            string location2 = Row1[4].ToString();



                            if (!string.IsNullOrEmpty(status) && status != "2")//存在单号且未审核
                            {
                                p.Clear();
                                //3.	查询WMS013A2该单是否存在差异数据，不存在报错：单号不存在实际入库数据。存在的话，根据入库数据（按物料进行汇总），调整WMS012M(库存资料）如果存在对应物料的库存，加上该单的入库数据，不存在新建一条库存数据，根据入库数据（按物料、库位进行汇总），调整WMS012A1(库存明细）如果存在对应物料、库位的库存，加上该单的入库数据，不存在新建一条库存数据。调用WMS012.DataEdit(User,物料代号,库位,数量)方法更新
                                sql = @"
SELECT [warehouse]
      ,[location]
      ,[UDF01]
      ,[material_no]
      ,[material_name]
      ,[material_specifications]
      ,[qty]
      ,qty_original
      ,qty_difference
  FROM [dbo].[WMS013A2] where inventory_form=@inventory_form  order by warehouse,location,UDF01
";
                                p.Add("@inventory_form", inventory_form);

                                //p.Add("@inventory_form", a[i].ToString());
                                string qty = string.Empty;
                                string location = string.Empty;
                                string material_no = string.Empty;
                                string material_name = string.Empty;
                                string material_specifications = string.Empty;
                                string warehouse = string.Empty;
                                string qty_original = string.Empty;
                                string qty_difference = string.Empty;
                                string size = string.Empty;

                                sql = @"
UPDATE CODE001M SET [status]='C'
WHERE location>='" + location1 + @"' AND location<='" + location2 + @"'

UPDATE CODE002M SET [status]='C'
WHERE location>='" + location1 + @"' AND location<='" + location2 + @"'

UPDATE CODE003M SET [status]='C'
WHERE location>='" + location1 + @"' AND location<='" + location2 + @"'
";
                                DB.ExecuteNonQueryOffline(sql);


                                System.Data.DataTable dtWMS013A2 = DB.GetDataTable(sql, p);
                                p.Clear();
                                if (dtWMS013A2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtWMS013A2.Rows)
                                    {
                                        qty = dr[7].ToString();//qty_difference
                                        location = dr[1].ToString();
                                        material_no = dr[2].ToString();
                                        material_name = dr[3].ToString();
                                        material_specifications = dr[4].ToString();
                                        warehouse = dr[0].ToString();
                                        qty_original = dr[6].ToString();
                                        qty_difference = dr[7].ToString();
                                        size = dr["UDF01"].ToString();

                                        WMS012.DataAdd_pandian(UserCode, material_no, material_name, material_specifications, location, warehouse,Convert.ToDouble(qty_difference), DB,size);//修改13A2表的数据
                                        
                                    }
                                    IsOk = true;
                                    IsSuccess = true;

                                   

                                    //更新条码数据
                                    sql = @"
SELECT * FROM WMS013A3(NOLOCK) WHERE inventory_form='"+ inventory_form+@"'
";
                                    DataTable dtA3 = DB.GetDataTable(sql);
                                    foreach (DataRow dr in dtA3.Rows)
                                    {

                                        sql = @"
UPDATE CODE002M SET [status]='6',qty ='" + dr["qty"].ToString() + @"',location='" + dr["location"].ToString() + @"',warehouse='" + dr["warehouse"].ToString() + @"'
WHERE lot_barcode='" + dr["code"].ToString() + @"' AND UDF01='"+dr["UDF01"].ToString()+@"'
";
                                        DB.ExecuteNonQueryOffline(sql);

                                        sql = @"
UPDATE WMS012A2 SET qty='" + dr["qty"].ToString() + @"',qty_availble='" + dr["qty"].ToString() + @"',warehouse='" + dr["warehouse"].ToString() + @"',location='" + dr["location"].ToString() + @"',
modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
WHERE lot_barcode='" + dr["code"].ToString() + @"' AND UDF01='" + dr["UDF01"].ToString() + @"'
";
                                        if (DB.ExecuteNonQueryOffline(sql) == 0)
                                        {
                                            sql = @"
INSERT INTO WMS012A2
(material_no,lot_barcode,warehouse,location,qty,qty_availble,qty_occupied,createby,createdate,createtime,UDF01)
VALUES
('" + dr["material_no"].ToString() + @"','" + dr["code"].ToString() + @"','" + dr["warehouse"].ToString() + @"','" + dr["location"].ToString() + @"','" + dr["qty"].ToString() + @"','" + dr["qty"].ToString() + @"',0,'" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + dr["UDF01"].ToString() + @"')
";
                                            DB.ExecuteNonQueryOffline(sql);
                                        }

                                    }

                                    sql = @"
SELECT
material_no,
material_name,
material_specifications,
SUM(qty_difference) as qty
from WMS013A2
where inventory_form='" + inventory_form+ @"'
GROUP BY material_no,material_name,
material_specifications
";
                                    IDataReader idr = DB.GetDataTableReader(sql);
                                    while(idr.Read())
                                    {
                                        if(Convert.ToDouble(idr["qty"].ToString())>=0)
                                        {
                                            sql = @"
UPDATE WMS012M SET qty=qty+"+ Convert.ToDouble(idr["qty"].ToString()) + @",
qty_availble=qty_availble+"+ Convert.ToDouble(idr["qty"].ToString())+@"
WHERE material_no='"+idr["material_no"].ToString()+@"'
";
                                        }
                                        else
                                        {
                                            sql = @"
UPDATE WMS012M SET qty=qty- " + -Convert.ToDouble(idr["qty"].ToString()) + @",
qty_availble=qty_availble- " + -Convert.ToDouble(idr["qty"].ToString()) + @"
WHERE material_no='" + idr["material_no"].ToString() + @"'
";
                                        }

                                        if(DB.ExecuteNonQueryOffline(sql)==0)
                                        {
                                            sql = @"
INSERT INTO WMS012M
(material_no,material_name,material_specifications,qty,qty_availble,qty_occupied)
VALUES
('"+idr["material_no"].ToString()+@"','" + idr["material_name"].ToString() + @"','" + idr["material_specifications"].ToString() + @"',"
+ Convert.ToDouble(idr["qty"].ToString()) + @"," + Convert.ToDouble(idr["qty"].ToString()) + @",0)
";
                                            DB.ExecuteNonQueryOffline(sql);

                                        }
                                    }

                                    //5.	更新单号对应的状态和审核人，审核时间，修改人，修改时间
                                    string auditby = UserCode;
                                    string auditdatetime = DateTime.Today.ToString("yyyy-MM-dd");
                                    string modifyby = UserCode;
                                    string modifydate = DateTime.Today.ToString("yyyy-MM-dd");
                                    status = "2";//已经审核
                                    DataEditStatus(a[0].ToString(), status, auditby, auditdatetime, modifyby, modifydate, DB);//修改04M表数据
                                }
                                else
                                {
                                    ErrMsg = "单号不存在差异数据";
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(status))
                                {
                                    ErrMsg = "单号不存在";
                                }
                                if (status == "2")
                                {
                                    ErrMsg = "单号状态为已审核，不能重复审核";
                                }
                            }//判断
                        }

                        RetData += @"<" + a[i].ToString() + @">
							<IsOk>" + IsOk + @"</IsOk> 
							<ErrMsg>" + ErrMsg + @"</ErrMsg> 
						</" + a[i].ToString() + @">";

                        p.Clear();
                    }
                }
                #endregion


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
        /// DataEditStatus(修改审核人信息)
        /// </summary>
        /// <param name="p1">单号</param>
        /// <param name="status">状态</param>
        /// <param name="auditby">审核人</param>
        /// <param name="auditdatetime">审核时间</param>
        /// <param name="modifyby">修改人</param>
        /// <param name="modifydate">修改时间</param>
        private static void DataEditStatus(string p1, string status, string auditby, string auditdatetime, string modifyby, string modifydate, GDSJ_Framework.DBHelper.DataBase DB)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();
            //GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
            string modifytime = DateTime.Now.ToString("hh:mm:ss");

            p.Add("inventory_form", p1);

            sql = @"
SELECT status,auditby,auditdatetime,modifyby,modifydate FROM WMS013M where inventory_form=@inventory_form 
";
            System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
            if (dr.Read())
            {
                p.Add("status", status);
                p.Add("auditby", auditby);
                p.Add("auditdatetime", auditdatetime);
                p.Add("modifyby", modifyby);
                p.Add("modifydate", modifydate);
                p.Add("modifytime", modifytime);
                //p.Add("@storage_doc", p1);

                sql = @"
 update WMS013M set status=@status,auditby=@auditby,auditdatetime=@auditdatetime,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where inventory_form=@inventory_form
";

                int i = DB.ExecuteNonQueryOffline(sql, p);
            }

        }

    }
}
