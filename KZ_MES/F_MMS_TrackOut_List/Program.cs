using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_MMS_TrackOut_List
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
            Client = new SJeMES_Framework.Class.ClientClass();
            Client.APIURL = "http://localhost:60626/api/CommonCall";
            //Client.APIURL = "http://10.2.1.46:8090/api/CommonCall";
            Client.UserToken = "bad1a7df-f8d9-4ba4-83a1-7a7697f4d248";//
            Client.Language = "cn";
            Application.Run(new F_MMS_TrackOut_ListForm());
        }
        public static SJeMES_Framework.Class.ClientClass Client;
    }
}
