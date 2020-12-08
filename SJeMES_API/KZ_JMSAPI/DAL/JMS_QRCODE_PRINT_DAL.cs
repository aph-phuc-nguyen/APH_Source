using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_JMSAPI.DAL
{
    public class JMS_QRCode_Print_DAL
    {
        public DataTable GetUnitList(DataBase DB)
        {
            string sqlunit = "select code,org from sjqdms_orginfo";
            System.Data.DataTable dt = DB.GetDataTable(sqlunit);
            return dt;
        }

        public DataTable GetRoutList(DataBase DB)
        {
            string sqlRout = "select rout_no,rout_name,print_num from JMS_QRCODE_ROUT_NUM";
            System.Data.DataTable dt = DB.GetDataTable(sqlRout);
            return dt;
        }

        public DataTable GetSeidList(DataBase DB, string CompanyCode)
        {
            string sqlunit = "select se_id from mv_se_ord_m where 1 = 1";
            sqlunit += " and org_id = '" + CompanyCode + "' and se_mark = '2' and status = '7'";
            System.Data.DataTable dt = DB.GetDataTable(sqlunit);
            return dt;
        }

        public DataTable GetSizeListBySeID(DataBase DB, string vSeId, string vRoutNo)
        {
            string sql = string.Format(@"select a.size_no,a.se_qty,nvl(b.finish_qty,0) as finish_qty,(a.se_qty-nvl(b.finish_qty,0)) as unfinish_qty,a.size_seq from mv_se_ord_size a left join 
(select se_id, size_no, sum(size_qty) as finish_qty from JMS_QRCODE_PRINT_D where rout_no = '{0}' group by se_id, size_no) b 
on a.se_id = b.se_id and a.size_no = b.size_no where a.se_id = '{1}' order by a.size_seq asc",vRoutNo, vSeId);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetMaxPrintVer(DataBase DB, string vSeId, string vRoutNo)
        {
            string sql = @"select print_ver,end_tieno from (select print_ver,end_tieno from JMS_QRCODE_PRINT_M where 1 = 1";
            sql += " and se_id = '" + vSeId + "'";
            sql += " and rout_no = '" + vRoutNo + "' order by print_ver desc) where rownum = 1";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }


        public bool InsertQrCodeData(DataBase DB, string CompanyCode, DataTable vDt, string userId, string vUnit, string vSeId, string vRoutNo, string vStartDate, string vFinishDate, int vPrintVer, int vEndTieno, int vPrintNum)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                bool result = InsertQrCodeData_trance(DB, CompanyCode, vDt, userId, vUnit, vSeId, vRoutNo, vStartDate, vFinishDate, vPrintVer, vEndTieno, vPrintNum);
                DB.Commit();
                return result;
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }

        public bool InsertQrCodeData_trance(DataBase DB, string CompanyCode, DataTable vDt, string userId, string vUnit, string vSeId, string vRoutNo, string vStartDate, string vFinishDate, int vPrintVer, int vEndTieno, int vPrintNum)
        {
            string sqlHasPrint = "select se_id from JMS_QRCODE_PRINT_M where 1 = 1"; 
            sqlHasPrint += " and rout_no = '" + vRoutNo + "'";
            sqlHasPrint += " and print_ver = '" + vPrintVer + "'";
            sqlHasPrint += " and se_id = '" + vSeId + "'";
            System.Data.DataTable dt_HasPrint = DB.GetDataTable(sqlHasPrint);
            if (dt_HasPrint.Rows.Count > 0)
            {
                return false;
            }

            string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='" + userId + "'";
            string grt_dept = DB.GetString(sqlDept);

            string thisTime = DB.GetString("select to_char(sysdate,'yyyymmddhh24miss') as time from dual");
            string mateTieNo = "SM0A" + thisTime;

            string sql_mold = @"select a.prod_no,b.mold_no from mv_se_ord_item a,mv_rd_prod b where a.prod_no = b.prod_no";
            sql_mold += " and a.se_id = '" + vSeId + "'";
            System.Data.DataTable dt_mold = DB.GetDataTable(sql_mold);

            string artNo = dt_mold.Rows[0][0].ToString();
            string moldNo = dt_mold.Rows[0][1].ToString();
            int end_tieno = vEndTieno;
            int qty = 0;

            for (int i = 0;i< vDt.Columns.Count;i++)
            {
                string size_no = vDt.Columns[i].ColumnName.Trim();
                int size_qty = int.Parse(vDt.Rows[0][i].ToString());
                qty = qty + size_qty;
                int size_seq = int.Parse(vDt.Rows[1][i].ToString());
                if (size_qty <= 0)
                {
                    continue;
                }
                int n = size_qty % vPrintNum == 0 ? size_qty / vPrintNum : size_qty / vPrintNum + 1;
                for (int j = 0;j<n;j++)
                {
                    end_tieno++;
                    int onceQty = j * vPrintNum + vPrintNum <= size_qty ? vPrintNum : size_qty - j * vPrintNum;
                    string sqlInsertD = @"insert into JMS_QRCODE_PRINT_D(
ORG_ID,UNIT_NO,ROUT_NO,SE_ID,PRINT_VER,TIE_NO,SIZE_NO,SIZE_QTY,GRT_DEPT,GRT_USER,
LAST_USER,START_DATE,FINISH_DATE,MATE_TIENO,SIZE_SEQ,ART_NO,MOD_NO,PINT_TYPE) 
VALUES(
'" + CompanyCode + "','" + vUnit + "','" + vRoutNo + "','" + vSeId + "','" + vPrintVer + "','" + end_tieno + "','" + size_no + "'," +
onceQty + ",'" + grt_dept + "','" + userId + "','" + userId + "',to_date('" + vStartDate + "','yyyy-mm-dd'),to_date('" +
vFinishDate + "','yyyy-mm-dd'),'" + mateTieNo + "'," + size_seq + ",'" + artNo + "','" + moldNo + "','A')";

                    DB.ExecuteNonQuery(sqlInsertD);
                }
            }
            if (qty > 0)
            {
                string sqlInsertM = @"insert into JMS_QRCODE_PRINT_M(
ORG_ID,UNIT_NO,ROUT_NO,SE_ID,PRINT_VER,STATUS,QTY,GRT_DEPT,GRT_USER,LAST_USER,
START_DATE,FINISH_DATE,MATE_TIENO,ART_NO,MOD_NO,BEGIN_TIENO,END_TIENO,PINT_TYPE) 
VALUES(
'" + CompanyCode + "','" + vUnit + "','" + vRoutNo + "','" + vSeId + "','" + vPrintVer + "',7," + qty + ",'" + grt_dept + "','" +
userId + "','" + userId + "',to_date('" + vStartDate + "','yyyy-mm-dd'),to_date('" + vFinishDate + "','yyyy-mm-dd'),'" +
mateTieNo + "','" + artNo + "','" + moldNo + "'," + (vEndTieno + 1) + "," + end_tieno + ",'A')";

                DB.ExecuteNonQuery(sqlInsertM);
            }
            
            return true;
        }

        public DataTable GetQrCodeBySeidAndVer(DataBase DB, string vSeId, string vPrintVer, string vRoutNo)
        {
            string sqlHasPrint = "select qr_code,org_id,org,unit_no,rout_no,se_id,size_no,size_qty,print_ver,mate_tieno,tie_no,art_no,mod_no,start_date,finish_date from VW_JMS_QRCODE_PRINT_D where 1 = 1";
            sqlHasPrint += " and rout_no = '" + vRoutNo + "'";
            sqlHasPrint += " and print_ver = '" + vPrintVer + "'";
            sqlHasPrint += " and se_id = '" + vSeId + "' order by tie_no asc";
            System.Data.DataTable dt = DB.GetDataTable(sqlHasPrint);
            return dt;
        }

        public DataTable GetMListBySeid(DataBase DB, string vSeId, string vRoutNo)
        {
            string sql = string.Format(@"select a.se_id,a.rout_no,a.print_ver,b.org,a.qty,to_char(a.start_date,'yyyy/mm/dd') as start_date,to_char(a.finish_date,'yyyy/mm/dd') as finish_date,a.grt_user,a.insert_date,a.begin_tieno,a.end_tieno 
from JMS_QRCODE_PRINT_M a,sjqdms_orginfo b where a.unit_no = b.code and a.rout_no = '{0}' and a.se_id = '{1}' order by a.print_ver asc", vRoutNo, vSeId);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryDListBySeidAndVer(DataBase DB, string vSeId, string vPrintVer, string vRoutNo)
        {
            string sql = string.Format(@"select se_id,print_ver,size_no,sum(size_qty) as size_qty,min(tie_no) as begin_tieno,max(tie_no) as end_tieno,'{1}' as rout_no from JMS_QRCODE_PRINT_D 
where  print_ver = '{0}' and rout_no = '{1}' and se_id = '{2}' group by se_id,print_ver,size_no,size_seq order by size_seq asc", vPrintVer, vRoutNo, vSeId); 
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryDDetialBySize(DataBase DB, string vSeId, string vPrintVer, string vSizeNo, string vRoutNo)
        {
            string sql = string.Format(@"select se_id,print_ver,tie_no,size_no,size_qty,rout_no from JMS_QRCODE_PRINT_D where rout_no = '{0}' and print_ver = '{1}' and size_no = '{2}' and se_id = '{3}' order by tie_no asc", vRoutNo, vPrintVer, vSizeNo, vSeId);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetQrCodeByTieNo(DataBase DB, string vSeId, string vTieNo, string vRoutNo)
        {
            string sql = string.Format(@"select qr_code,org_id,org,unit_no,rout_no,se_id,size_no,size_qty,print_ver,mate_tieno,tie_no,art_no,mod_no,start_date,finish_date from VW_JMS_QRCODE_PRINT_D 
where rout_no = '{0}' and tie_no = '{1}' and se_id = '{2}'", vRoutNo, vTieNo, vSeId);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetQrCodeBySizeAndVer(DataBase DB, string vSeId, string vPrintVer, string vSizeNo, string vRoutNo)
        {
            string sql = string.Format(@"select qr_code,org_id,org,unit_no,rout_no,se_id,size_no,size_qty,print_ver,mate_tieno,tie_no,art_no,mod_no,start_date,finish_date from VW_JMS_QRCODE_PRINT_D 
where rout_no = '{0}' and print_ver = '{1}' and size_no = '{2}' and se_id = '{3}' order by tie_no asc", vRoutNo, vPrintVer, vSizeNo, vSeId);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

    }
}
