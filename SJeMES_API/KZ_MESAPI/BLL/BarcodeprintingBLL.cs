using KZ_MESAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.BLL
{
    class BarcodeprintingBLL
    {
        public DataTable LoadReportData(DataBase DB, string ss, string xuh)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.LoadReportData(DB, ss, xuh);
        }

        public DataTable printquery(DataBase DB)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.printquery(DB);
        }

        public DataTable printquery1(DataBase DB)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.printquery1(DB);
        }

        //查看工单接口
        public DataTable VendnoQuery(DataBase DB)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.VendnoQuery(DB);
        }

        //查看部件名称
        public DataTable WkidQuery(DataBase DB, string wkid)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.WkidQuery(DB, wkid);
        }

        //制令查询接口
        public DataTable SeidQuery(DataBase DB)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.SeidQuery(DB);
        }

        //GetString 打印
        public string GetString(DataBase DB)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.GetString(DB);
        }

        //GetString1  打印
        public string GetString1(DataBase DB)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.GetString1(DB);
        }

        public string Group_method(DataBase DB,string usercode)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.Group_method(DB,usercode);
        }

        //打印ExecuteNonQuery
        public int ExecuteNonQuery(DataBase DB, string ID, string OUT_UNIT, string wk_id, string BAR_ID, string QTY2, string CREATEBY, string UNIT, string PROD_NO, string SHOE_NO, string TIME_ROTATION, string AD_DATA, string INSPECTOR, string PACKING_BARCODE, string UDF01,string UDF03)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            int Count = 0;
            if (Count == 0)
            {
                Dal.ExecuteNonQuery(DB, ID, OUT_UNIT, wk_id, BAR_ID, QTY2, CREATEBY, UNIT, PROD_NO, SHOE_NO, TIME_ROTATION, AD_DATA, INSPECTOR, PACKING_BARCODE, UDF01,UDF03);
            }
               return Count;
           
        }

        //ExecuteNonQuery1  打印
        public int ExecuteNonQuery1(DataBase DB, string ID, string MATERIAL_NO, string MATERIAL_NAME, string MATERIAL_SPECIFICATIONS, string WK_ID, string QTY, string user, string PACKING_BARCODE, string UDF01)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            int Count = 0;
            if (Count == 0)
            {
                Dal.ExecuteNonQuery1(DB, ID, MATERIAL_NO, MATERIAL_NAME, MATERIAL_SPECIFICATIONS, WK_ID, QTY, user, PACKING_BARCODE, UDF01);
            }
           
            return Count;
        }

        //外发单位查询接口
        public DataTable OutgoingQuery(DataBase DB,string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.OutgoingQuery(DB,s);
        }

        //加工单位查询接口
        public DataTable ProcessingQuery(DataBase DB, string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.ProcessingQuery(DB, s);
        }

        //制令带出鞋型
        public DataTable MakeshoeQuery(DataBase DB, string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.MakeshoeQuery(DB, s);
        }

        //当前制令对应的鞋型
         public DataTable ShoeNoQuery(DataBase DB, string whereID)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.ShoeNoQuery(DB, whereID);
        }

        //对应制令的码数方法接口
        public DataTable GETSizeQuery(DataBase DB, string wkid)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.GETSizeQuery(DB, wkid);
        }

        //部件的模糊查询
        public DataTable PartsQuery(DataBase DB, string wkid,string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.PartsQuery(DB, wkid,s);
        }

        //领料类别查询
        public DataTable comboBox7_TextUpdate(DataBase DB, string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.comboBox7_TextUpdate(DB, s);
        }

        //以下是报表查询的方法

        //制令，鞋型，类型，送料单位，日期选择查询
        public DataTable ReportingOrderQuery(DataBase DB, string where)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.ReportingOrderQuery(DB, where);
        }

        //报表查询
        public DataTable ReportingQuery(DataBase DB, string where, string size, string sum, string wkid, string xiex, string BAR_ID)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.ReportingQuery(DB, where,size,sum,wkid,xiex, BAR_ID);
        }

        //Test报表查询
        public DataTable TestReportingQuery(DataBase DB, string where, string size, string wkid, string art,string xiex, string BAR_ID, string unit,string there, string opertation_type, string scantime,string Deliverytime)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.TestReportingQuery(DB, where, size,  wkid, art, xiex, BAR_ID,unit,there, opertation_type, scantime, Deliverytime);
        }

        public DataTable TestReportingOrderQuery(DataBase DB, string where)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.TestReportingOrderQuery(DB, where);
        }

        //统计
        public String GetSum(DataBase DB, string str, string zhiling)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.GetSum(DB,str,zhiling);

        }

        //DelArraySame接口
        public string DelArraySame(DataBase DB, string TempArray)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.DelArraySame(DB, TempArray);
        }

        //获取制令或鞋型
        public String GetZhiling(DataBase DB, string zhiling)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.GetZhiling(DB, zhiling);
        }

        //制令输入框查询
        public DataTable comboBox11_TextUpdate(DataBase DB, string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.comboBox11_TextUpdate(DB, s);
        }

        //送料单位输入框
        public DataTable comboBox10_TextUpdate(DataBase DB, string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.comboBox10_TextUpdate(DB, s);
        }

        //送料单位输入框1
        public DataTable comboBox10_TextUpdate1(DataBase DB, string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.comboBox10_TextUpdate1(DB, s);
        }

        //获取当前仓库信息Getwarehouse
        public DataTable Getwarehouse(DataBase DB,string s)
        {
            BarcodeprintingDAL Dal = new BarcodeprintingDAL();
            return Dal.Getwarehouse(DB,s);
        }

    }
}
