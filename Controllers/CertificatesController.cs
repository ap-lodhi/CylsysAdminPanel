using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminCylsys.Controllers
{   
   public class CertificatesController : Controller
    { 
        CertificateInterface certi = new CertificateClass();
        // GET: Certificates

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllCertificates(int? id)
        {
            List<certificateModel> result = certi.getcertificateList(id);
            return View(result);
        }

        public ActionResult AddCertificates()
        {
            return View();
        }

        [HttpPost]

        public ActionResult AddCertificates(HttpPostedFileBase upload_certificate, certificateModel cer)
        {
            if (upload_certificate != null && upload_certificate.ContentLength > 0)
            {
                string imgname = Path.GetFileName(upload_certificate.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    Helper.WriteLog("this is image path for Certificate" + imgpath);
                    upload_certificate.SaveAs(imgpath);
                    ResponseModel res = new ResponseModel();
                    res = certi.Addcertificate(cer, imgname, imgpath);
                    if (res.status)
                    {
                        TempData["SuccessMessage"] = "Certificate added successfully!";
                        //   return RedirectToAction("AllClients");
                    }
                    else
                    {
                        // If the code reaches here, it means there was an issue with the file or data
                        TempData["ErrorMessage"] = "Error adding Certificate. Please check your input.";
                        return View();
                    }
                }
            }
            return View();
        }


        public ActionResult EditCertificate(int? id)
        {
            certificateModel result = certi.getcertificateList(id).FirstOrDefault();
            return View(result);
        }


      
     [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult UpdateCertificate(HttpPostedFileBase upload_certificate, certificateModel cer)

        {

            if (upload_certificate != null && upload_certificate.ContentLength > 0)

            {

                string imgname = Path.GetFileName(upload_certificate.FileName);

                string imgext = Path.GetExtension(imgname);

                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")

                {

                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);

                    upload_certificate.SaveAs(imgpath);

                    ResponseModel res = new ResponseModel();

                    res = certi.Addcertificate(cer, imgname, imgpath);

                    return Json(new { success = res.status, message = res.Message });

                }

                else

                {

                    return Json(new { success = false, message = "Invalid file format. Please upload a valid image." });

                }

            }

            else if (!string.IsNullOrEmpty(cer.upload_certificate))

            {

                // Use the value from objTeam.UploadTeamImage

                ResponseModel res = certi.Addcertificate(cer, cer.upload_certificate, cer.path); // Adjust parameters accordingly

                // Return a JSON response

                return Json(new { success = res.status, message = res.Message });

            }

            else

            {

                return Json(new { success = false, message = "Please upload an image." });

            }

        }



    }
}