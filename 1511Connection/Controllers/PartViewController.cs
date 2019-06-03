using _1511Connection.Filter;
using _1511Connection.Models;
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
    [LoginFilter]
    public class PartViewController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Map()
        {
            return PartialView();
        }

        public ActionResult UserInfo()
        {
            var user = GetCookieUserInfo();
            user = db.stu.SingleOrDefault(t => t.id == user.id);
            ViewBag.provinces = getProvinces();
            ViewBag.cities = new List<city>();
            ViewBag.conuntries = new List<city>();
            ViewBag.pvcode = "";
            ViewBag.ctcode = "";
            ViewBag.cycode = "";
            var city = db.city.SingleOrDefault(t => t.code == user.pcode);
            if (city != null)
            {
                while (city.level > 1)
                {
                    switch (city.level)
                    {
                        case 3:
                            ViewBag.cycode = city.code;
                            break;
                        case 2:
                            ViewBag.ctcode = city.code;
                            break;
                        default:
                            break;
                    }
                    city = db.city.SingleOrDefault(t => t.code == city.parentcode);
                }
                ViewBag.pvcode = city.code;
            }
            if(ViewBag.pvcode != "")
            {
                string temp = ViewBag.pvcode;
                ViewBag.cities = db.city.Where(t => t.parentcode == temp).ToList();
                if(ViewBag.ctcode != "")
                {
                    temp = ViewBag.ctcode;
                    ViewBag.conuntries = db.city.Where(t => t.parentcode == temp).ToList();
                }
            }
            return PartialView(user);
        }

        public ActionResult SaveUserInfo(SaeUserInfoModel res)
        {
            try
            {
                var user = GetCookieUserInfo();
                user = db.stu.SingleOrDefault(t => t.id == user.id);
                user.name = res.name;
                user.phone = res.phone;
                user.qq = res.qq;
                user.weixin = res.wx;

                //更新经纬度
                city city;
                if (res.cycode > -1)
                {
                    string s_code = res.cycode.ToString();
                    city = db.city.SingleOrDefault(t => t.code == s_code);
                    user.x = city.X;
                    user.y = city.Y;
                    user.pcode = s_code;
                }
                else if (res.ctcode > -1)
                {
                    string s_code = res.ctcode.ToString();
                    city = db.city.SingleOrDefault(t => t.code == s_code);
                    user.x = city.X;
                    user.y = city.Y;
                    user.pcode = s_code;
                }
                else
                {
                    string s_code = res.pcode.ToString();
                    city = db.city.SingleOrDefault(t => t.code == s_code);
                    user.x = city.X;
                    user.y = city.Y;
                    user.pcode = s_code;
                }
                db.SaveChanges();
                return Json(new { State = 1 });
            }
            catch (Exception)
            {
                return Json(new { State = 0 });
            }
        }

        public ActionResult UpdatePsw(UpdatePswModel res)
        {
            if(res.psw1 == res.psw2)
            {
                var user = GetCookieUserInfo();
                user = db.stu.SingleOrDefault(t => t.id == user.id);
                user.psw = res.psw1.GetMD5();
                db.SaveChanges();
                return Json(new { State = 1 });
            }
            else
            {
                return Json(new { State = 0 });
            }
        }



        public List<city> getProvinces()
        {
            var result = db.city.Where(t => t.level == 1).ToList();
            return result;
        }

        public ActionResult getCities(string pcode)
        {
            var result = db.city.Where(t => t.level == 2 & t.parentcode == pcode).ToList();
            return Json(result);
        }

        public ActionResult getCountries(string pcode)
        {
            var result = db.city.Where(t => t.parentcode == pcode).ToList();
            return Json(result);
            //return Json(CityInfo.getContries(pcode));
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

    public class SaeUserInfoModel
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string qq { get; set; }
        public string wx { get; set; }
        public int pcode  { get; set; }
        public int ctcode { get; set; }
        public int cycode { get; set; }
    }

    public class UpdatePswModel
    {
        public string psw1 { get; set; }
        public string psw2 { get; set; }
    }

}