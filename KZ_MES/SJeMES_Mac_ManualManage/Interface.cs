using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJeMES_Mac_ManualManage
{
   public class Interface
    {
        public static void RunApp(Object obj)
        {
            try
            {
                Program.Client = obj as SJeMES_Framework.Class.ClientClass;
                Program.Org = new GDSJ_Framework.Class.OrgClass();
                Program.Org.OrgName = Program.Client.Org.OrgName;
                Program.Org.DBServer = Program.Client.Org.DBServer;
                Program.Org.DBType = Program.Client.Org.DBType;
                Program.Org.DBName = Program.Client.Org.DBName;
                Program.Org.DBUser = Program.Client.Org.DBUser;
                Program.Org.DBPassword = Program.Client.Org.DBPassword;
                //string machine_no = (obj as Dictionary<string, object>).ContainsKey("Machine_no") ? (obj as Dictionary<string, object>)["Machine_no"] as string : "";
                string machine_no = "";
                //bool IsMaxWindow = Convert.ToBoolean((obj as Dictionary<string, object>)["IsMaxWindow"]);
                FrmManualManage frm = new FrmManualManage((obj as Dictionary<string, object>));
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
