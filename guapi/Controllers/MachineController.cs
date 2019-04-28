using MySqlUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace guapi.Controllers
{
    public class MachineController : ApiController
    {
        private guyunhaoEntities db = new guyunhaoEntities();

        [HttpGet]
        public List<machine> GetErrorMachine()
        {
            string sql = "select * from machine where id in(select MachineId FROM `status` where case WHEN MachineType = 0 THEN isRuning = 0 WHEN MachineType = 1 THEN (attr_value < min_attr_value OR attr_value > max_attr_value) END GROUP BY MachineId)";
            List<machine> result = db.Database.SqlQuery<machine>(sql).ToList();
            return result;
        }

        [HttpPost]
        public List<machine> PostErrorMachine()
        {
            string sql = "select * from machine where id in(select MachineId FROM `status` where case WHEN MachineType = 0 THEN isRuning = 0 WHEN MachineType = 1 THEN (attr_value < min_attr_value OR attr_value > max_attr_value) END GROUP BY MachineId)";
            List<machine> result = db.Database.SqlQuery<machine>(sql).ToList();
            return result;
        }

        [HttpGet]
        public List<machine> GetMachines(int type)
        {
            List<machine> result = new List<machine>();
            
            //所有的
            if(type == 0)
            {
                result = db.machine.ToList();
            }
            //有问题的
            else if (type == 1 )
            {
                string sql = "select * from machine where id in(select MachineId FROM `status` where case WHEN MachineType = 0 THEN isRuning = 0 WHEN MachineType = 1 THEN (attr_value < min_attr_value OR attr_value > max_attr_value) END GROUP BY MachineId)";
                result = db.Database.SqlQuery<machine>(sql).ToList();
            }
            //没问题的
            else if (type == 2)
            {
                string sql = "select * from machine where id in(select MachineId FROM `status` where case WHEN MachineType = 0 THEN isRuning = 1 WHEN MachineType = 1 THEN (attr_value >= min_attr_value OR attr_value <= max_attr_value) END GROUP BY MachineId)";
                result = db.Database.SqlQuery<machine>(sql).ToList();
            }

            return result;
        }

        [HttpGet]
        public List<machine> SearchMachines(string key)
        {
            int ikey = 0;
            try
            {
                ikey = int.Parse(key);
            }
            catch (Exception e)
            {

            }
            var result = db.machine.Where(t => t.id == ikey || t.name.Contains(key)).ToList();
            return result;
        }

        [HttpGet]
        public List<machine> SearcErrorhMachines(string key)
        {
            string sql = "select * from machine where id in(select MachineId FROM `status` where case WHEN MachineType = 0 THEN isRuning = 0 WHEN MachineType = 1 THEN (attr_value < min_attr_value OR attr_value > max_attr_value) END GROUP BY MachineId)";
            List<machine> result = db.Database.SqlQuery<machine>(sql).ToList();
            int ikey = 0;
            try
            {
                ikey = int.Parse(key);
            }
            catch (Exception e)
            {

            }
            result = result.Where(t => t.id == ikey || t.name.Contains(key)).ToList();
            return result;
        }


        [HttpGet]
        public List<farm> GetFarms()
        {
            var result = db.farm.ToList();
            return result;
        }

        [HttpGet]
        public List<farm> SearchFarms(string key)
        {
            int ikey = 0;
            try {
                ikey = int.Parse(key);
            }catch(Exception e)
            {

            }
            var result = db.farm.Where(t => t.id == ikey || t.name.Contains(key)).ToList();
            return result;
        }

        [HttpGet]
        public List<machine> GetMachinesByFarm(int farmid)
        {
            var result = db.machine.Where(t => t.farmid == farmid).ToList();
            return result;
        }

        [HttpGet]
        public List<machine> GetErrorMachinesByFarm(int farmid)
        {
            string sql = "select * from machine where farmid = " + farmid + " AND id in(select MachineId FROM `status` where case WHEN MachineType = 0 THEN isRuning = 0 WHEN MachineType = 1 THEN (attr_value < min_attr_value OR attr_value > max_attr_value) END GROUP BY MachineId)";

            var result = db.Database.SqlQuery<machine>(sql).ToList();
            return result;
        }

        /// <summary>
        /// 获取某台机器的所有属性状态
        /// </summary>
        /// <param name="machineid">机器id</param>
        /// <param name="type">0:机器的所有属性，1:机器的有问题的属性,2:机器正常的属性</param>
        /// <returns></returns>
        [HttpGet]
        public List<status> GetStatuses(int machineid,int type = 0)
        {
            List<status> result = new List<status>();
            string sql = "";
            result = db.status.Where(t => t.MachineId == machineid).ToList();
            //有问题的
            if(type == 1)
            {
                sql = "select * from `status` where id in(select id FROM `status` where MachineId = " + machineid + " AND case WHEN MachineType = 0 THEN isRuning = 0 WHEN MachineType = 1 THEN (attr_value < min_attr_value OR attr_value > max_attr_value) END)";
                result = db.Database.SqlQuery<status>(sql).ToList();
            }
            //没问题的
            else if(type == 2)
            { 
                sql = "select * from `status` where id in(select id FROM `status` where MachineId = " + machineid + " AND case WHEN MachineType = 0 THEN isRuning = 1 WHEN MachineType = 1 THEN (attr_value >= min_attr_value AND attr_value <= max_attr_value) END)";
                result = db.Database.SqlQuery<status>(sql).ToList();
            }
            return result;
        }

        [HttpGet]
        public string UpdateStatus(decimal data,int statusid)
        {
            try
            {
                var status = db.status.SingleOrDefault(t => t.id == statusid);
                if (status != null)
                {
                    if (status.MachineType == 0)
                    {
                        if(data == 0)
                        {
                            status.isRuning = 0;
                        }
                        else
                        {
                            status.isRuning = 1;
                        }
                    }
                    else
                    {
                        status.attr_value = data;
                    }
                    db.SaveChanges();
                    return "success";
                }
                return "fail";
            }catch(Exception ex)
            {
                return "fail";
            }
            
        }
        
    }
}
