using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionDashBoard
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
            Client.UserToken = "4565695d-6695-4a69-a8ca-c03ae77c0697";//
            DashBoardForm frm = new DashBoardForm();
            if (Screen.AllScreens.Count() != 1)
            {
                for (int i=0;i< Screen.AllScreens.Count();i++)
                {
                    Screen s = Screen.AllScreens[i];
                    if (!s.Primary)
                    {
                        frm.Location = new System.Drawing.Point(s.Bounds.X, s.Bounds.Y);
                        frm.Size = new System.Drawing.Size(Screen.AllScreens[i].Bounds.Width, Screen.AllScreens[i].Bounds.Height);
                    }      
                }
            }
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
            Application.Run(frm);
        }

        public static SJeMES_Framework.Class.ClientClass Client;
    }
}
