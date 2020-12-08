using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GDSJ_Framework.DBHelper;

namespace SJEMS_WBAPI
{
    class WMS008
    {
        public static string Audit(object OBJ, DataTable dtBarCode)
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
                DllName = "SJEMS_WBAPI";
                ClassName = "SJESM_WBAPI.WMS008";
                Method = "Audit";
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");
                string DBTYPE = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBTYPE>", "</DBTYPE>");
                string DBSERVER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBSERVER>", "</DBSERVER>");
                string DBNAME = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBNAME>", "</DBNAME>");
                string DBUSER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBUSER>", "</DBUSER>");
                string DBPASSWORD = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBPASSWORD>", "</DBPASSWORD>");


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);
                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 接口参数
                string moveout_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<moveout_plan>", "</moveout_plan>");
              
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                #endregion

                #region 

                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS003"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {


                    bool IsOk = false;
                    string ErrMsg = string.Empty;


                    //2.查询WMS003A2看是否有没有审核过的数据，如果没有，报错：该计划没有新的入库资料，不能进行审核。

                    #region 逻辑
                    string sql = @"
SELECT COUNT(*)
FROM WMS008A2
WHERE moveout_plan=@moveout_plan and status='1'
";
                    Dictionary<string, object> p1 = new Dictionary<string, object>();
                    p1.Add("@moveout_plan", moveout_plan);
                    int rowNumber = DB.GetInt32(sql, p1);

                    if (rowNumber == 0)  //报错
                    {
                        IsOk = false;
                        ErrMsg = "该计划没有新的出库资料，不能进行审核";
                    }
                    else //未审核
                    {
                        #region 产生出库单号
                        sql = @"
SELECT MAX(moveout_doc) FROM WMS009M
";
                        string moveout_doc = DB.GetString(sql);

                        if (moveout_doc.IndexOf(DateTime.Now.ToString("yyyyMMdd")) > -1)
                        {
                            moveout_doc = DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(moveout_doc.Replace(DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");
                        }
                        else
                        {
                            moveout_doc = DateTime.Now.ToString("yyyyMMdd") + "0001";
                        }
                        #endregion

                        sql = @"
SELECT 
moveout_plan,
moveout_type,
front_document_type,
front_document,
status,
dosureby,
dosuredatetime,
auditby,
auditdatetime from WMS008M
where moveout_plan=@moveout_plan 

";
                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        p2.Add("@moveout_plan", moveout_plan);
                        System.Data.IDataReader idr = DB.GetDataTableReader(sql, p2);
                        while (idr.Read())
                        {

                            string front_document_type = idr["front_document_type"].ToString();
                            string front_document = idr["front_document"].ToString();
                            string dosureby = idr["dosureby"].ToString();
                            string dosuredatetime = idr["dosuredatetime"].ToString();
                            string auditby = idr["auditby"].ToString();
                            string auditdatetime = idr["auditdatetime"].ToString();
                            string moveout_type = idr["moveout_type"].ToString();

                            sql = @"
INSERT INTO WMS009M(moveout_doc,front_document_type,front_document,moveout_type,status,createby,createdate,createtime)
values
('" + moveout_doc + "','11','" + moveout_plan + @"','" + moveout_type + @"','1','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')
";

                            DB.ExecuteNonQueryOffline(sql);

                            sql = @"
SELECT 
moveout_plan,
serial_number,
material_no,
material_name,
material_var,
material_specifications,
suppliers_lot,
lot_barcode,qty,
location,
UDF01,UDF02,UDF06,UDF07
from WMS008A2 
WHERE moveout_plan=@moveout_plan AND status<>'2'
";

                            Dictionary<string, object> p3 = new Dictionary<string, object>();
                            p3.Add("@moveout_plan", moveout_plan);
                            System.Data.IDataReader idr2 = DB.GetDataTableReader(sql, p3);
                            while (idr2.Read())
                            {
                                string serial_number = idr2["serial_number"].ToString();
                                string material_no = idr2["material_no"].ToString();
                                string material_name = idr2["material_name"].ToString();
                                string material_var = idr2["material_var"].ToString();
                                string material_specifications = idr2["material_specifications"].ToString();
                                string suppliers_lot = idr2["suppliers_lot"].ToString();
                                string lot_barcode = idr2["lot_barcode"].ToString();
                                string qty = idr2["qty"].ToString();
                                string location = idr2["location"].ToString();
                                string UDF01 = idr2["UDF01"].ToString();
                                string UDF02 = idr2["UDF02"].ToString();
                                string UDF06 = idr2["UDF06"].ToString();
                                string UDF07 = idr2["UDF07"].ToString();

                                sql = @"
INSERT INTO WMS009A1(moveout_doc,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,qty,location,createby,createdate,createtime,UDF01,UDF02,UDF06,UDF07)
values
('" + moveout_doc + "','" + serial_number + @"','" + material_no + @"','" + material_name + @"','" + material_var + "','" + material_specifications + @"','" + suppliers_lot + @"','" + lot_barcode + @"','" + qty + @"','" + location + @"','" + material_no + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + UDF01 + @"','" + UDF02 + @"','" + UDF06 + @"','" + UDF07 + @"')
";

                                DB.ExecuteNonQueryOffline(sql);



                            }

                            sql = @"
 Update WMS008A2 set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "'where moveout_plan='" + moveout_plan + @"'";
                            DB.ExecuteNonQueryOffline(sql);




                            #region 审核8M表
                            sql = @"
select 
* 
 from WMS008A1 
where moveout_plan='" + moveout_plan + @"'
AND qty<>qty_plan
AND UDF03<>UDF06
AND UDF04<>UDF07";



                            if (!DB.GetDataTableReader(sql).Read())
                            {
                                sql = @"
 Update WMS008M set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' where moveout_plan='" + moveout_plan + @"'";
                                DB.ExecuteNonQueryOffline(sql);

                            }
                            #endregion
                        }


                        foreach (DataRow dr in dtBarCode.Rows)
                        {
                            string barcode = dr["code"].ToString();

                            string UDF01 = dr["UDF01"].ToString();
                            string UDF02 = dr["UDF02"].ToString();

                            string warehouse = DB.GetString("select warehouse from BASE011M(NOLOCK) WHERE location_no ='" + dr["location"].ToString() + @"'");

                            string qty = string.Empty;
                            string UDF06 = string.Empty;
                            string UDF07 = string.Empty;

                            if (dr["UDF01"].ToString() != "" && dr["UDF01"].ToString().ToUpper() != "NO SIZE")
                            {
                                qty = "0";
                                UDF06 = dr["UDF03"].ToString();
                                UDF07 = dr["UDF04"].ToString();
                            }
                            else
                            {
                                qty = dr["UDF03"].ToString();
                                UDF06 = "0";
                                UDF07 = "0";
                            }
                            sql = @"
INSERT INTO CODE002A3
(lot_barcode,documents_type,documents,operation,warehouse_target,location_target,qty,UDF01,UDF02,UDF06,UDF07,createby,createdate,createtime,isdelete)
VALUES
('" + dr["code"].ToString() + @"','12','" + moveout_doc + @"','9','" + warehouse + @"','" + dr["location"].ToString() + @"','" + qty + @"','" + dr["UDF01"].ToString() + @"','" + dr["UDF02"].ToString() + @"','" + UDF06 + @"','" + UDF07 + @"','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','False')
";
                            DB.ExecuteNonQueryOffline(sql);


                        }

                        IsOk = true;

                        string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS009DoSure'");
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
                <moveout_doc>" + moveout_doc + @"</moveout_doc>
                </Data>
                
            </WebService>
";
                            ret = WMS009.Audit(s);
                            return ret;
                        }
                    }

                    IsSuccess = true;
                    RetData += "<" + moveout_plan + @">";
                    RetData += "<IsOk>" + IsOk + @"</IsOk>";
                    RetData += "<ErrMsg>" + ErrMsg + @"</ErrMsg>";
                    RetData += "</" + moveout_plan + @">";

                    #endregion


                }
                #endregion

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message ;

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
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
                string moveout_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<moveout_plan>", "</moveout_plan>");

                #endregion

                string where = string.Empty;

                if (moveout_plan.Contains(","))
                {
                    string[] tmpStr = moveout_plan.Split(',');
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
                    where = "'" + moveout_plan + @"'";
                }

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 逻辑
                string sql = @"
SELECT *
FROM WMS008M
WHERE moveout_plan in (" + where + @")
";
                string sql1 = @"
SELECT *
FROM WMS008A1
WHERE moveout_plan in (" + where + @")
";
                string sql2 = @"
SELECT *
FROM WMS008A2
WHERE moveout_plan in (" + where + @")
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
                    RetData = "<WMS008M>" + dtXML + @"</WMS008M>" + "<WMS008A1>" + dtXML1 + @"</WMS008A1>" + "<WMS008A2>" + dtXML2 + @"</WMS008A2>";
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
     
        public static string WBDoWork(object OBJ)
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
                DllName = "SJEMS_WBAPI";
                ClassName = "SJESM_WBAPI.WMS008";
                Method = "WBDoWork";
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");

                #region 接口参数
                string moveout_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<moveout_plan>", "</moveout_plan>");//入库计划
                
                string PlanDetails = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<PlanDetails>", "</PlanDetails>");
                string BarCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");//汇总出库
                #endregion

                DataTable dtPlanDetails = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(PlanDetails);
                DataTable dtBarCode = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(BarCode);


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);
                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
select top(1)id from WMS008A1(NOLOCK)
WHERE moveout_plan='" + moveout_plan + @"'
";
                #region 逻辑
                if (!string.IsNullOrEmpty(DB.GetString(sql)))
                {
                    Dictionary<string, string> tmp = new Dictionary<string, string>();
                    foreach(DataRow dr in dtBarCode.Rows)
                    {
                        if(tmp.ContainsKey(dr["material_no"].ToString() + @";" + dr["UDF01"].ToString() + @";"+dr["location"].ToString()))
                        {
                            string s = tmp[dr["material_no"].ToString() + @";" + dr["UDF01"].ToString() + @";" + dr["location"].ToString()];

                            double qty = Convert.ToDouble(s.Split(';')[0]) + Convert.ToDouble(dr["Qty"].ToString());
                            double UDF06= Convert.ToDouble(s.Split(';')[1]) + Convert.ToDouble(dr["UDF03"].ToString());
                            double UDF07 = Convert.ToDouble(s.Split(';')[2]) + Convert.ToDouble(dr["UDF07"].ToString());

                            tmp[dr["material_no"].ToString() + @";" + dr["UDF01"].ToString() + @";" + dr["location"].ToString()] =
                                qty.ToString() + ";" + UDF06.ToString() + @";" + UDF07.ToString();
                        }
                        else
                        {
                            double qty =  Convert.ToDouble(dr["Qty"].ToString());
                            double UDF06 = Convert.ToDouble(dr["UDF03"].ToString());
                            double UDF07 =  Convert.ToDouble(dr["UDF07"].ToString());

                            tmp[dr["material_no"].ToString() + @";" + dr["UDF01"].ToString() + @";" + dr["location"].ToString()] =
                                qty.ToString() + ";" + UDF06.ToString() + @";" + UDF07.ToString();
                        }
                    }

                    #region 更新WMS008A1并产生WMS008A2
                    foreach (DataRow dr in dtPlanDetails.Rows)
                    {
                        string material_name=string.Empty;
                        string material_specifications=string.Empty;
                        string qty_plan=string.Empty;
                        string UDF01 = string.Empty;
                        string UDF02 = string.Empty;
                        string UDF03 = string.Empty;
                        string UDF04 = string.Empty;
                      

                        if (dr["UDF01"].ToString() != "" && dr["UDF01"].ToString().ToUpper() != "NO SIZE")
                        {
                            

                            sql = @"
SELECT * FROM WMS008A1(NOLOCK)
WHERE
moveout_plan='" + moveout_plan + @"' 
AND material_no='" + dr["material_no"].ToString() + @"'
AND UDF01='" + dr["UDF01"] + @"'
AND UDF02='" + dr["UDF02"] + @"'
";
                            IDataReader idr = DB.GetDataTableReader(sql);
                            if(idr.Read())
                            {
                                material_name = idr["material_name"].ToString();
                                material_specifications = idr["material_specifications"].ToString();
                                qty_plan = (Convert.ToDouble(idr["qty_plan"].ToString())-Convert.ToDouble(idr["qty"].ToString())).ToString("0.00");
                                UDF01 = idr["UDF01"].ToString();
                                UDF02 = idr["UDF02"].ToString();
                                UDF03 = (Convert.ToDouble(idr["UDF03"].ToString()) - Convert.ToDouble(idr["UDF06"].ToString())).ToString("0.00");
                                UDF04 = (Convert.ToDouble(idr["UDF04"].ToString()) - Convert.ToDouble(idr["UDF07"].ToString())).ToString("0.00");
                              
                            }

                            sql = @"
Update WMS008A1 SET UDF06=UDF06+" + dr["UDF08"].ToString() + @",UDF07=UDF07+" + dr["UDF09"].ToString() + @"
WHERE 
moveout_plan='" + moveout_plan + @"' 
AND material_no='" + dr["material_no"].ToString() + @"'
AND UDF01='" + dr["UDF01"] + @"'
AND UDF02='" + dr["UDF02"] + @"'
";
                            DB.ExecuteNonQueryOffline(sql);
                        }
                        else
                        {

                            sql = @"
SELECT * FROM WMS008A1(NOLOCK)
WHERE
moveout_plan='" + moveout_plan + @"' 
AND material_no='" + dr["material_no"].ToString() + @"'
";
                            IDataReader idr = DB.GetDataTableReader(sql);
                            if (idr.Read())
                            {
                                material_name = idr["material_name"].ToString();
                                material_specifications = idr["material_specifications"].ToString();
                                qty_plan = (Convert.ToDouble(idr["qty_plan"].ToString()) - Convert.ToDouble(idr["qty"].ToString())).ToString("0.00");
                                UDF01 = idr["UDF01"].ToString();
                                UDF02 = idr["UDF02"].ToString();
                                UDF03 = (Convert.ToDouble(idr["UDF03"].ToString()) - Convert.ToDouble(idr["UDF06"].ToString())).ToString("0.00");
                                UDF04 = (Convert.ToDouble(idr["UDF04"].ToString()) - Convert.ToDouble(idr["UDF07"].ToString())).ToString("0.00");
                               

                            }

                            sql = @"
Update WMS008A1 SET qty=qty+" + dr["Qty"].ToString() + @"
WHERE 
moveout_plan='" + moveout_plan + @"' 
AND material_no='" + dr["material_no"].ToString() + @"'
";
                            DB.ExecuteNonQueryOffline(sql);
                        }


                        foreach (string key in tmp.Keys)
                        {
                            if (key.IndexOf(dr["material_no"].ToString() + ";" + dr["UDF01"].ToString()) > -1)
                            {
                                string serial_number = string.Empty;

                                string location = string.Empty;
                                string qty = string.Empty;
                                string UDF06 = string.Empty;
                                string UDF07 = string.Empty;

                                location = key.Split(';')[2];
                                qty = tmp[key].Split(';')[0];
                                UDF06 = tmp[key].Split(';')[1];
                                UDF07 = tmp[key].Split(';')[2];

                                sql = @"
SELECT ISNULL(MAX(serial_number,'000') FROM WMS008A2(NOLOCK)
WHERE moveout_plan='" + dr["moveout_plan"].ToString() + @"'
";
                                serial_number = DB.GetString(sql);

                                serial_number = (Convert.ToInt32(serial_number) + 1).ToString("0000");




                                sql = @"
INSERT INTO WMS008A2
(moveout_plan,serial_number,location,material_no,material_name,material_specifications,qty_plan,qty,UDF01,UDF02,UDF03,UDF04,UDF06,UDF07,createby,createdate,createtime,isdelete,status)
VALUES
('" + moveout_plan + @"','" + serial_number + @"','"+ location+@"','" + dr["material_no"].ToString() + @"','" + material_name + @"','" + material_specifications + @"','" + qty_plan + @"','" + qty + @"','" + UDF01 + @"','" + UDF02 + @"','" + UDF03 + @"','" + UDF04 + @"','" +UDF06 + @"','" +UDF07 + @"','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','False','1')
";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                        }
                    }
                    #endregion


                    #region 添加条码操作记录
                    foreach(DataRow dr in dtBarCode.Rows)
                    {
                        string warehouse = DB.GetString("select warehouse from BASE011M(NOLOCK) WHERE location_no ='" + dr["location"].ToString() + @"'");

                        string qty = string.Empty;
                        string UDF06 = string.Empty;
                        string UDF07 = string.Empty;

                        if (dr["UDF01"].ToString() != "" && dr["UDF01"].ToString().ToUpper() != "NO SIZE")
                        {
                            qty = "0";
                            UDF06 = dr["UDF03"].ToString();
                            UDF07 = dr["UDF04"].ToString();
                        }
                        else
                        {
                            qty = dr["UDF03"].ToString();
                            UDF06 = "0";
                            UDF07 = "0";
                        }
                        sql = @"
INSERT INTO CODE002A3
(lot_barcode,documents_type,documents,operation,warehouse_target,location_target,qty,UDF01,UDF02,UDF06,UDF07,createby,createdate,createtime,isdelete)
VALUES
('" + dr["code"].ToString() + @"','12','" + moveout_plan + @"','9','" + warehouse + @"','" + dr["location"].ToString() + @"','" + qty + @"','" + dr["UDF01"].ToString() + @"','" + dr["UDF02"].ToString() + @"','" + UDF06 + @"','" + UDF07 + @"','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','False')
";
                        DB.ExecuteNonQueryOffline(sql);
                    }

                    #endregion

                    //5.获取系统参数SYS002M,代号为WMS008TOWMS009，如果值为Auto,调用WMS008.DoSure方法，创建入库单
                    string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS008TOWMS009'");
                    if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                    {
                        ret = Audit(OBJ, dtBarCode);
                        return ret;
                    }
                }

                else
                {
                    RetData = "出库计划不存在";
                }
                #endregion




                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }


          

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return> 
                    <storage_doc>" + RetData + "</storage_doc>" + msg + @"
                </Return>
            </WebService>
            ";

            return ret;
        }

      
    }
}
