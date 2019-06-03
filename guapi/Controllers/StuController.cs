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
                var point = getCityPoint(stu);
                if (point.X != x || point.Y != y)
                {
                    if(cnd != 0)
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
                    var address = pvname == ""?"" : (pvname + (ctname == "" ? "" : (ctname + (cyname == "" ? "" : cyname))));
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
            return result;
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
            if(ctcode != "")
            {
                var item = db.city.SingleOrDefault(t => t.code == ctcode);
                if(item != null)
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
