using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_EAPAPI.DAL
{
    public class MachineLinkBasicDAL
    {
        public DataTable QueryMachineList(DataBase DB, string vMachineID, string vFactory, string vDept, string vType, string vWhetherLink)
        {
            string sql = @"select a.machine_no, a.machine_name, a.udf02 as machine_type, a.udf01 as dept, b.department_name, b.udf05 as org_code, c.org,a.udf03 from mes030m a, base005m b, sjqdms_orginfo c
where a.udf01 = b.department_code and b.udf05 = c.code";
            sql += " and a.machine_no like '%" + vMachineID + "%'";
            sql += " and a.udf01 like '%" + vDept + "%'";

            if (!"ALL".Equals(vFactory))
            {
                sql += " and b.udf05 = '" + vFactory + "'";
            }
            if (!"ALL".Equals(vType))
            {
                sql += " and a.UDF02 ='" + vType + "'";
            }
            if (!"ALL".Equals(vWhetherLink))
            {
                sql += " and a.UDF03 ='" + vWhetherLink + "'";
            }
            sql += " order by a.udf01 asc";

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryDeptList(DataBase DB, string vFactory, string vDept)
        {
            string sql = @"select distinct a.udf01 as dept, b.department_name, b.udf05 as org_code, c.org from mes030m a, base005m b, sjqdms_orginfo c
where a.udf03 = 'Y' and a.udf01 = b.department_code and b.udf05 = c.code";
            sql += " and a.udf01 like '%" + vDept + "%'";

            if (!"ALL".Equals(vFactory))
            {
                sql += " and b.udf05 = '" + vFactory + "'";
            }
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable QueryMachineIdByDept(DataBase DB, string vDept)
        {
            string sql = @"select a.machine_no, a.machine_name, a.udf02 as machine_type, a.udf01 as dept from mes030m a, base005m b
where a.udf03 = 'Y' and a.udf01 = b.department_code";
            sql += " and a.udf01 = '" + vDept + "'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetFactory(DataBase DB)
        {
            string sql = @"select distinct b.udf05 as code,c.org as name from mes030m a,base005m b,sjqdms_orginfo c 
where a.udf01 = b.department_code and  b.udf05 = c.code and a.udf03 = 'Y'";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetAllMachineType(DataBase DB)
        {
            string sql = @"select TYPE AS CODE,CN AS NAME from EAP_TYPE_INFO";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public string AddMachineContrast(DataBase DB, string vMachineID, string vMachineNO, string userId)
        {
            string type = "";

            string sql_detail = string.Format(@"select udf02 from mes030m where machine_no = '{0}'",vMachineID);
            string typeCode = DB.GetString(sql_detail);
            if (string.IsNullOrEmpty(typeCode))
            {
                return "B";
            }
            else
            {
                type = typeCode;
            }

            string sql = string.Format(@"select count(*) from EAP_CODE_CONTRAST where machine_id = '{0}'", vMachineID);
            int count = DB.GetInt16(sql);
            if (count > 0)
            {
                return "C";
            }

            string sql_con = string.Format(@"select count(*) from EAP_CODE_CONTRAST where type = '{0}' and machine_code = '{1}'", type, vMachineNO);
            int num = DB.GetInt16(sql_con);
            if (num > 0)
            {
                return "D";
            }

            string sql_insert = string.Format(@"insert into EAP_CODE_CONTRAST(MACHINE_CODE,TYPE,MACHINE_ID,GRT_USER,LAST_USER) VALUES('{0}','{1}','{2}','{3}','{4}')", vMachineNO,type,vMachineID,userId,userId);
            DB.ExecuteNonQueryOffline(sql_insert);
            return "A"; 
        }

        public void DelMachineContrastById(DataBase DB, string vMachineID)
        {
            string sql = string.Format(@"delete from EAP_CODE_CONTRAST where machine_id = '{0}'", vMachineID);
            DB.ExecuteNonQueryOffline(sql);
        }

        public DataTable GetMachineContrastList(DataBase DB, string vFactory, string vType, string vMachineID, string vMachineNO)
        {
            string sql = @"select a.machine_id,a.machine_code,b.machine_name,a.type,b.address,a.grt_user,a.createtime from eap_code_contrast a,mes030m b,base005m c where a.machine_id = b.machine_no and b.udf01 = c.department_code";
            sql += " and a.machine_id like '%" + vMachineID + "%'";
            sql += " and a.machine_code like '%" + vMachineNO + "%'";
            if (!"ALL".Equals(vFactory))
            {
                sql += " and c.udf05 = '" + vFactory + "'";
            }
            if (!"ALL".Equals(vType))
            {
                sql += " and a.type ='" + vType + "'";
            }

            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetEapContrastListByType(DataBase DB, string vType)
        {
            string sql = string.Format(@"select machine_code,machine_id from eap_code_contrast where type = '{0}'", vType);
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public void UpdateLinkedStatusById(DataBase DB, string vMachineID, string vWhetherLink)
        {
            string sql = string.Format(@"update mes030m set udf03 = '{0}' where machine_no = '{1}'", vWhetherLink, vMachineID);
            DB.ExecuteNonQueryOffline(sql);
        }

        public void updateEapRftAndRate(DataBase DB, DataTable dt, string userId)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string machineId = dt.Rows[i][0].ToString();
                string shiftDay = dt.Rows[i][2].ToString();
                if (!"".Equals(dt.Rows[i][3].ToString()))
                {
                    decimal rftA = decimal.Parse(dt.Rows[i][3].ToString());
                    System.Data.DataTable rft_Dt1 = DB.GetDataTable(string.Format(@"select machine_id,shift_day,shift from EAP_RFT_D where machine_id='{0}' and shift_day = '{1}' and shift = 'A'", machineId, shiftDay));
                    if (rft_Dt1.Rows.Count > 0)
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"update EAP_RFT_D set rft = {0},last_user = '{1}',last_date = (select sysdate from dual) where  machine_id='{2}' and shift_day = '{3}' and shift = 'A'", rftA, userId, machineId, shiftDay));
                    }
                    else
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"insert into EAP_RFT_D(MACHINE_ID,SHIFT_DAY,GRT_USER,LAST_USER,RFT,SHIFT) VALUES('{0}','{1}','{2}','{2}',{3},'A')", machineId, shiftDay, userId, rftA));
                    }
                }
                if (!"".Equals(dt.Rows[i][4].ToString()))
                {
                    decimal rftB = decimal.Parse(dt.Rows[i][4].ToString());
                    System.Data.DataTable rft_Dt2 = DB.GetDataTable(string.Format(@"select machine_id,shift_day,shift from EAP_RFT_D where machine_id='{0}' and shift_day = '{1}' and shift = 'B'", machineId, shiftDay));
                    if (rft_Dt2.Rows.Count > 0)
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"update EAP_RFT_D set rft = {0},last_user = '{1}',last_date = (select sysdate from dual) where  machine_id='{2}' and shift_day = '{3}' and shift = 'B'", rftB, userId, machineId, shiftDay));
                    }
                    else
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"insert into EAP_RFT_D(MACHINE_ID,SHIFT_DAY,GRT_USER,LAST_USER,RFT,SHIFT) VALUES('{0}','{1}','{2}','{2}',{3},'B')", machineId, shiftDay, userId, rftB));
                    }
                }
                if (!"".Equals(dt.Rows[i][5].ToString()))
                {
                    decimal rateA = decimal.Parse(dt.Rows[i][5].ToString());
                    System.Data.DataTable rft_Dt3 = DB.GetDataTable(string.Format(@"select machine_id,shift_day,shift,rate from EAP_CAPACITY_RATE_D where machine_id='{0}' and shift_day = '{1}' and shift = 'A'", machineId, shiftDay));
                    if (rft_Dt3.Rows.Count > 0)
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"update EAP_CAPACITY_RATE_D set rate = {0},last_user = '{1}',last_date = (select sysdate from dual) where  machine_id='{2}' and shift_day = '{3}' and shift = 'A'", rateA, userId, machineId, shiftDay));
                    }
                    else
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"insert into EAP_CAPACITY_RATE_D(MACHINE_ID,SHIFT_DAY,GRT_USER,LAST_USER,RATE,SHIFT) VALUES('{0}','{1}','{2}','{2}',{3},'A')", machineId, shiftDay, userId, rateA));
                    }
                }
                if (!"".Equals(dt.Rows[i][6].ToString()))
                {
                    decimal rateB = decimal.Parse(dt.Rows[i][6].ToString());
                    System.Data.DataTable rft_Dt4 = DB.GetDataTable(string.Format(@"select machine_id,shift_day,shift,rate from EAP_CAPACITY_RATE_D where machine_id='{0}' and shift_day = '{1}' and shift = 'B'", machineId, shiftDay));
                    if (rft_Dt4.Rows.Count > 0)
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"update EAP_CAPACITY_RATE_D set rate = {0},last_user = '{1}',last_date = (select sysdate from dual) where  machine_id='{2}' and shift_day = '{3}' and shift = 'B'", rateB, userId, machineId, shiftDay));
                    }
                    else
                    {
                        DB.ExecuteNonQueryOffline(string.Format(@"insert into EAP_CAPACITY_RATE_D(MACHINE_ID,SHIFT_DAY,GRT_USER,LAST_USER,RATE,SHIFT) VALUES('{0}','{1}','{2}','{2}',{3},'B')", machineId, shiftDay, userId, rateB));
                    }
                }
            }
        }
    }
}



