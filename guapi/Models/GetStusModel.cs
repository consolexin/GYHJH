using MySqlUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace guapi.Models
{
    public class GetStusModel
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public List<stu> Stus { get; set; }
    }
}