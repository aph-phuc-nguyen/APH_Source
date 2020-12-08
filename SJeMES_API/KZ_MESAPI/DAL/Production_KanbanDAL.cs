using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    class Production_KanbanDAL
    {
        public DataTable TabPage8_Query(DataBase DB, string userCode, string companyCode, string today, string deptNo, string dateFormat)
        {
            //string today = DateTime.Now.ToShortDateString();
            today = DateTime.Parse(today).ToString(dateFormat); //add 28/10/2020 by Giap
            string tomorrow = DateTime.Parse(today).AddDays(1).ToString(dateFormat);//modify 28/10/2020 by Giap .ToShortDateString();
            DateTime nowDate = DateTime.Now;
            DataTable dataTable = new DataTable();

            //从前端传过来
            //string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='" + userCode + "'";
            //string deptNo = DB.GetString(sqlDept);

            string workHoursSql = "select work_hours from mes_workinghours_01 where d_dept='" + deptNo + "' and  WORK_DAY=to_date('" + today + "','" + dateFormat + "')";//modify 28/10/2020 by Giap 'yyyy/mm/dd')";
            decimal workHours = DB.GetDecimal(workHoursSql);
            string sqlDeptName = "select DEPARTMENT_NAME from base005m where DEPARTMENT_CODE='" + deptNo + "'";
            string deptName = DB.GetString(sqlDeptName);

            string sqlWorkTarget = "select WORK_QTY from SJQDMS_WORKTARGET where D_DEPT='" + deptNo + "' and WORK_DAY=to_date('" + today + "','" + dateFormat + "')";//modify 28/10/2020 by Giap 'yyyy/mm/dd')";
            int workTarget = DB.GetInt16(sqlWorkTarget);
            decimal HourTargetQty = workTarget / workHours;

            string strSQL = "SELECT * FROM SJQDMS_MPBA WHERE BA008 = '" + deptNo + "' AND BA004>0 AND BA005>0 AND ROWNUM=1 ORDER BY BA002 DESC";
            var info = DB.GetDataTable(strSQL);
            decimal ba = 0;
            if (info.Rows.Count > 0)
            {
                ba = Convert.ToDecimal(info.Rows[0]["ba005"].ToString()) * 5 / Convert.ToDecimal(info.Rows[0]["ba004"].ToString());
            }

            decimal sumQty = 0;
            //今天扫描的数量
            string sqlDayQty = "select nvl(sum(LABEL_QTY),0) from MES_LABEL_D where scan_detpt='" + deptNo + "' and inout_pz='OUT' and SCAN_DATE>=to_date('" + today + "','" + dateFormat + "') " +//modify 28/10/2020 by Giap 'yyyy/mm/dd') "
            "and SCAN_DATE<=to_date('" + tomorrow + "','" + dateFormat + "')";//modify 28/10/2020 by Giap 'yyyy/mm/dd')";
            sumQty = DB.GetDecimal(sqlDayQty);

            //当前小时扫描的数量
            string sqlHourQty = "select LABEL_QTY,to_char(SCAN_DATE,'HH24') as SCAN_DATE from MES_LABEL_D where scan_detpt='" + deptNo + "' and inout_pz='OUT' and SCAN_DATE>=to_date('" + today + "','" + dateFormat + "') " +//modify 28/10/2020 by Giap 'yyyy/mm/dd')";
            "and SCAN_DATE<=to_date('" + tomorrow + "','" + dateFormat + "')";//modify 28/10/2020 by Giap 'yyyy/mm/dd')";
            DataTable table = DB.GetDataTable(sqlHourQty);


            decimal hourQty = 0;
            string timeSql = "select nvl(f_hour,0) as f_hour,nvl(t_hour,0) as t_hour from mes_workinghours_02 where d_dept='" + deptNo + "' and work_day=to_date('" + today + "','" + dateFormat + "') order by t_hour desc"; //modify 28/10/2020 by Giap 'yyyy/mm/dd')";
            string totalTimeSql = "select nvl(work_hours,0) as  work_hours  from mes_workinghours_01 where d_dept='" + deptNo + "' and work_day=to_date('" + today + "','" + dateFormat + "')";//modify 28/10/2020 by Giap 'yyyy/mm/dd'

            DataTable timeTable = DB.GetDataTable(timeSql);
            decimal totalHours = DB.GetDecimal(totalTimeSql);
            //默认
            string amBeginTime = "0:00";
            string amEndTime = "0:00";
            string pmBeginTime = "0:00";
            string pmEndTime = "0:00";
            double endTime = 0;
            double beginTime = 0;
            for (int i = 0; i < timeTable.Rows.Count; i++)
            {
                if (i == 0)
                {
                    pmEndTime = timeTable.Rows[i]["t_hour"].ToString();
                    pmBeginTime = timeTable.Rows[i]["f_hour"].ToString();
                }
                amBeginTime = timeTable.Rows[i]["f_hour"].ToString();
                amEndTime = timeTable.Rows[i]["t_hour"].ToString();
            }

            double time1 = Convert.ToDateTime(amBeginTime).TimeOfDay.TotalSeconds;
            double time2 = Convert.ToDateTime(amEndTime).TimeOfDay.TotalSeconds;
            double time3 = Convert.ToDateTime(pmBeginTime).TimeOfDay.TotalSeconds;
            double time4 = Convert.ToDateTime(pmEndTime).TimeOfDay.TotalSeconds;

            beginTime = time1 < time3 ? time1 : time3;
            endTime = time2 < time4 ? time2 : time4;
            string tempAm = amEndTime.LastIndexOf(":") > 0 ? amEndTime.Split(':')[0] : amEndTime;
            string tempPm = pmEndTime.LastIndexOf(":") > 0 ? pmEndTime.Split(':')[0] : pmEndTime;
            int tempScan = Convert.ToInt16(tempAm) > Convert.ToInt16(tempPm) ? Convert.ToInt16(tempAm) : Convert.ToInt16(tempPm);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (DateTime.Parse(today) < DateTime.Parse(nowDate.ToShortDateString()))
                {
                    if (Convert.ToInt16(table.Rows[i]["SCAN_DATE"].ToString()) == tempScan)
                    {
                        hourQty += Convert.ToInt16(table.Rows[i]["LABEL_QTY"].ToString());
                    }
                }
                else
                {
                    if (Convert.ToInt16(table.Rows[i]["SCAN_DATE"].ToString()) == nowDate.Hour)
                    {
                        hourQty += Convert.ToInt16(table.Rows[i]["LABEL_QTY"].ToString());
                    }
                }

            }

            decimal ration = 0;
            decimal presentQty = 0;
            double nowTime = System.DateTime.Now.TimeOfDay.TotalSeconds;
            //today是传过来的参数



            if (DateTime.Parse(today) < DateTime.Parse(nowDate.ToShortDateString()))
            {
                ration = sumQty / workTarget * 100;
                presentQty = workTarget;
            }
            else
            {
                //if (workTarget > 0 && ((nowTime - beginTime)) > 0)
                //{
                //    double totalUserTime = (nowTime - beginTime) * 3600;
                //    double needDeductionTime = 0;
                //    if (nowTime / 3600 > time2 && nowTime / 3600 < time3)
                //    {
                //        needDeductionTime = nowTime - time2 * 3600;
                //    }
                //    if (nowTime > time3 && time2 != 0)
                //    {
                //        needDeductionTime = time3 * 3600 - time2 * 3600;
                //    }
                //    if (nowTime / 3600 <= endTime)
                //    {
                //        ration = sumQty / Convert.ToDecimal(totalUserTime - needDeductionTime) / (workTarget / totalHours / 60 / 60) * 100;
                //        presentQty = Convert.ToDecimal(totalUserTime - needDeductionTime) / totalHours / 60 / 60 * workTarget;
                //    }
                //    else
                //    {
                //        ration = sumQty / workTarget * 100;
                //        presentQty = workTarget;
                //    }
                //}
                string sql = "select GF_WORKINGHOURS_PERC('" + deptNo + "') from dual";
                decimal rationHours = DB.GetDecimal(sql);
                ration = decimal.Round((sumQty * 100 * 100) / (workTarget * rationHours), 1);
                presentQty = decimal.Round(workTarget * rationHours / 100, 0);
            }

            dataTable.Columns.Add("Title");
            dataTable.Columns.Add("Target");
            dataTable.Columns.Add("SumQty");
            dataTable.Columns.Add("HourTargetQty");
            dataTable.Columns.Add("HourQty");
            dataTable.Columns.Add("BA");
            dataTable.Columns.Add("Ration");
            dataTable.Columns.Add("PresentQty");
            DataRow dr = dataTable.NewRow();
            dr[0] = deptName;
            dr[1] = workTarget;
            dr[2] = sumQty;
            dr[3] = HourTargetQty;
            dr[4] = hourQty;
            dr[5] = ba;
            dr[6] = ration;
            dr[7] = presentQty;
            dataTable.Rows.Add(dr);
            return dataTable;
        }

        public DataTable TabPage2_Query(DataBase dB, string userCode, string companyCode, string date, string line)
        {
            DataTable dataTable = new DataTable();
            return dataTable;
        }

        private bool Vali(string am_from, string am_to)
        {
            //星期一 am
            if (string.IsNullOrEmpty(am_from) && string.IsNullOrEmpty(am_to))
            {
                return false;
            }
            //星期一 am
            if (string.IsNullOrEmpty(am_from) && !string.IsNullOrEmpty(am_to))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(am_from) && string.IsNullOrEmpty(am_to))
            {
                return false;
            }
            return true;
        }

        private string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Hours.ToString() + ":" + ts.Minutes.ToString();
            }
            catch
            {
            }
            return dateDiff;
        }

        public DataTable TabPage1_Query(DataBase DB, string userCode, string companyCode, string today, string deptNo, string dateFomart)
        {
            //从客户端传过来
            //string today = DateTime.Now.ToShortDateString();
            string tomorrow = DateTime.Parse(today).AddDays(1).ToString(dateFomart);//modify 28/10/2020 by Giap .ToShortDateString();
            today = DateTime.Parse(today).ToString(dateFomart); //add 28/10/2020 by Giap
            DateTime nowDate = DateTime.Now;
            DataTable dataTable = new DataTable();
            //从客户端传过来
            //string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='" + userCode + "'";
            //string deptNo = DB.GetString(sqlDept);
            string sqlArtNo = @"select ART_NO from (
                                   SELECT ART_NO as ART_NO from mes_label_d where scan_detpt='" + deptNo + @"' and " +// modify 28/10/2020 by giap "scan_date>=to_date('" + today + @"','yyyy/mm/dd') and scan_date<to_date('" + tomorrow +@"','yyyy/mm/dd') and inout_pz='OUT'
                                   "scan_date>=to_date('" + today + @"','" + dateFomart + "') and scan_date<to_date('" + tomorrow + @"','" + dateFomart + "') and inout_pz='OUT'" +  /*add 28/10/2020 by giap*/
                                  "order by insert_date desc) where rownum=1";
            string artNo = DB.GetString(sqlArtNo);
            string sqlModelName = "select GF_SHOE_NAME_MG(" + companyCode + ",shoe_no,'T') from mv_rd_prod where prod_no='" + artNo + "'";
            string moldelName = DB.GetString(sqlModelName);
            string sqlWorkTarget = "select WORK_QTY from SJQDMS_WORKTARGET where D_DEPT='" + deptNo + "' and WORK_DAY=to_date('" + today + "','" + dateFomart + "')"; // modify 28/10/2020 by giap WORK_DAY=to_date('" + today + "','yyyy/mm/dd')";
            int workTarget = DB.GetInt16(sqlWorkTarget);

            string sql = @"select JOCKEY_QTY,UDF01 from mes_dept_manpower where d_dept='" + deptNo + @"' and " +// modify 28/10/2020 by giap begin_day<=to_date('" + today + "','yyyy/mm/dd') and (end_day>=to_date('" + today + "','yyyy/mm/dd') or end_day is null)";
                                               "begin_day <= to_date('" + today + "', '" + dateFomart + "') and(end_day >= to_date('" + today + "', '" + dateFomart + "') or end_day is null)"; //add 28/10/2020 by giap
            DataTable dataTable1 = DB.GetDataTable(sql);

            dataTable.Columns.Add("ModelName");
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Target");
            dataTable.Columns.Add("OperatorNo");
            dataTable.Columns.Add("WaterSpiderNo");
            dataTable.Columns.Add("ModelTHT");
            dataTable.Columns.Add("TaktTime");
            dataTable.Columns.Add("ART");

            DataRow dr = dataTable.NewRow();
            dr[0] = moldelName;
            dr[1] = today;
            dr[2] = workTarget;
            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                dr[3] = dataTable1.Rows[i][0];
                dr[4] = dataTable1.Rows[i][1];
            }
            dr[5] = "";
            dr[6] = "";
            dr["ART"] = artNo;
            dataTable.Rows.Add(dr);
            return dataTable;
        }


        public DataTable TabPage6_Query(DataBase DB, string userCode, string companyCode, string today, string deptNo, DataTable clinetTimeParams, string dateFormat, string dateTimeFormat)
        {
            //从客户端传过来
            //string today = DateTime.Now.ToShortDateString();
            today = DateTime.Parse(today).ToString(dateFormat);//add 28/10/2020 by giap
            string tomorrow = DateTime.Parse(today).AddDays(1).ToString(dateFormat);//modify 28/10/2020 by giap .ToShortDateString();
            DateTime nowDate = DateTime.Now;
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("moldelName");
            dataTable.Columns.Add("workQty");
            dataTable.Columns.Add("Target");
            dataTable.Columns.Add("OperatorNo");
            dataTable.Columns.Add("WaterSpiderNo");
            dataTable.Columns.Add("amfrom");
            dataTable.Columns.Add("amto");
            dataTable.Columns.Add("pmfrom");
            dataTable.Columns.Add("pmto");
            dataTable.Columns.Add("totalHours");

            string sqlWorkTarget = "select WORK_QTY from SJQDMS_WORKTARGET where D_DEPT='" + deptNo + "' and WORK_DAY=to_date('" + today + "','" + dateFormat + "')";//modify 28/10/2020 by giap yyyy/mm/dd
            int workTarget = DB.GetInt16(sqlWorkTarget);

            string sql = @"select JOCKEY_QTY,UDF01 from mes_dept_manpower where d_dept='" + deptNo + @"' and begin_day<=to_date('" + today + "','" + dateFormat + "') and (end_day>=to_date('" + today + "','" + dateFormat + "') or end_day is null)";
            DataTable dataTable1 = DB.GetDataTable(sql);

            string timeSql = "select datetype,nvl(f_hour,0) as f_hour,nvl(t_hour,0) as t_hour from mes_workinghours_02 where d_dept='" + deptNo + "' and work_day=to_date('" + today + "','" + dateFormat + "') order by t_hour desc";
            string totalTimeSql = "select nvl(work_hours,0) as  work_hours  from mes_workinghours_01 where d_dept='" + deptNo + "' and work_day=to_date('" + today + "','" + dateFormat + "')";

            DataTable timeTable = DB.GetDataTable(timeSql);
            decimal totalHours = DB.GetDecimal(totalTimeSql);

            for (int i = 0; i < clinetTimeParams.Rows.Count; i++)
            {
                string from = today + " " + clinetTimeParams.Rows[i][0].ToString().Split('~')[0].ToString();
                string to = today + " " + clinetTimeParams.Rows[i][0].ToString().Split('~')[1].ToString();
                string sqlArtNo = @"select ART_NO from (
                                    SELECT ART_NO from mes_label_d where scan_detpt='" + deptNo +
                                   @"' and insert_date>=to_date('" + from + @"','" + dateTimeFormat + "') and scan_date<to_date('" + to + @"','" + dateTimeFormat + "') and inout_pz='OUT'" + //modify 28/10/2020 by giap yyyy/mm/dd HH24:MI
                                  " order by insert_date desc) where rownum=1";
                string artNo = DB.GetString(sqlArtNo);
                string sqlModelName = "select GF_SHOE_NAME_MG(" + companyCode + ",shoe_no,'T') from mv_rd_prod where prod_no='" + artNo + "'";
                string moldelName = DB.GetString(sqlModelName);

                //string sqlModelName = @"select prod_name from (
                //                   SELECT GF_PROD_NAME_MG(ORG_ID,ART_NO,'T') as prod_name from mes_label_d where scan_detpt='" + deptNo +
                //                   @"' and insert_date>=to_date('" + from + @"','yyyy/mm/dd HH24:MI') and scan_date<to_date('" + to + @"','yyyy/mm/dd HH24:MI') and inout_pz='OUT'
                //                   order by insert_date desc) where rownum=1";
                //string moldelName = DB.GetString(sqlModelName);
                string workQtySql = @"SELECT nvl(sum(label_qty),0)  as work_qty from mes_label_d where scan_detpt='" + deptNo +
                                    @"' and insert_date>=to_date('" + from + @"','" + dateTimeFormat + "') and scan_date<to_date('" + to + @"','" + dateTimeFormat + "')  and inout_pz='OUT'";//modify 28/10/2020 by giap yyyy/mm/dd HH24:MI
                decimal workQty = DB.GetDecimal(workQtySql);
                DataRow dr = dataTable.NewRow();
                dr[0] = moldelName;
                dr[1] = workQty;
                dr[2] = workTarget;
                for (int j = 0; j < dataTable1.Rows.Count; j++)
                {
                    dr[3] = dataTable1.Rows[j][0];
                    dr[4] = dataTable1.Rows[j][1];
                }
                for (int j = 0; j < timeTable.Rows.Count; j++)
                {
                    if (timeTable.Rows[j]["datetype"].ToString().Equals("am"))
                    {
                        dr[5] = timeTable.Rows[j][1];
                        dr[6] = timeTable.Rows[j][2];
                    }
                    if (timeTable.Rows[j]["datetype"].ToString().Equals("pm"))
                    {
                        dr[7] = timeTable.Rows[j][1];
                        dr[8] = timeTable.Rows[j][2];
                    }
                }
                dr[9] = totalHours;
                dataTable.Rows.Add(dr);
            }
            return dataTable;
        }


        public DataTable TabPage6_Query_ScanDetail(DataBase DB, string userCode, string companyCode, string today, string deptNo, string dateFormat)
        {
            //从客户端传过来
            string tomorrow = DateTime.Parse(today).AddDays(1).ToString(dateFormat);//modify 28/10/2020 by GIAP .ToShortDateString();
            today = DateTime.Parse(today).ToString(dateFormat);//add 28/10/2020 GIAP
                                                               //modify 28/10/2020 by giap yyyy/mm/dd
            string sql = string.Format(@"select to_char(insert_date,'" + dateFormat + "') as insert_date,to_char(insert_date,'HH24:MI:ss') as insert_time,GF_PROD_NAME_MG(" + companyCode + ",a.art_no,'T')  as prodName from mes_label_d  a where scan_detpt='{0}' and  scan_date between to_date('{1}','" + dateFormat + "') and  to_date('{2}','" + dateFormat + "') and inout_pz='OUT'", deptNo, today, tomorrow);
            //string sql = string.Format(@"select insert_date,GF_PROD_NAME_MG(" + companyCode + ",a.art_no,'T')  as prodName from mes_label_d  a where scan_detpt='{0}' and  insert_date between to_date('{1}','yyyy/mm/dd') and  to_date('{2}','yyyy/mm/dd') and inout_pz='OUT'", deptNo, today, tomorrow);
            DataTable dataTable = DB.GetDataTable(sql);
            return dataTable;
        }

        /// <summary>
        /// 目标产量、操作工数量、水蜘蛛数量、工作时间
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="userCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="today"></param>
        /// <param name="deptNo"></param>
        /// <returns></returns>
        public DataTable TabPage6_Query_OtherDetail(DataBase DB, string userCode, string companyCode, string today, string deptNo, string dateFormat)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Target");
            dataTable.Columns.Add("OperatorNo");
            dataTable.Columns.Add("WaterSpiderNo");
            dataTable.Columns.Add("totalHours");
            dataTable.Columns.Add("amfrom");
            dataTable.Columns.Add("amto");
            dataTable.Columns.Add("pmfrom");
            dataTable.Columns.Add("pmto");
            DataRow dr = dataTable.NewRow();
            today = DateTime.Parse(today).ToString(dateFormat);//add 28/10/2020 by giap
            //modify 28/10/2020 by giap yyyy/mm/dd
            string sqlWorkTarget = "select WORK_QTY from SJQDMS_WORKTARGET where D_DEPT='" + deptNo + "' and WORK_DAY=to_date('" + today + "','" + dateFormat + "')";
            int workTarget = DB.GetInt16(sqlWorkTarget);

            string sql = @"select JOCKEY_QTY,UDF01 from mes_dept_manpower where d_dept='" + deptNo + @"' and begin_day<=to_date('" + today + "','" + dateFormat + "') and (end_day>=to_date('" + today + "','" + dateFormat + "') or end_day is null)";
            DataTable dataTable1 = DB.GetDataTable(sql);


            string timeSql = "select datetype,nvl(f_hour,0) as f_hour,nvl(t_hour,0) as t_hour from mes_workinghours_02 where d_dept='" + deptNo + "' and work_day=to_date('" + today + "','" + dateFormat + "') order by t_hour desc";
            string totalTimeSql = "select nvl(work_hours,0) as  work_hours  from mes_workinghours_01 where d_dept='" + deptNo + "' and work_day=to_date('" + today + "','" + dateFormat + "')";

            DataTable timeTable = DB.GetDataTable(timeSql);
            decimal totalHours = DB.GetDecimal(totalTimeSql);

            dr[0] = workTarget;
            for (int j = 0; j < dataTable1.Rows.Count; j++)
            {
                dr[1] = dataTable1.Rows[j][0];
                dr[2] = dataTable1.Rows[j][1];
            }
            dr[3] = totalHours;

            for (int j = 0; j < timeTable.Rows.Count; j++)
            {
                if (timeTable.Rows[j]["datetype"].ToString().Equals("am"))
                {
                    dr[4] = timeTable.Rows[j][1];
                    dr[5] = timeTable.Rows[j][2];
                }
                if (timeTable.Rows[j]["datetype"].ToString().Equals("pm"))
                {
                    dr[6] = timeTable.Rows[j][1];
                    dr[7] = timeTable.Rows[j][2];
                }
            }

            dataTable.Rows.Add(dr);
            return dataTable;
        }
    }
}
