using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    /// <summary>
    /// 用户API
    /// </summary>
    public class User
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
 User(用户)接口帮助 @END
 方法：Help(帮助),CheckUser(检验用户) @END
 @END
====================CheckUser方法调用=================== @END
 Data参数如下
 <Data> @END
 @END
 <UserCode></UserCode>用户账号 @END
 <UserPwd></UserPwd>用户密码 @END
 @END
 </Data> @END
 @END
 返回数据如下 @END
 <Return> @END
 <IsSuccess>true/false</IsSuccess> @END
 <RetData> @END
 如果成功：<UserName></UserName>用户名 @END
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


        /// <summary>
        /// CheckUser(检验用户)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string CheckUser(object OBJ)
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

                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string UserPwd = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserPwd>", "</UserPwd>");

                

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
SELECT MaxWindow
FROM SJEMSSYS.dbo.SYSUSER01M
WHERE [UserCode]=@UserCode AND [UserPwd]=@UserPwd
";
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("@UserCode", UserCode);
                p.Add("@UserPwd", UserPwd.ToUpper());

                string MaxWindow = DB.GetString(sql, p);
                if(!string.IsNullOrEmpty(MaxWindow))
                {
                    IsSuccess = true;

                    sql = @"
SELECT staff_name FROM HR001M
WHERE user_code=@UserCode
";
                    string UserName = DB.GetString(sql, p);
                    if(string.IsNullOrEmpty(UserName))
                    {
                        UserName = UserCode;
                    }

                    RetData = "<MaxWindow>" + MaxWindow + @"</MaxWindow>";
                    RetData += "<UserName>" + UserName + @"</UserName>";
                }
                else
                {
                    RetData = "账号或密码错误";
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
        /// ChangePasswork(修改密码)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string ChangePasswork(object OBJ)
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

                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string UserPwd = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserPwd>", "</UserPwd>");
                string UserPwdNew = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserPwdNew>", "</UserPwdNew>");


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
SELECT USERNAME
FROM user_info_table
WHERE [LOGINID]=@UserCode AND [PASSWORD]=@UserPwd
";
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("@UserCode", UserCode);
                p.Add("@UserPwd", UserPwd.ToUpper());
                p.Add("@UserPwdNew", UserPwdNew.ToUpper());

                string UserName = DB.GetString(sql, p);
                if (!string.IsNullOrEmpty(UserName))
                {
                    sql = @"
UPDATE user_info_table set [PASSWORD]=@UserPwdNew
where [LOGINID]=@UserCode 
";
                    DB.ExecuteNonQueryOffline(sql, p);
                    IsSuccess = true;
                    
                }
                else
                {
                    RetData = "账号或密码错误";
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
