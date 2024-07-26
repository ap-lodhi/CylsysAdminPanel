using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AdminCylsys.DAL
{
    public class RequestQuote : RequestQuoteInterface
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        string EmailPath = ConfigurationManager.AppSettings["EmailTemplate"].ToString();
        ResponseModel res = new ResponseModel();
        public ResponseModel AddRequestQuote(RequestQuoteModel request)
        {
            ResponseModel res = new ResponseModel();
            string service_name = "";
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertQuoteData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_name", request.name);
                    cmd.Parameters.AddWithValue("@p_email", request.email);
                    cmd.Parameters.AddWithValue("@p_phoneNo", request.phoneNo);
                    cmd.Parameters.AddWithValue("@p_services", request.services);
                    cmd.Parameters.AddWithValue("@p_requirementBrief", request.requirementBrief);

                    
                    var Id = cmd.ExecuteNonQuery();

                    if (Id > 0)
                    {
                        using (MySqlCommand cmd2 = new MySqlCommand("getServiceNameFormail", con))
                        {
                            cmd2.CommandType = CommandType.StoredProcedure;

                            MySqlDataReader reader = cmd2.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                service_name = string.IsNullOrWhiteSpace(reader["serviceName"].ToString()) ? "" : reader["serviceName"].ToString();
                                }
                            }

                            // Call the method to send email
                            if (SendEmail(request,  service_name))
                            {
                                res.status = true;
                                res.Message = "Data Saved and Mail Sent Successfully!!";
                            }
                            else
                            {
                                res.status = false;
                                res.Message = "Oops!! Error Occurred while sending email!!";
                            }
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "Oops!! Error Occurred while saving data!!";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while Add Request Quote is:" + ex);
            }
            finally
            {
                con.Close();
            }

            return res;
        }
        private bool SendEmail(RequestQuoteModel request,string service_name)
        {

            string sendMail = ConfigurationManager.AppSettings["SendEmail"].ToString();
            string toMail = ConfigurationManager.AppSettings["ToEmail"].ToString();
            string Password = ConfigurationManager.AppSettings["Password"].ToString();
            try
            {
                 string templatePath = EmailPath + "emailTemplate_1.html";
                string emailBody;

                // Load the HTML template
                using (StreamReader reader = new StreamReader(templatePath))
                {
                    emailBody = reader.ReadToEnd();
                }

                // Replace placeholders in the template with actual values
                emailBody = emailBody.Replace("{name}", request.name);
                emailBody = emailBody.Replace("{email}", request.email);
                emailBody = emailBody.Replace("{phoneNo}", request.phoneNo);
                emailBody = emailBody.Replace("{services}", service_name);
                emailBody = emailBody.Replace("{requirementBrief}", request.requirementBrief);
                emailBody = emailBody.Replace("{requestDate}", request.requestDate.ToString("yyyy-MM-dd HH:mm:ss"));


                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                msg.From = new MailAddress(sendMail);
                msg.To.Add(toMail);
                msg.Subject = "Request For Enquiry";

                // Set the HTML template as the email body
                msg.Body = emailBody;
                msg.IsBodyHtml = true;

                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(sendMail, Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(msg);

                return true;
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Error occurred while sending email: " + ex);
                return false;
            }
        }
        public List<RequestQuoteModel> getRequestQuoteList()
        {
            ResponseModel res = new ResponseModel();
            List<RequestQuoteModel> result = new List<RequestQuoteModel>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetRequestQuoteData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", null);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            RequestQuoteModel u = new RequestQuoteModel();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.name = string.IsNullOrWhiteSpace(reader["name"].ToString()) ? "" : reader["name"].ToString();
                            u.email = string.IsNullOrWhiteSpace(reader["email"].ToString()) ? "" : reader["email"].ToString();
                            u.phoneNo = string.IsNullOrWhiteSpace(reader["phoneNo"].ToString()) ? "" : reader["phoneNo"].ToString();
                            u.services = string.IsNullOrWhiteSpace(reader["serviceName"].ToString()) ? "" : reader["serviceName"].ToString();
                            u.requirementBrief = string.IsNullOrWhiteSpace(reader["requirementBrief"].ToString()) ? "" : reader["requirementBrief"].ToString();
                            result.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting all Request Quote List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public List<ServiceModel> GetServiceList()

        {

            List<ServiceModel> service = new List<ServiceModel>();

            ResponseModel res = new ResponseModel();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("GetServiceName", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            ServiceModel u = new ServiceModel();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            u.serviceName = string.IsNullOrWhiteSpace(reader["serviceName"].ToString()) ? "" : reader["serviceName"].ToString();
                            service.Add(u);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while getting Service List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return service;

        }
        
        public ResponseModel DeleteRequestQuote(int RequestQuoteId)

        {

            ResponseModel res = new ResponseModel();

            try

            {

                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DeleteRequestQuoteData", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_requestQuote_id", RequestQuoteId);

                    var affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0)

                    {

                        res.status = true;

                        res.Message = "Data deleted successfully";

                    }

                    else

                    {

                        res.status = false;

                        res.Message = "Data not found or error occurred during deletion";

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error during team deletion";

                Helper.WriteLog("The error while deleting Request Quote is: " + ex);

            }

            finally

            {

                con.Close();

            }

            return res;

        }
    }
}