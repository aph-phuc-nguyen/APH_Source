using System;
using System.Collections.Generic;
using System.Text;

namespace KZ_MESAPI.Controllers
{
    public class ComboBoxUIServer
    {
        //public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetComboBoxUI(object OBJ)
        //{
        //    SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
        //    SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
        //    string Data = string.Empty;
        //    string guid = string.Empty;
        //    SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(String.Empty);
        //    try
        //    {
        //        Data = ReqObj.Data.ToString();
        //        var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
        //        string ui_title = jarr["ui_title"].ToString();
        //        string Language = SJeMES_Framework_NETCore.WebAPI.RequestObject.CurrentLanguage;
        //        string sql = @"select ui_code,ui_id,
        //                       CASE '"+ Language + @"'
        //                            WHEN 'cn' THEN ui_cn
        //                            WHEN 'en' THEN ui_en
        //                            WHEN 'yn' THEN ui_yn
        //                            END AS ui_cn from  SJQDMS_UILAN where ui_tittle = 'ProductionTargetsForm' and ui_code like '%comboBox%' order by ui_code";
        //        System.Data.DataTable dt=DB.GetDataTable(sql);
        //        if (dt.Rows.Count > 0)
        //        {
        //            ret.IsSuccess = true;
        //            ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
        //        }
        //        else
        //        {
        //            ret.IsSuccess = false;
        //            ret.ErrMsg = "查无此数据！";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ret.IsSuccess = false;
        //        ret.ErrMsg = "00000:" + ex.Message;
        //    }
        //    return ret;
        //}

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetComboBoxUI(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            string guid = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
            SJeMES_Framework_NETCore.DBHelper.DataBase DBSYS = new SJeMES_Framework_NETCore.DBHelper.DataBase(String.Empty);
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string enmu_type = jarr["enmu_type"].ToString();
                string Language = SJeMES_Framework_NETCore.WebAPI.RequestObject.CurrentLanguage;
                string sql = @"select enum_code,
	             (case '"+Language+ @"'
	             when 'cn' then ui_cn
	             when 'en' then ui_en
	             when 'yn' then ui_yn
                 when 'hk' then ui_yn
	             else enum_value end) enum_value
                 from SYS001M where enum_type='" + enmu_type+"'";
                System.Data.DataTable dt = DB.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    ret.IsSuccess = true;
                    ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
                }
                else
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "查无此数据！";
                }
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
