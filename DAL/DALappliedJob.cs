using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace AdminCylsys.DAL
{
    public class DALappliedJob : AppliedJobInterface
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        string EmailPath = ConfigurationManager.AppSettings["EmailTemplate"].ToString();
        ResponseModel res = new ResponseModel();

        public ResponseModel appliedJobs(AppliedJobModel jobModel, string imgname, string imgpath)
        {
            ResponseModel res = new ResponseModel();
            string pos = "";
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_AppliedJobs", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id", jobModel.id);
                    cmd.Parameters.AddWithValue("@p_name", jobModel.name);
                    cmd.Parameters.AddWithValue("@p_email", jobModel.email);
                    cmd.Parameters.AddWithValue("@p_phoneNo", jobModel.phoneNo);
                    cmd.Parameters.AddWithValue("@p_position", jobModel.position);
                    cmd.Parameters.AddWithValue("@p_image_name", imgname);
                    cmd.Parameters.AddWithValue("@p_image_path", imgpath);
                    cmd.Parameters.AddWithValue("@p_requirement_Brief", jobModel.requirement_Brief);
                    // cmd.Parameters.AddWithValue("@p_size", "null");
                    // cmd.Parameters.AddWithValue("@p_alt_content", "null");
                    // cmd.Parameters.AddWithValue("@p_path", imgpath);
                    var Id = cmd.ExecuteNonQuery();
                    if (Id > 0)
                    {
                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_Position_name_for_email", con))
                        {
                            cmd2.CommandType = CommandType.StoredProcedure;
                            MySqlDataReader reader = cmd2.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    pos = string.IsNullOrWhiteSpace(reader["position"].ToString()) ? "" : reader["position"].ToString();
                                }
                            }
                            // Call the method to send email
                            if (SendEmail(jobModel, pos))
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
                Helper.WriteLog("The error for Add new Applied Post is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        private bool SendEmail(AppliedJobModel jobModel, string pos)
        {
            string sendMail = ConfigurationManager.AppSettings["SendEmail"].ToString();
            string toMail = ConfigurationManager.AppSettings["ToEmailhr"].ToString();
            string Password = ConfigurationManager.AppSettings["Password"].ToString();
            try
            {

                string templatePath = EmailPath + "EmailTemplate.html";

                string emailBody;

                // Load the HTML template

                using (StreamReader reader = new StreamReader(templatePath))

                {

                    emailBody = reader.ReadToEnd();

                }

                // Replace placeholders in the template with actual values

                emailBody = emailBody.Replace("{name}", jobModel.name);

                emailBody = emailBody.Replace("{email}", jobModel.email);

                emailBody = emailBody.Replace("{phoneNo}", jobModel.phoneNo);

                emailBody = emailBody.Replace("{position}", pos);

                emailBody = emailBody.Replace("{requirement_Brief}", jobModel.requirement_Brief);

                emailBody = emailBody.Replace("{requestDate}", jobModel.requestDate.ToString("yyyy-MM-dd HH:mm:ss"));


                MailMessage msg = new MailMessage();

                SmtpClient smtp = new SmtpClient();

                msg.From = new MailAddress(sendMail);

                msg.To.Add(toMail);

                msg.Subject = "Testing Mail For Applied Jobs";

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

                Helper.WriteLog("Error occurred while sending applied Post email: " + ex);

                return false;

            }

        }

        public List<AppliedJobModel> get_appliedPostList(int? id)

        {

            ResponseModel res = new ResponseModel();

            List<AppliedJobModel> result = new List<AppliedJobModel>();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetAppliedJobs", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            AppliedJobModel u = new AppliedJobModel();

                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            u.position_id = string.IsNullOrEmpty(reader["position_id"].ToString()) ? 0 :
                            Convert.ToInt32(reader["position_id"]);

                            u.name = string.IsNullOrWhiteSpace(reader["name"].ToString()) ? "" : reader["name"].ToString();

                            u.email = string.IsNullOrWhiteSpace(reader["email"].ToString()) ? "" : reader["email"].ToString();

                            u.phoneNo = string.IsNullOrWhiteSpace(reader["phoneNo"].ToString()) ? "" : reader["phoneNo"].ToString();

                            u.position = string.IsNullOrWhiteSpace(reader["position"].ToString()) ? "" : reader["position"].ToString();

                            u.image_name = string.IsNullOrWhiteSpace(reader["image_name"].ToString()) ? "" :
                            reader["image_name"].ToString();

                            u.image_path = string.IsNullOrWhiteSpace(reader["image_path"].ToString()) ? "" :
                            reader["image_path"].ToString();

                            u.requirement_Brief = string.IsNullOrWhiteSpace(reader["requirement_Brief"].ToString()) ? "" :
                            reader["requirement_Brief"].ToString();

                            // u.Active = (Boolean)reader["Active"];

                            result.Add(u);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while getting Applied Post List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return result;

        }


        public ResponseModel DeleteappliedPost(int postId)

        {

            ResponseModel res = new ResponseModel();

            try

            {

                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_Delete_AppliedPost", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_postId", postId);

                    var affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0)

                    {

                        res.status = true;

                        res.Message = "Data deleted successfully";

                    }

                    else

                    {

                        res.status = false;

                        res.Message = "Post not found or error occurred during deletion";

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error during post deletion";

                Helper.WriteLog("The error while Deleting Applied Post is: " + ex);

            }

            finally

            {

                con.Close();

            }

            return res;

        }


        public List<PostUser> GetPostionList()

        {

            List<PostUser> org = new List<PostUser>();

            ResponseModel res = new ResponseModel();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("sp_get_drop_down_positions", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            PostUser u = new PostUser();

                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            u.position = string.IsNullOrWhiteSpace(reader["position"].ToString()) ? "" :
                            reader["position"].ToString();

                            org.Add(u);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while getting Position List  is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return org;

        }


    }

}