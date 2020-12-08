using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class WorkingHoursDAL
    {
        public DataTable LoadSeDept(DataBase DB, string vDDept, string userId, string companyCode)
        {
            string sql = "select DEPARTMENT_CODE,DEPARTMENT_NAME from base005m where DEPARTMENT_CODE!='" + vDDept + "'";
            return DB.GetDataTable(sql);
        }

        public DataTable Query(DataBase DB, string orgNo, string plantNo, string deptNo, string routNo, string userCode, string companyCode)
        {
            string sql = @"select 'N' as isCheck,department_code,department_name from base005m d,sjqdms_orginfo m where 
                           d.udf05 = m.code  and d.udf06 = 'Y'  and m.COMPANY LIKE '"+orgNo+ @"%'
                           and m.code like '"+plantNo+ @"%' and d.department_code like '"+deptNo+ @"%' and d.udf01 like '" + routNo + @"%'
                           ";
            return DB.GetDataTable(sql);
        }
        public DataTable GetWorkingHoursData(DataBase DB, string orgNo, string plantNo, string deptNo, string routNo, string workDate,string userCode, string companyCode,string dateFormat)
        {
            string sql = string.Format(@"select c.department_code as department_code,c.department_name as department_name,to_char(a.work_day,'{0}') as work_day,b.JOCKEY_QTY as JOCKEY_QTY,b.PLURIPOTENT_WORKER as PLURIPOTENT_WORKER,b.OMNIPOTENT_WORKER as OMNIPOTENT_WORKER,b.UDF01 as UDF01,
                           a.WORK_HOURS as WORK_HOURS,a.TRANIN_HOURS as TRANIN_HOURS,a.TRANOUT_HOURS as TRANOUT_HOURS,c.udf01 as rout_no
                           from  sjqdms_orginfo m,base005m c, mes_workinghours_01 a
                           left join mes_dept_manpower  b on a.d_dept=b.d_dept and a.work_day>=b.begin_day and (a.work_day<b.end_day or b.end_day is null)
                           where c.department_code=a.d_dept and c.udf05 = m.code and m.MFG = 'Y'  and c.udf06 = 'Y'   and m.COMPANY LIKE '{1}%'
                           and m.code like '{2}%' and c.department_code like '{3}%' and c.udf01 like '{4}%'",dateFormat, orgNo, plantNo, deptNo, routNo);//" + orgNo + @" " + plantNo +  " + deptNo + @" " + routNo + @"
            if (!string.IsNullOrWhiteSpace(workDate))
            {
                sql += string.Format(@" and to_char(a.work_day,'{0}')='{1}'",dateFormat,workDate);
            }
            sql+=" order by c.department_code,a.work_day";
            return DB.GetDataTable(sql);
        }
        
        public void Save(DataBase DB, DataTable dataTable, string vfrom, string vto, string mon_am_from, string mon_am_to, string mon_pm_from, string mon_pm_to, string tue_am_from, string tue_am_to, string tue_pm_from, string tue_pm_to, string wed_am_from, string wed_am_to, string wed_pm_from, string wed_pm_to, string thu_am_from, string thu_am_to, string thu_pm_from, string thu_pm_to, string fri_am_from, string fri_am_to, string fri_pm_from, string fri_pm_to, string sat_am_from, string sat_am_to, string sat_pm_from, string sat_pm_to, string sun_am_from, string sun_am_to, string sun_pm_from, string sun_pm_to, string userCode, string companyCode,string dateFormat)
        {
            //把每组的资料塞到数据库
            for(int i=0;i<dataTable.Rows.Count;i++)
            {
                string deptNo = dataTable.Rows[i][0].ToString();
                string sql = string.Format("select trunc(to_date('{0}','{1}')-to_date('{2}','{3}')) from dual", vto,dateFormat,vfrom,dateFormat);
                int cishu = DB.GetInt16(sql);
                //把每天的数据塞到数据库
                for (int j=0;j<=cishu;j++)
                {
                    DateTime  now=DateTime.Parse(vfrom).AddDays(j);
                    int week =(int)now.DayOfWeek;
                    switch (week)
                    {
                        case 0:
                            SaveWorkCalendar(DB,deptNo,now, sun_am_from,sun_am_to, sun_pm_from, sun_pm_to,userCode,companyCode, dateFormat);
                            break;
                        case 1:
                            SaveWorkCalendar(DB, deptNo, now, mon_am_from, mon_am_to, mon_pm_from, mon_pm_to, userCode, companyCode, dateFormat);
                            break;
                        case 2:
                            SaveWorkCalendar(DB, deptNo, now, tue_am_from, tue_am_to, tue_pm_from, tue_pm_to, userCode, companyCode, dateFormat);
                            break;
                        case 3:
                            SaveWorkCalendar(DB, deptNo, now, wed_am_from, wed_am_to, wed_pm_from, wed_pm_to, userCode, companyCode, dateFormat);
                            break;
                        case 4:
                            SaveWorkCalendar(DB, deptNo, now, thu_am_from, thu_am_to, thu_pm_from, thu_pm_to, userCode, companyCode, dateFormat);
                            break;
                        case 5:
                            SaveWorkCalendar(DB, deptNo, now, fri_am_from, fri_am_to, fri_pm_from, fri_pm_to, userCode, companyCode, dateFormat);
                            break;
                        case 6:
                            SaveWorkCalendar(DB, deptNo, now, sat_am_from, sat_am_to, sat_pm_from, sat_pm_to, userCode, companyCode, dateFormat);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public bool QueryWorkingHours(DataBase DB, string vNewDdept,string vWrokDay,string dateFormat)
        {
            string sql = string.Format("select * from mes_workinghours_01 where d_dept='{0}' and to_char(work_day,'{1}')='{2}'", vNewDdept,dateFormat,vWrokDay);//" + vNewDdept + "yyyy/mm/dd " + vWrokDay + "
            if (DB.GetDataTable(sql).Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void InsertMesMoveWorkingHours(DataBase DB, string vMoveNo, string vMoveDate, string vDDept, string vWrokDay, string vNewDdept, string vTransTime, string userCode, string companyCode,string dateFormat)
        {
            string sql = string.Format("insert into MES_MOVE_WORKINGHOURS(org_id,move_no,d_dept,work_day,insert_user,transin_dept,transtime)" +
                "values('{0}','{1}','{2}',to_date('{3}','{4}'),'{5}','{6}',{7})", companyCode, vMoveNo, vDDept, vWrokDay, dateFormat, userCode, vNewDdept, vTransTime); //"+userCode+" "+vNewDdept+"
            DB.ExecuteNonQuery(sql);
        }

        public void UpdateWorkingHours(DataBase DB, string vDDept, string vWrokDay, string vTransOutTime, string vTransInTime, string userCode, string companyCode,string dateFormat)
        {
            if (!string.IsNullOrEmpty(vTransInTime) ||!string.IsNullOrEmpty(vTransOutTime))
            {
                string sql1 = "update mes_workinghours_01 set ";
                if (!string.IsNullOrEmpty(vTransInTime))
                {
                    sql1+= " tranin_hours=" + vTransInTime + " ";
                    if (!string.IsNullOrEmpty(vTransOutTime))
                    {
                        sql1 +=",";
                    }
                }
                if (!string.IsNullOrEmpty(vTransOutTime))
                {
                    sql1 += " tranout_hours=" + vTransOutTime + " ";
                }
                sql1+=" where d_dept = '" + vDDept + "' and to_char(work_day,'"+ dateFormat+"')= '" + vWrokDay + "'";
                DB.ExecuteNonQuery(sql1);
            }

        }

        public string LoadMoveNo(DataBase DB)
        {
            string sql = "select SEQ_MES_MOVE_WORKINGHOURS.Nextval from dual";
            return DB.GetString(sql);
        }

        private void SaveWorkCalendar(DataBase DB, string deptNo, DateTime now, string am_from, string am_to, string pm_from, string pm_to, string userCode, string companyCode,string dateFormat)
        {
            double workHours = 0.0;
            if (Vali(am_from,am_to))
            {
                DateTime amto_date = Convert.ToDateTime(am_to);
                DateTime amfrom_date = Convert.ToDateTime(am_from);
                string h1 = DateDiff(amto_date, amfrom_date);
                workHours = double.Parse(h1.Split(':')[0]) + double.Parse(h1.Split(':')[1]) / 60;
            }   
            if (Vali(pm_to, pm_from))
            {
                DateTime pmto_date = Convert.ToDateTime(pm_to);
                DateTime pmfrom_date = Convert.ToDateTime(pm_from);
                string h2 = DateDiff(pmto_date, pmfrom_date);
                workHours += double.Parse(h2.Split(':')[0]) + double.Parse(h2.Split(':')[1]) / 60;
            }
            if (workHours>0)
            {
                string exitsSql =string.Format("select * from mes_workinghours_01 where d_dept='{0}' and work_day=to_date('{1}','{2}')", deptNo,now.ToString(dateFormat),dateFormat);//" + deptNo + " " + now.ToString(dateFormat) + 
                if (DB.GetDataTablebyline(exitsSql).Rows.Count==0)
                {
                    string sql = string.Format(@"insert into mes_workinghours_01(d_dept,work_day,work_hours,insert_user) 
                               values('{0}',to_date('{1}','{2}'),{3},'{4}')", deptNo, now.ToString(dateFormat), dateFormat, workHours, userCode);
                    DB.ExecuteNonQuery(sql);
                    if (Vali(am_from, am_to))
                    {
                        string sql2 = string.Format(@"insert into mes_workinghours_02(d_dept,work_day,datetype,f_hour,t_hour,insert_user) 
                                values('{0}',to_date('{1}','{2}'),'am','{3}','{4}','{5}')", deptNo,now.ToString(dateFormat), dateFormat, am_from, am_to,userCode);
                        DB.ExecuteNonQuery(sql2);
                    }
                    if (Vali(pm_to, pm_from))
                    {
                        string sql3 =string.Format(@"insert into mes_workinghours_02(d_dept,work_day,datetype,f_hour,t_hour,insert_user) 
                               values('{0}', to_date('{1}', '{2}'), 'pm', '{3}', '{4}', '{5}')", deptNo,now.ToString(dateFormat), dateFormat, pm_from, pm_to, userCode);//values('" + deptNo + "',to_date('" + now.ToShortDateString() + "','yyyy/mm/dd'),'pm','" + pm_from + "','" + pm_to + "','" + userCode + "')");
                        DB.ExecuteNonQuery(sql3);
                    }           
                }
                else
                {
                    string sql = string.Format("update mes_workinghours_01 set work_hours={0} where d_dept='{1}' and work_day=to_date('{2}','{3}')", workHours, deptNo,now.ToString(dateFormat),dateFormat);
                    DB.ExecuteNonQuery(sql);
                    string exitsSql2 = string.Format("select * from mes_workinghours_02 where d_dept='{0}' and work_day=to_date('{1}','{2}') and datetype='am'",deptNo,now.ToString(dateFormat), dateFormat);
                    if (DB.GetDataTablebyline(exitsSql2).Rows.Count ==0)
                    {
                        string sql2 = string.Format(@"insert into mes_workinghours_02(d_dept,work_day,datetype,f_hour,t_hour,insert_user) 
                                values('{0}',to_date('{1}','{2}'),'am','{3}','{4}','{5}')", deptNo, now.ToString(dateFormat), dateFormat, am_from, am_to, userCode);
                        DB.ExecuteNonQuery(sql2);                    
                    }
                    else
                    {
                        string sql2= string.Format("update mes_workinghours_02 set f_hour='{0}',t_hour='{1}' where d_dept='{2}' and work_day=to_date('{3}','{4}') and datetype='am'",am_from,am_to,deptNo, now.ToString(dateFormat), dateFormat);
                        DB.ExecuteNonQuery(sql2);
                    }
                    string exitsSql3 = "select * from mes_workinghours_02 where d_dept='" + deptNo + "' and work_day=to_date('" + now.ToString(dateFormat) + "','"+dateFormat+"') and datetype='pm'";
                    if (DB.GetDataTable(exitsSql3).Rows.Count == 0)
                    {
                        string sql3 = string.Format(@"insert into mes_workinghours_02(d_dept,work_day,datetype,f_hour,t_hour,insert_user) 
                              values('{0}', to_date('{1}', '{2}'), 'pm', '{3}', '{4}', '{5}')", deptNo, now.ToString(dateFormat), dateFormat, pm_from, pm_to, userCode); //values('" + deptNo + "', to_date('" + now.ToShortDateString() + "', 'yyyy/mm/dd'), 'pm', '" + pm_from + "', '" + pm_to + "', '" + userCode + "')";
                        DB.ExecuteNonQuery(sql3);
                    }
                    else
                    {
                        string sql3 = string.Format("update mes_workinghours_02 set f_hour='{0}',t_hour='{1}' where d_dept='{2}' and work_day=to_date('{3}','{4}') and datetype='pm'", pm_from, pm_to, deptNo, now.ToString(dateFormat), dateFormat);//                        //string sql3 = "update mes_workinghours_02 set f_hour='" + pm_from + "',t_hour='" + pm_to + "' where d_dept='" + deptNo + "' and work_day=to_date('" + now.ToShortDateString() + "','yyyy/mm/dd') and datetype='pm'";
                        DB.ExecuteNonQuery(sql3);
                    }
                }

            }    
        }

        private bool Vali(string am_from,string am_to)
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
                dateDiff = ts.Hours.ToString()+":"+ts.Minutes.ToString();
            }
            catch
            {
            }
            return dateDiff;
        }
    }
}
