using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class MAC020
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
        /// 添加明细
        /// </summary>
        public static string AddDetail(object OBJ)
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
            string Datatable1 = string.Empty;
            string strlist = string.Empty;


            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                #region 接口数据
                Datatable1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DataTable1>", "</DataTable1>");
                strlist = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<datelist>", "</datelist>");


                string paln_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<paln_no>", "</paln_no>");
                string paln_name = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<paln_name>", "</paln_name>");
                string plan_type = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<plan_type>", "</plan_type>");
                string hours = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<hours>", "</hours>");
                string manual_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<manual_no>", "</manual_no>");
                string date = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<date>", "</date>");
                string date2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<date2>", "</date2>");
                string type_radio1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<type_radio1>", "</type_radio1>");
                string type_radio2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<type_radio2>", "</type_radio2>");
                string type_radio3 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<type_radio3>", "</type_radio3>");
                string type_radio4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<type_radio4>", "</type_radio4>");
                string year_radio1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<year_radio1>", "</year_radio1>");
                string year_radio2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<year_radio2>", "</year_radio2>");
                string year_radio3 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<year_radio3>", "</year_radio3>");
                string year_md = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<year_md>", "</year_md>");
                string month_radio1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<month_radio1>", "</month_radio1>");
                string month_radio2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<month_radio2>", "</month_radio2>");
                string month_radio3 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<month_radio3>", "</month_radio3>");
                string month_day = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<month_day>", "</month_day>");
                string week1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<week1>", "</week1>");
                string week2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<week2>", "</week2>");
                string week3 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<week3>", "</week3>");
                string week4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<week4>", "</week4>");
                string week5 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<week5>", "</week5>");
                string week6 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<week6>", "</week6>");
                string week7 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<week7>", "</week7>");
                string enable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<enable>", "</enable>");

                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                #endregion
                DataTable dt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(Datatable1);
                List<string> datelist =new List<string>(strlist.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries));

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                //判断是否存在相同计划编号的数据
                string sql = @"SELECT * FROM MAC020M WHERE paln_no='" + paln_no + "'";  //主表数据
                DataTable dtHaveRow = DB.GetDataTable(sql);
                if (dtHaveRow.Rows.Count>0)
                {
                    sql = @"DELETE FROM MAC020M WHERE paln_no='" + paln_no + "'";
                    DB.ExecuteNonQueryOffline(sql);
                }
                sql = @"SELECT * FROM MAC020A1 WHERE paln_no='" + paln_no + "'"; //明细表
                dtHaveRow = DB.GetDataTable(sql); ;
                if (dtHaveRow.Rows.Count > 0)
                {
                    sql = @"DELETE FROM MAC020A1 WHERE paln_no='" + paln_no + "'";
                    DB.ExecuteNonQueryOffline(sql);
                }

                string sqlnowID = @"select nvl(max(id),0)+1 from MAC020M";
                string nowID = DB.GetString(sqlnowID);
                //                sql = @"INSERT INTO MAC020M(" + "ID" + @",PALN_NO,PALN_NAME,PLAN_TYPE,MANUAL_NO,HOURS," + "DATE"+ @",DATE2,TYPE_RADIO1,TYPE_RADIO2,TYPE_RADIO3
                //,TYPE_RADIO4,YEAR_RADIO1,YEAR_RADIO2,YEAR_RADIO3,YEAR_MD,MONTH_RADIO1,MONTH_RADIO2,MONTH_RADIO3,MONTH_DAY
                //,WEEK1,WEEK2,WEEK3,WEEK4,WEEK5,WEEK6,WEEK7
                //,CREATEBY,CREATEDATE,CREATETIME," + "ENABLE" + @")  VALUES(" + nowID + ",'" + paln_no + "','" + paln_name + "','" + plan_type + "','" + manual_no + "','" + hours + "','" + date + "','" + date2 + "','" + type_radio1 + "','" + type_radio2 + "','" + type_radio3 + "','" + type_radio4 + "','" + year_radio1 + "','" + year_radio2 + "','" + year_radio3 + "','" +
                //year_md + "','" + month_radio1 + "','" + month_radio2 + "','" + month_radio3 + "','" + month_day + "','" + week1 + "','" + week2 + "','" + week3 + "','" + week4 + "','" + week5 + "','" + week6 + "','" + week7 + "','" + UserCode + "',TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),to_char(sysdate,'hh24:mi:ss'),'" + enable + "')";
                //                DB.ExecuteNonQueryOffline(sql);

                var enableColumn = "\"ENABLE\"";
                var ID = "\"ID\"";
                var DA = "\"date\"";
                sql = $@"INSERT INTO MAC020M({ID},PALN_NO,PALN_NAME,PLAN_TYPE,MANUAL_NO,HOURS,{DA},DATE2,TYPE_RADIO1,TYPE_RADIO2,TYPE_RADIO3
,TYPE_RADIO4,YEAR_RADIO1,YEAR_RADIO2,YEAR_RADIO3,YEAR_MD,MONTH_RADIO1,MONTH_RADIO2,MONTH_RADIO3,MONTH_DAY
,WEEK1,WEEK2,WEEK3,WEEK4,WEEK5,WEEK6,WEEK7
,CREATEBY,CREATEDATE,CREATETIME,{enableColumn})  VALUES(" + nowID + ",'" + paln_no + "','" + paln_name + "','" + plan_type + "','" + manual_no + "','" + hours + "','"+date+"','" + date2 + "','" + type_radio1 + "','" + type_radio2 + "','" + type_radio3 + "','" + type_radio4 + "','" + year_radio1 + "','" + year_radio2 + "','" + year_radio3 + "','" +
             year_md + "','" + month_radio1 + "','" + month_radio2 + "','" + month_radio3 + "','" + month_day + "','" + week1 + "','" + week2 + "','" + week3 + "','" + week4 + "','" + week5 + "','" + week6 + "','" + week7 + "','" + UserCode + "',TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),to_char(sysdate,'hh24:mi:ss'),'" + enable + "')";
                DB.ExecuteNonQueryOffline(sql);
                #region 逻辑
                for (int i = 0; i < dt.Rows.Count; i++)  //循环设备明细
                {
                    sqlnowID = @"select nvl(max(id),0)+1 from MAC020A1";
                    nowID = DB.GetString(sqlnowID);
                    string sqlA1 = $@"INSERT INTO MAC020A1({ID},PALN_NO,MACHINE_NO,MACHINE_NAME,ADDRESS,{enableColumn},CREATEBY,CREATEDATE,CREATETIME) VALUES(" + nowID + ",'" + paln_no + "','" + dt.Rows[i]["设备编号"] + "','" + dt.Rows[i]["设备名称"] + "','" + dt.Rows[i]["存放位置"] + "','" + enable + "','" + UserCode + "',TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),to_char(sysdate,'hh24:mi:ss'))";
                    DB.ExecuteNonQueryOffline(sqlA1);

                }
                #endregion
                IsSuccess = true;
                RetData += "保存成功!";

            }
            catch (Exception ex)
            {
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];
                RetData = ex.Message;
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


        public static string AddPlan(object OBJ)
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
            string Datatable1 = string.Empty;
            string strlist = string.Empty;


            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                #region 接口数据
                Datatable1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DataTable1>", "</DataTable1>");
                strlist = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<datelist>", "</datelist>");


                string paln_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<paln_no>", "</paln_no>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string paln_type = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<plan_type>", "</plan_type>");
                DataTable dt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(Datatable1);
                List<string> datelist = new List<string>(strlist.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑
                //查询是否存在该计划编号的计划来源
                string tablename = string.Empty;
                if (paln_type == "保养")
                    tablename = "MES030A4";
                else
                    tablename = "MES030A5";
                //bool b = DB.GetBool("SELECT * FROM " + tablename + " WHERE plan_source='" + paln_no + "'");

                string isHaveRowsql = "SELECT * FROM " + tablename + " WHERE plan_source='" + paln_no + "'";  //主表数据
                DataTable dtHaveRow = DB.GetDataTable(isHaveRowsql);
                if (dtHaveRow.Rows.Count > 0)
                { 
                    //重新添加新的保养计划
                    for (int i = 0; i < dt.Rows.Count; i++)  //循环设备明细
                    {
                        //如果存在该计划编号的计划来源，则删除状态不是完成的保养计划
                        string delsql = @"DELETE FROM " + tablename + " WHERE plan_source='" + paln_no + "' AND state!='计划' AND machine_no='" + dt.Rows[i]["设备编号"] + "'";
                        //DB.ExecuteNonQueryOffline(delsql);
                        DB.ExecuteNonQueryOffline(delsql);
                        string sql = string.Empty;
                        DataTable dt_itme = new DataTable();
                        string result = string.Empty;

                        //获取当前设备绑定了多少个保养内容项或者校正计划
                        if (paln_type == "保养")
                        {
                            sql = @"SELECT item_name FROM MAC001M LEFT JOIN MAC001A2 ON MAC001M.item_id=MAC001A2.item_id WHERE MAC001A2.machine_no='" + dt.Rows[i]["设备编号"] + @"'";
                            dt_itme = DB.GetDataTable(sql);
                            //dt_itme = DB.GetDataTable(sql);
                        }
                        else
                        {
                            sql = @"SELECT item_name FROM MAC002M LEFT JOIN MAC002A2 ON MAC002M.item_id=MAC002A2.item_id WHERE MAC002A2.machine_no='" + dt.Rows[i]["设备编号"] + @"'";
                            dt_itme = DB.GetDataTable(sql);
                        }
                        for (int j = 0; j < datelist.Count; j++)//循环生成条数 
                        {
                            for (int k = 0; k < dt_itme.Rows.Count; k++)
                            {
                                //往对应表插入数据
                                //sql = @"INSERT INTO " + tablename + "(machine_no,plan_source,sorting,date_plan,item_fix,state,createby,createdate,createtime) VALUES('" + dt.Rows[i]["设备编号"] + "','" + paln_no + "','" + (j + 1) + "','" + Convert.ToDateTime(datelist[j]).ToString("yyyy-MM-dd") + "','"+dt_itme.Rows[k]["item_name"]+"','计划','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                                string sqlnowID = @"select nvl(max(id),0)+1 from " + tablename + "";

                                string nowID = DB.GetString(sqlnowID);
                                sql = "INSERT INTO " + tablename;                               
                                sql += @"(""ID"",MACHINE_NO,PLAN_SOURCE,SORTING,DATE_PLAN,ITEM_FIX,STATE,CREATEBY,CREATEDATE,CREATETIME) VALUES(" + nowID + ",'" + dt.Rows[i]["设备编号"] + "','" + paln_no + "','" + (j + 1) + "','" + Convert.ToDateTime(datelist[j]).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dt_itme.Rows[k]["item_name"] + "','计划','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                                DB.ExecuteNonQueryOffline(sql);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)  //循环设备明细
                    {
                        string sql = string.Empty;
                        DataTable dt_itme = new DataTable();
                        string result = string.Empty;

                        //获取当前设备绑定了多少个保养内容项或者校正计划
                        if (paln_type == "保养")
                        {
                            sql = @"SELECT item_name FROM MAC001M LEFT JOIN MAC001A2 ON MAC001M.item_id=MAC001A2.item_id WHERE MAC001A2.machine_no='" + dt.Rows[i]["设备编号"] + @"'";
                            dt_itme = DB.GetDataTable(sql);
                        }
                        else
                        {
                            sql = @"SELECT item_name FROM MAC002M LEFT JOIN MAC002A2 ON MAC002M.item_id=MAC002A2.item_id WHERE MAC002A2.machine_no='" + dt.Rows[i]["设备编号"] + @"'";
                            dt_itme = DB.GetDataTable(sql);
                        }
                        for (int j = 0; j < datelist.Count; j++)//循环生成条数 
                        {
                            for (int k = 0; k < dt_itme.Rows.Count; k++)
                            {
                                //往对应表插入数据
                                //sql = @"INSERT INTO " + tablename + "(machine_no,plan_source,sorting,date_plan,item_fix,state,createby,createdate,createtime) VALUES('" + dt.Rows[i]["设备编号"] + "','" + paln_no + "','" + (j + 1) + "','" + Convert.ToDateTime(datelist[j]).ToString("yyyy-MM-dd") + "','" + dt_itme.Rows[k]["item_name"] + "','计划','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                                string sqlnowID = @"select nvl(max(id),0)+1 from " + tablename + "";
                                string nowID = DB.GetString(sqlnowID);
                                sql = "INSERT INTO " + tablename;
                                sql += @"(""ID"",MACHINE_NO,PLAN_SOURCE,SORTING,DATE_PLAN,ITEM_FIX,STATE,CREATEBY,CREATEDATE,CREATETIME) VALUES(" + nowID + ",'" + dt.Rows[i]["设备编号"] + "','" + paln_no + "','" + (j + 1) + "','" + Convert.ToDateTime(datelist[j]).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dt_itme.Rows[k]["item_name"] + "','计划','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                                RetData = RetData + sql;
                                DB.ExecuteNonQueryOffline(sql);
                            }
                        }
                    }
                }
                #endregion
                IsSuccess = true;
                RetData += "保存成功!";

            }
            catch (Exception ex)
            {
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
