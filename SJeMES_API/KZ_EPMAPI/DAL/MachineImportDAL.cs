using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_EPMAPI.DAL
{
    public class MachineImportDAL
    {
        public DataTable GetMachineNO(DataBase DB, string companyCode)
        {
            string sql = "select MACHINE_NO from MES030M";
            System.Data.DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        /// <summary>
        /// -------该功能主要执行验证上传的资料-------
        /// 1:验证设备编号唯一
        /// </summary>
        public DataTable ValiMachineUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string companyCode)
        {
            DataTable tab = new DataTable();
            tab.Columns.Add("POSITION");
            tab.Columns.Add("ROW_INDEX");
            tab.Columns.Add("MACHINE_NO");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string MACHINE_NO = dt.Rows[i][0].ToString().Trim();
                //string sql1 = @"select MACHINE_NO from MES030M where MACHINE_NO = '" + MACHINE_NO + "'";
                //System.Data.DataTable hasMachine = DB.GetDataTable(sql1);

                string sql1 = string.Format(@"select MACHINE_NO from MES030M where MACHINE_NO = '{0}'", MACHINE_NO);
                System.Data.DataTable hasMachine = DB.GetDataTable(sql1);

                if (hasMachine.Rows.Count > 0)
                {
                    //errorCount++;
                    DataRow dr = tab.NewRow();
                    dr[0] = "err-MachineNO";
                    dr[1] = (i + 1).ToString();
                    dr[2] = MACHINE_NO;
                    tab.Rows.Add(dr);
                    continue;
                }
            }
            return tab;
        }

        /// <summary>
        /// -------该功能主要执行验证上传的资料-------
        /// 1:验证保养编号唯一
        /// </summary>
        public DataTable ValiMaintainUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string companyCode)
        {
            DataTable tab = new DataTable();
            tab.Columns.Add("POSITION");
            tab.Columns.Add("ROW_INDEX");
            tab.Columns.Add("Item_ID");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ITEM_ID = dt.Rows[i][0].ToString().Trim();
                //string sql1 = @"select ITEM_ID from MAC001M where ITEM_ID = '" + ITEM_ID + "'";
                //System.Data.DataTable hasMachine = DB.GetDataTable(sql1);
                string sql1 = string.Format(@"select ITEM_ID from MAC001M where ITEM_ID = '{0}'", ITEM_ID);
                System.Data.DataTable hasMaintain = DB.GetDataTable(sql1);
                if (hasMaintain.Rows.Count > 0)
                {
                    //errorCount++;
                    DataRow dr = tab.NewRow();
                    dr[0] = "err-MachineNO";
                    dr[1] = (i + 1).ToString();
                    dr[2] = ITEM_ID;
                    tab.Rows.Add(dr);
                    continue;
                }
            }
            return tab;
        }

        /// <summary>
        /// -------该功能主要执行验证上传的资料-------
        /// 1:验证校正编号唯一
        /// </summary>
        public DataTable ValiCorrectionUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string companyCode)
        {
            DataTable tab = new DataTable();
            tab.Columns.Add("POSITION");
            tab.Columns.Add("ROW_INDEX");
            tab.Columns.Add("Item_ID");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ITEM_ID = dt.Rows[i][0].ToString().Trim();
                //string sql1 = @"select ITEM_ID from MAC002M where ITEM_ID = '" + ITEM_ID + "'";
                //System.Data.DataTable hasMachine = DB.GetDataTable(sql1);
                string sql1 = string.Format(@"select ITEM_ID from MAC002M where ITEM_ID = '{0}'", ITEM_ID);
                System.Data.DataTable hasCorrection = DB.GetDataTable(sql1);
                if (hasCorrection.Rows.Count > 0)
                {
                    //errorCount++;
                    DataRow dr = tab.NewRow();
                    dr[0] = "err-MachineNO";
                    dr[1] = (i + 1).ToString();
                    dr[2] = ITEM_ID;
                    tab.Rows.Add(dr);
                    continue;
                }
            }
            return tab;
        }
        /// <summary>
        /// -------该功能主要保存上传的资料-------
        /// 1:保存导入的设备基础资料信息
        /// </summary>
        public void MachineUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string companyCode)
        {
            string sqlDept = string.Format("select STAFF_DEPARTMENT from HR001M where STAFF_NO = '{0}'", userId);
            string grt_dept = DB.GetString(sqlDept);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string MACHINE_NO = dt.Rows[i][0].ToString();                                           //设备编号
                string MACHINE_NAME = dt.Rows[i][1].ToString();                                         //设备名称
                string Description = dt.Rows[i][2].ToString();                                          //备注
                string Type = dt.Rows[i][3].ToString();                                                 //设备类型
                string BRAND = dt.Rows[i][4].ToString();                                                //设备厂商
                string UDF04 = dt.Rows[i][5].ToString();                                                //固定资产编号
                DateTime UDF05 = DateTime.Parse(dt.Rows[i][6].ToString());                              //进厂日期
                decimal PRICE = dt.Rows[i][7] == null ? 0 : Decimal.Parse(dt.Rows[i][7].ToString());    //价格
                DateTime DATE_BUY = DateTime.Parse(dt.Rows[i][8].ToString());                           //购买日期
                string UDF01 = dt.Rows[i][9].ToString();                                                //部门编号
                string ADDRESS = dt.Rows[i][10].ToString();                                             //存放位置
                string UDF02 = dt.Rows[i][11].ToString();                                               //类别

                string sql1 = string.Format(@"select MACHINE_NO from MES030M where MACHINE_NO='{0}'", MACHINE_NO);
                System.Data.DataTable hasMachine = DB.GetDataTable(sql1);
                if (hasMachine.Rows.Count == 0)
                {                                        
                    /**
                     * 插入导入的数据
                     * insert into MES_DEPT_MANPOWER(D_DEPT,BEGIN_DAY,JOCKEY_QTY,PLURIPOTENT_WORKER,OMNIPOTENT_WORKER,UDF01) VALUES('','','','','','')
                     */
                     string insert = string.Format(@"insert into MES030M(MACHINE_NO, MACHINE_NAME, Description, Type, BRAND, UDF04, UDF05, PRICE, DATE_BUY, UDF01, ADDRESS, UDF02)
                                       VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                       MACHINE_NO, MACHINE_NAME, Description, Type, BRAND, UDF04, UDF05.ToString(), PRICE.ToString(), DATE_BUY.ToString(), UDF01, ADDRESS, UDF02);
                     DB.ExecuteNonQueryOffline(insert);
                }
            }
        }
        
        /// <summary>
        /// -------该功能主要保存上传的资料-------
        /// 1:保存导入的设备保养项目资料
        /// </summary>
        public void MaintainUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string companyCode)
        {
            string sqlDept = string.Format("select STAFF_DEPARTMENT from HR001M where STAFF_NO = '{0}'", userId);
            string grt_dept = DB.GetString(sqlDept);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Item_ID = dt.Rows[i][0].ToString();              //保养项目编号
                string Item_NAME = dt.Rows[i][1].ToString();            //保养项目名称
                string Item_DESC = dt.Rows[i][2].ToString();             //保养方法

                string sql1 = string.Format(@"select ITEM_ID from MAC001M where ITEM_ID = '{0}'", Item_ID);
                System.Data.DataTable hasMaintain = DB.GetDataTable(sql1);
                if (hasMaintain.Rows.Count == 0)
                {
                    /**
                     * 插入导入的数据
                     * insert into MES_DEPT_MANPOWER(D_DEPT,BEGIN_DAY,JOCKEY_QTY,PLURIPOTENT_WORKER,OMNIPOTENT_WORKER,UDF01) VALUES('','','','','','')
                     */
                    string insert = string.Format(@"insert into MAC001M(ITEM_ID, ITEM_NAME, ITEM_DESC)
                                       VALUES('{0}','{1}','{2}')",
                                       Item_ID, Item_NAME, Item_DESC);
                    DB.ExecuteNonQueryOffline(insert);
                }
            }
        }

        /// <summary>
        /// -------该功能主要保存上传的资料-------
        /// 1:保存导入的设备保养项目资料
        /// </summary>
        public void CorrectionUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string companyCode)
        {
            string sqlDept = string.Format("select STAFF_DEPARTMENT from HR001M where STAFF_NO = '{0}'", userId);
            string grt_dept = DB.GetString(sqlDept);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Item_ID = dt.Rows[i][0].ToString();              //校正项目编号
                string Item_NAME = dt.Rows[i][1].ToString();            //校正项目名称
                string Item_DESC = dt.Rows[i][2].ToString();            //校正标准
                string Tool = dt.Rows[i][2].ToString();                 //校正工具
                string InnerOrOut = dt.Rows[i][2].ToString();           //内/外校正

                string sql1 = string.Format(@"select ITEM_ID from MAC002M where ITEM_ID = '{0}'", Item_ID);
                System.Data.DataTable hasCorrection = DB.GetDataTable(sql1);
                if (hasCorrection.Rows.Count == 0)
                {
                    /**
                     * 插入导入的数据
                     * insert into MES_DEPT_MANPOWER(D_DEPT,BEGIN_DAY,JOCKEY_QTY,PLURIPOTENT_WORKER,OMNIPOTENT_WORKER,UDF01) VALUES('','','','','','')
                     */
                    string insert = string.Format(@"insert into MAC002M(ITEM_ID, ITEM_NAME, ITEM_DESC, UDF01, UDF02)
                                       VALUES('{0}','{1}','{2}','{3}','{4}')",
                                       Item_ID, Item_NAME, Item_DESC, Tool, InnerOrOut);
                    DB.ExecuteNonQueryOffline(insert);
                }
            }
        }
    }
}
