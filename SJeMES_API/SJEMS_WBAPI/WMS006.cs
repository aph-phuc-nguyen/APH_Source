using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class WMS006
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
 WMS006(调拨单)接口帮助 @END
 方法：Help(帮助),Audit(审核) @END
 @END
====================Audit方法调用=================== @END
 Data参数如下
 <Data> @END
 @END
 
 <moveout_doc></moveout_doc>调拨单 @END
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
        /// DoSure(审核)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string Audit(object OBJ, string allocation_doc,int flat)
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
            
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
            
                DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);
                #endregion

                #region 逻辑
                // 1.	请求User.DoSureCheck(User,WMS004)方法检查用户是否有权限进行审核，如果没有权限，报错该用户没有权限进行审核
                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS004"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {
                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                    string[] t = new string[1];
                    t[0] = ",";
                    string[] a = allocation_doc.Split(t, StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    for (int i = 0; i < a.Length; i++)
                    {
                        bool IsOk = false;
                        string ErrMsg = string.Empty;

                        //2.	查询WMS006M是否存在该单号以及状态不为已审核，不存在报错：单号不存在，已审核报错：单号状态为已审核，不能重复审核
                        string sql = @"
SELECT status FROM WMS006M WHERE allocation_doc=@allocation_doc 
";

                        p.Add("@allocation_doc", a[i].ToString());

                        string status = DB.GetString(sql, p);
                        if (!string.IsNullOrEmpty(status) && status == "1")//存在单号且未审核
                        {
                            p.Clear();
                            //3.	查询WMS006A1该单是否存在实际入库数据，不存在报错：单号不存在实际入库数据。存在的话，根据入库数据（按物料进行汇总），调整WMS012M(库存资料）如果存在对应物料的库存，加上该单的入库数据，不存在新建一条库存数据，根据入库数据（按物料、库位进行汇总），调整WMS012A1(库存明细）如果存在对应物料、库位的库存，加上该单的入库数据，不存在新建一条库存数据。调用WMS012.DataEdit(User,物料代号,库位,数量)方法更新
                            sql = @"
SELECT qty,location,material_no,material_name,material_specifications,suppliers_lot,warehouse,warehouse_target,location_target,qty_out  FROM WMS006A1 WHERE allocation_doc=@allocation_doc
";
                            p.Add("@allocation_doc", a[i].ToString());

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
                            DataTable dtWMS006A1 = DB.GetDataTable(sql, p);
                            p.Clear();
                            if (dtWMS006A1.Rows.Count > 0)
                            {
                                int j = 0;
                                foreach (DataRow dr in dtWMS006A1.Rows)
                                {
                                    qty_out = Convert.ToDouble(dr[9].ToString());
                                    qty = Convert.ToDouble(dr[0].ToString());
                                    location = dr[1].ToString();
                                    material_no = dr[2].ToString();
                                    material_name = dr[3].ToString();
                                    material_specifications = dr[4].ToString();
                                    suppliers_lot = dr[5].ToString();
                                    warehouse = dr[6].ToString();
                                    warehouse_target = dr[7].ToString();
                                    location_target = dr[8].ToString();

                                    IDataReader Row;

                                    sql = @"
SELECT top 1 qty,material_no FROM WMS012A1 WHERE material_no=@material_no  and location=@location
";
                                    p.Clear();
                                    p.Add("@material_no", material_no);
                                    p.Add("@location", location);
                                    Row = DB.GetDataTableReader(sql, p);
                                    if (Row.Read())//012A1表物料+库位 是唯一的
                                    {
                                        ErrMsg += WMS012.DataEdit_diaobo(UserCode, material_no, material_name, material_specifications, suppliers_lot, location, location_target, warehouse, warehouse_target, qty, qty_out, DB);//修改12A1表的数据
                                        if (tmdt.Rows.Count != 0)
                                        {
                                            if (tmdt.Rows[j][2].ToString() != "0")
                                            {
                                                ErrMsg += WMS012.DataEdit_barcode(UserCode, tmdt.Rows[j][1].ToString(), material_no, location, location_target, warehouse, warehouse_target, qty, qty_out, DB, tmdt.Rows[j][2].ToString());//修改12A2,3,4表的数据
                                            }
                                        }
                                     
                                        j++;
                                    }

                                    #region   更新单号对应的状态和审核人，审核时间，修改人，修改时间
                                    if (flat == 1)//计划调拨
                                    {
                                        sql = @"select front_document from WMS006M where allocation_doc ='" + a[i].ToString() + "'";
                                        DataTable dt_plan = DB.GetDataTable(sql);

                                        sql = @"select SUM(qty) from WMS006A1 where allocation_doc in (select allocation_doc from WMS006M where  front_document = '" + dt_plan.Rows[0][0].ToString() + "') and material_no='" + material_no + "'";
                                        DataTable dt_qty_int = DB.GetDataTable(sql);

                                        sql = @"select qty_plan from WMS005A1 where allocation_plan='" + dt_plan.Rows[0][0].ToString() + "' and material_no='" + material_no + "'";
                                        DataTable dt_qty_plan = DB.GetDataTable(sql);

                                        sql = @"select SUM(qty_out) from WMS006A1 where allocation_doc in (select allocation_doc from WMS006M where  front_document = '" + dt_plan.Rows[0][0].ToString() + "') and material_no='" + material_no + "'";
                                        DataTable dt_qty_out = DB.GetDataTable(sql);
                                        if (Convert.ToDouble(dt_qty_plan.Rows[0][0].ToString()) >= Convert.ToDouble(dt_qty_out.Rows[0][0].ToString()) || Convert.ToDouble(dt_qty_out.Rows[0][0].ToString()) >= Convert.ToDouble(dt_qty_int.Rows[0][0].ToString()))
                                        {
                                            string auditby = UserCode;
                                            string auditdatetime = DateTime.Today.ToString("yyyy-MM-dd");
                                            string modifyby = UserCode;
                                            string modifydate = DateTime.Today.ToString("yyyy-MM-dd");
                                            status = "2";//已经审核
                                            DataEditStatus(a[i].ToString(), status, auditby, auditdatetime, modifyby, modifydate, DB);//修改06M表数据
                                        }
                                    }
                                    else//快速调拨
                                    {
                                        sql = @"select SUM(qty) from WMS006A1 where allocation_doc ='" + allocation_doc + "' and material_no='" + material_no + "'";
                                        double dt_qty_int = DB.GetDouble(sql);

                                        sql = @"select SUM(qty_out) from WMS006A1 where allocation_doc ='" + allocation_doc + "' and material_no='" + material_no + "'";
                                        double dt_qty_out = DB.GetDouble(sql);

                                        if (dt_qty_int == dt_qty_out)
                                        {
                                            string auditby = UserCode;
                                            string auditdatetime = DateTime.Today.ToString("yyyy-MM-dd");
                                            string modifyby = UserCode;
                                            string modifydate = DateTime.Today.ToString("yyyy-MM-dd");
                                            status = "2";//已经审核
                                            DataEditStatus(a[i].ToString(), status, auditby, auditdatetime, modifyby, modifydate, DB);//修改06M表数据
                                        }
                                    }

                               
                                    #endregion

                                }//循环遍历实际数据
                                IsOk = true;
                            }//判断是否有实际数据
                            else
                            {
                                ErrMsg = "单号不存在实际入库数据";
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

                        RetData += @"<" + a[i].ToString() + @">
							<IsOk>" + IsOk + @"</IsOk> 
							<ErrMsg>" + ErrMsg + @"</ErrMsg> 
						</" + a[i].ToString() + @">";

                        p.Clear();
                    }
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
            string modifytime = DateTime.Now.ToString("hh:mm:ss");

            p.Add("@allocation_doc", p1);

            sql = @"
SELECT status,auditby,auditdatetime,modifyby,modifydate FROM WMS006M where allocation_doc=@allocation_doc 
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
 update WMS006M set status=@status,auditby=@auditby,auditdatetime=@auditdatetime,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where allocation_doc=@allocation_doc
";

                int i = DB.ExecuteNonQueryOffline(sql, p);
            }

        }


        public static string QuickDoOut(object OBJ)
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

                #region 接口参数
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                System.Data.DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);
                #endregion
                Dictionary<string, double> Nums = new Dictionary<string, double>();
                for (int i = 0; i < tmdt.Rows.Count; i++)
                {
                    if (Nums.Keys.ToString().Contains(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()))
                    {
                        Nums[tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCodeType"].ToString().Trim()] += Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim());
                    }
                    else
                    {
                        Nums.Add(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "*" + tmdt.Rows[i]["BarCodeType"].ToString().Trim(), Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim()));
                    }
                }

                string where = string.Empty;


                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);



                #region 逻辑
                string sql = string.Empty;
                DataTable dt4;
                string serial_number = "";

                //1.产生调拨单
                sql = @"
SELECT MAX(allocation_doc) FROM WMS006M
";
                string allocation_doc = DB.GetString(sql);

                if (allocation_doc.IndexOf(DateTime.Now.ToString("yyyyMMdd")) > -1)
                {
                    allocation_doc = DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(allocation_doc.Replace(DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");
                }
                else
                {
                    allocation_doc = DateTime.Now.ToString("yyyyMMdd") + "0001";
                }
                int j = 1;//序号
                int k = 0;
                foreach (KeyValuePair<string, double> kvp in Nums)
                {
                    sql = @"SELECT qty FROM WMS012A1 where material_no='" + kvp.Key.Split('*')[0] + "'  and location='" + kvp.Key.Split('*')[1] + "' ";
                    DataTable Wms012a1 = DB.GetDataTable(sql);//库存信息
                    if (Convert.ToDouble(Wms012a1.Rows[k][0]) - kvp.Value >= 0)
                    {
                        //2.新增调拨明细
                        sql = @"insert into WMS006M(allocation_doc,status,auditby,auditdatetime,createby,createtime) values('" + allocation_doc + "','1','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + UserCode + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                        DB.ExecuteNonQueryOffline(sql);


                        sql = @"select top 1 material_no,material_name,material_specifications from BASE007M where material_no = '" + kvp.Key.Split('*')[0].Trim() + "'";
                        DataTable Base007m = DB.GetDataTable(sql);//物料信息
                        sql = @"SELECT warehouse FROM BASE011M where location_no = '" + kvp.Key.Split('*')[1].Trim() + "'";
                        DataTable Base011m = DB.GetDataTable(sql);//库位信息
                        if (Base007m.Rows.Count > 0)
                        {
                            if (kvp.Key.Split('*')[3] == "2")
                            {
                                sql = @" insert into WMS006A1(allocation_doc,serial_number,material_no,material_name,material_specifications,lot_barcode,qty_out,warehouse,location,createby,createdate,createtime) values('" + allocation_doc + "','" + j.ToString("000") + "','" + kvp.Key.Split('*')[0].Trim() + "','" + Base007m.Rows[0][1].ToString() + "','" + Base007m.Rows[0][2].ToString() + "','" + kvp.Key.Split('*')[2] + "','" +Math.Round( kvp.Value,2) + "','" + Base011m.Rows[0][0].ToString() + "','" + kvp.Key.Split('*')[1].Trim() + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            }
                            else
                            {
                                sql = @" insert into WMS006A1(allocation_doc,serial_number,material_no,material_name,material_specifications,qty_out,warehouse,location,createby,createdate,createtime) values('" + allocation_doc + "','" + j.ToString("000") + "','" + kvp.Key.Split('*')[0].Trim() + "','" + Base007m.Rows[0][1].ToString() + "','" + Base007m.Rows[0][2].ToString() + "','" +Math.Round( kvp.Value,2) + "','" + Base011m.Rows[0][0].ToString() + "','" + kvp.Key.Split('*')[1].Trim() + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                            }
                            DB.ExecuteNonQueryOffline(sql);
                        }


                        RetData += WMS012.DataEdit_diaobo(UserCode, kvp.Key.Split('*')[0].Trim(), "", "", "", kvp.Key.Split('*')[1], "", Base011m.Rows[0][0].ToString(), "", -1, kvp.Value, DB);//修改12A1表的数据
                        if (kvp.Key.Split('*')[3] != "0")
                        {
                            RetData += WMS012.DataEdit_barcode(UserCode, kvp.Key.Split('*')[2], kvp.Key.Split('*')[0].Trim(), kvp.Key.Split('*')[1], "", Base011m.Rows[0][0].ToString(), "", -1, kvp.Value, DB, kvp.Key.Split('*')[3]);//修改12A2、3、4表数据
                        }
                        j++;
                    }
                    else
                    {
                        RetData += "物料：" + kvp.Key.Split('*')[0] + "库位：" + kvp.Key.Split('*')[1] + "库存不足，无法调拨。";
                    }
                }

                #endregion
                //4.把扫描条码，根据条码的类型，分别插入到 CODE001A1(单品条码操作记录)、CODE001A3(单品条码单据记录)、CODE002A1(批号条码操作记录)、CODE002A3(批号条码单据操作记录)、CODE003A1(包装条码操作记录)、CODE003A3(包装条码单据记录)中。
                //   0是品号 Base007m，1是单品code001m，2是批号code002m，3是包装code003m
                for (int i = 0; i < tmdt.Rows.Count; i++)
                {
                    dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE001A3 where documents='" + allocation_doc + "'");
                    if (dt4.Rows[0][0].ToString() == "")
                    {
                        serial_number = "001";
                    }
                    else
                    {
                        serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                    }

                    switch (tmdt.Rows[i]["BarCodeType"].ToString().Trim())
                    {

                        case "1":
                            string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            DataTable dt3 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE001M where products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE001A1 (products_barcode,operation,warehouse,location,packing_barcode,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','7','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + dt3.Rows[0]["packing_barcode"] + "','" + dt3.Rows[0]["lot_barcode"] + "','" + dt3.Rows[0]["org"] + "')";
                            sql += @"INSERT INTO CODE001A3 (products_barcode,documents_type,documents,serial_number,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + allocation_doc + "','" + serial_number + "','7','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt3.Rows[0]["org"] + "')";
                            sql += @"UPDATE CODE001M SET status='7' WHERE products_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            DB.ExecuteNonQueryOffline(sql);
                            break;

                        case "2":
                            string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','7','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "') ";
                            sql += "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse,location,qty,org) VALUES('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + allocation_doc + "','" + serial_number + "','7','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "')";
                            sql += @"UPDATE CODE002M SET status='7' WHERE lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            DB.ExecuteNonQueryOffline(sql);
                            break;

                        case "3":
                            string c = "SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'";
                            string warehouse2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE003M where packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE003A1 (packing_barcode,operation,warehouse,location,qty,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','7','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["lot_barcode"] + "','" + dt32.Rows[0]["org"] + "') ";
                            sql += @"INSERT INTO CODE003A3 (packing_barcode	,documents_type,documents,serial_number,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','8','" + allocation_doc + "','" + serial_number + "','7','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["org"] + "')";
                            sql += @"UPDATE CODE003M SET status='7' WHERE packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + @"'";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        default: break;
                    }

                }

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

        public static string QuickDoIn(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string msg1 = string.Empty;
            string msg2 = string.Empty;
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

                #region 接口参数
                //string allocation_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<allocation_plan>", "</allocation_plan>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string DataTable = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                #endregion

                string where = string.Empty;

                string sql = string.Empty;

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                Dictionary<string, object> p = new Dictionary<string, object>();

                string doc = "";
                DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable);

                #region 逻辑

                Dictionary<string, double> Nums = new Dictionary<string, double>();
                for (int i = 0; i < tmdt.Rows.Count; i++)
                {
                    if (Nums.ContainsKey(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()))
                    {
                        Nums[tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim()] += Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim());
                    }
                    else
                    {
                        Nums.Add(tmdt.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt.Rows[i]["Location"].ToString().Trim(), Convert.ToDouble(tmdt.Rows[i]["Qty"].ToString().Trim()));
                    }
                }
                foreach (KeyValuePair<string, double> kvp in Nums)
                {
                    int stop = 0;
                    double qty_x = 0;
                    int a = 1;
                    while (stop == 0)
                    {
                        qty_x = kvp.Value - qty_x;
                        if (qty_x <= 0)
                        {
                            stop = 1;
                        }
                        else
                        {
                            p.Add("@qty", qty_x);
                            p.Add("@material_no", kvp.Key.Split('*')[0].Trim());

                            sql = @"
 select top 1 allocation_doc from  WMS006A1 where material_no=@material_no 
and qty+@qty=qty_out
and qty != qty_out
order by allocation_doc
";
                            doc = DB.GetString(sql, p);//刚好拨出和拨入相同的调拨单

                            if (doc == null)
                            {
                                sql = @"
 select top 1 allocation_doc from  WMS006A1 where material_no=@material_no 
and qty+@qty>qty_out
and qty != qty_out
order by allocation_doc
";
                                doc = DB.GetString(sql, p);//满足拨入大于拨出的调拨单，会产生循环，更新两个以上调拨单数据
                                a = 2;
                                if (doc == null)
                                {
                                    sql = @"
 select top 1 allocation_doc from  WMS006A1 where material_no=@material_no 
and qty+@qty<qty_out
and qty != qty_out
order by allocation_doc
";
                                    doc = DB.GetString(sql, p);
                                    a = 3;
                                    stop = 1;
                                }
                            }
                        }
                        if (doc == null)
                        {
                            stop = 1;
                        }
                        else
                        {
                            if (a == 2)
                            {
                                sql = @"
                             select qty_out from  WMS006A1 where material_no='" + kvp.Key.Split('*')[0] + @"'
                            and allocation_doc='" + doc + @"'
                            ";
                                qty_x = DB.GetDouble(sql);
                                p.Add("@qty_x", qty_x);
                            }
                            else
                            {
                                p.Add("@qty_x",Math.Round( qty_x,2));
                            }
                            sql = @"SELECT warehouse FROM BASE011M where location_no = '" + kvp.Key.Split('*')[1].Trim() + "'";
                            DataTable Base011m = DB.GetDataTable(sql);
                            string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
                            string modifytime = DateTime.Now.ToString("HH:mm:ss");


                            p.Add("@warehouse_target", Base011m.Rows[0][0].ToString());
                            p.Add("@location_target", kvp.Key.Split('*')[1].Trim());
                            p.Add("@modifyby", UserCode);
                            p.Add("@modifydate", modifydate);
                            p.Add("@modifytime", modifytime);

                            p.Add("@allocation_doc", doc);

                            sql = @"
 update WMS006A1 set qty=@qty_x,warehouse_target=@warehouse_target,location_target=@location_target,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no 
and allocation_doc=@allocation_doc
";

                            int i = DB.ExecuteNonQueryOffline(sql, p);

                            p.Clear();

                            string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS005TOWMS006'");
                            if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                            {
                                ret += Audit(OBJ, doc,0);//快速调拨
                            }
                        }
                        doc = null;
                    }

                    return ret;
                }

                //string Auto = DB.GetDataTable("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS005TOWMS006'").Rows[0][0].ToString();
                //if (Auto == "Auto")
                //{
                //    ret = Audit(OBJ, doc);
                //    return ret;
                //}
                #endregion
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
                    <allocation_doc>" + RetData + "</allocation _doc>" + msg1 + @"
                </Return>
            </WebService>
            ";

            return ret;
        }
    }
}
