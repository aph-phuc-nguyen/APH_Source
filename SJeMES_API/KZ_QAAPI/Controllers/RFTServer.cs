using System;
using System.Collections.Generic;

namespace KZ_QAAPI.Controllers
{
    public class RFTServer
    {
         public static SJeMES_Framework_NETCore.WebAPI.ResultObject Query(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vPlant = "";
                // string vPlant = jarr["vPlant"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                if (string.IsNullOrEmpty(vPlant)|| vPlant.Equals("ALL"))
                {
                    string RFT = DB.GetString(@"select NVL(ROUND((SUM(sjqdms_mp.mp010)-SUM(sjqdms_mp.mp011))/SUM(sjqdms_mp.mp010)*100,2),0) as rft
                    from sjqdms_mp where  mp007='TQC' and  to_char(insert_date,'yyyyy')>=to_char(sysdate,'yyyy') ");
                    ret.RetData = RFT;
                }
                else
                {
                     string RFT = DB.GetString(@"select NVL(ROUND((SUM(sjqdms_mp.mp010)-SUM(sjqdms_mp.mp011))/SUM(sjqdms_mp.mp010)*100,2),0) as rft
                     from sjqdms_mp where  mp007='TQC' and  to_char(insert_date,'yyyyy')>=to_char(sysdate,'yyyy') and  MP003 like '"+vPlant+@"%'");
                    ret.RetData = RFT;
                }
                ret.IsSuccess = true;
                
            }
            catch (Exception ex)
            {

                ret.IsSuccess = false;
                ret.ErrMsg = "00000:" + ex.Message;
            }
            return ret;
        }
    }
}
