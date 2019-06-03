using MySqlUnit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace _1511Connection.Filter
{
    public class LoginFilter: System.Web.Mvc.ActionFilterAttribute
    {
        //加载Action前调用  
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //在此也可以进行权限的验证  
            base.OnActionExecuting(filterContext);
            var t = GetCookieUserInfo();
            if (t == null)
            {
                filterContext.HttpContext.Response.Redirect("/Home/login", true);
            }
        }

        public stu GetCookieUserInfo()
        {
            try
            {
                string t = HttpContext.Current.Session["userinfo"].ToString();
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