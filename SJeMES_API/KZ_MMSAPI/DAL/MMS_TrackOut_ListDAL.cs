using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MMSAPI.DAL
{
    class MMS_TrackOut_ListDAL
    {

        internal void save(DataBase DB, string se_id, string se_seq, string size_no, string qty, string size_seq, string art_no, string mate_tieno, string tieno,string stoc_no, string rout_no, string stoc_wh,string stoc_whname ,string to_company,string userCode, string companyCode)
        {
            string now = DateTime.Now.ToShortDateString();
            string sql = string.Format(@"insert into MMS_TrackOut_List(se_id,se_seq,art_no,rout_no,tie_no,size_no,size_seq,size_qty,mate_tieno,stoc_no,stoc_wh,stoc_whname,insert_user,scan_date,to_company) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',to_date('{13}','yyyy/mm/dd'),'{14}')", se_id, se_seq, art_no,rout_no, tieno, size_no, size_seq, qty, mate_tieno,stoc_no, stoc_wh, stoc_whname, userCode, now, to_company);
            DB.ExecuteNonQuery(sql);
            string MSqlQuery = string.Format(@"select count(*) from MMS_TrackOut_M where se_id='{0}' and se_seq='{1}' and  rout_no='{2}' and  stoc_wh='{3}' and to_company='{4}'", se_id, se_seq, rout_no, stoc_wh, to_company);
            string DSqlQuery = string.Format(@"select count(*) from MMS_TrackOut_D where se_id='{0}' and se_seq='{1}' and  rout_no='{2}' and  stoc_wh='{3}' and  size_no='{4}' and  scan_date=to_date('{5}','yyyy/mm/dd') and  to_company='{6}'", se_id, se_seq, rout_no, stoc_wh, size_no, now, to_company);
            int m=DB.GetInt16(MSqlQuery);
            int d = DB.GetInt16(DSqlQuery);
            if (m>0)
            {
                string sqlM = string.Format(@"update MMS_TrackOut_M set qty_no=qty_no+{0} where se_id='{1}' and se_seq='{2}' and  rout_no='{3}' and  stoc_wh='{4}' and  to_company='{5}'", qty,se_id, se_seq, rout_no, stoc_wh,to_company);
                DB.ExecuteNonQuery(sqlM);
            }
            else
            {
                string sqlM = string.Format(@"insert into MMS_TrackOut_M(se_id,se_seq,art_no,rout_no,stoc_wh,stoc_whname,qty_no,to_company) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", se_id, se_seq, art_no, rout_no, stoc_wh, stoc_whname, qty, to_company);
                DB.ExecuteNonQuery(sqlM);

            }
            if (d>0)
            {
                string sqlD = string.Format(@"update MMS_TrackOut_D set size_qty=size_qty+{0} where se_id='{1}' and se_seq='{2}' and  rout_no='{3}' and  stoc_wh='{4}' and  size_no='{5}' and  scan_date=to_date('{6}','yyyy/mm/dd') and   to_company='{7}'", qty,se_id, se_seq, rout_no, stoc_wh, size_no, now, to_company);
                DB.ExecuteNonQuery(sqlD);
            }
            else
            {
                string sqlD = string.Format(@"insert into MMS_TrackOut_D(se_id,se_seq,art_no,rout_no,size_no,size_seq,size_qty,stoc_wh,stoc_whname,scan_date,to_company) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',to_date('{9}','yyyy/mm/dd'),'{10}')", se_id, se_seq, art_no, rout_no, size_no, size_seq, qty, stoc_wh, stoc_whname, now, to_company);
                DB.ExecuteNonQuery(sqlD);

            }


        }

        internal int QuerySizeInfo(DataBase DB, string userCode, string companyCode, string seId, string sizeNo)
        {
            string sql = string.Format(@"select SIZE_SEQ from mv_se_ord_size where se_id='{0}' and size_no='{1}'", seId, sizeNo);
            return DB.GetInt16(sql);
        }

        internal string QuerySeInfo(DataBase DB, string userCode, string companyCode, string seId)
        {
            string sql = string.Format(@"select prod_no from mv_se_ord_item where se_id='{0}'",seId);
            return DB.GetString(sql);
        }

        internal DataTable QueryWareHouse(DataBase DB,string userCode,string companyCode)
        {
            string wareHouseKeeperSql = string.Format("select MANAGEMENTAREA_NO,MANAGEMENTAREA_MEMO from mms_warehouse_keeper where staff_no='{0}'",userCode);
            DataTable dataTable = DB.GetDataTable(wareHouseKeeperSql);
            return dataTable;
        }

        internal DataTable QueryScanInfo(DataBase DB, string userCode, string companyCode,string stocWhName,string beginDate,string endDate,string toCompany,string seId)
        {
            string sql = string.Format(@"select d.to_company,m.mer_po,to_char(i.lpd,'yyyy/mm/dd') as lpd,GF_PROD_NAME_MG('{0}', d.art_no, 'T') as art_name,d.se_id,d.art_no,d.size_no,d.size_qty,d.stoc_wh,d.stoc_whname,to_char(d.scan_date,'yyyy/mm/dd') as scan_date " +//,trunc(s.se_qty, 0) as se_qty,(trunc(s.se_qty, 0)-d.size_qty) as diff_qty  订单数量和差异数量不要
                       "from  mv_se_ord_size s,mv_se_ord_item i, mv_se_ord_m m,MMS_TrackOut_D d " +
                "where d.se_id = m.se_id and  m.org_id=i.org_id and  m.se_id=i.se_id and  i.org_id=s.org_id and  i.se_id=s.se_id and  i.se_seq=s.se_seq and  d.se_seq=s.se_seq and  d.size_no=s.size_no and " +
                "stoc_whname like '{1}%'  and scan_date between to_date('{2}','yyyy/MM/dd')  and to_date('{3}','yyyy/MM/dd') and d.to_company like '{4}%' and d.se_id like '{5}%' " +
                "order by d.scan_date,d.se_id,d.size_seq",
                companyCode,stocWhName, beginDate, endDate, toCompany,seId);
            DataTable dataTable = DB.GetDataTable(sql);
            return dataTable;
        }

        internal DataTable QueryDetailScanInfo(DataBase DB, string userCode, string companyCode, string stocWhName, string beginDate, string endDate, string toCompany, string seId)
        {
            string sql = string.Format(@"select * from mms_trackout_list " +
                "where stoc_whname like '{1}%'  and scan_date between to_date('{2}','yyyy/MM/dd')  and to_date('{3}','yyyy/MM/dd') and to_company like '{4}%' and se_id like '{5}%' " +
                "order by scan_date,se_id,size_seq",
                companyCode, stocWhName, beginDate, endDate, toCompany, seId);
            DataTable dataTable = DB.GetDataTable(sql);
            return dataTable;
        }
    }
}
