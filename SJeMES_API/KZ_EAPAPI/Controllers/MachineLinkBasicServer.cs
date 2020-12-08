using KZ_EAPAPI.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace KZ_EAPAPI.Controllers
{
    public class MachineLinkBasicServer
    {
        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryMachineList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vMachineID = jarr["vMachineID"].ToString();
                string vFactory = jarr["vFactory"].ToString();
                string vDept = jarr["vDept"].ToString();
                string vType = jarr["vType"].ToString();
                string vWhetherLink = jarr["vWhetherLink"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                System.Data.DataTable dt = bLL.QueryMachineList(DB, vMachineID, vFactory, vDept, vType, vWhetherLink);
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
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryDeptList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vFactory = jarr["vFactory"].ToString();
                string vDept = jarr["vDept"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                System.Data.DataTable dt = bLL.QueryDeptList(DB, vFactory, vDept);
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
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject QueryMachineIdByDept(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vDept = jarr["vDept"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                System.Data.DataTable dt = bLL.QueryMachineIdByDept(DB,vDept);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetAllMachineType(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                System.Data.DataTable dt = bLL.GetAllMachineType(DB);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetFactory(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                System.Data.DataTable dt = bLL.GetFactory(DB);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject AddMachineContrast(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vMachineID = jarr["vMachineID"].ToString();
                string vMachineNO = jarr["vMachineNO"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                string result = bLL.AddMachineContrast(DB, vMachineID, vMachineNO, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken));
                ret.IsSuccess = true;
                ret.RetData = result; 
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject DelMachineContrastById(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vMachineID = jarr["vMachineID"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                bLL.DelMachineContrastById(DB, vMachineID);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetMachineContrastList(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vFactory = jarr["vFactory"].ToString();
                string vType = jarr["vType"].ToString();
                string vMachineID = jarr["vMachineID"].ToString();
                string vMachineNO = jarr["vMachineNO"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                System.Data.DataTable dt = bLL.GetMachineContrastList(DB, vFactory, vType, vMachineID, vMachineNO);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject GetEapContrastListByType(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vType = jarr["vType"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                System.Data.DataTable dt = bLL.GetEapContrastListByType(DB, vType);
                ret.IsSuccess = true;
                ret.RetData = SJeMES_Framework_NETCore.Common.JsonHelper.GetJsonByDataTable(dt);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject UpdateLinkedStatusById(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string vMachineID = jarr["vMachineID"].ToString();
                string vWhetherLink = jarr["vWhetherLink"].ToString();
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                bLL.UpdateLinkedStatusById(DB, vMachineID, vWhetherLink);
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        public static SJeMES_Framework_NETCore.WebAPI.ResultObject updateEapRftAndRate(object OBJ)
        {
            SJeMES_Framework_NETCore.WebAPI.RequestObject ReqObj = (SJeMES_Framework_NETCore.WebAPI.RequestObject)OBJ;
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();
            string Data = string.Empty;
            SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase();
            try
            {
                Data = ReqObj.Data.ToString();
                var jarr = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Object>>(Data);
                string data = jarr["data"].ToString();
                System.Data.DataTable dt = SJeMES_Framework_NETCore.Common.JsonHelper.GetDataTableByJson(data);
                DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(ReqObj);
                MachineLinkBasicBLL bLL = new MachineLinkBasicBLL();
                bLL.updateEapRftAndRate(DB, dt, SJeMES_Framework_NETCore.Web.System.GetUserCodeByToken(ReqObj.UserToken));
                ret.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }
    }
}




