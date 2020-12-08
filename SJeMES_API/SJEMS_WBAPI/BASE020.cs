using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class BASE020
    {
        /// <summary>
        /// Init(初始化包装方式)
        /// </summary>
        /// <param name="OBJ">XML数据</param>
        /// <returns></returns>
        public static string Init(object OBJ)
        {
            string XML = (string)OBJ;
            string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            bool IsSuccess = false;
            string RetData = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();

            try
            {

                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");

                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

       

                DB = new GDSJ_Framework.DBHelper.DataBase(XML);

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);

                string sql = @"
DELETE FROM BASE020M
DELETE FROM BASE020A1
";
                DB.ExecuteNonQueryOffline(sql);

                sql = @"
SELECT
material_no,
material_name,
material_specifications
FROM BASE007M 
--WHERE material_type ='301'
ORDER BY material_no
";
                System.Data.IDataReader idr = DB.GetDataTableReader(sql);
                while(idr.Read())
                {
                    string material_no = idr["material_no"].ToString();
                    string material_name = idr["material_name"].ToString();
                    string material_specifications = idr["material_specifications"].ToString();
                    sql = @"
INSERT INTO BASE020M
(pack_no,pack_name,pack_description,material_no,material_name,material_specifications)
VALUES
('" + material_no+@"','"+material_name+@"','"+material_specifications+@"','"+ material_no + @"','" + material_name + @"','" + material_specifications + @"')
";
                    DB.ExecuteNonQueryOffline(sql);


                    sql = @"
INSERT INTO BASE020A1
(sorting,pack_no,material_no,material_name,material_specifications,sum,iscontrol)
VALUES
('001','" + material_no+@"','"+ material_no+@"-5A1','铝材包装1','"+material_no+ @"',1,'TRUE')

INSERT INTO BASE020A1
(sorting,pack_no,material_no,material_name,material_specifications,sum,iscontrol)
VALUES
('001','" + material_no + @"','" + material_no + @"-5B1','玻璃包装1','" + material_no + @"',1,'TRUE')

INSERT INTO BASE020A1
(sorting,pack_no,material_no,material_name,material_specifications,sum,iscontrol)
VALUES
('001','" + material_no + @"','" + material_no + @"-5C1','胶条包装1','" + material_no + @"',1,'TRUE')

INSERT INTO BASE020A1
(sorting,pack_no,material_no,material_name,material_specifications,sum,iscontrol)
VALUES
('001','" + material_no + @"','" + material_no + @"-5D1','拉杆包装1','" + material_no + @"',1,'TRUE')

INSERT INTO BASE020A1
(sorting,pack_no,material_no,material_name,material_specifications,sum,iscontrol)
VALUES
('001','" + material_no + @"','" + material_no + @"-5E1','拉手包装1','" + material_no + @"',1,'TRUE')

INSERT INTO BASE020A1
(sorting,pack_no,material_no,material_name,material_specifications,sum,iscontrol)
VALUES
('001','" + material_no + @"','" + material_no + @"-5F1','配件包装1','" + material_no + @"',1,'TRUE')
";
                    DB.ExecuteNonQueryOffline(sql);
                }

                IsSuccess = true;
                
            }
            catch (Exception ex)
            {
                RetData = "00000:" + ex.Message;
            }

            GDSJ_Framework.Common.WebServiceHelper.SaveRetLog(DB, guid, IsSuccess.ToString(), RetData);

            ret = @"
            <WebService>
                <DllName>" + DllName + @"</DllName>
                <ClassName>" + ClassName + @"</ClassName>
                <Method>" + Method + @"</Method>
                <Data>" + Data + @"</Data>
                <Return>
                    <IsSuccess>" + IsSuccess + @"</IsSuccess>
                    <RetData>" + RetData + @"</RetData>
                </Return>
            </WebService>
            ";

            return ret;
        }
    }
}
