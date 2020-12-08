using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TierMeeting.DAL
{
     public class TierMeetingDAL
    {
        #region Tier4Form
        public DataTable GetDepartmentByUser(DataBase DB, string userCode)
        {
            string sql = @"select a.STAFF_DEPARTMENT,
                               b.Department_Name,
                               COALESCE(b.udf05, '') as udf05,
                               COALESCE(b.udf07, '') as udf07
                          FROM hr001m a
                          left join base005m b
                            on a.STAFF_DEPARTMENT = b.department_code
                         where a.staff_no = '"+ userCode + "'";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetKZAPDataTable(DataBase DB, string dept, int type, int process)
        {
            string sql = "";
            switch (process)
            {
                case 4: //get at process T4
                    if (type == (int)Parameters.QueryDeptType.All) //get all
                    {
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T4 = 'P' and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    else
                    { //get by plant
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T4 = 'P' or (g_deptcode='"+dept+ @"' 
                                          and (G_T1 is null or G_T1 <> 'C') 
                                          and (G_T2 is null or G_T2 <> 'C')
                                          and (G_T3 is null or G_T3 <> 'C')
                                          and (G_T4 is null or G_T4 <> 'C'))
                                and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    break;
                case 3: //get at process T3
                    if (type == (int)Parameters.QueryDeptType.Plant) //get by plant
                    {
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T3 = 'P' or (g_deptcode='" + dept + @"' 
                                          and (G_T1 is null or G_T1 <> 'C') 
                                          and (G_T2 is null or G_T2 <> 'C')
                                          and (G_T3 is null or G_T3 <> 'C')
                                          and (G_T4 is null or G_T4 <> 'C'))
                                and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    else //get by section
                    {
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T3 = 'P' or (g_deptcode='" + dept + @"' 
                                          and (G_T1 is null or G_T1 <> 'C') 
                                          and (G_T2 is null or G_T2 <> 'C')
                                          and (G_T3 is null or G_T3 <> 'C')
                                          and (G_T4 is null or G_T4 <> 'C'))
                                and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    break;
                case 2: //get at process T2
                    if (type == (int)Parameters.QueryDeptType.Section) //get by section
                    {
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T2 = 'P' or (g_deptcode='" + dept + @"' 
                                          and (G_T1 is null or G_T1 <> 'C') 
                                          and (G_T2 is null or G_T2 <> 'C')
                                          and (G_T3 is null or G_T3 <> 'C')
                                          and (G_T4 is null or G_T4 <> 'C'))
                                and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    else //get by line
                    {
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list
                             where G_T2 = 'P' or (g_deptcode='" + dept + @"' 
                                          and (G_T1 is null or G_T1 <> 'C') 
                                          and (G_T2 is null or G_T2 <> 'C')
                                          and (G_T3 is null or G_T3 <> 'C')
                                          and (G_T4 is null or G_T4 <> 'C'))
                                    and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }


        public DataTable GetAPEKZAPDataTable(DataBase DB, string dept, int type, int process)
        {
            string sql = "";
            switch (process)
            {
                case 4: //get at process T4
                    if (type == (int)Parameters.QueryDeptType.All) //get all
                    {
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T4 = 'P' and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    else
                    { //get by plant
                        sql = string.Format(@"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T4 = 'P' 
                             and g_deptcode in (select department_code from base005m where udf07='{0}' and udf10='Y')
                             order by g_plandate asc",dept);
                    }
                    break;
                case 3: //get at process T3
                    if (type == (int)Parameters.QueryDeptType.Plant) //get by plant
                    {
                        //sql = @"select id,
                        //           g_problempoint,
                        //           g_measure,
                        //           g_princtipal,
                        //           g_plandate
                        //      from tms_kzactionrpt_list 
                        //     where G_T3 = 'P' AND (g_deptcode='" + dept + @"' 
                        //                  and (G_T1 is null or G_T1 <> 'C') 
                        //                  and (G_T2 is null or G_T2 <> 'C')
                        //                  and (G_T3 is null or G_T3 <> 'C')
                        //                  and (G_T4 is null or G_T4 <> 'C'))
                        //        and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                        //        or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                        //        or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                        //        order by g_plandate asc";
                        sql = string.Format(@"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list
                             where G_T3 = 'P' AND G_DEPTCODE IN(SELECT DEPARTMENT_CODE FROM BASE005M WHERE UDF05 = '{0}' AND UDF10 = 'Y')",dept);
                    }
                    else //get by section
                    {
                        //sql = @"select id,
                        //           g_problempoint,
                        //           g_measure,
                        //           g_princtipal,
                        //           g_plandate
                        //      from tms_kzactionrpt_list 
                        //     where G_T3 = 'P' AND  (g_deptcode='" + dept + @"' 
                        //                  and (G_T1 is null or G_T1 <> 'C') 
                        //                  and (G_T2 is null or G_T2 <> 'C')
                        //                  and (G_T3 is null or G_T3 <> 'C')
                        //                  and (G_T4 is null or G_T4 <> 'C'))
                        //        and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                        //        or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                        //        or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                        //     order by g_plandate asc";
                        sql = string.Format(@"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T3 = 'P' 
                             and g_deptcode in (select department_code from base005m where udf07='{0}' and udf10='Y')
                             order by g_plandate asc",dept);
                    }
                    break;
                case 2: //get at process T2
                    if (type == (int)Parameters.QueryDeptType.Section) //get by section
                    {
                        sql = string.Format(@"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list 
                             where G_T2 = 'P' 
                             and g_deptcode in (select department_code from base005m where udf07='{0}' and udf10='Y')
                             order by g_plandate asc", dept);
                    }
                    else //get by line
                    {
                        sql = @"select id,
                                   g_problempoint,
                                   g_measure,
                                   g_princtipal,
                                   g_plandate
                              from tms_kzactionrpt_list
                             where G_T2 = 'P' AND (g_deptcode='" + dept + @"' 
                                          and (G_T1 is null or G_T1 <> 'C') 
                                          and (G_T2 is null or G_T2 <> 'C')
                                          and (G_T3 is null or G_T3 <> 'C')
                                          and (G_T4 is null or G_T4 <> 'C'))
                                    and (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                             order by g_plandate asc";
                    }
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }


        public DataTable GetKZAPDataBarChart(DataBase DB, string dept, int type, int process)
        {
            string sql = "";
            switch (process)
            {
                case 4: //get at process 4
                    if (type == (int)Parameters.QueryDeptType.All)
                    { //get all
                        sql = @"select coalesce(sum(count),0) as count, udf05 as department 
                            from (
                            select  g_deptcode as department, count(*) as count from tms_kzactionrpt_list 
                            where g_deptcode in (select distinct udf05 from base005m where udf10='Y') and G_T4 ='P'
                            group by g_deptcode
                            union
                            select  b.udf05 as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct udf07 from base005m where udf10='Y') and G_T4 ='P'
                            group by b.udf05
                            union
                            select  b.udf05 as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct department_code from base005m where udf10='Y' and department_code <> udf07) and G_T4 ='P'
                            group by b.udf05
                            ) x right outer join (select distinct udf05 from BASE005M where udf10='Y') y on x.department = y.udf05 
                            where udf05 is not null                            
                            group by udf05
                            order by udf05 desc";
                    }
                    else
                    { //get by plant
                        sql = @"select coalesce(sum(count),0) as count, udf07 as department 
                            from (
                            select  g_deptcode as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05='" + dept + @"') and G_T4='P'
                            group by g_deptcode
                            union
                            select  b.udf07 as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct department_code from base005m where udf10='Y' and udf05='" + dept + @"' and department_code <> udf07)  
                                and G_T4='P'
                            group by b.udf07
                            ) x right outer join (select distinct udf07 from BASE005M where udf10='Y' and udf05='" + dept + @"') y on x.department = y.udf07 
                            where udf07 is not null                            
                            group by udf07
                            order by udf07 desc";
                    }
                    break;
                case 3: //get at process 3
                    if (type == (int)Parameters.QueryDeptType.Plant)
                    { //get by plant
                        sql = @"select coalesce(sum(count),0) as count, udf07 as department 
                            from (
                            select  g_deptcode as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05='" + dept + @"')  and G_T3='P'
                            group by g_deptcode
                            union
                            select  b.udf07 as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct department_code from base005m where udf10='Y' and udf05='" + dept + @"' and department_code <> udf07)  
                               and G_T3='P'
                            group by b.udf07
                            ) x right outer join (select distinct udf07 from BASE005M where udf10='Y' and udf05='" + dept + @"') y on x.department = y.udf07 
                            where udf07 is not null                            
                            group by udf07
                            order by udf07 desc";
                    }
                    else { //get by section
                        sql = @"select coalesce(sum(count),0) as count, department_code as department 
                                from(
                                select  g_deptcode as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                                where g_deptcode in (select distinct department_code from base005m where udf10 = 'Y' and udf07 = '" + dept + @"' and department_code <> udf07)
                                      and G_T3 = 'P'
                                group by g_deptcode
                                ) x right outer join(select distinct department_code from BASE005M where udf10= 'Y' and udf07 = '" + dept + @"' and department_code <> udf07) y on x.department = y.department_code
                                where department_code is not null
                                group by department_code
                                order by department_code desc";
                    }
                    break;
                case 2: //get at process 2
                    if (type == (int)Parameters.QueryDeptType.Section)
                    { //get by section
                        sql = @"select coalesce(sum(count),0) as count, department_code as department 
                            from (
                            select  g_deptcode as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct department_code from base005m where udf10='Y' and udf07='" + dept + @"' and department_code <> udf07) 
                                and G_T2 = 'P'
                            group by g_deptcode
                            ) x right outer join (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"' and department_code <> udf07) y on x.department = y.department_code 
                            group by department_code
                            order by department_code desc";
                    }
                    else
                    {  //get by line
                        sql = @"select coalesce(sum(count),0) as count, department_code as department 
                            from (
                            select  g_deptcode as department, count(*) as count from tms_kzactionrpt_list a join base005m b on a.g_deptcode = b.department_code
                            where g_deptcode in (select distinct department_code from base005m where udf10='Y' and department_code='" + dept + @"' and department_code <> udf07) 
                                and G_T2 = 'P'
                            group by g_deptcode
                            ) x right outer join (select distinct department_code from BASE005M where udf10='Y' and department_code='" + dept + @"' and department_code <> udf07) y on x.department = y.department_code 
                            group by department_code
                            order by department_code desc";
                    }
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetAPEKZAPDataPieChart(DataBase DB, string dept, int type, int process)
        {
            string sql = "";
            switch (process)
            {
                case 4: //get at process 4
                    if (type == (int)Parameters.QueryDeptType.All)
                    { //get all
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select department_code from base005m where udf10='Y') AND G_T4 IS NOT NULL) as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select department_code from base005m where udf10='Y') and G_T4 = 'P') as open
                          from dual";
                    }
                    else { //get by plant
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') AND G_T4 IS NOT NULL) as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') and G_T4 = 'P') as open
                          from dual";
                    }
                    
                    break;
                case 3: //get at process 3
                    if (type == (int)Parameters.QueryDeptType.Plant)
                    { //get by plant
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') AND G_T3 IS NOT NULL) as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') and G_T3 = 'P') as open
                          from dual";
                    }
                    else { //get by section
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"') AND G_T3 IS NOT NULL) as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"') and G_T3 = 'P') as open
                          from dual";
                    }
                    break;
                case 2: //get at process 2
                    if (type == (int)Parameters.QueryDeptType.Section)
                    { //get by plant
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"') AND G_T2 IS NOT NULL) as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"') and G_T2 = 'P') as open
                          from dual";
                    }
                    else { //get by line
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and department_code='" + dept + @"') AND G_T2 IS NOT NULL) as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and department_code='" + dept + @"') and G_T2 = 'P') as open
                          from dual";
                    }
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }


        public DataTable GetKZAPDataPieChart(DataBase DB, string dept, int type, int process)
        {
            string sql = "";
            switch (process)
            {
                case 4: //get at process 4
                    if (type == (int)Parameters.QueryDeptType.All)
                    { //get all
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select department_code from base005m where udf10='Y') and G_T4 ='P') as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select department_code from base005m where udf10='Y') and G_T4 = 'C') as open
                          from dual";
                    }
                    else
                    { //get by plant
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') and G_T4 ='P') as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') and G_T4 = 'C') as open
                          from dual";
                    }

                    break;
                case 3: //get at process 3
                    if (type == (int)Parameters.QueryDeptType.Plant)
                    { //get by plant
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') and G_T3 ='P') as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf05='" + dept + @"') and G_T3 = 'C') as open
                          from dual";
                    }
                    else
                    { //get by section
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"' and department_code <> udf07) and G_T3 ='P') as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"' and department_code <> udf07) and G_T3 = 'C') as open
                          from dual";
                    }
                    break;
                case 2: //get at process 2
                    if (type == (int)Parameters.QueryDeptType.Section)
                    { //get by plant
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"' and department_code <> udf07) and G_T2 ='P') as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and udf07='" + dept + @"' and department_code <> udf07) and G_T2 = 'C') as open
                          from dual";
                    }
                    else
                    { //get by line
                        sql = @"select (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and department_code='" + dept + @"' and department_code <> udf07) and G_T2 ='P') as total,
                               (select count(*)
                                  from tms_kzactionrpt_list
                                 where g_deptcode in (select distinct department_code from BASE005M where udf10='Y' and department_code='" + dept + @"' and department_code <> udf07) and G_T2 = 'C') as open
                          from dual";
                    }
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetSafetyDataByDate(DataBase DB, DateTime date, string dept, int type)
        {
            string sql = "";
            switch (type) {
                case (int) Parameters.QueryDeptType.All: //get all
                    sql = @"select count(*) 
                            from MFG_ACCIDENT 
                            where (deptcode in (select distinct department_code from base005m where udf10='Y') 
                                or deptcode in (select distinct udf05 from base005m where udf10='Y') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y'))
                                and trunc(HAPPENEDDATE) = to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Plant: //get by plant
                    sql = @"select count(*) 
                            from MFG_ACCIDENT 
                            where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf05 = '"+dept+ @"') 
                                or deptcode in (select distinct udf05 from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05 = '" + dept + @"'))
                                and trunc(HAPPENEDDATE) = to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Section: //get by section
                    sql = @"select count(*) 
                            from MFG_ACCIDENT 
                            where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf07 = '" + dept + @"') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf07 = '" + dept + @"'))
                                and trunc(HAPPENEDDATE) = to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Line: //get by line
                    sql = @"select count(*) 
                            from MFG_ACCIDENT 
                            where deptcode in (select distinct department_code from base005m where udf10='Y' and department_code = '" + dept + @"')
                                and trunc(HAPPENEDDATE) = to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + "')";
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetSafetyDataUntilDate(DataBase DB, DateTime date, DateTime firstDateOfYear, string dept, int type)
        {
            string sql = "";
            switch (type)
            {
                case (int)Parameters.QueryDeptType.All: //get all
                    sql = @"select count(*) from MFG_ACCIDENT 
                            where (deptcode in (select distinct department_code from base005m where udf10='Y') 
                                or deptcode in (select distinct udf05 from base005m where udf10='Y') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y'))
                                and trunc(happeneddate) between to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Plant: //get by plant
                    sql = @"select count(*) 
                            from MFG_ACCIDENT 
                            where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                or deptcode in (select distinct udf05 from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05 = '" + dept + @"'))
                                and trunc(happeneddate) between to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Section: //get by section
                    sql = @"select count(*) 
                            from MFG_ACCIDENT 
                            where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf07 = '" + dept + @"') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf07 = '" + dept + @"'))
                                and trunc(happeneddate) between to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Line: //get by line
                    sql = @"select count(*) 
                            from MFG_ACCIDENT 
                            where deptcode in (select distinct department_code from base005m where udf10='Y' and department_code = '" + dept + @"')
                                and trunc(happeneddate) between to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetSafetyDays(DataBase DB, DateTime date, DateTime firstDateOfYear, string dept, int type)
        {
            string sql = "";
            switch (type)
            {
                case (int)Parameters.QueryDeptType.All: //get all
                    sql = @"select to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"') - trunc(happeneddate) as datediff 
                            from
                            (select happeneddate
                            from (
                            select happeneddate from(
                            select happeneddate from mfg_accident 
                            where deptcode in (select distinct department_code from base005m where udf10='Y') 
                                or deptcode in (select distinct udf05 from base005m where udf10='Y') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y')
                            union all 
                            select to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + @"','" + Parameters.dateFormat + @"') as happeneddate from dual)
                            order by happeneddate desc)
                            where rownum = 1)";
                    break;
                case (int)Parameters.QueryDeptType.Plant: //get by plant
                    sql = @"select to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"') - trunc(happeneddate) as datediff 
                            from
                            (select happeneddate
                            from (
                            select happeneddate from(
                            select happeneddate from mfg_accident 
                            where deptcode in (select distinct department_code from base005m where udf10='Y' and udf05='"+dept+ @"') 
                                or deptcode in (select distinct udf05 from base005m where udf10='Y' and udf05='" + dept + @"') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05='" + dept + @"')
                            union all 
                            select to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + @"','" + Parameters.dateFormat + @"') as happeneddate from dual)
                            order by happeneddate desc)
                            where rownum = 1)";
                    break;
                case (int)Parameters.QueryDeptType.Section: //get by section
                    sql = @"select to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"') - trunc(happeneddate) as datediff 
                            from
                            (select happeneddate
                            from (
                            select happeneddate from(
                            select happeneddate from mfg_accident 
                            where deptcode in (select distinct department_code from base005m where udf10='Y' and udf07='" + dept + @"')  
                                or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf07='" + dept + @"')
                            union all 
                            select to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + @"','" + Parameters.dateFormat + @"') as happeneddate from dual)
                            order by happeneddate desc)
                            where rownum = 1)";
                    break;
                case (int)Parameters.QueryDeptType.Line: //get by line
                    sql = @"select to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"') - trunc(happeneddate) as datediff 
                            from
                            (select happeneddate
                            from (
                            select happeneddate from(
                            select happeneddate from mfg_accident 
                            where deptcode in (select distinct department_code from base005m where udf10='Y' and department_code='" + dept + @"')  
                            union all 
                            select to_date('" + firstDateOfYear.ToString(Parameters.dateFormat) + @"','" + Parameters.dateFormat + @"') as happeneddate from dual)
                            order by happeneddate desc)
                            where rownum = 1)";
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetKaizenUntilDate(DataBase DB, DateTime firstDate, DateTime date, string dept, int type)
        {
            string sql = "";
            switch (type)
            {
                case (int) Parameters.QueryDeptType.All: //get all
                    sql = @" select coalesce(sum(ACCEPTED),0) as ACCEPTED  
                            from mfg_kaizen
                            where (deptcode in (select distinct department_code from base005m where udf10='Y') 
                                    or deptcode in (select distinct udf05 from base005m where udf10='Y') 
                                    or deptcode in (select distinct udf07 from base005m where udf10='Y'))
                                and trunc(createddate) between to_date('" + firstDate.ToString(Parameters.dateFormat)+"','"+Parameters.dateFormat+ "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Plant: //get by plant
                    sql = @" select coalesce(sum(ACCEPTED),0) as ACCEPTED  
                            from mfg_kaizen
                            where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                    or deptcode in (select distinct udf05 from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                    or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05 = '" + dept + @"'))
                                and trunc(createddate) between to_date('" + firstDate.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Section: //get by section
                    sql = @" select coalesce(sum(ACCEPTED),0) as ACCEPTED  
                            from mfg_kaizen
                            where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf07 = '" + dept + @"') 
                                    or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf07 = '" + dept + @"'))
                                and trunc(createddate) between to_date('" + firstDate.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                case (int)Parameters.QueryDeptType.Line:  //get by line
                    sql = @" select coalesce(sum(ACCEPTED),0) as ACCEPTED  
                            from mfg_kaizen
                            where deptcode in (select distinct department_code from base005m where udf10='Y' and department_code = '" + dept + @"')
                                and trunc(createddate) between to_date('" + firstDate.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "') and to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetManpower(DataBase DB, string dept, int type, DateTime date)
        {
            string sql = "";
            switch (type)
            {
                case (int)Parameters.QueryDeptType.All: //get all
                    sql = @"select sum(manpower) as manpower from mfg_department where code in (select department_code from base005m) and parent is null and year = '" + date.ToString("yyyy")+"'";
                    break;
                case (int)Parameters.QueryDeptType.Plant: //get by plant
                    sql = @"select sum(manpower) as manpower from mfg_department where code in (select department_code from base005m) and year = '" + date.ToString("yyyy") + "' and code='"+dept+"'";
                    break;
                case (int)Parameters.QueryDeptType.Section: //get by section
                    sql = @"select sum(manpower) as manpower from mfg_department where code in (select department_code from base005m) and  year = '" + date.ToString("yyyy") + "' and code='" + dept + "'";
                    break;
                case (int)Parameters.QueryDeptType.Line:  //get by line
                    sql = @"select sum(manpower) as manpower from mfg_department where code in (select department_code from base005m) and  year = '" + date.ToString("yyyy") + "' and code='" + dept + "'";
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetMainKaizenChart(DataBase DB, DateTime date, string dept, int type)
        {
            string sql = "";
            switch (type) {
                case (int)Parameters.QueryDeptType.All: //get all
                    sql = @"select COALESCE(ACCEPTED, 0) AS QTY, y.month as MONTH 
                              from (select to_char(createddate, 'mm') as month,
                                           sum(ACCEPTED) as ACCEPTED
                                      from mfg_kaizen 
                                     where (deptcode in (select distinct department_code from base005m where udf10='Y') 
                                            or deptcode in (select distinct udf05 from base005m where udf10='Y') 
                                            or deptcode in (select distinct udf07 from base005m where udf10='Y'))
                                           and to_char(createddate, 'yyyy') =
                                           to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"'), 'yyyy')
                                     group by to_char(createddate, 'mm')) x
                             right outer join (select to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"') +
                                                              numtoyminterval(level, 'month'),
                                                              'mm') as month
                                                 from dual
                                               connect by level <= 12) y
                                on x.month = y.month
                             order by month";
                    break;
                case (int)Parameters.QueryDeptType.Plant: //get by plant
                    sql = @"select COALESCE(ACCEPTED, 0) AS QTY, y.month as MONTH 
                              from (select to_char(createddate, 'mm') as month,
                                           sum(ACCEPTED) as ACCEPTED
                                      from mfg_kaizen 
                                     where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                            or deptcode in (select distinct udf05 from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                            or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05 = '" + dept + @"'))
                                           and to_char(createddate, 'yyyy') =
                                           to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"'), 'yyyy')
                                     group by to_char(createddate, 'mm')) x
                             right outer join (select to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"') +
                                                              numtoyminterval(level, 'month'),
                                                              'mm') as month
                                                 from dual
                                               connect by level <= 12) y
                                on x.month = y.month
                             order by month";
                    break;
                case (int)Parameters.QueryDeptType.Section: //get by section
                    sql = @"select COALESCE(ACCEPTED, 0) AS QTY, y.month as MONTH 
                              from (select to_char(createddate, 'mm') as month,
                                           sum(ACCEPTED) as ACCEPTED
                                      from mfg_kaizen 
                                     where (deptcode in (select distinct department_code from base005m where udf10='Y' and udf07 = '" + dept + @"') 
                                            or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf07 = '" + dept + @"'))
                                           and to_char(createddate, 'yyyy') =
                                           to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"'), 'yyyy')
                                     group by to_char(createddate, 'mm')) x
                             right outer join (select to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"') +
                                                              numtoyminterval(level, 'month'),
                                                              'mm') as month
                                                 from dual
                                               connect by level <= 12) y
                                on x.month = y.month
                             order by month";
                    break;
                case (int)Parameters.QueryDeptType.Line:  //get by line
                    sql = @"select COALESCE(ACCEPTED, 0) AS QTY, y.month as MONTH 
                              from (select to_char(createddate, 'mm') as month,
                                           sum(ACCEPTED) as ACCEPTED
                                      from mfg_kaizen 
                                     where deptcode in (select distinct department_code from base005m where udf10='Y' and department_code = '" + dept + @"')
                                           and to_char(createddate, 'yyyy') =
                                           to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"'), 'yyyy')
                                     group by to_char(createddate, 'mm')) x
                             right outer join (select to_char(to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"') +
                                                              numtoyminterval(level, 'month'),
                                                              'mm') as month
                                                 from dual
                                               connect by level <= 12) y
                                on x.month = y.month
                             order by month";
                    break;
                default:
                    break;
            }
            
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        #endregion
        #region DetailSafetyForm
        public DataTable GetSafetyTable(DataBase DB, DateTime firstDateOfMonth, string dept, int type)
        {
            string sql = "";
            switch (type)
            {
                case (int)Parameters.QueryDeptType.All: //get all
                    sql = @"select coalesce(happeneddate, to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"')) AS happeneddate, udf05 as code, coalesce(sum(count),0) as count
                                from(
                            select deptcode as code, trunc(happeneddate) as happeneddate, count(*) as count
                            from mfg_accident 
                            where deptcode in (select distinct udf05 from base005m where udf10 = 'Y')
                            and to_char(trunc(happeneddate),'MONTH') = to_char(to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"'),'MONTH')
                            group by deptcode, trunc(happeneddate)
                                union all 
                            select b.udf05 as code, trunc(happeneddate) as happeneddate, count(*) as count
                            from mfg_accident a join base005m b on a.deptcode = b.department_code
                            where deptcode in (select distinct udf07 from base005m where udf10 = 'Y')
                            and to_char(trunc(happeneddate),'MONTH') = to_char(to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"'),'MONTH')
                            group by b.udf05, trunc(happeneddate)
                                union all 
                            select b.udf05 as code, trunc(happeneddate) as happeneddate, count(*) as count
                            from mfg_accident a join base005m b on a.deptcode = b.department_code
                            where deptcode in (select distinct department_code from base005m where udf10 = 'Y')
                            and to_char(trunc(happeneddate),'MONTH') = to_char(to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + @"'),'MONTH')
                            group by b.udf05, trunc(happeneddate)
                                ) x right outer join (select distinct udf05 from base005m where udf10='Y') y on x.code = y.udf05
                                group by udf05, happeneddate
                                order by udf05";
                    break;
                case (int)Parameters.QueryDeptType.Plant: //get by plant
                    sql = @"select coalesce(happeneddate, to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"')) AS happeneddate, udf07 as code, coalesce(sum(count),0) as count
                                from(
                            select b.department_code as code, trunc(happeneddate) as happeneddate, count(*) as count
                            from mfg_accident a join base005m b on a.deptcode = b.department_code
                            where deptcode in (select distinct udf07 from base005m where udf10 = 'Y' and udf05 = '"+dept+ @"' and udf07 is not null)
                            and to_char(trunc(happeneddate),'MONTH') = to_char(to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"'),'MONTH')
                            group by b.department_code, trunc(happeneddate)
                                union all 
                            select b.udf07 as code, trunc(happeneddate) as happeneddate, count(*) as count
                            from mfg_accident a join base005m b on a.deptcode = b.department_code
                            where deptcode in (select distinct department_code from base005m where udf10= 'Y' and udf05 = '" + dept + @"' and udf07 is not null)
                            and to_char(trunc(happeneddate),'MONTH') = to_char(to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"'),'MONTH')
                            group by b.udf07, trunc(happeneddate)
                                ) x right outer join (select distinct udf07 from base005m where udf10='Y' and udf05 = '" + dept + @"' and udf07 is not null) y on x.code = y.udf07
                                group by udf07, happeneddate
                                order by udf07";
                    break;
                case (int)Parameters.QueryDeptType.Section: //get by section
                    sql = @"select coalesce(happeneddate, to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"')) AS happeneddate, department_code as code, coalesce(sum(count),0) as count
                                from(
                            select b.department_code as code, trunc(happeneddate) as happeneddate, count(*) as count
                            from mfg_accident a join base005m b on a.deptcode = b.department_code
                            where deptcode in (select distinct department_code from base005m where udf10 = 'Y' and udf07 = '" + dept + @"')
                            and to_char(trunc(happeneddate),'MONTH') = to_char(to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"'),'MONTH')
                            group by b.department_code, trunc(happeneddate)
                                ) x right outer join (select distinct department_code from base005m where udf10='Y' and udf07 = '" + dept + @"') y on x.code = y.department_code
                                group by department_code, happeneddate
                                order by department_code";                    
                    break;
                case (int)Parameters.QueryDeptType.Line: //get by line
                    sql = @"select coalesce(happeneddate, to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"')) AS happeneddate, department_code as code, coalesce(sum(count),0) as count
                                from(
                            select b.department_code as code, trunc(happeneddate) as happeneddate, count(*) as count
                            from mfg_accident a join base005m b on a.deptcode = b.department_code
                            where deptcode in (select distinct department_code from base005m where udf10 = 'Y' and department_code = '" + dept + @"')
                            and to_char(trunc(happeneddate),'MONTH') = to_char(to_date('" + firstDateOfMonth.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + @"'),'MONTH')
                            group by b.department_code, trunc(happeneddate)
                                ) x right outer join (select distinct department_code from base005m where udf10='Y' and department_code = '" + dept + @"') y on x.code = y.department_code
                                group by department_code, happeneddate
                                order by department_code";
                    break;
                default:
                    break;
            }
                    
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetSafetyChart(DataBase DB, string firstDateOfMonth, string dept, int type)
        {
            string sql = "";
            switch (type)
            {
                case (int)Parameters.QueryDeptType.All: //get all
                    sql = @"SELECT trunc(HAPPENEDDATE), COUNT(trunc(HAPPENEDDATE)) AS COUNT 
                            FROM MFG_ACCIDENT
                            WHERE TO_CHAR(trunc(HAPPENEDDATE), 'MONTH') = TO_CHAR(TO_DATE('" + firstDateOfMonth + "', '"+Parameters.dateFormat+"'), 'MONTH') " +
                                @"AND (deptcode in (select distinct department_code from base005m where udf10='Y') 
                                or deptcode in (select distinct udf05 from base005m where udf10='Y') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y')) 
                                GROUP BY trunc(HAPPENEDDATE)";
                    break;
                case (int)Parameters.QueryDeptType.Plant://get by plant
                    sql = @"SELECT trunc(HAPPENEDDATE), COUNT(trunc(HAPPENEDDATE)) AS COUNT
                            FROM MFG_ACCIDENT
                            WHERE TO_CHAR(trunc(HAPPENEDDATE), 'MONTH') = TO_CHAR(TO_DATE('"+firstDateOfMonth+"', '"+Parameters.dateFormat+"'), 'MONTH') " +
                                @"AND (deptcode in (select distinct department_code from base005m where udf10='Y' and udf05 = '" + dept + @"') 
                                or deptcode in (select distinct udf07 from base005m where udf10='Y' and udf05 = '" + dept + @"')) 
                                GROUP BY trunc(HAPPENEDDATE)";
                    break;
                case (int)Parameters.QueryDeptType.Section://get by section
                    sql = @"SELECT trunc(HAPPENEDDATE), COUNT(trunc(HAPPENEDDATE)) AS COUNT
                            FROM MFG_ACCIDENT
                            WHERE TO_CHAR(trunc(HAPPENEDDATE), 'MONTH') = TO_CHAR(TO_DATE('" + firstDateOfMonth + "', '" + Parameters.dateFormat + "'), 'MONTH') " +
                                @"AND (deptcode in (select distinct department_code from base005m where udf10='Y' and udf07 = '" + dept + @"')) 
                            GROUP BY trunc(HAPPENEDDATE)";
                    break;
                case (int)Parameters.QueryDeptType.Line: //get by line
                    sql = @"SELECT trunc(HAPPENEDDATE), COUNT(trunc(HAPPENEDDATE)) AS COUNT
                            FROM MFG_ACCIDENT
                            WHERE TO_CHAR(trunc(HAPPENEDDATE), 'MONTH') = TO_CHAR(TO_DATE('" + firstDateOfMonth + "', '" + Parameters.dateFormat + "'), 'MONTH') " +
                                @"AND deptcode in (select distinct department_code from base005m where udf10='Y' and department_code = '" + dept + @"') 
                            GROUP BY trunc(HAPPENEDDATE)";
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        #endregion
        #region DetailKaizenForm
        public DataTable GetKaizenData(DataBase DB, DateTime date, string dept, int type)
        {
            string sql = "";
            string p_year = date.Year.ToString();
            switch (type)
            {
                case (int)Parameters.QueryDeptType.All: //get all      
                    sql = string.Format(@"select udf05 as department, coalesce(RECEIVED,0) as RECEIVED, coalesce(ACCEPTED,0) as ACCEPTED, coalesce(PERPEOPLE,0) as PERPEOPLE
                                            from (
                                            select udf05  as DEPARTMENT ,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_PLANT_PERSON(UDF05,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where udf05 in (select distinct udf05 from base005m where udf10 = 'Y') and udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  udf05 ) a right outer join (select distinct udf05 from base005m where udf10 = 'Y') on department = udf05 order by department", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;
                case (int)Parameters.QueryDeptType.Plant://get by plant
                    sql = string.Format(@"select udf07 as department, coalesce(RECEIVED,0) as RECEIVED, coalesce(ACCEPTED,0) as ACCEPTED, coalesce(PERPEOPLE,0) as PERPEOPLE
                                            from (
                                            select udf07 as DEPARTMENT,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_SECTION_PERSON(UDF07,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where udf05='{0}' and udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  udf07 ) a right outer join (select distinct udf07 from base005m where udf10 = 'Y' and udf07 is not null and udf05 = '{0}') on department = udf07 order by department", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;
                case (int)Parameters.QueryDeptType.Section://get by section
                    sql = string.Format(@"select department_code as department, coalesce(RECEIVED,0) as RECEIVED, coalesce(ACCEPTED,0) as ACCEPTED, coalesce(PERPEOPLE,0) as PERPEOPLE
                                            from (
                                            select department_code   as DEPARTMENT,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_LINE_PERSON(department_code,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where udf07='{0}' and udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  department_code ) a right outer join (select distinct department_code from base005m where udf10 = 'Y' and department_code is not null and udf07 = '{0}') on department = department_code order by department", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;
                case (int)Parameters.QueryDeptType.Line: //get by line
                    sql = string.Format(@"select department_code as DEPARTMENT,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_LINE_PERSON(department_code,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where udf07='{0}' and udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  department_code", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        #endregion


        #region DetailKaizenForm
        public DataTable GetAPEKaizenData(DataBase DB, DateTime date, string dept, int type)
        {
            string sql = "";
            string p_year = date.Year.ToString();
            switch (type)
            {
                case (int)Parameters.QueryDeptType.All: //get all      
                    sql = string.Format(@"select udf05  as DEPARTMENT ,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_PLANT_PERSON(UDF05,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where  udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  udf05", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;

                case (int)Parameters.QueryDeptType.Plant://get by plant
                    sql = string.Format(@"select udf07 as DEPARTMENT,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_SECTION_PERSON(UDF07,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where udf05='{0}' and udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  udf07", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;
                case (int)Parameters.QueryDeptType.Section://get by section
                    sql = string.Format(@"select department_code   as DEPARTMENT,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_LINE_PERSON(department_code,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where udf07='{0}' and udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  department_code", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;
                case (int)Parameters.QueryDeptType.Line: //get by line
                    sql = string.Format(@"select department_code as DEPARTMENT,sum(received) as received,sum(accepted) as accepted,round(sum(accepted)/GF_KAIZEN_LINE_PERSON(department_code,'{3}'),2) as perpeople
                                          from base005m,mfg_kaizen
                                          where udf07='{0}' and udf10='Y' and department_code=deptcode
                                           and to_char(createddate,'mm') = to_char(to_date('{1}','{2}'),'mm') 
                                           and to_char(createddate,'yyyy') = to_char(to_date('{1}','{2}'),'yyyy') 
                                           group by  department_code", dept, date.ToString(Parameters.dateFormat), Parameters.dateFormat, p_year);
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        #endregion

        #region Kaizen Action
        public DataTable GetKaizenAction(DataBase DB, string from, string to, int type, string dept, int process)
        {
            from = from.Replace("-", "/");
            to = to.Replace("-", "/");
            string sql = "";
            switch (process)
            {
                case 4: //get at process 4
                    if (type == (int)Parameters.QueryDeptType.All) //get all 
                    {
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat+@"') 
                                AND  (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                                and G_T4 is not null 
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    else { //get by plant
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND  (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                                and (G_T4 is not null or g_deptcode = '"+dept+@"')
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    break;
                case 3: //get at process 3
                    if (type == (int)Parameters.QueryDeptType.Plant) //get by plant 
                    {
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND  (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                                and (G_T3 is not null or g_deptcode = '" + dept + @"')
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    else
                    { //get by section
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND  (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                                and (G_T3 is not null or g_deptcode = '" + dept + @"')
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                   break;
                case 2: //get at process 2
                    if (type == (int)Parameters.QueryDeptType.Section) //get by section 
                    {
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND  (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                                and (G_T2 is not null or g_deptcode = '" + dept + @"')
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    else
                    { //get by line
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND  (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                                and (G_T2 is not null or g_deptcode = '" + dept + @"')
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                   break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        //change it 
        public DataTable GetAPEKaizenAction(DataBase DB, string from, string to, int type, string dept, int process)
        {
            from = from.Replace("-", "/");
            to = to.Replace("-", "/");
            string sql = "";
            switch (process)
            {
                case 4: //get at process 4
                    if (type == (int)Parameters.QueryDeptType.All) //get all 
                    {
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                and G_T4 is not null 
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    else
                    { //get by plant
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND G_DEPTCODE in(select department_code from base005m where udf05='" + dept + @"' and udf10='Y')  
                                AND  G_T3 IS NOT NULL
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    break;
                case 3: //get at process 3
                    if (type == (int)Parameters.QueryDeptType.Plant) //get by plant 
                    {
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND G_DEPTCODE in(select department_code from base005m where udf05='" + dept + @"' and udf10='Y')  
                                AND  G_T3 IS NOT NULL
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    else
                    { //get by section
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND G_DEPTCODE in(select department_code from base005m where udf07='" + dept + @"' and udf10='Y')  
                                AND  G_T2 IS NOT NULL
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    break;
                case 2: //get at process 2
                    if (type == (int)Parameters.QueryDeptType.Section) //get by section 
                    {
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM MES00.TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND G_DEPTCODE in(select department_code from base005m where udf07='" + dept + @"' and udf10='Y')  
                                AND  G_T2 IS NOT NULL
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    else
                    { //get by line
                        sql = @"SELECT ID, G_DEPTCODE, G_GREATEDDATE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, G_PLANDATE, G_FINISHDATE, G_REMARK, G_T1, G_T2, G_T3, G_T4 
                                FROM TMS_KZACTIONRPT_LIST WHERE to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') >= to_date('" + from + "','" + Parameters.dateFormat + @"') 
                                AND to_date(G_GREATEDDATE,'" + Parameters.dateFormat + @"') <= to_date('" + to + "', '" + Parameters.dateFormat + @"') 
                                AND  (G_DEPTCODE in (select distinct department_code from base005m where udf10='Y') 
                                or G_DEPTCODE in (select distinct udf05 from base005m where udf10 = 'Y') 
                                or G_DEPTCODE in (select distinct udf07 from base005m where udf10 = 'Y'))
                                and (G_T2 is not null or g_deptcode = '" + dept + @"')
                                ORDER BY G_GREATEDDATE, G_DEPTCODE";
                    }
                    break;
                default:
                    break;
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public void EditKaizenAction(DataBase DB, string id, string department, string finder, string problem, string measure, string principal, string remark, string createdDate, string planDate, string finishDate)
        {
            string sql = @"UPDATE MES00.TMS_KZACTIONRPT_LIST " +
                        "SET "+
                        "G_DEPTCODE = '" + department + "', " +
                        "G_FINDER = '" + finder + "', " +
                        "G_PROBLEMPOINT = '" + problem + "', " +
                        "G_MEASURE = '" + measure + "', " +
                        "G_PRINCTIPAL = '" + principal + "', " +
                        "G_REMARK = '" + remark + "', " +
                        "G_GREATEDDATE = '" + createdDate + "', " +
                        "G_PLANDATE = '" + planDate + "', " +
                        "G_FINISHDATE = '" + finishDate + "' " +
                        "WHERE ID = '" + id + "'";
            
            DB.ExecuteNonQuery(sql);
        }
        public void CreateKaizenAction(DataBase DB, string department, string finder, string problem, string measure, string principal, string remark, string createdDate, string planDate, string finishDate, string T1, string T2, string T3, string T4)
        {
            string sql = @"INSERT INTO MES00.TMS_KZACTIONRPT_LIST " +
                        "(G_DEPTCODE, G_FINDER, G_PROBLEMPOINT, G_MEASURE, G_PRINCTIPAL, " +
                        "G_REMARK, G_GREATEDDATE, G_PLANDATE, G_FINISHDATE, G_T1, G_T2, G_T3, G_T4) VALUES " +
                        "('" + department + "', '" + finder + "','" + problem + "','" + measure + "','" + principal + "', " +
                        "'" + remark + "', '" + createdDate + "', '" + planDate + "','" + finishDate + "','" + T1 + "','" + T2 + "', '" + T3 + "', '" + T4 + "') ";
            DB.ExecuteNonQuery(sql);
        }
        public void UpdateStatus(DataBase DB, int id, string T1, string T2, string T3, string T4)
        {
            string sql = @"UPDATE MES00.TMS_KZACTIONRPT_LIST " +
                       "SET G_T1 = '" + T1 + "', " +
                       "G_T2 = '" + T2 + "', " +
                       "G_T3 = '" + T3 + "', " +
                       "G_T4 = '" + T4 + "' " +
                       "WHERE ID = '" + id + "'";
            DB.ExecuteNonQuery(sql);
        }

        #endregion
        #region Maturity Assessment
        public DataTable GetMaturityList(DataBase DB)
        {
            string sql = @"SELECT * FROM  TMS_MATURITY_LIST";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetMaturityAssessmentList(DataBase DB, string deptCode, DateTime date)
        {
            string sql = @"select a.*, b.namecn,b.nameen,b.nameyn, b.code
                                from 
                                    (select * from tms_maturity_assessment_list where deptcode = '"+deptCode+ "' and updateddate = to_date('" + date.ToString(Parameters.dateFormat)+"', '"+Parameters.dateFormat+"')) " +
                                    "a right outer join tms_maturity_list b on a.maturitycode = b.code ";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public void SaveMaturityAssessment(DataBase DB, string deptCode, DateTime date, string[] listCode, string[] listStatus, string[] listNote)
        {

            string sql = @"INSERT ALL ";
            for (var i = 0; i < listCode.Length; i++) {
                sql += @"into tms_maturity_assessment_list (deptcode, updateddate, maturitycode, status, note) values ('"+deptCode+ "',TO_DATE('"+ date.ToString(Parameters.dateFormat) + "', '"+ Parameters.dateFormat + "'),'"+listCode[i]+"','"+listStatus[i]+"','"+listNote[i]+"') \n";
            }
            sql += @" SELECT 1 FROM DUAL";
            DB.ExecuteNonQuery(sql);
        }
        public void DeleteMaturityAssessment(DataBase DB, string deptCode, DateTime date) {
            try
            {
                string sqlDelete = @"delete from tms_maturity_assessment_list where deptcode = '" + deptCode + "' and updateddate = to_date('" + date.ToString(Parameters.dateFormat) + "', '" + Parameters.dateFormat + "')";
                DB.ExecuteNonQueryOffline(sqlDelete);

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        #endregion
        public DataTable GetDeptList(DataBase DB, string plant, string section)
        {
            string sql = @"";
            if (section != "") //get Line
            {
                sql = @"select DEPARTMENT_CODE, DEPARTMENT_NAME from BASE005M where UDF07 = '" + section + "' and UDF06 = 'Y' order by DEPARTMENT_CODE";
            }
            else if (plant != "") //get Section 
            {
                sql = @"select DEPARTMENT_CODE, DEPARTMENT_NAME from BASE005M where UDF05 = '" + plant + "' and DEPARTMENT_CODE in(select distinct udf07 from BASE005M where udf10='Y') order by DEPARTMENT_CODE";
            } else //get Plant
            {
                sql = @"select DEPARTMENT_CODE, DEPARTMENT_NAME from BASE005M where  DEPARTMENT_CODE in(select distinct udf05 from BASE005M where udf10='Y') order by DEPARTMENT_CODE";
            }
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        #region Tier 1
        public DataTable GetTier1(DataBase DB, string dept, DateTime date)
        {
            string sql = @"select * from TMS_TIER1 where G_DEPTCODE = '"+dept+ "' and trunc(G_DATE) = to_date('"+ date.ToString(Parameters.dateFormat) + "','"+ Parameters.dateFormat + "')";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable GetTier1Standard(DataBase DB, string dept, DateTime date)
        {
            string sql = @"select * from TMS_TIER1_STANDARD where G_DEPTCODE = '" + dept + "' and trunc(G_DATE) = to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public void UpdateTier1(DataBase DB, string G_DEPTCODE, DateTime G_DATE, string G_1, string G_2, string G_3, string G_4,
                        string G_5, string G_6, string G_7, string G_8, string G_RESULT, string G_AUDITOR,
                         string username)
        {
            string now = DateTime.Now.ToString(Parameters.dateFormat + " hh:mm:ss");
            string sql = @"update TMS_TIER1 set
                            g_last_update_date = to_date('"+ now + "','"+ Parameters.dateTimeFormat+ @"'),
                            g_updated_by = '"+ username + @"',
                            g_1 = '"+ G_1 + @"',
                            g_2 = '" + G_2 + @"',
                            g_3 = '" + G_3 + @"',
                            g_4 = '" + G_4 + @"',
                            g_5 = '" + G_5 + @"',
                            g_6 = '" + G_6 + @"',
                            g_7 = '" + G_7 + @"',
                            g_8 = '" + G_8 + @"',
                            g_result = '"+ G_RESULT + @"',
                            g_auditor = '"+ G_AUDITOR + @"'
                        where G_DEPTCODE = '" + G_DEPTCODE + "' and trunc(G_DATE) = to_date('" + G_DATE.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
            DB.ExecuteNonQuery(sql);
        }
        public void UpdateTier1Standard(DataBase DB, string G_DEPTCODE, DateTime G_DATE,
            string G_SUPERVISOR_1, string G_SUPERVISOR_2, string G_SUPERVISOR_3, string G_SUPERVISOR_4,
            string G_SUPERVISOR_5, string G_SUPERVISOR_6, string G_SUPERVISOR_7, string G_SUPERVISOR_8, string G_SUPERVISOR_AUDITOR,
            string G_VSM_1, string G_VSM_2, string G_VSM_3, string G_VSM_4,
            string G_VSM_5, string G_VSM_6, string G_VSM_7, string G_VSM_8, string G_VSM_AUDITOR,
            string G_THIRD_PARTY_1, string G_THIRD_PARTY_2, string G_THIRD_PARTY_3, string G_THIRD_PARTY_4,
            string G_THIRD_PARTY_5, string G_THIRD_PARTY_6, string G_THIRD_PARTY_7, string G_THIRD_PARTY_8, string G_THIRD_PARTY_AUDITOR,
            string username)
        {
            string now = DateTime.Now.ToString(Parameters.dateFormat + " hh:mm:ss");
            string sql = @"update TMS_TIER1_STANDARD set
                            g_last_update_date = to_date('" + now + "','" + Parameters.dateTimeFormat + @"'),
                            g_updated_by = '" + username + @"',
                            G_SUPERVISOR_1 = '" + G_SUPERVISOR_1 + @"',
                            G_SUPERVISOR_2 = '" + G_SUPERVISOR_2 + @"',
                            G_SUPERVISOR_3 = '" + G_SUPERVISOR_3 + @"',
                            G_SUPERVISOR_4 = '" + G_SUPERVISOR_4 + @"',
                            G_SUPERVISOR_5 = '" + G_SUPERVISOR_5 + @"',
                            G_SUPERVISOR_6 = '" + G_SUPERVISOR_6 + @"',
                            G_SUPERVISOR_7 = '" + G_SUPERVISOR_7 + @"',
                            G_SUPERVISOR_8 = '" + G_SUPERVISOR_8 + @"',
                            G_SUPERVISOR_AUDITOR = '" + G_SUPERVISOR_AUDITOR + @"',
                            G_VSM_1 = '" + G_VSM_1 + @"',
                            G_VSM_2 = '" + G_VSM_2 + @"',
                            G_VSM_3 = '" + G_VSM_3 + @"',
                            G_VSM_4 = '" + G_VSM_4 + @"',
                            G_VSM_5 = '" + G_VSM_5 + @"',
                            G_VSM_6 = '" + G_VSM_6 + @"',
                            G_VSM_7 = '" + G_VSM_7 + @"',
                            G_VSM_8 = '" + G_VSM_8 + @"',
                            G_VSM_AUDITOR = '" + G_VSM_AUDITOR + @"',
                            G_THIRD_PARTY_1 = '" + G_THIRD_PARTY_1 + @"',
                            G_THIRD_PARTY_2 = '" + G_THIRD_PARTY_2 + @"',
                            G_THIRD_PARTY_3 = '" + G_THIRD_PARTY_3 + @"',
                            G_THIRD_PARTY_4 = '" + G_THIRD_PARTY_4 + @"',
                            G_THIRD_PARTY_5 = '" + G_THIRD_PARTY_5 + @"',
                            G_THIRD_PARTY_6 = '" + G_THIRD_PARTY_6 + @"',
                            G_THIRD_PARTY_7 = '" + G_THIRD_PARTY_7 + @"',
                            G_THIRD_PARTY_8 = '" + G_THIRD_PARTY_8 + @"',
                            G_THIRD_PARTY_AUDITOR = '" + G_THIRD_PARTY_AUDITOR + @"'
                        where G_DEPTCODE = '" + G_DEPTCODE + "' and trunc(G_DATE) = to_date('" + G_DATE.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
            DB.ExecuteNonQuery(sql);
        }
        public void InsertTier1(DataBase DB, string G_DEPTCODE, DateTime G_DATE, string G_1, string G_2, string G_3, string G_4,
                        string G_5, string G_6, string G_7, string G_8, string G_RESULT, string G_AUDITOR,
                         string username )
        {
            string now = DateTime.Now.ToString(Parameters.dateFormat + " hh:mm:ss");
            string sql = @"insert into TMS_TIER1 values('"+ G_DEPTCODE + "', to_date('" + G_DATE.ToString(Parameters.dateFormat)+ "','"+Parameters.dateFormat + "')," +
                "to_date('" + now + "','"+Parameters.dateTimeFormat + "'),'"+username+@"',
                null,null,'"+ G_1 + "','" + G_2 + "','" + G_3 + "','" + G_4 + "','" + G_5 + "','" + G_6 + "','" + G_7 + "','" + G_8 + @"',
                    '"+ G_RESULT + "','"+ G_AUDITOR + "')";
            DB.ExecuteNonQuery(sql);
        }
        public void InsertTier1Standard(DataBase DB, string G_DEPTCODE, DateTime G_DATE, 
            string G_SUPERVISOR_1, string G_SUPERVISOR_2, string G_SUPERVISOR_3, string G_SUPERVISOR_4,
            string G_SUPERVISOR_5, string G_SUPERVISOR_6, string G_SUPERVISOR_7, string G_SUPERVISOR_8, string G_SUPERVISOR_AUDITOR,
            string G_VSM_1, string G_VSM_2, string G_VSM_3, string G_VSM_4,
            string G_VSM_5, string G_VSM_6, string G_VSM_7, string G_VSM_8, string G_VSM_AUDITOR,
            string G_THIRD_PARTY_1, string G_THIRD_PARTY_2, string G_THIRD_PARTY_3, string G_THIRD_PARTY_4,
            string G_THIRD_PARTY_5, string G_THIRD_PARTY_6, string G_THIRD_PARTY_7, string G_THIRD_PARTY_8, string G_THIRD_PARTY_AUDITOR,
            string username)
        {
            string now = DateTime.Now.ToString(Parameters.dateFormat + " hh:mm:ss");
            string sql = @"insert into TMS_TIER1_STANDARD values('" + G_DEPTCODE + "', to_date('" + G_DATE.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')," +
                "to_date('" + now + "','" + Parameters.dateTimeFormat + "'),'" + username + @"',
                null,null,
                '" + G_SUPERVISOR_1 + "','" + G_SUPERVISOR_2 + "','" + G_SUPERVISOR_3 + "','" + G_SUPERVISOR_4 + @"',
                '" + G_SUPERVISOR_5 + "','" + G_SUPERVISOR_6 + "','" + G_SUPERVISOR_7 + "','" + G_SUPERVISOR_8 + "','" + G_SUPERVISOR_AUDITOR + @"',
                '" + G_VSM_1 + "','" + G_VSM_2 + "','" + G_VSM_3 + "','" + G_VSM_4 + @"',
                '" + G_VSM_5 + "','" + G_VSM_6 + "','" + G_VSM_7 + "','" + G_VSM_8 + "','" + G_VSM_AUDITOR + @"',
                '" + G_THIRD_PARTY_1 + "','" + G_THIRD_PARTY_2 + "','" + G_THIRD_PARTY_3 + "','" + G_THIRD_PARTY_4 + @"',
                '" + G_THIRD_PARTY_5 + "','" + G_THIRD_PARTY_6 + "','" + G_THIRD_PARTY_7 + "','" + G_THIRD_PARTY_8 + "','" + G_THIRD_PARTY_AUDITOR + @"')";
            DB.ExecuteNonQuery(sql);
        }
        public DataTable GetTHTByART(DataBase DB, string ART)
        {
            string sql = @"select G_THT from tms_tier1_tht where G_ART = '"+ART+"'";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public void UpdateTHT(DataBase DB, string ART, string THT)
        {
            string sql = @"update tms_tier1_tht set
                            G_THT = '" + THT+@"'
                        where G_ART = '"+ART+"'";
            DB.ExecuteNonQuery(sql);
        }
        public void InsertTHT(DataBase DB, string ART, string THT)
        {
            string sql = @"insert into tms_tier1_tht values('" + ART+"','"+THT+"')";
            DB.ExecuteNonQuery(sql);
        }
        public DataTable Tier1_WeekSafety(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select day, coalesce(count,0) as count
                            from 
                            (select happeneddate as WORKDATE, count(*) as COUNT
                            from mfg_accident
                            where deptcode in (select department_code from base005m)
                            and deptcode='"+line+@"'
                            and happeneddate between to_date('"+ firstDate + "','"+Parameters.dateFormat+"') and to_date('"+seventhDate+"','"+Parameters.dateFormat+@"')
                            group by happeneddate) a
                            right outer join 
                            (SELECT
                              (to_date('"+seventhDate+"','"+Parameters.dateFormat+@"') - level + 1) AS day
                            FROM
                              dual
                            CONNECT BY LEVEL <= (to_date('"+seventhDate+"','"+Parameters.dateFormat+"') - to_date('"+firstDate+"','"+Parameters.dateFormat+@"') + 1)) b on a.workdate = b.day
                            order by day asc";
            return DB.GetDataTable(sql);
        }
        public DataTable GetDeptType(DataBase DB, string dept)
        {
            string sql = @"select UDF01 from base005m where department_code = '"+dept+"'";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekOutput(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select coalesce(QTY,0) as QTY, coalesce(WORK_QTY,0) as WORK_QTY, day as work_day
                            from
                            (select QTY, WORK_QTY, x.work_day
                            from
                            (select sum(a.finish_qty) as QTY, trunc(a.Work_day) as work_day
                                  from SJQDMS_WORK_DAY a
                                 where a.d_dept = '" + line + @"' 
                                  and trunc(a.Work_day) between to_date('" + firstDate + "','" + Parameters.dateFormat + "') and to_date('" + seventhDate + "','" + Parameters.dateFormat + @"')
                                  and inout_pz='OUT'
                                  group by trunc(a.Work_day)) x
                                  join 
                                  (select work_qty, trunc(work_day) as work_day from sjqdms_worktarget 
                                  where d_dept = '" + line + @"' and
                                  trunc(Work_day) between to_date('" + firstDate + "', '" + Parameters.dateFormat + "') and to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"')) y on x.work_day = y.work_day
                                  order by x.work_day) a right outer join 
                                      (SELECT
                                        (to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"') - level + 1) AS day
                                      FROM
                                        dual
                                      CONNECT BY LEVEL <= (to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"') - to_date('" + firstDate + "', '" + Parameters.dateFormat + @"') + 1)) b 
                                      on a.work_day = b.day
                                      order by day asc";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_Kaizen(DataBase DB, string dept, DateTime date)
        {
            string sql = @"select sum(year) as year, sum(month) as month, sum(perpeople) as perpeople
                            from (
                            select sum(received) as year , 0 as month, round(sum(accepted)/GF_KAIZEN_LINE_PERSON(deptcode,'"+ date.ToString("yyyy")+ @"'),2) as perpeople
                            from mfg_kaizen
                            where deptcode = '"+dept+ @"'
                            and deptcode in (select department_code from base005m)
                            and to_char(createddate,'yyyy')  = '" + date.ToString("yyyy") + @"'
                            group by deptcode
                            union all
                            select 0 as year , sum(received) as month, 0 as perpeople
                            from mfg_kaizen
                            where deptcode = '" + dept+ @"'
                            and deptcode in (select department_code from base005m)
                            and to_char(createddate,'yyyy/mm')  = '" + date.ToString("yyyy/MM").Replace('-','/') + @"')";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekPPHTarget(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select work_day, coalesce((case when gf_linemanpower(d_dept, to_char(work_day,'" + Parameters.dateFormat + @"')) <> 0 then round(work_qty/gf_linemanpower(d_dept, to_char(work_day,'" + Parameters.dateFormat + @"')),2)  end),0) as  PPHTarget
                            from sjqdms_worktarget  
                            where d_dept = '" + line+"' and work_day between to_date('"+ firstDate + "','" + Parameters.dateFormat + @"') and to_date('" + seventhDate + "','" + Parameters.dateFormat + @"')
                            order by work_day";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekPPH(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select day as work_date, coalesce(qty,0) as qty, coalesce(MANPOWER,'0') as MANPOWER
                            from (
                            SELECT to_char(scan_date,'" + Parameters.dateFormat + @"') as WORK_DATE,sum(label_qty) as qty,GF_LINEMANPOWER('" + line + @"',to_char(scan_date,'" + Parameters.dateFormat + @"')) as MANPOWER from  (           
                                   SELECT  scan_date,label_qty  from mes_label_d,base005m 
                              where   
                              inout_pz='OUT' 
                              and  scan_date between  to_date('" + firstDate + "','" + Parameters.dateFormat + @"') and to_date('" + seventhDate + "','" + Parameters.dateFormat + @"')
                              and department_code=scan_detpt and department_code='"+ line + @"')
                              group by  to_char(scan_date,'" + Parameters.dateFormat + @"') order by to_char(scan_date,'" + Parameters.dateFormat + @"')) a right outer join 
                               (SELECT
                                    to_char(to_date('" + seventhDate + "','" + Parameters.dateFormat + @"') - level + 1,'" + Parameters.dateFormat + @"') AS day
                                  FROM
                                    dual
                                  CONNECT BY LEVEL <= (to_date('" + seventhDate + "','" + Parameters.dateFormat + @"') - to_date('" + firstDate + "','" + Parameters.dateFormat + @"') + 1)) b 
                                  on a.WORK_DATE = b.day
                                  order by day asc";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekLLER(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select work_day, coalesce(round(THT/TT/OPERATORNO*10,2),0) as LLER
                        from(
                        select work_day, WORK_QTY,GF_LINEOperatorNo(d_dept,to_char(work_day,'yyyy/mm/dd')) as OPERATORNO,
                        round(3600/WORK_QTY/10,2) as TT,GF_LINETHT(d_dept,to_char(work_day,'yyyy/mm/dd')) as THT
                        from SJQDMS_WORKTARGET
                        where D_DEPT = '" + line+ @"'
                            and WORK_DAY between to_date('" + firstDate + "','" + Parameters.dateFormat + @"') and to_date('" + seventhDate + "','" + Parameters.dateFormat + @"'))";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekMulti(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select work_day,  coalesce((case when operatorno <> 0 then round(multiskill/operatorno*100,2) end),0) as multi
                            from (
                            select distinct trunc(work_day) as work_day, GF_LINEMULTISKILL(d_dept,to_char(work_day,'yyyy/mm/dd')) as multiskill, GF_LINEOperatorNo(d_dept,to_char(work_day,'yyyy/mm/dd')) as operatorno
                            from sjqdms_work_day
                            where d_dept = '" + line + @"'
                            and trunc(work_day) between to_date('" + firstDate + "','" + Parameters.dateFormat + @"') and to_date('" + seventhDate + "','" + Parameters.dateFormat + @"')
                            order by trunc(work_day) )";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public void InsertTier1_Downtime(DataBase DB, string dept, string downtime, DateTime date)
        {
            string sql = @"insert into tms_tier1_downtime values('" + dept + "',to_date('" + date.ToString(Parameters.dateFormat) + "','"+Parameters.dateFormat+"'),'"+downtime+"')";
            DB.ExecuteNonQuery(sql);
        }
        public void UpdateTier1_Downtime(DataBase DB, string dept, string downtime, DateTime date)
        {
            string sql = @"update tms_tier1_downtime set
                            G_DOWNTIME = '" + downtime + @"'
                        where G_DEPTCODE = '" + dept + "' and trunc(G_DATE) = to_date('"+date.ToString(Parameters.dateFormat)+"','"+Parameters.dateFormat+"')";
            DB.ExecuteNonQuery(sql);
        }
        public DataTable GetTier1_Downtime(DataBase DB, string dept, DateTime date)
        {
            string sql = @"select G_DOWNTIME from tms_tier1_downtime 
                    where G_DEPTCODE = '" + dept + "' and trunc(G_DATE) = to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekDowntime(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select G_date,g_downtime
            from tms_tier1_downtime
            where g_deptcode = '"+line+@"'
            and trunc(g_date) between to_date('"+ firstDate + "','"+Parameters.dateFormat+ "') and to_date('"+ seventhDate + "','" + Parameters.dateFormat + @"')
            order by g_date";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public void InsertTier1_COT(DataBase DB, string dept, DateTime date, string target, string actual)
        {
            string sql = @"insert into tms_tier1_cot values('" + dept + "',to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "'),'" + target + "','" + actual + "')";
            DB.ExecuteNonQuery(sql);
        }
        public void UpdateTier1_COT(DataBase DB, string dept,  DateTime date,string target, string actual)
        {
            string sql = @"update tms_tier1_cot set ";
            if (target != "") {
                sql += @"G_TARGET_COT = '" + target + @"' ";
            }
            if (actual != "")
            {
                sql += @"G_ACTUAL_COT = '" + actual + @"' ";
            }
            sql +=@"where G_DEPTCODE = '" + dept + "' and trunc(G_DATE) = to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
            DB.ExecuteNonQuery(sql);
        }
        public DataTable GetTier1_COT(DataBase DB, string dept, DateTime date)
        {
            string sql = @"select G_TARGET_COT,G_ACTUAL_COT from tms_tier1_cot 
                    where G_DEPTCODE = '" + dept + "' and trunc(G_DATE) = to_date('" + date.ToString(Parameters.dateFormat) + "','" + Parameters.dateFormat + "')";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekCOT(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select G_date,G_TARGET_COT,G_ACTUAL_COT
            from tms_tier1_cot
            where g_deptcode = '" + line + @"'
            and trunc(g_date) between to_date('" + firstDate + "','" + Parameters.dateFormat + "') and to_date('" + seventhDate + "','" + Parameters.dateFormat + @"')
            order by g_date";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekHourlyOutput(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select coalesce(QTY,0) as QTY, coalesce(WORK_QTY,0) as WORK_QTY, day as work_day, coalesce(work_hours,0) as work_hours
                            from(
select distinct QTY, WORK_QTY, x.work_day, work_hours
                          from (select sum(a.finish_qty) as QTY, trunc(a.Work_day) as work_day
                                  from SJQDMS_WORK_DAY a
                                 where a.d_dept = '" + line + @"'
                                   and trunc(a.Work_day) between to_date('" + firstDate + "','" + Parameters.dateFormat + @"') and
                                       to_date('" + seventhDate + "','" + Parameters.dateFormat + @"')
                                   and inout_pz = 'OUT'
                                 group by trunc(a.Work_day)) x
                          join (select work_qty, trunc(work_day) as work_day
                                  from sjqdms_worktarget
                                 where d_dept = '" + line + @"'
                                   and trunc(Work_day) between to_date('" + firstDate + "','" + Parameters.dateFormat + @"') and
                                       to_date('" + seventhDate + "','" + Parameters.dateFormat + @"')) y
                            on x.work_day = y.work_day ,
                            (select * from mes_workinghours_01 where d_dept = '"+ line + @"') p
                            where trunc(x.work_day) = trunc(p.work_day)
                                  and d_dept = p.d_dept 
                         order by x.work_day) a right outer join 
                                      (SELECT
                                        (to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"') - level + 1) AS day
                                      FROM
                                        dual
                                      CONNECT BY LEVEL <= (to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"') - to_date('" + firstDate + "', '" + Parameters.dateFormat + @"') + 1)) b 
                                      on a.work_day = b.day
                                      order by day asc";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public DataTable Tier1_WeekRFT(DataBase DB, string line, string firstDate, string seventhDate)
        {
            string sql = @"select coalesce(RFT,0) as RFT, day as RIQI
                                from(
                                select NVL(ROUND((SUM(sjqdms_mp.mp010) - SUM(sjqdms_mp.mp011)) /
                        SUM(sjqdms_mp.mp010) * 100,
                                         2),
                                   0) as rft,
                               to_char(insert_date, '"+Parameters.dateFormat+ @"') as riqi
                          from sjqdms_mp
                         where insert_date between to_date('" + firstDate + "', '" + Parameters.dateFormat + @"') and
                               to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"')
                           and MP003 = '" + line + @"'
                         group by to_char(insert_date, '" + Parameters.dateFormat + @"')
                         order by to_char(insert_date, '" + Parameters.dateFormat + @"')
                         ) a right outer join 
                                (SELECT
                                to_char((to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"') - level + 1),'"+ Parameters.dateFormat + @"') AS day
                              FROM
                                dual
                              CONNECT BY LEVEL <= (to_date('" + seventhDate + "', '" + Parameters.dateFormat + @"') - to_date('" + firstDate + "', '" + Parameters.dateFormat + @"') + 1)) b 
                              on a.riqi = b.day
                              order by day asc";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        #endregion

    }
}
