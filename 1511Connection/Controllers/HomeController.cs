using _1511Connection.Filter;
using Helper;
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
            var md5_psw = lcModel.password.GetMD5();
            var stu = db.stu.SingleOrDefault(t => t.number == lcModel.username&t.psw == md5_psw);
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

        public ActionResult GetStus()
        {
            List<GetStusModel> result = new List<GetStusModel>();
            var stus = db.stu.OrderBy(t => t.x).OrderBy(t => t.y).ToList();
            decimal x = 400;
            decimal y = 100;
            GetStusModel model = new GetStusModel();
            int cnd = 0;
            foreach (stu stu in stus)
            {
                var point = getCityPoint(stu);
                if (point.X != x || point.Y != y)
                {
                    if (cnd != 0)
                    {
                        result.Add(model);
                    }
                    model = new GetStusModel();
                    model.Stus = new List<stu>();
                    model.X = point.X;
                    model.Y = point.Y;
                    var pvcode = "";
                    var ctcode = "";
                    var cycode = "";
                    var pvname = "";
                    var ctname = "";
                    var cyname = "";
                    var city = db.city.SingleOrDefault(t => t.code == stu.pcode);
                    if (city != null)
                    {
                        while (city.level > 1)
                        {
                            switch (city.level)
                            {
                                case 3:
                                    cycode = city.code;
                                    var item = db.city.SingleOrDefault(t => t.code == city.code);
                                    if (item != null)
                                    {
                                        cyname = item.name;
                                    }
                                    break;
                                case 2:
                                    ctcode = city.code;
                                    item = db.city.SingleOrDefault(t => t.code == city.code);
                                    if (item != null)
                                    {
                                        ctname = item.name;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            city = db.city.SingleOrDefault(t => t.code == city.parentcode);
                        }
                        pvcode = city.code;
                        pvname = city.name;
                    }
                    var address = pvname == "" ? "" : (pvname + (ctname == "" ? "" : (ctname + (cyname == "" ? "" : cyname))));
                    stu.pcode = address;
                    model.Stus.Add(stu);
                }
                else
                {
                    model.X = point.X;
                    model.Y = point.Y;
                    var pvcode = "";
                    var ctcode = "";
                    var cycode = "";
                    var pvname = "";
                    var ctname = "";
                    var cyname = "";
                    var city = db.city.SingleOrDefault(t => t.code == stu.pcode);
                    if (city != null)
                    {
                        while (city.level > 1)
                        {
                            switch (city.level)
                            {
                                case 3:
                                    cycode = city.code;
                                    var item = db.city.SingleOrDefault(t => t.code == city.code);
                                    if (item != null)
                                    {
                                        cyname = item.name;
                                    }
                                    break;
                                case 2:
                                    ctcode = city.code;
                                    item = db.city.SingleOrDefault(t => t.code == city.code);
                                    if (item != null)
                                    {
                                        ctname = item.name;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            city = db.city.SingleOrDefault(t => t.code == city.parentcode);
                        }
                        pvcode = city.code;
                        pvname = city.name;
                    }
                    var address = pvname == "" ? "" : (pvname + (ctname == "" ? "" : (ctname + (cyname == "" ? "" : cyname))));
                    stu.pcode = address;
                    model.Stus.Add(stu);
                }
                x = point.X;
                y = point.Y;
                cnd++;
            }
            result.Add(model);
            return Json(result);
        }

        public GerStuPosRes getCityPoint(stu stu)
        {
            GerStuPosRes result = new GerStuPosRes();
            var pvcode = "";
            var ctcode = "";
            var cycode = "";
            var pvname = "";
            var ctname = "";
            var cyname = "";
            var city = db.city.SingleOrDefault(t => t.code == stu.pcode);
            if (city != null)
            {
                while (city.level > 1)
                {
                    switch (city.level)
                    {
                        case 3:
                            cycode = city.code;
                            var item = db.city.SingleOrDefault(t => t.code == city.code);
                            if (item != null)
                            {
                                cyname = item.name;
                            }
                            break;
                        case 2:
                            ctcode = city.code;
                            item = db.city.SingleOrDefault(t => t.code == city.code);
                            if (item != null)
                            {
                                ctname = item.name;
                            }
                            break;
                        default:
                            break;
                    }
                    city = db.city.SingleOrDefault(t => t.code == city.parentcode);
                }
                pvcode = city.code;
                pvname = city.name;
            }
            if (ctcode != "")
            {
                var item = db.city.SingleOrDefault(t => t.code == ctcode);
                if (item != null)
                {
                    result.X = item.X.Value;
                    result.Y = item.Y.Value;
                }
            }
            else
            {
                var item = db.city.SingleOrDefault(t => t.code == pvcode);
                if (item != null)
                {
                    result.X = item.X.Value;
                    result.Y = item.Y.Value;
                }
            }
            return result;
        }

        public ActionResult LogOut()
        {
            try
            {
                Session.Remove("userinfo");
                return Json(new { State = 1 });
            }
            catch (Exception)
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
    public class GetStusModel
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public List<stu> Stus { get; set; }
    }

    public class GerStuPosRes
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}