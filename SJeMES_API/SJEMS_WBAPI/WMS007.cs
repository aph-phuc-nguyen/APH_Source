using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class WMS007
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
 WMS007(仓内移库单)接口帮助 @END
 方法：Help(帮助),Audit(审核) @END
 @END
====================Audit方法调用=================== @END
 Data参数如下
 <Data> @END
 @END
 
 <moveout_doc></moveout_doc>仓内移库单 @END
 @END
 </Data> @END
 @END
 返回数据如下 @END
 <Return> @END
 <IsSuccess>true/false</IsSuccess> @END
 <RetData> @END
 如果成功：<IsOk>True/False</IsOk> @END
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
        /// Audit(审核)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string Audit(object OBJ)
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

                #region 参数
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string move_doc = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<move_doc>", "</move_doc>");
                #endregion

                #region 逻辑
                // 1.	请求User.DoSureCheck(User,WMS007)方法检查用户是否有权限进行审核，如果没有权限，报错该用户没有权限进行审核
                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS007"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {
                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);


                    bool IsOk = false;
                    string ErrMsg = string.Empty;

                    //2.	查询WMS007M是否存在该单号以及状态不为已审核，不存在报错：单号不存在，已审核报错：单号状态为已审核，不能重复审核
                    string sql = @"
SELECT status FROM WMS007M WHERE move_doc='" + move_doc + @"'
";



                    string status = DB.GetString(sql);
                    if (!string.IsNullOrEmpty(status) && status == "1")//存在单号且未审核
                    {

                        //3.	查询WMS007A1该单是否存在实际入库数据，不存在报错：单号不存在实际入库数据。存在的话，根据入库数据（按物料进行汇总），调整WMS012M(库存资料）如果存在对应物料的库存，加上该单的入库数据，不存在新建一条库存数据，根据入库数据（按物料、库位进行汇总），调整WMS012A1(库存明细）如果存在对应物料、库位的库存，加上该单的入库数据，不存在新建一条库存数据。调用WMS012.DataEdit(User,物料代号,库位,数量)方法更新
                        sql = @"
SELECT qty,location,material_no,material_name,material_specifications,suppliers_lot,warehouse,warehouse_target,location_target,qty_out,UDF01,UDF06,UDF07  FROM WMS007A1 WHERE move_doc='" + move_doc + @"'
";


                        double qty = 0;
                        double qty_out = 0;
                        string location = string.Empty;
                        string material_no = string.Empty;
                        string material_name = string.Empty;
                        string material_specifications = string.Empty;
                        string suppliers_lot = string.Empty;
                        string warehouse = string.Empty;
                        string warehouse_target = string.Empty;
                        string location_target = string.Empty;
                        string UDF01 = string.Empty;
                        string UDF06 = string.Empty;
                        string UDF07 = string.Empty;
                        System.Data.DataTable dtWMS007A1 = DB.GetDataTable(sql);

                        if (dtWMS007A1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtWMS007A1.Rows)
                            {
                                qty = Convert.ToDouble(dr[0].ToString());
                                qty_out = Convert.ToDouble(dr[9].ToString());
                                location = dr[1].ToString();
                                material_no = dr[2].ToString();
                                material_name = dr[3].ToString();
                                material_specifications = dr[4].ToString();
                                suppliers_lot = dr[5].ToString();
                                warehouse = dr[6].ToString();
                                warehouse_target = dr[7].ToString();
                                location_target = dr[8].ToString();
                                UDF01 = dr["UDF01"].ToString();
                                UDF06 = dr["UDF06"].ToString();
                                UDF07 = dr["UDF07"].ToString();


                                sql = @"
UPDATE WMS012A1 set qty=qty-" + qty + @",qty_availble=qty_availble-" + qty + @",UDF06=UDF06-" + UDF06 + @",UDF07=UDF07-" + UDF07 + @"
WHERE material_no='" + material_no + @"' and location='" + location + @"' and UDF01='" + UDF01 + @"'
";
                                DB.ExecuteNonQueryOffline(sql);

                                sql = @"
UPDATE WMS012A1 set qty=qty+" + qty + @",qty_availble=qty_availble+" + qty + @",UDF06=UDF06+" + UDF06 + @",UDF07=UDF07+" + UDF07 + @"
WHERE material_no='" + material_no + @"' and location='" + location_target + @"' and UDF01='" + UDF01 + @"'
";
                                if (DB.ExecuteNonQueryOffline(sql) == 0)
                                {
                                    sql = @"
INSERT INTO WMS012A1
(material_no,location,warehouse,qty,qty_availble,createby,createdate,createtime,qty_occupied,UDF01,UDF06,UDF07)
VALUES
('" + material_no + @"','" + location_target + @"','" + warehouse_target + @"'," + qty + "," + qty + ",'" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','0','" + UDF01 + @"','" + UDF06 + "','" + UDF07 + @"')
";
                                    DB.ExecuteNonQueryOffline(sql);
                                }

                                if (!string.IsNullOrEmpty(ErrMsg))
                                {
                                    ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + false.ToString() + @"</IsSuccess>
                    <RetData>" + ErrMsg + @"</RetData>
                </Return>
            </WebService>
            ";
                                    return ret;

                                }
                            }

                            //5.	更新单号对应的状态和审核人，审核时间，修改人，修改时间
                            #region
                            string auditby = UserCode;
                            string auditdatetime = DateTime.Today.ToString("yyyy-MM-dd");
                            string modifyby = UserCode;
                            string modifydate = DateTime.Today.ToString("yyyy-MM-dd");
                            status = "2";//已经审核
                            DataEditStatus(move_doc, status, auditby, auditdatetime, modifyby, modifydate, DB);//修改07M表数据


                            #region 批号
                            sql = @"
SELECT lot_barcode,warehouse,location,warehouse_target,location_target,qty,UDF01,UDF06,UDF07 FROM CODE002A3 WHERE documents_type='10' and documents=@documents and UDF01='" + UDF01 + @"'
";


                            IDataReader Row = DB.GetDataTableReader(sql);

                            string lot_barcode = string.Empty;
                            while (Row.Read())
                            {
                                UDF01 = Row["UDF01"].ToString();
                                UDF06 = Row["UDF06"].ToString();
                                UDF07 = Row["UDF07"].ToString();
                                lot_barcode = Row[0].ToString();
                                warehouse = Row[1].ToString();
                                location = Row[2].ToString();
                                warehouse_target = Row[3].ToString();
                                location_target = Row[4].ToString();
                                qty = Convert.ToDouble(Row[5].ToString());

                                material_no = DB.GetString("SELECT material_no FROM CODE002M(NOLOCK) WHERE lot_barcode='" + lot_barcode + @"'");


                                sql = @"
UPDATE WMS012A2 SET qty=qty-'" + qty + @"',qty_availble=qty_availble-'" + qty + @"',UDF06=UDF06-'" + UDF06+ @"',UDF07=UDF07-'" + UDF07+@"'
WHERE lot_barcode='" + lot_barcode + @"' AND location='" + location + @"' and warehouse='" + warehouse + @"' and UDF01='" + UDF01 + @"'
";
                                DB.ExecuteNonQueryOffline(sql);

                                sql = @"
UPDATE WMS012A2 SET qty=qty+'" + qty + @"',qty_availble=qty_availble+'" + qty + @"',UDF06=UDF06+'" + UDF06 + @"',UDF07=UDF07+'" + UDF07 + @"'
WHERE lot_barcode='" + lot_barcode + @"' AND location='" + location_target + @"' and warehouse='" + warehouse_target + @"' and UDF01='" + UDF01 + @"'
";
                                if (DB.ExecuteNonQueryOffline(sql) < 1)
                                {
                                    sql = @"
INSERT INTO WMS012A2
(lot_barcode,material_no,location,warehouse,qty,qty_availble,createby,createdate,createtime,qty_occupied,UDF01,UDF06,UDF07)
VALUES
('"+lot_barcode+@"','" + material_no + @"','" + location_target + @"','" + warehouse_target + @"'," + qty + "," + qty + ",'" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','0','" + UDF01 + @"','" + UDF06 + "','" + UDF07 + @"')
";
                                    DB.ExecuteNonQueryOffline(sql);
                                }

                                sql = @"
UPDATE CODE002M SET location ='" + location_target + @"'
where lot_barcode='" + lot_barcode + @"'
";
                                DB.ExecuteNonQueryOffline(sql);

                            }

                            #endregion






                            #endregion


                            IsOk = true;
                        }//判断是否有实际数据
                        else
                        {
                            ErrMsg = "单号不存在实际移库数据";
                        }
                    }//是否存在单号
                    else
                    {
                        if (string.IsNullOrEmpty(status))
                        {
                            ErrMsg = "单号不存在";
                        }
                        if (status == "2")
                        {
                            ErrMsg = "单号状态为已审核，不能重复审核";
                        }

                    }

                    RetData += @"<" + move_doc + @">
							<IsOk>" + IsOk + @"</IsOk> 
							<ErrMsg>" + ErrMsg + @"</ErrMsg> 
						</" + move_doc + @">";



                }//1.审核
                #endregion

                IsSuccess = true;
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
        /// DataEditStatus(修改审核人信息)
        /// </summary>
        /// <param name="p1">单号</param>
        /// <param name="status">状态</param>
        /// <param name="auditby">审核人</param>
        /// <param name="auditdatetime">审核时间</param>
        /// <param name="modifyby">修改人</param>
        /// <param name="modifydate">修改时间</param>
        private static void DataEditStatus(string p1, string status, string auditby, string auditdatetime, string modifyby, string modifydate, GDSJ_Framework.DBHelper.DataBase DB)
        {
            string sql = string.Empty;
            Dictionary<string, object> p = new Dictionary<string, object>();
            //GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
            string modifytime = DateTime.Now.ToString("HH:mm:ss");

            p.Add("@move_doc", p1);

            sql = @"
SELECT status,auditby,auditdatetime,modifyby,modifydate FROM WMS007M where move_doc=@move_doc
";
            System.Data.IDataReader dr = DB.GetDataTableReader(sql, p);
            if (dr.Read())
            {
                p.Add("@status", status);
                p.Add("@auditby", auditby);
                p.Add("@auditdatetime", auditdatetime);
                p.Add("@modifyby", modifyby);
                p.Add("@modifydate", modifydate);
                p.Add("@modifytime", modifytime);
                //p.Add("@storage_doc", p1);

                sql = @"
 update WMS007M set status=@status,auditby=@auditby,auditdatetime=@auditdatetime,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where move_doc=@move_doc
";

                int i = DB.ExecuteNonQueryOffline(sql, p);
            }

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
            string move_doc = string.Empty;
            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
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
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");//账号

                string BarCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");//条码信息
                System.Data.DataTable dtBarCode = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(BarCode);
                #endregion

                


                #region 逻辑
                string sql = string.Empty;
                
                //逻辑：根据传入的条码信息，创建移库单
                //1.根据传入的条码信息，创建移库单WMS007M 和WMS007A1

               
                    move_doc = DB.GetString("SELECT ISNULL(MAX(move_doc),'0000') FROM WMS007M(NOLOCK) where move_doc LIKE '"+ DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "%'");

                    move_doc = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + (Convert.ToInt32(move_doc.Replace(DateTime.Now.ToString("yyyy-MM-dd").Replace("-", ""), "")) + 1).ToString("0000");


              
                //创建移库单头WMS007M
                sql = @"INSERT INTO WMS007M (move_doc,status) VALUES ('" + move_doc + "','1')";
                DB.ExecuteNonQueryOffline(sql);





                //根据扫描到的条码获取插入wms007a1的信息
                //如果是箱号条码 code003a3 插入一条移库记录，其中document字段保存的wms007m 的单据
                //如果是批号条码 code002a3 插入一条移库记录，其中document字段保存的wms007m 的单据
                //如果是单品条码 code001a3 插入一条移库记录，其中document字段保存的wms007m 的单据

                foreach (DataRow dr in dtBarCode.Rows)
                {
                    string serial_number = string.Empty;


                    serial_number = DB.GetString("SELECT isnull(max(CONVERT(int,serial_number)),'000') FROM WMS007A1(NOLOCK) where move_doc ='" + move_doc + "'");

                    serial_number = int.Parse(serial_number + 1).ToString("000");


                    sql = @"SELECT lot_barcode AS '批号条码',	
                                    production_order AS '生产工单',
                                    production_records_card AS '途程单',	
                                    material_no	AS '物料代号',
                                    material_name AS '物料名称',	
                                    material_var AS '版本号',
                                    material_specifications AS '物料规格',	
                                    suppliers_lot AS '供应商批号'	
                                     FROM CODE002M(NOLOCK)
                                   WHERE  location='" + dr["Location"].ToString() + @"' AND UDF01='" + dr["UDF01"].ToString() + @"' AND lot_barcode='" + dr["code"].ToString() + @"'";

                    System.Data.DataTable dt2 = DB.GetDataTable(sql);
                    string warehouse_target2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + dr["TargetLocation"].ToString().Trim() + "'").Rows[0][0].ToString();//目标仓库
                    string warehouse2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + dr["Location"].ToString().Trim() + "'").Rows[0][0].ToString();//原仓库

                    //创建移库单身WMS007A1
                    sql = @"
INSERT INTO WMS007A1
(move_doc,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,warehouse,location,warehouse_target,location_target,qty,qty_out,UDF01,UDF06,UDF07)
VAlUES 
('" + move_doc + "','" + serial_number + "','" + dt2.Rows[0]["物料代号"].ToString() + "','" + dt2.Rows[0]["物料名称"].ToString() + "','" + dt2.Rows[0]["版本号"].ToString() + "','" + dt2.Rows[0]["物料规格"].ToString() + "','" + dt2.Rows[0]["供应商批号"].ToString() + "','" + dt2.Rows[0]["批号条码"].ToString() + "','" + warehouse2 + "','" + dr["Location"].ToString() + "','" + warehouse_target2 + "','" + dr["TargetLocation"].ToString() + "','" + dr["Qty"].ToString() + "','" + dr["Qty"].ToString() + "','" + dr["UDF01"].ToString().Trim() + @"','" + dr["UDF06"].ToString() + "','" + dr["UDF07"].ToString() + "')";
                    // DB.ExecuteNonQueryOffline(sql);
                    //
                    //移库记录
                    sql += @"
INSERT INTO CODE002A3 
(lot_barcode,documents_type,documents,serial_number,operation,warehouse,location,warehouse_target,location_target,qty,UDF01,UDF06,UDF07) 
VALUES 
('" + dt2.Rows[0]["批号条码"].ToString().Trim() + "','10','" + move_doc + "','" + serial_number + "','11','" + warehouse2 + "','" + dr["Location"].ToString().Trim() + "','" + warehouse_target2 + "','" + dr["TargetLocation"].ToString() + "','" + dr["Qty"].ToString() + "','" + dr["UDF01"].ToString().Trim() + "','"+ dr["UDF06"].ToString() + "','"+ dr["UDF07"].ToString() +  @"')";

                    DB.ExecuteNonQueryOffline(sql);




                }

                string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS007DoSure'");
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
                <move_doc>" + move_doc + @"</move_doc>
                </Data>
                
            </WebService>
";

                    ret = Audit(s);

                    return ret;
                }
                //2.获取系统参数SYS002M中的WMS007DoSure参数，如果是Auto的话，调用DoSure方法自动审核单据
                #endregion
                IsSuccess = true;
                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }

           
            if (IsSuccess == true)
            {
                ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return> 
                    <IsSuccess>" + IsSuccess + "</IsSuccess><单号>" + move_doc + "</单号>" + msg + @"
                </Return>
            </WebService>
            ";
            }
            else
            {
                ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return> 
                    <IsSuccess>" + IsSuccess + "</IsSuccess>" + RetData + msg + @"
                </Return>
            </WebService>
            ";
            }

            return ret;
        }
    }
}

