using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MesCloudSync
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
            //client.APIURL = "http://localhost:60626/api/CommonCall";
            client.APIURL = "http://10.2.1.46:8090/api/CommonCall";
            //client.APIURL = "http://10.2.1.50:80/api/CommonCall";
            client.UserToken = "84eab445-0ac3-4338-b68a-1fcb1020fb59";
            client.Language = "cn";
            Application.Run(new MesCloudSyncForm());
        }
        public static SJeMES_Framework.Class.ClientClass client;

    }
}
