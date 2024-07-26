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
    public class AnnouncementController : Controller
    {
        AnnouncementInterface announcement = new AnnouncementClass();
        // GET: Announcement
        public ActionResult Index()
        {
            return View();
        }
        //[HttpPost]

        public ActionResult AddAnnouncement(HttpPostedFileBase announcement_Image, AnnouncementModel am)
        {
            if (announcement_Image != null && announcement_Image.ContentLength > 0)
            {
                string imgname = Path.GetFileName(announcement_Image.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    announcement_Image.SaveAs(imgpath);
                    Helper.WriteLog("this is image path for Announcement_Image" + imgpath);
                    ResponseModel res = new ResponseModel();
                    res = announcement.AddAnnouncement(am, imgname, imgpath);
                    if (res.status)
                    {
                        TempData["SuccessMessage"] = "Announcement Details added successfully!";
                    }
                    else
                    {
                        // If the code reaches here, it means there was an issue with the file or data
                        TempData["ErrorMessage"] = "Error adding client. Please check your input.";
                        return View();
                    }
                }
            }
            return View();
        }

        public ActionResult AllAnnouncement(int? id)
        {
            List<AnnouncementModel> result = announcement.getAnnouncementList(id);
            return View(result);
        }

        public JsonResult DeleteAnnouncement(int announcementId)
        {
            var result = announcement.DeleteAnnouncement(announcementId);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAnnouncement(HttpPostedFileBase announcement_Image, AnnouncementModel am)
        {
            if (announcement_Image != null && announcement_Image.ContentLength > 0)
            {
                string imgname = Path.GetFileName(announcement_Image.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    announcement_Image.SaveAs(imgpath);
                    ResponseModel res = new ResponseModel();
                    res = announcement.AddAnnouncement(am, imgname, imgpath);
                    return Json(new { success = res.status, message = res.Message });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid file format. Please upload a valid image." });
                }
            }
            else if (!string.IsNullOrEmpty(am.announcement_image))
            {
                // Use the value from objTeam.UploadTeamImage
                ResponseModel res = announcement.AddAnnouncement(am, am.announcement_image, am.image_path); // Adjust parameters accordingly
                // Return a JSON response
                return Json(new { success = res.status, message = res.Message });
            }
            else
            {
               return Json(new { success = false, message = "Please upload an image." });
            }
        }

        public ActionResult EditAnnouncement(int? id)
        {
            AnnouncementModel result = announcement.getAnnouncementList(id).FirstOrDefault();
            return View(result);
        }

    }

}