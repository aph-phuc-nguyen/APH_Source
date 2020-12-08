using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace  DayActualManually
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
            // client.APIURL = "http://10.2.1.46:8090/api/CommonCall";
            //  client.UserToken = "4b90dc28-0ae8-4147-a832-84335379c2f8";//  
            client.UserToken = "0c70195b-61a4-4dba-8244-aae4821bc037";
            client.Language = "hk";
            Application.Run(new  DayActualManuallyForm());
        }

        public static SJeMES_Framework.Class.ClientClass client;
    }
}
