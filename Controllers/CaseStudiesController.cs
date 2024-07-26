using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
namespace AdminCylsys.Controllers

{
    public class CaseStudiesController : Controller
    {
        // GET: CaseStudies


        CaseStudiesEdit_Inter rs = new CaseStudiesEdit();
        //CaseStudiesEdit_Inter CSDal = new CaseStudiesEdit();

        public ActionResult CaseStudies()
        {
            List<CaseStudiesEdit_Mod> result = rs.GetCaseStudies();
            return View(result);
        }

        public ActionResult AddCaseStudies(HttpPostedFileBase upload_case_study, CaseStudiesEdit_Mod cm)
        {
            if (upload_case_study != null && upload_case_study.ContentLength > 0)
            {
                string fileName = Path.GetFileName(upload_case_study.FileName);
                string fileExtension = Path.GetExtension(fileName);
                // You can add additional logic here to validate file types if needed
                string filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                Helper.WriteLog("this is image path for case study" + filePath);
                upload_case_study.SaveAs(filePath);
                ResponseModel res = new ResponseModel();
                res = rs.AddCaseStudies(cm, fileName, filePath);
                if (res.status)
                {
                    TempData["SuccessMessage"] = "CaseStudy added successfully!";
                    //   return RedirectToAction("AllClients");
                }
                else
                {
                    // If the code reaches here, it means there was an issue with the file or data
                    TempData["ErrorMessage"] = "Error adding client. Please check your input.";
                    return View();
                }

            }
            return View();
        }

        public ActionResult GetCaseStudyById(int id)
        {
            var result = rs.GetCaseStudyById(id);
            return View(result);
        }


        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult UpdateCaseStudy(HttpPostedFileBase upload_case_study, CaseStudiesEdit_Mod cm)
        {
            try
            {
                string fileName = null;
                string imgpath = null;
                // Check if a new file is uploaded
                if (upload_case_study != null && upload_case_study.ContentLength > 0)
                {
                    fileName = Path.GetFileName(upload_case_study.FileName);
                    string fileExtension = Path.GetExtension(fileName);
                    if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg" || fileExtension == ".svg" || fileExtension == ".pdf")
                    {
                        imgpath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        upload_case_study.SaveAs(imgpath);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid file format. Please upload a valid image." });
                    }
                }
                // Check if UploadCaseStudy is not modified, use the existing file path
                if (string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(cm.upload_case_study))
                {
                    fileName = cm.upload_case_study;
                    imgpath = cm.path;
                    // Update the file name to ensure it is displayed correctly
                    cm.upload_case_study = fileName;
                }
                ResponseModel res = rs.UpdateCaseStudy(cm, fileName, imgpath);
                return Json(new { success = res.status, message = res.Message });
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Error in UpdateCaseStudy action: " + ex.Message);
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }

        }

        //public JsonResult DeleteCase(int caseId)

        //{

        //    var result = rs.DeleteCase(caseId);

        //    return Json(result);

        //}


        public JsonResult DisablePost(int id, bool Active)
        {
            var result = rs.DisablePost(id, Active);
            return Json(result);
        }


    }

}