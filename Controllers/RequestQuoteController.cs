using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

using System.Web.Mvc;

namespace AdminCylsys.Controllers
{
   
    public class RequestQuoteController : Controller
    {

        RequestQuoteInterface request = new RequestQuote();
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SaveRequest(RequestQuoteModel req)
        {
            return Json(request.AddRequestQuote(req));
        }


        public ActionResult AllRequestQuote()
        {
            List<RequestQuoteModel> result = request.getRequestQuoteList();
            return View(result);
        }
        [HttpGet]
        public JsonResult AllServices()
        {
            List<ServiceModel> result = request.GetServiceList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRequest()
        {
            var service = request.GetServiceList();
            ViewBag.orga = new SelectList(service, "id", "serviceName");
            return View();
        }

        
        public JsonResult DeleteRequestQuote(int RequestQuoteId)

        {

            var result = request.DeleteRequestQuote(RequestQuoteId);

            return Json(result);

        }



    }
   
}