using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AdminCylsys.Controllers

{
    public class TestimonialController : Controller
    {
        TestimonialInterface test = new TestimonialClass();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTestimonial(HttpPostedFileBase testimonial_Image, TestimonialModel tm)
        {
            var client_name = test.GetClientList();
            ViewBag.cli = new SelectList(client_name, "id", "client_name");
            var project_name = test.GetProjectList();
            ViewBag.pro = new SelectList(project_name, "id", "project_name");

            if (testimonial_Image != null && testimonial_Image.ContentLength > 0)
            {
                string imgname = Path.GetFileName(testimonial_Image.FileName);
                string imgext = Path.GetExtension(imgname).ToUpper(); 
                string[] validExtensions = { ".JPG", ".PNG", ".JPEG", ".SVG" }; 

                if (validExtensions.Contains(imgext)) 
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                //    Helper.WriteLog("this is image path for TestiMonial" + imgpath);
                    testimonial_Image.SaveAs(imgpath);
                    ResponseModel res = test.Addtestimonial(tm, imgname, imgpath);
                    return Json(new { success = res.status, message = res.Message });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid file format. Please upload a valid image." });
                }
            }
            else if (!string.IsNullOrEmpty(tm.testimonial_Image))
            {
           
                ResponseModel res = test.Addtestimonial(tm, tm.testimonial_Image, tm.path); 
                                                                                            
                return Json(new { success = res.status, message = res.Message });
            }
            else
            {
                return Json(new { success = false, message = "Please upload an image." });
            }
        }


        public ActionResult AllTestimonial()
        {
            List<TestimonialModel> result = test.getTestimonialList();
            return View(result);
        }

        public ActionResult GetTestimonialById(int id)
        {
            var client_name = test.GetClientList();
            ViewBag.cli = new SelectList(client_name, "id", "client_name");
            //var project_name = test.GetProjectList();
            //ViewBag.pro = new SelectList(project_name, "id", "project_name");
            var result = test.GetTestimonialById(id);
            return View(result);
        }

        public ActionResult AddTestimonial(HttpPostedFileBase testimonial_Image, TestimonialModel tm)
        {
            var client_name = test.GetClientList();
            ViewBag.cli = new SelectList(client_name, "id", "client_name");

            if (testimonial_Image != null && testimonial_Image.ContentLength > 0)
            {
                string imgname = Path.GetFileName(testimonial_Image.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    testimonial_Image.SaveAs(imgpath);
                    ResponseModel res = new ResponseModel();
                    res = test.Addtestimonial(tm, imgname, imgpath);
                    if (res.status)
                    {
                        TempData["SuccessMessage"] = "Testimonial added successfully!";
                        //   return RedirectToAction("AllClients");
                    }
                    else
                    {
                       
                        TempData["ErrorMessage"] = "Error adding testimonial. Please check your input.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid image format.";
                }
            }


            return View();
        }



        public JsonResult DeleteTestimonial(int testimonialId)
        {
            var result = test.DeleteTestimonial(testimonialId);
            return Json(result);
        }

    }

}