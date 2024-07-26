using System;

namespace AdminCylsys.Models
{

    public class CaseStudiesEdit_Mod

    {


        public int id { get; set; }

        public string case_studies_Name { get; set; }

        public string description { get; set; }

        public string upload_case_study { get; set; }

        public string path { get; set; }

        public string size { get; set; }

        public string stringActive { get; set; }

        public bool Active { get; set; }

        public DateTime created_date { get; set; }

        public string created_by { get; set; }

        public DateTime modified_date { get; set; }

        public string modified_by { get; set; }




    }
}