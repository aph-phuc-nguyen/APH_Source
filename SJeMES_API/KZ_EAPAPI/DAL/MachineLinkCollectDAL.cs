using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_EAPAPI.DAL
{
    public class MachineLinkCollectDAL
    {
        public DataTable QuerySeCutPart(DataBase DB, string vSeId)
        {
            string sql = "select part_name from vw_eqp_cut_part where 1=1 ";
            sql += " and se_id='" + vSeId + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryEapStatusById(DataBase DB, string vMachineID)
        {
            string sql = @"select * from vw_eap_status_present where 1 = 1";
            sql += " and machine_id ='" + vMachineID + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryQty(DataBase DB, string vDept, string vOvenId, string vPressId, string vFreezerId)
        {
            string sql = @"select * from (select nvl(sum(label_qty),0) as qty_in from mes_label_d where scan_date >= to_date((select time1 from vw_eap_daybefor_times),'yyyy/mm/dd hh24:mi:ss') and scan_date < to_date((select time2 from vw_eap_daybefor_times),'yyyy/mm/dd hh24:mi:ss') and inout_pz = 'IN'";
            sql += " and scan_detpt = '" + vDept + "'),(select nvl(sum(qty),0) as qty_oven from EAP_OVEN_PRODUCTION_D where transfer_time >= (select time1 from vw_eap_daybefor_times) and transfer_time < (select time2 from vw_eap_daybefor_times)";
            sql += " and machine_id = '" + vOvenId + "'),(select nvl(sum(left_qty + right_qty)/2,0) as qty_press from EAP_PRESS_PRODUCTION_D where transfer_time >= (select time1 from vw_eap_daybefor_times) and transfer_time < (select time2 from vw_eap_daybefor_times)";
            sql += " and machine_id = '" + vPressId + "'),(select nvl(sum(qty),0) as qty_freezer from EAP_FREEZER_PRODUCTION_D where transfer_time >= (select time1 from vw_eap_daybefor_times) and transfer_time < (select time2 from vw_eap_daybefor_times)";
            sql += " and machine_id = '" + vFreezerId + "'),(select nvl(sum(label_qty),0) as qty_out from mes_label_d where scan_date >= to_date((select time1 from vw_eap_daybefor_times),'yyyy/mm/dd hh24:mi:ss') and scan_date < to_date((select time2 from vw_eap_daybefor_times),'yyyy/mm/dd hh24:mi:ss') and inout_pz = 'OUT'";
            sql += " and scan_detpt = '" + vDept + "')union all select * from (select nvl(sum(label_qty),0) as qty_in from mes_label_d where scan_date >= (sysdate-1/24) and inout_pz = 'IN'";
            sql += " and scan_detpt = '" + vDept + "'),(select nvl(sum(qty),0) as qty_oven from EAP_OVEN_PRODUCTION_D where transfer_time >= to_char(sysdate-1/24,'yyyymmddhh24miss')";
            sql += " and machine_id = '" + vOvenId + "'),(select nvl(sum(left_qty + right_qty)/2,0) as qty_press from EAP_PRESS_PRODUCTION_D where transfer_time >= to_char(sysdate-1/24,'yyyymmddhh24miss')";
            sql += " and machine_id = '" + vPressId + "'),(select nvl(sum(qty),0) as qty_freezer from EAP_FREEZER_PRODUCTION_D where transfer_time >= to_char(sysdate-1/24,'yyyymmddhh24miss')";
            sql += " and machine_id = '" + vFreezerId + "'),(select nvl(sum(label_qty),0) as qty_out from mes_label_d where scan_date >= (sysdate-1/24) and inout_pz = 'OUT'";
            sql += " and scan_detpt = '" + vDept + "')";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryEqpStatusByDept(DataBase DB, string vDept)
        {
            string sql = @"select a.machine_no, a.machine_name, a.udf02 as machine_type,c.type_code,c.description,c.status from mes030m a, base005m b,vw_eap_status_present c
where a.udf03 = 'Y' and a.udf01 = b.department_code  and a.machine_no = c.machine_id";
            sql += " and a.udf01 = '" + vDept + "'";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryOvenParmById(DataBase DB, string vMachineID)
        {
            string sql = @"select machine_id,message_class,TRANSFER_TIME,KWH,SPEED,UPER_SETUP1,UUCL1,ULCL1,UPER_ACTUAL1,LOWER_SETUP1,LUCL1,LLCL1,LOWER_ACTUAL1,UPER_SETUP2,UUCL2,ULCL2,UPER_ACTUAL2,LOWER_SETUP2,LUCL2,LLCL2,LOWER_ACTUAL2,UPER_SETUP3,UUCL3,ULCL3,UPER_ACTUAL3,LOWER_SETUP3,LUCL3,LLCL3,LOWER_ACTUAL3 
from (select * from EAP_OVEN_PRODUCTION_D where 1 = 1";
            sql += " and machine_id = '" + vMachineID + "' order by transfer_time desc) where rownum = 1";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryFreezerParmById(DataBase DB,string vMachineID)
        {
            string sql = @"select MACHINE_ID,MESSAGE_CLASS,TRANSFER_TIME,KWH,SPEED,SETUP,UCL,LCL,ACTUAL from (select * from EAP_FREEZER_PRODUCTION_D where 1 = 1 ";
            sql += " and machine_id = '" + vMachineID + "' order by transfer_time desc) where rownum = 1";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryPressParmById(DataBase DB, string vMachineID)
        {
            string sql = @"select MACHINE_ID,MESSAGE_CLASS,TRANSFER_TIME,KWH,(LP1+LP2+LP3+LP4+LP5+LP6+LP7+LP8+LP9+LP10)/10 as LP_AVERAGE,(RP1+RP2+RP3+RP4+RP5+RP6+RP7+RP8+RP9+RP10)/10 as RP_AVERAGE
from (select * from eap_press_production_d where 1 = 1 ";
            sql += " and machine_id = '" + vMachineID + "'order by transfer_time desc) where rownum = 1";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryEqpAlarmByDept(DataBase DB, string vDept)
        {
            string sql = @"select a.machine_no, a.machine_name, a.udf02 as machine_type,b.TRANSFER_TIME,b.ALARM_CODE,b.DESCRIPTION,c.cn from mes030m a,EAP_ALARM_D b,EAP_ALARM_INFO c
where a.udf03 = 'Y' and a.machine_no = b.machine_id and a.udf02 = c.machine_type and b.ALARM_CODE = c.code and b.TRANSFER_TIME > to_char(sysdate-2,'yyyymmdd')||'000000' ";
            sql += " and a.udf01 = '" + vDept + "' ORDER BY b.TRANSFER_TIME DESC";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryOvenHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            string sql = @"select * from vw_eap_oven_hour_output where 1 = 1";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and days = '" + vDate + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryFreezerHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            string sql = @"select * from vw_eap_freezer_hour_output where 1 = 1";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and days = '" + vDate + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryPressHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            string sql = @"select * from vw_eap_press_hour_output where 1 = 1";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and days = '" + vDate + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            string sql = @"select * from vw_eap_cut_hour_output where 1 = 1";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and days = '" + vDate + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryEqpHourOutput(DataBase DB, string vDate, string vDept)
        {
            string sql = @"select a.hours,nvl(b.qty,0) as oven_qty,nvl(c.qty,0) as press_qty,nvl(d.qty,0) as freezer_qty
from ((select days,hours,d_dept from vw_eap_oven_hour_output) union
(select days,hours,d_dept from vw_eap_press_hour_output) union
(select days,hours,d_dept from vw_eap_freezer_hour_output)) a 
left join vw_eap_oven_hour_output b on a.days = b.days and a.hours = b.hours and a.d_dept = b.d_dept
left join vw_eap_press_hour_output c on a.days = c.days and a.hours = c.hours and a.d_dept = c.d_dept
left join vw_eap_freezer_hour_output d on a.days = d.days and a.hours = d.hours and a.d_dept = d.d_dept where 1 = 1";
            sql += " and a.days = '" + vDate + "'";
            sql += " and a.d_dept = '" + vDept + "'";
            sql += " order by a.hours asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryOvenHourParams(DataBase DB, string vMachineID, string vMaxTime, string vMinTime)
        {
            string sql = @"select transfer_time as times,uper_actual1,lower_actual1,uper_actual2,lower_actual2,uper_actual3,lower_actual3 from  EAP_OVEN_PRODUCTION_D where 1 = 1";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and transfer_time < '" + vMaxTime + "'";
            //sql += " and transfer_time >= '" + vMinTime + "' - '300'";
            sql += " and transfer_time >= '" + vMinTime + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryFreezerHourParams(DataBase DB, string vMachineID, string vMaxTime, string vMinTime)
        {
            string sql = @"select transfer_time as times,actual,ucl,lcl,setup from  EAP_FREEZER_PRODUCTION_D where 1 = 1";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and transfer_time < '" + vMaxTime + "'";
            //sql += " and transfer_time >= '" + vMinTime + "' - '300'";
            sql += " and transfer_time >= '" + vMinTime + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable Query7DaysRunTime(DataBase DB, string vMachineID)
        {
            string sql = @"select round(NVL(b.duration,0),0) as day_duration,a.days from (select * from (select to_char(sysdate- level, 'yyyymmdd') days FROM DUAL connect BY LEVEL <= 7)) a 
LEFT join vw_eap_cut_day1_status_time b on a.days = b.days and b.type_code = 'B02'";
            sql += " and b.machine_id = '" + vMachineID + "' order by a.days asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCut7DaysOutput(DataBase DB, string vMachineID)
        {
            string sql = @"select sum(B.QTY) as day_QTY,a.days from (select * from (select to_char(sysdate- level, 'yyyymmdd') days FROM DUAL connect BY LEVEL <= 7)) a 
LEFT join vw_eap_cut_hour_output b on a.days = b.days";
            sql += " and b.machine_id = '" + vMachineID + "' GROUP BY a.days order by a.days asc";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCut7DayOee(DataBase DB, string vMachineID)
        {
            string sql = string.Format(@"select round(nvl(b.duration,0)*nvl(c.rft,0)*nvl(d.rate,0)/60000,1) as oee,a.days from (select * from (select to_char(sysdate- level, 'yyyymmdd') days FROM DUAL connect BY LEVEL <= 7)) a 
left join vw_eap_cut_shift_run_time b on a.days = b.days and b.machine_id = '{0}' and b.shift = 'A'
left join EAP_RFT_D c on b.machine_id = c.machine_id and c.shift_day =  a.days and c.shift =  'A'
left join EAP_CAPACITY_RATE_D d on b.machine_id = d.machine_id and d.shift_day = a.days and d.shift =  'A' order by a.days asc", vMachineID);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutDayStatusTime(DataBase DB, string vMachineID, string vDate)
        {
            string sql = @"select sum(duration) as duration,type from (select machine_id,days,type_code,decode(type_code,'B01','空闲','B02','运行','C02','故障','C03','故障','C06','暂停','C09','暂停','C10','暂停','其它')
as type,duration from vw_eap_cut_day1_status_time where type_code <> 'C05') where 1 = 1 ";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and days = '" + vDate + "' group by type ";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutRunTimeByDay(DataBase DB, string vMachineID, string vDate)
        {
            string sql = @"SELECT machine_id,days,duration FROM vw_eap_cut_day1_status_time WHERE TYPE_CODE = 'B02' ";
            sql += " and machine_id like '%" + vMachineID + "%'";
            sql += " and days like '%" + vDate + "%' ORDER BY DAYS desc,MACHINE_ID";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutOutPutByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            string sql = string.Format(@"select a.machine_id,b.machine_code,to_char(to_date(a.udf02,'yyyymmdd'),'yyyy/mm/dd') as days,a.udf01 as shift,sum(a.qty) as qty from EAP_CUT_PRODUCTION_D a left join EAP_CODE_CONTRAST b on a.machine_id = b.machine_id
where b.type = 'CU' and a.machine_id like '%{0}%' and a.udf02 like '%{1}%' and a.udf01 like '%{2}%'group by a.machine_id,a.udf02,a.udf01,b.machine_code order by a.udf02 desc,b.machine_code asc,a.udf01 asc", vMachineID, vDate, vShift);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutRunTimeByDetail(DataBase DB, string vMachineID, string vDate)
        {
            string sql = @"select MACHINE_ID,TO_CHAR(TO_DATE(transfer_time,'yyyy/mm/dd hh24:mi:ss'),'yyyy/mm/dd hh24:mi:ss') as star_time,duration from vw_eap_status_duration 
where message_class = '3000' AND SUBSTR(transfer_time,9,4) > '0700' AND SUBSTR(transfer_time,9,4) < '1930' and type_code = 'B02' ";
            sql += " and machine_id = '" + vMachineID + "'";
            sql += " and SUBSTR(transfer_time,0,8) = '" + vDate + "' ORDER BY transfer_time DESC";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutOutPutByDetail(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            string sql = string.Format(@"select machine_id,to_char(to_date(transfer_time,'yyyy/mm/dd hh24:mi:ss'),'yyyy/mm/dd hh24:mi:ss') as time,qty,layer,udf01 as shift from eap_cut_production_d 
where machine_id = '{0}' and udf02 = '{1}' and udf01 = '{2}' order by transfer_time desc", vMachineID, vDate, vShift);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryOeeByDeptShift(DataBase DB, string vDept, string vDate)
        {
            string sql = string.Format(@"select c.machine_code,c.machine_id,'A' as shift,round(nvl(d.duration,0),1) as duration,e.rft,f.rate,round(nvl(d.duration,0)*e.rft*f.rate/60000,1) as oee from 
(select a.machine_code,a.machine_id,'{0}' as shift_day from EAP_CODE_CONTRAST a,mes030m b where a.machine_id = b.machine_no and b.udf01 = '{1}') c 
left join vw_eap_cut_shift_run_time d on c.machine_id = d.machine_id and d.days = c.shift_day and d.shift = 'A' 
left join EAP_RFT_D e on c.machine_id = e.machine_id and e.shift = 'A' and e.shift_day =  c.shift_day
left join EAP_CAPACITY_RATE_D f on c.machine_id = f.machine_id and f.shift = 'A' and f.shift_day =  c.shift_day order by c.machine_code asc", vDate, vDept);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryDayOutByDept(DataBase DB, string vDept, string vDate)
        {
            string sql = string.Format(@"select c.machine_code,c.machine_id,nvl(d.qty,0) as a_qty,nvl(e.qty,0) as b_qty from 
(select a.machine_code,a.machine_id from EAP_CODE_CONTRAST a,mes030m b where a.machine_id = b.machine_no and b.udf01 = '{0}') c
left join (select machine_id,sum(qty) as qty from EAP_CUT_PRODUCTION_D where udf01 = 'A' and udf02 = '{1}' group by machine_id) d on c.machine_id = d.machine_id
left join (select machine_id,sum(qty) as qty from EAP_CUT_PRODUCTION_D where udf01 = 'B' and udf02 = '{1}' group by machine_id) e on c.machine_id = e.machine_id order by c.machine_code asc", vDept,vDate);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutRateByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            string sql = string.Format(@"select machine_id,to_char(to_date(shift_day,'yyyymmdd'),'yyyy/mm/dd') as shift_day,shift,rate,grt_user,grt_date from EAP_CAPACITY_RATE_D where shift like '%{0}%' and machine_id like '%{1}%' and shift_day like '%{2}%' order by shift_day desc", vShift, vMachineID, vDate);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutRftByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            string sql = string.Format(@"select machine_id,to_char(to_date(shift_day,'yyyymmdd'),'yyyy/mm/dd') as shift_day,shift,rft,grt_user,grt_date from EAP_RFT_D where shift like '%{0}%' and machine_id like '%{1}%' and shift_day like '%{2}%' order by shift_day desc", vShift, vMachineID, vDate);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryCutOeeByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            string sql = string.Format(@"select d.machine_code,d.machine_id,d.days,d.shift,d.duration,e.rft,f.rate,round(d.duration*e.rft*f.rate/60000,1) as oee from
(select a.machine_code,a.machine_id,c.days,c.shift,round(nvl(c.duration,0),1) as duration from EAP_CODE_CONTRAST a,mes030m b,vw_eap_cut_shift_run_time c where a.machine_id = b.machine_no and a.machine_id = c.machine_id) d
left join EAP_RFT_D e on d.machine_id = e.machine_id and e.shift_day =  d.days and e.shift =  d.shift
left join EAP_CAPACITY_RATE_D f on d.machine_id = f.machine_id and f.shift = d.shift and f.shift_day =  d.days 
where d.shift like '%{0}%' and d.machine_id like '%{1}%' and d.days like '%{2}%' order by d.days desc,d.machine_code desc,d.shift asc", vShift, vMachineID, vDate);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
    }
}

