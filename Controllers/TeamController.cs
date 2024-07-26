using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AdminCylsys.Controllers
{
    public class TeamController : Controller
    {
        IdalTeam team = new DalTeam();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetTeam()
        {
            List<TeamModel> result = team.GetTeamList();
            return View(result);
        }

        public ActionResult AddTeam(HttpPostedFileBase UploadTeamImage, TeamModel objteam)
        {
            if (UploadTeamImage != null && UploadTeamImage.ContentLength > 0)
            {
                string imgname = Path.GetFileName(UploadTeamImage.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    Helper.WriteLog("this is image path for Team" + imgpath);
                    UploadTeamImage.SaveAs(imgpath);
                    ResponseModel res = new ResponseModel();
                    res = team.AddTeam(objteam, imgname, imgpath);
                    if (res.status)
                    {
                        TempData["SuccessMessage"] = "Teams added successfully!";
                        //   return RedirectToAction("AllClients");
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


        [HttpPost]
        public JsonResult DeleteTeam(int teamId)
        {
            var result = team.DeleteTeam(teamId);
            return Json(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTeam(HttpPostedFileBase UploadTeamImage, TeamModel objTeam)
        {
            if (UploadTeamImage != null && UploadTeamImage.ContentLength > 0)
            {
                string imgname = Path.GetFileName(UploadTeamImage.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    UploadTeamImage.SaveAs(imgpath);
                    ResponseModel res = team.UpdateTeamData(objTeam, imgname, imgpath);
                    // Return a JSON response
                    return Json(new { success = res.status, message = res.Message });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid file format. Please upload a valid image." });
                }
            }
            else if (!string.IsNullOrEmpty(objTeam.UploadTeamImage))
            {
                // Use the value from objTeam.UploadTeamImage
                ResponseModel res = team.UpdateTeamData(objTeam, objTeam.UploadTeamImage, objTeam.Path); // Adjust parameters accordingly
                // Return a JSON response
                return Json(new { success = res.status, message = res.Message });
            }
            else
            {
                return Json(new { success = false, message = "Please upload an image." });
            }
        }

        public ActionResult Edit(int id)
        {
            TeamModel teams = team.GetTeamById(id);
            return View(teams);
        }



    }
}