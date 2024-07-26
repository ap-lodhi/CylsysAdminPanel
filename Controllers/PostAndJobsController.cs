using AdminCylsys.DAL;
using AdminCylsys.Interface;
using AdminCylsys.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace AdminCylsys.Controllers
{
    public class PostAndJobsController : Controller
    {
        PostandJobInterface IPost = new PostClass();
        // GET: PostAndJobs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostAndJobs()
        {
            List<PostUser> result = IPost.getPostAndJobsList();
            return View(result);
        }

        public ActionResult ApplyPost(int? id)
        {
            var country = IPost.GetCountryList();
            ViewBag.country = new SelectList(country, "country_id", "country_name");

            var state = IPost.GetStateList(id);
            // ViewBag.state = new SelectList(state, "id", "state_name");
            ViewBag.state = new SelectList("");

            // var city = IPost.GetCityList(id);
            // ViewBag.city = new SelectList(city, "city_id", "city_name");
            ViewBag.city = new SelectList("");

            var payroll = IPost.GetPayrollList();
            ViewBag.pay = new SelectList(payroll, "id", "payroll");

            var experience = IPost.GetExperienceList();
            ViewBag.exp = new SelectList(experience, "id", "experience");

            return View();
        }

        public JsonResult SavePost(PostUser user)
        {
            return Json(IPost.AddPostandJob(user));
        }


        public JsonResult statefilter(int id)
        {
            var state = IPost.GetStateList(id);
            return Json(state);
        }

        public JsonResult cityfilter(int id)
        {
            var city = IPost.GetCityList(id);
            return Json(city);
        }

        public ActionResult GetPostById(int id)
        {

            var result = IPost.GetPostById(id);

            var country = IPost.GetCountryList();
            ViewBag.country = new SelectList(country, "country_id", "country_name");

            var state = IPost.GetStateList(Convert.ToInt32(result.country_id));
            ViewBag.state = new SelectList(state, "id", "state_name");

            var city = IPost.GetCityList(Convert.ToInt32(result.state_id));
            ViewBag.city = new SelectList(city, "city_id", "city_name");

            var payroll = IPost.GetPayrollList();
            ViewBag.pay = new SelectList(payroll, "id", "payroll");

            var experience = IPost.GetExperienceList();
            ViewBag.exp = new SelectList(experience, "id", "experience");

            return View(result);
        }

        public JsonResult UpdatePost(PostUser user)
        {
            return Json(IPost.UpdatePostandJob(user));
            //return Json(user);
        }

        public JsonResult DisablePost(int id, bool Active)
        {
            var result = IPost.DisablePost(id, Active);
            return Json(result);
        }

        public ActionResult DownloadRowPDF(int id)
        {
            var rowData = IPost.GetPostById(id);
            byte[] pdfBytes = GeneratePDF(rowData);

            return File(pdfBytes, "application/pdf", $"RowPDF_{id}.pdf");
        }

        //private byte[] GeneratePDF(PostUser rowData)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (Document document = new Document())
        //        {
        //            PdfWriter writer = PdfWriter.GetInstance(document, ms);
        //            document.Open();

        //            document.Add(new Paragraph($"Position: {rowData.position}"));
        //            document.Add(new Paragraph($"Country: {rowData.country_name}"));
        //            document.Add(new Paragraph($"State: {rowData.state_name}"));
        //            document.Add(new Paragraph($"City: {rowData.city_name}"));
        //            document.Add(new Paragraph($"Experience: {rowData.experience}"));
        //            document.Add(new Paragraph($"Joining Period: {rowData.joining_period}"));
        //            document.Add(new Paragraph($"Mail Id: {rowData.mail_Id}"));
        //            document.Add(new Paragraph($"Number of Positions: {rowData.number_of_position}"));
        //            document.Add(new Paragraph($"Payroll: {rowData.Payroll}"));
        //            document.Add(new Paragraph($"Valid Till: {rowData.valid_till:dd/MM/yyyy}"));
        //            document.Add(new Paragraph($"Phone Number: {rowData.phone_number}"));
        //            document.Add(new Paragraph($"Budget: {rowData.budget}"));
        //            document.Add(new Paragraph($"Description: {rowData.description}"));


        //            document.Close();
        //        }

        //        return ms.ToArray();
        //    }
        //}



        private byte[] GeneratePDF(PostUser rowData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    // Add a title above the table
                    Paragraph title = new Paragraph("Post and Job Deatils");
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    // Add some spacing between title and table
                    document.Add(new Paragraph("\n"));

                    // Create a table with 2 columns
                    PdfPTable table = new PdfPTable(2);
                    table.DefaultCell.Border = Rectangle.BOX; // Set border for the cells

                    AddCellToTable(table, "Position:", rowData.position);
                    AddCellToTable(table, "Country:", rowData.country_name);
                    AddCellToTable(table, "State:", rowData.state_name);
                    AddCellToTable(table, "City:", rowData.city_name);
                    AddCellToTable(table, "Experience:", rowData.experience.ToString());
                    AddCellToTable(table, "Joining Period:", rowData.joining_period);
                    AddCellToTable(table, "Mail Id:", rowData.mail_Id);
                    AddCellToTable(table, "Number of Positions:", rowData.number_of_position.ToString());
                    AddCellToTable(table, "Payroll:", rowData.Payroll.ToString());
                    AddCellToTable(table, "Valid Till:", rowData.valid_till.ToString("dd/MM/yyyy"));
                    AddCellToTable(table, "Phone Number:", rowData.phone_number);
                    AddCellToTable(table, "Budget:", rowData.budget.ToString());
                    AddCellToTable(table, "Description:", rowData.description);

                    document.Add(table);
                    document.Close();
                }

                return ms.ToArray();
            }
        }

        private void AddCellToTable(PdfPTable table, string label, string value)
        {
            PdfPCell cellLabel = new PdfPCell(new Phrase(label));
            PdfPCell cellValue = new PdfPCell(new Phrase(value));

            // Set padding, border, etc. as needed
            cellLabel.Padding = 5f;
            cellLabel.Border = Rectangle.BOX;
            cellValue.Padding = 5f;
            cellValue.Border = Rectangle.BOX;

            table.AddCell(cellLabel);
            table.AddCell(cellValue);
        }





        // API for get Active Job!!
        [HttpGet]
        public JsonResult activeJobs()
        {
            List<PostUser> result = IPost.getActiveJobsList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }






    }
}