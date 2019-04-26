using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jhh5ios.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "后台管理";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}
