﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_TMS_TierMeeting3_Main
{
    static class Program
    {
        public static SJeMES_Framework.Class.ClientClass client;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            client = new SJeMES_Framework.Class.ClientClass();
            client.APIURL = "http://localhost:60626/api/CommonCall";
            client.UserToken = "01d45300-946e-4b34-b8cf-58635e0994f2";


            client.Language = "en";
            Application.Run(new IntroForm());
        }
    }
}
