using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
   public class PostUser
    {
        public int id { get; set; }
        public string position { get; set; }
        public string country_name { get; set; }
        public string country_id { get; set; }
        public string state_name { get; set; }
        public string state_id { get; set; }
        public string city_name { get; set; }
        public string city_id { get; set; }

        public string expid { get; set; }
        public string experience { get; set; }
        public string joining_period { get; set; }
        public string mail_Id { get; set; }
        public int number_of_position { get; set; }
        public string pay_id { get; set; }
        public string Payroll { get; set; }
        public DateTime valid_till { get; set; }
        public string phone_number { get; set; }
        public string budget { get; set; }
        public string description { get; set; }
        public bool Active { get; set; }

    }
    public class countrylist
    {
        public int country_id { get; set; }
        public string country_name { get; set; }

    }

    public class statelist
    {
        public int id { get; set; }
        public string state_name { get; set; }

    }

    public class citylist
    {
        public int city_id { get; set; }
        public string city_name { get; set; }

    }
    public class payrolllist
    {
        public int id { get; set; }
        public string Payroll { get; set; }

    }

    public class experiencelist
    {
        public int id { get; set; }
        public string experience { get; set; }

    }

}