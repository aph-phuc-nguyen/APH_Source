﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_JMS_QrCode_Print
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
            client.UserToken = "710daf3b-5afe-49c8-9400-c15a4f22f1c3";//
            client.Language = "cn";//
            Application.Run(new F_JMS_QrCode_Print());
        }
        public static SJeMES_Framework.Class.ClientClass client;
    }
}