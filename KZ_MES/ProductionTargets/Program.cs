using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionTargets
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
            //client.APIURL = "http://10.2.1.50:80/api/CommonCall";
            client.UserToken = "d4fcdd25-f699-4c42-89ae-e83fd6cb6fb6";//
            client.Language = "cn";
            Application.Run(new ProductionTargetsForm());
        }

        public static SJeMES_Framework.Class.ClientClass client;


    }
}
