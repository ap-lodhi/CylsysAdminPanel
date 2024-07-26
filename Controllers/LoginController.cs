using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System.Web.Mvc;

namespace AdminCylsys.Controllers
{
    public class LoginController : Controller
    {
        LoginInterface ILogin = new RegUser();
        ResponseModel result = new ResponseModel();
        // GET: Login
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Login()
        {
           // Session["Result"] = null;
            //return RedirectToAction("Login");
            return View();
            
        }

        public ActionResult Logout()
        {
            Session["Result"] = null;
            return RedirectToAction("Login");
        }


        public JsonResult loginUser(Register objmodel)
        {
           // ResponseModel sessval = new ResponseModel();

            result = ILogin.loginUser(objmodel);
            Session["Result"] = result;
            return Json(result);

           
        }

    }
}