using MySqlUnit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _1511Connection.Controllers
{
    public class ImageController : Controller
    {
        private Entities db = new Entities();
        
        //private string ImgPath = "D:\\";
        private string PublishedImgPath = "http://www.820803.xyz:2004/Upload/";

        /// <summary>
        /// 上传图片接口
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public ActionResult UploadImg(UploadImg img)
        {
            string test = "";
            test = img.Content;
            var user = GetCookieUserInfo();
            if (user != null)
            {
                user = db.stu.SingleOrDefault(t => t.id == user.id);
            }
            try
            {
                string jpgcontent = img.Content.Split(',')[1];
                string type = img.Content.Split(';')[0].Split('/')[1];
                var filename = user.id + "-" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "." + type;
                var path = Server.MapPath("~/Upload/") + filename;
                //if (type != "jpg")
                //{
                //    Base64ToImg(jpgcontent).Save(path);
                //}
                //else
                //{
                //    Compress(Base64ToImg(jpgcontent), path, 40);
                //}
                Base64ToImg(jpgcontent).Save(path);
                System.Drawing.Image localimage = System.Drawing.Image.FromFile(path);
                //double hdivw = (double)localimage.Height / (double)localimage.Width;
                string url = PublishedImgPath + filename;
                user.headUrl = url;
                db.SaveChanges();
                return Json(new { State = 1,Url = url});
            }
            catch (Exception ex)
            {
                return Json(new { State = 0 });
            }

        }
        //解析base64编码获取图片
        private Bitmap Base64ToImg(string base64Code)
        {
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Code));
            return new Bitmap(stream);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitmap">传入的Bitmap对象</param>
        /// <param name="destStream">压缩后的Stream对象</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with 给定的 quality level
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitMap">传入的Bitmap对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level);
            s.Close();
        }

        public stu GetCookieUserInfo()
        {
            try
            {
                string t = Session["userinfo"].ToString(); ;
                Encoding utf8 = Encoding.GetEncoding(65001);
                byte[] temp = utf8.GetBytes(t);
                t = utf8.GetString(temp);
                stu result = JsonConvert.DeserializeObject<stu>(t);

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

    public class UploadImg
    {
        public string Content { get; set; }//base64码内容
        //public string phoneNumber { get; set; }//手机号
    }
}