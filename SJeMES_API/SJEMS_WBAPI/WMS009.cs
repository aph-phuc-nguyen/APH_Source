using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class WMS009
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
 WMS009(出库单)接口帮助 @END
 方法：Help(帮助),Audit(审核) @END
 @END
====================Audit方法调用=================== @END
 Data参数如下
 <Data> @END
 @END
 
 <moveout_doc></moveout_doc>入库单 @END
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

                DllName = "SJEMS_WBAPI";
                ClassName = "SJESM_WBAPI.WMS009";
                Method = "Audit";
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string moveout_doc = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<moveout_doc>", "</moveout_doc>");//出库单

                #endregion

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 逻辑
                // 1.	请求User.DoSureCheck(User,WMS009)方法检查用户是否有权限进行审核，如果没有权限，报错该用户没有权限进行审核
                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS009"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {

                    bool IsOk = false;
                    string ErrMsg = string.Empty;

                    //2.	查询WMS009M是否存在该单号以及状态不为已审核，不存在报错：单号不存在，已审核报错：单号状态为已审核，不能重复审核
                    string sql = @"
SELECT status FROM WMS009M WHERE moveout_doc='"+moveout_doc+@"'
";


                    string status = DB.GetString(sql);

                    if (!string.IsNullOrEmpty(status) && status == "1")//存在单号且未审核
                    {

                        //3.	查询WMS009A1该单是否存在实际出库数据，不存在报错：单号不存在实际出库数据。存在的话，根据出库数据（按物料进行汇总），查询WMS012M(库存资料)逐一物料判断库存数据是否大于等于出库数据，如果不满足，报错误：物料【物料编号，物料名称，物料规格】库存不足。根据出库数据（按物料、库位进行汇总），查询WMS012A1(库存明细)逐一物料、库位判断库存数据是否大于等于出库数据，如果不满足，报错误：物料【物料编号，物料名称，物料规格，库位】库存不足。
                        sql = @"
SELECT qty,location,material_no,material_name,material_specifications,suppliers_lot,UDF01,UDF06,UDF07 
FROM WMS009A1 
WHERE moveout_doc='" + moveout_doc + @"'
";
                        double qty = 0;
                        string location = string.Empty;
                        string material_no = string.Empty;
                        string material_name = string.Empty;
                        string material_specifications = string.Empty;
                        string suppliers_lot = string.Empty;
                        string UDF01 = string.Empty;
                        string UDF06 = string.Empty;
                        string UDF07 = string.Empty;

                        System.Data.DataTable dtWMS009A1 = DB.GetDataTable(sql);


                        if (dtWMS009A1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtWMS009A1.Rows)
                            {
                                

                                qty = Convert.ToDouble(dr[0].ToString());
                                location = dr[1].ToString();
                                material_no = dr[2].ToString();
                                material_name = dr[3].ToString();
                                material_specifications = dr[4].ToString();
                                suppliers_lot = dr[5].ToString();
                                UDF01 = dr["UDF01"].ToString();
                                UDF06 = dr["UDF06"].ToString();
                                UDF07 = dr["UDF07"].ToString();


                                #region 更新WMS012M
                                sql = @"
UPDATE WMS012M set qty=qty-" + qty + @",qty_availble=qty_availble-" + qty + @",UDF06=UDF06-" + UDF06 + @",UDF07-" + UDF07 + @"
WHERE material_no='" + material_no + @"'
AND UDF01='" + UDF01 + @"'
";
                                DB.ExecuteNonQueryOffline(sql);
                                #endregion


                                #region 更新WMS012A1
                                sql = @"
UPDATE WMS012M set qty=qty-" + qty + @",qty_availble=qty_availble-" + qty + @",UDF06=UDF06-" + UDF06 + @",UDF07-" + UDF07 + @"
WHERE material_no='" + material_no + @"'
AND UDF01='" + UDF01 + @"'
AND location='" + location + @"'
";
                                #endregion
                            }

                            //4	根据出库单号，CODE002A3(批号单据记录)、根据条码记录分别删除WMS012A2(库存批号条码记录)
                            #region

                            //批号
                            sql = @"
SELECT lot_barcode,warehouse,location_target,qty,UDF01,UDF06,UDF07 
FROM CODE002A3 
WHERE documents='" + moveout_doc + @"' and documents_type='12' AND UDF01='" + UDF01 + @"' and location_target='" + location + @"'";


                            foreach (DataRow dr in DB.GetDataTable(sql).Rows)
                            {
                                string lot_barcode = dr["lot_barcode"].ToString();
                                string warehouse = dr["warehouse"].ToString();
                                UDF01 = dr["UDF01"].ToString();
                                location = dr["location_target"].ToString();
                                qty = Convert.ToDouble( dr["qty"].ToString());
                                UDF06 = dr["UDF06"].ToString();
                                UDF07 = dr["UDF07"].ToString();

                               
                                    sql = @"
DELETE FROM WMS012A2 WHERE
lot_barcode='"+lot_barcode+@"'
AND location='"+location+@"'
AND UDF01='"+UDF01+@"'
AND qty-"+qty+@"<=0
AND UDF06-"+UDF06+@"<=0
AND UDF07-"+UDF07+@"<=0
";
                                DB.ExecuteNonQueryOffline(sql);

                                sql = @"
UPDATE CODE002M SET qty=0,status='9'
WHERE lot_barcode='" + lot_barcode + @"'
AND location='" + location + @"'
AND UDF01='" + UDF01 + @"'
AND qty-" + qty + @"<=0
AND UDF06-" + UDF06 + @"<=0
AND UDF07-" + UDF07 + @"<=0
";
                                DB.ExecuteNonQueryOffline(sql);

                            }




                            #endregion

                            //5.	更新单号对应的状态和审核人，审核时间，修改人，修改时间
                            #region

                            string date = DateTime.Now.ToString("yyyy-MM-dd");
                            string time = DateTime.Now.ToString("HH:mm:ss");

                            sql = @"
UPDATE WMS009M SET auditby='"+UserCode+@"',auditdatetime='"+date+" "+time+@"',modifyby='"+UserCode+@"',modifydate='"+date+@"',modifytime='"+time+@"'
WHERE moveout_doc='"+moveout_doc+@"'
";
                            DB.ExecuteNonQueryOffline(sql);
                            
                            #endregion



                            IsOk = true;
                        }//判断是否有实际数据
                        else
                        {
                            ErrMsg = "单号不存在实际出库数据";
                        }
                    }//是否存在单号

                    else
                    {
                        if (!string.IsNullOrEmpty(status))
                        {
                            ErrMsg = "单号不存在";
                        }
                        if (status == "2")
                        {
                            ErrMsg = "单号状态为已审核，不能重复审核";
                        }

                    }

                    RetData += @"<" +moveout_doc + @">
							<IsOk>" + IsOk + @"</IsOk> 
							<ErrMsg>" + ErrMsg + @"</ErrMsg> 
						</" + moveout_doc + @">";

                   


                }//1.审核
                #endregion
                   
                IsSuccess = true;
                GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);
            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
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

            p.Add("@moveout_doc", p1);

            sql = @"
SELECT status,auditby,auditdatetime,modifyby,modifydate FROM WMS009M where moveout_doc=@moveout_doc 
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
 update WMS009M set status=@status,auditby=@auditby,auditdatetime=@auditdatetime,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where moveout_doc=@moveout_doc
";

                int i = DB.ExecuteNonQueryOffline(sql, p);

            }

        }


        public static string QuickDelivery(object OBJ)
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

                #region 接口参数
                //string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);
                #endregion

                string where = string.Empty;


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                //2.根据扫描的条码数据进行汇总（按物料库位）入库数量，添加汇总数据到wms004a1(入库明细表)
                Dictionary<string, double> Nums1 = new Dictionary<string, double>();

                for (int i = 0; i < tmdt.Rows.Count; i++)
                {
                    if (Nums1.ContainsKey(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()))
                    {
                        Nums1[tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()] += Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim());
                    }
                    else
                    {
                        Nums1.Add(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim(), Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim()));
                    }
                }


                #region 逻辑
                string sql = string.Empty;
                DataTable dt4;
                //1.产生出库单
                sql = @"
SELECT MAX(moveout_doc) FROM WMS009M
";
                string moveout_doc = DB.GetString(sql);

                if (moveout_doc.IndexOf(DateTime.Now.ToString("yyyyMMdd")) > -1)
                {
                    moveout_doc = DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(moveout_doc.Replace(DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");
                }
                else
                {
                    moveout_doc = DateTime.Now.ToString("yyyyMMdd") + "0001";
                }

                //2.新增出库明细
                sql = @"insert into WMS009M(moveout_doc,moveout_type,status,auditby,auditdatetime,createby,createtime) values('" + moveout_doc + "','4','1','" + UserCode + "','" + DateTime.Now.ToString() + "','" + UserCode + "','" + DateTime.Now.ToString() + "')";
                DB.ExecuteNonQueryOffline(sql);

                foreach (KeyValuePair<string, double> kvp1 in Nums1)
                {

                    string material_no = kvp1.Key.Split('*')[0].Trim();
                    string location = kvp1.Key.Split('*')[1].Trim();
                    double qty = Convert.ToDouble(kvp1.Value.ToString());


                    string serial_number = string.Empty;

                    dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM WMS009A1 where moveout_doc='" + moveout_doc + "'");
                    if (dt4.Rows[0][0].ToString() == "")
                    {
                        serial_number = "001";
                    }
                    else
                    {
                        serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                    }

                    sql = @"select top 1 material_no,material_name,material_specifications from BASE007M where material_no = '" + material_no + "'";
                    DataTable Base007m = DB.GetDataTable(sql);
                    if (Base007m.Rows.Count > 0)
                    {

                        sql = @" insert into WMS009A1(moveout_doc,serial_number,material_no,material_name,material_specifications,qty,location,createby,createdate,createtime) 
values('" + moveout_doc + "','" + serial_number + "','" + material_no + "','" + Base007m.Rows[0][1].ToString() + "','" + Base007m.Rows[0][2].ToString() + "','" +Math.Round(qty,2) + "','" + location + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                        DB.ExecuteNonQueryOffline(sql);

                    }
                }

                //System.Data.DataTable dt4;
                for (int i = 0; i < tmdt.Rows.Count; i++)
                {

                    string serial_number = string.Empty;

                    switch (tmdt.Rows[i]["BarCodeType"].ToString().Trim())
                    {

                        case "1":
                            dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE001A3 where documents='" + moveout_doc + "'");
                            if (dt4.Rows[0][0].ToString() == "")
                            {
                                serial_number = "001";
                            }
                            else
                            {
                                serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                            }
                            string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            DataTable dt3 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE001M where products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE001A1 (products_barcode,operation,warehouse,location,packing_barcode,lot_barcode,org,createby,createdate,createtime) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','9','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + dt3.Rows[0]["packing_barcode"] + "','" + dt3.Rows[0]["lot_barcode"] + "','" + dt3.Rows[0]["org"] + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            sql += @"INSERT INTO CODE001A3 (products_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','12','" + moveout_doc + "','" + serial_number + "','9','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt3.Rows[0]["org"] + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        case "2":
                            dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE002A3 where documents='" + moveout_doc + "'");
                            if (dt4.Rows[0][0].ToString() == "")
                            {
                                serial_number = "001";
                            }
                            else
                            {
                                serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                            }
                            string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org,createby,createdate,createtime) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','9','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "') ";
                            sql += "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','12','" + moveout_doc + "','" + serial_number + "','9','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        case "3":
                            dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE003A3 where documents='" + moveout_doc + "'");
                            if (dt4.Rows[0][0].ToString() == "")
                            {
                                serial_number = "001";
                            }
                            else
                            {
                                serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                            }
                            string warehouse2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE003M where packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE003A1 (packing_barcode,operation,warehouse,location,qty,lot_barcode,org,createby,createdate,createtime) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','9','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["lot_barcode"] + "','" + dt32.Rows[0]["org"] + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "') ";
                            sql += @"INSERT INTO CODE003A3 (packing_barcode	,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','12','" + moveout_doc + "','" + serial_number + "','9','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["org"] + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        default: break;
                    }

                }


                string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS008TOWMS009'");
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
                <moveout_doc>" + moveout_doc + @"</moveout_doc>
                </Data>
                
            </WebService>
";
                    ret = Audit(s);
                    return ret;
                }
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
                    < RetData>" + RetData + "</RetData>" + @"
                </Return>
            </WebService>
            ";

            return ret;
        }

    }
}
