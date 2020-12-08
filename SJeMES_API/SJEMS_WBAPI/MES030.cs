using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class MES030
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
        public static string CkMachineNo_Register(object OBJ)
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

            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 接口参数
                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);


                #region 逻辑
                /*
                 接口逻辑：
   1：根据【machine_no】：设备号校验机器状态。
      1.1：当扫入设备号时，检测设备工作状态，      
           SQL: select * from MES030M a where （a.productimachine_no= 条码【machine_no】 
                if “找不到资料”  then 
                    返回参数：【IsSuccess】 = "FLASE"   ,返回信息提示：“此机器码不存在”
                else if  找到机器设备资料，但是 【work_state】工作状态 <> 【维护】 then 
                     返回参数：【IsSuccess】 = "FLASE"   ,返回信息提示：“此机器码不在维护中”
                else 
                     返回参数：【IsSuccess】 = "TRUE" 
                     附带返回：【设备名称】【设备描述】【工作中心】【型号】【故障现象】                                      
                end 
                 */
                string sql = string.Empty;
                sql = @"SELECT machine_name AS '设备名称',description AS '设备描述',work_center AS '工作中心',type AS '型号',(SELECT top(1) bad_no FROM MES030A3(NOLOCK) WHERE machine_no='" + machine_no + "' AND state='N') AS '故障现象',work_state AS '工作状态' FROM MES030M a WHERE a.machine_no='" + machine_no + "'";
                DataTable dt = DB.GetDataTable(sql);

                if (dt.Rows.Count == 0)
                {
                    IsSuccess = false;
                    RetData = "此机器码不存在";
                }
                else if (dt.Rows.Count > 0 && dt.Rows[0]["工作状态"].ToString() != "维护")
                {
                    IsSuccess = false;
                    RetData = "此机器码不在维护中";
                }
                else
                {
                    string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                    IsSuccess = true;
                    RetData = "<DataTable>" + dtXML + @"</DataTable>";
                }
                #endregion
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
        public static string Confirm_Register(object OBJ)
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

            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 接口参数
                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");//设备号
                string memo = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<memo>", "</memo>");//备注
                string reason_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<reason_no>", "</reason_no>");//故障原因
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");//登录人
                string DataTable= GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<dt_MES030A7>", "</dt_MES030A7>");
                DataTable dt_MES030A7 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);


                #region 逻辑
                /*
   1：处理设备维护报警记录，
      1.1：在设备维护记录表【MES030A3】中表更新维护记录，根据【设备号machine_no】及【状态state】更新字段
              （【维修日期date_fix】【维修时间time_fix】【维修人员person_fix】【故障原因reason_no】【处理状态state】【备注memo】【修改日期modifydate】【修改人员modifyby】【修改时间modifytime】）
           返回参数【IsSucess】="True"   ,返回信息“维修成功”
                 */
                string sql = string.Empty;
                sql = @"UPDATE MES030A3 SET date_fix='" + DateTime.Now.ToString("yyyy-MM-dd") + "',time_fix='" + DateTime.Now.ToString("HH:mm:ss") + "',person_fix='" + UserCode + "',reason_no='" + reason_no + "',state='Y',memo=memo+';" + memo + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + "',modifyby='" + UserCode + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' WHERE machine_no='" + machine_no + "' AND state='N'";
                // DB.ExecuteNonQueryOffline(sql);

                sql += @" UPDATE MES030M SET work_state='工作中',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' WHERE machine_no='" + machine_no + "' AND work_state='维护'";
                DB.ExecuteNonQueryOffline(sql);

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
                        sql = "INSERT INTO MES030A7(machine_no,sorting,date_plan,products_barcode,lot_barcode,packing_barcode,production_code,material_no,material_name,qty,memo,org,createby,createdate,createtime) "+
                            @"VALUES('"+ machine_no + "','"+ dt_MES030A7.Rows[i]["sorting"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','"+ dt_MES030A7.Rows[i]["products_barcode"] + 
                            @"','"+ dt_MES030A7.Rows[i]["lot_barcode"] + "','"+ dt_MES030A7.Rows[i]["packing_barcode"] + "','"+ dt_MES030A7.Rows[i]["production_code"] + "','"+ dt_MES030A7.Rows[i]["material_no"] +
                            "','"+ dt_MES030A7.Rows[i]["material_name"] + "','"+ dt_MES030A7.Rows[i]["qty"] + "','"+ dt_MES030A7.Rows[i]["memo"] + "','"+ dt_MES030A7.Rows[i]["org"] + 
                            @"','"+ dt_MES030A7.Rows[i]["createby"] + "','"+ DateTime.Now.ToString("yyyy-MM-dd") + "','"+ DateTime.Now.ToString("HH:mm:ss") + "')";
                        DB.ExecuteNonQueryOffline(sql);
                    }
                   
                }
                
                IsSuccess = true;
                RetData = "维修成功";
                #endregion
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

        public static string CkMachineNo(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = true;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            GDSJ_Framework.DBHelper.Oracle DB = getOracleDb();

            try
            {
                GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");// DllName = "SJEMS_WBAPI";//
                GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>"); //ClassName = "SJEMS_WBAPI.WMS003";//
                GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");// Method = "GETDATA";//
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                //guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 接口参数
                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<machine_no>", "</machine_no>");//机器码

                string flat = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<flat>", "</flat>");

                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");//操作人（新增、修改、审核）

                DataTable dt;
                string where = string.Empty;

                string sql = string.Empty;
                #endregion

                #region 逻辑
                //          1：根据【machine_no】：设备号校验【机器设备】及设备【工作状态】。
                //1.1：当扫入机器编号时，检测机器设备主档【MES030M】是否存在以及【工作状态】是否相符。      
                //     SQL: select* from MES030M a where a.machine_no = 条码【machine_no】 
                //          if “找不到资料”  then
                //              返回参数：【IsSuccess】 = "FLASE"   ,返回信息提示：“此机器码不存在”
                //          else if  找到机器设备资料，但是 【work_state】工作状态 = 【维护】 then
                //               返回参数：【IsSuccess】 = "FLASE"   ,返回信息提示：“此设备在维护中”
                //          else 
                //               返回参数：【IsSuccess】 = "TRUE"
                //               附带返回： 【设备名称】【设备描述】【工作中心】【型号】                                    
                //          end
                //工作中，保养，维护

                sql = @"
select COUNT(*) from MES030M where machine_no='" + machine_no + @"'
union all
select COUNT(*) from MES030M where machine_no='" + machine_no + @"' and work_state='维护'";

                DataTable dt_ck = DB.GetDataTable(sql);

                if (flat == "1")//报警
                {

                    if (dt_ck.Rows[0][0].ToString() == "0")
                    {
                        IsSuccess = false;
                        RetData = "此机器码不存在";
                    }
                    else
                    {
                        if (dt_ck.Rows[1][0].ToString() != "0")
                        {
                            IsSuccess = false;
                            RetData = "此设备在维护中";
                        }
                    }
                }

                if (flat == "2")//登记
                {

                    if (dt_ck.Rows[0][0].ToString() == "0")
                    {
                        IsSuccess = false;
                        RetData = "此机器码不存在";
                    }
                    else
                    {
                        if (dt_ck.Rows[1][0].ToString() == "0")
                        {
                            IsSuccess = false;
                            RetData = "此设备不在维护中";
                        }
                    }
                }

                if (IsSuccess)
                {
                    sql = "SELECT [machine_no],[machine_name],[description],[type],[work_center] FROM [MES030M](nolock) where machine_no='" + machine_no + "'";
                    dt = DB.GetDataTable(sql);

                    //string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);
                    string machine_name = dt.Rows[0]["machine_name"].ToString();
                    string description = dt.Rows[0]["description"].ToString();
                    string type = dt.Rows[0]["type"].ToString();
                    string work_center = dt.Rows[0]["work_center"].ToString();
                  //  string memo = dt.Rows[0]["memo"].ToString();

                    RetData = "<machine_name>" + machine_name + @"</machine_name><description>" + description + @"</description><type> " + type + @" </type><work_center>" + work_center + @"</work_center><memo>" + DB.GetString("SELECT memo FROM [MES030A3](nolock) where machine_no='" + machine_no + "' and state='N'") + @"</memo>";
                    if (flat == "2")
                    {
                        RetData += "<trouble_no>" + DB.GetString("SELECT M.trouble_name FROM [MES030A3](nolock) A3 left join MES005M(nolock) M on M.trouble_no=A3.trouble_no  where machine_no='"+ machine_no + "' and state='N'") + "</trouble_no>";
                    }
                }

                #endregion

               // GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                string[] s1 = new string[1]; s1[0] = "\r\n"; string[] s = ex.StackTrace.Split(s1, StringSplitOptions.RemoveEmptyEntries); RetData = "00000:" + ex.Message + "\r\n" + s[s.Length - 1];

                //GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
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

        public static string Confirm_Alarm(object OBJ)
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

                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string memo = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<memo>", "</memo>");
                string trouble_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<trouble_no>", "</trouble_no>");

                //string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<MES010A4>", "</MES010A4>");
                //DataTable MES010A4 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);

                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserCode>", "</UserCode>");

                string org = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<org>", "</org>");

                string orgno = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<orgno>", "</orgno>"); //报警组织编号
                string MsgType = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MsgType>", "</MsgType>"); //消息类型
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 

                //          1：产生设备维护报警记录，
                //1.1：在设备维护记录表【MES030A3】中表写入维护记录
                //        （【设备编号】【序号】【报修日期】【报修时间】【报修人员】【故障现象】【处理状态】【备注】
                //        【组织】【是否启用】【建立日期】【建立人员】【建立时间】）
                //     返回参数【IsSucess】= "True"   ,返回信息“报修成功”

                #region 产生序号
                string sorting = DB.GetString("SELECT max(CONVERT(int,sorting)) FROM MES030A3 where machine_no='" + machine_no + "'");
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "001";
                }
                else
                {
                    sorting = (int.Parse(sorting) + 1).ToString("000");
                }
                #endregion

                sql = @"
Insert into MES030A3([machine_no]
      ,[sorting]
      ,[date_report]
      ,[time_report]
      ,[trouble_no]
      ,[state]
      ,[memo]
      ,[org]
    
      ,[createby]
      ,[createdate]
      ,[createtime] ) values ('" + machine_no + "','" + sorting + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + trouble_no + "','N','" + memo + "','" + org + "','" + UserCode + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";

                //int i = DB.ExecuteNonQueryOffline(sql);
                #region 报警发送
                string WinXinMsg = @"{" + '"' + "keyword1" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "设备编号：" + machine_no + ",故障现象：" + trouble_no + '"' + "}," + '"' + "keyword2" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + '"' + "}," + '"' + "remark" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "" + '"' + "}," + '"' + "first" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "【商基网络】" + '"' + "}}";


                string EmailMsg = @"【商基网络】设备编号：" + machine_no + ",故障现象：" + trouble_no;

                string SMSMsg = @"【商基网络】设备编号：" + machine_no + ",故障现象：" + trouble_no;

                //Alarm.SendAlarm("设备", OBJ, WinXinMsg, EmailMsg, SMSMsg,"1");

                string isql = @"INSERT INTO AWASC002M(NewMsg,WxNewMsg,OrgNo,NoticeTime，HandleState，createby，createdate，createtime)VALUES('" + SMSMsg + "','" + WinXinMsg + "','" + orgno + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','N','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                DB.ExecuteNonQueryOffline(sql);

                #endregion

                sql += @" UPDATE MES030M SET work_state='维护',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' WHERE machine_no='" + machine_no + "'  AND work_state !='维护'";
                DB.ExecuteNonQueryOffline(sql);

                IsSuccess = true;
                RetData = "报修成功";


                #endregion
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
        /// PC端获取保养数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetData_BaoYang_PC(object OBJ)
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

                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserName>", "</UserName>");
                string org = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<org>", "</org>");

                string machine_name = string.Empty;
                string work_state = string.Empty;
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                sql = @"
select machine_name 机器名称  from  MES030M where machine_no ='" + machine_no + "'";
                machine_name = DB.GetString(sql);


                if (string.IsNullOrEmpty(machine_name))
                {
                    RetData = "不存在该设备条码！";
                    IsSuccess = false;
                }
                else
                {
                    //查询机械状态
                    sql = @"
select work_state 机械状态 from  MES030M where machine_no ='" + machine_no + "'";
                    work_state = DB.GetString(sql);
                    sql = @"select owner 管理人员  from  MES030M where machine_no ='" + machine_no + "'";
                    string owner = DB.GetString(sql);
                    //存在设备条码获取设备计划信息
                    //sql = @"select sorting as '序号',date_plan as '计划日期',state as 状态,item_fix as '保养内容项',state_machine as 设备状态, from MES030A4(NOLOCK) where machine_no='" + machine_no + "' and date_plan<='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    sql = @"select sorting 序号,date_plan 计划日期,plan_no 计划编号,state 状态  from MES030A4 where machine_no='" + machine_no + "' " +
                        "and date_plan<='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and state='计划'";
                    DataTable dtMES030A4 = DB.GetDataTable(sql);
                    //dtMES030A4.DefaultView.Sort = "计划日期 DESC";
                    //dtMES030A4 = dtMES030A4.DefaultView.ToTable();
                    bool b = false;
                    for (int i = 0; i < dtMES030A4.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (dtMES030A4.Rows[i]["状态"].ToString() == "完成")
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    if (!b)
                    {
                        //获取历史保养数据  前10
                        sql = @"select date_plan 计划日期, date_fix 保养日期,person_fix 保养人员,state_machine 机械状态,memo_fix 处理意见 
from MES030A4 where machine_no='"+ machine_no + "' and state='完成'  and ROWNUM<=10  order by date_fix desc";
                        DataTable olddtMES030A4 = DB.GetDataTable(sql);

                        //获取MAC001A2对应的设备信息以及MAC001M的对应的保养内容项
                        sql = @"select MAC001M.item_id 内容项编号,item_name 内容项名称 from MAC001M left join MAC001A2 on MAC001M.item_id=MAC001A2.item_id where machine_no='" + machine_no + "'";
                        DataTable dtMAC001 = DB.GetDataTable(sql);


                        string strMES030A4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMES030A4);
                        string stroldMES030A4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(olddtMES030A4);
                        string strMAC001 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMAC001);

                        RetData = @"<MES030A4>" + strMES030A4 + @"</MES030A4><oldMES030A4>" + stroldMES030A4 + @"</oldMES030A4><MAC001>" + strMAC001 + @"</MAC001><machine_name>" + machine_name + @"</machine_name><machine_no>"+machine_no+@"</machine_no><work_state>" + work_state + @"</work_state><owner>" + owner + "</owner>";
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                        RetData = "该设备已做过保养!";
                    }
                }
                    
                


                #endregion
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
        /// PC端获取校正数据
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetData_JiaoZheng_PC(object OBJ)
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

                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserName>", "</UserName>");
                string org = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<org>", "</org>");

                string machine_name = string.Empty;
                string work_state = string.Empty;
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                sql = @"
select machine_name 机器名称  from  MES030M where machine_no ='" + machine_no + "'";
                machine_name = DB.GetString(sql);

                if (string.IsNullOrEmpty(machine_name))
                {
                    RetData = "不存在该设备条码！";
                    IsSuccess = false;
                }
                else
                {
                    //查询机械状态
                    sql = @"
select work_state 机械状态 from  MES030M where machine_no ='" + machine_no + "'";
                    work_state = DB.GetString(sql);
                    sql = @"select owner 管理人员 from  MES030M where machine_no ='" + machine_no + "'";
                    string owner = DB.GetString(sql);
                    //存在设备条码获取设备计划信息
                    sql = @"select sorting 序号,date_plan 计划日期, plan_no 计划编号,state 状态 from MES030A5 where machine_no='" + machine_no + "' and date_plan<='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and state='计划'";
                    DataTable dtMES030A5 = DB.GetDataTable(sql);
                    dtMES030A5.DefaultView.Sort = "计划日期 DESC";
                    dtMES030A5 = dtMES030A5.DefaultView.ToTable();
                    bool b = false;
                    for (int i = 0; i < dtMES030A5.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (dtMES030A5.Rows[i]["状态"].ToString() == "完成")
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    if (!b)
                    {
                        //获取历史保养数据  前10
                        sql = @"select date_plan 计划日期, date_fix 校正日期,person_fix 校正人员,state_machine 机械状态,memo_fix 处理意见  
                    from MES030A5 where machine_no='" + machine_no + "' and state='完成'  and ROWNUM<=10  order by date_fix desc";

                        //DataTable olddtMES030A5 = DB.GetDataTable(sql);
                        DataTable olddtMES030A5 = DB.GetDataTable(sql);
                        //获取MAC001A2对应的设备信息以及MAC001M的对应的保养内容项
                        sql = @"select MAC002M.item_id 内容项编号,item_name 内容项名称 from MAC002M left join MAC002A2 on MAC002M.item_id=MAC002A2.item_id where machine_no='" + machine_no + "'";
                        //DataTable dtMAC002 = DB.GetDataTable(sql);
                        DataTable dtMAC002 = DB.GetDataTable(sql);

                        string strMES030A5 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMES030A5);
                        string stroldMES030A5 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(olddtMES030A5);
                        string strMAC002 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMAC002);


                        RetData = @"<MES030A5>" + strMES030A5 + @"</MES030A5><oldMES030A5>" + stroldMES030A5 + @"</oldMES030A5><MAC002>" + strMAC002 + @"</MAC002><machine_name>" + machine_name + @"</machine_name><machine_no>" + machine_no + "</machine_no><work_state>" + work_state + @"</work_state>";
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                        RetData = "该设备已做过校正!";
                    }

                }



                #endregion
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
        /// 设备保养数据查询PDA
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetData_BaoYang(object OBJ)
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

                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserName>", "</UserName>");
                string org = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<org>", "</org>");

                string machine_name = string.Empty;
                string work_state = string.Empty;
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                sql = @"
select machine_name  from  MES030M where machine_no ='" + machine_no + "'";
                machine_name = DB.GetString(sql);


                if (string.IsNullOrEmpty(machine_name))
                {
                    RetData = "不存在该设备条码！";
                    IsSuccess = false;
                }
                else
                {
                    //查询机械状态
                    sql = @"
select work_state  from  MES030M where machine_no ='" + machine_no + "'";
                    work_state = DB.GetString(sql);
                    sql = @"select owner  from  MES030M where machine_no ='" + machine_no + "'";
                    string owner = DB.GetString(sql);

                    //存在设备条码获取设备计划信息
                    //sql = @"select sorting as '序号',date_plan as '计划日期',state as 状态,item_fix as '保养内容项',state_machine as 设备状态, from MES030A4(NOLOCK) where machine_no='" + machine_no + "' and date_plan<='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    sql = @"select sorting,date_plan,[state] from MES030A4(NOLOCK) where machine_no='" + machine_no + "' and date_plan<='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and state='计划'";
                    DataTable dtMES030A4 = DB.GetDataTable(sql);
                    dtMES030A4.DefaultView.Sort = "date_plan DESC";
                    dtMES030A4 = dtMES030A4.DefaultView.ToTable();
                    bool b = false;
                    for (int i = 0; i < dtMES030A4.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (dtMES030A4.Rows[i]["state"].ToString() == "完成")
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    if (!b)
                    {
                        //获取历史保养数据  前10
                        sql = @"select date_plan , date_fix ,person_fix,item_fix,state_machine,memo_fix
from MES030A4 where machine_no='"+ machine_no + "' and state='完成'  and ROWNUM<=10  order by date_fix desc";
                        DataTable olddtMES030A4 = DB.GetDataTable(sql);

                        //获取MAC001A2对应的设备信息以及MAC001M的对应的保养内容项
                        sql = @"select MAC001M.item_id ,item_name  from MAC001M left join MAC001A2 on MAC001M.item_id=MAC001A2.item_id where machine_no='" + machine_no + "'";
                        DataTable dtMAC001 = DB.GetDataTable(sql);
                        //内容明细
                        sql = @"select MAC001A2.item_id,MAC001A2.machine_no,MAC001A2.machine_name,MAC001A1.value,MAC001M.item_name from MAC001A2 
left join MAC001A1 on MAC001A2.item_id=MAC001A1.item_id 
left join MAC001M on MAC001M.item_id=MAC001A2.item_id where MAC001A2.machine_no='" + machine_no + "'";
                        DataTable dtMAC001A2 = DB.GetDataTable(sql);


                        string strMES030A4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMES030A4);
                        string stroldMES030A4 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(olddtMES030A4);
                        string strMAC001 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMAC001);
                        string MAC001A2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMAC001A2);

                        RetData = @"<MES030A4>" + strMES030A4 + @"</MES030A4><oldMES030A4>" + stroldMES030A4 + @"</oldMES030A4><MAC001>" + strMAC001 + @"</MAC001><machine_name>" + machine_name + @"</machine_name><MAC002A2>" + MAC001A2 + @"</MAC002A2><work_state>" + work_state + @"</work_state><machine_no>" + machine_no + "</machine_no>";
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                        RetData = "该设备已做过保养!";
                    }
                    
                }
                #endregion

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
        /// 设备校正数据获取PDA
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string GetData_JiaoZheng(object OBJ)
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

                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                string username = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<UserName>", "</UserName>");
                string org = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<org>", "</org>");

                string machine_name = string.Empty;
                string work_state = string.Empty;
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                sql = @"
select machine_name 机器名称  from  MES030M where machine_no ='" + machine_no + "'";
                machine_name = DB.GetString(sql);


                if (string.IsNullOrEmpty(machine_name))
                {
                    RetData = "不存在该设备条码！";
                    IsSuccess = false;
                }
                else
                {
                    //查询机械状态
                    sql = @"
select work_state from  MES030M where machine_no ='" + machine_no + "'";
                    work_state = DB.GetString(sql);
                    sql = @"select owner  from  MES030M where machine_no ='" + machine_no + "'";
                    string owner = DB.GetString(sql);
                    //存在设备条码获取设备计划信息
                    sql = @"select sorting,date_plan,state,item_fix,state_machine from MES030A5(NOLOCK) where machine_no='" + machine_no + "' and date_plan<='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and state='计划'";
                    DataTable dtMES030A5 = DB.GetDataTable(sql);
                    dtMES030A5.DefaultView.Sort = "date_plan DESC";
                    dtMES030A5 = dtMES030A5.DefaultView.ToTable();
                    bool b = false;
                    for (int i = 0; i < dtMES030A5.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (dtMES030A5.Rows[i]["state"].ToString() == "完成")
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    if (!b)
                    {
                        //获取历史保养数据  前10
                        sql = @"select date_plan, date_fix,person_fix,item_fix,state_machine,memo_fix 
from MES030A5 where machine_no='"+ machine_no + "' and state='完成'   and ROWNUM<=10 and date_plan <='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' order by date_fix desc";
                        DataTable olddtMES030A5 = DB.GetDataTable(sql);

                        if (dtMES030A5.Rows.Count > 0)
                        {
                            //获取MAC001A2对应的设备信息以及MAC001M的对应的保养内容项
                            sql = @"select MAC002M.item_id,item_name from MAC002M left join MAC002A2 on MAC002M.item_id=MAC002A2.item_id where machine_no='" + machine_no + "'";
                            DataTable dtMAC002 = DB.GetDataTable(sql);
                            //内容明细
                            sql = @"select MAC002A2.item_id,MAC002A2.machine_no,MAC002A2.machine_name,MAC002A1.value,MAC002M.item_name from MAC002A2 
left join MAC002A1 on MAC002A2.item_id=MAC002A1.item_id 
left join MAC002M on MAC002M.item_id=MAC002A2.item_id where MAC002A2.machine_no='" + machine_no + "'";
                            DataTable dtMAC002A2 = DB.GetDataTable(sql);


                            string strMES030A5 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMES030A5);
                            string stroldMES030A5 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(olddtMES030A5);
                            string strMAC002 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMAC002);
                            string MAC002A2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dtMAC002A2);

                            RetData = @"<MES030A4>" + strMES030A5 + @"</MES030A4><oldMES030A4>" + stroldMES030A5 + @"</oldMES030A4><MAC001>" + strMAC002 + @"</MAC001><machine_name>" + machine_name + @"</machine_name><MAC002A2>" + MAC002A2 + @"</MAC002A2><machine_no>" + machine_no + "</machine_no><work_state>" + work_state + @"</work_state>";
                            IsSuccess = true;
                        }
                        else
                        {
                            IsSuccess = false;
                            RetData = "该机器没有校正计划!";
                        }

                       
                    }
                    else
                    {
                        IsSuccess = false;
                        RetData = "该设备已做过校正!";
                    }
                    
                }



                #endregion
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

                string machine_no = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<machine_no>", "</machine_no>");
                #endregion

                //DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                #region 逻辑 
                if (!String.IsNullOrEmpty(machine_no))
                {
                    string machine_name = DB.GetString("select machine_name from MES030M where machine_no='"+ machine_no + "'");
                    if (!string.IsNullOrEmpty(machine_name))
                    {
                        RetData = machine_name;
                        IsSuccess = true;
                    }
                    else
                    {
                        RetData = "不存在此数据！";
                        IsSuccess = false;
                    }
                }
                else
                {
                    RetData = "传入参数为空！";
                    IsSuccess = false;
                }
                #endregion
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
