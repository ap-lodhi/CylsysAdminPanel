using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class RequestQuoteModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string services { get; set; }
        public string requirementBrief { get; set; }
        public DateTime requestDate { get; set; } = DateTime.Now;
    }

    public class ServiceModel
    {
        public int id { get; set; }
        public string serviceName { get; set; }
    }
    }