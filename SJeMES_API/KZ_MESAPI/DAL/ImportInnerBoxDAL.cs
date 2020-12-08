using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_MESAPI.DAL
{
    public class ImportInnerBoxDAL
    {
        /// <summary>
        /// 1:先检查内盒资料是否已经生效了
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="dt"></param>
        /// <param name="userCode"></param>
        /// <param name="companyCode"></param>
        public void UpLoad(DataBase DB, DataTable dt, string userCode, string companyCode, string dateFormat, string dateTimeFormat)
        {
            string sqlDept = "select STAFF_DEPARTMENT from HR001M where STAFF_NO='"+userCode+"'";
            string grt_dept = DB.GetString(sqlDept);
            for (int i=0;i<dt.Rows.Count;i++)
            {
                var o = dt.Rows[i];
                string item_type = dt.Rows[i][0].ToString();
                string sql = "select * from mes_label_type where item_type='"+item_type+"' and  status=7";
                System.Data.DataTable mesLableType = DB.GetDataTable(sql);
                if (mesLableType.Rows.Count <= 0)
                {
                    throw new Exception($"{item_type} not vali!");
                }
                string sql1 = "select * from mes_label_type_position where item_type='" + item_type + "'";
                System.Data.DataTable positions = DB.GetDataTable(sql1);
                string v_po = "";
                string v_oldpo = "";
                string v_art = "";
                string v_name = "";
                string v_size = "";
                string v_se_id = "";
                string v_label_id = "";
                string v_label_brand = "";//鞋型系列
                string v_label_features = "";//特征

                for (int j = 0; j < positions.Rows.Count; j++)
                {
                    var op = positions.Rows[j];
                    var begin = int.Parse(op["begin_position"].ToString());
                    var end = int.Parse(op["end_position"].ToString());
                    if (op["STR_TYPE"] != null && op["STR_TYPE"].Equals("A"))
                    {
                        v_po = o[1].ToString().Substring(begin, end - begin).Trim();
                    }
                    if (op["STR_TYPE"] != null && op["STR_TYPE"].Equals("B"))
                    {
                        v_art = o[1].ToString().Substring(begin, end - begin).Trim();
                    }
                    if (op["STR_TYPE"] != null && op["STR_TYPE"].Equals("C"))
                    {
                        v_name = o[1].ToString().Substring(begin, end - begin).Trim();
                    }
                    if (op["STR_TYPE"] != null && op["STR_TYPE"].Equals("D"))
                    {
                        v_size = o[1].ToString().Substring(begin, end - begin).Trim();
                        if (v_size != null && v_size.Contains("½K"))
                        {
                            v_size = v_size.Substring(0, v_size.Length - 2) + ".5K";
                        }
                        else if (v_size != null && v_size.Contains("½"))
                        {
                            v_size = v_size.Substring(0, v_size.Length - 1) + ".5";
                        }
                    }
                    if (op["STR_TYPE"] != null && op["STR_TYPE"].Equals("E"))
                    {
                        v_label_id = o[1].ToString().Substring(begin, end - begin).Trim();
                    }
                    if (op["STR_TYPE"] != null && op["STR_TYPE"].Equals("F"))
                    {
                        v_label_brand = o[1].ToString().Substring(begin, end - begin).Trim();
                    }
                    if (op["STR_TYPE"] != null && op["STR_TYPE"].Equals("G"))
                    {
                        v_label_features = o[1].ToString().Substring(begin, end - begin).Trim();
                    }

                }

                DataTable seOrdM =null;
                seOrdM = DB.GetDataTable(GetSql(v_po));
                if (seOrdM==null|| seOrdM.Rows.Count==0)
                {
                    v_oldpo = v_po;
                    v_po = "0" + v_oldpo;
                    seOrdM = DB.GetDataTable(GetSql(v_po));
                }
                if (seOrdM == null || seOrdM.Rows.Count == 0)
                {
                    v_po = "A" + v_oldpo;
                    seOrdM = DB.GetDataTable(GetSql(v_po));
                }
                if (seOrdM == null || seOrdM.Rows.Count == 0)
                {
                    v_po = "B" + v_oldpo;
                    seOrdM = DB.GetDataTable(GetSql(v_po));
                }
                if (seOrdM == null || seOrdM.Rows.Count == 0)
                {
                    v_po = "C" + v_oldpo;
                    seOrdM = DB.GetDataTable(GetSql(v_po));
                }
                if (seOrdM == null || seOrdM.Rows.Count == 0)
                {
                    v_po = "J" + v_oldpo;
                    seOrdM = DB.GetDataTable(GetSql(v_po));
                }
                if (seOrdM.Rows.Count>0)
                {
                    v_se_id = seOrdM.Rows[0]["SE_ID"].ToString();
                }
                else
                {
                    throw new Exception($"po{ v_po } unvali");
                }

                string sql2= "select * from mes_label_m where label_id='"+v_label_id+"'";
                DataTable mesLabelM = DB.GetDataTable(sql2);
                if (mesLabelM.Rows.Count==0)
                {
                    DateTime thisTimeTemp = DateTime.Now;
                    string thisTime = thisTimeTemp.ToString(dateFormat + " HH:mm:ss");
                    string sql3 = "insert into mes_label_m(org_id,label_type,se_id,size_no,art_no," +
                        "art_name,label_id,po_no,label_brand,label_features," +
                        "label_specifications,label_qty,insert_date,last_date,barcode_type," +
                        "last_user,grt_user,grt_dept,status)" +
                        "values('"+companyCode+"','A','"+v_se_id+"','"+v_size+"','"+v_art+"'," +
                        "'" +v_name+"','"+v_label_id+"','"+v_po+"','"+v_label_brand+"','"+v_label_features+"'," +
                        "'"+item_type+"',1,to_date('"+ thisTime+ "','"+dateTimeFormat+"'),to_date('" + thisTime+ "', '"+ dateTimeFormat + "'),'1'," +
                        "'"+userCode+"','"+userCode+"','"+grt_dept+"','7')";
                    DB.ExecuteNonQueryOffline(sql3);
                }

            }
        }


        private string GetSql(string param)
        {
            string sql = @"select * from mv_se_ord_m where mer_po='" + param + "'";
            return sql;
        }


        public DataTable Query(DataBase DB, string date, string userCode, string companyCode, string dateFormat, string dateTimeFormat)
        {

            string sql = "";
            if (!(string.IsNullOrEmpty(date)&&string.IsNullOrWhiteSpace(date)))
            {
                sql="select * from VW_MES_LABEL_M where work_day=to_date('" + date + "','"+dateFormat+"')";
            }
            else
            {
                sql = "select * from VW_MES_LABEL_M2";
            }
            return DB.GetDataTable(sql);
        }
    }
}
