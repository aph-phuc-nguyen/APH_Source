using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace KZ_MESAPI.DAL
{
    /// <summary>
    /// 用于做生管日生产计划的数据库操作
    /// </summary>
    public class ProductionTargetsDAL
    {
        /// <summary>
        /// -------该功能主要执行验证生管上传的日计划-------
        /// 1:验证是否有改组别资料
        /// 2:检验订单是否存在这个size
        /// 3:检验完工数量是否大于用户排的入的排产数量
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        //public int ValiUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string companyCode)
        //{
        //    int errorCount = 0;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        byte v_status = 7;
        //        string is_in = "";
        //        string is_out = "";
        //        string se_id = dt.Rows[i][0].ToString();
        //        string size_no = dt.Rows[i][1].ToString();
        //        decimal v_work_qty = dt.Rows[i][2] == null ? 0 : Decimal.Parse(dt.Rows[i][2].ToString());
        //        decimal v_supplement_qty = dt.Rows[i][3] == null || string.IsNullOrWhiteSpace(dt.Rows[i][3].ToString()) ? 0 : decimal.Parse(dt.Rows[i][3].ToString());
        //        string d_dept = dt.Rows[i][4].ToString();
        //        DateTime workDate = DateTime.Parse(dt.Rows[i][5].ToString());
        //        string sql1 = @"select * from BASE005M " +
        //            "where  department_code='" + d_dept + "'";
        //        System.Data.DataTable base005M = DB.GetDataTable(sql1);
        //        if (base005M.Rows.Count <= 0)
        //        {
        //            v_status = 1;
        //        }
        //        else
        //        {
        //            is_in = base005M.Rows[0]["UDF02"].ToString();
        //            is_out = base005M.Rows[0]["UDF03"].ToString();
        //        }
        //        string sql2 = @"select * from mv_se_ord_size " +
        //            "where  org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1' and size_no='" + size_no + "'";
        //        System.Data.DataTable seOrdSize = DB.GetDataTable(sql2);
        //        if (seOrdSize.Rows.Count <= 0)
        //        {
        //            v_status = 1;
        //        }
        //        //如果该工厂,部门，生产时间，订单号，size,用户投入排产（COLUMN1='U' AND  INOUT_PZ='IN'）的完工数量大于了排产数量，报错
        //        string sql3 = @"select * from sjqdms_work_day_size  " +
        //               "where org_id='" + companyCode + "' " +
        //               "and d_dept='" + d_dept + "' " +
        //               "and to_char(work_day,'yyyy/mm/dd')='" + workDate + "' " +
        //               "and se_id='" + se_id + "' " +
        //               "and size_no='" + size_no + "' " +
        //               "and column1='U' " +
        //               "and  finish_qty>" + (v_supplement_qty + v_work_qty);
        //        if (is_in == "Y")
        //        {
        //            sql3 += "and inout_pz='IN'";
        //        }
        //        else if (is_in == "N" && is_out == "Y")
        //        {
        //            sql3 += "and inout_pz='OUT'";
        //        }
        //        System.Data.DataTable sjqdmsWorkDaySize = DB.GetDataTable(sql3);
        //        if (sjqdmsWorkDaySize.Rows.Count > 0)
        //        {
        //            v_status = 1;
        //        }
        //        if (v_status == 1)
        //        {
        //            errorCount++;
        //        }
        //    }
        //    return errorCount;
        //}

        public DataTable ValiUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string companyCode, string dateFormate, string datetimeFormate)
        {
            //int errorCount = 0;
            DataTable tab = new DataTable();
            tab.Columns.Add("POSITION");
            tab.Columns.Add("ROW_INDEX");
            tab.Columns.Add("SIZE");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                string is_in = "";
                string is_out = "";
                string d_dept = dt.Rows[i][0].ToString();
                string se_id = dt.Rows[i][1].ToString();
                DateTime workDate = DateTime.Parse(dt.Rows[i][3].ToString());

                string sql1 = @"select * from BASE005M " +
                    "where  department_code='" + d_dept + "'";
                System.Data.DataTable base005M = DB.GetDataTable(sql1);
                if (base005M.Rows.Count <= 0)
                {
                    //errorCount++;
                    DataRow dr = tab.NewRow();
                    dr[0] = "msg-plan-00001";
                    dr[1] = i+1;
                    dr[2] = "";
                    tab.Rows.Add(dr);
                    continue;
                }
                else
                {
                    is_in = base005M.Rows[0]["UDF02"].ToString();
                    is_out = base005M.Rows[0]["UDF03"].ToString();
                }

                string sql2 = @"select * from mv_se_ord_m where org_id='" + companyCode + "' and se_id='" + se_id + "'";
                System.Data.DataTable seOrdM = DB.GetDataTable(sql2);
                if (seOrdM.Rows.Count <= 0)
                {
                    //errorCount++;
                    DataRow dr = tab.NewRow();
                    dr[0] = "msg-plan-00002";
                    dr[1] = i+1;
                    dr[2] = "";
                    tab.Rows.Add(dr);
                    continue;
                }

                for (int j = 4;j < dt.Columns.Count-1;j++)
                {
                    decimal v_work_qty = dt.Rows[i][j].ToString().Trim() == "" ? -1 : Decimal.Parse(dt.Rows[i][j].ToString());
                    if (v_work_qty >= 0)
                    {
                        string size_no = dt.Columns[j].ColumnName.Trim();

                        string sql3 = @"select * from mv_se_ord_size where org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1' and size_no='" + size_no + "'";
                        System.Data.DataTable seOrdSize = DB.GetDataTable(sql3);
                        if (seOrdSize.Rows.Count <= 0)
                        {
                            //errorCount++;
                            DataRow dr = tab.NewRow();
                            dr[0] = "msg-plan-00003";
                            dr[1] = i+1;
                            dr[2] = size_no;
                            tab.Rows.Add(dr);
                            continue;
                        }

                        //如果该工厂,部门，生产时间，订单号，size,用户投入排产（COLUMN1='U' AND  INOUT_PZ='IN'）的完工数量大于了排产数量，报错
                        string sql4 = @"select * from sjqdms_work_day_size  " +
                       "where org_id='" + companyCode + "' " +
                       "and d_dept='" + d_dept + "' " +
                       "and work_day=to_date('" + workDate.ToString(dateFormate) + "','" + dateFormate + "') " +
                       "and se_id='" + se_id + "' " +
                       "and size_no='" + size_no + "' " +
                       "and column1='U' " +
                       "and  finish_qty>" + v_work_qty;
                        if (is_in == "Y")
                        {
                            sql4 += " and inout_pz='IN'";
                        }
                        else if (is_in == "N" && is_out == "Y")
                        {
                            sql4 += " and inout_pz='OUT'";
                        }
                        System.Data.DataTable sjqdmsWorkDaySize = DB.GetDataTable(sql4);
                        if (sjqdmsWorkDaySize.Rows.Count > 0)
                        {
                            //errorCount++;
                            DataRow dr = tab.NewRow();
                            dr[0] = "msg-plan-00004";
                            dr[1] = i+1;
                            dr[2] = size_no;
                            tab.Rows.Add(dr);
                            continue;
                        }
                    }
                }
            }
            return tab;
        }

        public DataTable LoadSeDept(DataBase DB, string userId, string companyCode)
        {
            string sql = "SELECT  DEPARTMENT_CODE,DEPARTMENT_NAME FROM base005m";
            return DB.GetDataTable(sql);
        }


        /// <summary>
        /// -------该功能执行生管日生产计划的上传操作(（COLUMN1='U' AND  INOUT_PZ='IN')-------
        /// 1：如果没有日排产，直接插入日排程（主表跟size表）
        /// 2：如果有日排产，则修改日排程
        /// </summary>
        /// <param name="dB"></param>
        /// <param name="dt"></param>
        /// <param name="userId"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public void UpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string companyCode, string dateFormate,string datetimeFormate)
        {
            string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='" + userId + "'";
            string grt_dept = DB.GetString(sqlDept);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rout_no = "";
                string is_in = "";
                string is_out = "";
                string vInOut = "";
                string po = "";
                string art_no = "";
                string se_day = "";
                string thisTime = DateTime.Now.ToString( dateFormate + "HH:mm:ss");
                string d_dept = dt.Rows[i][0].ToString();
                string se_id = dt.Rows[i][1].ToString();
                DateTime workDate = DateTime.Parse(dt.Rows[i][3].ToString());

                string depSql = @"select * from BASE005M " +
                    "where  department_code='" + d_dept + "'";
                System.Data.DataTable base005M = DB.GetDataTable(depSql);
                if (base005M.Rows.Count > 0)
                {
                    rout_no = base005M.Rows[0]["UDF01"].ToString();
                    is_in = base005M.Rows[0]["UDF02"].ToString();
                    is_out = base005M.Rows[0]["UDF03"].ToString();
                }

                string poSql = "select mer_po from mv_se_ord_m " +
                    "where  org_id='" + companyCode + "' and se_id='" + se_id + "'";
                System.Data.DataTable seOrdM = DB.GetDataTable(poSql);
                if (seOrdM.Rows.Count > 0)
                {
                    po = seOrdM.Rows[0]["MER_PO"].ToString();
                }

                string artSql = @"select * from mv_se_ord_item " +
                   "where  org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1'";
                System.Data.DataTable seOrdItem = DB.GetDataTable(artSql);
                if (seOrdItem.Rows.Count > 0)
                {
                    art_no = seOrdItem.Rows[0]["PROD_NO"].ToString();
                    se_day = ((DateTime)seOrdItem.Rows[0]["LPD"]).ToString(dateFormate);
                }

                for (int j = 4; j < dt.Columns.Count-1; j++)
                {
                    decimal v_work_qty = dt.Rows[i][j].ToString().Trim() == "" ? -1 : Decimal.Parse(dt.Rows[i][j].ToString());
                    decimal v_supplement_qty = 0;
                    if (v_work_qty >= 0)
                    {
                        string size_no = dt.Columns[j].ColumnName.Trim();

                        string size_seq = "0";
                        string sizeSql = @"select * from mv_se_ord_size where org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1' and size_no='" + size_no + "'";
                        System.Data.DataTable seOrdSize = DB.GetDataTable(sizeSql);
                        if (seOrdSize.Rows.Count > 0)
                        {
                            size_seq = seOrdSize.Rows[0]["SIZE_SEQ"].ToString();
                        }

                        string sqlDay = @"select * from sjqdms_work_day  " +
                       "where org_id='" + companyCode + "' " +
                       "and d_dept='" + d_dept + "' " +
                       "and work_day=to_date('" + workDate.ToString(dateFormate) + "','"+dateFormate+"') " +
                       "and se_id='" + se_id + "' " +
                       "and column1='U' ";
                        if (is_in == "Y")
                        {
                            sqlDay += "and inout_pz='IN'";
                            vInOut = "IN";
                        }
                        else if (is_in == "N" && is_out == "Y")
                        {
                            sqlDay += "and inout_pz='OUT'";
                            vInOut = "OUT";
                        }
                        System.Data.DataTable sjqdmsWorkDay = DB.GetDataTable(sqlDay);
                        if (sjqdmsWorkDay.Rows.Count <= 0)
                        {
                            insertSjqdmsWorkDay(DB, companyCode, se_id, po, se_day,
                                d_dept, userId, d_dept, workDate, rout_no,
                                v_work_qty, art_no, v_supplement_qty, vInOut, thisTime, dateFormate, datetimeFormate);
                            insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
                                d_dept, userId, d_dept, workDate, v_work_qty,
                                v_supplement_qty, vInOut, thisTime, dateFormate, datetimeFormate);
                            insertMesLabelM_BLL(DB, companyCode, se_id, po, size_no, thisTime, userId, grt_dept, dateFormate, datetimeFormate);
                        }
                        else
                        {
                            decimal oldSizeWorkQty = 0;
                            decimal oldSizeSupplementQty = 0;
                            string sql4 = @"select * from sjqdms_work_day_size  " +
                              "where org_id='" + companyCode + "' " +
                              "and d_dept='" + d_dept + "' " +
                              "and work_day=to_date('" + workDate.ToString(dateFormate) + "','"+dateFormate+"') " +
                              "and se_id='" + se_id + "' " +
                              "and size_no='" + size_no + "' " +
                              "and column1='U' ";
                            if (is_in == "Y")
                            {
                                sql4 += "and inout_pz='IN'";
                            }
                            else if (is_in == "N" && is_out == "Y")
                            {
                                sql4 += "and inout_pz='OUT'";
                            }
                            System.Data.DataTable sjqdmsWorkDaySize = DB.GetDataTable(sql4);
                            if (sjqdmsWorkDaySize.Rows.Count <= 0)
                            {
                                insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
                                d_dept, userId, d_dept, workDate, v_work_qty,
                                v_supplement_qty, vInOut, thisTime,dateFormate, datetimeFormate);
                                insertMesLabelM_BLL(DB, companyCode, se_id, po, size_no, thisTime, userId, grt_dept, dateFormate, datetimeFormate);
                            }
                            else
                            {
                                oldSizeWorkQty = decimal.Parse(sjqdmsWorkDaySize.Rows[0]["WORK_QTY"].ToString());
                                oldSizeSupplementQty = decimal.Parse(sjqdmsWorkDaySize.Rows[0]["SUPPLEMENT_QTY"].ToString());
                                updateSjqdmsWorkDaySize(DB, companyCode, se_id, size_no,
                                d_dept, userId, d_dept, workDate,
                                v_work_qty, v_supplement_qty, vInOut, thisTime, oldSizeWorkQty, oldSizeSupplementQty, dateFormate, datetimeFormate);
                            }
                            updateSjqdmsWorkDay(DB, companyCode, se_id, d_dept, userId,
                                 d_dept, workDate, v_work_qty, v_supplement_qty, vInOut,
                                 thisTime, oldSizeWorkQty, oldSizeSupplementQty, dateFormate, datetimeFormate);
                        }
                    }
                }
            }
        }


        //public void UpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string companyCode)
        //{
        //    string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='" + userId + "'";
        //    string grt_dept = DB.GetString(sqlDept);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        string rout_no = "";
        //        string is_in = "";
        //        string is_out = "";
        //        string vInOut = "";
        //        string thisTime = DateTime.Now;
        //        string se_id = dt.Rows[i][0].ToString();
        //        string size_no = dt.Rows[i][1].ToString();
        //        decimal v_work_qty = dt.Rows[i][2] == null ? 0 : Decimal.Parse(dt.Rows[i][2].ToString());
        //        decimal v_supplement_qty = dt.Rows[i][3] == null || string.IsNullOrWhiteSpace(dt.Rows[i][3].ToString()) ? 0 : decimal.Parse(dt.Rows[i][3].ToString());
        //        string d_dept = dt.Rows[i][4].ToString();
        //        DateTime workDate = DateTime.Parse(dt.Rows[i][5].ToString());
        //        string sql1 = @"select * from BASE005M " +
        //            "where  department_code='" + d_dept + "'";
        //        System.Data.DataTable base005M = DB.GetDataTable(sql1);
        //        if (base005M.Rows.Count > 0)
        //        {
        //            rout_no= base005M.Rows[0]["UDF01"].ToString();
        //            is_in = base005M.Rows[0]["UDF02"].ToString();
        //            is_out = base005M.Rows[0]["UDF03"].ToString();
        //        }
        //        string po = "";
        //        string se_day = "";
        //        string poSql = "select mer_po from mv_se_ord_m " +
        //            "where  org_id='" + companyCode + "' and se_id='" + se_id +"'";
        //        System.Data.DataTable seOrdM = DB.GetDataTable(poSql);
        //        if (seOrdM.Rows.Count > 0)
        //        {
        //            po = seOrdM.Rows[0]["MER_PO"].ToString();
        //        }

        //        string art_no = "";
        //        string artSql = @"select * from mv_se_ord_item " +
        //           "where  org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1'";
        //        System.Data.DataTable seOrdItem = DB.GetDataTable(artSql);
        //        if (seOrdItem.Rows.Count > 0)
        //        {
        //            art_no = seOrdItem.Rows[0]["PROD_NO"].ToString();
        //            se_day = seOrdItem.Rows[0]["LPD"].ToString();
        //        }
        //        string size_seq = "0";
        //        string sql2 = @"select * from mv_se_ord_size " +
        //            "where  org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1' and size_no='" + size_no + "'";
        //        System.Data.DataTable seOrdSize = DB.GetDataTable(sql2);
        //        if (seOrdSize.Rows.Count >0)
        //        {
        //            size_seq= seOrdSize.Rows[0]["SIZE_SEQ"].ToString();
        //        }     
        //        //如果该工厂,部门，生产时间，订单号，size,用户投入排产（COLUMN1='U' AND  INOUT_PZ='IN'）的完工数量大于了排产数量，报错
        //        string sql3 = @"select * from sjqdms_work_day  " +
        //               "where org_id='" + companyCode + "' " +
        //               "and d_dept='" + d_dept + "' " +
        //               "and work_day=to_date('" + workDate.ToString(dateFormate) + "','yyyy/mm/dd') " +
        //               "and se_id='" + se_id + "' " +
        //               "and column1='U' ";
        //        if (is_in == "Y")
        //        {
        //            sql3 += "and inout_pz='IN'";
        //            vInOut = "IN";
        //        }
        //        else if (is_in == "N" && is_out == "Y")
        //        {
        //            sql3 += "and inout_pz='OUT'";
        //            vInOut = "OUT";
        //        }
        //        System.Data.DataTable sjqdmsWorkDay = DB.GetDataTable(sql3);
        //        if (sjqdmsWorkDay.Rows.Count <= 0)
        //        {
        //            insertSjqdmsWorkDay(DB, companyCode, se_id, po,se_day,
        //                d_dept, userId, d_dept, workDate, rout_no,
        //                v_work_qty, art_no, v_supplement_qty, vInOut,thisTime);
        //            insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
        //                d_dept, userId, d_dept, workDate, v_work_qty,
        //                v_supplement_qty, vInOut, thisTime);
        //            insertMesLabelM_BLL(DB, companyCode, se_id, po, size_no, thisTime, userId, grt_dept);
        //        }
        //        else 
        //        {
        //            decimal oldSizeWorkQty = 0;
        //            decimal oldSizeSupplementQty = 0;
        //            string sql4 = @"select * from sjqdms_work_day_size  " +
        //              "where org_id='" + companyCode + "' " +
        //              "and d_dept='" + d_dept + "' " +
        //              "and work_day=to_date('" + workDate.ToString(dateFormate) + "','yyyy/mm/dd') " +
        //              "and se_id='" + se_id + "' " +
        //              "and size_no='" + size_no + "' " +
        //              "and column1='U' ";
        //            if (is_in == "Y")
        //            {
        //                sql4 += "and inout_pz='IN'";
        //            }
        //            else if (is_in == "N" && is_out == "Y")
        //            {
        //                sql4 += "and inout_pz='OUT'";
        //            }
        //            System.Data.DataTable sjqdmsWorkDaySize = DB.GetDataTable(sql4);
        //            if (sjqdmsWorkDaySize.Rows.Count<=0)
        //            {
        //                insertSjqdmsWorkDaySize(DB, companyCode, se_id, size_no, size_seq,
        //                d_dept, userId, d_dept, workDate, v_work_qty,
        //                v_supplement_qty, vInOut, thisTime);
        //                insertMesLabelM_BLL(DB, companyCode, se_id, po,size_no,thisTime, userId, grt_dept);
        //            }
        //            else
        //            {
        //                oldSizeWorkQty = decimal.Parse(sjqdmsWorkDaySize.Rows[0]["WORK_QTY"].ToString());
        //                oldSizeSupplementQty= decimal.Parse(sjqdmsWorkDaySize.Rows[0]["SUPPLEMENT_QTY"].ToString());
        //                updateSjqdmsWorkDaySize(DB,companyCode,se_id,size_no,
        //                d_dept, userId, d_dept,workDate,
        //                v_work_qty,v_supplement_qty,vInOut,thisTime,oldSizeWorkQty,oldSizeSupplementQty);
        //            }
        //            updateSjqdmsWorkDay(DB,companyCode,se_id, d_dept, userId, 
        //                 d_dept,workDate,v_work_qty,v_supplement_qty,vInOut, 
        //                 thisTime,oldSizeWorkQty,oldSizeSupplementQty);
        //        }
        //    }
        //}

        private void insertMesLabelM_BLL(DataBase DB, string companyCode, string se_id, string po,string size_no, string thisTime,string userCode, string grt_dept,string dateFormate,string datetimeFormate)
        {
            string sql = "select * from mes_label_m where org_id='"+companyCode+"' and se_id='"+se_id+"' and size_no='"+size_no+"'";
            DataTable mesLabelM = DB.GetDataTable(sql);
            if (mesLabelM.Rows.Count<1)
            {
                string sql2 = "select * from mv_se_ord_size where org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1' and size_no='" + size_no + "'";
                DataTable seOrdSize = DB.GetDataTable(sql2);
                if (seOrdSize.Rows.Count>0&&seOrdSize.Rows[0]["UPC"]!=null&& !string.IsNullOrEmpty(seOrdSize.Rows[0]["UPC"].ToString())&& !string.IsNullOrWhiteSpace(seOrdSize.Rows[0]["UPC"].ToString()))
                {
                    string sql3 = "select * from mv_se_ord_item  where org_id='" + companyCode + "' and se_id='" + se_id + "' and se_seq='1'";
                    DataTable seOrdItem = DB.GetDataTable(sql3);
                    string artName = getArtName(DB, companyCode, seOrdItem.Rows[0]["PROD_NO"].ToString());
                    string sql4 = "insert into mes_label_m(org_id,label_type,se_id,size_no,art_no," +
                       "art_name,label_id,po_no,label_brand,label_features," +
                       "label_specifications,label_qty,insert_date,last_date,barcode_type," +
                       "last_user,grt_user,grt_dept,status)" +
                       "values('" + companyCode + "','A','" + se_id + "','" + size_no + "','" + seOrdItem.Rows[0]["PROD_NO"] + "'," +
                       "'"+artName+"','" + seOrdSize.Rows[0]["UPC"].ToString() + "','" + po + "','mv_se_ord_size_upc_input','mv_se_ord_size_upc_input'," +
                       "'mv_se_ord_size_upc_input',1,to_date('" + thisTime + "','"+datetimeFormate+"'),to_date('" + thisTime + "', '"+datetimeFormate+"'),'1'," +
                       "'" + userCode + "','" + userCode + "','" + grt_dept + "','7')";
                    DB.ExecuteNonQueryOffline(sql4);
                }
            }
        }

        private void insertSjqdmsWorkDay(SJeMES_Framework_NETCore.DBHelper.DataBase DB,string companyCode,string se_id,string po,
            string se_day,string grt_dept,string grt_user,string d_dept,DateTime workDate,string routNo,
            decimal work_qty,string art_no, decimal supplement_qty,string vInOut, string thisTime,string dateFormate, string datetimeFormate)
        {
            //vInOut  must not null
            string sql = @"insert into SJQDMS_WORK_DAY(org_id,se_id,se_seq,po,se_day,
                             column1,status,grt_dept,grt_user,last_user,
                             d_dept,work_day,rout_no,work_qty,art_no,
                             supplement_qty,inout_pz,insert_date)
                             values('"+ companyCode + "','" + se_id + "','1','"+po+"',to_date('"+se_day.Substring(0,10)+ "','" + dateFormate + "')," +
                             "'U','7','"+grt_dept+"','"+ grt_user+"','"+ grt_user+"',"+
                             "'"+ d_dept + "',to_date('" + workDate.ToString(dateFormate) + "','"+ dateFormate+"'),'" + routNo+"'," + work_qty + ",'"+ art_no+"'," +
                             supplement_qty + ",'"+ vInOut+ "',to_date('" + thisTime + "','"+ datetimeFormate + "'))";
            DB.ExecuteNonQueryOffline(sql);
        }

        private void updateSjqdmsWorkDay(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id,
            string grt_dept, string grt_user, string d_dept, DateTime workDate,
            decimal v_work_qty, decimal v_supplement_qty, string vInOut, string thisTime,decimal oldSizeWorkQty,decimal oldSizeSupplementQty, string dateFormate, string datetimeFormate)
        {
            string sql = "update sjqdms_work_day set " +
                "WORK_QTY=WORK_QTY+"+(v_work_qty-oldSizeWorkQty)+"," +
                "SUPPLEMENT_QTY=SUPPLEMENT_QTY+"+(v_supplement_qty-oldSizeSupplementQty)+","+
                "LAST_USER='" + grt_user+"',"+
                "LAST_DATE=to_date('" + thisTime + "', '" + datetimeFormate + "') " +
                " WHERE org_id='" + companyCode+ "' and " + 
                " d_dept='" + d_dept+"' and " +
                " work_day=to_date('" + workDate.ToString(dateFormate) + "','" + dateFormate + "') and " +
                 " se_id='" + se_id + "' and " +
                " COLUMN1='U' and " +
                " INOUT_PZ='"+vInOut+"'";
            DB.ExecuteNonQueryOffline(sql);
        }

        private void insertSjqdmsWorkDaySize(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id, string size_no,
            string size_seq, string grt_dept, string grt_user, string d_dept, DateTime workDate,
            decimal work_qty, decimal supplement_qty, string vInOut,string thisTime,string dateFormate, string datetimeFormate)
        {
            string sql = @"insert into sjqdms_work_day_size(
                          ORG_ID,SE_ID,SE_SEQ,SIZE_NO,SIZE_SEQ,
                          STATUS,GRT_DEPT,GRT_USER,LAST_USER,D_DEPT,
                          WORK_DAY,WORK_QTY,LAST_DATE,INSERT_DATE,SUPPLEMENT_QTY,
                          COLUMN1,COLUMN2,INOUT_PZ) VALUES(
                          '" + companyCode + "','" + se_id + "','1','" + size_no + "','" + size_seq + "'," +
                          "'7','" + grt_dept + "','" + grt_user + "','" + grt_user + "'," +"'" + d_dept +
                          "',to_date('" + workDate.ToString(dateFormate) + "','"+ dateFormate+"')," + work_qty + ",to_date('" + thisTime + "', '"+datetimeFormate+"') ,to_date('" + thisTime + "', '"+datetimeFormate+"')," + supplement_qty + "," +
                          "'U','Y','" + vInOut + "')";
            DB.ExecuteNonQueryOffline(sql);
        }

        private void updateSjqdmsWorkDaySize(SJeMES_Framework_NETCore.DBHelper.DataBase DB, string companyCode, string se_id, string size_no,
            string grt_dept, string grt_user, string d_dept, DateTime workDate,
            decimal v_work_qty, decimal v_supplement_qty, string vInOut, string thisTime, decimal oldSizeWorkQty, decimal oldSizeSupplementQty, string dateFormate, string datetimeFormate)
        {
            string sql = "update sjqdms_work_day_size set " +
              "WORK_QTY=WORK_QTY+" + (v_work_qty - oldSizeWorkQty) + "," +
              "SUPPLEMENT_QTY=SUPPLEMENT_QTY+" + (v_supplement_qty - oldSizeSupplementQty) + "," +
              "LAST_USER='" + grt_user + "'," +
              "LAST_DATE=to_date('" + thisTime + "', '" + datetimeFormate + "') " +
              " WHERE org_id='" + companyCode + "' and " +
              " d_dept='" + d_dept + "' and " +
              " work_day=to_date('" + workDate.ToString(dateFormate) + "','" + dateFormate + "') and " +
              " se_id='" + se_id + "' and " +
              " size_no='" + size_no + "' and " +
              " COLUMN1='U' and " +
              " INOUT_PZ='" + vInOut + "'";
            DB.ExecuteNonQueryOffline(sql);
        }

        public DataTable Query(DataBase DB, string vSeId, string vDDept, string vArtNo, string vWrokDay, string vEndWrokDay,string vStatus, string vColumn1, string vInOut, string userId, string companyCode, string grantUserCode, string dateFormate ,string datetimeFormate)
        {
            string sql = "select se_id,art_no,po,to_char(work_day,'"+dateFormate+"') as work_day,work_qty," +
                "status,rout_no,to_char(insert_date,'"+datetimeFormate+"') as insert_date,last_user," +
                "d_dept,SUPPLEMENT_QTY,column1,INOUT_PZ " +
                "from sjqdms_work_day where 1=1 " +
                " and se_id like '%" + vSeId + "%'" +
                " and d_dept like '%" + vDDept + "%'" +
                " and art_no like '%" + vArtNo + "%'" +
                " and work_day>=to_date('" + vWrokDay + "','" + dateFormate + "')" +
                " and work_day<=to_date('" + vEndWrokDay + "','" + dateFormate + "')";
            if (!vStatus.Equals("ALL")&&int.Parse(vStatus) != (0 + 1 + 7 + 99))
            {
                sql += " and status='" + vStatus + "'";
            }
            if (!vColumn1.Equals("ALL"))
            {
                sql += " and column1='" + vColumn1 + "'";
            }
            sql += " and INOUT_PZ='" + vInOut + "'" +
                   " and ORG_ID='" + companyCode + "'";
            if (!string.IsNullOrEmpty(grantUserCode))
            {
                sql += " and grt_user in ("+grantUserCode+")";
            }
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QuerySize(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vColumn1, string vInOut, string userId, string companyCode, string dateFormate, string datetimeFormate)
        {
            string sql = "select se_id,size_no,column1,work_qty," +
               "status,insert_date,last_user," +
               "d_dept,SUPPLEMENT_QTY,INOUT_PZ,finish_qty " +
               "from sjqdms_work_day_size where 1=1 ";
            sql += " and se_id='" + vSeId + "'";
            sql += " and d_dept='" + vDDept + "'";
            sql += " and work_day=to_date('" + vWrokDay.Substring(0,10) + "','"+dateFormate+"')";
            //sql += " and work_day=to_date('" + vWrokDay.Substring(0, vWrokDay.IndexOf(":") - 2) + "','" + dateFormate + "')";
            sql += " and column1='" + vColumn1 + "'";
            sql += " and INOUT_PZ='" + vInOut + "'";
            sql += " and ORG_ID='" + companyCode + "'";
            sql += "order by size_seq";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public void UpdateStatus(DataBase DB, DataTable dt,string vStatus, string userId, string companyCode, string dateFormate, string datetimeFormate)
        {
            foreach (DataRow row in dt.Rows)
            {
                string thisTime = DateTime.Now.ToString(dateFormate+" HH:mm:ss");//here will be a bug
                string vSeId = row[0].ToString();
                string vWrokDay = row[1].ToString();
                string vDDept = row[2].ToString();
                string vColumn1 = row[3].ToString();
                string vInOut = row[4].ToString();

                updateSjqdmsWorkDayStatus(DB, vSeId, vDDept, vWrokDay,
                         vColumn1, vInOut, vStatus,
                         userId, companyCode, thisTime, dateFormate, datetimeFormate);
                updateSjqdmsWorkDaySizeStatus(DB, vSeId, vDDept, vWrokDay,
                             vColumn1, vInOut, vStatus,
                             userId, companyCode, thisTime , dateFormate, datetimeFormate);
            }
        }

        public void updateSjqdmsWorkDaySizeStatus(DataBase DB, string vSeId, string vDDept, string vWrokDay, string vColumn1, string vInOut, string vStatus, string userId, string companyCode, string thisTime, string dateFormate, string datetimeFormate)
        {
            //.Substring(0, vWrokDay.LastIndexOf(" ")) 
            string sql = "update sjqdms_work_day_size set " +
             "status=" + vStatus + "," +
             "LAST_USER='" + userId + "'," +
             "LAST_DATE=to_date('" + thisTime + "', '" + datetimeFormate + "') " +
             " WHERE org_id='" + companyCode + "' and " +
             " d_dept='" + vDDept + "' and " +
             " work_day=to_date('" + vWrokDay+ "','" + dateFormate + "') and " +
             " se_id='" + vSeId + "' and " +
             " COLUMN1='" + vColumn1 + "' and " +
             " INOUT_PZ='" + vInOut + "'";
            DB.ExecuteNonQueryOffline(sql);
        }

        public void updateSjqdmsWorkDayStatus(DataBase DB, string vSeId, string vDDept, string vWrokDay, 
            string vColumn1, string vInOut, string vStatus, string userId, string companyCode, string thisTime, string dateFormate, string  datetimeFormate)
        {
            int str = vWrokDay.LastIndexOf(" ");//???-1 what  ??? 2020/08/13 DATE, NOT DATETIME change it
            //.Substring(0, vWrokDay.LastIndexOf(" "))
            string sql = "update sjqdms_work_day set " +
             "status=" + vStatus + "," +
             "LAST_USER='" + userId + "'," +
             "LAST_DATE=to_date('" + thisTime + "', '" + datetimeFormate + "') " +
             " WHERE org_id='" + companyCode + "' and " +
             " d_dept='" + vDDept + "' and " +
             " work_day=to_date('" + vWrokDay + "','" + dateFormate + "') and " +
             " se_id='" + vSeId + "' and " +
             " COLUMN1='"+vColumn1+"' and " +
             " INOUT_PZ='" + vInOut + "'";
            DB.ExecuteNonQueryOffline(sql);
        }

        public string getArtName(DataBase DB, string companyCode,string artNo)
        {
            string sql = "select name_t from mv_rd_prod where org_id='"+companyCode+"' and prod_no='"+artNo+"'";
            return DB.GetString(sql);
        }
    }
}
