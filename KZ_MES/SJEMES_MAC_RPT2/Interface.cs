using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJEMES_MAC_RPT2
{
   public class Interface
    {
        public static void RunApp(Object obj)
        {
            try
            {
                Program.Client = obj as SJeMES_Framework.Class.ClientClass;
                Program.Org = new GDSJ_Framework.Class.OrgClass();
                Program.Org.Org = "gdsj";
                Program.Org.OrgName = "易飞";
                //Org.DBServer = a[3].ToString().Substring(12);
                //Org.DBType = "SqlServer";
                //Org.DBName = a[5].ToString().Substring(16);
                //Org.DBUser = a[2].ToString().Substring(8);
                //Org.DBPassword = a[4].ToString().Substring(9);

                Program.Org.DBServer = "";
                Program.Org.DBType = "oracle";
                Program.Org.DBName = "";
                Program.Org.DBUser = "";
                Program.Org.DBPassword = "";
                //string machine_no = (obj as Dictionary<string, object>).ContainsKey("Machine_no") ? (obj as Dictionary<string, object>)["Machine_no"] as string : "";
                string machine_no = "";
                //bool IsMaxWindow = Convert.ToBoolean((obj as Dictionary<string, object>)["IsMaxWindow"]);
                frm设备采集报表 frm = new frm设备采集报表((obj as Dictionary<string, object>));
                //if (IsMaxWindow)
                //{
                //    frm.WindowState = FormWindowState.Maximized;
                //}
                frm.ShowDialog();
                //frm.TopMost = true;
                //frm.Show();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
