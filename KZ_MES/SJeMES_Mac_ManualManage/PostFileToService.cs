using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SJeMES_Mac_ManualManage
{
  public static  class PostFileToService
    {
        /// <summary>
        /// Post文件到服务器
        /// </summary>
        /// <param name="tagHost">目标主机地址地址</param>
        /// <param name="filePath">上传的文件，本地路径</param>
        /// <returns></returns>
        public static bool PostFile(string tagHost, string filePath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tagHost);//目标主机ip地址
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] infbytes = new byte[(int)fs.Length];
                fs.Read(infbytes, 0, infbytes.Length);
                fs.Close();

                string cookieheader = string.Empty;
                CookieContainer cookieCon = new CookieContainer();
                request.Method = "POST";
                request.CookieContainer = cookieCon;
                request.ContentType = "text/html";
                request.ContentLength = infbytes.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(infbytes, 0, infbytes.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //获得响应流
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                response.Close();
                receiveStream.Close();
                readStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="tagHost"></param>
        /// <returns></returns>
        public static bool UpLoadFile(string savePath,string tagHost)
        {
            try
            {
                #region MyRegion
                // //HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(tagHost);

                // //HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                // //Stream st = webResponse.GetResponseStream();
                // //向服务器请求，获得服务器的回应数据流
                // WebRequest myWebRequest = WebRequest.Create(tagHost);
                //HttpWebRequest web= (HttpWebRequest)HttpWebRequest.Create(tagHost);
                // WebClient client = new WebClient();
                // client.DownloadFile(tagHost, tagHost);
                //byte[] arraryByte;
                //Uri u = new Uri(tagHost);
                //HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
                //mRequest.Method = "GET";
                //mRequest.ContentType = "application/x-www-form-urlencoded";

                //HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();
                //int length = (int)wr.ContentLength;
                //byte[] bs = new byte[length];

                //HttpWebResponse response = wr as HttpWebResponse;
                //Stream stream = response.GetResponseStream();

                ////读取到内存
                //MemoryStream stmMemory = new MemoryStream();
                //byte[] buffer1 = new byte[length];
                //int i;
                ////将字节逐个放入到Byte 中
                //while ((i = stream.Read(buffer1, 0, buffer1.Length)) > 0)
                //{
                //    stmMemory.Write(buffer1, 0, i);
                //}
                //arraryByte = stmMemory.ToArray();
                //stmMemory.Close();



                //Stream sIn = wr.GetResponseStream();
                //FileStream fs = new FileStream(savePath+"管理2.mp3", FileMode.Create, FileAccess.Write);

                //    fs.Write(arraryByte, 0, arraryByte.Length);
                //    fs.Close();
                //    FileStream fs = new FileStream(@"D:\光良 - 童话.mp3", FileMode.Create, FileAccess.Write);
                //    fs.Write(infbytes, 0, infbytes.Length);
                //    fs.Close();

                //string b = "";
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tagHost);
                //request.Method = WebRequestMethods.Http.Get;
                //System.Net.WebResponse wResp = request.GetResponse();
                //foreach (string item in wResp.Headers)
                //{
                //    if (item == "Content-File")
                //    {

                //        b= wResp.Headers[item];

                //    }
                //}
                //byte[] byteArray = System.Text.Encoding.Default.GetBytes(b);
                //FileStream fs = new FileStream(savePath + "1.mp3", FileMode.Create, FileAccess.Write);
                //fs.Write(byteArray, 0, byteArray.Length);
                //   fs.Close();

                ////直到request.GetResponse()程序才开始向目标网页发送Post请求
                //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                //{
                //    var baiduInnerHtml = sr.ReadToEnd();
                //}
                ////创建本地文件写入流
                //Stream stream = new FileStream(savePath+"1.mp3", FileMode.Create);


                #endregion
                WebClient client = new WebClient();
                client.DownloadFile(savePath, tagHost);
                return true;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        
        public static bool DeleteFile(string tagHost)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tagHost);//目标主机ip地址
                request.Method = "Get";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //获得响应流
                Stream receiveStream = response.GetResponseStream();
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
    }
}
