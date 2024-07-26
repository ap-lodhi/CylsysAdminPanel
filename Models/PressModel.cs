using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class PressModel
    {
		public int Id { get; set; }
		public string ImagePath { get; set; }
		public string Heading { get; set; }
		public string Url { get; set; }
		public string FileName { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsActive { get; set; }
	}
}