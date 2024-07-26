using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
namespace AdminCylsys.Models
{
    public class TestimonialModel
    {
        public int id { get; set; }
        public string client_id { get; set; }
        //public string project_id { get; set; }

        public string client_name { get; set; }
        public string project_name { get; set; }
        public int rating { get; set; } = 1;
        public string testimonial_Image { get; set; }
        public string description { get; set; }
        public string path { get; set; }

    }

    public class clientlist
    {
        public int id { get; set; }
        public string client { get; set; }
    }

    public class projectlist
    {
        public int id { get; set; }
        public string project { get; set; }
    }

}