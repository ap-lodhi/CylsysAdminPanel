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
    public class ClientController : Controller
    {
        ClientInterface cli = new ClientClass();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AllClients(int? id)
        {
            List<clientModel> result = cli.getClientList(id);
            return View(result);
        }

        public ActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddClient(HttpPostedFileBase client_Image, clientModel cm)
        {
            if (client_Image != null && client_Image.ContentLength > 0)
            {
                string imgname = Path.GetFileName(client_Image.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    client_Image.SaveAs(imgpath);
                    Helper.WriteLog("this is image path for client" + imgpath);
                    ResponseModel res = new ResponseModel();
                    res = cli.Addclient(cm, imgname, imgpath);
                    if (res.status)
                    {
                        TempData["SuccessMessage"] = "Clients added successfully!";
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
        public ActionResult EditClient(int? id)
        {
            // List<clientModel> result = cli.getClientList(id);
            clientModel result = cli.getClientList(id).FirstOrDefault();
            return View(result);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult UpdateClient(HttpPostedFileBase client_Image, clientModel cm)
        {
            if (client_Image != null && client_Image.ContentLength > 0)
            {
                string imgname = Path.GetFileName(client_Image.FileName);
                string imgext = Path.GetExtension(imgname);
                if (imgext == ".jpg" || imgext == ".png" || imgext == ".jpeg" || imgext == ".svg")
                {
                    string imgpath = Path.Combine(Server.MapPath("~/Uploads"), imgname);
                    client_Image.SaveAs(imgpath);
                    ResponseModel res = new ResponseModel();
                    res = cli.Addclient(cm, imgname, imgpath);


                    return Json(new { success = res.status, message = res.Message });

                }

                else

                {

                    return Json(new { success = false, message = "Invalid file format. Please upload a valid image." });

                }
            }

            else if (!string.IsNullOrEmpty(cm.client_Image))

            {

                // Use the value from objTeam.UploadTeamImage

                ResponseModel res = cli.Addclient(cm, cm.client_Image, cm.path); // Adjust parameters accordingly

                // Return a JSON response

                return Json(new { success = res.status, message = res.Message });

            }

            else

            {

                return Json(new { success = false, message = "Please upload an image." });

            }
        }

        public JsonResult DeleteClient(int clientId)

        {

            var result = cli.DeleteClient(clientId);

            return Json(result);

        }


    }

}