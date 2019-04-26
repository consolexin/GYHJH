using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace guapi.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        public object test()
        {
            return new { msg=2 };
        }
    }
}
