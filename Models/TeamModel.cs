using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class TeamModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string EventName { get; set; }

        [StringLength(250)]
        public string UploadTeamImage { get; set; }

        [StringLength(155)]
        public string AltContent { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Size { get; set; }

        [StringLength(150)]
        public string Path { get; set; }


        public DateTime? CreatedDate { get; set; }

        [StringLength(45)]
        public string CreatedBy { get; set; }

        [StringLength(45)]
        public string ModifiedBy { get; set; }


    }
}