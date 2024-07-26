using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdminCylsys.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        DemoInterface IDemo = new DemoClass();
        public ActionResult Index()
        {
            List<DemoModel> result = IDemo.getDemoList();
            return View(result);
        }


        public JsonResult DeleteDemo(int demoId)

        {

            var result = IDemo.DeleteDemo(demoId);

            return Json(result);

        }
    }
}