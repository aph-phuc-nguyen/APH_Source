using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class ProductionEfficiencyDAL
    {
        /// <summary>
        /// -------该功能主要执行验证上传的资料-------
        /// 1:验证是否有改组别资料
        /// </summary>
        public int ValiUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string companyCode)
        {
            int errorCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {               
                Boolean v_status = false;
                string d_dept = dt.Rows[i][0].ToString();
                string sql1 = @"select * from BASE005M " +
                    "where department_code='" + d_dept + "'";
                System.Data.DataTable base005M = DB.GetDataTable(sql1);
                if (base005M.Rows.Count <= 0)
                {
                    v_status = true;
                }         
                if (v_status == true)
                {
                    errorCount++;
                }
            }
            return errorCount;
        }

        public void UpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string companyCode,string dateFormat)
        {
            string sqlDept = string.Format("select STAFF_DEPARTMENT from HR001M where STAFF_NO = '{0}'",userId);
            string grt_dept = DB.GetString(sqlDept);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string d_dept = dt.Rows[i][0].ToString();
                decimal operator_qty = dt.Rows[i][1] == null ? 0 : Decimal.Parse(dt.Rows[i][1].ToString());
                decimal multi_skill_qty = dt.Rows[i][2] == null ? 0 : Decimal.Parse(dt.Rows[i][2].ToString());
                decimal all_rounder_qty = dt.Rows[i][3] == null ? 0 : Decimal.Parse(dt.Rows[i][3].ToString());
                decimal mobile_worker_qty = dt.Rows[i][4] == null ? 0 : Decimal.Parse(dt.Rows[i][4].ToString());
                DateTime workDate = DateTime.Parse(dt.Rows[i][5].ToString());
                string sql1 =string.Format(@"select * from BASE005M where department_code='{0}'",d_dept);
                System.Data.DataTable base005M = DB.GetDataTable(sql1);
                if (base005M.Rows.Count > 0)
                {
                    string sql2 =string.Format(@"select * from MES_DEPT_MANPOWER where d_dept = '{0}' and BEGIN_DAY = to_date('{1}','{2}')", d_dept,workDate.ToString(dateFormat),dateFormat);//" + d_dept + " " + workDate.ToShortDateString() + "
                    System.Data.DataTable isSame = DB.GetDataTable(sql2);
                    //部门、开始时间相同的，就update
                    if (isSame.Rows.Count > 0)
                    {
                        string update = string.Format(@"update MES_DEPT_MANPOWER set JOCKEY_QTY ={0},PLURIPOTENT_WORKER ={1},OMNIPOTENT_WORKER ={2},UDF01 = {3} 
                                                       where d_dept = '{4}' and BEGIN_DAY = to_date('{5}','{6}')", 
                                                       operator_qty, multi_skill_qty, all_rounder_qty, mobile_worker_qty,d_dept,workDate.ToString(dateFormat),dateFormat); //" + operator_qty + 
                        DB.ExecuteNonQueryOffline(update);//" + workDate.ToShortDateString() + "
                    }
                    else
                    {
                        /**
                         * 修改小于导入时间中最大的开始时间的结束时间为导入的时间
                         * update MES_DEPT_MANPOWER set END_DAY = to_date('2020-05-05','yyyy-mm-dd') where D_DEPT = '1111' and BEGIN_DAY =
                            (select MAX(BEGIN_DAY) as BEGIN_DAY from MES_DEPT_MANPOWER WHERE D_DEPT = '1111' and BEGIN_DAY<to_date('2020-05-05','yyyy-mm-dd') GROUP BY D_DEPT);
                         */
                        string sql3 = string.Format(@"update MES_DEPT_MANPOWER set END_DAY = to_date('{0}','{1}') where d_dept = '{2}' 
                                         and BEGIN_DAY = (select MAX(BEGIN_DAY) as BEGIN_DAY from MES_DEPT_MANPOWER WHERE D_DEPT = '{3}'
                                         and BEGIN_DAY < to_date('{4}','{5}') GROUP BY D_DEPT)", workDate.ToString(dateFormat),dateFormat,d_dept, d_dept, workDate.ToString(dateFormat), dateFormat);//" + d_dept + "
                        DB.ExecuteNonQueryOffline(sql3);

                        /**
                         * 插入导入的数据
                         * insert into MES_DEPT_MANPOWER(D_DEPT,BEGIN_DAY,JOCKEY_QTY,PLURIPOTENT_WORKER,OMNIPOTENT_WORKER,UDF01) VALUES('','','','','','')
                         */
                        string insert =string.Format(@"insert into MES_DEPT_MANPOWER(D_DEPT,BEGIN_DAY,JOCKEY_QTY,PLURIPOTENT_WORKER,OMNIPOTENT_WORKER,UDF01) 
                                       VALUES('{0}',to_date('{1}','{2}'),{3},{4},{5},{6})",
                                       d_dept, workDate.ToString(dateFormat),dateFormat, operator_qty, multi_skill_qty,all_rounder_qty, mobile_worker_qty);
                        DB.ExecuteNonQueryOffline(insert);

                        /**
                         * 查找大于导入时间中最小的开始时间
                         * select MIN(BEGIN_DAY) as BEGIN_DAY from MES_DEPT_MANPOWER WHERE D_DEPT = '1111' and BEGIN_DAY > to_date('2020-05-04','yyyy-mm-dd') GROUP BY D_DEPT
                         */
                        string sql4 = string.Format( @"select MIN(BEGIN_DAY) as BEGIN_DAY from MES_DEPT_MANPOWER WHERE D_DEPT = '{0}'and 
                                      BEGIN_DAY > to_date('{1}','{2}') GROUP BY D_DEPT", d_dept, workDate.ToString(dateFormat),dateFormat);
                        System.Data.DataTable hasBig = DB.GetDataTable(sql4);
                        if (hasBig.Rows.Count > 0)
                        {
                            string sql5 =string.Format(@"update MES_DEPT_MANPOWER 
                                 set END_DAY = (select MIN(BEGIN_DAY) as BEGIN_DAY from MES_DEPT_MANPOWER WHERE D_DEPT = '{0}'
                                 and BEGIN_DAY > to_date('{1}','{2}') GROUP BY D_DEPT) where D_DEPT = '{3}' and BEGIN_DAY = to_date('{4}','{5}')",
                                 d_dept,workDate.ToString(dateFormat),dateFormat, d_dept,workDate.ToString(dateFormat), dateFormat);
                            DB.ExecuteNonQueryOffline(sql5);
                        }
                    }
                }
            }
        }

        public DataTable QueryNumber(DataBase DB, string CompanyCode, string vDept, string vWrokDay,string dateFormat)
        {
            string sql_date = "";
            if (vWrokDay != "")
            {
                DateTime workDate = DateTime.Parse(vWrokDay);
                sql_date =string.IsNullOrEmpty(workDate.ToString()) ? "" : " and BEGIN_DAY <= to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+ "') and (end_day >to_date('" + workDate.ToString(dateFormat) + "','"+dateFormat+"') or end_day is null) ";          
            }
            string sql_dept = string.IsNullOrEmpty(vDept) ? "" : " and D_DEPT like '" + vDept + "%'";
            string sql = "select d_dept,JOCKEY_QTY,PLURIPOTENT_WORKER,OMNIPOTENT_WORKER,UDF01,to_char(BEGIN_DAY,'" + dateFormat + "') as BEGIN_DAY , to_char(END_DAY,'" + dateFormat + "') as END_DAY from MES_DEPT_MANPOWER where 1=1 " + sql_dept + sql_date;
            sql += " order by d_dept,begin_day,end_day,insert_date";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable QueryWorkDay(DataBase DB,DateTime date,int numberday, string dateFormat)
        {
            string sql = "select to_char(work_day,'"+dateFormat+"')as work_day from (select distinct work_day from mes_workinghours_01 where work_day <= to_date('"+date.ToString(dateFormat)+"','"+dateFormat+"') order by work_day desc ) where rownum <="+numberday;
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
       
    }
}
