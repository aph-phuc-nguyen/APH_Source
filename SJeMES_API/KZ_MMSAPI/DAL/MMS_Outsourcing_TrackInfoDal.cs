using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MMSAPI.DAL
{
    class MMS_Outsourcing_TrackInfoDal
    {
        /// <summary>
        /// 扫描明细查询
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="userCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="vSeId"></param>
        /// <param name="vCompanyCode"></param>
        /// <param name="vPartName"></param>
        /// <param name="vScanType"></param>
        /// <param name="vBeginDate"></param>
        /// <param name="vEndDate"></param>
        /// <returns></returns>
        internal DataTable Query(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vScanType, string vBeginDate, string vEndDate)
        {
            string sql = string.Format("select * from sfc_trackinfo_listm where se_id like '%{0}%' and company_code like '{1}%'   and part_name like '{2}%' and Scan_type like '{3}%' and scan_date between to_date('{4}','yyyy/mm/dd') and  to_date('{5}','yyyy/mm/dd')", vSeId,vCompanyCode,vPartName,vScanType, vBeginDate, vEndDate);
            return DB.GetDataTable(sql);
        }

     
        /// <summary>
        /// 增补查询
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="userCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="vSeId"></param>
        /// <param name="vCompanyCode"></param>
        /// <param name="vPartName"></param>
        /// <param name="vScanType"></param>
        /// <param name="vBeginDate"></param>
        /// <param name="vEndDate"></param>
        /// <returns></returns>
        internal DataTable GetSupplemtData(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vScanType, string vBeginDate, string vEndDate,string  dateFormat)
        {
            if (string.IsNullOrEmpty(vScanType)|| vScanType.Equals("ALL"))
            {
                string sql = string.Format(@"select BARCODE,COMPANY_CODE,COMPANY_NAME,SUPPLEMENT_QTY,SE_ID,ART_NO,GF_PROD_NAME_MG({6},ART_NO,'T') as prod_name,PART_NAME,SIZE_NO,TO_CHAR(SCAN_DATE,'{5}') AS SCAN_DATE,SCAN_TYPE,COLUMN2
                                            from sfc_supplement_list 
                                            where se_id like '%{0}%' and company_code like '{1}%'   and part_name like '{2}%'  and scan_date between to_date('{3}','{5}') and  to_date('{4}','{5}')", vSeId, vCompanyCode, vPartName, vBeginDate, vEndDate, dateFormat,companyCode);
                return DB.GetDataTable(sql);
            }
            else
            {
                string sql = string.Format(@"select BARCODE,COMPANY_CODE,COMPANY_NAME,SUPPLEMENT_QTY,SE_ID,ART_NO,GF_PROD_NAME_MG({7},ART_NO,'T') as prod_name,PART_NAME,SIZE_NO,TO_CHAR(SCAN_DATE,'{6}') AS SCAN_DATE,SCAN_TYPE,COLUMN2
                                            from sfc_supplement_list 
                                            where se_id like '%{0}%' and company_code like '{1}%'   and part_name like '{2}%' and column2 like '{3}%' and scan_date between to_date('{4}','{6}') and  to_date('{5}','{6}')", vSeId, vCompanyCode, vPartName, vScanType, vBeginDate, vEndDate, dateFormat,companyCode);
                return DB.GetDataTable(sql);
            }
        }

        

        internal DataTable GetCode003AData(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vBeginDate, string vEndDate,string vSizeNo)
        {
            string sql = string.Format("SELECT PACKING_BARCODE,WK_ID,PROD_NO,SHOE_NO,BAR_ID,QTY,UNIT,TO_CHAR(CREATEDATE,'YYYY/MM/DD') AS CREATEDATE,GF_SFC_TRACKINFO(PACKING_BARCODE,10) as BarStoc,GF_SFC_TRACKINFO(PACKING_BARCODE,20) as BarIn,GF_SFC_TRACKINFO(PACKING_BARCODE,30) as BarOut,GF_SFC_TRACKINFO(PACKING_BARCODE,40) as BarSell,GF_PROD_NAME_MG({0},PROD_NO,'T') AS PROD_NAME " +
                "FROM CODE003A3 " +
                "WHERE OPERATION='9' and unit like '{1}%'  and shoe_no like '{2}%'  and wk_id like '%{3}%' and  createdate between to_date('{4}','yyyy/mm/dd') and  to_date('{5}','yyyy/mm/dd') and Bar_id like '{6}%'",companyCode,vCompanyCode, vPartName, vSeId, vBeginDate, vEndDate, vSizeNo);
            return DB.GetDataTable(sql);
        }

        internal DataTable GetUpateCode003ADetailData(DataBase DB, string userCode, string companyCode,string id, string dateFormat)
        {
            string sql = string.Format("select id,oldqty,curqty,createby,createtime,memo,GF_PACKING_BARCODE_QTY(packing_barcode) as BARCODE_QTY from MMS_PROC_Change_list where id={0}", id);
            return DB.GetDataTable(sql);
        }

        internal void UpateCode003AData(DataBase DB, string userCode, string companyCode, string id, string oldQty, string curQty, string memo, string dateFormat, string dateTimeFormat,string dateTimeNowFormat)
        {
            string sql = string.Format("update code003a3 set qty={0},modifyby='{1}',modifydate=to_date('{2}','{3}'),modifytime=to_date('{4}','{5}') where id={6}", curQty, userCode,DateTime.Now.ToString(dateFormat),dateFormat, DateTime.Now.ToString(dateTimeNowFormat), dateTimeFormat, id);
            DB.ExecuteNonQueryOffline(sql);
        }

        internal void DeleteCode003AData(DataBase DB, string id)
        {
            string sql = string.Format("delete from code003a3 where id={0}", id);
            DB.ExecuteNonQueryOffline(sql);
        }

        internal DataTable GetCode003MData(DataBase DB, string userCode, string companyCode, string vSeId, string vfrom, string vSizeNo,string vPartName,string vto,string vOperation)
        {
            //6|入库  9 | 出库

            string tempUnit = "";
            string tempUdf02= "";
            if (vOperation.Equals("6"))
            {
                tempUnit = vfrom;
                tempUdf02 = vto;
            }
            if (vOperation.Equals("9"))
            {
                tempUnit = vto;
                tempUdf02 = vfrom;
            }
            string sql = string.Format(@"
            SELECT packing_barcode,wk_id,prod_no,gf_prod_name_mg({0},prod_no,'T') as prod_name,shoe_no,bar_id,
            GF_PACKING_BARCODE_QTY(packing_barcode) as qty,
            GF_PACKING_BARCODE_SCAN_QTY(packing_barcode, '6', '{1}','{2}') as qty1,
            GF_PACKING_BARCODE_SCAN_QTY(packing_barcode, '9', '{1}','{2}') as qty2,
            (GF_PACKING_BARCODE_QTY(packing_barcode) - GF_PACKING_BARCODE_SCAN_QTY(packing_barcode, '6', '{1}','{2}')) as diff1,
            (GF_PACKING_BARCODE_QTY(packing_barcode) - GF_PACKING_BARCODE_SCAN_QTY(packing_barcode, '9', '{1}','{2}')) as diff2,
            (GF_PACKING_BARCODE_SCAN_QTY(packing_barcode, '6', '{1}', '{2}') - GF_PACKING_BARCODE_SCAN_QTY(packing_barcode, '9', '{1}','{2}')) as diff3
            from code003m where wk_id like '%{3}%' and bar_id='{4}' and shoe_no='{5}'", companyCode, tempUnit, tempUdf02, vSeId,vSizeNo, vPartName);
            return DB.GetDataTable(sql);
        }

        internal void updateSupplemtStatus(DataBase DB, string userCode, string companyCode, DataTable dt,string dateFormat)
        {
            for (int i=0;i<dt.Rows.Count;i++)
            {
                string supplemtBarCode = dt.Rows[i]["supplemtBarCode"].ToString();
                string company_code = dt.Rows[i]["COMPANY_CODE"].ToString();
                string scan_date = dt.Rows[i]["SCAN_DATE"].ToString();
                string scan_type = dt.Rows[i]["SCAN_TYPE"].ToString();
                string sql = string.Format(@"update sfc_supplement_list set column2='Y' 
                             where BARCODE='{0}' and COMPANY_CODE='{1}' and SCAN_DATE=to_date('{2}','{3}') and SCAN_TYPE='{4}'"
                             , supplemtBarCode, company_code, scan_date, dateFormat, scan_type);
                DB.ExecuteNonQueryOffline(sql);
            }
        }

        internal DataTable GetCode003ADetailData(DataBase DB, string userCode, string companyCode,string packing_barcode)
        {
            string sql = string.Format(@"select packing_barcode,operation_type,createdate,createtime,decode(OPERATION,'6',unit,udf02) as instoc,decode(OPERATION,'9',unit,udf02) as outstoc,qty 
                                        from code003a3 where  packing_barcode='{0}' order by createdate,createtime", packing_barcode);
            return DB.GetDataTable(sql);
        }

        internal void InserCode003ABak(DataBase dB, string id)
        {
            string sql = string.Format("insert into code003a3bak select * from code003a3 where id={0}",id);
            dB.ExecuteNonQueryOffline(sql);
        }

        internal void InsertMMS_PROC_Change_List(DataBase DB, string userCode, string companyCode, string id, string oldQty, string curQty, string memo,string packing_barcode,string dateFormat)
        {
            string sql = string.Format("insert into MMS_PROC_Change_list(id,oldqty,curqty,createby,memo,packing_barcode) values({0},{1},{2},'{3}','{4}','{5}')", id,oldQty,curQty,userCode,memo, packing_barcode);
            DB.ExecuteNonQueryOffline(sql);
        }

        internal DataTable GetUpateCode003AData(DataBase DB, string userCode, string companyCode, string vSeId, string vCompanyCode, string vPartName, string vBeginDate, string vEndDate, string vSizeNo,string dateFormat)
        {
            string areaNoSql = string.Format("select MANAGEMENTAREA_NO from mms_warehouse_keeper where staff_no='{0}'", "#" + userCode);
            string areaNo = DB.GetString(areaNoSql);
            if (string.IsNullOrEmpty(areaNo))
            {
                 areaNoSql = string.Format("select MANAGEMENTAREA_NO from mms_warehouse_keeper where staff_no='{0}'", userCode);
                 areaNo = DB.GetString(areaNoSql);
            }
            string sql = string.Format(@"SELECT ID,PACKING_BARCODE,OPERATION_TYPE,TO_CHAR(CREATEDATE,'{0}') AS CREATEDATE,CREATETIME,UNIT,WK_ID,PROD_NO,GF_PROD_NAME_MG({1},PROD_NO,'T') AS PROD_NAME,SHOE_NO,BAR_ID,GF_PACKING_BARCODE_QTY(PACKING_BARCODE) AS BARCODE_QTY,QTY 
                            FROM CODE003A3 
                            WHERE   wk_id like '%{2}%' and unit like '{3}%'  and shoe_no='{4}'   and Bar_id='{5}' and  udf02='{6}' and createdate between to_date('{7}','{8}') and  to_date('{9}','{10}')", dateFormat, companyCode, vSeId,vCompanyCode, vPartName, vSizeNo, areaNo,vBeginDate, dateFormat,vEndDate,dateFormat); 
            return DB.GetDataTable(sql);
        }

        internal DataTable GetDetail(DataBase DB, string userCode, string companyCode, string vPacking_barcode)
        {
            string sql = string.Format("select insert_date,scan_type_name,scan_qty from sfc_trackinfo_listm" +
             " WHERE barcode='{0}' order by scan_type", vPacking_barcode);
            return DB.GetDataTable(sql);
        }

        private DataTable GetPartName(DataBase DB, string vWK_ID)
        {
            string sql = string.Format("select distinct SHOE_NO from code003m" +
             " WHERE wk_id like '%{0}%'",vWK_ID);
            return DB.GetDataTable(sql);
        }

        private string GetDecodeString(DataTable dataTable,string vWK_ID,string vCompany)
        {
            string str = "select max(unit) AS \"unit\",max(WK_ID) AS \"订单号\",BAR_ID as \"size\",GF_SE_SIZE_QTY(max(wk_id),BAR_ID) AS \"订单size数量\",";
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                str += @" sum(decode(A.SHOE_NO,'" + dataTable.Rows[i][0] + "',A.QTY,0)) as \"" + dataTable.Rows[i][0] + "\",";
            }
            str += " '' AS \"配套数量\" from  CODE003A3 A WHERE wk_id='" + vWK_ID+ "' and OPERATION='6' and unit='"+vCompany+ "' group by BAR_ID ORDER BY BAR_ID";
            return str;
        }

        /// <summary>
        /// 获取配套资料
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="userCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="vWK_ID"></param>
        /// <returns></returns>
        internal DataTable GetMatchingPart(DataBase DB, string userCode, string companyCode, string vWK_ID,string vCompany)
        {
            DataTable dataTable=GetPartName(DB,vWK_ID); 
            string sql = GetDecodeString(dataTable, vWK_ID,vCompany);
            return DB.GetDataTable(sql);
        }

        internal DataTable GetOutsourcingDetail(DataBase DB, string userCode, string companyCode, string vWK_ID, string vCompany, string vSizeNo)
        {
            string sql = string.Format(@"SELECT  id,PACKING_BARCODE,WK_ID,UNIT,SHOE_NO,BAR_ID,TO_CHAR(CREATEDATE,'YYYY/MM/DD') AS CREATEDATE,GF_PACKING_BARCODE_QTY(PACKING_BARCODE) AS PACKING_BARCODE_QTY,QTY FROM CODE003A3 WHERE WK_ID='{0}' AND OPERATION='9' AND BAR_ID='{1}' AND UNIT='{2}' " +
                         "ORDER BY SHOE_NO, BAR_ID,CREATEDATE", vWK_ID, vSizeNo,vCompany);
            return DB.GetDataTable(sql);
        }

        internal DataTable GetReceivingDetail(DataBase DB, string userCode, string companyCode, string vWK_ID, string vCompany, string vSizeNo)
        {
            string sql = string.Format(@"SELECT  id,PACKING_BARCODE,WK_ID,UNIT,SHOE_NO,BAR_ID,TO_CHAR(CREATEDATE,'YYYY/MM/DD') AS CREATEDATE,GF_PACKING_BARCODE_QTY(PACKING_BARCODE) AS PACKING_BARCODE_QTY,QTY FROM CODE003A3 WHERE WK_ID='{0}' AND OPERATION='6' AND BAR_ID='{1}' AND UNIT='{2}' " +
                         "ORDER BY SHOE_NO, BAR_ID,CREATEDATE", vWK_ID, vSizeNo, vCompany);
            return DB.GetDataTable(sql);
        }

        internal DataTable GetScanDetail(DataBase DB,string vWK_ID, string vCompany)
        {
            string sql = string.Format(@"SELECT  wk_id,to_char(createdate,'yyyy/mm/dd') as createdate,shoe_no,operation_type,bar_id,sum(qty) as qty from code003a3 where wk_id LIKE '%{0}%'  and UNIT='{1}'   
                                group by wk_id,shoe_no,operation_type,bar_id,createdate 
                                order by operation_type,shoe_no,createdate,bar_id", vWK_ID,vCompany);
            return DB.GetDataTable(sql);
        }
    }
}
