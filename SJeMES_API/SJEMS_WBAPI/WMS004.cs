using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class WMS004
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
 WMS004(入库单)接口帮助 @END
 方法：Help(帮助),DoSure(审核) @END
 @END
====================DoSure方法调用=================== @END
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
      /// 
      /// </summary>
      /// <param name="OBJ"></param>
      /// <param name="storage_doc"></param>
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
                DllName = "SJEMS_WBAPI";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = "SJEMS_WBAPI.WMS003";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = "Audit";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 参数
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");

                string storage_doc = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<storage_doc>", "</storage_doc>");//入库单
              
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
                    string[] a = storage_doc.Split(t, StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    for (int i = 0; i < a.Length; i++)
                    {
                        bool IsOk = false;
                        string ErrMsg = string.Empty;

                        //2.	查询WMS004M是否存在该单号以及状态不为已审核，不存在报错：单号不存在，已审核报错：单号状态为已审核，不能重复审核
                        string sql = @"SELECT status FROM WMS004M WHERE storage_doc=@storage_doc";

                        p.Add("@storage_doc", a[i].ToString());

                        string status = DB.GetString(sql, p);
                        if (!string.IsNullOrEmpty(status) && status == "1")//存在单号且未审核
                        {
                            p.Clear();
                        
                            sql = @"SELECT qty,location,material_no,material_name,material_specifications,suppliers_lot,UDF01,UDF02,UDF03,UDF04,isnull(UDF06,0)UDF06,isnull(UDF07,0)UDF07 FROM WMS004A1 WHERE storage_doc=@storage_doc";
                            p.Add("@storage_doc", a[i].ToString());

                            double qty = 0;
                            string location = string.Empty;
                            string material_no = string.Empty;
                            string material_name = string.Empty;
                            string material_specifications = string.Empty;
                            string suppliers_lot = string.Empty;
                            string size = string.Empty;
                            double UDF06 = 0;
                            double UDF07 = 0;

                            System.Data.DataTable dtWMS004A1 = DB.GetDataTable(sql, p);
                            p.Clear();

                           //  查询WMS004A1该单是否存在实际入库数据
                            //  不存在报错：单号不存在实际入库数据。
                            //   存在的话，根据入库数据（按物料进行汇总）
                            if (dtWMS004A1.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtWMS004A1.Rows)
                                {
                                    qty = Convert.ToDouble(dr[0].ToString());
                                    location = dr[1].ToString();
                                    material_no = dr[2].ToString();
                                    material_name = dr[3].ToString();
                                    material_specifications = dr[4].ToString();
                                    suppliers_lot = dr[5].ToString();
                                    size = dr["UDF01"].ToString();
                                    //左右脚领料数量
                                    UDF06 = Convert.ToDouble(dr["UDF06"] + "");
                                    UDF07 = Convert.ToDouble(dr["UDF07"] + "");


                                    //判断是否是中性物料
                                    Boolean isNeuter = (size.Equals("") || size.Equals("NO SIZE")) ? true : false;

                                    IDataReader Row;
                                    sql = @"SELECT qty,material_no,isnull(UDF06,0)UDF06,isnull(UDF07,0)UDF07 FROM WMS012M WHERE material_no=@material_no";
                                    p.Add("@material_no", material_no);
                                    Row = DB.GetDataTableReader(sql, p);
                                    int code = 1;//入库
                                    if (Row.Read())//存在对应物料的库存
                                    {
                                        //调整WMS012M(库存资料）如果存在对应物料的库存，加上该单的入库数据，
                                        //WMS012.DataEdit(UserCode, material_no, qty, code, DB);//修改12M表
                                        Dictionary<string, object> tp = new Dictionary<string, object>();

                                        string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
                                        string modifytime = DateTime.Now.ToString("HH:mm:ss");
                                        tp.Add("@material_no", material_no);
                                        tp.Add("@qty", Math.Round(qty, 2));
                                        tp.Add("@modifyby", UserCode);
                                        tp.Add("@modifydate", modifydate);
                                        tp.Add("@modifytime", modifytime);

                                        qty += Convert.ToDouble(Row[0].ToString());

                                        if (isNeuter)
                                        {
                                            sql = @"
 update WMS012M set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no  and id = (select top 1 id from WMS012M where material_no=@material_no )
";
                                        }
                                        else
                                        {
                                            UDF06 += Convert.ToDouble((Row[2]+"").Equals("")?"0": (Row[2] + ""));       //左脚实际入库数量
                                            UDF07 += Convert.ToDouble((Row[2] + "").Equals("") ? "0" : (Row[2] + ""));       //右脚实际入库数量
                                            tp.Add("@UDF06",UDF06);
                                            tp.Add("@UDF07", UDF07);
                                            sql = @"
 update WMS012M set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime,UDF06=@UDF06,UDF07=@UDF07 where material_no=@material_no  and id = (select top 1 id from WMS012M where material_no=@material_no )
";
                                        }

                                        int ri = DB.ExecuteNonQueryOffline(sql, tp);
                                        if (ri < 1)
                                        {
                                            ErrMsg = "没有更新到数据";
                                        }

                                    }
                                    else
                                    {
                                        //不存在新建一条库存数据，调用WMS012.DataAdd(User,物料代号,库位,数量)方法新增
                                        if (isNeuter)
                                        {
                                            WMS012.DataAdd(UserCode, material_no, material_name, qty, material_specifications, DB);//增加12M表的数据
                                        }
                                        else
                                        {
                                            WMS012.DataAdd(UserCode, material_no, material_name, qty, UDF06, UDF07, material_specifications, DB);//增加12M表的数据
                                        }

                                    }

                                    sql = @"SELECT qty,material_no FROM WMS012A1 WHERE material_no=@material_no  and location=@location and UDF01='"+ size+@"'";
                                    p.Clear();
                                    p.Add("@material_no", material_no);
                                    p.Add("@location", location);
                                    Row = DB.GetDataTableReader(sql, p);
                                    if (Row.Read())
                                    {
                                        //调整WMS012A1(库存明细）如果存在对应物料、库位的库存，加上该单的入库数据，
                                        //WMS012.DataEdit(UserCode, material_no, location, qty, code, DB,size);//修改12A1表的数据
                                        Dictionary<string, object> p2 = new Dictionary<string, object>();

                                        string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
                                        string modifytime = DateTime.Now.ToString("HH:mm:ss");
                                        p2.Add("@material_no", material_no);
                                        p2.Add("@location", location);

                                        p2.Add("@qty", Math.Round(qty, 2));
                                        p2.Add("@modifyby", UserCode);
                                        p2.Add("@modifydate", modifydate);
                                        p2.Add("@modifytime", modifytime);

                                        if (isNeuter)
                                        {
                                            sql = @"
 update WMS012A1 set qty=@qty,qty_availble=@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and UDF01='" + size + @"'
";
                                        }
                                        else
                                        {
                                            UDF06 += Convert.ToDouble((Row[2] + "").Equals("") ? "0" : (Row[2] + ""));       //左脚实际入库数量
                                            UDF07 += Convert.ToDouble((Row[2] + "").Equals("") ? "0" : (Row[2] + ""));       //右脚实际入库数量
                                            p2.Add("@UDF06", UDF06);
                                            p2.Add("@UDF07", UDF07);
                                            sql = @"
 update WMS012A1 set qty=@qty,qty_availble=@qty,UDF06=@UDF06,UDF07=@UDF07,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where material_no=@material_no and location=@location and UDF01='" + size + @"'
";
                                        }
                                        

                                        int ri = DB.ExecuteNonQueryOffline(sql, p);
                                        if (ri < 1)
                                        {
                                            ErrMsg = "没有更新到数据";
                                        }



                                    }
                                    else
                                    {
                                        //不存在新建一条库存数据。调用WMS012.DataAdd(User,物料代号,库位,数量)方法新增
                                        if (isNeuter)
                                        {
                                            WMS012.DataAdd(UserCode, material_no, material_name, location, qty, material_specifications, DB, size);//增加12A1表的数据
                                        }
                                        else
                                        {
                                            WMS012.DataAdd(UserCode, material_no, material_name, location, qty,UDF06,UDF07, material_specifications, DB, size);//增加12A1表的数据
                                        }
                                        
                                    }


                                        //4.	根据入库单号，分别到CODE001A3(单品单据记录)、CODE002A3(批号单据记录)、CODE003A3(包装单据记录)里查询入库单号对应的条码记录，根据条码记录分别添加到WMS012A2(库存批号条码记录)、WMS012A3(库存包装条码记录)、WMS012A4(库存单品条码记录)中去
                                        #region
                                        try
                                    {
                                        //批号
                                        sql = @"SELECT DISTINCT CODE002A3.lot_barcode,warehouse_target,material_no,CODE002A3.qty,CODE002A3.UDF01,CODE002A3.UDF06,CODE002A3.UDF07 FROM CODE002A3 join CODE002M on CODE002M.lot_barcode=CODE002A3.lot_barcode WHERE CODE002A3.documents_type='7' and CODE002A3.documents=@documents and CODE002M.material_no='" + material_no + "' and CODE002A3.location_target='" + location + "' and CODE002A3.UDF01='"+size+@"'";

                                        p.Clear();
                                        p.Add("@documents", a[i].ToString());

                                        Row = DB.GetDataTableReader(sql, p);

                                        string lot_barcode = string.Empty;
                                        string warehouse = string.Empty;
                                        while (Row.Read())
                                        {
                                            lot_barcode = Row[0].ToString();
                                            warehouse = Row[1].ToString();
                                            qty = Convert.ToDouble(Row[3].ToString());
                                           
                                            //增加12A2表的数据,修改CODE002M的状态和库位
                                            WMS012.DataAdd(UserCode, material_no, material_name, location, qty,UDF06,UDF07, material_specifications, lot_barcode, suppliers_lot, DB, warehouse,size);
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        throw;
                                    }
                                    #endregion

                                    //5.	更新单号对应的状态和审核人，审核时间，修改人，修改时间
                                    #region
                                    sql = @"select front_document from WMS004M where storage_doc ='" + a[i].ToString() + "'";
                                    DataTable dt_plan = DB.GetDataTable(sql);
                                    if (dt_plan.Rows[0][0].ToString() != "")//计划出库
                                    {
                                        
                                            string auditby = UserCode;
                                            string auditdatetime = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
                                            string modifyby = UserCode;
                                            string modifydate = DateTime.Today.ToString("yyyy-MM-dd");
                                            status = "2";//已经审核
                                            DataEditStatus(a[i].ToString(), status, auditby, auditdatetime, modifyby, modifydate, DB);//修改04M表数据
                                       
                                    }
                                    else//快速入库
                                    {
                                        string auditby = UserCode;
                                        string auditdatetime = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
                                        string modifyby = UserCode;
                                        string modifydate = DateTime.Today.ToString("yyyy-MM-dd");
                                        status = "2";//已经审核
                                        DataEditStatus(a[i].ToString(), status, auditby, auditdatetime, modifyby, modifydate, DB);//修改04M表数据
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

            p.Add("@storage_doc", p1);

            sql = @"
SELECT status,auditby,auditdatetime,modifyby,modifydate FROM WMS004M where storage_doc=@storage_doc 
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
 update WMS004M set status=@status,auditby=@auditby,auditdatetime=@auditdatetime,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where storage_doc=@storage_doc
";

                int i = DB.ExecuteNonQueryOffline(sql, p);
            }

        }

        /// <summary>
        /// 快速入库
        /// </summary>
        /// <param name="OBJ"></param>
        /// <returns></returns>
        public static string QuickWarehousing(object OBJ)
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

                #region 根据扫描的条码数据进行汇总（按物料库位）入库数量，添加汇总数据到wms004a1(入库明细表)
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
                #endregion

                #region 逻辑
                string sql = string.Empty;
                DataTable dt4;

                #region  1.产生入库单号
                sql = @"
SELECT MAX(storage_doc) FROM WMS004M
";
                string storage_doc = DB.GetString(sql);

                if (storage_doc.IndexOf(DateTime.Now.ToString("yyyyMMdd")) > -1)
                {
                    storage_doc = DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(storage_doc.Replace(DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");
                }
                else
                {
                    storage_doc = DateTime.Now.ToString("yyyyMMdd") + "0001";
                }

                #endregion

                #region 2.新增入库明细
                //产生入库表头（汇总状态，每次入库只有一条）
                sql = @"insert into WMS004M(storage_doc,storage_type,status,auditby,auditdatetime,createby,createtime) values('" + storage_doc + "','6','1','" + UserCode + "','" + DateTime.Now.ToString() + "','" + UserCode + "','" + DateTime.Now.ToString() + "')";
                DB.ExecuteNonQueryOffline(sql);

                foreach (KeyValuePair<string, double> kvp1 in Nums1)
                {
                    string material_no = kvp1.Key.Split('*')[0].Trim();
                    string location = kvp1.Key.Split('*')[1].Trim();
                    double qty = Convert.ToDouble(kvp1.Value.ToString());
                    string serial_number = string.Empty;

                    //产生序号
                    dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM WMS004A1 where storage_doc='" + storage_doc + "'");
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
                    //判断入库物料是否存在
                    if (Base007m.Rows.Count > 0)
                    {
                        //新增入库明细（实际物料入库情况，同一物料同一库位一条记录）
                        sql = @" insert into WMS004A1
(storage_doc,serial_number,material_no,material_name,material_specifications,qty,location,createby,createdate,createtime)  
values('" + storage_doc + "','" + serial_number + "','" + material_no + "','" + Base007m.Rows[0][1].ToString() + "','" + Base007m.Rows[0][2].ToString() + "','" + qty + "','" + location + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                        DB.ExecuteNonQueryOffline(sql);
                    }

                    RetData += kvp1.Key.Split('*')[0].Trim() + ",";
                }

                #endregion

                for (int i = 0; i < tmdt.Rows.Count; i++)
                {
                    string serial_number = string.Empty;
                    //根据传入的单品、批号、箱号，记录条码，往A1表插入一条记录记录操作和计划库位，往A3表插入一条记录记录单据类型和实际库位和数量，每个条码都有一条记录
                    switch (tmdt.Rows[i]["BarCodeType"].ToString().Trim())
                    {
                        //单品
                        case "1":
                            dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE001A3 where documents='" + storage_doc + "'");
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
                            sql = @"INSERT INTO CODE001A1 (products_barcode,operation,warehouse,location,packing_barcode,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','5','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + dt3.Rows[0]["packing_barcode"] + "','" + dt3.Rows[0]["lot_barcode"] + "','" + dt3.Rows[0]["org"] + "')";
                            sql += @"INSERT INTO CODE001A3 (products_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','6','" + storage_doc + "','" + serial_number + "','5','" + warehouse + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt3.Rows[0]["org"] + "')";
                            DB.ExecuteNonQueryOffline(sql);
                            break;

                        //批号
                        case "2":
                            dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE002A3 where documents='" + storage_doc + "'");
                            if (dt4.Rows[0][0].ToString() == "")
                            {
                                serial_number = "001";
                            }
                            else
                            {
                                serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                            }
                            string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            System.Data.DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','5','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "') ";
                            sql += "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org) VALUES('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','6','" + storage_doc + "','" + serial_number + "','5','" + warehouse1 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "')";
                            DB.ExecuteNonQueryOffline(sql);
                            break;

                        //箱号
                        case "3":
                            dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE003A3 where documents='" + storage_doc + "'");
                            if (dt4.Rows[0][0].ToString() == "")
                            {
                                serial_number = "001";
                            }
                            else
                            {
                                serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                            }
                            string warehouse2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                            System.Data.DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE003M where packing_barcode='" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "'");
                            sql = @"INSERT INTO CODE003A1 (packing_barcode,operation,warehouse,location,qty,lot_barcode,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','6','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["lot_barcode"] + "','" + dt32.Rows[0]["org"] + "') ";
                            sql += @"INSERT INTO CODE003A3 (packing_barcode	,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org) VALUES ('" + tmdt.Rows[i]["BarCode"].ToString().Trim() + "','6','" + storage_doc + "','" + serial_number + "','5','" + warehouse2 + "','" + tmdt.Rows[i]["Location"].ToString().Trim() + "','" + tmdt.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["org"] + "')";
                            DB.ExecuteNonQueryOffline(sql);
                            break;
                        default: break;
                    }

                }
                string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS003TOWMS004'");
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
                <storage_doc>" + storage_doc + @"</storage_doc>
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
