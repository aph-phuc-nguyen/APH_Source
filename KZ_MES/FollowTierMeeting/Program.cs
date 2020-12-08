using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace FollowTierMeeting
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
            client.UserToken = "8cc11c31-dd6a-442b-809c-e09a6cd4baaa";
            client.Language = "hk";

            Application.Run(new FollowTierMeetingForm());
        }
        public static SJeMES_Framework.Class.ClientClass client;
    }
}
