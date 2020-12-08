using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
   public  class GetData
    {
        private static GDSJ_Framework.DBHelper.Oracle getOracleDb()
        {

            string strConfig = GDSJ_Framework.Common.TXTHelper.ReadToEnd(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + @"\SJQMS_API_Config.xml");

            string DataSource = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(strConfig, "<DataSource>", "</DataSource>");
            string UserId = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(strConfig, "<UserId>", "</UserId>");
            string Password = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(strConfig, "<Password>", "</Password>");

            GDSJ_Framework.DBHelper.Oracle DB = new GDSJ_Framework.DBHelper.Oracle(DataSource, UserId, Password);

            return DB;
        }
        private static GDSJ_Framework.DBHelper.Oracle getOracleERPDb()
        {

            string strConfig = GDSJ_Framework.Common.TXTHelper.ReadToEnd(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + @"\DBConfig.xml");

            string DataSource = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(strConfig, "<DataSource>", "</DataSource>");
            string UserId = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(strConfig, "<UserId>", "</UserId>");
            string Password = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(strConfig, "<Password>", "</Password>");

            GDSJ_Framework.DBHelper.Oracle DB = new GDSJ_Framework.DBHelper.Oracle(DataSource, UserId, Password);

            return DB;
        }
        /// <summary>
        /// GetDataTable(获取DataTable)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string GetDataTable(object OBJ)
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
            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                if (!string.IsNullOrEmpty(sql))
                {
                    DataTable dt = DB.GetDataTable(sql);
                    string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                    IsSuccess = true;
                    RetData = dtXML;
                }
                else
                {
                    IsSuccess = false;
                    RetData = "sql不能为空！";
                }
              

            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

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

        public static string GetDataTableERP(object OBJ)
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
            GDSJ_Framework.DBHelper.Oracle DB = getOracleERPDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                if (!string.IsNullOrEmpty(sql))
                {
                    DataTable dt = DB.GetDataTable(sql);
                    string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                    IsSuccess = true;
                    RetData = dtXML;
                }
                else
                {
                    IsSuccess = false;
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

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

        public static string GetString(object OBJ)
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
            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                if (!string.IsNullOrEmpty(sql))
                {
                    string dt = DB.GetString(sql);
                    IsSuccess = true;
                    RetData = dt;
                }
                else
                {
                    IsSuccess = false;
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

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

        public static string GetStringERP(object OBJ)
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
            GDSJ_Framework.DBHelper.Oracle DB = getOracleERPDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                if (!string.IsNullOrEmpty(sql))
                {
                    string dt = DB.GetString(sql);
                    IsSuccess = true;
                    RetData = dt;
                }
                else
                {
                    IsSuccess = false;
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

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

        public static string ExecuteNonQuery(object OBJ)
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
            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                if (!string.IsNullOrEmpty(sql))
                {
                    string r = DB.ExecuteNonQueryOffline(sql).ToString();
                    IsSuccess = true;
                    RetData = r;
                }
                else
                {
                    IsSuccess = false;
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

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


        public static string ExecuteNonQueryERP(object OBJ)
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
            GDSJ_Framework.DBHelper.Oracle DB = getOracleERPDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                if (!string.IsNullOrEmpty(sql))
                {
                    string r = DB.ExecuteNonQueryOffline(sql).ToString();
                    IsSuccess = true;
                    RetData = r;
                }
                else
                {
                    IsSuccess = false;
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

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

        public static string GetString_sql(string sql)
        {
            string RetData = string.Empty;            
            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    string dt = DB.GetString(sql);
                    RetData = dt;
                }
                else
                {
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            return RetData;
        }

        public static DataTable GetDataTable_sql(string sql)
        {                        
            string RetData = string.Empty;
            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();
            DataTable dt = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    dt = DB.GetDataTable(sql);
                    string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                    RetData = dtXML;
                }
                else
                {
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData)；

            return dt;
        }

        public static string ExecuteNonQuery_sql(string sql)
        {
            string RetData = string.Empty;
            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    string r = DB.ExecuteNonQueryOffline(sql).ToString();
                    RetData = r;
                }
                else
                {
                    RetData = "sql不能为空！";
                }


            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            return RetData;
        }
    }
}
