using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace AdminCylsys.Controllers
{
    public class PressController : Controller
    {

        //  PressInterface Ipress = new PressDal();
        PressDal PD = new PressDal();


        //-------------------------------Get All Press Details
        public ActionResult GetAllPress()
        {
            List<PressModel> result = PD.GetAllPress();
            return View(result);
        }


        //---------------------------- send email with HTML template
        public void SendEmailWithTemplate(string from, string password, string to, string subject, string viewName, object model)
        {
            string body = RenderRazorViewToString(viewName, model);

            SendEmail(from, password, to, subject, body);
        }

        //--------------------------------------Add Press Details
        public ActionResult AddPressDetails(HttpPostedFileBase FileName, PressModel pressModel)
        {
            if (FileName != null && FileName.ContentLength > 0)
            {
                string fileName = Path.GetFileName(FileName.FileName);
                string fileExtension = Path.GetExtension(fileName);
                string filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);

                FileName.SaveAs(filePath);

                ResponseModel res = new ResponseModel();
                res = PD.AddPressDetails(pressModel, filePath, fileName);

                if (res.status)
                {
                    // Press details added successfully, send email with HTML template
                    SendEmailWithTemplate("waleed.kapade@cylsys.com", "Cylsys@2", "waleed.  kapade@cylsys.com", "Press Details Added", "PressDetailsTemplate", pressModel);

                    return RedirectToAction("GetAllPress");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding Press Details. Please check your input.";
                    return View();
                }
            }
            return View();
        }


        //-------------------------------- render Razor view to string
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }

        //----------------------------------method to send email
        public void SendEmail(string from, string password, string to, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient("smtp.outlook.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential(from, password);
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }
            }
        }
        //-------------------------Disable Press
        public JsonResult DisablePress(int id, bool Active)
        {
            var result = PD.DisablePress(id, Active);
            return Json(result);
        }

        //--------------------Get Post By  Id
        public ActionResult GetPressById(int id)
        {
            var result = PD.GetPressById(id);
            return View(result);
        }

        ///----------------------------------UPDATE Press Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePress(HttpPostedFileBase FileName, PressModel pressModel)
        {
            if (FileName != null && FileName.ContentLength > 0)
            {
                string imgname = Path.GetFileName(FileName.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    Helper.WriteLog("this is image path for Press" + imgpath);
                    FileName.SaveAs(imgpath);
                    ResponseModel res = new ResponseModel();
                    res = PD.UpdatePress(pressModel, imgname, imgpath);
                    return Json(new { success = res.status, message = res.Message });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid file format. Please upload a valid image." });
                }
            }
            else if (!string.IsNullOrEmpty(pressModel.FileName))
            {
                ResponseModel res = PD.UpdatePress(pressModel, pressModel.FileName, pressModel.ImagePath);
                return Json(new { success = res.status, message = res.Message });
            }
            else
            {
                return Json(new { success = false, message = "Please upload an image." });
            }
        }

    


        //--------------------------------------------------Enable Press 
        public JsonResult EnablePress(int id)
        {
            var result = PD.EnablePress(id);
            return Json(result);
        }


        //-------------------------------Get All Press Details API
        //[HttpGet]
        //public ActionResult PressList()
        //{
        //    List<PressModel> result = PD.PressList();
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}

        //-------------------------------Get All Press Details API
        [HttpGet]
        public ActionResult PressList()
        {
            List<PressModel> result = PD.PressList();
            var formattedResult = result.Select(p => new
            {
                p.Id,
                p.ImagePath,
                p.Heading,
                p.Url,
                p.FileName,
                CreatedDate = p.CreatedDate.ToString("dd/MM/yyyy"),
                p.IsActive
            }).ToList();

            return Json(formattedResult, JsonRequestBehavior.AllowGet);
        }
    }
}