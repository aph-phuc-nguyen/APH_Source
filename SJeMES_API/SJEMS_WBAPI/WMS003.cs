using GDSJ_Framework.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    class WMS003
    {

        public static string GETDATA(object OBJ)
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
                DllName = "SJEMS_WBAPI";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = "SJEMS_WBAPI.WMS003";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = "GETDATA";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 接口参数
                string storage_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<storage_plan>", "</storage_plan>");

                #endregion

                string where = string.Empty;

                if (storage_plan.Contains(","))
                {
                    string[] tmpStr = storage_plan.Split(',');
                    foreach (string s in tmpStr)
                    {
                        if (!string.IsNullOrEmpty(where))
                        {
                            where += ",";
                        }

                        where += "'" + s + @"'";
                    }
                }
                else
                {
                    where = "'" + storage_plan + @"'";
                }

               

                #region 逻辑
                string sql = @"
SELECT storage_plan,storage_type,front_document,front_document_type,status 
FROM WMS003M
WHERE storage_plan in (" + where + @")
";
                string sql1 = @"
SELECT storage_plan,serial_number,material_no,material_name,material_specifications,qty_plan,location_plan,qty,UDF01,UDF02,UDF03,UDF04,UDF05,UDF06,UDF07,UDF08,UDF09,UDF10 
FROM WMS003A1
WHERE storage_plan in (" + where + @")
";
                string sql2 = @"
SELECT storage_plan,serial_number,material_no,material_name,material_specifications,status,qty_plan,location_plan,qty,UDF01,UDF02,UDF03,UDF04,UDF05,UDF06,UDF07,UDF08,UDF09,UDF10 
FROM WMS003A2
WHERE storage_plan in (" + where + @")
";
                Dictionary<string, object> p = new Dictionary<string, object>();

                System.Data.DataTable dt = DB.GetDataTable(sql, p);//3m表
                string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                System.Data.DataTable dt1 = DB.GetDataTable(sql1, p);//03A1表
                string dtXML1 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt1);

                System.Data.DataTable dt2 = DB.GetDataTable(sql2, p);//03A2表
                string dtXML2 = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt2);

                if (dt.Rows.Count > 0)
                {
                    IsSuccess = true;
                    RetData = "<WMS003M>" + dtXML + @"</WMS003M>" + "<WMS003A1>" + dtXML1 + @"</WMS003A1>" + "<WMS003A2>" + dtXML2 + @"</WMS003A2>";
                }

                else
                {
                    RetData = "不存在数据";
                }
                #endregion


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


        public static string DoWork(object OBJ)
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


                DllName = "SJEMS_WBAPI";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = "SJEMS_WBAPI.WMS003";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = "DoWork";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);
                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 接口参数
                string storage_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<storage_plan>", "</storage_plan>");//入库计划
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");//操作人（新增、修改、审核）
                string BarCodes = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodes>", "</BarCodes>");
                string planDetails = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<PlanDetails>", "</PlanDetails>");
                string hzrk = "";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<collectInStock>", "</collectInStock>");//汇总入库
                //
                DataTable tmdt_temp = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(BarCodes);//计划入库实际明细
                //条码明细
                DataTable codeTable = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(planDetails);

                DataTable tmdt1;//汇总出库实际明细1
                DataTable tmdt2;//汇总出库实际明细2
                string where = string.Empty;
                #endregion

                if (hzrk != "")//汇总入库
                {
                    string DataTable1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Collect>", "</Collect>");
                    tmdt1 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable1);
                    
                    DataView dv = new DataView(tmdt1); //虚拟视图
                    DataTable dt2 = dv.ToTable(true, "Plan");
                    foreach (DataRow dr_dt2 in dt2.Rows)
                    {
                        storage_plan += dr_dt2.ItemArray[0].ToString() + ",";
                    }
                    
                    #region 汇总入库截取计划数组
                    string[] b;
                    if (storage_plan.Contains(","))
                    {
                        string[] s = new string[1];
                        s[0] = ",";
                        b = storage_plan.Split(s, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        b = new string[1];
                        b[0] = storage_plan;
                    }

                    #endregion
                    
                    string DataTable2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Operation>", "</Operation>");
                    tmdt2 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable2);

                    for (int j = 0; j < b.Length; j++)//多个入库计划
                    {
                        DataRow[] dr_tmdt = tmdt1.Select("Plan='" + b[j].ToString() + "'");
                        DataTable tmdt_3 = tmdt1.Clone();
                        DataTable tmdt_4 = tmdt2.Clone();
                        string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS003TOWMS004'");//是否自动审核

                        foreach (DataRow row in dr_tmdt)
                        {
                            tmdt_3.ImportRow(row); // 将DataRow添加到DataTable中
                        }

                        if (!string.IsNullOrEmpty(tmdt2.Rows[0][5].ToString()))//单、批、箱
                        {
                            DataRow[] dr_tmdt2 = tmdt2.Select("Plan='" + b[j].ToString() + "'");
                            foreach (DataRow row in dr_tmdt2)
                            {
                                tmdt_4.ImportRow(row); // 将DataRow添加到DataTable中
                            }

                            RetData = DoWork_hzrk2(tmdt_3, tmdt_4, DB, UserCode, b[j].ToString());
                            if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                            {
                                ret += Audit_hzrk(OBJ, tmdt_4, b[j].ToString());
                            }
                        }
                        else//料
                        {
                            RetData = DoWork_hzrk2(tmdt_3, tmdt2, DB, UserCode, b[j].ToString());
                            if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                            {
                                ret += Audit_hzrk(OBJ, tmdt2, b[j].ToString());
                            }
                        }

                    }
                    return ret;
                }
                else
                {
                    #region 截取计划字段
                    if (storage_plan.Contains(","))
                    {
                        string[] tmpStr = storage_plan.Split(',');
                        foreach (string s in tmpStr)
                        {
                            if (!string.IsNullOrEmpty(where))
                            {
                                where += ",";
                            }

                            where += "'" + s + @"'";
                        }
                    }
                    else
                    {
                        where = "'" + storage_plan + @"'";
                    }
                    #endregion

                    #region 计划入库截取计划数组
                    string[] a;
                    if (storage_plan.Contains(","))
                    {
                        string[] s = new string[1];
                        s[0] = ",";
                        a = storage_plan.Split(s, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        a = new string[1];
                        a[0] = storage_plan;
                    }
                    #endregion

                
                    for (int j = 0; j < a.Length; j++)//多个入库计划
                    {
                        string sql = string.Empty;
                        
                        if (string.IsNullOrEmpty(RetData))
                        {
                            #region 逻辑
                            sql = @"SELECT storage_plan FROM WMS003M WHERE storage_plan in (" + where + @")";

                            //Dictionary<string, object> p = new Dictionary<string, object>();
                            DataTable dt = DB.GetDataTable(sql);
                            //string dtXML = GDSJ_Framework.Common.StringHelper.GetXMLFromDataTable(dt);

                            //2.根据扫描的条码数据进行汇总（按物料）入库数量，获取WMS003A1的资料，汇总WMS003A1的可入库数量（按物料，汇总计划入库数量- 汇总实际入库数量），
                            //如果扫描条码的物料入库数量大于对应物料的可入库数量，获取物料资料的
                            //StorageType,如果是不允许超入的话，报错：入库数量大于计划入库数量（物料代号、物料名称、物料规格、计划数量、已入库数量、当前入库数量）

                            Dictionary<string, double> Nums = new Dictionary<string, double>();
                            Dictionary<string, double> Nums1 = new Dictionary<string, double>();
                            if (dt.Rows.Count > 0)
                            {
                                #region 根据物料和数量汇总，根据物料加库位汇总
                                int rowsSize = tmdt_temp.Rows.Count;
                                for (int i = 0; i < rowsSize; i++)
                                {
                                    DataRow dtRow = tmdt_temp.Rows[i];
                                    //判断是否是中性物料（是否分size）
                                    string Size = dtRow["Size"].ToString().Trim();
                                    Boolean isNeuter = false;
                                    if (Size.Equals("")||Size.Equals("NO SIZE"))
                                    {
                                        isNeuter = true;
                                    }

                                    if (isNeuter)
                                    {
                                        if (Nums.ContainsKey(dtRow["Material_no"].ToString()))//按物料汇总，当传入的物料和库位都相同的时候，把他看作一条入库明细
                                        {
                                            Nums[dtRow["Material_no"].ToString()] += Convert.ToDouble(dtRow["Qty"].ToString());
                                        }
                                        else
                                        {
                                            Nums.Add(dtRow["Material_no"].ToString(), Convert.ToDouble(dtRow["Qty"].ToString()));
                                        }
                                        //按物料和库位+size+size序号进行汇总，保证传入的物料加库位+size序号是唯一的
                                        if (Nums1.ContainsKey(dtRow["Material_no"].ToString().Trim() + "*" + dtRow["Location"].ToString().Trim() + "*" + dtRow["Size"].ToString().Trim() +"*"+dtRow["UDF02"].ToString().Trim()))
                                        {
                                            Nums1[dtRow["Material_no"].ToString().Trim() + "*" + dtRow["Location"].ToString().Trim() + "*" + dtRow["Size"].ToString().Trim() + "*" + dtRow["UDF02"].ToString().Trim()] += Convert.ToDouble(dtRow["Qty"].ToString().Trim());
                                        }
                                        else
                                        {
                                            Nums1.Add(dtRow["Material_no"].ToString().Trim() + "*" + dtRow["Location"].ToString().Trim()+"*" + dtRow["Size"].ToString().Trim() + "*" + dtRow["UDF02"].ToString().Trim(), Convert.ToDouble(dtRow["Qty"].ToString().Trim()));
                                        }
                                    }
                                    else
                                    {

                                    }

                                    


                                }
                                #endregion

                                //kvp.Key;物料代号
                                //kvp.Value;入库数量

                                string StorageType = string.Empty;
                                string serial_number = string.Empty;

                                DataTable dt4;//产生序号

                               
                                foreach (KeyValuePair<string, double> kvp in Nums)
                                {
                                    double qty1 = 0;//汇总计划入库数量
                                    double qty2 = 0;//汇总实际入库数量  可入库数量=qty1-qty2

                                    try
                                    {
                                        sql = "SELECT material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,location_plan FROM WMS003A1 WHERE material_no='" + kvp.Key.Trim() + "'and storage_plan='" + storage_plan + "'";
                                        DataTable dt1 = DB.GetDataTable(sql);
                                        //根据扫描的条码数据进行汇总（按物料）入库数量，获取WMS003A1的资料
                                        if (dt1.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt1.Rows.Count; i++)
                                            {
                                                qty1 += double.Parse(dt1.Rows[i]["qty_plan"].ToString());//计划入库
                                                qty2 += double.Parse(dt1.Rows[i]["qty"].ToString());//已入库
                                            }
                                        }

                                        //sql = "SELECT material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,location_plan FROM WMS003A1 WHERE material_no='" + kvp.Key.Trim() + "' and storage_plan='" + storage_plan + "'";
                                        //DataTable dt2 = DB.GetDataTable(sql);

                                        if (kvp.Value <= (qty1 - qty2))
                                        {
                                            // 3.根据扫描条码数据进行汇总（按物料、入库库位），把数据插入到WMS003A2中去，审核状态为1。 
                                            //更新WMS003A1的实际入库数量WMS003A1.qty
                                            //更新WMS003A2的qty_plan=WMS003A1.qty_plan-WMS003A1.qty
                                            foreach (KeyValuePair<string, double> kvp1 in Nums1)
                                            {
                                                if (kvp.Key.Trim() == kvp1.Key.Split('*')[0].Trim())
                                                {


                                                    Double qtyReal = 0;
                                                    Double qtyReal2 = kvp1.Value;

                                                    DataTable dttmp = DB.GetDataTable(@"SELECT serial_number,qty_plan,qty,lot_barcode FROM WMS003A1(NOLOCK) WHERE storage_plan='" + storage_plan + @"'  AND material_no='" + kvp.Key.Trim() + "'  AND qty<qty_plan ORDER BY serial_number");

                                                    for (int i = 0; i < dttmp.Rows.Count; i++)
                                                    {
                                                        sql = string.Empty;

                                                        if (qtyReal2 > 0)
                                                        {
                                                            string sn = dttmp.Rows[i][0].ToString();

                                                            double i_qty = Convert.ToDouble(dttmp.Rows[i][2].ToString());
                                                            double i_qty_plan = Convert.ToDouble(dttmp.Rows[i][1].ToString());
                                                            string lot_barcode = dttmp.Rows[i][3].ToString();

                                                            if (i == dttmp.Rows.Count - 1)
                                                            {
                                                                qtyReal = qtyReal2;
                                                            }
                                                            else
                                                            {
                                                                if (qtyReal2 > i_qty_plan - i_qty)
                                                                {
                                                                    qtyReal = i_qty_plan - i_qty;
                                                                    qtyReal2 -= qtyReal;
                                                                }
                                                                else
                                                                {
                                                                    qtyReal = qtyReal2;
                                                                    qtyReal2 -= qtyReal2;
                                                                }
                                                            }

                                                            sql += @"
UPDATE WMS003A1 SET qty=Convert(decimal(18,2),qty+" + qtyReal + @"),modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
WHERE storage_plan = '" + storage_plan + @"' 
and serial_number='" + sn + @"'";

                                                            #region 产生调拨序号
                                                            serial_number = DB.GetString("SELECT max(CONVERT(int,serial_number)) FROM WMS003A2 where storage_plan='" + storage_plan + "'");
                                                            if (serial_number == "")
                                                            {
                                                                serial_number = "001";
                                                            }
                                                            else
                                                            {
                                                                serial_number = (int.Parse(serial_number) + 1).ToString("000");
                                                            }
                                                            #endregion

                                                            string size = kvp1.Key.Split('*')[2];

                                                            

                                                            sql += "INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,location,status,createby,createdate,createtime,UDF01) VALUES ('" + storage_plan + "','" + serial_number + "','" + kvp.Key.Trim() + "','" + dt1.Rows[0]["material_name"].ToString() + "','" + dt1.Rows[0]["material_specifications"].ToString() + "','" + dt1.Rows[0]["suppliers_lot"].ToString() + "','" + lot_barcode + "','" + i_qty_plan + "','" + dt1.Rows[0]["location_plan"].ToString() + "','" + Math.Round(qtyReal, 2) + "','" + kvp1.Key.Split('*')[1] + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','"+ size+@"') ";

                                                            //WMS003A2表是实际入库，先插入一条数据，记录实际入库数量，WMS003A1是计划入库，再将实际入库数量记录更新A1表，将A1表的计划数量对比实际入库数量得出差，将差更新到A2表就是还需入库的数量
                                                            //sql = "INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,location,status,createby,createdate,createtime) VALUES ('" + storage_plan + "','" + serial_number + "','" + kvp.Key.Trim() + "','" + dt2.Rows[0]["material_name"].ToString() + "','" + dt2.Rows[0]["material_specifications"].ToString() + "','" + dt2.Rows[0]["suppliers_lot"].ToString() + "','" + dt2.Rows[0]["lot_barcode"].ToString() + "','" + dt2.Rows[0]["qty_plan"].ToString() + "','" + dt2.Rows[0]["location_plan"].ToString() + "','" + kvp1.Value + "','" + kvp1.Key.Split('*')[1] + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ") + "','" + DateTime.Now.ToString("HH:mm:ss") + "') ";
                                                            //sql += " UPDATE WMS003A1 SET qty=qty+" + kvp1.Value + " where storage_plan='" + storage_plan + "' and material_no='" + kvp.Key.Trim() + "'";
                                                            //sql += " UPDATE WMS003A2 set qty_plan= (select qty_plan-qty from WMS003A1 where storage_plan='" + storage_plan + "' and material_no='" + kvp.Key.Trim() + "') where storage_plan='" + storage_plan + "' and material_no='" + kvp.Key.Trim() + "' and serial_number='" + serial_number + "' ";
                                                            DB.ExecuteNonQueryOffline(sql);
                                                            RetData += kvp1.Key.Split('*')[0].Trim() + ",";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //StorageType = "0";
                                            StorageType = DB.GetDataTable("SELECT isover_in FROM BASE007M WHERE material_no='" + kvp.Key.Trim() + "'").Rows[0][0].ToString().Trim();
                                            if (StorageType == "FALSE")//如果是不允许超入
                                            {
                                                RetData = "入库数量大于计划入库数量[" + kvp.Key.Trim() + "," + dt1.Rows[0]["material_name"].ToString() + "," + dt1.Rows[0]["material_specifications"].ToString() + "," + dt1.Rows[0]["qty_plan"].ToString() + "," + dt1.Rows[0]["qty"].ToString() + "," + kvp.Value + "]";
                                            }
                                            else
                                            {
                                                // 3.根据扫描条码数据进行汇总（按物料、入库库位），把数据插入到WMS003A2中去，审核状态为1。
                                                //更新WMS003A1的实际入库数量WMS003A1.qty
                                                //更新WMS003A2的qty_plan=WMS003A1.qty_plan-WMS003A1.qty
                                                foreach (KeyValuePair<string, double> kvp1 in Nums1)
                                                {
                                                    if (kvp.Key.Trim() == kvp1.Key.Split('*')[0].Trim())
                                                    {
                                                        Double qtyReal = 0;
                                                        Double qtyReal2 = kvp.Value;

                                                        DataTable dttmp = DB.GetDataTable(@"SELECT serial_number,qty_plan,qty,lot_barcode FROM WMS003A1(NOLOCK) WHERE storage_plan='" + storage_plan + @"'  AND material_no='" + kvp.Key.Trim() + "'  AND qty<qty_plan ORDER BY serial_number");

                                                        for (int i = 0; i < dttmp.Rows.Count; i++)
                                                        {
                                                            sql = string.Empty;

                                                            if (qtyReal2 > 0)
                                                            {
                                                                string sn = dttmp.Rows[i][0].ToString();

                                                                double i_qty = Convert.ToDouble(dttmp.Rows[i][2].ToString());
                                                                double i_qty_plan = Convert.ToDouble(dttmp.Rows[i][1].ToString());
                                                                string lot_barcode = dttmp.Rows[i][3].ToString();

                                                                if (i == dttmp.Rows.Count - 1)
                                                                {
                                                                    qtyReal = qtyReal2;
                                                                }
                                                                else
                                                                {
                                                                    if (qtyReal2 > i_qty_plan - i_qty)
                                                                    {
                                                                        qtyReal = i_qty_plan - i_qty;
                                                                        qtyReal2 -= qtyReal;
                                                                    }
                                                                    else
                                                                    {
                                                                        qtyReal = qtyReal2;
                                                                        qtyReal2 -= qtyReal2;
                                                                    }
                                                                }

                                                                sql += @"
UPDATE WMS003A1 SET qty=Convert(decimal(18,2),qty+" + qtyReal + @"),modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
WHERE storage_plan = '" + storage_plan + @"' 
and serial_number='" + sn + @"'";

                                                                #region 产生调拨序号
                                                                serial_number = DB.GetString("SELECT max(CONVERT(int,serial_number)) FROM WMS003A2 where storage_plan='" + storage_plan + "'");
                                                                if (serial_number == "")
                                                                {
                                                                    serial_number = "001";
                                                                }
                                                                else
                                                                {
                                                                    serial_number = (int.Parse(serial_number) + 1).ToString("000");
                                                                }
                                                                #endregion


                                                                string size = kvp1.Key.Split('*')[2];


                                                                sql += "INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,location,status,createby,createdate,createtime,UDF01) VALUES ('" + storage_plan + "','" + serial_number + "','" + kvp.Key.Trim() + "','" + dt1.Rows[0]["material_name"].ToString() + "','" + dt1.Rows[0]["material_specifications"].ToString() + "','" + dt1.Rows[0]["suppliers_lot"].ToString() + "','" + lot_barcode + "','" + i_qty_plan + "','" + dt1.Rows[0]["location_plan"].ToString() + "','" + Math.Round(qtyReal, 2) + "','" + kvp1.Key.Split('*')[1] + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','"+size+@"') ";

                                                                //WMS003A2表是实际入库，先插入一条数据，记录实际入库数量，WMS003A1是计划入库，再将实际入库数量记录更新A1表，将A1表的计划数量对比实际入库数量得出差，将差更新到A2表就是还需入库的数量
                                                                //sql = "INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,location,status,createby,createdate,createtime) VALUES ('" + storage_plan + "','" + serial_number + "','" + kvp.Key.Trim() + "','" + dt2.Rows[0]["material_name"].ToString() + "','" + dt2.Rows[0]["material_specifications"].ToString() + "','" + dt2.Rows[0]["suppliers_lot"].ToString() + "','" + dt2.Rows[0]["lot_barcode"].ToString() + "','" + dt2.Rows[0]["qty_plan"].ToString() + "','" + dt2.Rows[0]["location_plan"].ToString() + "','" + kvp1.Value + "','" + kvp1.Key.Split('*')[1] + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ") + "','" + DateTime.Now.ToString("HH:mm:ss") + "') ";
                                                                //sql += " UPDATE WMS003A1 SET qty=qty+" + kvp1.Value + " where storage_plan='" + storage_plan + "' and material_no='" + kvp.Key.Trim() + "'";
                                                                //sql += " UPDATE WMS003A2 set qty_plan= (select qty_plan-qty from WMS003A1 where storage_plan='" + storage_plan + "' and material_no='" + kvp.Key.Trim() + "') where storage_plan='" + storage_plan + "' and material_no='" + kvp.Key.Trim() + "' and serial_number='" + serial_number + "' ";
                                                                DB.ExecuteNonQueryOffline(sql);
                                                                RetData += kvp1.Key.Split('*')[0].Trim() + ",";
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex) { }

                                }

                                List<string> bars = new List<string>();

                                //4.把扫描条码，根据条码的类型，分别插入到 CODE001A1(单品条码操作记录)、CODE001A3(单品条码单据记录)、CODE002A1(批号条码操作记录)、CODE002A3(批号条码单据操作记录)、CODE003A1(包装条码操作记录)、CODE003A3(包装条码单据记录)中。
                                //   0是品号 Base007m，1是单品code001m，2是批号code002m，3是包装code003m
                                for (int i = 0; i < tmdt_temp.Rows.Count; i++)
                                {
                                    #region 产生批号

                                    string barcode = tmdt_temp.Rows[i]["BarCode"].ToString();

                                    string size = tmdt_temp.Rows[i]["size"].ToString();

                                    string location = tmdt_temp.Rows[i]["Location"].ToString();


                                    string lot_barcode;

                                    string material_no = barcode.Split(',')[4];
                                    string material_name = barcode.Split(',')[10];

                                    lot_barcode = DB.GetString("select lot_barcode FROM CODE002M(NOLOCK) WHERE UDF02='" + barcode + @"' and UDF01='"+size+@"' ");

                                        if (string.IsNullOrEmpty(lot_barcode))
                                        {
                                            lot_barcode = DB.GetString("SELECT ISNULL(MAX(lot_barcode),'LOT" + DateTime.Now.ToString("yyyyMMdd") + "0000') FROM CODE002M WHERE lot_barcode like 'LOT" + DateTime.Now.ToString("yyyyMMdd") + @"%'");

                                            lot_barcode = "LOT" + DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(lot_barcode.Replace("LOT" + DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");

                                            

                                            sql = @"
INSERT INTO CODE002M
(lot_barcode,material_no,location,material_name,material_specifications,qty,status,createby,createdate,createtime,UDF01,UDF02)
VALUES
('" + lot_barcode + @"','" + material_no + @"','"+location+@"','" + material_name + @"',''," + tmdt_temp.Rows[i]["qty"].ToString() + @",'1','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + size + @"','" + barcode + @"')
";
                                            DB.ExecuteNonQueryOffline(sql);


                                            
                                        }
                                        else
                                        {

                                        string id= DB.GetString("select id FROM CODE002M(NOLOCK) WHERE lot_barcode='"+lot_barcode+@"' and UDF02='" + barcode + @"' and UDF01='" + size + @"' and location='"+location+@"'");


                                        if(!string.IsNullOrEmpty(id))
                                        {
                                            sql = @"
UPDATE CODE002M SET qty=qty+"+ tmdt_temp.Rows[i]["qty"].ToString()+@"
WHERE id ="+id+@"
";
                                            DB.ExecuteNonQueryOffline(sql);
                                        }
                                        else
                                        {
                                            sql = @"
INSERT INTO CODE002M
(lot_barcode,material_no,location,material_name,material_specifications,qty,status,createby,createdate,createtime,UDF01,UDF02)
VALUES
('" + lot_barcode + @"','" + material_no + @"','" + location + @"','" + material_name + @"',''," + tmdt_temp.Rows[i]["qty"].ToString() + @",'1','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + size + @"','" + barcode + @"')
";
                                            DB.ExecuteNonQueryOffline(sql);

                                           
                                        }

                                        
                                    }
                                    


                                    #endregion

                                    lot_barcode = DB.GetString("select lot_barcode FROM CODE002M(NOLOCK) WHERE UDF02='" + barcode + @"' and UDF01='"+size+@"' and location='"+tmdt_temp.Rows[i]["Location"].ToString().Trim()+@"'");

                                    //批号

                                    dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE002A3 where documents='" + storage_plan + "'");
                                    if (dt4.Rows[0][0].ToString() == "")
                                    {
                                        serial_number = "001";
                                    }
                                    else
                                    {
                                        serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                                    }
                                    string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt_temp.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                    System.Data.DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + lot_barcode + "'");
                                    sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org,createby,createdate,createtime,UDF01) VALUES ('" + lot_barcode + "','6','" + warehouse1 + "','" + tmdt_temp.Rows[i]["Location"].ToString().Trim() + "','" + tmdt_temp.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','"+size+@"')";
                                    sql += "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime,UDF01) VALUES('" + lot_barcode + "','6','" + storage_plan + "','" + serial_number + "','5','" + warehouse1 + "','" + tmdt_temp.Rows[i]["Location"].ToString().Trim() + "','" + tmdt_temp.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + size + @"')";
                                    DB.ExecuteNonQueryOffline(sql);




                                }
                                string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS003TOWMS004'");
                                if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                                {
                                    ret = Audit(OBJ, tmdt_temp, a[j].ToString());
                                    return ret;
                                }
                            }

                            else
                            {
                                RetData = "入库计划不存在";
                            }
                            #endregion
                            
                        }

                    }//汇总入库判断
                }

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
                    <storage_doc>" + RetData + "</storage_doc>" + msg + @"
                </Return>
            </WebService>
            ";

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OBJ"></param>
        /// <param name="tmdt"></param>
        /// <param name="Plan"></param>
        /// <returns></returns>
        private static string Audit_hzrk(object OBJ, DataTable tmdt, string Plan)
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
                string DBTYPE = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBTYPE>", "</DBTYPE>");
                string DBSERVER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBSERVER>", "</DBSERVER>");
                string DBNAME = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBNAME>", "</DBNAME>");
                string DBUSER = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBUSER>", "</DBUSER>");
                string DBPASSWORD = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DBPASSWORD>", "</DBPASSWORD>");

                #region 接口参数
                //string storage_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<storage_plan>", "</storage_plan>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string DataTableBarCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                #endregion


                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS003"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {
                    bool IsOk = false;
                    string ErrMsg = string.Empty;

                    DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                    guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                    //2.查询WMS003A2看是否有没有审核过的数据，如果没有，报错：该计划没有新的入库资料，不能进行审核。

                    string sql = @"
SELECT COUNT(*) FROM WMS003A2 WHERE storage_plan=@storage_plan
";
                    Dictionary<string, object> p1 = new Dictionary<string, object>();
                    p1.Add("@storage_plan", Plan);
                    string rowNumber = DB.GetString(sql, p1);

                    if (rowNumber == "0")  //报错
                    {
                        IsOk = false;
                        ErrMsg = "该计划没有新的入库资料，不能进行审核";
                    }
                    else //未审核
                    {
                        string storage_doc = string.Empty;

                        #region 根据WMS003M,WMS003A2数据产生WMS004M和WMS004A1数据，并更新03A2表的状态为2已经审核
                        sql = @"SELECT storage_plan,storage_type,front_document_type,front_document,status,dosureby,dosuredatetime,auditby,auditdatetime from WMS003M where storage_plan=@storage_plan  ";
                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        p2.Add("@storage_plan", Plan);
                        IDataReader idr = DB.GetDataTableReader(sql, p2);

                        #region 产生入库单号
                        sql = @"
SELECT ISNULL(MAX(storage_doc),'" + DateTime.Now.ToString("yyyyMMdd") + @"0000')
FROM WMS004M(NOLOCK)
WHERE storage_doc LIKE '" + DateTime.Now.ToString("yyyyMMdd") + @"%'
";

                        IDataReader dr = DB.GetDataTableReader(sql);
                        if (dr.Read())//004M
                        {
                            storage_doc = DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(dr[0].ToString().Replace(DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");
                        }
                        #endregion

                        while (idr.Read())//003M
                        {
                            string storage_type = idr["storage_type"].ToString();
                            string front_document_type = idr["front_document_type"].ToString();
                            string front_document = idr["front_document"].ToString();

                            string dosureby = idr["dosureby"].ToString();
                            string dosuredatetime = idr["dosuredatetime"].ToString();
                            string auditby = idr["auditby"].ToString();
                            string auditdatetime = idr["auditdatetime"].ToString();

                            sql = @"
INSERT INTO WMS004M(storage_doc,storage_type,front_document_type,front_document,status,createby,createdate,createtime)
values
('" + storage_doc + "','" + storage_type + @"','6','" + Plan + @"','1','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')
";

                            DB.ExecuteNonQueryOffline(sql);

                            sql = @"
SELECT storage_plan,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,qty,location from WMS003A2 WHERE storage_plan=@storage_plan  AND status='1'
";

                            Dictionary<string, object> p3 = new Dictionary<string, object>();
                            p3.Add("@storage_plan", Plan);
                            IDataReader idr2 = DB.GetDataTableReader(sql, p3);
                            while (idr2.Read())//003A2表
                            {
                                string serial_number = idr2["serial_number"].ToString();
                                string material_no = idr2["material_no"].ToString();
                                string material_name = idr2["material_name"].ToString();
                                string material_var = idr2["material_var"].ToString();
                                string material_specifications = idr2["material_specifications"].ToString();
                                string suppliers_lot = idr2["suppliers_lot"].ToString();
                                string lot_barcode = idr2["lot_barcode"].ToString();
                                double qty = Convert.ToDouble(idr2["qty"]);
                                string location = idr2["location"].ToString();

                                //审核3A2表
                                sql = @"
 Update WMS003A2 set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "'where storage_plan='" + Plan + @"'";
                                DB.ExecuteNonQueryOffline(sql);

                                sql = @"
INSERT INTO WMS004A1(storage_doc,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,qty,location,createby,createdate,createtime)
values
('" + storage_doc + "','" + serial_number + @"','" + material_no + @"','" + material_name + @"','" + material_var + "','" + material_specifications + @"','" + suppliers_lot + @"','" + lot_barcode + @"','" + Math.Round(qty, 2) + @"','" + location + @"','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')
";

                                DB.ExecuteNonQueryOffline(sql);

                                #region 审核3M表
                                sql = @"
select 
case 
when SUM(qty) >= sum(qty_plan) then 'true'
else 'false'
end AS temp 
 from WMS003A1 where storage_plan='" + Plan + "' group by material_no";

                                bool istrue = false;
                                DataTable dt = DB.GetDataTable(sql);
                                //判断物料的计划是否都满足于计划，每个物料都有一个满足状态，全为true时通过
                                foreach (DataRow item in dt.Rows)
                                {
                                    if (item.ItemArray[0].ToString().Contains("false"))
                                    {
                                        istrue = false;
                                        break;
                                    }
                                    else
                                    {
                                        istrue = true;
                                    }
                                }

                                //如果通过，则更新03m表的状态为已审核
                                if (istrue)
                                {
                                    sql = @"
 Update WMS003M set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' where storage_plan='" + Plan + @"'";
                                    DB.ExecuteNonQueryOffline(sql);

                                }

                            }
                            #endregion

                            #region 新增条码状态
                            for (int M = 0; M < tmdt.Rows.Count; M++)
                            {
                                switch (tmdt.Rows[M]["BarCodeType"].ToString().Trim())
                                {
                                    case "1":
                                        string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[M]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                        DataTable dt3 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE001M where products_barcode='" + tmdt.Rows[M]["BarCode"].ToString().Trim() + "'");

                                        sql = @"INSERT INTO CODE001A3 (products_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES ('" + tmdt.Rows[M]["BarCode"].ToString().Trim() + "','7','" + storage_doc + "','','5','" + warehouse + "','" + tmdt.Rows[M]["Location"].ToString().Trim() + "','" + tmdt.Rows[M]["Qty"].ToString().Trim() + "','" + dt3.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                        DB.ExecuteNonQueryOffline(sql);
                                        break;
                                    case "2":
                                        string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[M]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                        DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + tmdt.Rows[M]["BarCode"].ToString().Trim() + "'");

                                        sql = "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES('" + tmdt.Rows[M]["BarCode"].ToString().Trim() + "','7','" + storage_doc + "','','5','" + warehouse1 + "','" + tmdt.Rows[M]["Location"].ToString().Trim() + "','" + tmdt.Rows[M]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                        DB.ExecuteNonQueryOffline(sql);
                                        break;
                                    case "3":
                                        string warehouse2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt.Rows[M]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                        DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE003M where packing_barcode='" + tmdt.Rows[M]["BarCode"].ToString().Trim() + "'");

                                        sql = @"INSERT INTO CODE003A3 (packing_barcode	,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES ('" + tmdt.Rows[M]["BarCode"].ToString().Trim() + "','7','" + storage_doc + "','','5','" + warehouse2 + "','" + tmdt.Rows[M]["Location"].ToString().Trim() + "','" + tmdt.Rows[M]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                        DB.ExecuteNonQueryOffline(sql);
                                        break;
                                    default: break;
                                }

                            }
                            #endregion

                            IsOk = true;
                            IsSuccess = true;

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

                                ret = WMS004.Audit(s);

                                return ret;
                            }
                        }
                    }

                    RetData += "<" + Plan + @">";
                    RetData += "<IsOk>" + IsOk + @"</IsOk>";
                    RetData += "<ErrMsg>" + ErrMsg + @"</ErrMsg>";
                    RetData += "</" + Plan + @">";

                    #endregion

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

        private static string DoWork_hzrk2(DataTable tmdt1, DataTable tmdt2, GDSJ_Framework.DBHelper.DataBase DB, string UserCode, string plan)
        {
            #region 逻辑
            string sql = "";
            string RetData = "";
            string StorageType = "";

            //DataTable tmdt_x = DoWork_hzrk(tmdt1, tmdt2, plan);

            sql = @"
SELECT count(*)
FROM WMS003M
WHERE storage_plan in ('" + plan + @"')
";

            DataTable dt = DB.GetDataTable(sql);

            if (dt.Rows.Count != 0)
            {
                Dictionary<string, object> p = new Dictionary<string, object>();

                DataTable dt4;
                string serial_number = "0";
                string createby = UserCode;
                string createdate = DateTime.Now.ToString("yyyy-MM-dd");
                string createtime = DateTime.Now.ToString("HH:mm:ss");
                string modifyby = UserCode;
                string modifydate = DateTime.Now.ToString("yyyy-MM-dd");
                string modifytime = DateTime.Now.ToString("HH:mm:ss");

                if (tmdt2.Rows[0][5].ToString() == "null")//料号
                {
                    int col = 0;
                    string Material_no = "";
                    foreach (DataRow dr_tmdt1 in tmdt1.Rows)
                    {
                        if (dr_tmdt1.ItemArray[1].ToString() != Material_no)
                        {
                            Material_no = dr_tmdt1.ItemArray[1].ToString();
                            col = 0;
                        }
                        else
                        {
                            col++;
                        }
                        DataRow[] dr_tmdt4 = tmdt2.Select("Material_no='" + Material_no + "'");
                        sql = @"select top 1 material_no,material_name,material_specifications from BASE007M where material_no = '" + dr_tmdt1.ItemArray[1].ToString() + "'";
                        DataTable Base007m = DB.GetDataTable(sql);//物料信息
                                                                  //sql = @"SELECT warehouse FROM BASE011M where location_no = '" + kvp.Key.Split('*')[1].Trim() + "'";
                                                                  //DataTable Base011m = DB.GetDataTable(sql);//库位信息
                        sql = @"INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,location,status,createby,createdate,createtime) 
                    VALUES (@storage_plan,@serial_number,@material_no,@material_name,@material_specifications,@suppliers_lot,@lot_barcode,@qty_plan,@location_plan,@qty,@location,@status,@createby,@createdate,@createtime) ";
                        p.Clear();
                        p.Add("@storage_plan", plan);
                        p.Add("@serial_number", serial_number);
                        p.Add("@material_no", Base007m.Rows[0][0].ToString());
                        p.Add("@material_name", Base007m.Rows[0][1].ToString());
                        p.Add("@material_specifications", Base007m.Rows[0][2].ToString());
                        p.Add("@suppliers_lot", "");
                        p.Add("@lot_barcode", "");
                        p.Add("@qty_plan", Math.Round(Convert.ToDouble(dr_tmdt1.ItemArray[4]), 2));
                        p.Add("@location_plan", dr_tmdt1.ItemArray[3].ToString());
                        p.Add("@qty", Math.Round(Convert.ToDouble(dr_tmdt1.ItemArray[2]), 2));
                        p.Add("@location", dr_tmdt4[col].ItemArray[4].ToString());//实际库位？
                        p.Add("@status", "1");
                        p.Add("@createby", createby);
                        p.Add("@createdate", createdate);
                        p.Add("@createtime", createtime);
                        p.Add("@modifyby", modifyby);
                        p.Add("@modifydate", modifydate);
                        p.Add("@modifytime", modifytime);

                        sql += " UPDATE WMS003A1 SET qty=qty+@qty,modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where storage_plan=@storage_plan and material_no=@material_no";
                        DB.ExecuteNonQueryOffline(sql, p);

                        //sql = " UPDATE WMS003A2 set qty_plan= (select qty_plan-qty from WMS003A1 where storage_plan=@storage_plan and material_no=@material_no),modifyby=@modifyby,modifydate=@modifydate,modifytime=@modifytime where storage_plan=@storage_plan and material_no=@material_no  ";
                        //DB.ExecuteNonQueryOffline(sql, p);

                    }

                }
                else//批号、箱号、单品
                {
                    Dictionary<string, double> Nums = new Dictionary<string, double>();
                    Dictionary<string, double> Nums1 = new Dictionary<string, double>();
                    #region 根据物料和数量汇总，根据物料加库位汇总
                    for (int i = 0; i < tmdt2.Rows.Count; i++)
                    {
                        if (Nums.ContainsKey(tmdt2.Rows[i]["Material_no"].ToString()))//按物料汇总，当传入的物料和库位都相同的时候，把他看作一条入库明细
                        {
                            Nums[tmdt2.Rows[i]["Material_no"].ToString()] += Convert.ToDouble(tmdt2.Rows[i]["Qty"].ToString());
                        }
                        else
                        {
                            Nums.Add(tmdt2.Rows[i]["Material_no"].ToString(), Convert.ToDouble(tmdt2.Rows[i]["Qty"].ToString()));
                        }
                        if (Nums1.ContainsKey(tmdt2.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt2.Rows[i]["Location"].ToString().Trim()))//按物料和库位进行汇总，保证传入的物料加库位是唯一的
                        {
                            Nums1[tmdt2.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt2.Rows[i]["Location"].ToString().Trim()] += Convert.ToDouble(tmdt2.Rows[i]["Qty"].ToString().Trim());
                        }
                        else
                        {
                            Nums1.Add(tmdt2.Rows[i]["Material_no"].ToString().Trim() + "*" + tmdt2.Rows[i]["Location"].ToString().Trim(), Convert.ToDouble(tmdt2.Rows[i]["Qty"].ToString().Trim()));
                        }
                    }
                    #endregion
                    foreach (KeyValuePair<string, double> kvp in Nums)
                    {
                        double qty1 = 0;//汇总计划入库数量
                        double qty2 = 0;//汇总实际入库数量  可入库数量=qty1-qty2

                        try
                        {
                            sql = "SELECT material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,location_plan FROM WMS003A1 WHERE material_no='" + kvp.Key.Trim() + "'and storage_plan='" + plan + "'";
                            DataTable dt1 = DB.GetDataTable(sql);
                            //根据扫描的条码数据进行汇总（按物料）入库数量，获取WMS003A1的资料
                            if (dt1.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    qty1 += double.Parse(dt1.Rows[i]["qty_plan"].ToString());//计划入库
                                    qty2 += double.Parse(dt1.Rows[i]["qty"].ToString());//已入库
                                }
                            }

                            sql = "SELECT material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,location_plan FROM WMS003A1 WHERE material_no='" + kvp.Key.Trim() + "' and storage_plan='" + plan + "'";
                            DataTable dt2 = DB.GetDataTable(sql);

                            if (kvp.Value <= (qty1 - qty2))
                            {
                                // 3.根据扫描条码数据进行汇总（按物料、入库库位），把数据插入到WMS003A2中去，审核状态为1。 
                                //更新WMS003A1的实际入库数量WMS003A1.qty
                                //更新WMS003A2的qty_plan=WMS003A1.qty_plan-WMS003A1.qty
                                foreach (KeyValuePair<string, double> kvp1 in Nums1)
                                {
                                    if (kvp.Key.Trim() == kvp1.Key.Split('*')[0].Trim())
                                    {
                                        //WMS003A2表是实际入库，先插入一条数据，记录实际入库数量，WMS003A1是计划入库，再将实际入库数量记录更新A1表，将A1表的计划数量对比实际入库数量得出差，将差更新到A2表就是还需入库的数量
                                        sql = "INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,location,status,createby,createdate,createtime) VALUES ('" + plan + "','" + serial_number + "','" + kvp.Key.Trim() + "','" + dt2.Rows[0]["material_name"].ToString() + "','" + dt2.Rows[0]["material_specifications"].ToString() + "','" + dt2.Rows[0]["suppliers_lot"].ToString() + "','" + dt2.Rows[0]["lot_barcode"].ToString() + "','" + dt2.Rows[0]["qty_plan"].ToString() + "','" + dt2.Rows[0]["location_plan"].ToString() + "','" + kvp1.Value + "','" + kvp1.Key.Split('*')[1] + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ") + "','" + DateTime.Now.ToString("HH:mm:ss") + "') ";
                                        sql += " UPDATE WMS003A1 SET qty=qty+" + kvp1.Value + " where storage_plan='" + plan + "' and material_no='" + kvp.Key.Trim() + "'";
                                        // sql += " UPDATE WMS003A2 set qty_plan= (select qty_plan-qty from WMS003A1 where storage_plan='" + plan + "' and material_no='" + kvp.Key.Trim() + "') where storage_plan='" + plan + "' and material_no='" + kvp.Key.Trim() + "' and serial_number='" + serial_number + "' ";
                                        DB.ExecuteNonQueryOffline(sql);
                                        RetData += kvp1.Key.Split('*')[0].Trim() + ",";

                                    }
                                }
                            }
                            else
                            {
                                //StorageType = "0";
                                StorageType = DB.GetDataTable("SELECT isover_in FROM BASE007M WHERE material_no='" + kvp.Key.Trim() + "'").Rows[0][0].ToString().Trim();
                                if (StorageType == "FALSE")//如果是不允许超入
                                {
                                    // RetData = "入库数量大于计划入库数量[" + kvp.Key.Trim() + "," + dt1.Rows[0]["material_name"].ToString() + "," + dt1.Rows[0]["material_specifications"].ToString() + "," + dt1.Rows[0]["qty_plan"].ToString() + "," + dt1.Rows[0]["qty"].ToString() + "," + kvp.Value + "]";
                                }
                                else
                                {
                                    // 3.根据扫描条码数据进行汇总（按物料、入库库位），把数据插入到WMS003A2中去，审核状态为1。
                                    //更新WMS003A1的实际入库数量WMS003A1.qty
                                    //更新WMS003A2的qty_plan=WMS003A1.qty_plan-WMS003A1.qty
                                    foreach (KeyValuePair<string, double> kvp1 in Nums1)
                                    {
                                        if (kvp.Key.Trim() == kvp1.Key.Split('*')[0].Trim())
                                        {
                                            sql = "INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,location,status,createby,createdate,createtime) VALUES ('" + plan + "','" + serial_number + "','" + kvp.Key.Trim() + "','" + dt2.Rows[0]["material_name"].ToString() + "','" + dt2.Rows[0]["material_specifications"].ToString() + "','" + dt2.Rows[0]["suppliers_lot"].ToString() + "','" + dt2.Rows[0]["lot_barcode"].ToString() + "','" + dt2.Rows[0]["qty_plan"].ToString() + "','" + dt2.Rows[0]["location_plan"].ToString() + "','" + kvp1.Value + "','" + kvp1.Key.Split('*')[1] + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')";
                                            sql += " UPDATE WMS003A1 SET qty=qty+'" + kvp1.Value + "' where storage_plan='" + plan + "' and material_no='" + kvp.Key.Trim() + "'";
                                            // sql += " UPDATE WMS003A2 set qty_plan= (select qty_plan-qty from WMS003A1 where storage_plan='" + plan + "' and material_no='" + kvp.Key.Trim() + "') where storage_plan='" + plan + "' and material_no='" + kvp.Key.Trim() + "'";
                                            DB.ExecuteNonQueryOffline(sql);
                                            RetData += kvp1.Key.Split('*')[0].Trim() + ",";
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            RetData += ex.ToString();
                        }

                    }


                    //4.把扫描条码，根据条码的类型，分别插入到 CODE001A1(单品条码操作记录)、CODE001A3(单品条码单据记录)、CODE002A1(批号条码操作记录)、CODE002A3(批号条码单据操作记录)、CODE003A1(包装条码操作记录)、CODE003A3(包装条码单据记录)中。
                    //   0是品号 Base007m，1是单品code001m，2是批号code002m，3是包装code003m
                    for (int i = 0; i < tmdt2.Rows.Count; i++)
                    {
                        switch (tmdt2.Rows[i]["BarCodeType"].ToString().Trim())
                        {
                            //单品
                            case "1":
                                dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE001A3 where documents='" + plan + "'");
                                if (dt4.Rows[0][0].ToString() == "")
                                {
                                    serial_number = "001";
                                }
                                else
                                {
                                    serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                                }
                                string warehouse = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt2.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                DataTable dt3 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE001M where products_barcode='" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "'");
                                sql = @"INSERT INTO CODE001A1 (products_barcode,operation,warehouse,location,packing_barcode,lot_barcode,org,createby,createdate,createtime) VALUES ('" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "','6','" + warehouse + "','" + tmdt2.Rows[i]["Location"].ToString().Trim() + "','" + dt3.Rows[0]["packing_barcode"] + "','" + dt3.Rows[0]["lot_barcode"] + "','" + dt3.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                sql += @"INSERT INTO CODE001A3 (products_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES ('" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "','6','" + plan + "','" + serial_number + "','5','" + warehouse + "','" + tmdt2.Rows[i]["Location"].ToString().Trim() + "','" + tmdt2.Rows[i]["Qty"].ToString().Trim() + "','" + dt3.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                DB.ExecuteNonQueryOffline(sql);
                                break;
                            //批号
                            case "2":
                                dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE002A3 where documents='" + plan + "'");
                                if (dt4.Rows[0][0].ToString() == "")
                                {
                                    serial_number = "001";
                                }
                                else
                                {
                                    serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                                }
                                string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt2.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "'");
                                sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org,createby,createdate,createtime) VALUES ('" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "','6','" + warehouse1 + "','" + tmdt2.Rows[i]["Location"].ToString().Trim() + "','" + tmdt2.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                sql += "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES('" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "','6','" + plan + "','" + serial_number + "','5','" + warehouse1 + "','" + tmdt2.Rows[i]["Location"].ToString().Trim() + "','" + tmdt2.Rows[i]["Qty"].ToString().Trim() + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                DB.ExecuteNonQueryOffline(sql);
                                break;
                            //箱号
                            case "3":
                                dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE003A3 where documents='" + plan + "'");
                                if (dt4.Rows[0][0].ToString() == "")
                                {
                                    serial_number = "001";
                                }
                                else
                                {
                                    serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                                }
                                string warehouse2 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + tmdt2.Rows[i]["Location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                System.Data.DataTable dt32 = DB.GetDataTable("SELECT packing_barcode,lot_barcode,org FROM CODE003M where packing_barcode='" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "'");
                                sql = @"INSERT INTO CODE003A1 (packing_barcode,operation,warehouse,location,qty,lot_barcode,org,createby,createdate,createtime) VALUES ('" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "','6','" + warehouse2 + "','" + tmdt2.Rows[i]["Location"].ToString().Trim() + "','" + tmdt2.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["lot_barcode"] + "','" + dt32.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                sql += @"INSERT INTO CODE003A3 (packing_barcode	,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime) VALUES ('" + tmdt2.Rows[i]["BarCode"].ToString().Trim() + "','6','" + plan + "','" + serial_number + "','5','" + warehouse2 + "','" + tmdt2.Rows[i]["Location"].ToString().Trim() + "','" + tmdt2.Rows[i]["Qty"].ToString().Trim() + "','" + dt32.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')";
                                DB.ExecuteNonQueryOffline(sql);
                                break;
                            default: break;
                        }

                    }
                }

            }
            else
            {
                RetData = "入库计划不存在";
            }
            #endregion
            return RetData;
        }

        private static DataTable DoWork_hzrk(DataTable tmdt1, DataTable tmdt2, string plan)
        {
            int a = 0;
            foreach (DataRow dr_tmdt2 in tmdt2.Rows)//判断第二个table计划是否为空
            {
                if (dr_tmdt2.ItemArray[5].ToString().Contains("null"))
                {
                    a = 1;
                    break;
                }
            }
            if (a == 1)
            {
                //int col2 = 0;
                DataTable tmdt = tmdt2.Clone();
                DataRow[] dr_tmdt1 = tmdt1.Select("Plan='" + plan + "'");
                foreach (DataRow dr_tmdt11 in dr_tmdt1)
                {
                    decimal c = 0;

                    DataRow[] dr_tmdt2 = tmdt2.Select("Material_no='" + dr_tmdt11.ItemArray[1].ToString() + "' and Plan='null'");
                    //and ChildNumber = '" + Convert.ToDecimal(dr_tmdt11.ItemArray[4]) + "'
                    foreach (DataRow dr_tmdt22 in dr_tmdt2)
                    {
                        c = c + Convert.ToDecimal(dr_tmdt22.ItemArray[3]);
                        if (c < Convert.ToDecimal(dr_tmdt11.ItemArray[4]))
                        {
                            DataRow dr_tmdt3 = tmdt.NewRow();
                            dr_tmdt3[0] = dr_tmdt22.ItemArray[0].ToString();
                            dr_tmdt3[1] = dr_tmdt22.ItemArray[1].ToString();
                            dr_tmdt3[2] = dr_tmdt22.ItemArray[2].ToString();
                            dr_tmdt3[3] = Convert.ToDecimal(dr_tmdt22.ItemArray[3]);
                            dr_tmdt3[4] = dr_tmdt22.ItemArray[4].ToString();
                            dr_tmdt3[5] = plan;
                            tmdt.Rows.Add(dr_tmdt3);
                            //col2++;
                        }
                        else if (c == Convert.ToDecimal(dr_tmdt11.ItemArray[4]))
                        {
                            DataRow dr_tmdt3 = tmdt.NewRow();
                            dr_tmdt3[0] = dr_tmdt22.ItemArray[0].ToString();
                            dr_tmdt3[1] = dr_tmdt22.ItemArray[1].ToString();
                            dr_tmdt3[2] = dr_tmdt22.ItemArray[2].ToString();
                            dr_tmdt3[3] = Convert.ToDecimal(dr_tmdt22.ItemArray[3]);
                            dr_tmdt3[4] = dr_tmdt22.ItemArray[4].ToString();
                            dr_tmdt3[5] = plan;
                            tmdt.Rows.Add(dr_tmdt3);
                            //col2++;
                            break;
                        }
                        else if (c > Convert.ToDecimal(dr_tmdt11.ItemArray[4]))
                        {
                            DataRow dr_tmdt3 = tmdt.NewRow();
                            dr_tmdt3[0] = dr_tmdt22.ItemArray[0].ToString();
                            dr_tmdt3[1] = dr_tmdt22.ItemArray[1].ToString();
                            dr_tmdt3[2] = dr_tmdt22.ItemArray[2].ToString();
                            dr_tmdt3[3] = Convert.ToDecimal(dr_tmdt22.ItemArray[3]);
                            dr_tmdt3[4] = dr_tmdt22.ItemArray[4].ToString();
                            dr_tmdt3[5] = plan;
                            tmdt.Rows.Add(dr_tmdt3);
                            //int col2 = 0;
                            //for (int i = 0; i < tmdt2.Rows.Count; i++)
                            //{
                            //    if  (tmdt2.Rows[i][0].ToString() == dr_tmdt22.ItemArray[0].ToString() && tmdt2.Rows[i][1].ToString() == dr_tmdt22.ItemArray[1].ToString() && tmdt2.Rows[i][2].ToString() == dr_tmdt22.ItemArray[2].ToString()
                            //     && tmdt2.Rows[i][3].ToString() == dr_tmdt22.ItemArray[3].ToString() && tmdt2.Rows[i][4].ToString() == dr_tmdt22.ItemArray[4].ToString())
                            //        {
                            //        col2 = i;
                            //        break;
                            //    }
                            //}
                            //tmdt2.Rows[col2][3] = Convert.ToDecimal(tmdt2.Rows[col2][3]) - (Convert.ToDecimal(dr_tmdt11.ItemArray[4]));
                            //col2++;
                            break;
                        }
                    }

                }
                DataRow[] dr_tmdt = tmdt.Select("Plan='" + plan + "'");
                DataTable tmdt_3 = tmdt.Clone();
                foreach (DataRow row in dr_tmdt)
                {
                    tmdt_3.ImportRow(row); // 将DataRow添加到DataTable中
                }

                return tmdt_3;
            }
            else
            {
                DataRow[] dr_tmdt = tmdt2.Select("Plan='" + plan + "'");
                DataTable tmdt_3 = tmdt2.Clone();
                foreach (DataRow row in dr_tmdt)
                {
                    tmdt_3.ImportRow(row); // 将DataRow添加到DataTable中
                }
                return tmdt_3;
            }

        }

        public static string Audit(object OBJ, DataTable tmdt, string Plan)
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


                DllName = "SJEMS_WBAPI";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = "SJEMS_WBAPI.WMS003";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = "Audit";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
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
                string storage_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<storage_plan>", "</storage_plan>");
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");
                string DataTableBarCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCode>", "</BarCode>");
                //System.Data.DataTable tmdt = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTableBarCode);
                #endregion


                if (false)
                //if (!User.DoSureCheck(UserCode,"WMS003"))
                {
                    RetData = "该用户没有权限进行审核";
                }
                else
                {
                    bool IsOk = false;
                    string ErrMsg = string.Empty;

                   

                    //2.查询WMS003A2看是否有没有审核过的数据，如果没有，报错：该计划没有新的入库资料，不能进行审核。

                    string sql = @"
SELECT COUNT(*) FROM WMS003A2 WHERE storage_plan=@storage_plan
";
                    Dictionary<string, object> p1 = new Dictionary<string, object>();
                    p1.Add("@storage_plan", Plan);
                    string rowNumber = DB.GetString(sql, p1);

                    if (rowNumber == "0")  //报错
                    {
                        IsOk = false;
                        ErrMsg = "该计划没有新的入库资料，不能进行审核";
                    }
                    else //未审核
                    {
                        string storage_doc = string.Empty;

                        #region 根据WMS003M,WMS003A2数据产生WMS004M和WMS004A1数据，并更新03A2表的状态为2已经审核
                        sql = @"SELECT storage_plan,storage_type,front_document_type,front_document,status,dosureby,dosuredatetime,auditby,auditdatetime from WMS003M where storage_plan=@storage_plan  ";
                        Dictionary<string, object> p2 = new Dictionary<string, object>();
                        p2.Add("@storage_plan", Plan);
                        System.Data.IDataReader idr = DB.GetDataTableReader(sql, p2);

                        #region 产生入库单号
                        sql = @"
SELECT ISNULL(MAX(storage_doc),'" + DateTime.Now.ToString("yyyyMMdd") + @"0000')
FROM WMS004M(NOLOCK)
WHERE storage_doc LIKE '" + DateTime.Now.ToString("yyyyMMdd") + @"%'
";

                        IDataReader dr = DB.GetDataTableReader(sql);
                        if (dr.Read())
                        {
                            storage_doc = DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(dr[0].ToString().Replace(DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");
                        }
                        #endregion
                        while (idr.Read())
                        {
                            // string storage_doc = idr["storage_plan"].ToString();
                            string storage_type = idr["storage_type"].ToString();
                            string front_document_type = idr["front_document_type"].ToString();
                            string front_document = idr["front_document"].ToString();
                            //string status = idr["status"].ToString();
                            string dosureby = idr["dosureby"].ToString();
                            string dosuredatetime = idr["dosuredatetime"].ToString();
                            string auditby = idr["auditby"].ToString();
                            string auditdatetime = idr["auditdatetime"].ToString();

                            sql = @"
INSERT INTO WMS004M(storage_doc,storage_type,front_document_type,front_document,status,createby,createdate,createtime)
values
('" + storage_doc + "','" + storage_type + @"','6','" + storage_plan + @"','1','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"')
";

                            DB.ExecuteNonQueryOffline(sql);

                            sql = @"
SELECT storage_plan,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,qty,location,UDF01,UDF03,UDF04,UDF06,UDF07 from WMS003A2 WHERE storage_plan=@storage_plan  AND status='1'
";

                            Dictionary<string, object> p3 = new Dictionary<string, object>();
                            p3.Add("@storage_plan", Plan);
                            System.Data.IDataReader idr2 = DB.GetDataTableReader(sql, p3);
                            while (idr2.Read())
                            {
                                string serial_number = idr2["serial_number"].ToString();
                                string material_no = idr2["material_no"].ToString();
                                string material_name = idr2["material_name"].ToString();
                                string material_var = idr2["material_var"] + "";
                                string material_specifications = idr2["material_specifications"].ToString();
                                string suppliers_lot = idr2["suppliers_lot"].ToString();
                                string lot_barcode = idr2["lot_barcode"].ToString();
                                double qty = Math.Round(Convert.ToDouble(idr2["qty"]), 2);
                                string location = idr2["location"].ToString();
                                string size = idr2["UDF01"].ToString();
                                //左右脚计划入库数量
                                string UDF03 = idr2["UDF03"].ToString().Equals("") ? "0" : idr2["UDF03"] + "";
                                string UDF04 = idr2["UDF04"].ToString().Equals("") ? "0" : idr2["UDF04"] + "";


                                string UDF06 = idr2["UDF06"].ToString().Equals("")?"0":idr2["UDF06"]+"";
                                string UDF07 = idr2["UDF07"].ToString().Equals("")?"0":idr2["UDF07"]+"";


                                //审核3A2表
                                sql = @"
 Update WMS003A2 set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "'where storage_plan='" + storage_plan + @"'";
                                DB.ExecuteNonQueryOffline(sql);

                                sql = @"
INSERT INTO WMS004A1(storage_doc,serial_number,material_no,material_name,material_var,material_specifications,suppliers_lot,lot_barcode,qty,location,createby,createdate,createtime,UDF01,UDF03,UDF04,UDF06,UDF07)
values
('" + storage_doc + "','" + serial_number + @"','" + material_no + @"','" + material_name + @"','" + material_var + "','" + material_specifications + @"','" + suppliers_lot + @"','" + lot_barcode + @"','" + qty + @"','" + location + @"','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','"+size+@"','"+UDF03+"','"+UDF04+@",'"+UDF06+"','"+UDF07+@"'')
";

                                DB.ExecuteNonQueryOffline(sql);

                                #region 审核3M表

                                if (size.Equals("NO SIZE") || size.Equals(""))
                                {
                                    sql = @"
select 
case 
when SUM(qty) >= sum(qty_plan) then 'true'
else 'false'
end AS temp 
 from WMS003A1 where storage_plan='" + storage_plan + "' group by material_no";
                                }
                                else
                                {
                                    sql = @"
select 
case 
when SUM(cast(UDF06 as decimal)) >= sum(cast(UDF03 as decimal)) and SUM(cast(UDF07 as decimal))>=SUM(cast(UDF04 as decimal)) then 'true'
else 'false'
end AS temp 
 from WMS003A1 where storage_plan='" + storage_plan + "' group by material_no";
                                }


                                bool istrue = false;
                                DataTable dt = DB.GetDataTable(sql);
                                //判断物料的计划是否都满足于计划，每个物料都有一个满足状态，全为true时通过
                                foreach (DataRow item in dt.Rows)
                                {
                                    if (item.ItemArray[0].ToString().Contains("false"))
                                    {
                                        istrue = false;
                                        break;
                                    }
                                    else
                                    {
                                        istrue = true;
                                    }
                                }

                                //如果通过，则更新03m表的状态为已审核
                                if (istrue)
                                {
                                    sql = @"
 Update WMS003M set status='2', auditby='" + auditby + "',auditdatetime='" + auditdatetime + "',modifyby='" + UserCode + "',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd ") + "',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + "' where storage_plan='" + storage_plan + @"'";
                                    DB.ExecuteNonQueryOffline(sql);

                                }

                            }
                            #endregion

                            #region 新增条码状态
                            for (int M = 0; M < tmdt.Rows.Count; M++)
                            {
                                DataRow dtCode = tmdt.Rows[M];
                                string barcode = dtCode["code"].ToString();

                                string size = dtCode["size"].ToString();

                                string strlqty = dtCode["UDF03"] + "";
                                string strrqty = (dtCode["UDF04"] + "").Equals("") ? "0" : dtCode["UDF04"] + "";
                                //左右脚收料数量
                                double LQTY = double.Parse(strlqty);
                                double RQTY = double.Parse(strrqty);

                                string lot_barcode;

                                lot_barcode = DB.GetString("select lot_barcode FROM CODE002M(NOLOCK) WHERE UDF02='" + barcode + @"' and UDF01='" + size + @"'");


                                string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + dtCode["location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                        System.Data.DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + lot_barcode + "'");

                                        sql = "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime,UDF01,UDF03,UDF04) VALUES('" + lot_barcode + "','7','" + storage_doc + "','','5','" + warehouse1 + "','" + dtCode["location"].ToString().Trim() + "','" + (LQTY+RQTY) + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','"+size+@"','"+LQTY+"','"+RQTY+"')";
                                        DB.ExecuteNonQueryOffline(sql);
                                      
                                

                            }
                            #endregion

                            IsOk = true;
                            IsSuccess = true;

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

                                ret = WMS004.Audit(s);

                                return ret;
                            }
                        }
                    }

                    RetData += "<" + storage_plan + @">";
                    RetData += "<IsOk>" + IsOk + @"</IsOk>";
                    RetData += "<ErrMsg>" + ErrMsg + @"</ErrMsg>";
                    RetData += "</" + storage_plan + @">";

                    #endregion

                }

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



        public static string WbDoWork(object OBJ)
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


                DllName = "SJEMS_WBAPI";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = "SJEMS_WBAPI.WMS003";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = "WbDoWork";// GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);
                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                #region 接口参数
                string storage_plan = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<storage_plan>", "</storage_plan>");//入库计划
                string UserCode = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<UserCode>", "</UserCode>");//操作人（新增、修改、审核）
                string BarCodes = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<BarCodes>", "</BarCodes>");
                string planDetails = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<PlanDetails>", "</PlanDetails>");
                string hzrk = "";//GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<collectInStock>", "</collectInStock>");//汇总入库
                //
                DataTable tmdt_temp = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(planDetails);//计划入库实际明细
                //条码明细
                DataTable codeTable = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(BarCodes);

                DataTable tmdt1;//汇总出库实际明细1
                DataTable tmdt2;//汇总出库实际明细2
                string where = string.Empty;
                #endregion

                if (hzrk != "")//汇总入库
                {
                    string DataTable1 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Collect>", "</Collect>");
                    tmdt1 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable1);

                    DataView dv = new DataView(tmdt1); //虚拟视图
                    DataTable dt2 = dv.ToTable(true, "Plan");
                    foreach (DataRow dr_dt2 in dt2.Rows)
                    {
                        storage_plan += dr_dt2.ItemArray[0].ToString() + ",";
                    }

                    #region 汇总入库截取计划数组
                    string[] b;
                    if (storage_plan.Contains(","))
                    {
                        string[] s = new string[1];
                        s[0] = ",";
                        b = storage_plan.Split(s, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        b = new string[1];
                        b[0] = storage_plan;
                    }

                    #endregion

                    string DataTable2 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(Data, "<Operation>", "</Operation>");
                    tmdt2 = GDSJ_Framework.Common.StringHelper.GetDataTableFromXML(DataTable2);

                    for (int j = 0; j < b.Length; j++)//多个入库计划
                    {
                        DataRow[] dr_tmdt = tmdt1.Select("Plan='" + b[j].ToString() + "'");
                        DataTable tmdt_3 = tmdt1.Clone();
                        DataTable tmdt_4 = tmdt2.Clone();
                        string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS003TOWMS004'");//是否自动审核

                        foreach (DataRow row in dr_tmdt)
                        {
                            tmdt_3.ImportRow(row); // 将DataRow添加到DataTable中
                        }

                        if (!string.IsNullOrEmpty(tmdt2.Rows[0][5].ToString()))//单、批、箱
                        {
                            DataRow[] dr_tmdt2 = tmdt2.Select("Plan='" + b[j].ToString() + "'");
                            foreach (DataRow row in dr_tmdt2)
                            {
                                tmdt_4.ImportRow(row); // 将DataRow添加到DataTable中
                            }

                            RetData = DoWork_hzrk2(tmdt_3, tmdt_4, DB, UserCode, b[j].ToString());
                            if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                            {
                                ret += Audit_hzrk(OBJ, tmdt_4, b[j].ToString());
                            }
                        }
                        else//料
                        {
                            RetData = DoWork_hzrk2(tmdt_3, tmdt2, DB, UserCode, b[j].ToString());
                            if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                            {
                                ret += Audit_hzrk(OBJ, tmdt2, b[j].ToString());
                            }
                        }

                    }
                    return ret;
                }
                else
                {
                    #region 截取计划字段
                    if (storage_plan.Contains(","))
                    {
                        string[] tmpStr = storage_plan.Split(',');
                        foreach (string s in tmpStr)
                        {
                            if (!string.IsNullOrEmpty(where))
                            {
                                where += ",";
                            }

                            where += "'" + s + @"'";
                        }
                    }
                    else
                    {
                        where = "'" + storage_plan + @"'";
                    }
                    #endregion

                    #region 计划入库截取计划数组
                    string[] a;
                    if (storage_plan.Contains(","))
                    {
                        string[] s = new string[1];
                        s[0] = ",";
                        a = storage_plan.Split(s, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        a = new string[1];
                        a[0] = storage_plan;
                    }
                    #endregion


                    for (int j = 0; j < a.Length; j++)//多个入库计划
                    {
                        string sql = string.Empty;

                        if (string.IsNullOrEmpty(RetData))
                        {
                            #region 逻辑
                            sql = @"SELECT storage_plan FROM WMS003M WHERE storage_plan in (" + where + @")";
                            DataTable dt = DB.GetDataTable(sql);
                            //2.根据扫描的条码数据进行汇总（按物料）入库数量，获取WMS003A1的资料，汇总WMS003A1的可入库数量（按物料，汇总计划入库数量- 汇总实际入库数量），
                            //如果扫描条码的物料入库数量大于对应物料的可入库数量，获取物料资料的
                            //StorageType,如果是不允许超入的话，报错：入库数量大于计划入库数量（物料代号、物料名称、物料规格、计划数量、已入库数量、当前入库数量）

                            /**
                             * 根据提交上来的条码数据更新WMS003A1的实际入库数量 
                             * UDF01：size 
                             * UDF02:size序号
                             * UDF03:左脚计划领料数量
                             * UDF04:右脚计划领料数量
                             * UDF06:左脚实际领料数量
                             * UDF07:右脚实际领料数量
                             * qty: 当物料是中性物料（不分size）则更新这个字段，否则不更新
                             * 
                             **/
                            if (dt.Rows.Count>0)
                            {

                                //标记是否允许超入
                                string isOverIn = string.Empty;
                                //序列号
                                string serial_number = string.Empty;

                                //计划明细Count
                                int psize = tmdt_temp.Rows.Count;

                                //
                                StringBuilder sqlBuilder = new StringBuilder("");

                                for (int k=0; k<psize; k++){
                                    DataRow tempdt = tmdt_temp.Rows[k];
                                    //标记是否是中性物料
                                    Boolean isNeuter = false;
                                    sql = "SELECT serial_number,material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,location_plan,UDF01,UDF02,UDF03,UDF04,UDF06,UDF07 FROM WMS003A1 WHERE material_no='" + tempdt["material_no"] + "' and storage_plan='" + storage_plan + "'";
                                    if (tempdt["UDF01"].Equals("") || tempdt["UDF01"].Equals("NO SIZE"))
                                    {
                                        isNeuter = true;
                                        sql = "SELECT serial_number,material_name,material_specifications,qty_plan,qty,suppliers_lot,lot_barcode,location_plan,UDF01,UDF02,UDF03,UDF04,UDF06,UDF07 " +
                                            "FROM WMS003A1 WHERE material_no='" + tempdt["material_no"] + "' and='"+tempdt["UDF01"]+"' and='"+tempdt["UDF02"]+"' and storage_plan='" + storage_plan + "'";
                                    }

                                    DataTable dt1 = DB.GetDataTable(sql);


                                    isOverIn = DB.GetDataTable("SELECT isover_in FROM BASE007M WHERE material_no='" + tempdt["material_no"] + "'").Rows[0][0].ToString().Trim();

                                    if (dt1.Rows.Count>0)
                                    {
                                        DataRow dt1Row = dt1.Rows[0];

                                        #region 产生序号
                                        serial_number = DB.GetString("SELECT max(CONVERT(int,serial_number)) FROM WMS003A2 where storage_plan='" + storage_plan + "'");
                                        if (serial_number == "")
                                        {
                                            serial_number = "001";
                                        }
                                        else
                                        {
                                            serial_number = (int.Parse(serial_number) + 1).ToString("000");
                                        }
                                        #endregion

                                        #region

                                        if (isNeuter)
                                        {
                                            #region 中性物料入库逻辑
                                            //计划数量
                                            double qty_plan = double.Parse(dt1Row["qty_plan"].ToString());
                                            //实际已入数量
                                            double qty = double.Parse(dt1Row["qty"].ToString());
                                            //PDA 扫描提交数量
                                            double qtyScan = double.Parse(tempdt["Qty"].ToString());
                                            if (qty+qtyScan<=qty_plan || isOverIn.Equals("TRUE"))
                                            {
                                                sqlBuilder.Append( @"UPDATE WMS003A1 SET qty=Convert(decimal(18,2),qty+" + qtyScan + @"),modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
                                                        WHERE storage_plan = '" + storage_plan + @"' 
                                                        and serial_number='" + dt1Row["serial_number"] + @"' and material_no='"+tempdt["material_no"]+@"' ");

                                               
                                                sqlBuilder.Append("INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,location_plan,qty,status,createby,createdate,createtime,UDF01) VALUES ('"
                                                        + storage_plan + "','" + serial_number + "','" + tempdt["material_no"] + "','" + dt1Row["material_name"] + "','" + dt1Row["material_specifications"] + "','" + dt1Row["suppliers_lot"]
                                                        + "','" + dt1Row["lot_barcode"] + "','" + qty_plan + "','" + dt1Row["location_plan"] + "','" + Math.Round(qtyScan, 2) + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ")
                                                        + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + tempdt["UDF01"] + @"') ");
                                               
                                               
                                            }    
                                            else
                                            {
                                                RetData = "物料：" + tempdt["material_no"] + "入库数量超过计划数量！";
                                            }


                                            #endregion
                                        }
                                        else
                                        {
                                            #region 非中性物料入库逻辑
                                            double lqty_plan = double.Parse(dt1Row["UDF03"].ToString());//左脚计划入库数量
                                            double rqty_plan = double.Parse(dt1Row["UDF04"].ToString());//右脚计划入库数量
                                            //左脚已入库数
                                            string strLqty = (dt1Row["UDF06"] + "").Equals("") ? "0" : dt1Row["UDF06"] + "";
                                            //右脚已入库数
                                            string strRQTY = (dt1Row["UDF07"] + "").Equals("") ? "0" : dt1Row["UDF07"] + "";
                                            double lqty = double.Parse(strLqty);
                                            double rqyt = double.Parse(strRQTY);

                                            //左右脚扫描数量
                                            double scanlQty = double.Parse(tempdt["UDF08"]+"");
                                            double scanrQty = double.Parse(tempdt["UDF09"]+"");

                                            if ((lqty_plan>=(lqty+scanlQty) && rqty_plan>=(rqyt+scanrQty)) || isOverIn.Equals("TRUE"))
                                            {
                                                sqlBuilder.Append(@"UPDATE WMS003A1 SET UDF06='" + (lqty + scanlQty) + @"',UDF07='"+ (rqyt + scanrQty) + "',modifyby='" + UserCode + @"',modifydate='" + DateTime.Now.ToString("yyyy-MM-dd") + @"',modifytime='" + DateTime.Now.ToString("HH:mm:ss") + @"'
                                                        WHERE storage_plan = '" + storage_plan + @"' 
                                                        and serial_number='" + dt1Row["serial_number"] + @"' and material_no='" + tempdt["material_no"] + @"'");

                                                sqlBuilder.Append("INSERT INTO WMS003A2 (storage_plan,serial_number,material_no,material_name,material_specifications,suppliers_lot,lot_barcode,qty_plan,qty,location_plan,status,createby,createdate,createtime,UDF01,UDF02,UDF03,UDF04,UDF06,UDF07) VALUES ('"
                                                        + storage_plan + "','" + serial_number + "','" + tempdt["material_no"] + "','" + dt1Row["material_name"] + "','" + dt1Row["material_specifications"] + "','" + dt1Row["suppliers_lot"]
                                                        + "','" + dt1Row["lot_barcode"] + "','" + dt1Row["qty_plan"] + "','"+(scanlQty+scanrQty) +"','" + dt1Row["location_plan"] + "','" + 1 + "','" + UserCode + "','" + DateTime.Now.ToString("yyyy-MM-dd ")
                                                        + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + tempdt["UDF01"] + @"','"+tempdt["UDF02"]+"','"+ dt1Row["UDF03"]+"','"+ dt1Row["UDF04"]+"','" + scanlQty + "','" + scanrQty + "') ");

                                            }
                                            else
                                            {
                                                RetData = "物料：" + tempdt["material_no"] + "入库数量超过计划数量！";
                                            }

                                            #endregion
                                        }
                                        #endregion
                                        DB.ExecuteNonQueryOffline(sqlBuilder.ToString());

                                    }
                                    else
                                    {
                                        RetData = "不存在物料："+tempdt["material_no"]+"的入库计划明细";
                                    }
                                }

                                //4.把扫描条码，根据条码的类型，分别插入到 CODE001A1(单品条码操作记录)、CODE001A3(单品条码单据记录)、CODE002A1(批号条码操作记录)、CODE002A3(批号条码单据操作记录)、CODE003A1(包装条码操作记录)、CODE003A3(包装条码单据记录)中。
                                //   0是品号 Base007m，1是单品code001m，2是批号code002m，3是包装code003m
                                for (int i=0; i< codeTable.Rows.Count; i++)
                                {
                                    #region 产生批号
                                    DataRow dtCode = codeTable.Rows[i];
                                    string barcode = dtCode["code"].ToString();
                                    string size = dtCode["UDF01"].ToString();
                                    string location = dtCode["location"].ToString();

                                    string lot_barcode;

                                    string material_no = barcode.Split(',')[4];
                                    string material_name = barcode.Split(',')[10];

                                    string strlqty = dtCode["UDF03"]+"";
                                    string strrqty = (dtCode["UDF04"] + "").Equals("")? "0":dtCode["UDF04"] + "";
                                    //左右脚收料数量
                                    double LQTY = double.Parse(strlqty);
                                    double RQTY = double.Parse(strrqty);



                                    lot_barcode = DB.GetString("select lot_barcode FROM CODE002M(NOLOCK) WHERE UDF02='" + barcode + @"' and UDF01='" + size + @"' ");

                                    if (string.IsNullOrEmpty(lot_barcode))
                                    {
                                        string material_specifications = DB.GetString("SELECT material_specifications FROM BASE007M WHERE material_no='"+ dtCode ["material_no"]+ "'");

                                        lot_barcode = DB.GetString("SELECT ISNULL(MAX(lot_barcode),'LOT" + DateTime.Now.ToString("yyyyMMdd") + "0000') FROM CODE002M WHERE lot_barcode like 'LOT" + DateTime.Now.ToString("yyyyMMdd") + @"%'");

                                        lot_barcode = "LOT" + DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(lot_barcode.Replace("LOT" + DateTime.Now.ToString("yyyyMMdd"), "")) + 1).ToString("0000");

                                        sql = @"
INSERT INTO CODE002M
(lot_barcode,material_no,location,material_name,material_specifications,qty,status,createby,createdate,createtime,UDF01,UDF02,UDF06,UDF07)
VALUES
('" + lot_barcode + @"','" + material_no + @"','" + location + @"','" + material_name + @"','"+material_specifications+"'," + (LQTY+RQTY) + @",'1','" + UserCode + @"','" 
+ DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + size + @"','" + barcode + @"','"+LQTY+@"','"+RQTY+"')";
                                        DB.ExecuteNonQueryOffline(sql);



                                    }
                                    else
                                    {

                                        string id = DB.GetString("select id FROM CODE002M(NOLOCK) WHERE lot_barcode='" + lot_barcode + @"' and UDF02='" + barcode + @"' and UDF01='" + size + @"' and location='" + location + @"'");


                                        if (!string.IsNullOrEmpty(id))
                                        {
                                            sql = @"
UPDATE CODE002M SET qty=qty+" + (LQTY + RQTY) + @",UDF06=UDF06+" + LQTY+@",UDF07=UDF07+"+RQTY+@"
WHERE id =" + id + @"
";
                                            DB.ExecuteNonQueryOffline(sql);
                                        }
                                        else
                                        {
                                            sql = @"
INSERT INTO CODE002M
(lot_barcode,material_no,location,material_name,material_specifications,qty,status,createby,createdate,createtime,UDF01,UDF02,UDF06,UDF07)
VALUES
('" + lot_barcode + @"','" + material_no + @"','" + location + @"','" + material_name + @"',''," + (LQTY+LQTY) + @",'1','" + UserCode + @"','" 
+ DateTime.Now.ToString("yyyy-MM-dd") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + size + @"','" + barcode + @"','" + LQTY + @"','" + RQTY + "')";
                                            DB.ExecuteNonQueryOffline(sql);


                                        }


                                    }
                                    //lot_barcode = DB.GetString("select lot_barcode FROM CODE002M(NOLOCK) WHERE UDF02='" + barcode + @"' and UDF01='" + size + @"' and location='" + dtCode["location"].ToString().Trim() + @"'");

                                    DataTable dt4 = DB.GetDataTable("SELECT max(CONVERT(int,serial_number)) FROM CODE002A3 where documents='" + storage_plan + "'");
                                    if (dt4.Rows[0][0].ToString() == "")
                                    {
                                        serial_number = "001";
                                    }
                                    else
                                    {
                                        serial_number = (int.Parse(dt4.Rows[0][0].ToString()) + 1).ToString("000");
                                    }
                                    string warehouse1 = DB.GetDataTable("SELECT warehouse FROM BASE011M where location_no='" + dtCode["location"].ToString().Trim() + "'").Rows[0][0].ToString();
                                    System.Data.DataTable dt31 = DB.GetDataTable("SELECT lot_barcode,org FROM CODE002M where lot_barcode='" + lot_barcode + "'");
                                    sql = @"INSERT INTO CODE002A1 (lot_barcode,operation,warehouse,location,qty,org,createby,createdate,createtime,UDF01,UDF06,UDF07) VALUES ('" + lot_barcode + "','6','" + warehouse1 + "','" + dtCode["location"].ToString().Trim() + "','" + (LQTY+RQTY) + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + size + @"','"+LQTY+"','"+RQTY+"')";
                                    sql += "INSERT INTO CODE002A3 (lot_barcode,documents_type,documents,serial_number,operation,warehouse_target,location_target,qty,org,createby,createdate,createtime,UDF01,UDF06,UDF07) VALUES('" + lot_barcode + "','6','" + storage_plan + "','" + serial_number + "','5','" + warehouse1 + "','" + dtCode["location"].ToString().Trim() + "','" + (LQTY+RQTY) + "','" + dt31.Rows[0]["org"] + "','" + UserCode + @"','" + DateTime.Now.ToString("yyyy-MM-dd ") + @"','" + DateTime.Now.ToString("HH:mm:ss") + @"','" + size + @"','"+LQTY+"','"+RQTY+"')";
                                    DB.ExecuteNonQueryOffline(sql);

                                    #endregion
                                }
                                string Auto = DB.GetString("SELECT parameters_value FROM SYS002M WHERE parameters_code='WMS003TOWMS004'");
                                if (Auto == "Auto" || string.IsNullOrEmpty(Auto))
                                {
                                    ret = Audit(OBJ, codeTable, a[j].ToString());
                                    return ret;
                                }

                            }

                            else
                            {
                                RetData = "入库计划不存在";
                            }
                            #endregion

                        }

                    }//汇总入库判断
                }

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
                    <storage_doc>" + RetData + "</storage_doc>" + msg + @"
                </Return>
            </WebService>
            ";

            return ret;
        }

    }
}



