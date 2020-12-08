using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class CODE
    {
        public static string GetBarCodeInfo(object OBJ)
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
                string barcode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode>", "</barcode>");
                //string BarCodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodeType>", "</BarCodeType>");
                #endregion
                string[] a;
                if (barcode.Contains(","))
                {

                    string[] s = new string[1];
                    s[0] = ",";
                    a = barcode.Split(s, StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    a = new string[1];
                    a[0] = barcode;
                }
                for (int i = 0; i < a.Length; i++)
                {


                    string b = string.Empty;



                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                    #region 逻辑

                    // 是否存在于CODE002M(批号)

                    string sql = @"
SELECT *
FROM CODE002M
WHERE lot_barcode=@barcode
";


                    Dictionary<string, object> p1 = new Dictionary<string, object>();
                    p1.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt1 = DB.GetDataTable(sql, p1);



                    sql = @"
SELECT *
FROM BASE007M
WHERE material_no=@barcode
";


                    Dictionary<string, object> p2 = new Dictionary<string, object>();
                    p2.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt2 = DB.GetDataTable(sql, p2);





                    sql = @"
SELECT *
FROM CODE001M
WHERE products_barcode=@barcode
";


                    Dictionary<string, object> p3 = new Dictionary<string, object>();
                    p3.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt3 = DB.GetDataTable(sql, p3);



                    sql = @"
SELECT *
FROM CODE003M
WHERE packing_barcode=@barcode
";


                    Dictionary<string, object> p4 = new Dictionary<string, object>();
                    p4.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt4 = DB.GetDataTable(sql, p4);


                    if (dt1.Rows.Count > 0)    //0品号1单品2批号3包装
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                        <BarCodeType>2</BarCodeType>
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1) + @"
                        </" + a[i] + @">";
                    }
                    else if (dt2.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                        <BarCodeType>0</BarCodeType>
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2) + @"
                        </" + a[i] + @">";
                    }
                    else if (dt3.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                        <BarCodeType>1</BarCodeType>
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3) + @"
                        </" + a[i] + @">";
                    }

                    else if (dt4.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                        <BarCodeType>3</BarCodeType>
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4) + @"
                        </" + a[i] + @">";
                    }

                    else
                    {

                        RetData = "不存在数据";
                    }
                    #endregion
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
        
        public static string GetBarCodeOperation(object OBJ)
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
                string barcode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode>", "</barcode>");
                //string BarCodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodeType>", "</BarCodeType>");
                #endregion
                string[] a;
                if (barcode.Contains(","))
                {

                    string[] s = new string[1];
                    s[0] = ",";
                    a = barcode.Split(s, StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    a = new string[1];
                    a[0] = barcode;
                }
                for (int i = 0; i < a.Length; i++)
                {


                    string b = string.Empty;



                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                    #region 逻辑



                    string sql = @"
SELECT *
FROM CODE001A1
WHERE products_barcode=@barcode
";


                    Dictionary<string, object> p1 = new Dictionary<string, object>();
                    p1.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt1 = DB.GetDataTable(sql, p1);



                    sql = @"
SELECT *
FROM CODE002A1
WHERE lot_barcode=@barcode
";


                    Dictionary<string, object> p2 = new Dictionary<string, object>();
                    p2.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt2 = DB.GetDataTable(sql, p2);





                    sql = @"
SELECT *
FROM CODE003A1
WHERE packing_barcode=@barcode
";


                    Dictionary<string, object> p3 = new Dictionary<string, object>();
                    p3.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt3 = DB.GetDataTable(sql, p3);




                    if (dt1.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                        
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1) + @"
                        </" + a[i] + @">";
                    }
                    else if (dt2.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                       
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2) + @"
                        </" + a[i] + @">";
                    }
                    else if (dt3.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                      
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3) + @"
                        </" + a[i] + @">";
                    }


                    else
                    {

                        RetData = "不存在数据";
                    }
                    #endregion
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

        public static string CreateBarCode(object OBJ)
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
                string doc_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<doc_no>", "</doc_no>");
                string Type = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Type>", "</Type>");
                string DocType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<DocType>", "</DocType>");
                string org = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<org>", "</org>");
                #endregion

                string sql = string.Empty;

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                switch (DocType)
                {
                    case "6":
                        sql = @"
SELECT
serial_number as 'serial_number',
WMS003A1.material_no,
WMS003A1.material_name,
WMS003A1.material_specifications,
qty_plan as 'qty',
BASE007M.qty as 'material_qty'
FROM WMS003A1
JOIN BASE007M ON BASE007M.material_no =WMS003A1.material_no
WHERE storage_plan='" + doc_no+@"'
";
                        break;
                    case "3":
                        sql = @"
SELECT
serial_number as 'serial_number',
WMS001A1.material_no,
WMS001A1.material_name,
WMS001A1.material_specifications,
WMS001A1.qty  as 'qty',
BASE007M.qty as 'material_qty'
FROM WMS001A1
JOIN BASE007M ON BASE007M.material_no =WMS001A1.material_no
WHERE delivery_note='" + doc_no + @"'
";
                        break;

                }

                System.Data.IDataReader idr = DB.GetDataTableReader(sql);
                while(idr.Read())
                {
                    string serial_number = idr["serial_number"].ToString();
                    string material_no = idr["material_no"].ToString();
                    string material_name = idr["material_name"].ToString();
                    string material_specifications = idr["material_specifications"].ToString();
                    double qty = Convert.ToDouble( idr["qty"].ToString());
                    double material_qty =Convert.ToDouble( idr["material_qty"].ToString());
                    string barcode = string.Empty;
                    switch (Type)
                    {
                        case "CODE001":
                            sql = @"
DELETE FROM CODE001A1 WHERE products_barcode in (SELECT products_barcode from CODE001A3 where documents_type='" + DocType + @"' and documents='" + doc_no + @"')
DELETE FROM CODE001M WHERE products_barcode in (SELECT products_barcode from CODE001A3 where documents_type='" + DocType + @"' and documents='" + doc_no + @"')
DELETE FROM CODE001A3 WHERE  documents_type='" + DocType + @"' and documents='" + doc_no + @"'
";

                            for (int i = 0; i < qty; i++)
                            {
                                sql = @"
SELECT ISNULL(MAX(products_barcode),'0000') FROM CODE001M
WHERE products_barcode LIKE 'TM" + DateTime.Now.ToString("yyyyMMdd") + @"%'
";
                               barcode = DB.GetString(sql);

                                barcode = "TM" + DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(barcode.Replace("TM" + DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");

                                sql = @"
INSERT INTO CODE001M
(products_barcode,material_no,material_name,material_specifications,status,org,documents,documents_type)
VALUES
('" + barcode + @"','" + material_no + @"','" + material_name + @"','" + material_specifications + @"','1','" + org + @"','" + doc_no + @"','" + DocType + @"')

INSERT INTO CODE001A1
(products_barcode,operation,org)
VALUES
('" + barcode + @"','1','" + org + @"')

INSERT INTO CODE001A3
(products_barcode,documents_type,documents,serial_number,qty,operation,org)
VALUES
('" + barcode + @"','" + DocType + @"','" + doc_no + @"','" + serial_number + @"',1,'1','" + org + @"')
";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                            break;

                        case "CODE002":
                            sql = @"
DELETE FROM CODE002A1 WHERE lot_barcode in (SELECT lot_barcode from CODE002A3 where documents_type='" + DocType + @"' and documents='" + doc_no + @"')
DELETE FROM CODE002M WHERE lot_barcode in (SELECT lot_barcode from CODE002A3 where documents_type='" + DocType + @"' and documents='" + doc_no + @"')
DELETE FROM CODE002A3 WHERE  documents_type='" + DocType + @"' and documents='" + doc_no + @"'
";


                            sql = @"
SELECT ISNULL(MAX(lot_barcode),'0000') FROM CODE002M
WHERE lot_barcode LIKE 'LOT" + DateTime.Now.ToString("yyyyMMdd") + @"%'
";
                            barcode = DB.GetString(sql);

                            barcode = "LOT" + DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(barcode.Replace("LOT" + DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");

                            sql = @"
INSERT INTO CODE002M
(lot_barcode,material_no,material_name,material_specifications,status,org,documents,documents_type,qty)
VALUES
('" + barcode + @"','" + material_no + @"','" + material_name + @"','" + material_specifications + @"','1','" + org + @"','" + doc_no + @"','" + DocType + @"',"+qty+ @")

INSERT INTO CODE002A1
(lot_barcode,operation,org,qty)
VALUES
('" + barcode + @"','1','" + org + @"',"+qty+@")

INSERT INTO CODE002A3
(lot_barcode,documents_type,documents,serial_number,qty,operation,org)
VALUES
('" + barcode + @"','" + DocType + @"','" + doc_no + @"','" + serial_number + @"',"+qty+@",'1','" + org + @"')
";
                            DB.ExecuteNonQueryOffline(sql);

                            break;

                        case "CODE003":
                            sql = @"
DELETE FROM CODE003A1 WHERE packing_barcode in (SELECT packing_barcode from CODE003A3 where documents_type='" + DocType + @"' and documents='" + doc_no + @"')
DELETE FROM CODE003M WHERE packing_barcode in (SELECT packing_barcode from CODE003A3 where documents_type='" + DocType + @"' and documents='" + doc_no + @"')
DELETE FROM CODE003A3 WHERE  documents_type='" + DocType + @"' and documents='" + doc_no + @"'
";
                            if(material_qty==0)
                            {
                                material_qty = qty;
                            }
                            while (qty > 0)
                            {
                                if(qty>=material_qty)
                                {
                                    qty -= material_qty;
                                }
                                else
                                {
                                    material_qty = qty;
                                    qty -= material_qty;
                                }


                                sql = @"
SELECT ISNULL(MAX(packing_barcode),'0000') FROM CODE003M
WHERE packing_barcode LIKE 'PK" + DateTime.Now.ToString("yyyyMMdd") + @"%'
";
                                 barcode = DB.GetString(sql);

                                barcode = "PK" + DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(barcode.Replace("PK" + DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");

                                sql = @"
INSERT INTO CODE003M
(packing_barcode,material_no,material_name,material_specifications,status,org,documents,documents_type,qty)
VALUES
('" + barcode + @"','" + material_no + @"','" + material_name + @"','" + material_specifications + @"','1','" + org + @"','" + doc_no + @"','" + DocType + @"'," + material_qty + @")

INSERT INTO CODE003A1
(packing_barcode,operation,org,qty)
VALUES
('" + barcode + @"','1','" + org + @"'," + material_qty + @")

INSERT INTO CODE003A3
(packing_barcode,documents_type,documents,serial_number,qty,operation,org)
VALUES
('" + barcode + @"','" + DocType + @"','" + doc_no + @"','" + serial_number + @"'," + material_qty + @",'1','" + org + @"')
";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                            break;
                    }

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





        public static string GetBarCodeDocuments(object OBJ)
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
                string barcode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode>", "</barcode>");
                //string BarCodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodeType>", "</BarCodeType>");
                #endregion
                string[] a;
                if (barcode.Contains(","))
                {

                    string[] s = new string[1];
                    s[0] = ",";
                    a = barcode.Split(s, StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    a = new string[1];
                    a[0] = barcode;
                }
                for (int i = 0; i < a.Length; i++)
                {


                    string b = string.Empty;



                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                    #region 逻辑



                    string sql = @"
SELECT *
FROM CODE001A3
WHERE products_barcode=@barcode
";


                    Dictionary<string, object> p1 = new Dictionary<string, object>();
                    p1.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt1 = DB.GetDataTable(sql, p1);



                    sql = @"
SELECT *
FROM CODE002A3
WHERE lot_barcode=@barcode
";


                    Dictionary<string, object> p2 = new Dictionary<string, object>();
                    p2.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt2 = DB.GetDataTable(sql, p2);





                    sql = @"
SELECT *
FROM CODE003A3
WHERE packing_barcode=@barcode
";


                    Dictionary<string, object> p3 = new Dictionary<string, object>();
                    p3.Add("@barcode", a[i].ToString());
                    System.Data.DataTable dt3 = DB.GetDataTable(sql, p3);




                    if (dt1.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                       
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1) + @"
                        </" + a[i] + @">";
                    }
                    else if (dt2.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                       
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2) + @"
                        </" + a[i] + @">";
                    }
                    else if (dt3.Rows.Count > 0)
                    {
                        IsSuccess = true;
                        RetData += "<" + a[i] + ">" + @"
                        
                        " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3) + @"
                        </" + a[i] + @">";
                    }


                    else
                    {

                        RetData = "不存在数据";
                    }
                    #endregion
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


        public static string Together(object OBJ)
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
                string barcode1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode1>", "</barcode1>");
                string barcode2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode2>", "</barcode2>");
                string User = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                //string BarCodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodeType>", "</BarCodeType>");
                #endregion



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                #region 逻辑
                if (barcode1.Contains("PK") && barcode2.Contains("PK"))  //箱号条码的时候
                {
                    string sql = @"select distinct material_no from CODE003M where packing_barcode='" + barcode1 + "' or packing_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt = DB.GetDataTable(sql);
                    sql = @"select distinct [status] from CODE003M where packing_barcode='" + barcode1 + "' or packing_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt1 = DB.GetDataTable(sql);
                    string status = (string)dt1.Rows[0][0];
                    //string status2 = (string)dt1.Rows[0][1];

                    if (dt.Rows.Count > 1)
                    {
                        RetData = "物料不相同，不能操作";

                    }
                    else if (barcode1 == barcode2)
                    {
                        RetData = "请不要选择相同的条码";
                    }
                    else if (status == "C")
                    {
                        RetData = "状态为锁定，不能合拼";
                    }
                    else if (dt1.Rows.Count <= 1 && dt1.Rows.Count > 0 && status == "6")    //status1!="6" 
                    {



                        //if (status1 != "6")
                        //{
                        //    RetData += "该箱号不是库存状态，不能操作";
                        //}
                        sql = @"
select qty from CODE003M where packing_barcode=@barcode1
";


                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        p1.Add("@barcode1", barcode1);
                        double qty1 = DB.GetDouble(sql, p1);



                        sql = @"
select qty from CODE003M where packing_barcode=@barcode2
";


                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        p2.Add("@barcode2", barcode2);
                        double qty2 = DB.GetDouble(sql, p2);


                        qty1 += qty2;


                        sql = @"
update CODE003M set qty='" + qty1 + @"',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + "'where packing_barcode=@barcode1";


                        Dictionary<string, object> p3 = new Dictionary<string, object>();
                        p3.Add("@barcode1", barcode1);
                        DB.ExecuteNonQueryOffline(sql, p3);    //这里报错

                        sql = @"
update CODE003M set qty='0',status='C',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + @"' where packing_barcode=@barcode2
";


                        Dictionary<string, object> p4 = new Dictionary<string, object>();
                        p4.Add("@barcode2", barcode2);

                        DB.ExecuteNonQueryOffline(sql, p4);

                        sql = @"insert into CODE003A1(packing_barcode,operation,lot_barcode,qty,createby,createdate,createtime) values('" + barcode1 + "','合拼','" + barcode2 + "','" + qty1 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8))
insert into CODE003A1(packing_barcode,operation, lot_barcode,qty,createby,createdate,createtime) values('" + barcode2 + "','合拼', '" + barcode1 + "','" + qty2 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8)) 

";
                        //insert into CODE003A1(operation, lot_barcode) values('合拼', '" + barcode1 + @"') where packing_barcode = @barcode2
                        //Dictionary<string, object> p5 = new Dictionary<string, object>();
                        //p5.Add("@barcode2", barcode2);
                        //p5.Add("@barcode1", barcode1);

                        int a = DB.ExecuteNonQueryOffline(sql);
                        //sql = @"select * from CODE003A1 where packing_barcode='" + barcode1 + "'";
                        //System.Data.DataTable dt = DB.GetDataTable(sql);
                        //if (dt.Rows.Count>0)
                        //{
                        //    RetData = "已经执行过一次了";
                        //}


                        sql = @" delete from WMS012A3 where packing_barcode = '" + barcode2 + "'";

                        DB.ExecuteNonQueryOffline(sql);
                        sql = @"select * from WMS012A3 where packing_barcode= '" + barcode1 + "'";
                        System.Data.DataTable dt3 = DB.GetDataTable(sql);
                        if (dt3.Rows.Count > 0)
                        {
                            sql = @"update WMS012A3 set qty='" + qty1 + "',qty_availble='" + qty1 + @"',qty_occupied='0',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby = '" + User + "' where packing_barcode='" + barcode1 + "'";
                            DB.ExecuteNonQueryOffline(sql);
                        }
                        else
                        {
                            sql = @"insert into WMS012A3(packing_barcode,material_no,lot_barcode,suppliers_lot,warehouse,location,qty,qty_availble,qty_occupied,createby,createdate,createtime) select packing_barcode,material_no,lot_barcode,suppliers_lot,warehouse,location,qty,qty,0,'" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8) from CODE003M
where packing_barcode='" + barcode1 + "'";
                            DB.ExecuteNonQueryOffline(sql);
                        }


                        if (a <= 0)
                        {

                            RetData = "没有执行成功";

                        }
                        else
                        {
                            IsSuccess = true;
                        }

                    }
                }
                else if (barcode1.Contains("LOT") && barcode2.Contains("LOT"))
                {
                    string sql = @"select distinct material_no from CODE002M where lot_barcode='" + barcode1 + "' or lot_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt = DB.GetDataTable(sql);
                    sql = @"select distinct [status] from CODE002M where lot_barcode='" + barcode1 + "' or lot_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt1 = DB.GetDataTable(sql);
                    string status = (string)dt1.Rows[0][0];
                    //string status2 = (string)dt1.Rows[0][1];

                    if (dt.Rows.Count > 1)
                    {
                        RetData = "物料不相同，不能操作";

                    }
                    else if (barcode1 == barcode2)
                    {
                        RetData = "请不要选择相同的条码";
                    }
                    else if (status == "C")
                    {
                        RetData = "状态为锁定，不能合拼";
                    }
                    else if (dt1.Rows.Count <= 1 && dt1.Rows.Count > 0 && status == "6")    //status1!="6" 
                    {



                        //if (status1 != "6")
                        //{
                        //    RetData += "该箱号不是库存状态，不能操作";
                        //}
                        sql = @"
select qty from CODE002M where lot_barcode=@barcode1
";


                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        p1.Add("@barcode1", barcode1);
                        double qty1 = DB.GetDouble(sql, p1);



                        sql = @"
select qty from CODE002M where lot_barcode=@barcode2
";


                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        p2.Add("@barcode2", barcode2);
                        double qty2 = DB.GetDouble(sql, p2);


                        qty1 += qty2;


                        sql = @"
update CODE002M set qty='" + qty1 + @"',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + "'where lot_barcode=@barcode1";


                        Dictionary<string, object> p3 = new Dictionary<string, object>();
                        p3.Add("@barcode1", barcode1);
                        DB.ExecuteNonQueryOffline(sql, p3);    //这里报错

                        sql = @"
update CODE002M set qty='0',status='C',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + @"' where lot_barcode=@barcode2
";


                        Dictionary<string, object> p4 = new Dictionary<string, object>();
                        p4.Add("@barcode2", barcode2);

                        DB.ExecuteNonQueryOffline(sql, p4);

                        sql = @"insert into CODE002A1(lot_barcode,operation,qty,createby,createdate,createtime) values('" + barcode1 + "','合拼','" + qty1 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8))
insert into CODE002A1(lot_barcode,operation,qty,createby,createdate,createtime) values('" + barcode2 + "','合拼','" + qty2 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8)) 

";
                        //insert into CODE003A1(operation, lot_barcode) values('合拼', '" + barcode1 + @"') where packing_barcode = @barcode2
                        //Dictionary<string, object> p5 = new Dictionary<string, object>();
                        //p5.Add("@barcode2", barcode2);
                        //p5.Add("@barcode1", barcode1);

                        int a = DB.ExecuteNonQueryOffline(sql);
                        //sql = @"select * from CODE003A1 where packing_barcode='" + barcode1 + "'";
                        //System.Data.DataTable dt = DB.GetDataTable(sql);
                        //if (dt.Rows.Count>0)
                        //{
                        //    RetData = "已经执行过一次了";
                        //}
                        sql = @"delete from WMS012A2 where lot_barcode='" + barcode2 + "'";
                        DB.ExecuteNonQueryOffline(sql);

                        sql = @"select * from WMS012A2 where lot_barcode= '" + barcode1 + "'";
                        System.Data.DataTable dt3 = DB.GetDataTable(sql);
                        if (dt3.Rows.Count > 0)
                        {
                            sql = @"update WMS012A2 set qty='" + qty1 + "',qty_availble='" + qty1 + @"',qty_occupied='0',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby = '" + User + "' where lot_barcode='" + barcode1 + "'";
                            DB.ExecuteNonQueryOffline(sql);
                        }
                        else
                        {
                            sql = @"insert into WMS012A2(lot_barcode,material_no,suppliers_lot,location,qty,qty_availble,qty_occupied,createby,createdate,createtime) select lot_barcode,material_no,suppliers_lot,location,qty,qty,0,'" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8)) from CODE002M
where lot_barcode='" + barcode1 + "'";
                            DB.ExecuteNonQueryOffline(sql);
                        }

                        if (a <= 0)
                        {

                            RetData = "没有执行成功";
                        }
                        else
                        {
                            IsSuccess = true;
                        }

                    }
                }


                else
                {

                    RetData += "该批号不是库存状态，不能操作";


                }


                //if (dt1.Rows.Count > 0)
                //{
                //    IsSuccess = true;
                //    RetData += " < " + a[i] + ">" + @"

                //    " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1) + @"
                //    </" + a[i] + @">";
                //}
                //else if (dt2.Rows.Count > 0)
                //{
                //    IsSuccess = true;
                //    RetData += "<" + a[i] + ">" + @"

                //    " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2) + @"
                //    </" + a[i] + @">";
                //}



                //else
                //{

                //    RetData = "不存在数据";
                //}
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



        public static string Split(object OBJ)
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
                string barcode1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode1>", "</barcode1>");
                string barcode2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<barcode2>", "</barcode2>");
                string qty = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<qty>", "</qty>");
                string User = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                //string BarCodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodeType>", "</BarCodeType>");
                #endregion



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                #region 逻辑

                if (barcode1.Contains("PK") && barcode2.Contains("PK"))
                {


                    string sql = @"select distinct material_no from CODE003M where packing_barcode='" + barcode1 + "' or packing_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt = DB.GetDataTable(sql);
                    sql = @"select distinct [status] from CODE003M where packing_barcode='" + barcode1 + "' or packing_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt1 = DB.GetDataTable(sql);
                    string status = (string)dt1.Rows[0][0];
                    //string status2 = (string)dt1.Rows[0][1];

                    if (dt.Rows.Count > 1)
                    {
                        RetData = "物料不相同，不能操作";

                    }
                    else if (barcode1 == barcode2)
                    {
                        RetData = "请不要选择相同的条码";
                    }
                    else if (status == "C")
                    {
                        RetData = "状态为锁定，不能操作";
                    }
                    else if (dt1.Rows.Count <= 1 && dt1.Rows.Count > 0 && status == "6")    //status1!="6" 
                    {
                        sql = @"
select qty from CODE003M where packing_barcode=@barcode1
";


                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        p1.Add("@barcode1", barcode1);
                        double qty1 = DB.GetDouble(sql, p1);
                        if (qty1 < Convert.ToDouble(qty))
                        {
                            RetData = "分拆的数量必须小于条码1的数量";
                        }

                        else
                        {
                            sql = @"
select qty from CODE003M where packing_barcode=@barcode2
";


                            Dictionary<string, object> p2 = new Dictionary<string, object>();
                            p2.Add("@barcode2", barcode2);
                            double qty2 = DB.GetDouble(sql, p2);


                            qty1 = qty1 - Convert.ToDouble(qty);
                            qty2 = qty2 + Convert.ToDouble(qty);


                            sql = @"
update CODE003M set qty='" + qty1 + @"',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + "' where packing_barcode=@barcode1";


                            Dictionary<string, object> p3 = new Dictionary<string, object>();
                            p3.Add("@barcode1", barcode1);
                            DB.ExecuteNonQueryOffline(sql, p3);    //这里报错

                            sql = @"
update CODE003M set qty='" + qty2 + @"',modifydate= Datename(year,GetDate())+'-'+Datename
(month,GetDate())+'-'+Datename(day,GetDate()),modifytime=CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + @"' where packing_barcode=@barcode2
";


                            Dictionary<string, object> p4 = new Dictionary<string, object>();
                            p4.Add("@barcode2", barcode2);

                            DB.ExecuteNonQueryOffline(sql, p4);
                            //sql = @"select * from CODE003A1 where packing_barcode='" + barcode1 + "'";
                            //System.Data.DataTable dt3 = DB.GetDataTable(sql);
                            //int a = 0;
                            //if (dt3.Rows.Count > 0)
                            //{
                            //    RetData = "已经执行过一次了";

                            //}


                            sql = @"insert into CODE003A1(packing_barcode,operation,lot_barcode,qty,createby,createdate,createtime) values('" + barcode1 + "','分拆','" + barcode2 + "','" + qty1 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8)) 
insert into CODE003A1(packing_barcode,operation, lot_barcode,qty,createby,createdate,createtime) values('" + barcode2 + "','分拆', '" + barcode1 + "','" + qty2 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8)) 

";
                            //insert into CODE003A1(operation, lot_barcode) values('合拼', '" + barcode1 + @"') where packing_barcode = @barcode2
                            //Dictionary<string, object> p5 = new Dictionary<string, object>();
                            //p5.Add("@barcode2", barcode2);
                            //p5.Add("@barcode1", barcode1);

                            int a = DB.ExecuteNonQueryOffline(sql);


                            //数量拆分中barcode1减少后的数量
                            sql = @"select * from WMS012A3 where packing_barcode= '" + barcode1 + "'";
                            System.Data.DataTable dt3 = DB.GetDataTable(sql);
                            //string qtySplitAdd=Convert.ToInt32(qty1)
                            if (dt3.Rows.Count > 0)
                            {
                                sql = @"update WMS012A3 set qty-='" + qty + "',qty_availble-='" + qty + @"',qty_occupied='0',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + "'  where packing_barcode='" + barcode1 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                            else
                            {
                                sql = @"insert into WMS012A3(packing_barcode,material_no,lot_barcode,suppliers_lot,warehouse,location,qty,qty_availble,qty_occupied,createby,createdate,createtime) select packing_barcode,material_no,lot_barcode,suppliers_lot,warehouse,location,qty,qty,0,'" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8)) from CODE003M
where packing_barcode='" + barcode1 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }

                            //数量拆分中barcode2增加后的数量
                            sql = @"select * from WMS012A3 where packing_barcode= '" + barcode2 + "'";
                            System.Data.DataTable dt4 = DB.GetDataTable(sql);
                            //string qtySplitAdd=Convert.ToInt32(qty1)
                            if (dt4.Rows.Count > 0)
                            {
                                sql = @"update WMS012A3 set qty+='" + qty + "',qty_availble+='" + qty + @"',qty_occupied='0',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + "' where packing_barcode='" + barcode2 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                            else
                            {
                                sql = @"insert into WMS012A3(packing_barcode,material_no,lot_barcode,suppliers_lot,warehouse,location,qty,qty_availble,qty_occupied,createby,createdate,createtime) select packing_barcode,material_no,lot_barcode,suppliers_lot,warehouse,location,qty,qty,0,'" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8) from CODE003M
where packing_barcode='" + barcode2 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }







                            if (a <= 0)
                            {

                                RetData = "没有执行成功";
                            }
                            else
                            {
                                IsSuccess = true;
                            }
                        }
                    }





                    else
                    {



                        RetData += "该箱号不是库存状态，不能操作";




                        //if (dt1.Rows.Count > 0)
                        //{
                        //    IsSuccess = true;
                        //    RetData += " < " + a[i] + ">" + @"

                        //    " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1) + @"
                        //    </" + a[i] + @">";
                        //}
                        //else if (dt2.Rows.Count > 0)
                        //{
                        //    IsSuccess = true;
                        //    RetData += "<" + a[i] + ">" + @"

                        //    " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2) + @"
                        //    </" + a[i] + @">";
                        //}



                        //else
                        //{

                        //    RetData = "不存在数据";
                        //}

                    }
                }
                else if (barcode1.Contains("LOT") && barcode2.Contains("LOT"))
                {
                    string sql = @"select distinct material_no from CODE002M where lot_barcode='" + barcode1 + "' or lot_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt = DB.GetDataTable(sql);
                    sql = @"select distinct [status] from CODE002M where lot_barcode='" + barcode1 + "' or lot_barcode='" + barcode2 + "'";
                    System.Data.DataTable dt1 = DB.GetDataTable(sql);
                    string status = (string)dt1.Rows[0][0];
                    //string status2 = (string)dt1.Rows[0][1];

                    if (dt.Rows.Count > 1)
                    {
                        RetData = "物料不相同，不能操作";

                    }
                    else if (barcode1 == barcode2)
                    {
                        RetData = "请不要选择相同的条码";
                    }
                    else if (status == "C")
                    {
                        RetData = "状态为锁定，不能操作";
                    }
                    else if (dt1.Rows.Count <= 1 && dt1.Rows.Count > 0 && status == "6")    //status1!="6" 
                    {
                        sql = @"
select qty from CODE002M where lot_barcode=@barcode1
";


                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        p1.Add("@barcode1", barcode1);
                        double qty1 = DB.GetDouble(sql, p1);
                        if (qty1 < Convert.ToDouble(qty))
                        {
                            RetData = "分拆的数量必须小于条码1的数量";
                        }

                        else
                        {
                            sql = @"
select qty from CODE002M where lot_barcode=@barcode2
";


                            Dictionary<string, object> p2 = new Dictionary<string, object>();
                            p2.Add("@barcode2", barcode2);
                            double qty2 = DB.GetDouble(sql, p2);


                            qty1 = qty1 - Convert.ToDouble(qty);
                            qty2 = qty2 + Convert.ToDouble(qty);


                            sql = @"
update CODE002M set qty='" + qty1 + @"',modifydate= Datename(year,GetDate())+'-'+Datename
(month,GetDate())+'-'+Datename(day,GetDate()),modifytime=CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + "' where lot_barcode=@barcode1";


                            Dictionary<string, object> p3 = new Dictionary<string, object>();
                            p3.Add("@barcode1", barcode1);
                            DB.ExecuteNonQueryOffline(sql, p3);    //这里报错

                            sql = @"
update CODE002M set qty='" + qty2 + @"',modifydate= Datename(year,GetDate())+'-'+Datename
(month,GetDate())+'-'+Datename(day,GetDate()),modifytime=CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + @"' where lot_barcode=@barcode2
";


                            Dictionary<string, object> p4 = new Dictionary<string, object>();
                            p4.Add("@barcode2", barcode2);

                            DB.ExecuteNonQueryOffline(sql, p4);
                            //sql = @"select * from CODE003A1 where packing_barcode='" + barcode1 + "'";
                            //System.Data.DataTable dt3 = DB.GetDataTable(sql);
                            //int a = 0;
                            //if (dt3.Rows.Count > 0)
                            //{
                            //    RetData = "已经执行过一次了";

                            //}


                            sql = @"insert into CODE002A1(lot_barcode,operation,qty,createby,createdate,createtime) values('" + barcode1 + "','分拆','" + qty1 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8))
insert into CODE002A1(lot_barcode,operation, qty,createby,createdate,createtime) values('" + barcode2 + "','分拆', '" + qty2 + @"','" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8))

";
                            //insert into CODE003A1(operation, lot_barcode) values('合拼', '" + barcode1 + @"') where packing_barcode = @barcode2
                            //Dictionary<string, object> p5 = new Dictionary<string, object>();
                            //p5.Add("@barcode2", barcode2);
                            //p5.Add("@barcode1", barcode1);

                            int a = DB.ExecuteNonQueryOffline(sql);


                            //数量拆分中barcode1减少后的数量
                            sql = @"select * from WMS012A2 where lot_barcode= '" + barcode1 + "'";
                            System.Data.DataTable dt3 = DB.GetDataTable(sql);
                            //string qtySplitAdd=Convert.ToInt32(qty1)
                            if (dt3.Rows.Count > 0)
                            {
                                sql = @"update WMS012A2 set qty-='" + qty + "',qty_availble-='" + qty + @"',qty_occupied='0',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby = '" + User + "' where lot_barcode='" + barcode1 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                            else
                            {
                                sql = @"insert into WMS012A2(material_no,lot_barcode,suppliers_lot,location,qty,qty_availble,qty_occupied,createby,createdate,createtime) select material_no,lot_barcode,suppliers_lot,location,qty,qty,0,'" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8) from CODE002M
where lot_barcode='" + barcode1 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }

                            //数量拆分中barcode2增加后的数量
                            sql = @"select * from WMS012A2 where lot_barcode= '" + barcode2 + "'";
                            System.Data.DataTable dt4 = DB.GetDataTable(sql);
                            //string qtySplitAdd=Convert.ToInt32(qty1)
                            if (dt4.Rows.Count > 0)
                            {
                                sql = @"update WMS012A2 set qty+='" + qty + "',qty_availble+='" + qty + @"',qty_occupied='0',modifydate= Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),modifytime = CONVERT(varchar(100), GETDATE(), 8),modifyby='" + User + "' where lot_barcode='" + barcode2 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                            else
                            {
                                sql = @"insert into WMS012A2(material_no,lot_barcode,suppliers_lot,location,qty,qty_availble,qty_occupied,createby,createdate,createtime) select material_no,lot_barcode,suppliers_lot,location,qty,qty,0,'" + User + @"',Datename(year,GetDate())+'-'+Datename
(month, GetDate()) + '-' + Datename(day, GetDate()),CONVERT(varchar(100), GETDATE(), 8) from CODE002M
where lot_barcode='" + barcode2 + "'";
                                DB.ExecuteNonQueryOffline(sql);
                            }






                            if (a <= 0)
                            {

                                RetData = "没有执行成功";
                            }
                            else
                            {
                                IsSuccess = true;
                            }
                        }
                    }





                    else
                    {



                        RetData += "该箱号不是库存状态，不能操作";


                        #endregion

                        //if (dt1.Rows.Count > 0)
                        //{
                        //    IsSuccess = true;
                        //    RetData += " < " + a[i] + ">" + @"

                        //    " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1) + @"
                        //    </" + a[i] + @">";
                        //}
                        //else if (dt2.Rows.Count > 0)
                        //{
                        //    IsSuccess = true;
                        //    RetData += "<" + a[i] + ">" + @"

                        //    " + GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2) + @"
                        //    </" + a[i] + @">";
                        //}



                        //else
                        //{

                        //    RetData = "不存在数据";
                        //}

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
    }
}
