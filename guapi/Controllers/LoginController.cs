using MySqlUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace guapi.Controllers
{
    public class LoginController : ApiController
    {
        private guyunhaoEntities db = new guyunhaoEntities();

        [HttpGet]
        public object GetLogin(string username, string password)
        {
            var user = db.user.SingleOrDefault(t => t.username == username & t.password == password);
            if (user == null)
            {
                return new { Status = 0 };
            }
            user.password = "";
            return new { Status = 1, Info = user };
        }

        [HttpPost]
        public object Login([FromBody]thisuser thisuser)
        {
            string username = thisuser.username;
            string password = thisuser.password;
            var user = db.user.SingleOrDefault(t => t.username == username & t.password == password);
            if(user == null)
            {
                return new { Status = 0 };
            }
            user.password = "";
            return new { Status = 1, Info = user };
        }
    }

    public class thisuser
    {
        public string username { get; set; }
        public string password { get; set; }
    }

}
