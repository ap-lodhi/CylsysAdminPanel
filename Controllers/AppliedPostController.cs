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

    public class AppliedPostController : Controller

    {

        AppliedJobInterface jobInterface = new DALappliedJob();

        // GET: AppliedPost

        public ActionResult AllPost(int? id)

        {

            List<AppliedJobModel> result = jobInterface.get_appliedPostList(id);

            return View(result);

        }


        public ActionResult AddJob()

        {

            var position = jobInterface.GetPostionList();

            ViewBag.position = new SelectList(position, "id", "position");

            return View();

        }

        [HttpPost]

        public JsonResult appliedJobs(HttpPostedFileBase image_name, AppliedJobModel jobModel)

        {

            if (image_name != null && image_name.ContentLength > 0)

            {

                string imgname = Path.GetFileName(image_name.FileName);

                string imgext = Path.GetExtension(imgname);


                // Validate that the file is either a Word or PDF document

                if (imgext.Equals(".doc", StringComparison.OrdinalIgnoreCase) ||

                    imgext.Equals(".docx", StringComparison.OrdinalIgnoreCase) ||

                    imgext.Equals(".pdf", StringComparison.OrdinalIgnoreCase))

                {

                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);

                    image_name.SaveAs(imgpath);

                    // Assuming jobInterface is a service or repository to interact with the database

                    ResponseModel res = jobInterface.appliedJobs(jobModel, imgname, imgpath);

                    if (res.status)

                    {

                        TempData["SuccessMessage"] = "Job application submitted successfully!";

                        return Json(new { success = true });

                    }

                    else

                    {

                        TempData["ErrorMessage"] = "Error submitting job application. Please check your input.";

                        return Json(new { success = false, message = "Error submitting job application. Please check your input." });

                    }

                }

            }

            TempData["ErrorMessage"] = "Invalid file format or missing file.";

            return Json(new { success = false, message = "Invalid file format or missing file." });


        }


        [HttpPost]

        public JsonResult DeleteappliedPost(int postId)

        {

            var result = jobInterface.DeleteappliedPost(postId);

            return Json(result);

        }


        // public ActionResult AllAppliedPost(int? id)

        // {

        //List<AppliedJobModel> result = jobInterface.get_appliedPostList(id);

        //return View(result);

        //    return View();

        //

        //}

    }

}