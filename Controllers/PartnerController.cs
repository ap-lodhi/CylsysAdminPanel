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
    public class PartnerController : Controller
    {
        // GET: Partner

        PartnerInterface Ipartner = new DalPartner();
        public ActionResult Index()
        {
            List<PartnerModel> result = Ipartner.PartnerList();
            return View(result);
        }

       
        public ActionResult AddPartner()
        {
            var org = Ipartner.GetOrganizationList();
            ViewBag.orga = new SelectList(org, "id", "organization_type");
            return View();
        }

        [HttpPost]
        public JsonResult savepartner(PartnerModel pm)
        {
            var res = Ipartner.AddPartner(pm);

            return Json(res);
        }
        [HttpGet]
        public JsonResult OrgList()

        {

            List<OrganizationList> org = Ipartner.GetOrganizationList();

            return Json(org, JsonRequestBehavior.AllowGet);
             
        }


        [HttpGet]
        public JsonResult partnerList()

        {

            List<PartnerModel> result = Ipartner.PartnerList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DisablePartner(int id, bool Active)
        {
            var result = Ipartner.DisablePartner(id, Active);
            return Json(result);
        }
    }
}