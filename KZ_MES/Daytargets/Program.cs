﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayTargets
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
            //client.APIURL = "http://10.2.1.46:8090/api/CommonCall";
            client.UserToken = "01d45300-946e-4b34-b8cf-58635e0994f2";//
            client.Language = "en";
            Application.Run(new DayTargetsForm());
        }

        public static SJeMES_Framework.Class.ClientClass client;
    }
}
