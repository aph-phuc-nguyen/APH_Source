using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCS_BonusCalculationSystem
{
    static class Program
    {
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
            client.UserToken = "c04e29fd-63ca-41a5-ace9-7b3242b14318";
            client.Language = "en";
            Application.Run(new F_BCS_BonusCalculation_Main());
        }

        public static SJeMES_Framework.Class.ClientClass client;
    }
}
