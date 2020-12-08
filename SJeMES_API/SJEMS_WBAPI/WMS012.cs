using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class WMS012
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

        public static string GetData(object OBJ)
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
                string code = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<code>", "</code>");
                string type = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<type>", "</type>");
                string page = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<page>", "</page>");
                //string BarCodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodeType>", "</BarCodeType>");
                #endregion
                string[] a;
                if (code.Contains(","))
                {

                    string[] s = new string[1];
                    s[0] = ",";
                    a = code.Split(s, StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    a = new string[1];
                    a[0] = code;
                }

                for (int i = 0; i < a.Length; i++)
                {

                    string b = string.Empty;



                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                    int pageSize = 10;

                    #region 逻辑

                    #region type=1


                    if (type == "1")
                    {

                        //                        string sql = @"
                        //SELECT *
                        //FROM WMS012M
                        //WHERE material_no=@code
                        //";


                        //                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        //                        p1.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt1 = DB.GetDataTable(sql, p1);
                        //                        string dtXML1 = "";
                        //                        if (dt1.Rows.Count > 0)
                        //                        {
                        //                            dtXML1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1);
                        //                        }



                        //                        string sql = @"
                        //SELECT *
                        //FROM WMS012A1
                        //WHERE material_no=@code
                        //";


                        //                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        //                        p2.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt2 = DB.GetDataTable(sql, p2);
                        //                        string dtXML2 = "";
                        //                        if (dt2.Rows.Count > 0)
                        //                        {
                        //                            dtXML2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);
                        //                        }

                        int start = (Convert.ToInt32(page) - 1) * pageSize + 1;
                        int end = Convert.ToInt32(page) * pageSize;
                        string sql = "select * from(select *,row_number()over(order by id) as num from WMS012M where material_no='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt1 = DB.GetDataTable(sql);
                        string dtXMLM = "";
                        if (dt1.Rows.Count > 0)
                        {
                            dtXMLM = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1);
                        }


                        sql = "select count(*) from WMS012M where material_no='" + code + "'";
                        int pageCount = DB.GetInt32(sql);
                        int pageCountM = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));





                        //int start = (Convert.ToInt32(page) - 1) * pageSize + 1;
                        //int end = Convert.ToInt32(page) * pageSize;
                        sql = @"select * from(select WMS012A1.*,material_name,material_specifications,row_number()over(order by WMS012A1.id) as num 
from WMS012A1 
join BASE007M on WMS012A1.material_no = BASE007M.material_no where WMS012A1.material_no='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt2 = DB.GetDataTable(sql);
                        string dtXMLA1 = "";
                        if (dt2.Rows.Count > 0)
                        {
                            dtXMLA1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);
                        }


                        sql = "select count(*) from WMS012A1 where material_no='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA1 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));



                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A2
                        //WHERE material_no=@code
                        //";


                        //                        Dictionary<string, object> p3 = new Dictionary<string, object>();
                        //                        p3.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt3 = DB.GetDataTable(sql, p3);
                        //                        string dtXML3 = "";
                        //                        if (dt3.Rows.Count > 0)
                        //                        {
                        //                            dtXML3 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3);
                        //                        }

                        //当前页
                        sql = @"select * from(select WMS012A2.*,material_name,material_specifications,row_number()over(order by wms012A2.id) as num 
from WMS012A2 
join BASE007M on WMS012A2.material_no = BASE007M.material_no where WMS012A2.material_no='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt3 = DB.GetDataTable(sql);
                        string dtXMLA2 = "";
                        if (dt3.Rows.Count > 0)
                        {
                            dtXMLA2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3);
                        }



                        sql = "select count(*) from WMS012A2 where material_no='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA2 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));


                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A3
                        //WHERE material_no=@code
                        //";


                        //                        Dictionary<string, object> p4 = new Dictionary<string, object>();
                        //                        p4.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt4 = DB.GetDataTable(sql, p4);
                        //                        string dtXML4 = "";
                        //                        if (dt4.Rows.Count > 0)
                        //                        {
                        //                            dtXML4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4);
                        //                        }

                        //WMS012A3的当前页的内容
                        sql = @"select * from(select WMS012A3.*,material_name,material_specifications,row_number()over(order by wms012A3.id) as num 
from WMS012A3 
join BASE007M on WMS012A3.material_no = BASE007M.material_no where WMS012A3.material_no='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt4 = DB.GetDataTable(sql);
                        string dtXMLA3 = "";
                        if (dt4.Rows.Count > 0)
                        {
                            dtXMLA3 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4);
                        }

                        //WMS012A3的所有页数
                        sql = "select count(*) from WMS012A3 where material_no='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA3 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));

                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A4
                        //WHERE material_no=@code
                        //";


                        //                        Dictionary<string, object> p5 = new Dictionary<string, object>();
                        //                        p5.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt5 = DB.GetDataTable(sql, p5);

                        //                        string dtXML5 = "";
                        //                        if (dt5.Rows.Count > 0)
                        //                        {
                        //                            dtXML5 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt5);
                        //                        }


                        sql = @"select * from(select WMS012A4.*,material_name,material_specifications,row_number()over(order by wms012A4.id) as num 
from WMS012A4 
join BASE007M on WMS012A4.material_no = BASE007M.material_no where WMS012A4.material_no='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        string dtXMLA4 = "";
                        System.Data.DataTable dt5 = DB.GetDataTable(sql);
                        if (dt5.Rows.Count > 0)
                        {
                            dtXMLA4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt5);
                        }



                        sql = "select count(*) from WMS012A4 where material_no='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA4 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));


                        if (dt2.Rows.Count > 0 || dt3.Rows.Count > 0 || dt4.Rows.Count > 0 || dt5.Rows.Count > 0)
                        {
                            //RetData += "<WMS012M>" + dtXML1 + @"</WMS012M>";
                            IsSuccess = true;
                            RetData += "<WMS012MPageCount>" + pageCountM + @"</WMS012MPageCount>";
                            RetData += "<WMS012M>" + dtXMLM + @"</WMS012M>";
                            RetData += "<WMS012A1PageCount>" + pageCountA1 + @"</WMS012A1PageCount>";
                            RetData += "<WMS012A1>" + dtXMLA1 + @"</WMS012A1>";
                            RetData += "<WMS012A2PageCount>" + pageCountA2 + @"</WMS012A2PageCount>";
                            RetData += "<WMS012A2>" + dtXMLA2 + @"</WMS012A2>";
                            RetData += "<WMS012A3PageCount>" + pageCountA3 + @"</WMS012A3PageCount>";
                            RetData += "<WMS012A3>" + dtXMLA3 + @"</WMS012A3>";
                            RetData += "<WMS012A4PageCount>" + pageCountA4 + @"</WMS012A4PageCount>";
                            RetData += "<WMS012A4>" + dtXMLA4 + @"</WMS012A4>";
                        }


                        else
                        {

                            RetData = "不存在数据";
                        }
                    }
                    #endregion

                    #region type=2
                    else if (type == "2")
                    {

                        //                        string sql = @"
                        //SELECT *
                        //FROM WMS012M
                        //WHERE material_no=@code
                        //";


                        //                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        //                        p1.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt1 = DB.GetDataTable(sql, p1);
                        //                        string dtXML1 = "";
                        //                        if (dt1.Rows.Count > 0)
                        //                        {
                        //                            dtXML1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1);
                        //                        }



                        //                        string sql = @"
                        //SELECT *
                        //FROM WMS012A1
                        //WHERE location=@code
                        //";


                        //                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        //                        p2.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt2 = DB.GetDataTable(sql, p2);
                        //                        string dtXML2 = "";
                        //                        if (dt2.Rows.Count > 0)
                        //                        {
                        //                            dtXML2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);
                        //                        }




                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A2
                        //WHERE location=@code
                        //";


                        //                        Dictionary<string, object> p3 = new Dictionary<string, object>();
                        //                        p3.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt3 = DB.GetDataTable(sql, p3);
                        //                        string dtXML3 = "";
                        //                        if (dt3.Rows.Count > 0)
                        //                        {
                        //                            dtXML3 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3);
                        //                        }


                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A3
                        //WHERE location=@code
                        //";


                        //                        Dictionary<string, object> p4 = new Dictionary<string, object>();
                        //                        p4.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt4 = DB.GetDataTable(sql, p4);
                        //                        string dtXML4 = "";
                        //                        if (dt4.Rows.Count > 0)
                        //                        {
                        //                            dtXML4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4);
                        //                        }

                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A4
                        //WHERE location=@code
                        //";


                        //                        Dictionary<string, object> p5 = new Dictionary<string, object>();
                        //                        p5.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt5 = DB.GetDataTable(sql, p5);

                        //                        string dtXML5 = "";
                        //                        if (dt5.Rows.Count > 0)
                        //                        {
                        //                            dtXML5 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt5);
                        //                        }


                        int start = (Convert.ToInt32(page) - 1) * pageSize + 1;
                        int end = Convert.ToInt32(page) * pageSize;
                        string sql = @"select * from(select WMS012A1.*,material_name,material_specifications,row_number()over(order by WMS012A1.id) as num 
from WMS012A1 
join BASE007M on WMS012A1.material_no = BASE007M.material_no where WMS012A1.location='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt2 = DB.GetDataTable(sql);
                        string dtXMLA1 = "";
                        if (dt2.Rows.Count > 0)
                        {
                            dtXMLA1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);
                        }
                        sql = "select count(*) from WMS012A1 where location='" + code + "'";
                        int pageCount = DB.GetInt32(sql);
                        int pageCountA1 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));



                        sql = @"select * from(select WMS012A2.*,material_name,material_specifications,row_number()over(order by wms012A2.id) as num 
from WMS012A2 
join BASE007M on WMS012A2.material_no = BASE007M.material_no where WMS012A2.location='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt3 = DB.GetDataTable(sql);
                        string dtXMLA2 = "";
                        if (dt3.Rows.Count > 0)
                        {
                            dtXMLA2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3);
                        }



                        sql = "select count(*) from WMS012A2 where location='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA2 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));



                        sql = @"select * from(select WMS012A3.*,material_name,material_specifications,row_number()over(order by wms012A3.id) as num 
from WMS012A3 
join BASE007M on WMS012A3.material_no = BASE007M.material_no where WMS012A3.location='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt4 = DB.GetDataTable(sql);
                        string dtXMLA3 = "";
                        if (dt4.Rows.Count > 0)
                        {
                            dtXMLA3 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4);
                        }

                        //WMS012A3的所有页数
                        sql = "select count(*) from WMS012A3 where location='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA3 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));



                        sql = @"select * from(select WMS012A4.*,material_name,material_specifications,row_number()over(order by wms012A4.id) as num 
from WMS012A4 
join BASE007M on WMS012A4.material_no = BASE007M.material_no where WMS012A4.location='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        string dtXMLA4 = "";
                        System.Data.DataTable dt5 = DB.GetDataTable(sql);
                        if (dt5.Rows.Count > 0)
                        {
                            dtXMLA4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt5);
                        }



                        sql = "select count(*) from WMS012A4 where location='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA4 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));




                        if (dt2.Rows.Count > 0 || dt3.Rows.Count > 0 || dt4.Rows.Count > 0 || dt5.Rows.Count > 0)
                        {
                            //RetData += "<WMS012M>" + dtXML1 + @"</WMS012M>";
                            IsSuccess = true;
                            RetData += "<WMS012A1PageCount>" + pageCountA1 + @"</WMS012A1PageCount>";
                            RetData += "<WMS012A1>" + dtXMLA1 + @"</WMS012A1>";
                            RetData += "<WMS012A2PageCount>" + pageCountA2 + @"</WMS012A2PageCount>";
                            RetData += "<WMS012A2>" + dtXMLA2 + @"</WMS012A2>";
                            RetData += "<WMS012A3PageCount>" + pageCountA3 + @"</WMS012A3PageCount>";
                            RetData += "<WMS012A3>" + dtXMLA3 + @"</WMS012A3>";
                            RetData += "<WMS012A4PageCount>" + pageCountA4 + @"</WMS012A4PageCount>";
                            RetData += "<WMS012A4>" + dtXMLA4 + @"</WMS012A4>";

                        }


                        else
                        {

                            RetData = "不存在数据";
                        }
                    }
                    #endregion


                    #region type=3
                    else if (type == "3")
                    {

                        //                        string sql = @"
                        //SELECT *
                        //FROM WMS012M
                        //WHERE material_no=@code
                        //";


                        //                        Dictionary<string, object> p1 = new Dictionary<string, object>();
                        //                        p1.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt1 = DB.GetDataTable(sql, p1);
                        //                        string dtXML1 = "";
                        //                        if (dt1.Rows.Count > 0)
                        //                        {
                        //                            dtXML1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1);
                        //                        }



                        //                        string sql = @"
                        //SELECT *
                        //FROM WMS012A1
                        //WHERE warehouse=@code
                        //";


                        //                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        //                        p2.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt2 = DB.GetDataTable(sql, p2);
                        //                        string dtXML2 = "";
                        //                        if (dt2.Rows.Count > 0)
                        //                        {
                        //                            dtXML2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);
                        //                        }




                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A2
                        //WHERE warehouse=@code
                        //";


                        //                        Dictionary<string, object> p3 = new Dictionary<string, object>();
                        //                        p3.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt3 = DB.GetDataTable(sql, p3);
                        //                        string dtXML3 = "";
                        //                        if (dt3.Rows.Count > 0)
                        //                        {
                        //                            dtXML3 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3);
                        //                        }


                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A3
                        //WHERE warehouse=@code
                        //";


                        //                        Dictionary<string, object> p4 = new Dictionary<string, object>();
                        //                        p4.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt4 = DB.GetDataTable(sql, p4);
                        //                        string dtXML4 = "";
                        //                        if (dt4.Rows.Count > 0)
                        //                        {
                        //                            dtXML4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4);
                        //                        }

                        //                        sql = @"
                        //SELECT *
                        //FROM WMS012A4
                        //WHERE warehouse=@code
                        //";


                        //                        Dictionary<string, object> p5 = new Dictionary<string, object>();
                        //                        p5.Add("@code", a[i].ToString());
                        //                        System.Data.DataTable dt5 = DB.GetDataTable(sql, p5);

                        //                        string dtXML5 = "";
                        //                        if (dt5.Rows.Count > 0)
                        //                        {
                        //                            dtXML5 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt5);
                        //                        }

                        int start = (Convert.ToInt32(page) - 1) * pageSize + 1;
                        int end = Convert.ToInt32(page) * pageSize;
                        string sql = @"select * from(select WMS012A1.*,material_name,material_specifications,row_number()over(order by WMS012A1.id) as num 
from WMS012A1 
join BASE007M on WMS012A1.material_no = BASE007M.material_no where WMS012A1.warehouse='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt2 = DB.GetDataTable(sql);
                        string dtXMLA1 = "";
                        if (dt2.Rows.Count > 0)
                        {
                            dtXMLA1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);
                        }
                        sql = "select count(*) from WMS012A1 where warehouse='" + code + "'";
                        int pageCount = DB.GetInt32(sql);
                        int pageCountA1 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));

                        sql = @"select * from(select WMS012A2.*,material_name,material_specifications,row_number()over(order by wms012A2.id) as num 
from WMS012A2 
join BASE007M on WMS012A2.material_no = BASE007M.material_no where WMS012A2.warehouse='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt3 = DB.GetDataTable(sql);
                        string dtXMLA2 = "";
                        if (dt3.Rows.Count > 0)
                        {
                            dtXMLA2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3);
                        }

                        sql = "select count(*) from WMS012A2 where warehouse='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA2 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));


                        sql = @"select * from(select WMS012A3.*,material_name,material_specifications,row_number()over(order by wms012A3.id) as num 
from WMS012A3 
join BASE007M on WMS012A3.material_no = BASE007M.material_no where WMS012A3.warehouse='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        System.Data.DataTable dt4 = DB.GetDataTable(sql);
                        string dtXMLA3 = "";
                        if (dt4.Rows.Count > 0)
                        {
                            dtXMLA3 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4);
                        }

                        //WMS012A3的所有页数
                        sql = "select count(*) from WMS012A3 where warehouse='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA3 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));


                        sql = @"select * from(select WMS012A4.*,material_name,material_specifications,row_number()over(order by wms012A4.id) as num 
from WMS012A4 
join BASE007M on WMS012A4.material_no = BASE007M.material_no where WMS012A4.warehouse='" + code + "') as t where t.num>=" + start + " and  t.num<=" + end;
                        string dtXMLA4 = "";
                        System.Data.DataTable dt5 = DB.GetDataTable(sql);
                        if (dt5.Rows.Count > 0)
                        {
                            dtXMLA4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt5);
                        }

                        sql = "select count(*) from WMS012A4 where warehouse='" + code + "'";
                        pageCount = DB.GetInt32(sql);
                        int pageCountA4 = Convert.ToInt32(Math.Ceiling((double)pageCount / Convert.ToInt32(pageSize)));


                        if (dt2.Rows.Count > 0 || dt3.Rows.Count > 0 || dt4.Rows.Count > 0 || dt5.Rows.Count > 0)
                        {
                            //RetData += "<WMS012M>" + dtXML1 + @"</WMS012M>";
                            IsSuccess = true;
                            RetData += "<WMS012A1PageCount>" + pageCountA1 + @"</WMS012A1PageCount>";
                            RetData += "<WMS012A1>" + dtXMLA1 + @"</WMS012A1>";
                            RetData += "<WMS012A2PageCount>" + pageCountA2 + @"</WMS012A2PageCount>";
                            RetData += "<WMS012A2>" + dtXMLA2 + @"</WMS012A2>";
                            RetData += "<WMS012A3PageCount>" + pageCountA3 + @"</WMS012A3PageCount>";
                            RetData += "<WMS012A3>" + dtXMLA3 + @"</WMS012A3>";
                            RetData += "<WMS012A4PageCount>" + pageCountA4 + @"</WMS012A4PageCount>";
                            RetData += "<WMS012A4>" + dtXMLA4 + @"</WMS012A4>";
                        }

                        else
                        {

                            RetData = "不存在数据";
                        }
                    }
                    #endregion

                    else
                    {
                        RetData = "此类型不存在";
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

        public static string GetDataByMaterialAndLocation(object OBJ)
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
                string material = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<material>", "</material>");
                string location = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<location>", "</location>");
                string page = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<page>", "</page>");
                //string BarCodeType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodeType>", "</BarCodeType>");
                #endregion


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                int pageSize = 10;

                #region 逻辑





                string sql = @"select * from(select WMS012A2.*,material_name,material_specifications,row_number()over(order by wms012A2.id) as num 
from WMS012A2 
join BASE007M on WMS012A2.material_no = BASE007M.material_no 
where WMS012A2.location='" + location + "' AND WMS012A2.material_no='" + material + @"') as t where t.num>=" + (Convert.ToInt32(page) - 1) * pageSize + " and  t.num<=" + (Convert.ToInt32(page)) * pageSize;
                System.Data.DataTable dt3 = DB.GetDataTable(sql);
                string dtXMLA2 = "";
                if (dt3.Rows.Count > 0)
                {
                    dtXMLA2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt3);
                }



                sql = "select count(*) from WMS012A2 where WMS012A2.location='" + location + "' AND WMS012A2.material_no='" + material + @"'";
                int pageCount = DB.GetInt32(sql);
                int pageCountA2 = pageCount / pageSize;

                if (pageCount % pageSize > 0)
                {
                    pageCountA2++;
                }



                sql = @"select * from(select WMS012A3.*,material_name,material_specifications,row_number()over(order by wms012A3.id) as num 
from WMS012A3 
join BASE007M on WMS012A3.material_no = BASE007M.material_no 
where WMS012A3.location='" + location + "' AND WMS012A3.material_no='" + material + @"') as t where t.num>=" + (Convert.ToInt32(page) - 1) * pageSize + " and  t.num<=" + (Convert.ToInt32(page)) * pageSize;
                System.Data.DataTable dt4 = DB.GetDataTable(sql);
                string dtXMLA3 = "";
                if (dt4.Rows.Count > 0)
                {
                    dtXMLA3 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt4);
                }

                //WMS012A3的所有页数
                sql = "select count(*) from WMS012A3 where WMS012A3.location='" + location + "' AND WMS012A3.material_no='" + material + @"'";
                pageCount = DB.GetInt32(sql);
                int pageCountA3 = pageCount / pageSize;

                if (pageCount % pageSize > 0)
                {
                    pageCountA3++;
                }





                sql = @"select * from(select WMS012A4.*,material_name,material_specifications,row_number()over(order by wms012A4.id) as num 
from WMS012A4 
join BASE007M on WMS012A4.material_no = BASE007M.material_no 
where WMS012A4.location='" + location + "' AND WMS012A4.material_no='" + material + @"') as t where t.num>=" + (Convert.ToInt32(page) - 1) * pageSize + " and  t.num<=" + (Convert.ToInt32(page)) * pageSize;
                string dtXMLA4 = "";
                System.Data.DataTable dt5 = DB.GetDataTable(sql);
                if (dt5.Rows.Count > 0)
                {
                    dtXMLA4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt5);
                }



                sql = "select count(*) from WMS012A4 where WMS012A4.location='" + location + "' AND WMS012A4.material_no='" + material + @"'";
                pageCount = DB.GetInt32(sql);
                int pageCountA4 = pageCount / pageSize;

                if (pageCount % pageSize > 0)
                {
                    pageCountA4++;
                }






                if (dt3.Rows.Count > 0 || dt4.Rows.Count > 0 || dt5.Rows.Count > 0)
                {
                    //RetData += "<WMS012M>" + dtXML1 + @"</WMS012M>";
                    IsSuccess = true;
                    RetData += "<WMS012A2PageCount>" + pageCountA2 + @"</WMS012A2PageCount>";
                    RetData += "<WMS012A2>" + dtXMLA2 + @"</WMS012A2>";
                    RetData += "<WMS012A3PageCount>" + pageCountA3 + @"</WMS012A3PageCount>";
                    RetData += "<WMS012A3>" + dtXMLA3 + @"</WMS012A3>";
                    RetData += "<WMS012A4PageCount>" + pageCountA4 + @"</WMS012A4PageCount>";
                    RetData += "<WMS012A4>" + dtXMLA4 + @"</WMS012A4>";
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


        internal static string DataEdit(string UserCode, string material_no, double qty, int c, GDSJ_Framework.DBHelper.DataBase DB)//更新WMS012M表
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
            string modifytime = DateTime.Now.ToString("HH:mm:ss");
            p.Add("@material_no", material_no);
            string ErrMsg = string.Empty;

            sql = @"
SELECT qty FROM WMS012M where material_no=@material_no 
";
            System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
            if (dr.Read())//012M
            {
                if (c == 1)
                {
                    qty = Convert.ToDouble (dr[0].ToString()) + Convert.ToDouble(qty);
                }
                else if (c == 0)
                {
                    if (Convert.ToDecimal(dr[0].ToString()) - Convert.ToDecimal(qty) >= 0)
                    {
                        qty = Convert.ToDouble(dr[0].ToString()) - Convert.ToDouble(qty);
                    }
                    else
                    {
                        return ErrMsg = material_no + "物料【物料编号，物料名称，物料规格】库存不足。";
                    }
                }

                p.Add("@qty",Math.Round(qty,2));
                p.Add("@modifyby", UserCode);
                p.Add("@modifydate", modifydate);
                p.Add("@modifytime", modifytime);

                sql = @"
 update WMS012M set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no  and id = (select top 1 id from WMS012M where material_no=@material_no )
";

                int i = DB.ExecuteNonQueryOffline(sql, p);
                if (i < 1)
                {
                    return ErrMsg = "没有更新到数据";
                }
            }
            return ErrMsg = "";
        }

        /// <summary>
        /// 新增12A1表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="DB"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, string location, double qty, string material_specifications, GDSJ_Framework.DBHelper.DataBase DB,string size)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            sql = @"
Insert into WMS012A1([material_no]
      ,[warehouse]
      ,[location]
      ,[qty]
      ,[qty_availble]
      ,[qty_occupied]
      ,[createby]
      ,[createdate]
      ,[createtime]
      ,[UDF01]
      ) values (@material_no,(select top(1) warehouse from BASE011M WHERE location_no=@location),@location,@qty,@qty_availble,@qty_occupied,@createby,@createdate,@createtime,'"+size+@"')
";

            p.Add("@material_no", material_no);
            p.Add("@location", location);
            p.Add("@qty", Math.Round(qty, 2));
            p.Add("@qty_availble", Math.Round(qty, 2));
            p.Add("@qty_occupied", 0);
            p.Add("@createby", UserCode);
            p.Add("@createdate", DateTime.Today.ToString("yyyy-MM-dd"));
            p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));


            int i = DB.ExecuteNonQueryOffline(sql, p);
        }


        /// <summary>
        /// 新增12A1表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="DB"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, string location, double qty,double UDF06,double UDF07, string material_specifications, GDSJ_Framework.DBHelper.DataBase DB, string size)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            sql = @"
Insert into WMS012A1([material_no]
      ,[warehouse]
      ,[location]
      ,[qty]
      ,[qty_availble]
      ,[qty_occupied]
      ,[createby]
      ,[createdate]
      ,[createtime]
      ,[UDF01]
      ,[UDF06]
      ,[UDF07]
      ) values (@material_no,(select top(1) warehouse from BASE011M WHERE location_no=@location),@location,@qty,@qty_availble,@qty_occupied,@createby,@createdate,@createtime,'" + size + @"')
";

            p.Add("@material_no", material_no);
            p.Add("@location", location);
            p.Add("@qty", Math.Round(qty, 2));
            p.Add("@qty_availble", Math.Round(qty, 2));
            p.Add("@qty_occupied", 0);
            p.Add("@createby", UserCode);
            p.Add("@createdate", DateTime.Today.ToString("yyyy-MM-dd"));
            p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));
            p.Add("@UDF06", UDF06);
            p.Add("@UDF07", UDF07);

            int i = DB.ExecuteNonQueryOffline(sql, p);
        }


        /// <summary>
        /// 新增012A2表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="lot_barcode"></param>
        /// <param name="suppliers_lot"></param>
        /// <param name="DB"></param>
        /// <param name="warehouse"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, string location, double qty, string material_specifications, string lot_barcode, string suppliers_lot, GDSJ_Framework.DBHelper.DataBase DB, string warehouse,string size)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            sql = @"
UPDATE WMS012A2 SET qty=qty+"+qty+ @",qty_availble=qty_availble+"+qty+@"
WHERE lot_barcode='"+lot_barcode+@"' and location='"+location+@"' and UDF01='"+size+@"'
";
            if (DB.ExecuteNonQueryOffline(sql) == 0)
            {

                sql = @"
Insert into WMS012A2([material_no]
      ,[warehouse]
      ,[location]
      ,[qty]
      ,[qty_availble]  
      ,[lot_barcode]
      ,[suppliers_lot]
      ,[createby]
      ,[createdate]
      ,[createtime]
      ,[UDF01]
) values (@material_no,@warehouse,@location,@qty,@qty,@lot_barcode,@suppliers_lot,@createby,@createdate,@createtime,'" + size + @"')
";

                p.Add("@material_no", material_no);
                p.Add("@warehouse", warehouse);
                p.Add("@location", location);
                p.Add("@qty", Math.Round(qty, 2));
                p.Add("@lot_barcode", lot_barcode);
                p.Add("@suppliers_lot", suppliers_lot);
                p.Add("@createby", UserCode);
                p.Add("@createdate", DateTime.Today.ToString("yyyy-MM-dd"));
                p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));

                 DB.ExecuteNonQueryOffline(sql, p);

            }

            sql = @"update CODE002M set [status]='6',[modifyby]='" + UserCode + @"',[modifydate]='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',[modifytime]='" + DateTime.Now.ToString("HH:mm:ss") + @"' where lot_barcode='" + lot_barcode + @"' and UDF01='"+size+ @"' and location='" + location + "'";
           DB.ExecuteNonQueryOffline(sql);
        }


        /// <summary>
        /// 新增012A2表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="lot_barcode"></param>
        /// <param name="suppliers_lot"></param>
        /// <param name="DB"></param>
        /// <param name="warehouse"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, string location, double qty,double UDF06, double UDF07, string material_specifications, string lot_barcode, string suppliers_lot, GDSJ_Framework.DBHelper.DataBase DB, string warehouse, string size)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            sql = @"
UPDATE WMS012A2 SET qty=qty+" + qty + @",qty_availble=qty_availble+" + qty + @"
WHERE lot_barcode='" + lot_barcode + @"' and location='" + location + @"' and UDF01='" + size + @"'
";
            if (DB.ExecuteNonQueryOffline(sql) == 0)
            {

                sql = @"
Insert into WMS012A2([material_no]
      ,[warehouse]
      ,[location]
      ,[qty]
      ,[qty_availble]  
      ,[lot_barcode]
      ,[suppliers_lot]
      ,[createby]
      ,[createdate]
      ,[createtime]
      ,[UDF01]
      ,[UDF06]
      ,[UDF07]
) values (@material_no,@warehouse,@location,@qty,@qty,@lot_barcode,@suppliers_lot,@createby,@createdate,@createtime,'" + size + @"')
";

                p.Add("@material_no", material_no);
                p.Add("@warehouse", warehouse);
                p.Add("@location", location);
                p.Add("@qty", Math.Round(qty, 2));
                p.Add("@lot_barcode", lot_barcode);
                p.Add("@suppliers_lot", suppliers_lot);
                p.Add("@createby", UserCode);
                p.Add("@createdate", DateTime.Today.ToString("yyyy-MM-dd"));
                p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));
                p.Add("@UDF06", UDF06);
                p.Add("@UDF07", UDF07);

                DB.ExecuteNonQueryOffline(sql, p);

            }

            sql = @"update CODE002M set [status]='6',[modifyby]='" + UserCode + @"',[modifydate]='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',[modifytime]='" + DateTime.Now.ToString("HH:mm:ss") + @"' where lot_barcode='" + lot_barcode + @"' and UDF01='" + size + @"' and location='" + location + "'";
            DB.ExecuteNonQueryOffline(sql);
        }

        /// <summary>
        ///  新增012A3表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="lot_barcode"></param>
        /// <param name="suppliers_lot"></param>
        /// <param name="packing_barcode"></param>
        /// <param name="DB"></param>
        /// <param name="warehouse"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, string location, double qty, string material_specifications, string lot_barcode, string suppliers_lot, string packing_barcode, GDSJ_Framework.DBHelper.DataBase DB, string warehouse)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            sql = @"
Insert into WMS012A3([material_no]
      ,[location]
      ,[qty]
      ,[qty_availble]
      ,[warehouse]
      ,[lot_barcode]
      ,[suppliers_lot]
      ,[packing_barcode]
      ,[createby]
      ,[createdate]
      ,[createtime] ) values (@material_no,@location,@qty,@qty,@warehouse,@lot_barcode,@suppliers_lot,@packing_barcode,@createby,@createdate,@createtime)
";

            p.Add("@material_no", material_no);
            p.Add("@location", location);
            p.Add("@qty", Math.Round(qty, 2));
            p.Add("@warehouse", warehouse);
            p.Add("@lot_barcode", lot_barcode);
            p.Add("@suppliers_lot", suppliers_lot);
            p.Add("@packing_barcode", packing_barcode);
            p.Add("@createby", UserCode);
            p.Add("@createdate", DateTime.Now.ToString("yyyy-MM-dd"));
            p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));

            int i = DB.ExecuteNonQueryOffline(sql, p);
            string where = string.Empty;
            if (lot_barcode != "")
            {
                where += "and lot_barcode='" + lot_barcode + @"'";
            }
            if (packing_barcode != "")
            {
                where += "and packing_barcode='" + packing_barcode + @"'";
            }



            sql = @"update CODE003M set [status]='6',[location]='" + location + @"',[warehouse]='" + warehouse + @"',[modifyby]='" + UserCode + @"',[modifydate]='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',[modifytime]='" + DateTime.Now.ToString("HH:mm:ss") + @"' where 1=1 " + where + @"";
            i = DB.ExecuteNonQueryOffline(sql);


        }

        /// <summary>
        ///  新增012A4表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="lot_barcode"></param>
        /// <param name="suppliers_lot"></param>
        /// <param name="packing_barcode"></param>
        /// <param name="products_barcode"></param>
        /// <param name="DB"></param>
        /// <param name="warehouse"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, string location, double qty, string material_specifications, string lot_barcode, string suppliers_lot, string packing_barcode, string products_barcode, GDSJ_Framework.DBHelper.DataBase DB, string warehouse)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            sql = @"
Insert into WMS012A4([material_no]
      ,[warehouse]
      ,[location]
      ,[qty]
      ,[qty_availble]
      ,[lot_barcode]
      ,[suppliers_lot]
      ,[packing_barcode]
      ,[products_barcode]
      ,[createby]
      ,[createdate]
      ,[createtime] ) values ('" + material_no + "','" + warehouse + "','" + location + "','" + Math.Round(qty, 2) + "','" + Math.Round(qty, 2) + "','" + lot_barcode + "','" + suppliers_lot + "','" + packing_barcode + "','" + products_barcode + "','" + UserCode + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";

            int i = DB.ExecuteNonQueryOffline(sql);

            sql = @"update CODE001M set [status]='6',[location]='" + location + "',[warehouse]='" + warehouse + "',[modifyby]='" + UserCode + "',[modifydate]='" + DateTime.Now.ToString("yyyy-MM-dd") + "',[modifytime]='" + DateTime.Now.ToString("HH:mm:ss") + "' where products_barcode='" + products_barcode + "'";
            i = DB.ExecuteNonQueryOffline(sql);
        }


        /// <summary>
        /// 新增012M表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="DB"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, double qty, string material_specifications, GDSJ_Framework.DBHelper.DataBase DB)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();
            sql = @"
Insert into WMS012M([material_no]
      ,[material_name]
      ,[material_type]
      ,[qty]
      ,[qty_availble]
      ,[qty_occupied]
      ,[createby]
      ,[createdate]
      ,[createtime]
      ,[material_specifications]) values (@material_no,@material_name,@material_type,@qty,@qty_availble,@qty_occupied,@createby,@createdate,@createtime,@material_specifications)
";

            p.Add("@material_no", material_no);
            p.Add("@material_name", material_name);
            p.Add("@material_type", "");
            p.Add("@qty", Math.Round(qty, 2));
            p.Add("@qty_availble", Math.Round(qty, 2));
            p.Add("@qty_occupied", 0);
            p.Add("@createby", UserCode);
            p.Add("@createdate", DateTime.Now.ToString("yyyy-MM-dd"));
            p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));
            p.Add("@material_specifications", material_specifications);

            int i = DB.ExecuteNonQueryOffline(sql, p);

        }


        /// <summary>
        /// 新增012M表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="qty"></param>
        /// <param name="material_specifications"></param>
        /// <param name="DB"></param>
        internal static void DataAdd(string UserCode, string material_no, string material_name, double qty, double UDF06, double UDF07, string material_specifications, GDSJ_Framework.DBHelper.DataBase DB)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();
            sql = @"
Insert into WMS012M([material_no]
      ,[material_name]
      ,[material_type]
      ,[qty]
      ,[qty_availble]
      ,[qty_occupied]
      ,[createby]
      ,[createdate]
      ,[createtime]
      ,[material_specifications]
      ,[UDF06]
      ,[UDF07]) values (@material_no,@material_name,@material_type,@qty,@qty_availble,@qty_occupied,@createby,@createdate,@createtime,@material_specifications,@UDF06,@UDF07)
";

            p.Add("@material_no", material_no);
            p.Add("@material_name", material_name);
            p.Add("@material_type", "");
            p.Add("@qty", Math.Round(qty, 2));
            p.Add("@qty_availble", Math.Round(qty, 2));
            p.Add("@qty_occupied", 0);
            p.Add("@createby", UserCode);
            p.Add("@createdate", DateTime.Now.ToString("yyyy-MM-dd"));
            p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));
            p.Add("@material_specifications", material_specifications);
            p.Add("@UDF06",UDF06);
            p.Add("@UDF07",UDF07);

            int i = DB.ExecuteNonQueryOffline(sql, p);

        }

        /// <summary>
        /// 修改12A1表的数据
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="c"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        internal static string DataEdit(string UserCode, string material_no, string location, double qty, int c, GDSJ_Framework.DBHelper.DataBase DB,string size)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
            string modifytime = DateTime.Now.ToString("HH:mm:ss");
            p.Add("@material_no", material_no);
            p.Add("@location", location);
            string ErrMsg = string.Empty;
            sql = @"
SELECT qty FROM WMS012A1 where material_no=@material_no and location=@location and UDF01='" + size + @"'
";
            System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
            if (dr.Read())
            {
                if (c == 1)//入库
                {
                    qty = Convert.ToDouble(dr[0].ToString()) + Convert.ToDouble(qty);
                }
                else if (c == 0)//出库
                {
                    if (Convert.ToDecimal(dr[0].ToString()) - Convert.ToDecimal(qty) >= 0)
                    {
                        qty = Convert.ToDouble(dr[0].ToString()) - Convert.ToDouble(qty);
                    }
                    else
                    {
                        return ErrMsg = "物料：" + material_no + "库位：" + location + "库存不足。";
                    }
                }

                p.Add("@qty", Math.Round(qty, 2));
                p.Add("@modifyby", UserCode);
                p.Add("@modifydate", modifydate);
                p.Add("@modifytime", modifytime);


                sql = @"
 update WMS012A1 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and UDF01='"+size+@"'
";

                int i = DB.ExecuteNonQueryOffline(sql, p);
                if (i < 1)
                {
                    return ErrMsg = "没有更新到数据";
                }
                return ErrMsg = "";
            }
            return ErrMsg = "";
        }

        /// <summary>
        /// 删除12A2、12A3、12A4表的数据
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="del"></param>
        /// <param name="DB"></param>
        internal static void DataDel(string barcode, string del, GDSJ_Framework.DBHelper.DataBase DB,String size)
        {
            if (del == "1")
            {
                string sql = string.Empty;
                Dictionary<string, object> p = new Dictionary<string, object>();

                sql = @"
delete from WMS012A2 where lot_barcode=@lot_barcode and UDF01='"+size+@"'
";

                p.Add("@lot_barcode", barcode);

                int i = DB.ExecuteNonQueryOffline(sql, p);

                //sql = @"UPDATE CODE002M SET status='9' where lot_barcode='" + barcode + @"'";
                //DB.ExecuteNonQueryOffline(sql);
            }
            else if (del == "2")
            {
                string sql = string.Empty;
                Dictionary<string, object> p = new Dictionary<string, object>();



                sql = @"
delete from WMS012A3 where packing_barcode=@packing_barcode
";
                p.Add("@packing_barcode", barcode);


                int i = DB.ExecuteNonQueryOffline(sql, p);

                //sql = @"UPDATE CODE003M SET status='9' where packing_barcode='" + barcode + @"'";
                //DB.ExecuteNonQueryOffline(sql);
            }
            else if (del == "3")
            {
                string sql = string.Empty;
                Dictionary<string, object> p = new Dictionary<string, object>();

                sql = @"
delete from WMS012A4 where products_barcode=@products_barcode
";

                p.Add("@products_barcode", barcode);

                int i = DB.ExecuteNonQueryOffline(sql, p);

                //sql = @"UPDATE CODE001M SET status='9' where products_barcode='" + barcode + @"'";
                //DB.ExecuteNonQueryOffline(sql);
            }
        }


        /// <summary>
        /// 调拨
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="material_specifications"></param>
        /// <param name="suppliers_lot"></param>
        /// <param name="location"></param>
        /// <param name="location_target"></param>
        /// <param name="warehouse"></param>
        /// <param name="warehouse_target"></param>
        /// <param name="qty"></param>
        /// <param name="qty_out"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        internal static string DataEdit_diaobo(string UserCode, string material_no, string material_name, string material_specifications, string suppliers_lot, string location, string location_target, string warehouse, string warehouse_target, double qty, double qty_out, GDSJ_Framework.DBHelper.DataBase DB)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();
            double qty2 = qty;
            string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
            string modifytime = DateTime.Now.ToString("HH:mm:ss");
            p.Add("@material_no", material_no);
            p.Add("@location", location);
            p.Add("@warehouse", warehouse);
            string ErrMsg = string.Empty;

            sql = @"
SELECT top 1 qty FROM WMS012A1 where material_no=@material_no  and location=@location and warehouse=@warehouse
";
            System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
            if (dr.Read())
            {
                if (qty != -1)//拨入
                {
                    //decimal a = Convert.ToDecimal(dr[0].ToString());
                    //decimal b = Convert.ToDecimal(qty);
                    //if (Convert.ToDouble(dr[0].ToString()) - Convert.ToDouble(qty_out) >= 0)
                    //{

                    p.Clear();
                    p.Add("@material_no", material_no);
                    p.Add("@location", location);
                    p.Add("@warehouse", warehouse);
                    p.Add("@location_target", location_target);
                    p.Add("@warehouse_target", warehouse_target);

                    sql = @"
SELECT qty FROM WMS012A1 where material_no=@material_no  and location=@location_target and warehouse=@warehouse_target
";

                    double qty3 = 0;
                    qty3 = DB.GetDouble(sql, p);

                    p.Add("@modifyby", UserCode);
                    p.Add("@modifydate", modifydate);
                    p.Add("@modifytime", modifytime);
                    if (qty3 != 0)
                    {
                        #region 目标库位有该物料库存 编辑，加上入库数据
                        p.Add("@qty_occupied", Convert.ToDecimal(qty));
                        qty = qty3 + Convert.ToDouble(qty);

                        p.Add("@qty", Math.Round(qty, 2));

                        //qty_occupied = qty_occupied - @qty_occupied
                        sql = @"
 update WMS012A1 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location_target and warehouse=@warehouse_target
";
                        DB.ExecuteNonQueryOffline(sql, p);
                        #endregion
                    }
                    else
                    {
                        #region 目标库位没有该物料的库存，新增，入库数据
                        sql = @"
INSERT INTO WMS012A1
(material_no,location,warehouse,qty,qty_availble,createby,createdate,createtime,qty_occupied)
VALUES
(@material_no,@location_target,@warehouse_target,@qty,@qty,'" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','0')
";
                        p.Add("@qty", Math.Round(qty, 2));
                        p.Add("@qty_occupied", Convert.ToDecimal(qty));
                        int j = DB.ExecuteNonQueryOffline(sql, p);
                        #endregion

                    }

                    #region 更新原库位的占用数量
                    p.Add("@qty_occupied_2", Math.Round(qty2, 2));
                    //DB.ExecuteNonQueryOffline(sql, p);
                    sql = @"
 update WMS012A1 set qty_occupied=isnull(qty_occupied,0)-@qty_occupied_2,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location
";
                    int i = DB.ExecuteNonQueryOffline(sql, p);
                    #endregion
                    //}
                    //else
                    //{
                    //    return ErrMsg = "物料：" + material_no + "库位：" + location + "库存不足，无法调拨。";
                    //}

                }
                else//拨出
                {
                    #region 原库位

                    decimal qty_out_2 = Convert.ToDecimal(dr[0].ToString()) - Convert.ToDecimal(qty_out);

                    p.Add("@qty",Math.Round(qty_out_2));
                    p.Add("@modifyby", UserCode);
                    p.Add("@qty_occupied",Math.Round(qty_out));
                    p.Add("@modifydate", modifydate);
                    p.Add("@modifytime", modifytime);

                    sql = @"
 update WMS012A1 set qty=@qty,qty_availble=@qty,qty_occupied=isnull(qty_occupied,0)+@qty_occupied,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location
";
                    int i = DB.ExecuteNonQueryOffline(sql, p);
                    #endregion
                }
            }
            return ErrMsg = "";
        }

        /// <summary>
        /// 盘点
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="material_no"></param>
        /// <param name="material_name"></param>
        /// <param name="material_specifications"></param>
        /// <param name="location"></param>
        /// <param name="warehouse"></param>
        /// <param name="qty"></param>
        /// <param name="DB"></param>
        internal static void DataAdd_pandian(string UserCode, string material_no, string material_name, string material_specifications, string location, string warehouse, double qty, GDSJ_Framework.DBHelper.DataBase DB,string size)
        {
            string sql = string.Empty;
           
            Dictionary<string, object> p = new Dictionary<string, object>();
           

            p.Add("@material_no", material_no);
            p.Add("@location", location);


            
            p.Add("@modifyby", UserCode);

            p.Add("@modifydate", DateTime.Today.ToString("yyyy-MM-dd"));
            p.Add("@modifytime", DateTime.Now.ToString("HH:mm:ss"));

            if (qty > 0)
            {
                p.Add("@qty", Math.Round(qty, 2));
                sql = @"
 update WMS012A1 set qty=qty+@qty,qty_availble=qty_availble+@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and UDF01='"+size+@"'
";

            }
            else
            {
                p.Add("@qty", -Math.Round(qty, 2));
                sql = @"
 update WMS012A1 set qty=qty-@qty,qty_availble=qty_availble-@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and UDF01='"+size+@"'
";
            }

            if(DB.ExecuteNonQueryOffline(sql, p)==0)
            {
                sql = @"
INSERT INTO WMS012A1
(material_no,warehouse,location,qty,qty_availble,qty_occupied,UDF01)
VALUES
('"+material_no+@"','"+warehouse+@"','"+location+@"','"+ Math.Round(qty, 2) + @"','"+ Math.Round(qty, 2) + @"','0','"+size+@"')
";
                DB.ExecuteNonQueryOffline(sql);
            }

        }


        internal static string DataEdit(string UserCode, string barcode, string material_no, string location, double qty, GDSJ_Framework.DBHelper.DataBase DB, int codetype,string size)
        {

            string ErrMsg = string.Empty;
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();

            p.Add("@material_no", material_no);
            p.Add("@location", location);

            string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
            string modifytime = DateTime.Now.ToString("HH:mm:ss");

            p.Add("@qty", Math.Round(qty, 2));
            p.Add("@barcode", barcode);
            p.Add("@modifyby", UserCode);
            p.Add("@modifydate", modifydate);
            p.Add("@modifytime", modifytime);

            try
            {
                switch (codetype)
                {
                    //单品
                    case 1:
                        sql = @"
 update WMS012A4 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location
";
                        DB.ExecuteNonQueryOffline(sql, p);
                        break;

                    //批号
                    case 2:
                        sql = @"
 update WMS012A2 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,
modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and UDF01='"+size+@"'
and lot_barcode=@barcode ";
                        DB.ExecuteNonQueryOffline(sql, p);

                        sql = @"
 update CODE002M set qty=@qty,modifyby=@modifyby,
modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and UDF01='" + size + @"'
and lot_barcode=@barcode ";
                        DB.ExecuteNonQueryOffline(sql, p);
                        break;

                    //箱号
                    case 3:
                        sql = @"
 update WMS012A3 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,
modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location 
and packing_barcode=@barcode ";
                        DB.ExecuteNonQueryOffline(sql, p);
                        break;
                }
                return ErrMsg = material_no + "更新成功";
            }
            catch (Exception ex)
            {
                return ErrMsg = material_no + ex.ToString();
            }

        }



        /// <summary>
        /// 编辑12A2、12A3、12A4表的数据
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="lot_barcode"></param>
        /// <param name="material_no"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="DB"></param>
        /// <param name="flat"></param>
        /// <returns></returns>
        internal static string DataEdit_barcode(string UserCode, string barcode, string material_no, string location, string location_target, string warehouse, string warehouse_target, double qty, double qty_out, GDSJ_Framework.DBHelper.DataBase DB, string barcode_type)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();
            double qty2 = qty;
            string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
            string modifytime = DateTime.Now.ToString("HH:mm:ss");
            p.Add("@material_no", material_no);
            p.Add("@location", location);
            p.Add("@warehouse", warehouse);
            p.Add("@barcode", barcode);
            string ErrMsg = string.Empty;

            if (barcode_type == "2")
            {
                p.Add("@lot_barcode", barcode);
                sql = @"SELECT top 1 qty FROM WMS012A2 where lot_barcode=@lot_barcode and location=@location and material_no=@material_no ";
            }
            else if (barcode_type == "3")
            {
                p.Add("@packing_barcode", barcode);
                sql = @"SELECT top 1 qty FROM WMS012A3 where packing_barcode=@packing_barcode and location=@location and material_no=@material_no";
            }
            else if (barcode_type == "1")
            {
                p.Add("@products_barcode", barcode);
                sql = @"SELECT top 1 qty FROM WMS012A4 where products_barcode=@products_barcode and location=@location and material_no=@material_no";
            }

            System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
            if (dr.Read())
            {
                if (qty != -1)//拨入
                {
                    //if (Convert.ToDouble(dr[0].ToString()) - Convert.ToDouble(qty_out) >= 0)
                    //{
                    //p.Clear();
                    //p.Add("@material_no", material_no);
                    //p.Add("@location", location);
                    //p.Add("@warehouse", warehouse);
                    p.Add("@location_target", location_target);
                    p.Add("@warehouse_target", warehouse_target);
                    if (barcode_type == "1")
                    {
                        sql = @"
SELECT qty FROM WMS012A4 where material_no=@material_no  and location=@location_target and warehouse=@warehouse_target and products_barcode=@products_barcode
";
                    }
                    else if (barcode_type == "2")
                    {
                        sql = @"
SELECT qty FROM WMS012A2 where material_no=@material_no  and location=@location_target and warehouse=@warehouse_target and lot_barcode=@lot_barcode
";
                    }
                    else if (barcode_type == "3")
                    {
                        sql = @"
SELECT qty FROM WMS012A3 where material_no=@material_no  and location=@location_target and warehouse=@warehouse_target and packing_barcode=@packing_barcode
";
                    }

                    double qty3 = 0;
                    qty3 = DB.GetDouble(sql, p);
                    p.Add("@modifyby", UserCode);
                    p.Add("@modifydate", modifydate);
                    p.Add("@modifytime", modifytime);
                    if (qty3 != 0)
                    {
                        #region 目标库位有该物料库存 编辑，加上入库数据
                        p.Add("@qty_occupied", Convert.ToDecimal(qty));
                        qty = qty3 + Convert.ToDouble(qty);

                        p.Add("@qty", Math.Round(qty, 2));

                        //qty_occupied = qty_occupied - @qty_occupied
                        if (barcode_type == "1")
                        {
                            sql = @"
 update WMS012A4 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location_target and warehouse=@warehouse_target and products_barcode=@products_barcode
";
                        }
                        else if (barcode_type == "2")
                        {
                            sql = @"
 update WMS012A2 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location_target and warehouse=@warehouse_target and lot_barcode=@lot_barcode
";
                        }
                        else if (barcode_type == "3")
                        {
                            sql = @"
 update WMS012A3 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location_target and warehouse=@warehouse_target and packing_barcode=@packing_barcode
";
                        }

                        DB.ExecuteNonQueryOffline(sql, p);
                        #endregion
                    }
                    else
                    {
                        #region 目标库位没有该物料的库存，新增，入库数据
                        if (barcode_type == "1")
                        {
                            sql = @"
INSERT INTO WMS012A4
(products_barcode,material_no,location,warehouse,qty,qty_availble,createby,createdate,createtime,qty_occupied)
VALUES
(@products_barcode,@material_no,@location_target,@warehouse_target,@qty,@qty,'" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','0')
";
                        }
                        else if (barcode_type == "2")
                        {
                            sql = @"
INSERT INTO WMS012A2
(lot_barcode,material_no,location,warehouse,qty,qty_availble,createby,createdate,createtime,qty_occupied)
VALUES
(@lot_barcode,@material_no,@location_target,@warehouse_target,@qty,@qty,'" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','0')
";
                        }
                        else if (barcode_type == "3")
                        {
                            sql = @"
INSERT INTO WMS012A3
(packing_barcode,material_no,location,warehouse,qty,qty_availble,createby,createdate,createtime,qty_occupied)
VALUES
(@packing_barcode,@material_no,@location_target,@warehouse_target,@qty,@qty,'" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','0')
";
                        }

                        p.Add("@qty", Math.Round(qty, 2));
                        p.Add("@qty_occupied", Convert.ToDecimal(qty));
                        int j = DB.ExecuteNonQueryOffline(sql, p);
                        #endregion

                    }

                    #region 更新原库位的占用数量
                    p.Add("@qty_occupied_2", Math.Round(qty2, 2));
                    //DB.ExecuteNonQueryOffline(sql, p);
                    if (barcode_type == "1")
                    {
                        sql = @"
 delete from WMS012A4 where products_barcode=@products_barcode and material_no=@material_no and location=@location
";
                    }
                    else if (barcode_type == "2")
                    {
                        sql = @"
 update WMS012A2 set qty_occupied=isnull(qty_occupied,0)-@qty_occupied_2,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and lot_barcode=@lot_barcode
";
                    }
                    else if (barcode_type == "3")
                    {
                        sql = @"
 update WMS012A3 set qty_occupied=isnull(qty_occupied,0)-@qty_occupied_2,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and packing_barcode=@packing_barcode
";
                    }

                    int i = DB.ExecuteNonQueryOffline(sql, p);
                    #endregion
                    //}
                    //else
                    //{
                    //    return ErrMsg = "物料：" + material_no + "库位：" + location + "库存不足，无法调拨。";
                    //}

                }
                else//拨出
                {
                    #region 原库位

                    decimal qty_out_2 = Convert.ToDecimal(dr[0].ToString()) - Convert.ToDecimal(qty_out);

                    p.Add("@qty",Math.Round( qty_out_2));
                    p.Add("@modifyby", UserCode);
                    p.Add("@qty_occupied",Math.Round( qty_out,2));
                    p.Add("@modifydate", modifydate);
                    p.Add("@modifytime", modifytime);
                    //p.Add("@barcode", barcode);

                    if (barcode_type == "1")
                    {
                        sql = @"
 update WMS012A4 set qty=@qty,qty_availble=@qty,qty_occupied=isnull(qty_occupied,0)+@qty_occupied,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and products_barcode=@products_barcode
";
                    }
                    else if (barcode_type == "2")
                    {
                        sql = @"
 update WMS012A2 set qty=@qty,qty_availble=@qty,qty_occupied=isnull(qty_occupied,0)+@qty_occupied,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and lot_barcode=@lot_barcode
";
                    }
                    else if (barcode_type == "3")
                    {
                        sql = @"
 update WMS012A3 set qty=@qty,qty_availble=@qty,qty_occupied=isnull(qty_occupied,0)+@qty_occupied,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and packing_barcode=@packing_barcode
";
                    }

                    int i = DB.ExecuteNonQueryOffline(sql, p);
                    #endregion
                }
            }

            return ErrMsg = material_no + "更新成功";
        }
    }
}
