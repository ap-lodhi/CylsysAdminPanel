using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminCylsys.Models
{
    public class Register
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}