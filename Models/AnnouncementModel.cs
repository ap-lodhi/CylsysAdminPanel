using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class AnnouncementModel
    {

        public int id { get; set; }
        public string announcement_Name { get; set; }
        public string announcement_image { get; set; }
        public string image_path { get; set; }
        public bool IsActive { get; set; }
        public DateTime start_Date { get; set; }
        public DateTime end_Date { get; set; }
        public string description { get; set; }

    }
}