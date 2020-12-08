using KZ_MESAPI.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace KZ_MESAPI.Controllers
{
    public class MesCloudSyncServer
    {
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetSyncDBConfig(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MesCloudSyncBll bLL = new MesCloudSyncBll();
                System.Data.DataTable dt = bLL.GetSyncDBConfig(DB);
                ret.IsSuccess = true;
                ret.RetData = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
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
