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

    public class DalPartner : PartnerInterface

    {

        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());

        ResponseModel res = new ResponseModel();

        public ResponseModel AddPartner(PartnerModel pm)

        {

            ResponseModel res = new ResponseModel();

            string industry = "";

            try

            {

                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertBecomOurPartner", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_First_name", pm.First_name);

                    cmd.Parameters.AddWithValue("@p_Last_name", pm.Last_name);

                    cmd.Parameters.AddWithValue("@p_Email", pm.Email);

                    cmd.Parameters.AddWithValue("@p_Organization_name", pm.Organization_name);

                    cmd.Parameters.AddWithValue("@p_Organization_type", pm.Organization_type);

                    cmd.Parameters.AddWithValue("@p_Expectations", pm.Expectations);


                    var Id = cmd.ExecuteNonQuery();

                    if (Id > 0)

                    {

                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_Industry_Name_for_Email", con))

                        {

                            cmd2.CommandType = CommandType.StoredProcedure;

                            MySqlDataReader reader = cmd2.ExecuteReader();

                            if (reader.HasRows)

                            {

                                while (reader.Read())

                                {

                                    industry = string.IsNullOrWhiteSpace(reader["Organization_type"].ToString()) ? "" :
                                    reader["Organization_type"].ToString();

                                }

                            }

                            // Call the method to send email

                            if (SendEmail(pm, industry))

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

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while Adding New Partner is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return res;

        }


        private bool SendEmail(PartnerModel pm, string industry)

        {

            string EmailPath = ConfigurationManager.AppSettings["EmailTemplate"].ToString();

            string sendMail = ConfigurationManager.AppSettings["SendEmail"].ToString();

            string toMail = ConfigurationManager.AppSettings["ToEmail"].ToString();

            string Password = ConfigurationManager.AppSettings["Password"].ToString();

            try

            {

                string templatePath = EmailPath + "partner_mailtemplate.html";

                string emailBody;

                // Load the HTML template

                using (StreamReader reader = new StreamReader(templatePath))

                {

                    emailBody = reader.ReadToEnd();

                }

                // Replace placeholders in the template with actual values

                emailBody = emailBody.Replace("{FirstName}", pm.First_name);

                emailBody = emailBody.Replace("{LastName}", pm.Last_name);

                emailBody = emailBody.Replace("{email}", pm.Email);

                emailBody = emailBody.Replace("{org_name}", pm.Organization_name);

                emailBody = emailBody.Replace("{industry}", industry);

                emailBody = emailBody.Replace("{Expectations}", pm.Expectations);

                emailBody = emailBody.Replace("{requestDate}", pm.date.ToString("yyyy-MM-dd HH:mm:ss"));


                MailMessage msg = new MailMessage();

                SmtpClient smtp = new SmtpClient();

                msg.From = new MailAddress(sendMail);

                msg.To.Add(toMail);

                msg.Subject = "Request For Partner";

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

        public List<PartnerModel> PartnerList()

        {

            List<PartnerModel> result = new List<PartnerModel>();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_sp_GetPartnerData", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@input_id", null);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            PartnerModel u = new PartnerModel();

                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            u.First_name = string.IsNullOrWhiteSpace(reader["First_name"].ToString()) ? "" :
                            reader["First_name"].ToString();

                            u.Last_name = string.IsNullOrWhiteSpace(reader["Last_name"].ToString()) ? "" :
                            reader["Last_name"].ToString();

                            u.Email = string.IsNullOrWhiteSpace(reader["Email"].ToString()) ? "" : reader["Email"].ToString();

                            u.Organization_name = string.IsNullOrWhiteSpace(reader["Organization_name"].ToString()) ? "" :
                            reader["Organization_name"].ToString();

                            u.Organization_type = string.IsNullOrWhiteSpace(reader["Organization_type"].ToString()) ? "" :
                            reader["Organization_type"].ToString();

                            u.Expectations = string.IsNullOrWhiteSpace(reader["Expectations"].ToString()) ? "" :
                            reader["Expectations"].ToString();

                            u.Active = Convert.ToByte(reader["Is_Active"]) == 1;


                            result.Add(u);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while getting Partner List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return result;

        }


        public List<OrganizationList> GetOrganizationList()

        {

            List<OrganizationList> org = new List<OrganizationList>();

            ResponseModel res = new ResponseModel();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("cylsys_sp_get_Organization_type", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            OrganizationList u = new OrganizationList();

                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            u.organization_type = string.IsNullOrWhiteSpace(reader["organization_type"].ToString()) ? "" :
                            reader["organization_type"].ToString();

                            org.Add(u);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while getting Organisation type List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return org;

        }


        public ResponseModel DisablePartner(int id, bool Active)

        {

            ResponseModel res = new ResponseModel();

            try

            {

                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DisableBecomeOurPartner", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_id", id);

                    cmd.Parameters.AddWithValue("@p_Active", Active);

                    var affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0)

                    {

                        res.status = true;

                        res.Message = "Data Disabled successfully";

                    }

                    else

                    {

                        res.status = false;

                        res.Message = "Data not found or error occurred during process";

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!";

                Helper.WriteLog("The error while disable Partner is: " + ex);

            }

            finally

            {

                con.Close();

            }

            return res;

        }

    }

}