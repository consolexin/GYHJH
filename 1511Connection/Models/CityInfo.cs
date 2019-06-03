using Helper;
using MySqlUnit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1511Connection.Models
{
    public class CityInfo
    {
        private static string PrivinceUrl = "http://datavmap-public.oss-cn-hangzhou.aliyuncs.com/areas/csv/100000_province.json";
        private static string CityUrl = "http://datavmap-public.oss-cn-hangzhou.aliyuncs.com/areas/csv/{0}_city.json";
        private static string CountryUrl = "http://datavmap-public.oss-cn-hangzhou.aliyuncs.com/areas/csv/{0}_district.json";


        public static void SavePrivinceInfo()
        {
            var result = MyhttpRequestHelper.GetDataGetHtml(PrivinceUrl);
            JObject jobject = (JObject)JsonConvert.DeserializeObject(result);
            var db = new Entities();
            foreach (JObject province in jobject["rows"])
            {
                var new_province = new city();
                new_province.name = province["name"].ToString();
                new_province.level = 1;
                new_province.code = province["adcode"].ToString();
                new_province.parentcode = "china";
                new_province.X = decimal.Parse(province["lng"].ToString());
                new_province.Y = decimal.Parse(province["lat"].ToString());
                db.city.Add(new_province);
                //获取市
                try
                {
                    var cityResult = MyhttpRequestHelper.GetDataGetHtml(string.Format(CityUrl, new_province.code));
                    JObject cityObj = (JObject)JsonConvert.DeserializeObject(cityResult);
                    foreach (JObject city in cityObj["rows"])
                    {
                        var new_city = new city();
                        new_city.name = city["name"].ToString();
                        new_city.level = 2;
                        new_city.code = city["adcode"].ToString();
                        new_city.parentcode = new_province.code;
                        new_city.X = decimal.Parse(city["lng"].ToString());
                        new_city.Y = decimal.Parse(city["lat"].ToString());
                        db.city.Add(new_city);
                        //获取县
                        try
                        {
                            var countryResult = MyhttpRequestHelper.GetDataGetHtml(string.Format(CountryUrl, new_city.code));
                            JObject countryObj = (JObject)JsonConvert.DeserializeObject(countryResult);
                            foreach (JObject country in countryObj["rows"])
                            {
                                var new_country = new city();
                                new_country.name = country["name"].ToString();
                                new_country.level = 3;
                                new_country.code = country["adcode"].ToString();
                                new_country.parentcode = new_city.code;
                                new_country.X = decimal.Parse(country["lng"].ToString());
                                new_country.Y = decimal.Parse(country["lat"].ToString());
                                db.city.Add(new_country);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        
                    }
                }
                catch (Exception)
                {
                    continue;
                }
                finally
                {
                    db.SaveChanges();
                }
            }
            db.SaveChanges();
        }

        public static List<city> getContries(string code)
        {
            var db = new Entities();
            List<city> result = new List<city>();
            var countryResult = MyhttpRequestHelper.GetDataGetHtml(string.Format(CountryUrl, code));
            JObject countryObj = (JObject)JsonConvert.DeserializeObject(countryResult);
            foreach (JObject country in countryObj["rows"])
            {
                var new_country = new city();
                new_country.name = country["name"].ToString();
                new_country.level = 3;
                new_country.code = country["adcode"].ToString();
                new_country.parentcode = country["parent"].ToString();
                new_country.X = decimal.Parse(country["lng"].ToString());
                new_country.Y = decimal.Parse(country["lat"].ToString());
                result.Add(new_country);
            }
            return result;
        }

        public static void update()
        {
            var db = new Entities();
            var cities = db.city.Where(t => t.level == 2).ToList();
            List<string> codes = new List<string>();
            for(int i = 0; i < cities.Count(); i++)
            {
                if (codes.Contains(cities[i].parentcode))
                {
                    continue;
                }
                else
                {
                    var temp = getContries(cities[i].code);
                    for(int j = 0;j< temp.Count(); j++)
                    {
                        foreach(city ct in cities)
                        {
                            if(ct.name == temp[j].parentcode)
                            {
                                temp[j].parentcode = ct.code;
                                db.city.Add(temp[j]);
                                db.SaveChanges();
                                break;
                            }
                        }
                    }
                    codes.Add(cities[i].parentcode);
                }
            }
        }
   
    }
}