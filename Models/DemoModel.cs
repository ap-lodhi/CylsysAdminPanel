using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class DemoModel
    {
        public int demo_id { get; set; }
        public string application_id { get; set; }
        public string user_name { get; set; }
        public string email_id { get; set; }
        public string mobile_no { get; set; }
        public string country_id { get; set; }
        public string details { get; set; }
    }
}