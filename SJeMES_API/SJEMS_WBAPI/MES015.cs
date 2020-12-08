using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class MES015
    {
        /// <summary>
        /// GetOrderInfo(获取工单信息)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string GetOrderInfo(object OBJ)
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

                string Order = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Order>", "</Order>");

               
                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
SELECT * FROM MES015M(NOLOCK)
WHERE production_order =@production_order
";
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("@production_order", Order);

                System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
                if(dr.Read())
                {
                    string qty_plan = dr["qty"].ToString();
                    string qty_ok = dr["qty_finish"].ToString();

                    sql = @"
SELECT 
material_no AS '料号',
material_name AS '品名',
qty AS '需包装数量',
qty_finish AS '包装数量',
qty_usage AS '使用量'
FROM MES015A1(NOLOCK)
WHERE production_order =@production_order
";
                    System.Data.DataTable dt = DB.GetDataTable(sql, p);

                    string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                    RetData = "<qty_plan>" + qty_plan + @"</qty_plan>";
                    RetData += "<qty_ok>" + qty_ok + @"</qty_ok>";
                    RetData += "<dtMES015A1>"+dtXML+@"</dtMES015A1>";
                    IsSuccess = true;
                }
                else
                {
                    RetData = "没有该工单号";
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
        /// GetPHInfo(获取品号包装信息)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string GetPHInfo(object OBJ)
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

                string PH = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<PH>", "</PH>");

                
                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
SELECT 
material_no AS '品号',
material_name AS '品名',
material_specifications AS '规格'
FROM BASE007M
WHERE material_no=@material_no
";
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("@material_no", PH);

                System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
                if (dr.Read())
                {
                    string PM = dr["品名"].ToString();
                    string GG = dr["规格"].ToString();

                    sql = @"
SELECT 
material_no AS '品号',
material_name AS '品名',
material_specifications AS '规格',
[SUM] AS '使用量'
FROM BASE020A1
WHERE pack_no=@pack_no
ORDER BY sorting
";
                    p = new Dictionary<string, object>();
                    p.Add("@pack_no", PH);
                    System.Data.DataTable dt = DB.GetDataTable(sql, p);

                    string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                    RetData = "<品名>" + PH + @"</品名>";
                    RetData += "<规格>" + GG + @"</规格>";
                    RetData += "<dtMES015A1>" + dtXML + @"</dtMES015A1>";
                    IsSuccess = true;
                }
                else
                {
                    RetData = "没有该物料包装方式";
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
