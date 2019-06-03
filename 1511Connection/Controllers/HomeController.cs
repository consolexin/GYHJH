using _1511Connection.Filter;
using MySqlUnit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _1511Connection.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        [LoginFilter]
        public ActionResult Index()
        {
            var user = GetCookieUserInfo();
            if(user != null)
            {
                user = db.stu.SingleOrDefault(t => t.id == user.id);
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginCheck(LoginCheckModel lcModel)
        {
            var stu = db.stu.SingleOrDefault(t => t.number == lcModel.username);
            if (stu != null)
            {
                Session["userinfo"] = JsonConvert.SerializeObject(stu);
                return Json(new { State = 1});
            }
            else
            {
                return Json(new { State = 0 });
            }
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

    public class LoginCheckModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}