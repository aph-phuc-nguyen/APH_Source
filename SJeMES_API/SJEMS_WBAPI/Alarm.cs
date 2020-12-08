using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace SJEMS_WBAPI
{
   
    class Alarm
    {
        private static string Server = string.Empty;
        private static string UserName = string.Empty;
        private static string Pwd = string.Empty;
        private static string DBName = string.Empty;
        private static string wx_appid = string.Empty;
        private static string wx_secret = string.Empty;
        private static string SendText = string.Empty;
        private static GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
        /// <summary>
        /// 发送消息到dmp消息平台
        /// </summary>
        /// <param name="AlarmType">报警类型</param>
        /// <param name="OBJ"></param>
        /// <param name="WinXinMsg">微信的消息内容</param>
        /// <param name="EmailMsg">邮件的消息内容</param>
        /// <param name="SMSMsg">短信的消息内容</param>
        public static void SendAlarm(string AlarmType,object OBJ,string WinXinMsg,string EmailMsg,string SMSMsg,string sorting)
        {
            try
            {
                
                
                string XMLOBJ = (string)OBJ;
                //判断是否存在dmp数据库的config文件
                if (File.Exists(Application.StartupPath + @"DMPDBConfig.xml"))
                {
                    return;
                }
                else
                {
                    //读取xml文件的数据库连接

                    string XML = File.ReadAllText(Application.StartupPath + @"\DMPDBConfig.xml");
                    Server = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<SERVER>", "</SERVER>");
                    UserName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<USER>", "</USER>");
                    Pwd = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<PWD>", "</PWD>");
                    DBName= GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DB>", "</DB>");
                    wx_appid= GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<wx_appid>", "</wx_appid>");
                    wx_secret = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<wx_secret>", "</wx_secret>");
                    DB = new GDSJ_Framework.DBHelper.DataBase(XMLOBJ);


                    //判断报警类型 生产进度、质量、缺料、设备
                    //根据不同的类型查询AWACS001绑定的员工
                    string sql = string.Empty;

                    if (AlarmType == "生产进度")
                    {
                        InsertData("生产进度",WinXinMsg,EmailMsg,SMSMsg,sorting);
                    }
                    else if (AlarmType == "质量")
                    {
                        InsertData("质量",WinXinMsg,EmailMsg,SMSMsg,sorting);
                    }
                    else if (AlarmType == "缺料")
                    {
                        InsertData("缺料", WinXinMsg, EmailMsg, SMSMsg,sorting);
                    }
                    else if (AlarmType == "设备")
                    {
                        InsertData("设备", WinXinMsg, EmailMsg, SMSMsg,sorting);
                    }
                }
            }catch(Exception ex)
            {

            }
           
        }

        /// <summary>
        ///插入对应表数据 
        /// </summary>
        /// <param name="QueryType">查询类型</param>
        private static void InsertData(string QueryType, string WinXinMsg, string EmailMsg, string SMSMsg,string sorting)
        {
            try
            {
                GDSJ_Framework.DBHelper.DataBase DMPDB = new GDSJ_Framework.DBHelper.DataBase();
                DMPDB = new GDSJ_Framework.DBHelper.DataBase("SqlServer", Server, DBName, UserName, Pwd, string.Empty);
                string sql = string.Empty;

                sql = @"SELECT AWACS001A1.person_no as '员工编号',AWACS001A1.person_name as '员工名称' FROM AWACS001M 
LEFT JOIN AWACS001A1 ON AWACS001M.alarm_no=AWACS001A1.alarm_no
WHERE AWACS001M.alarm_type='" + QueryType + @"' AND AWACS001A1.hierarchy='"+sorting+@"' 
GROUP BY AWACS001A1.person_no,AWACS001A1.person_name";
                DataTable dt = DB.GetDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++) //循环与绑定对应的报警类型员工
                    {
                        //循环查询对应的员工是以什么形式发送报警信息 HR001M表
                        sql = @"SELECT staff_mobile as '移动电话',staff_qq as 'QQ号码',staff_email as '电子邮箱',isweixin as '微信',isemail as '邮箱',issms as '短信',openid 
FROM HR001M(NOLOCK) WHERE staff_no='" + dt.Rows[i]["员工编号"] + "' AND staff_name='" + dt.Rows[i]["员工名称"] + "'";
                        DataTable Pdt = DB.GetDataTable(sql);
                        for (int j = 0; j < Pdt.Rows.Count; j++)
                        {
                            if (Pdt.Rows[i]["微信"].ToString() == "True")
                            {
                                //往DMP的微信数据库插入一条报警信息
                                sql = @"INSERT INTO ORDER_WEIXINMODEL(usercode,weixincode,wx_appid,wx_secret,wx_platform,model_id,model_name,model_data,model_url,datetime) VALUES('NULL','" + Pdt.Rows[i]["openid"] + "','" + wx_appid + "','" + wx_secret + "','EMS','-busI45-z9wKu_gahiKuOQ6EbWMTIy1wWs29wiUEwGg','NULL','" + WinXinMsg + "','NULL','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                DMPDB.ExecuteNonQueryOffline(sql);

                            }
                            if (Pdt.Rows[i]["短信"].ToString() == "True")
                            {
                                sql = @"INSERT INTO ORDER_SMS(usercode,phone,[date],[time],sendtext) VALUES('NULL','" + Pdt.Rows[i]["移动电话"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + SMSMsg + "')";
                                DMPDB.ExecuteNonQueryOffline(sql);
                            }
                            if (Pdt.Rows[i]["邮箱"].ToString() == "True")
                            {
                                sql = @"INSERT INTO ORDER_EMAIL(usercode,email,[date],[time],sendtext) VALUES('NULL','" + Pdt.Rows[i]["电子邮箱"] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + EmailMsg + "')";
                                DMPDB.ExecuteNonQueryOffline(sql);
                            }
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {

            }
            
        }

        /// <summary>
        /// 验证是否需要报警升级
        /// </summary>
        public static string UpgradeWarm(object OBJ)
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

            try
            {
                GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
                DB = new GDSJ_Framework.DBHelper.DataBase(XML);


                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                string type = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<type>", "</type>"); //报警类型
                string sorting = string.Empty;
                if (type == "缺料")
                {
                    sorting = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<sorting>", "</sorting>");
                }
                else if(type=="质量")
                {
                    sorting = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<sorting_ZL>", "</sorting_ZL>");
                }
                else if (type == "设备")
                {
                    sorting = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<sorting_SB>", "</sorting_SB>");
                }
                else if (type == "生产进度")
                {
                    sorting = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<sorting_SCJD>", "</sorting_SCJD>");
                }


                //获取第一层级人员的处理时间限制
                string sql = @"SELECT A1.limited_time FROM AWACS001M M LEFT JOIN AWACS001A1 A1 ON M.alarm_no=A1.alarm_no
WHERE A1.hierarchy='" + sorting + "' AND M.alarm_type='" + type + "'";
                int time = DB.GetInt32(sql);
                sql = @"SELECT A1.limited_time FROM AWACS001M M LEFT JOIN AWACS001A1 A1 ON M.alarm_no=A1.alarm_no
WHERE A1.hierarchy='1' AND M.alarm_type='" + type + "'";
                int time1 = DB.GetInt32(sql);

                if (type == "缺料")
                {
                    sql = @"SELECT productionline_no,productionsite_no,material_no,material_name,material_spec,qty_unit,date_jit,times_start 
FROM MES100A1 A1 LEFT JOIN MES100M M ON A1.jit_doc=M.jit_doc WHERE status='N'";
                }else if (type == "质量")
                {
                    sql = @"SELECT production_order as '生产工单',route_order as '途程工单',productionline_no as '生产线'
,productionsite_no as '站点',material_no as '物料代号',material_name as '物料名称'
,material_spec as '规格',sorting as '工序',procedure_no as '工序代号',procedure_name as '工序名称',bad as '不良现象'
,bad_reason as '不良原因',qty as '不良数量' 
FROM MES120M(NOLOCK) WHERE state='N'";
                }
                else if (type == "生产进度")
                {
                    sql = @"SELECT production_order as '工单',material_no as '产品代号',material_name as '产品名称'
,material_spec as '规格',qty_plan as '计划数量',qty_finish as '完成数量',date_start as '开始日期'
,date_finish_plan as '计划完工日期',type_report as '报警类型',memo_report as '报警说明' FROM MES130M(NOLOCK) where state_do = 'N'";
                }else if (type == "设备")
                {
                    sql = @"SELECT machine_no as '设备编号',trouble_no as '故障现象' FROM MES030A3(NOLOCK) WHERE state='N'";
                }
                //获取当前叫料数据表未处理的数据
                
                DataTable dt = DB.GetDataTable(sql);

                //判断是否存在未处理的数据
                if (dt.Rows.Count > 0)
                {
                    //如果存在未处理数据，对未处理数据进行判断，是否已经超过对应层级的人员的处理时间
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //存在超出处理的数据将给下一层级的人员发送报警信息
                        DateTime dtiem = Convert.ToDateTime(dt.Rows[i]["date_jit"].ToString() +" "+ dt.Rows[i]["times_start"].ToString());//时间转换
                        DateTime aa = DateTime.Now.AddMinutes(-time);
                        if (dtiem < DateTime.Now.AddMinutes(-time))
                        {
                            //调用发送报警消息方法
                            //拼接报警消息
                            if (type == "缺料")
                            {
                                #region 缺料报警消息
                                string WinXinMsg = @"{" + '"' + "keyword1" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + ",生产线：" + dt.Rows[i]["productionline_no"] + ",站点：" + dt.Rows[i]["productionsite_no"] + ",物料代号：" + dt.Rows[i]["material_no"].ToString() + ",物料名称：" + dt.Rows[i]["material_name"].ToString() + ",物料规格：" + dt.Rows[i]["material_spec"].ToString() + ",请求数量：" + dt.Rows[i]["qty_unit"].ToString() + '"' + "}," + '"' + "keyword2" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + '"' + "}," + '"' + "remark" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "" + '"' + "}," + '"' + "first" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "【商基网络】" + '"' + "}}";


                                string EmailMsg = @"【商基网络】生产线：" + dt.Rows[i]["productionline_no"] + ",站点：" + dt.Rows[i]["productionsite_no"] + ",物料代号：" + dt.Rows[i]["material_no"].ToString() + ",物料名称：" + dt.Rows[i]["material_name"].ToString() + ",物料规格：" + dt.Rows[i]["material_spec"].ToString() + ",请求数量：" + dt.Rows[i]["qty_unit"].ToString();
                                string SMSMsg = @"【商基网络】生产线：" + dt.Rows[i]["productionline_no"] + ",站点：" + dt.Rows[i]["productionsite_no"] + ",物料代号：" + dt.Rows[i]["material_no"].ToString() + ",物料名称：" + dt.Rows[i]["material_name"].ToString() + ",物料规格：" + dt.Rows[i]["material_spec"].ToString() + ",请求数量：" + dt.Rows[i]["qty_unit"].ToString();
                                SendAlarm("缺料", OBJ, WinXinMsg, EmailMsg, SMSMsg, (int.Parse(sorting) + 1).ToString());
                                #endregion
                            }
                            else if (type == "质量")
                            {
                                #region 质量报警消息
                                string WinXinMsg = @"{" + '"' + "keyword1" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "工单号：" + dt.Rows[i]["生产工单"] + ",途程单号：" + dt.Rows[i]["途程工单"] + ",生产线：" + dt.Rows[i]["生产线"] + ",站点：" + dt.Rows[i]["站点"] + ",物料代号：" + dt.Rows[i]["物料代号"].ToString() + ",物料名称：" + dt.Rows[i]["物料名称"].ToString() + ",物料规格：" + dt.Rows[i]["规格"].ToString() + ",工序：" + dt.Rows[i]["工序"].ToString() + ",工序代号：" + dt.Rows[i]["工序代号"].ToString() + ",工序名称：" + dt.Rows[i]["工序名称"].ToString() + ",不良现象：" + dt.Rows[i]["不良现象"].ToString() + ",不良原因：" + dt.Rows[i]["不良原因"].ToString() + ",不良数量" + dt.Rows[i]["不良数量"].ToString() + '"' + "}," + '"' + "keyword2" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + '"' + "}," + '"' + "remark" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "" + '"' + "}," + '"' + "first" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "【商基网络】" + '"' + "}}";


                                string EmailMsg = @"【商基网络】工单号：" + dt.Rows[i]["生产工单"] + ",途程单号：" + dt.Rows[i]["途程工单"] + ",生产线：" + dt.Rows[i]["生产线"] + ",站点：" + dt.Rows[i]["站点"] + ",物料代号：" + dt.Rows[i]["物料代号"].ToString() + ",物料名称：" + dt.Rows[i]["物料名称"].ToString() + ",物料规格：" + dt.Rows[i]["规格"].ToString() + ",工序：" + dt.Rows[i]["工序"].ToString() + ",工序代号：" + dt.Rows[i]["工序代号"].ToString() + ",工序名称：" + dt.Rows[i]["工序名称"].ToString() + ",不良现象：" + dt.Rows[i]["不良现象"].ToString() + ",不良原因：" + dt.Rows[i]["不良原因"].ToString() + ",不良数量" + dt.Rows[i]["不良数量"].ToString();

                                string SMSMsg = @"【商基网络】工单号：" + dt.Rows[i]["生产工单"] + ",途程单号：" + dt.Rows[i]["途程工单"] + ",生产线：" + dt.Rows[i]["生产线"] + ",站点：" + dt.Rows[i]["站点"] + ",物料代号：" + dt.Rows[i]["物料代号"].ToString() + ",物料名称：" + dt.Rows[i]["物料名称"].ToString() + ",物料规格：" + dt.Rows[i]["规格"].ToString() + ",工序：" + dt.Rows[i]["工序"].ToString() + ",工序代号：" + dt.Rows[i]["工序代号"].ToString() + ",工序名称：" + dt.Rows[i]["工序名称"].ToString() + ",不良现象：" + dt.Rows[i]["不良现象"].ToString() + ",不良原因：" + dt.Rows[i]["不良原因"].ToString() + ",不良数量" + dt.Rows[i]["不良数量"].ToString();

                                Alarm.SendAlarm("质量", OBJ, WinXinMsg, EmailMsg, SMSMsg, (int.Parse(sorting) + 1).ToString());
                                #endregion
                            }
                            else if (type == "设备")
                            {
                                #region 设备报警消息
                                string WinXinMsg = @"{" + '"' + "keyword1" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "设备编号：" + dt.Rows[i]["设备编号"] + ",故障现象：" + dt.Rows[i]["故障现象"] + '"' + "}," + '"' + "keyword2" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + '"' + "}," + '"' + "remark" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "" + '"' + "}," + '"' + "first" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "【商基网络】" + '"' + "}}";


                                string EmailMsg = @"【商基网络】设备编号：" + dt.Rows[i]["设备编号"] + ",故障现象：" + dt.Rows[i]["故障现象"];

                                string SMSMsg = @"【商基网络】设备编号：" + dt.Rows[i]["设备编号"] + ",故障现象：" + dt.Rows[i]["故障现象"];

                                Alarm.SendAlarm("设备", OBJ, WinXinMsg, EmailMsg, SMSMsg, (int.Parse(sorting) + 1).ToString());
                                #endregion
                            }
                            else if (type == "生产进度")
                            {
                                #region 生产进度报警消息
                                string WinXinMsg = @"{" + '"' + "keyword1" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "工单：" + dt.Rows[i]["工单"] + ",产品代号：" + dt.Rows[i]["产品代号"] + "产品名称：" + dt.Rows[i]["产品名称"] + "产品规格：" + dt.Rows[i]["规格"] + "计划数量：" + dt.Rows[i]["计划数量"] + "完工数量：" + dt.Rows[i]["完成数量"] + "开始日期:" + dt.Rows[i]["开始日期"] + "计划完成日期：" + dt.Rows[i]["计划完工日期"] + "报警类型：" + dt.Rows[i]["报警类型"] + "报警说明：" + dt.Rows[i]["报警说明"] + '"' + "}," + '"' + "keyword2" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + '"' + "}," + '"' + "remark" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "" + '"' + "}," + '"' + "first" + '"' + ":{" + '"' + "color" + '"' + ":" + '"' + "#173177" + '"' + "," + '"' + "value" + '"' + ":" + '"' + "【商基网络】" + '"' + "}}";


                                string EmailMsg = @"【商基网络】工单：" + dt.Rows[i]["工单"] + ",产品代号：" + dt.Rows[i]["产品代号"] + "产品名称：" + dt.Rows[i]["产品名称"] + "产品规格：" + dt.Rows[i]["规格"] + "计划数量：" + dt.Rows[i]["计划数量"] + "完工数量：" + dt.Rows[i]["完成数量"] + "开始日期:" + dt.Rows[i]["开始日期"] + "计划完成日期：" + dt.Rows[i]["计划完工日期"] + "报警类型：" + dt.Rows[i]["报警类型"] + "报警说明：" + dt.Rows[i]["报警说明"];

                                string SMSMsg = @"【商基网络】工单：" + dt.Rows[i]["工单"] + ",产品代号：" + dt.Rows[i]["产品代号"] + "产品名称：" + dt.Rows[i]["产品名称"] + "产品规格：" + dt.Rows[i]["规格"] + "计划数量：" + dt.Rows[i]["计划数量"] + "完工数量：" + dt.Rows[i]["完成数量"] + "开始日期:" + dt.Rows[i]["开始日期"] + "计划完成日期：" + dt.Rows[i]["计划完工日期"] + "报警类型：" + dt.Rows[i]["报警类型"] + "报警说明：" + dt.Rows[i]["报警说明"];

                                Alarm.SendAlarm("生产进度", OBJ, WinXinMsg, EmailMsg, SMSMsg, (int.Parse(sorting) + 1).ToString());
                                #endregion
                            }

                        }
                        
                    }

                    IsSuccess = true;
                    RetData = time.ToString();
                }
                else
                {
                    IsSuccess = true;
                    RetData = time1.ToString();
                }
            }
            catch(Exception ex)
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
