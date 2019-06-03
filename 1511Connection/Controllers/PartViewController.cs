using _1511Connection.Filter;
using _1511Connection.Models;
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
            return PartialView(user);
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
}