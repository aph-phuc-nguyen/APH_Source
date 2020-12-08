using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionWorkingHours
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
            Client.UserToken = "01d45300-946e-4b34-b8cf-58635e0994f2";//
            WorkingHoursForm frm = new WorkingHoursForm();
            if (Screen.AllScreens.Count() != 1)
            {
                for (int i = 0; i < Screen.AllScreens.Count(); i++)
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
