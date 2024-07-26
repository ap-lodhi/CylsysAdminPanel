using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class AppliedJobModel
    {
        public int id { get; set; } 
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string position { get; set; }
        public int position_id { get; set; }
        public string image_name { get; set; }
        public string image_path { get; set; }
        public string requirement_Brief { get; set; }
        public DateTime requestDate { get; set; } = DateTime.Now;
    }
}