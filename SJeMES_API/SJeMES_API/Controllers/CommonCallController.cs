using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SJeMES_API.Controllers
{

    [Route("api/[controller]")]
    public class CommonCallController : Controller
    {
        [HttpGet]
        public string Get()
        {
            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            string s = Newtonsoft.Json.JsonConvert.SerializeObject("欢迎使用万邦鞋业有限公司Web API");

            return s;
        }

        [HttpPost]
        public string Post([FromBody]object parameter)
        {
            string lanauage = Request.Headers["lanauage"];

            SJeMES_Framework_NETCore.WebAPI.ResultObject ret = new SJeMES_Framework_NETCore.WebAPI.ResultObject();

            SJeMES_Framework_NETCore.WebAPI.RequestObject p = new SJeMES_Framework_NETCore.WebAPI.RequestObject();

            SJeMES_Framework_NETCore.WebAPI.RequestObject.CurrentLanguage = lanauage;
            string guid = string.Empty;

            try
            {
                //var p = ReqObj.ToObj(parameter);
                try
                {
                    p = Newtonsoft.Json.JsonConvert.DeserializeObject<SJeMES_Framework_NETCore.WebAPI.RequestObject>(parameter.ToString());
                }
                catch (Exception)
                {
                    string RASKey = string.Empty;
                    SJeMES_Framework_NETCore.DBHelper.DataBase DB = new SJeMES_Framework_NETCore.DBHelper.DataBase(string.Empty);
                    RASKey = DB.GetString(@"SELECT Key2 FROM rasinfo");

                    string data = SJeMES_Framework_NETCore.Common.RASHelper.RASDecrypt(parameter.ToString(), RASKey);
                    p = Newtonsoft.Json.JsonConvert.DeserializeObject<SJeMES_Framework_NETCore.WebAPI.RequestObject>(data);
                }

                //guid = SJeMES_Framework_NETCore.Web.System.Log(p);
                Assembly assembly = null;




#if DEBUG  
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
                assembly = Assembly.LoadFrom(path + @"\" + p.DllName + ".dll");
#else
                    assembly = Assembly.LoadFrom(p.DllName + ".dll");
#endif






                Type type = assembly.GetType(p.ClassName);

                object instance = null;


                instance = Activator.CreateInstance(type);


                MethodInfo mi = type.GetMethod(p.Method);


                object[] args = new object[1];

                args[0] = p;

                object obj = mi.Invoke(instance, args);
                ret = (SJeMES_Framework_NETCore.WebAPI.ResultObject)obj;

                if (p.IsRasResult)
                {
                    ret.RetData = SJeMES_Framework_NETCore.Common.RASHelper.RASEncryption(p.RasResultKey, ret.RetData);
                }
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = ex.Message;

            }
            if (!string.IsNullOrEmpty(ret.ErrMsg))
            {
                ret.ErrMsg = SJeMES_Framework_NETCore.Common.UIHelper.UImsg(ret.ErrMsg, lanauage, "http://" + Request.Host.Value + "/api/CommonCall", p.UserToken);
            }
            string retjson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);

            //SJeMES_Framework_NETCore.Web.System.UpdateLog(guid, retjson, p);

            return retjson;
        }
    }
}
