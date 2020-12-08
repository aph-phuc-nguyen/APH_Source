using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class ProductionDashBoardDAL
    {
        public DataTable Query(DataBase DB, string userCode, string companyCode, string vCompany, string vPlant, string vDept, string vRountNo)
        {
            //            string sql = @"select rownum as sno,a.* from sjqdms_orginfo d,(SELECT  MAX(B.UDF01) as udf01,MAX(B.UDF05) as udf05,MAX(B.UDF06) as udf06,b.department_code as scan_detpt,max(b.department_name) as department_name,max(c.work_qty) as target,
            //SUM(DECODE(hours,'07',label_qty,null)) H07,
            //SUM(DECODE(hours,'08',label_qty,null)) H08,
            //SUM(DECODE(hours,'09',label_qty,null)) H09,
            //SUM(DECODE(hours,'10',label_qty,null)) H10,
            //SUM(DECODE(hours,'11',label_qty,null)) H11,
            //SUM(DECODE(hours,'12',label_qty,null)) H12,
            //SUM(DECODE(hours,'13',label_qty,null)) H13,
            //SUM(DECODE(hours,'14',label_qty,null)) H14,
            //SUM(DECODE(hours,'15',label_qty,null)) H15,
            //SUM(DECODE(hours,'16',label_qty,null)) H16,
            //SUM(DECODE(hours,'17',label_qty,null)) H17,
            //SUM(DECODE(hours,'18',label_qty,null)) H18,
            //SUM(DECODE(hours,'19',label_qty,null)) H19,
            //sum(A.label_qty) as Total,
            //sum(A.label_qty)-max(c.work_qty) as Balance,
            //case when nvl(max(c.work_qty),0)=0
            //  then 
            //   '无目标产量'
            //  else
            //  round(sum(A.label_qty)/max(c.work_qty),3)*100||'%'
            //end as Acheivement,
            //case when nvl(max(c.work_qty),0)=0
            //  then 
            //   0
            //  else
            //  round(sum(A.label_qty)/max(c.work_qty),3)*100
            //end as Bcheivement  
            //FROM  base005m b left join (
            //select  scan_detpt,hours,SUM(label_qty)  label_qty FROM 
            //(
            //select scan_detpt,to_char(scan_date,'HH24') as hours, label_qty from mes_label_d
            //where scan_date>=TRUNC(sysdate) and scan_date<TRUNC(sysdate+1) and inout_pz='OUT' group by  scan_detpt,hours ) 
            //A on b.department_code=A.scan_detpt
            //left join sjqdms_worktarget c on b.department_code=c.d_dept and c.work_day>=TRUNC(sysdate) and c.work_day<TRUNC(sysdate+1)
            //group by  b.department_code ) a  where d.code=udf05 and udf06='Y' ";
            string sql = @"select rownum as sno,a.* from sjqdms_orginfo d,(
                        SELECT  MAX(B.UDF01) as udf01,MAX(B.UDF05) as udf05,MAX(B.UDF06) as udf06,b.department_code as scan_detpt,max(b.department_name) as department_name,max(c.work_qty) as target,
                        SUM(DECODE(hours,'07',label_qty,null)) H07,
                        SUM(DECODE(hours,'08',label_qty,null)) H08,
                        SUM(DECODE(hours,'09',label_qty,null)) H09,
                        SUM(DECODE(hours,'10',label_qty,null)) H10,
                        SUM(DECODE(hours,'11',label_qty,null)) H11,
                        SUM(DECODE(hours,'12',label_qty,null)) H12,
                        SUM(DECODE(hours,'13',label_qty,null)) H13,
                        SUM(DECODE(hours,'14',label_qty,null)) H14,
                        SUM(DECODE(hours,'15',label_qty,null)) H15,
                        SUM(DECODE(hours,'16',label_qty,null)) H16,
                        SUM(DECODE(hours,'17',label_qty,null)) H17,
                        SUM(DECODE(hours,'18',label_qty,null)) H18,
                        SUM(DECODE(hours,'19',label_qty,null)) H19,
                        sum(A.label_qty) as Total,
                        sum(A.label_qty)-max(c.work_qty) as Balance,
                        case 
                          when nvl(max(c.work_qty),0)=0 then 
                           '无目标产量'
                           when GF_WORKINGHOURS_PERC(department_code)=0 then
                            '无工作历资料'
                          else
                          nvl(round(sum(A.label_qty)/max(c.work_qty),3)*100,0)||'%'
                        end as Acheivement,
                        case when nvl(max(c.work_qty),0)=0
                          then 
                           0
                          else
                          nvl(round(sum(A.label_qty)/max(c.work_qty),3),0)*100
                        end as Bcheivement,
                        GF_WORKINGHOURS_PERC(department_code) as Ccheivement
                        FROM  base005m b left join (
                        select  scan_detpt,hours,SUM(label_qty)  label_qty FROM 
                        (
                        select scan_detpt,to_char(scan_date,'HH24') as hours, label_qty from mes_label_d
                        where scan_date>=TRUNC(sysdate) and scan_date<TRUNC(sysdate+1) and inout_pz='OUT')
                        group by  scan_detpt,hours
                        ) A on b.department_code=A.scan_detpt
                        left join sjqdms_worktarget c on b.department_code=c.d_dept and c.work_day>=TRUNC(sysdate) and c.work_day<TRUNC(sysdate+1)
                        group by  b.department_code ) a 
                        where 
                        d.code=udf05 and
                        udf06='Y'";
            if (!string.IsNullOrWhiteSpace(vCompany))
            {
                sql += " and d.company='" + vCompany + "' ";
            }
            if (!string.IsNullOrWhiteSpace(vPlant))
            {
                sql += " and udf05='" + vPlant + "' ";
            }
            if (!string.IsNullOrWhiteSpace(vDept))
            {
                sql += " and scan_detpt='" + vDept + "' ";
            }
            if (!string.IsNullOrWhiteSpace(vRountNo))
            {
                sql += " and udf01='" + vRountNo + "' ";
            }
            sql += " order by a.Bcheivement,a.scan_detpt";
            return DB.GetDataTable(sql);
        }

        public DataTable LoadRoutNo(DataBase DB, string userCode, string companyCode)
        {
            string sql = @"select * from base24M";
            return DB.GetDataTable(sql);
        }

        public DataTable LoadPlant(DataBase DB, string userCode, string companyCode)
        {
            string sql = @"select distinct code,org from sjqdms_orginfo";
            return DB.GetDataTable(sql);
        }

        public DataTable LoadOrg(DataBase DB, string userCode, string companyCode)
        {
            string sql = @"select distinct company from sjqdms_orginfo where company is not null";
            return DB.GetDataTable(sql);
        }
    }
}
