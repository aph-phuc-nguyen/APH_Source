using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StitchingInput
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
            client.APIURL = "http://10.2.1.46:8090/api/CommonCall";
            //client.APIURL = "http://localhost:60626/api/CommonCall";
            client.UserToken = "c5c60f22-bf86-4ef8-a392-dff7a5df6618";//
            client.Language = "cn";
            Application.Run(new StitchingInputForm());
        }

        public static SJeMES_Framework.Class.ClientClass client;
    }
}
