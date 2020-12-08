using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class MES010
    {
        //获取工单资料
        public static string GetProductionOrder(object OBJ)
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
                string ProductionOrder = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<ProductionOrder>", "</ProductionOrder>");

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                #region 逻辑
                string sql = @"
SELECT * FROM MES010M WHERE production_order = '" + ProductionOrder + @"'";
                string sql1 = @"
SELECT * FROM MES010A1 WHERE production_order = '" + ProductionOrder + @"'";
                string sql2 = @"
select (select sorting from MES010A1 where MES010A1.production_order=MES010A5.production_order and MES010A1.procedure_no=MES010A5.procedure_no) AS 'sorting',* from MES010A5
 WHERE production_order = '" + ProductionOrder + @"'";
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
                    RetData = "<MES010M>" + dtXML + @"</MES010M>" + "<MES010A1>" + dtXML1 + @"</MES010A1>" + "<MES010A5>" + dtXML2 + @"</MES010A5>";
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

        //提交移转数据
        public static string DoWorkProcedure(object OBJ)
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
            string sql = string.Empty;
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
                string ProductionOrder = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<ProductionOrder>", "</ProductionOrder>");
                string procedure_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<procedure_no>", "</procedure_no>");
                string qty = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<qty>", "</qty>");
                string qty2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<qty2>", "</qty2>");
                string qty3 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<qty3>", "</qty3>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                #endregion
                string ErrMsg = string.Empty;
                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);
                string ProductionOver = DB.GetString("SELECT top(1) parameters_value FROM SYS002M where parameters_code='ProductionOver'");

                double qtyA5 = 0;
                double qtyM = 0;
                if (ProductionOver == "True")//为True不进行管控
                {
                    ErrMsg = "1";
                }
                else//否则检查MES010A5的qty总数不能超出MES010M的计划数量，提示生产总数超出
                {
                    qtyA5 = DB.GetDouble("SELECT ISNULL(SUM(qty),0) FROM MES010A5 where production_order='" + ProductionOrder + "' and procedure_no='" + procedure_no + "'");
                    qtyM =DB.GetDouble("SELECT ISNULL(SUM(qty),0) FROM MES010M where production_order='" + ProductionOrder + "'");
                    if (qtyA5 + double.Parse(qty) > qtyM)
                    {
                        ErrMsg = "生产总数超出";
                        IsSuccess = false;
                    }
                    else
                    {
                        ErrMsg = "1";
                    }
                }
                if (ErrMsg == "1")
                {
                    //2.产生MES010A5的数据		
                    sql = @"INSERT INTO MES010A5 (production_order,procedure_no,procedure_name,procedure_description,qty,qty2,qty3,createby,createdate,createtime) VALUES ('" + ProductionOrder + "','" + procedure_no + "',(SELECT procedure_name FROM MES001M WHERE procedure_no='" + procedure_no + "'),(SELECT procedure_description FROM MES001M WHERE procedure_no='" + procedure_no + "'),'" + double.Parse(qty) + "','" + double.Parse(qty2) + "','" + double.Parse(qty3) + "','"+ UserCode + "','"+ DateTime.Now.ToShortDateString().ToString() + "','"+ DateTime.Now.ToLongTimeString().ToString() + "')";
                    //3.更新MES010M的qty_bad=qty_bad+提交的验退数量和报废数量		
                    sql += @"UPDATE MES010M SET qty_bad=qty_bad+'" + double.Parse(qty2) + "' + '" + double.Parse(qty3) + "' where production_order= '" + ProductionOrder + "'";
                    DB.ExecuteNonQueryOffline(sql);
                    //4.判断工艺是否工艺流程中最后一个工艺，如果是的话，MES010M的qty_finish=qty_finish+提交的验收数量	
                    sql = @"SELECT * FROM MES010A1 WHERE production_order='" + ProductionOrder + "' AND procedure_no='" + procedure_no + "' AND sorting = (SELECT MAX(sorting) from MES010A1)";
                    System.Data.DataTable dt = DB.GetDataTable(sql);
                    if (dt.Rows.Count > 0)//是最后一道工序
                    {
                        sql += @"UPDATE MES010M SET qty_finish=qty_finish+'" + double.Parse(qty) + "'  where production_order= '" + ProductionOrder + "'";
                        DB.ExecuteNonQueryOffline(sql);
                    }
                    ErrMsg = string.Empty;
                    IsSuccess = true;
                }
                else
                {
                    RetData = ErrMsg;
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

        public static string ReportWork(object OBJ)
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
            string sql = string.Empty;
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
                string ProductionOrder = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<ProductionOrder>", "</ProductionOrder>");
                string procedure_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<procedure_no>", "</procedure_no>");
                string machine = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<machine>", "</machine>");
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<MES010A8>", "</MES010A8>");
                
                #endregion
                string ErrMsg = string.Empty;
                DB = new GDSJ_Framework.DBHelper.DataBase(XML);
                string UserCode = "admin";

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 逻辑
                System.Data.DataTable dt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);
                double qty_count = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        qty_count += double.Parse(dt.Rows[i]["qty"].ToString());
                    }
                }
                double qtyM = DB.GetDouble("SELECT qty FROM MES010M where production_order='" + ProductionOrder + "'");//计划数量
                if (qty_count > qtyM)//合格数量超出工单计划数
                {
                    RetData = "合格数量超出工单计划数";
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sql += @"INSERT INTO MES010A8 (production_order,procedure_no,machine,person,qty,qty2,qty3,createby,createdate,createtime) VALUES ('" + ProductionOrder+"','"+procedure_no+"','"+machine+"','"+ dt.Rows[i]["person"].ToString() + "','" + dt.Rows[i]["qty"].ToString() + "','" + dt.Rows[i]["qty2"].ToString() + "','" + dt.Rows[i]["qty3"].ToString() + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                        }
                    }
                    DB.ExecuteNonQueryOffline(sql);
                    IsSuccess = true;
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

        //
        public string GetData(object OBJ)
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

                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<sql>", "</sql>");
                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑


                Dictionary<string, object> p = new Dictionary<string, object>();

                System.Data.DataTable dt = DB.GetDataTable(sql, p);
                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                if (dt.Rows.Count > 0)
                {
                    IsSuccess = true;
                    RetData = "<DataTable>" + dtXML + @"</DataTable>";

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
