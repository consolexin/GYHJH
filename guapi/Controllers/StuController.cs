using guapi.Models;
using MySqlUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace guapi.Controllers
{
    public class StuController : ApiController
    {
        private Entities db = new Entities();
        [HttpPost]
        public List<GetStusModel> GetStus()
        {
            List<GetStusModel> result = new List<GetStusModel>();
            var stus = db.stu.OrderBy(t => t.x).OrderBy(t => t.y).ToList();
            decimal x = 400;
            decimal y = 100;
            GetStusModel model = new GetStusModel();
            int cnd = 0;
            foreach (stu stu in stus)
            {
                if (stu.x != x || stu.y != y)
                {
                    if(cnd != 0)
                    {
                        result.Add(model);
                    }
                    model = new GetStusModel();
                    model.Stus = new List<stu>();
                    model.X = stu.x.Value;
                    model.Y = stu.y.Value;
                    model.Stus.Add(stu);
                }
                else
                {
                    model.X = stu.x.Value;
                    model.Y = stu.y.Value;
                    model.Stus.Add(stu);
                }
                x = stu.x.Value;
                y = stu.y.Value;
                cnd++;
            }
            result.Add(model);
            return result;
        }
        
        [HttpGet]
        public GerStuPosRes SearchStu(string key)
        {
            GerStuPosRes result = new GerStuPosRes();
            var stu = db.stu.FirstOrDefault(t => t.number == key || t.name == key);
            if (stu != null)
            {
                result.X = stu.x.Value;
                result.Y = stu.y.Value;
            }
            else
            {
                result.X = -1;
                result.Y = -1;
            }
            return result;
        }
        [HttpPost]
        public List<city> getProvinces()
        {
            var result = db.city.Where(t => t.level == 1).ToList();
            return result;
        }

        [HttpGet]
        public List<city> getCities(string pcode)
        {
            var result = db.city.Where(t => t.level == 2&t.parentcode == pcode).ToList();
            return result;
        }

        [HttpGet]
        public List<city> getCountries(string pcode)
        {
            var result = db.city.Where(t => t.level == 3 & t.parentcode == pcode).ToList();
            return result;
        }
    }

    public class GerStuPosRes
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }


}
