using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SJEMS_API
{
    public class DataBase
    {
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
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                string sqlp = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sqlp>", "</sqlp>");
                string[] s = new string[1];
                s[0] = ",";
                string[] pname = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<pname>", "</pname>").Split(s, StringSplitOptions.RemoveEmptyEntries);


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                Dictionary<string, object> p = new Dictionary<string, object>();
                foreach (string pn in pname)
                {
                    p.Add(pn, GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(sqlp, "<" + pn + ">", "</" + pn + ">"));
                }

                System.Data.DataTable dt = DB.GetDataTable(sql, p);

                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                IsSuccess = true;
                RetData += dtXML;

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetPageDataTable(object OBJ)
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

                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                string orderBy = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<orderBy>", "</orderBy>");
                string pageNo = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<pageNo>", "</pageNo>");
                string pageSize = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<pageSize>", "</pageSize>");
                string sqlWhere = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sqlWhere>", "</sqlWhere>");

                string newSql = "select top " + pageSize + "* from(select Row_Number() over(" + orderBy + ") as '序号',* from(" + sql + ")T where 1=1 " + sqlWhere + ")tmp where tmp.序号>" +
                    (Convert.ToInt32(pageNo) - 1) * Convert.ToInt32(pageSize);
                System.Data.DataTable dt = DB.GetDataTable(newSql);

                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                IsSuccess = true;
                RetData += dtXML;

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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

        /// <summary>
        /// 获取DataTable和SQL链接字符串
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetDataTableAndConn(object OBJ)
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



                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                string sqlp = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sqlp>", "</sqlp>");
                string[] s = new string[1];
                s[0] = ",";
                string[] pname = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<pname>", "</pname>").Split(s, StringSplitOptions.RemoveEmptyEntries);


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                Dictionary<string, object> p = new Dictionary<string, object>();
                foreach (string pn in pname)
                {
                    p.Add(pn, GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(sqlp, "<" + pn + ">", "</" + pn + ">"));
                }

                System.Data.DataTable dt = DB.GetDataTable(sql, p);

                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                IsSuccess = true;
                RetData = "<Table>" + dtXML + "</Table><ConnectionString>" + DB.ConnectionText + "</ConnectionString>";

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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

        /// <summary>
        /// GetString(获取String)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
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
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");
                string sqlp = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sqlp>", "</sqlp>");
                string[] s = new string[1];
                s[0] = ",";
                string[] pname = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<pname>", "</pname>").Split(s, StringSplitOptions.RemoveEmptyEntries);


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                Dictionary<string, object> p = new Dictionary<string, object>();
                foreach (string pn in pname)
                {
                    p.Add(pn, GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(sqlp, "<" + pn + ">", "</" + pn + ">"));
                }

                string r = DB.GetString(sql, p);



                IsSuccess = true;
                RetData += r;

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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


        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
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
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                string sql = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sql>", "</sql>");

                string sqlp = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<sqlp>", "</sqlp>");
                string[] s = new string[1];
                s[0] = ",";
                string[] pname = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<pname>", "</pname>").Split(s, StringSplitOptions.RemoveEmptyEntries);



                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                Dictionary<string, object> p = new Dictionary<string, object>();
                foreach (string pn in pname)
                {
                    p.Add(pn, GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(sqlp, "<" + pn + ">", "</" + pn + ">"));
                }

                string r = DB.ExecuteNonQueryOffline(sql, p).ToString();



                IsSuccess = true;
                RetData += r;

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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



        /// <summary>
        /// GetUploadPhoto(获取上传的图片)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string GetUploadPhoto(object OBJ)
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

                string photoBase64 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<photoBase64>", "</photoBase64>");
                // 去掉前面的data:image/png;base64,  
                if (photoBase64.IndexOf("data:image/png;base64,") != -1)
                {
                    photoBase64 = photoBase64.Replace("data:image/png;base64,", "");
                }
                byte[] bytes = Convert.FromBase64String(photoBase64);
                MemoryStream memStream = new MemoryStream(bytes);
                Image mImage = Image.FromStream(memStream);
              

                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\temp"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\temp");
                }
                string fileName = AppDomain.CurrentDomain.BaseDirectory + "temp\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".Jpeg";
                mImage.Save(fileName,ImageFormat.Jpeg);
                RetData = "<url>" + fileName + "</url>";


                IsSuccess = true;

                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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
    }




}
