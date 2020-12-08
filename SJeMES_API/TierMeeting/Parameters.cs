namespace TierMeeting
{
    class Parameters
    {
        public static string dateFormat = "yyyy/MM/dd";
        public static string dateTimeFormat = "yyyy/MM/dd hh24:mi:ss";
        public enum QueryDeptType : int
        {
            All = 0,
            Plant = 1,
            Section = 2,
            Line = 3
        }
    }
}
