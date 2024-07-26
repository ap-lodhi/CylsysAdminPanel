using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminCylsys.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        PostandJobInterface IPost = new PostClass();
        public ActionResult Index()
        {
            List<PostUser> result = IPost.getPostAndJobsList();
            if (Session["Result"] != null)
            {
                return View(result);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            
            
        }
    }
}