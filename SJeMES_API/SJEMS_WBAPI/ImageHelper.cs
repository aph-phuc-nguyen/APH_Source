using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SJEMS_WBAPI
{
    public class ImageHelper
    {
        public static string ChangeImageToString(object OBJ)
        {
            string XML = (string)OBJ;
            //string ret = string.Empty;
            string DllName = string.Empty;
            string ClassName = string.Empty;
            string Method = string.Empty;
            string Data = string.Empty;
            //bool IsSuccess = false;
            //string RetData = string.Empty;
            //string msg = string.Empty;
            string IP4 = string.Empty;
            string MAC = string.Empty;
            string guid = string.Empty;
            GDSJ_Framework.DBHelper.DataBase DB = new GDSJ_Framework.DBHelper.DataBase();
            DB = new GDSJ_Framework.DBHelper.DataBase(XML);
          
             
                try
                {
                DllName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<DllName>", "</DllName>");
                ClassName = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<ClassName>", "</ClassName>");
                Method = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Method>", "</Method>");
                //Data = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Data>", "</Data>");
                IP4 = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<IP4>", "</IP4>");
                MAC = GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<MAC>", "</MAC>");

                #region 接口参数
                string Picture= GDSJ_Framework.Common.StringHelper.GetDataFromFirstTag(XML, "<Picture>", "</Picture>");
                #endregion

                guid = GDSJ_Framework.Common.WebServiceHelper.SavePostLog(DB, IP4, MAC, DllName, ClassName, Method, Data);
                //Image image = Image.FromFile("D://shang//新建文件夹//SJ_EMS//SJEMS_WBAPI//Pictrue//" + Picture + "");
                string fileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Pictrue\" + Picture + "");
                Image image = Image.FromFile(fileName);
              
                return ImageToString(image);
             
              
            }
            catch (Exception ex)
            {
                return "Fail to change bitmap to string!";
            }
        }

        private static string ImageToString(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            string pic = Convert.ToBase64String(arr);


            return pic;
        }

        public static Image ChangeStringToImage(string pic)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(pic);
                //读入MemoryStream对象
                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                //转成图片
                Image image = Image.FromStream(memoryStream);

                return image;
            }
            catch (Exception)
            {
                Image image = null;
                return image;
            }
        }
    }
}
