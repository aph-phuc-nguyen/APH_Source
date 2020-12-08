using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class MAC040
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
        /// <summary>
        /// PC保养计划提交数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string Confirm_BaoYang_PC(object OBJ)
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
            string DBSERVER = string.Empty;
            string DBNAME = string.Empty;
            string DBUSER = string.Empty;
            string DBPASSWORD = string.Empty;

            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                DBSERVER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBSERVER>", "</DBSERVER>");
                DBNAME = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBNAME>", "</DBNAME>");
                DBUSER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBUSER>", "</DBUSER>");
                DBPASSWORD = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBPASSWORD>", "</DBPASSWORD>");
                string sql = string.Empty;

                #region 接口参数

                string DataTable1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DataTable1>", "</DataTable1>");
                string DataTable2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DataTable2>", "</DataTable2>");
                string EqNo = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<EqNo>", "</EqNo>");
                string EqName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<EqName>", "</EqName>");
                string People = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<People>", "</People>");
                string state = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<state>", "</state>");
                string book = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<book>", "</book>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<username>", "</username>");

                DataTable dt1 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable1);
                DataTable dt2 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable2);
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                //创建维护编号
                string sqlNumber = @"select to_number(nvl(MAX(fix_no), to_char(sysdate, 'yyyymmddhh24miss')||'000'))+1 from MAC040M join dual on 1 = 1 WHERE fix_no LIKE to_char(sysdate, 'yyyymmddhh24miss')||'%'";
                string fix_no = DB.GetString(sqlNumber);
                //MAC040M表插入数据
                string sqlnowID = @"select nvl(max(id),0)+1 from MAC040M";
                string nowID = DB.GetString(sqlnowID);
                var ID = "\"ID\"";
                var VALUE = "\"VALUE\"";
                sql = $@"INSERT INTO MAC040M({ID},FIX_NO,MACHINE_NO,MACHINE_NAME,FIX_TYPE,MANUAL_NO,STATUS_MAC,MEMO,CREATEBY,CREATEDATE,CREATETIME) 
values(" + nowID + ",'" + fix_no + "','" + EqNo + "','" + EqName + "'," + "'保养','" + book + "','" + state + "','" + "NULL','" + username + "'" +
",TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),to_char(sysdate,'hh24:mi:ss'))";
                DB.ExecuteNonQueryOffline(sql);

                //MAC040A1表
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    sqlnowID = @"select nvl(max(id),0)+1 from MAC040A1";
                    nowID = DB.GetString(sqlnowID);
                    sql = $@"INSERT INTO MAC040A1({ID},FIX_NO,SORTING,PLAN_NO,DATETIME_PLAN,ITEM_NO,ITEM_NAME,{VALUE},MEMO,
CREATEBY,CREATEDATE,CREATETIME,DATETIME_FIX,STATUS) values(" + nowID + ",'" + fix_no + "','" + (i + 1).ToString("00") + "','" + dt1.Rows[i]["计划编号"] + "'" +
",'" + dt1.Rows[i]["计划日期"] + "','" + dt1.Rows[i]["内容编号"] + "','" + dt1.Rows[i]["内容名称"] + "','" + dt1.Rows[i]["标准值"] + "','" + "NULL" + "'," +
"'" + username + "',TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),to_char(sysdate,'hh24:mi:ss')," +
"TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD hh24:mi:ss'),'" + dt1.Rows[i]["机械状态"] +"')";
                    DB.ExecuteNonQueryOffline(sql);
                }

                //更新MES030A4表
                dt2.DefaultView.Sort = "计划日期 DESC";
                dt2 = dt2.DefaultView.ToTable();
                string editsql = string.Empty;
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        editsql = @"UPDATE MES030A4 SET STATE='完成',DATE_FIX=to_char(sysdate,'yyyy-mm-dd hh24:mi:ss')," +
                            "PERSON_FIX='" + People + "',STATE_MACHINE='" + state + "' where MACHINE_NO='" + EqNo + "' and date_plan='" + dt2.Rows[i]["计划日期"] + "'";
                    }
                    else
                    {
                        editsql = @"UPDATE MES030A4 SET STATE='取消' where MACHINE_NO='" + EqNo + "' and date_plan='" + dt2.Rows[i]["计划日期"] + "' and state='计划'";
                    }
                    DB.ExecuteNonQueryOffline(editsql);
                }
                #endregion
                IsSuccess = true;
                RetData = "保养成功！";
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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

        public static string Confirm_JiaoZheng_PC(object OBJ)
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
            string DBSERVER = string.Empty;
            string DBNAME = string.Empty;
            string DBUSER = string.Empty;
            string DBPASSWORD = string.Empty;

            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                DBSERVER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBSERVER>", "</DBSERVER>");
                DBNAME = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBNAME>", "</DBNAME>");
                DBUSER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBUSER>", "</DBUSER>");
                DBPASSWORD = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBPASSWORD>", "</DBPASSWORD>");
                string sql = string.Empty;

                #region 接口参数


                string strMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<strMES030A5>", "</strMES030A5>");
                string stroldMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<stroldMES030A5>", "</stroldMES030A5>");
                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string machine_name = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_name>", "</machine_name>");
                string work_state = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<state>", "</state>");
                string memo = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<memo>", "</memo>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<username>", "</username>");


                DataTable dtMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(strMES030A5);
                DataTable dtoldMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(stroldMES030A5);
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                //创建维护编号

                string sqlNumber = @"select to_number(nvl(MAX(fix_no), to_char(sysdate, 'yyyymmddhh24miss')||'000'))+1 from MAC040M join dual on 1 = 1 WHERE fix_no LIKE to_char(sysdate, 'yyyymmddhh24miss')||'%'";
                //string fix_no = DB.GetString("SELECT cast(ISNULL(MAX(fix_no),CONVERT (nvarchar(12),GETDATE(),112)+'000') as float)+1 FROM MAC040M(NOLOCK) WHERE fix_no LIKE CONVERT (nvarchar(12),GETDATE(),112)+'%'");
                string fix_no = DB.GetString(sqlNumber);
                //MAC040M表插入数据
                //获取当前ID
                string sqlnowID = @"select nvl(max(id),0)+1 from MAC040M";
                string nowID = DB.GetString(sqlnowID);

                sql = @"INSERT INTO MAC040M(id,fix_no,machine_no,machine_name,fix_type,manual_no,status_mac,memo,createby,createdate,createtime) values("+ nowID + ",'" + fix_no + "','" + machine_no + "','" + machine_name + "'," + "'校正','" + "NULL" + "','" + work_state + "','" + "NULL','" + username + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                //Dictionary<string, object> pp = new Dictionary<string, object>();
                DB.ExecuteNonQueryOffline(sql);

                //MAC040A1表
                for (int i = 0; i < dtMES030A5.Rows.Count; i++)
                {
                    sqlnowID = @"select nvl(max(id),0)+1 from MAC040A1";
                    nowID = DB.GetString(sqlnowID);
                    sql = @"INSERT INTO MAC040A1(""ID"",FIX_NO,SORTING,PLAN_NO,DATETIME_PLAN,ITEM_NO,ITEM_NAME,""VALUE"",MEMO,CREATEBY,CREATEDATE,CREATETIME,DATETIME_FIX,STATUS) values(" + nowID + ",'" + fix_no + "','" + (i + 1).ToString("00") + "','"+dtMES030A5.Rows[i]["计划编号"] +"','"+dtMES030A5.Rows[i]["计划日期"] +"','" + dtMES030A5.Rows[i]["内容编号"] + "','" + dtMES030A5.Rows[i]["内容名称"] + "','" + dtMES030A5.Rows[i]["标准值"] + "','" + "NULL" + "','" + username + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+dtMES030A5.Rows[i]["机械状态"] +"')";
                    DB.ExecuteNonQueryOffline(sql);
                }

                //更新MES030A4表
                dtoldMES030A5.DefaultView.Sort = "计划日期 DESC";
                dtoldMES030A5 = dtoldMES030A5.DefaultView.ToTable();
                string editsql = string.Empty;
                for (int i = 0; i < dtoldMES030A5.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        editsql = @"UPDATE MES030A5 SET STATE='完成',DATE_FIX=to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),PERSON_FIX='" + username + "',STATE_MACHINE='" + work_state + "' where MACHINE_NO='" + machine_no + "' and date_plan='" + dtoldMES030A5.Rows[i]["计划日期"] + "'";
                    }
                    else
                    {
                        editsql = @"UPDATE MES030A5 SET STATE='取消' where MACHINE_NO='" + machine_no + "' and date_plan='" + dtoldMES030A5.Rows[i]["计划日期"] + "' and state='计划'";
                    }
                    DB.ExecuteNonQueryOffline(editsql);
                }
                #endregion
                IsSuccess = true;
                RetData = "校正成功！";
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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
        /// PDA保养提交数据接口
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string Confirm_BaoYan(object OBJ)
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
            string DBSERVER = string.Empty;
            string DBNAME = string.Empty;
            string DBUSER = string.Empty;
            string DBPASSWORD = string.Empty;

            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                DBSERVER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBSERVER>", "</DBSERVER>");
                DBNAME = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBNAME>", "</DBNAME>");
                DBUSER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBUSER>", "</DBUSER>");
                DBPASSWORD = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBPASSWORD>", "</DBPASSWORD>");
                string sql = string.Empty;

                #region 接口参数

                string DataTable1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DataTable1>", "</DataTable1>");

                string strMES030A4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<strMES030A4>", "</strMES030A4>");
                string stroldMES030A4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<stroldMES030A4>", "</stroldMES030A4>");
                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string machine_name = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_name>", "</machine_name>");
                string work_state = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<work_state>", "</work_state>");
                string memo = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<memo>", "</memo>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<dt_MES030A7>", "</dt_MES030A7>");
                DataTable dt_MES030A7 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);

                DataTable dt1 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable1);

                DataTable dtMES030A4 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(strMES030A4);
                DataTable dtoldMES030A4 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(stroldMES030A4);
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                //创建维护编号
                string sqlNumber = @"select to_number(nvl(MAX(fix_no), to_char(sysdate, 'yyyymmddhh24miss')||'000'))+1 from MAC040M join dual on 1 = 1 WHERE fix_no LIKE to_char(sysdate, 'yyyymmddhh24miss')||'%'";
                string fix_no = DB.GetString(sqlNumber);

                //MAC040M表插入数据
                string sqlnowID = @"select nvl(max(id),0)+1 from MAC040M";
                string nowID = DB.GetString(sqlnowID);
                sql = @"INSERT INTO MAC040M(""ID"",FIX_NO,MACHINE_NO,MACHINE_NAME,FIX_TYPE,MANUAL_NO,STATUS_MAC,MEMO,CREATEBY,CREATEDATE,CREATETIME) values(" + nowID + ",'" + fix_no + "','" + machine_no + "','" + machine_name + "'," + "'保养','" + "NULL" + "','" + work_state + "','" + "NULL','" + username + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";

                DB.ExecuteNonQueryOffline(sql);

                //MAC040A1表
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    sqlnowID = @"select nvl(max(id),0)+1 from MAC040A1";
                    nowID = DB.GetString(sqlnowID);
                    sql = @"INSERT INTO MAC040A1(""ID"",FIX_NO,SORTING,ITEM_NO,ITEM_NAME,""VALUE"",MEMO,CREATEBY,CREATEDATE,CREATETIME) values(" + nowID + ",'" + fix_no + "','" + (i + 1).ToString("00") + "','" + dt1.Rows[i]["item_id"] + "','" + dt1.Rows[i]["item_name"] + "','" + dt1.Rows[i]["value"] + "','" + "NULL" + "','" + username + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                    DB.ExecuteNonQueryOffline(sql);
                }

                //更新MES030A4表
                dtMES030A4.DefaultView.Sort = "date_plan DESC";
                dtMES030A4 = dtMES030A4.DefaultView.ToTable();
                string editsql = string.Empty;
                for (int i = 0; i < dtMES030A4.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        editsql = @"UPDATE MES030A4 SET STSTE='完成',DATE_FIX='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',PERSON_FIX='" + username + "',STATE_MACHINE='" + work_state + "' where MACHINE_NO='" + machine_no + "' and date_plan='" + dtMES030A4.Rows[i]["date_plan"] + "'";
                    }
                    else
                    {
                        editsql = @"UPDATE MES030A4 SET STSTE='取消' where MACHINE_NO='" + machine_no + "' and date_plan='" + dtMES030A4.Rows[i]["date_plan"] + "' and state='计划'";
                    }
                    DB.ExecuteNonQueryOffline(editsql);
                }
                if (dt_MES030A7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_MES030A7.Rows.Count; i++)
                    {
                        //Dictionary<string, object> p = new Dictionary<string, object>();
                        //p.Add("@machine_no", machine_no);
                        ////p.Add("@sorting", int.Parse(DB.GetString("SELECT MAX(CONVERT(int,sorting))+1 FROM MES030A7 WHERE machine_no='" + dt_MES030A7.Rows[i]["machine_no"] + "'")));
                        //p.Add("@sorting", dt_MES030A7.Rows[i]["sorting"]);
                        //p.Add("@date_plan", DateTime.Now.ToString("yyyy-MM-dd"));
                        //p.Add("@products_barcode", dt_MES030A7.Rows[i]["products_barcode"]);
                        //p.Add("@lot_barcode", dt_MES030A7.Rows[i]["lot_barcode"]);
                        //p.Add("@packing_barcode", dt_MES030A7.Rows[i]["packing_barcode"]);
                        //p.Add("@production_code", dt_MES030A7.Rows[i]["production_code"]);
                        //p.Add("@material_no", dt_MES030A7.Rows[i]["material_no"]);
                        //p.Add("@material_name", dt_MES030A7.Rows[i]["material_name"]);
                        //p.Add("@qty", dt_MES030A7.Rows[i]["qty"]);
                        //p.Add("@memo", dt_MES030A7.Rows[i]["memo"]);
                        //p.Add("@org", dt_MES030A7.Rows[i]["org"]);
                        //p.Add("@createby", dt_MES030A7.Rows[i]["createby"]);
                        //p.Add("@createdate", DateTime.Now.ToString("yyyy-MM-dd"));
                        //p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));
                        sqlnowID = @"select nvl(max(id),0)+1 from MES030A7";
                        nowID = DB.GetString(sqlnowID);
                        sql = @"INSERT INTO MES030A7(""ID"",MACHINE_NO,SORTING,DATE_PLAN,PRODUCTS_BARCODE,LOT_BARCODE,PACKING_BARCODE,PRODUCTION_CODE,MATERIAL_NO,MATERIAL_NAME,QTY,MEMO,ORG,CREATEBY,CREATEDATE,CREATETIME) " +
                           @"VALUES(" + nowID + ",'" + machine_no + "','" + dt_MES030A7.Rows[i]["sorting"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + dt_MES030A7.Rows[i]["products_barcode"] +
                           @"','" + dt_MES030A7.Rows[i]["lot_barcode"] + "','" + dt_MES030A7.Rows[i]["packing_barcode"] + "','" + dt_MES030A7.Rows[i]["production_code"] + "','" + dt_MES030A7.Rows[i]["material_no"] +
                           "','" + dt_MES030A7.Rows[i]["material_name"] + "','" + dt_MES030A7.Rows[i]["qty"] + "','" + dt_MES030A7.Rows[i]["memo"] + "','" + dt_MES030A7.Rows[i]["org"] +
                           @"','" + dt_MES030A7.Rows[i]["createby"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                        DB.ExecuteNonQueryOffline(sql);
                    }

                }

                #region 机器设备增加  使用剩余厚度
                sql = @"UPDATE MES030M SET  HIGH_LAST = nvl(high_std,'0') where machine_no = '" + machine_no + "'";
                DB.ExecuteNonQueryOffline(sql);
                #endregion

                #endregion
                IsSuccess = true;
                RetData = "保养成功！";
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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
        /// PDA校正提交数据
        /// </summary>
        /// <param name="OBJ"></param> 
        /// <returns></returns>
        public static string Confirm_JiaoZheng(object OBJ)
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
            string DBSERVER = string.Empty;
            string DBNAME = string.Empty;
            string DBUSER = string.Empty;
            string DBPASSWORD = string.Empty;

            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                DBSERVER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBSERVER>", "</DBSERVER>");
                DBNAME = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBNAME>", "</DBNAME>");
                DBUSER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBUSER>", "</DBUSER>");
                DBPASSWORD = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBPASSWORD>", "</DBPASSWORD>");
                string sql = string.Empty;

                #region 接口参数

                string DataTable1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DataTable1>", "</DataTable1>");

                string strMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<strMES030A4>", "</strMES030A4>");
                string stroldMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<stroldMES030A4>", "</stroldMES030A4>");
                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string machine_name = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_name>", "</machine_name>");
                string work_state = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<work_state>", "</work_state>");
                string memo = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<memo>", "</memo>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<dt_MES030A7>", "</dt_MES030A7>");
                DataTable dt_MES030A7 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);

                DataTable dt1 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable1);

                DataTable dtMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(strMES030A5);
                DataTable dtoldMES030A5 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(stroldMES030A5);
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                //创建维护编号
                string sqlNumber = @"select to_number(nvl(MAX(fix_no), to_char(sysdate, 'yyyymmddhh24miss')||'000'))+1 from MAC040M join dual on 1 = 1 WHERE fix_no LIKE to_char(sysdate, 'yyyymmddhh24miss')||'%'";
                string fix_no = DB.GetString(sqlNumber);

                //MAC040M表插入数据


                //MAC040A1表
                for (int i = 0; i < dtMES030A5.Rows.Count; i++)
                {
                    
                   
                    DB.ExecuteNonQueryOffline(sql);
                }

                //MAC040M表插入数据
                //获取当前ID
                string sqlnowID = @"select nvl(max(id),0)+1 from MAC040M";
                string nowID = DB.GetString(sqlnowID);
                sql = @"INSERT INTO MAC040M(""ID"",FIX_NO,MACHINE_NO,MACHINE_NAME,FIX_TYPE,MANUAL_NO,STATUS_MAC,MEMO,CREATEBY,CREATEDATE,CREATETIME) values(" + nowID + ",'" + fix_no + "','" + machine_no + "','" + machine_name + "'," + "'校正','" + "NULL" + "','" + work_state + "','" + "NULL','" + username + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";

                DB.ExecuteNonQueryOffline(sql);

                //MAC040A1表
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    sqlnowID = @"select nvl(max(id),0)+1 from MAC040A1";
                    nowID = DB.GetString(sqlnowID);
                    //sql = @"INSERT INTO MAC040A1(fix_no,sorting,item_no,item_name,value,memo,createby,createdate,createtime) values('" + fix_no + "','" + (i + 1).ToString("00") + "','" + dt1.Rows[i]["item_id"] + "','" + dt1.Rows[i]["item_name"] + "','" + dt1.Rows[i]["value"] + "','" + "NULL" + "','" + username + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";

                    sql = @"INSERT INTO MAC040A1(""ID"",FIX_NO,SORTING,ITEM_NO,ITEM_NAME,VALUE,MEMO,CREATEBY,CREATEDATE,CREATETIME) values(" + nowID + ",'" + fix_no + "','" + (i + 1).ToString("00") + "','" + dt1.Rows[i]["item_id"] + "','" + dt1.Rows[i]["item_name"] + "','" + dt1.Rows[i]["value"] + "','" + "NULL" + "','" + username + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                    DB.ExecuteNonQueryOffline(sql);
                }

                //更新MES030A4表
                dtMES030A5.DefaultView.Sort = "date_plan DESC";
                dtMES030A5 = dtMES030A5.DefaultView.ToTable();
                string editsql = string.Empty;
          
                    for (int i = 0; i < dtMES030A5.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            editsql = @"UPDATE MES030A5 SET ""STATE""='完成',DATE_FIX='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',PERSON_FIX='" + username + "',STATE_MACHINE='" + work_state + "' WHERE MACHINE_NO='" + machine_no + "' and DATE_PLAN='" + dtMES030A5.Rows[i]["date_plan"] + "'";
                        }
                        else
                        {
                            editsql = @"UPDATE MES030A5 SET ""STATE""='取消' WHERE MACHINE_NO='" + machine_no + "' and DATE_PLAN='" + dtMES030A5.Rows[i]["DATE_PLAN"] + @"' and ""STATE""='计划'";
                        }
                        DB.ExecuteNonQueryOffline(editsql);
                    }
                
  
                if (dt_MES030A7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_MES030A7.Rows.Count; i++)
                    {
                        //Dictionary<string, object> p = new Dictionary<string, object>();
                        //p.Add("@machine_no", machine_no);
                        ////p.Add("@sorting", int.Parse(DB.GetString("SELECT MAX(CONVERT(int,sorting))+1 FROM MES030A7 WHERE machine_no='" + dt_MES030A7.Rows[i]["machine_no"] + "'")));
                        //p.Add("@sorting", dt_MES030A7.Rows[i]["sorting"]);
                        //p.Add("@date_plan", DateTime.Now.ToString("yyyy-MM-dd"));
                        //p.Add("@products_barcode", dt_MES030A7.Rows[i]["products_barcode"]);
                        //p.Add("@lot_barcode", dt_MES030A7.Rows[i]["lot_barcode"]);
                        //p.Add("@packing_barcode", dt_MES030A7.Rows[i]["packing_barcode"]);
                        //p.Add("@production_code", dt_MES030A7.Rows[i]["production_code"]);
                        //p.Add("@material_no", dt_MES030A7.Rows[i]["material_no"]);
                        //p.Add("@material_name", dt_MES030A7.Rows[i]["material_name"]);
                        //p.Add("@qty", dt_MES030A7.Rows[i]["qty"]);
                        //p.Add("@memo", dt_MES030A7.Rows[i]["memo"]);
                        //p.Add("@org", dt_MES030A7.Rows[i]["org"]);
                        //p.Add("@createby", dt_MES030A7.Rows[i]["createby"]);
                        //p.Add("@createdate", DateTime.Now.ToString("yyyy-MM-dd"));
                        //p.Add("@createtime", DateTime.Now.ToString("HH:mm:ss"));
                        sqlnowID = @"select nvl(max(id),0)+1 from MES030A7";
                        nowID = DB.GetString(sqlnowID);
                        sql = @"INSERT INTO MES030A7(""ID"",MACHINE_NO,SORTING,DATE_PLAN,PRODUCTS_BARCODE,LOT_BARCODE,PACKING_BARCODE,PRODUCTION_CODE,MATERIAL_NO,MATERIAL_NAME,QTY,MEMO,ORG,CREATEBY,CREATEDATE,CREATETIME) " +
                            @"VALUES(" + nowID + ",'" + machine_no + "','" + dt_MES030A7.Rows[i]["sorting"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + dt_MES030A7.Rows[i]["products_barcode"] +
                            @"','" + dt_MES030A7.Rows[i]["lot_barcode"] + "','" + dt_MES030A7.Rows[i]["packing_barcode"] + "','" + dt_MES030A7.Rows[i]["production_code"] + "','" + dt_MES030A7.Rows[i]["material_no"] +
                            "','" + dt_MES030A7.Rows[i]["material_name"] + "','" + dt_MES030A7.Rows[i]["qty"] + "','" + dt_MES030A7.Rows[i]["memo"] + "','" + dt_MES030A7.Rows[i]["org"] +
                            @"','" + dt_MES030A7.Rows[i]["createby"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                        DB.ExecuteNonQueryOffline(sql);
                    }

                }
                #endregion
                IsSuccess = true;
                RetData = "保养成功！";
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
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
    

