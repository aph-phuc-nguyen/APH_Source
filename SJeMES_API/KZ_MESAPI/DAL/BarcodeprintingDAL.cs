using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    class BarcodeprintingDAL
    {
        public DataTable LoadReportData(DataBase DB, string ss, string xuh)
        {
            string sql = @"select OUT_UNIT,'" + ss + @"' as zhiling,SHOE_NO,QTY2,CREATEBY,UNIT,PROD_NO,BAR_ID,TIME_ROTATION, AD_DATA,INSPECTOR,
UDF01,UDF02,UDF03,UDF04,UDF05,UDF06,PACKING_BARCODE from CODE003M where PACKING_BARCODE in (" + xuh + ")";
            return DB.GetDataTable(sql);
        }

        public DataTable printquery(DataBase DB)
        {
            string sql = "select max(ID) as ID from CODE003M";
            return DB.GetDataTable(sql);
        }

        public DataTable printquery1(DataBase DB)
        {
            string sql = @"SELECT nvl(MAX(PACKING_BARCODE),'000') FROM CODE003M 
WHERE PACKING_BARCODE LIKE '" + DateTime.Now.ToString("yyyyMMdd") + @"%'";
            return DB.GetDataTable(sql);
        }
         
        //查询工单接口
        public DataTable VendnoQuery(DataBase DB)
        {
            string sql = "select VEND_NO,SHORTNM_S from mv_po_vender where ROWNUM<=100";
            return DB.GetDataTable(sql);
        }

        //查看部件名称  
        public DataTable WkidQuery(DataBase DB, string wkid)
        {
            string sql = @"select * from (SELECT DISTINCT 
 gf_partnm(a.org_id, a.part_no, 't') 部件名称
  FROM MV_SE_BOM_PART A where  A.SE_ID in (" + wkid + ")) tab ";
            return DB.GetDataTable(sql);
        }

        //制令查询接口
        public DataTable SeidQuery(DataBase DB)
        {
            string sql = "select m.se_id as se_id,m.mer_po as po_no, d.prod_no as art_no from mv_se_ord_m m,mv_se_ord_item d where m.org_id = d.org_id and m.se_id = d.se_id and m.ORG_ID='200' and m.SE_MARK='2'";
            return DB.GetDataTable(sql);
        }

        //外发单位查询接口   
        public DataTable OutgoingQuery(DataBase DB,string s)
        {
            string sql = "select DEPARTMENT_CODE,DEPARTMENT_NAME from base005m where ROWNUM<=100 and (DEPARTMENT_NAME like '%" + s + "%' or DEPARTMENT_CODE like '%" + s + "%') ";
            return DB.GetDataTable(sql);
        }

        //加工单位查询
        public DataTable ProcessingQuery(DataBase DB, string s)
        {
            string sql = "select VEND_NO,SHORTNM_S from mv_po_vender where ROWNUM<=100 and (VEND_NO like '%" + s + "%' or SHORTNM_S like '%" + s + "%')";
            return DB.GetDataTable(sql);
        }

        //制令带出鞋型
        public DataTable MakeshoeQuery(DataBase DB, string s)
        {
            string sql = @"select m.se_id as se_id,m.mer_po as po_no, d.prod_no as art_no from mv_se_ord_m m,mv_se_ord_item d
where m.org_id = d.org_id AND ROWNUM<=50 
and m.se_id = d.se_id and m.ORG_ID = '200' and m.SE_MARK = '2' and (m.se_id like '" + s + "%' or m.mer_po like '" + s + "%' or d.prod_no like '" + s + "%')  ";
            return DB.GetDataTable(sql);
        }

        //当前制令对应的鞋型
        public DataTable ShoeNoQuery(DataBase DB, string whereID)
        {
            string sql = @"SELECT DISTINCT GF_SE_PROD(A.ORG_ID,A.SE_ID,1) SHOE_NO FROM MV_SE_BOM_PART A where  A.SE_ID in (" + whereID + ")";
            return DB.GetDataTable(sql);
        }

        //对应制令的码数方法
        public DataTable GETSizeQuery(DataBase DB, string wkid)
        {
            string sql = @"select * from (select size_no from mv_se_ord_size  A where  A.SE_ID in (" + wkid + ")) tab";
            return DB.GetDataTable(sql);
        }

        //部件的模糊查询  
        public DataTable PartsQuery(DataBase DB, string wkid,string s)
        {
           string sql = @"select * from (SELECT DISTINCT 
 gf_partnm(a.org_id, a.part_no, 'T') 部件名称
  FROM MV_SE_BOM_PART A where  A.SE_ID in (" + wkid + ")) tab WHERE  部件名称 like '%" + s + "%'";
            return DB.GetDataTable(sql);
        }
        //领料类别查询
        public DataTable comboBox7_TextUpdate(DataBase DB, string s)
        {
            string sql = @"select enum_value from sys001m where enum_value like '%" + s + "%' and enum_type = 'Pickingclasses'";
            return DB.GetDataTable(sql);
        }

        //GetString
        public string GetString(DataBase DB)
        {
            string sql = @"select max(ID) as ID from CODE003M";
            return DB.GetString(sql);
        }

        //GetString1
        public string GetString1(DataBase DB)
        {
            string sql = @"SELECT nvl(MAX(UDF03),'000') FROM CODE003M  WHERE UDF03 LIKE '" + DateTime.Now.ToString("yyyyMMdd") + @"%'";
            return DB.GetString(sql);
        }

        public string Group_method(DataBase DB,string usercode)
        {
            string sql = "select udf05 from base005m where department_code in(select staff_department from hr001m where user_code = '"+usercode+"')";
            return DB.GetString(sql);
        }

        //ExecuteNonQuery
        public void ExecuteNonQuery(DataBase DB, string ID,string OUT_UNIT,string wk_id,string BAR_ID,string QTY2,string CREATEBY,string UNIT,string PROD_NO,string SHOE_NO,string TIME_ROTATION,string AD_DATA,string INSPECTOR,string PACKING_BARCODE,string UDF01,string UDF03)
        {
            string sql = @"insert into CODE003M (ID,OUT_UNIT,WK_ID,BAR_ID,QTY2,CREATEBY,UNIT,PROD_NO,SHOE_NO,TIME_ROTATION,AD_DATA,INSPECTOR,AR_DATA,FR_DATA,PACKING_BARCODE,UDF01,UDF03) 
                        VALUES('" + ID + "','" + OUT_UNIT + "','" + wk_id + "','" + BAR_ID + "','" + QTY2 + "'," +
                         "'" + CREATEBY + "','" + UNIT + "','" + PROD_NO + "','" + SHOE_NO + "','" + TIME_ROTATION + "'," +
                         "'" + AD_DATA + "','" + INSPECTOR + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                         "'" + PACKING_BARCODE + "','" + UDF01 + "','"+UDF03+"')";
            DB.ExecuteNonQueryOffline(sql);
        }

        //ExecuteNonQuery1
        public void ExecuteNonQuery1(DataBase DB, string ID,string MATERIAL_NO, string MATERIAL_NAME, string MATERIAL_SPECIFICATIONS, string WK_ID, string QTY, string user, string PACKING_BARCODE, string UDF01)
        {
            string sqll = "insert into CODE003A1(ID,PACKING_BARCODE,MATERIAL_NO,MATERIAL_NAME,MATERIAL_SPECIFICATIONS,WK_ID,QTY," +
                                    "CREATEBY,CREATEDATE,CREATETIME,UDF01) VALUES('" + ID + "','" + PACKING_BARCODE + "','" + MATERIAL_NO + "','" + MATERIAL_NAME + "','" + MATERIAL_SPECIFICATIONS + "','" + WK_ID + "','" + QTY + "','" + user + "','" +
                                     DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + UDF01 + "')";
            DB.ExecuteNonQueryOffline(sqll);
        }



        //以下是报表查询的方法

        //制令，鞋型，类型，送料单位，日期选择查询
        public DataTable ReportingOrderQuery(DataBase DB, string where)
        {
            string sql = "select DISTINCT SHOE_NO from CODE003A3  where 1=1 " + where + "";
            return DB.GetDataTable(sql);
        }

        //报表查询
        public DataTable ReportingQuery(DataBase DB, string where,string size,string sum,string wkid,string xiex,string BAR_ID)
        {
            string sql = @"select WK_ID as " + wkid + @", BAR_ID AS " + size + @",PROD_NO as " + xiex + @",'0' as " + sum + @",
" + BAR_ID + @"
from CODE003A3 where 1=1 " + where + " group by BAR_ID,WK_ID,PROD_NO";
            return DB.GetDataTable(sql);
        }


        //Test报表查询
        public DataTable TestReportingQuery(DataBase DB, string where, string size, string wkid, string art,string xiex, string BAR_ID,string unit,string there,string opertation_type,string scantime,string Deliverytime)
        {
            string sql = @"select A.WK_ID as " + wkid + @", A.BAR_ID AS " + size + @",A.PROD_NO as " + art + @",GF_PROD_NAME_MG('200',A.PROD_NO,'T') as " + xiex+@",
decode(A.OPERATION_TYPE,'入库',A.UNIT,A.UDF02) AS " + unit + @",decode(A.OPERATION_TYPE,'出库',A.UNIT,A.UDF02) AS "+ there + @",A.OPERATION_TYPE AS"+ opertation_type + @", to_char(A.CREATEDATE,'YYYY-MM-DD') AS "+ scantime + @",B.AD_DATA as "+ Deliverytime + @"," + BAR_ID + @"
from CODE003A3 A LEFT JOIN CODE003M B ON A.PACKING_BARCODE = B.PACKING_BARCODE where 1=1" + where + "and  A.UNIT is not null  group by A.BAR_ID,A.WK_ID,A.PROD_NO,A.UNIT,A.CREATEDATE,B.AD_DATA,A.OPERATION_TYPE,A.UDF02";
            return DB.GetDataTable(sql);
        }



        public DataTable TestReportingOrderQuery(DataBase DB, string where)
        {
            string sql = "select DISTINCT A.SHOE_NO from CODE003A3 A left join CODE003M B  ON A.PACKING_BARCODE = B.PACKING_BARCODE where 1=1 " + where + "";
            return DB.GetDataTable(sql);
        }
        //统计
        public String GetSum(DataBase DB, string str,string zhiling)
        {
            string sql;
            if (str.Contains('T') || str.Contains('t'))
            {
                string val = ".5";
                sql = "select sum(se_qty) from mv_se_ord_size  where se_id " +
               "in(" + ArraySame(zhiling) + ")  and " +
               "size_no='" + str.Substring(0, str.Length - 1) + val + "'";
            }
            else
            {
                sql = "select sum(se_qty) from mv_se_ord_size  where se_id " +
               "in(" + ArraySame(zhiling) + ")  and " +
               "size_no='" + str.Substring(0, str.Length - 1) + "'";
            }
            return DB.GetString(sql);

        }

        //统计需要使用的方法
        public string ArraySame(string TempArray)
        {
            //MessageBox.Show(TempArray);
            string nStr = string.Empty;
            for (int i = 0; i < TempArray.Split(',').Length; i++)
            {
                if (!nStr.Contains(TempArray.Split(',')[i]))
                {
                    if (string.IsNullOrEmpty(nStr))
                    {
                        nStr = "'" + TempArray.Split(',')[i] + "'";
                    }
                    else
                    {
                        nStr += ",'" + TempArray.Split(',')[i] + "'";
                    }
                }
            }
            return nStr;
        }


        //DelArraySame接口
        public string DelArraySame(DataBase DB, string TempArray)
        {
            string sql = @"select GF_PROD_NAME_MG('200','" + TempArray + "','T') from dual";
            return DB.GetString(sql);
        }

        //获取制令或鞋型
        public string GetZhiling(DataBase DB, string zhiling)
        {
            string sql = @"select sum(se_qty) from mv_se_ord_item  where se_id in(" + ArraySame(zhiling) + ")";
            return DB.GetString(sql);
        }


        //制令输入框查询
        public DataTable comboBox11_TextUpdate(DataBase DB, string s)
        {
            string sql = @"select m.se_id as se_id from mv_se_ord_m m
where  ROWNUM<=50 
 and m.ORG_ID = '200' and m.SE_MARK = '2' and (m.se_id like '" + s + "%')  ";
            return DB.GetDataTable(sql);
        }

        //送料单位输入框
        public DataTable comboBox10_TextUpdate(DataBase DB, string s)
        {
            string sql = "select VEND_NO from mv_po_vender where ROWNUM<=100 and (VEND_NO like '" + s + "%' )";
            return DB.GetDataTable(sql);
        }

        //送料单位输入框1
        public DataTable comboBox10_TextUpdate1(DataBase DB, string s)
        {
            string sql = "SELECT DEPARTMENT_CODE FROM base005m where ROWNUM<=100 and (DEPARTMENT_CODE like '" + s + "%') ";
            return DB.GetDataTable(sql);
        }

        //获取当前仓库信息Getwarehouse
        public DataTable Getwarehouse(DataBase DB,string s)
        {
            string sql = "select distinct(ManagementArea_no) as ManagementArea_no from MMS_Warehouse_Keeper where ManagementArea_no like '" + s+"%'";
            return DB.GetDataTable(sql);
        }
    }
}
