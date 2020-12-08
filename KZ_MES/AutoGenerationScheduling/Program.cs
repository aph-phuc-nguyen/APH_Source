﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoGenerationScheduling
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
            client.UserToken = "0a2d262f-b886-4d22-ad54-efaa5b120858";//
            client.Language = "cn";//
            Application.Run(new AutoGenerationSchedulingForm());
        }

        public static SJeMES_Framework.Class.ClientClass client;
    }
}
