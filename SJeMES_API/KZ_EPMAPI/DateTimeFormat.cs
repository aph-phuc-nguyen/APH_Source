using System;
using System.Collections.Generic;
using System.Text;

namespace KZ_EPMAPI
{
    class DateTimeFormat
    {
        public static string getDateFormatValue(string mesdb)
        {
            string dateFormat;
            switch (mesdb)
            {
                case "apemes":
                    dateFormat = "yyyy/MM/dd";
                    break;
                case "aphmes":
                    //
                    dateFormat = "yyyy/MM/dd";
                    break;
                case "apcmes":
                    dateFormat = "dd/MM/yyyy";
                    break;
                default:
                    dateFormat = "yyyy/MM/dd";
                    break;
            }
            return dateFormat;
        }


        public static string getDateTimeFormatValue(string mesdb)
        {
            string dateFormat;
            switch (mesdb)
            {
                case "apemes":
                    dateFormat = "yyyy/MM/dd HH24:mi:ss";
                    break;
                ///format???
                case "aphmes":
                    dateFormat = "yyyy/MM/dd HH24:mi:ss";
                    break;
                case "apcmes":
                    dateFormat = "dd/MM/yyyy";
                    break;
                default:
                    dateFormat = "yyyy/MM/dd";
                    break;
            }
            return dateFormat;
        }

        public static string getDateTimeNowFormatValue(string mesdb)
        {
            string dateFormat;
            switch (mesdb)
            {
                case "apemes":
                    dateFormat = "yyyy/MM/dd HH:mm:ss";
                    break;
                ///format???
                case "aphmes":
                    dateFormat = "yyyy/MM/dd HH:mi:ss";
                    break;
                case "apcmes":
                    dateFormat = "dd/MM/yyyy";
                    break;
                default:
                    dateFormat = "yyyy/MM/dd";
                    break;
            }
            return dateFormat;

        }
    }
}
