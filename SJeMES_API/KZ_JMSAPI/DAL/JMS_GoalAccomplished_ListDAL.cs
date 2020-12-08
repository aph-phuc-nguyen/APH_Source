using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_JMSAPI.DAL
{
    class JMS_GoalAccomplished_ListDAL
    {

        public void insert(DataBase DB, DataTable dt, string userId, string dateFormat)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string now = DateTime.Now.ToString("yyyyMMddhhmmss");
                string SID = now.ToString()+i;//当前时间  时分秒+i = SID
                string plant = dt.Rows[i][1].ToString();
                string department_code = dt.Rows[i][2].ToString();
                string shoe_name = dt.Rows[i][3].ToString();
                string model_number = dt.Rows[i][4].ToString();
                string wk_id = dt.Rows[i][5].ToString();
                string art_no = dt.Rows[i][6].ToString();
                decimal qty = Decimal.Parse(dt.Rows[i][7].ToString());
                string plan_finish_date = dt.Rows[i][8].ToString();
                string finish_situation = dt.Rows[i][9] == null ? "" : dt.Rows[i][9].ToString();
                string packing_owe = dt.Rows[i][10] == null ? "" : dt.Rows[i][10].ToString();
                string actual_date = dt.Rows[i][11] == null ? null : dt.Rows[i][11].ToString();
                string under_reason = dt.Rows[i][12] == null ? "" : dt.Rows[i][12].ToString();
                string createby = userId;
                string createdate = DateTime.Now.ToString(dateFormat);            
                string sql = string.Format(@"select * from JMS_Reach_list where plant= '{0}' and department_code = '{1}'" +
                "and wk_id ='{2}' and plan_finish_date = to_date('{3}','{4}')", plant, department_code, wk_id, plan_finish_date, dateFormat);
                DataTable dt1 = DB.GetDataTable(sql);                
                if (dt1.Rows.Count <= 0)
                {
                    insertPOdata(DB, SID, plant, department_code, shoe_name, model_number, wk_id, art_no, qty, plan_finish_date, finish_situation, packing_owe, actual_date,
                    under_reason, dateFormat, createby, createdate);
                }
                else
                {
                    update(DB, SID, plant, department_code, shoe_name, model_number, wk_id, art_no, qty, plan_finish_date, finish_situation, packing_owe, actual_date,
                    under_reason, dateFormat, createby, createdate);
                }
            }
        }

        public DataTable Tier4_DayQuery(DataBase DB, string date, string dateFormat)
        {
            string sql = string.Format(@"select  plant,department_code,shoe_name,model_number,wk_id,art_no,qty,to_char(plan_finish_date,'{1}') as plan_finish_date,finish_situation,packing_owe,actual_date,under_reason from jms_reach_list
                         where plan_finish_date = to_date('{0}', '{1}') order by plant,department_code,finish_situation", date,dateFormat);
            return DB.GetDataTable(sql);
        }

        public DataTable Tier4_PORateQuery(DataBase DB, string FirstDay, string SeventhDay,string dateFormat)
        {                      
            string sql = string.Format(@"select a.PLAN_FINISH_DATE,a.OK,b.intersection,c.total from 
(select COUNT(distinct WK_ID) as OK,PLAN_FINISH_DATE
            from JMS_Reach_list
           where  PLAN_FINISH_DATE between to_date('{0}', '{1}') and to_date('{2}', '{3}') and
              finish_situation = 'OK'
           group by PLAN_FINISH_DATE)a left join                                  
(SELECT COUNT(*) as intersection,PLAN_FINISH_DATE
    FROM (select distinct WK_ID,PLAN_FINISH_DATE
            from JMS_Reach_list 
           where  PLAN_FINISH_DATE between to_date('{4}', '{5}') and to_date('{6}', '{7}')
            and  finish_situation = 'OK'          
          intersect
          select distinct wk_id,PLAN_FINISH_DATE
            from JMS_Reach_list
           where  PLAN_FINISH_DATE between to_date('{8}', '{9}') and to_date('{10}', '{11}')
           and   finish_situation = 'NO'          
          ) 
         where PLAN_FINISH_DATE between to_date('{12}', '{13}') and to_date('{14}', '{15}')  
         group by PLAN_FINISH_DATE)b on a.PLAN_FINISH_DATE = b.PLAN_FINISH_DATE left join              
(SELECT COUNT(distinct WK_ID) as total,PLAN_FINISH_DATE
  FROM JMS_Reach_list
 WHERE PLAN_FINISH_DATE between to_date('{16}', '{17}') and
       to_date('{18}', '{19}')
       group by PLAN_FINISH_DATE)c
       on a.PLAN_FINISH_DATE = c.PLAN_FINISH_DATE", 
FirstDay, dateFormat, SeventhDay, dateFormat, FirstDay, dateFormat, SeventhDay, dateFormat, FirstDay, dateFormat, SeventhDay, dateFormat,
FirstDay, dateFormat, SeventhDay, dateFormat, FirstDay, dateFormat, SeventhDay, dateFormat);
            return DB.GetDataTable(sql);
        }


        public void insertPOdata(DataBase DB, string SID, string plant, string department_code,
            string shoe_name, string model_number, string wk_id, string art_no, decimal qty,
            string plan_finish_date, string finish_situation, string packing_owe,string actual_date,string under_reason,string dateFormat,string createby,string createdate)
        {
            string sql = string.Format(@"insert into JMS_Reach_list(SID,plant,department_code,shoe_name,model_number,wk_id,art_no,qty,
            plan_finish_date,finish_situation,packing_owe,actual_date,under_reason,createby,createdate
) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',to_date('{8}','{9}'),'{10}','{11}','{12}','{13}','{14}',to_date('{15}','{16}'))", SID, plant, department_code, shoe_name, model_number, wk_id, art_no, qty, plan_finish_date,dateFormat,
finish_situation, packing_owe, actual_date, under_reason, createby, createdate, dateFormat);
            DB.ExecuteNonQueryOffline(sql);
        }


        public void update(DataBase DB, string SID, string plant, string department_code,
            string shoe_name, string model_number, string wk_id, string art_no, decimal qty,
            string plan_finish_date, string finish_situation, string packing_owe, string actual_date, string under_reason, string dateFormat, string createby, string createdate)
        {
            string sql = string.Format(@"update JMS_Reach_list set SID = '{0}',plant = '{1}',department_code='{2}',shoe_name = '{3}',model_number = '{4}',
wk_id = '{5}',art_no = '{6}',qty = '{7}',plan_finish_date = to_date('{8}','{9}'),finish_situation = '{10}',packing_owe='{11}',actual_date='{12}',under_reason='{13}',modifyby = '{14}',
modifydate = to_date('{15}','{16}') where plant= '{17}' and department_code = '{18}'" + "and wk_id ='{19}' and plan_finish_date = to_date('{20}','{21}') ", SID,plant, department_code, shoe_name,
model_number,wk_id, art_no, qty, plan_finish_date, dateFormat, finish_situation, packing_owe, actual_date, under_reason, createby, createdate, dateFormat, plant, department_code,
wk_id, plan_finish_date, dateFormat);
            DB.ExecuteNonQueryOffline(sql);
        }
    }
}
