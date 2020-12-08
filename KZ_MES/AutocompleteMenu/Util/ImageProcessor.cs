using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace mes_master.Util
{
    public class ImageProcessor
    {
        public ImageProcessor(string filePath)
        {
            this.FilePath = filePath;
        }

        public string FilePath { get; private set; }

        /// <summary>
        /// 调整图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="scale">按比例调整</param>
        /// <returns></returns>
        public Image ResizeImage(Image image, double scale)
        {
            double width = (double)image.Width * scale;
            double height = (double)image.Height * scale;

            Bitmap bitmap = new Bitmap((int)width, (int)height);
            Graphics g = Graphics.FromImage(bitmap as Image);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, (int)width, (int)height);
            g.Dispose();

            return (bitmap as Image);
        }

        /// <summary>
        /// 调整图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="scale">按比例调整</param>
        /// <returns></returns>
        public Image ResizeImage(Bitmap image, double scale)
        {
            return this.ResizeImage((image as Image), scale);
        }

        /// <summary>
        /// Bytes转Bitmap
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                return new Bitmap(stream);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Bitmap转byte[]   
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                bitmap.Save(ms, bitmap.RawFormat);
                return ms.ToArray();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }
        }
    }
}