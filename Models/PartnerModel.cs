using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class PartnerModel
    {
        public int id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public string Organization_name { get; set; }
        public string Organization_type { get; set; }
        public string Expectations { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public bool Active { get; set; }


    }

    public class OrganizationList
    {
        public int id { get; set; }
        public string organization_type { get; set; }

    }
}