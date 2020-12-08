using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_EAP_MachineLink_BalanceKanBan
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            client = new SJeMES_Framework.Class.ClientClass();
            client.APIURL = "http://localhost:60626/api/CommonCall";
            client.UserToken = "1838a47f-fd4b-40b1-9eb8-6a3dbc5b90bd";//
            client.Language = "en";
            Application.Run(new F_EAP_Balance_Kanban());
        }

        public static SJeMES_Framework.Class.ClientClass client;
    }
}
