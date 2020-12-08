using System;
using System.Collections.Generic;
using System.Text;

namespace KZ_MESAPI
{
    public  static class Organization
    {
        public static string getValue(string mesdb)
        {
            string org_id;
            switch (mesdb)
            {
                case "apemes":
                    org_id = "200";
                    break;
                    //APH
                case "aphmes":
                    org_id = "700";
                    break;
                case "aphmestest":
                    org_id = "700";
                    break;
                    //
                case "apcmes":
                    org_id = "100";
                    break;
                case "mestest":
                    org_id = "200";
                    break;
                default:
                    org_id = "0";
                    break;
            }
            return org_id;
        }
    }
}
