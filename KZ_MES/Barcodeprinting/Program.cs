using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barcodeprinting
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
           // client.APIURL = "http://10.2.1.50:80/api/CommonCall";
            client.UserToken = "11cc1fca-0441-4be2-bca0-c272c3e91982";//
            client.Language = "cn";//
            Application.Run(new BarcodeprintingForm());
        }
        public static SJeMES_Framework.Class.ClientClass client;
    }
}
