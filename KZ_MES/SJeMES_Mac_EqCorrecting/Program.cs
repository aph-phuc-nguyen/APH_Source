﻿using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SJeMES_Mac_EqCorrecting
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //WebServiceUrl = @"http://192.168.0.122:8088/SJ-WebService.asmx";
            

                WebServiceUrl = @"http://localhost:8081/SJ-WebService.asmx";
                WebServiceUrl = @"http://192.168.0.200:8081/SJ-WebService.asmx";
                WebServiceUrl = @"http://192.168.1.125/SJ-WebService.asmx";


                string sqlconn = ConfigurationSettings.AppSettings["ConStr"].ToString();
                string[] t = new string[1];
                t[0] = ";";
                string[] a = sqlconn.Split(t, StringSplitOptions.RemoveEmptyEntries);
                WebServiceUrl = ConfigurationSettings.AppSettings["WebService"].ToString();

                //string sqlconn_ERP = ConfigurationSettings.AppSettings["ConStr_ERP"].ToString();
                //string[] t2 = new string[1];
                //t[0] = ";";
                //string[] a2 = sqlconn_ERP.Split(t, StringSplitOptions.RemoveEmptyEntries);

                Org = new GDSJ_Framework.Class.OrgClass();
                Org.Org = "gdsj";
                Org.OrgName = "易飞";
                //Org.DBServer = a[3].ToString().Substring(12);
                //Org.DBType = "SqlServer";
                //Org.DBName = a[5].ToString().Substring(16);
                //Org.DBUser = a[2].ToString().Substring(8);
                //Org.DBPassword = a[4].ToString().Substring(9);

                Org.DBServer = "";
                Org.DBType = "oracle";
                Org.DBName = "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.125)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME =SJWB )))";
                Org.DBUser = "SJWB";
                Org.DBPassword = "123";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmEqCorrecting(""));
            Environment.SetEnvironmentVariable("NLS_LANG", "SIMPLIFIEDCHINESE_CHINA.ZHS16GBK", EnvironmentVariableTarget.Process);
        }


        public static List<SJeMES_Framework.Web.JSONMenu> Menus;
        public static Dictionary<string, SJeMES_Framework.Web.JSONMenu> MenusInfo;
        public static string configstring;
        public static SJeMES_Framework.Class.ClientClass Client = new SJeMES_Framework.Class.ClientClass();
        public static MaterialSkinManager.Themes SkinThemes = MaterialSkinManager.Themes.LIGHT;

        public static string User = "ADMIN";
        public static string WebServiceUrl;
        public static GDSJ_Framework.Class.OrgClass Org;

        public static string UserName = "admin";
    }
}
