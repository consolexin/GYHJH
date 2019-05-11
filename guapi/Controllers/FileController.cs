using MySqlUnit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Web;
using System.Web.Http;

namespace guapi.Controllers
{
    public class FileController : ApiController
    {
        private guyunhaoEntities db = new guyunhaoEntities();
        private string serverPath = "http://www.mzhdemo.xyz:2001/UploadFile/";
        [HttpPost]
        public int PostFile()
        {
            //HttpResponseMessage result = null;
            int result = 0;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var userid = int.Parse(httpRequest.Form["userid"].ToString());
                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[0];
                    var filename = postedFile.FileName;
                    var path = HttpContext.Current.Server.MapPath("~/UploadFile/");
                    var nowtime = DateTime.Now;
                    var time = nowtime.ToString("yyyyMMddHHmmss");
                    var filePath = path + userid + "-" + time + "-" + filename;
                    //var filePath = path + userid+","+DateTime.Now.ToString("yyyyMMddHH:mm:ss")+","+filename;
                    postedFile.SaveAs(filePath);

                    var user = db.user.SingleOrDefault(t => t.id == userid);
                    uploadfiles uploadfiles = new uploadfiles();
                    uploadfiles.name = filename;
                    uploadfiles.date = nowtime;
                    uploadfiles.userid = userid;
                    uploadfiles.url = filePath.Replace(path, serverPath);
                    uploadfiles.level = user.level;
                    db.uploadfiles.Add(uploadfiles);
                    db.SaveChanges();
                    result = 1;
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
            
        }


        [HttpGet]
        public List<uploadfiles> getAllFilesByUser(int userid)
        {
            List<uploadfiles> uploadfiles = new List<uploadfiles>();
            var user = db.user.SingleOrDefault(t => t.id == userid);
            if (user == null) return uploadfiles;
            uploadfiles = db.uploadfiles.Where(t => t.level <= user.level).OrderByDescending(t => t.date).ToList();
            return uploadfiles;
        } 
        
    }


}
