using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace AdminCylsys.DAL
{
    public class PostClass : PostandJobInterface
    {
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        ResponseModel res = new ResponseModel();


        public List<countrylist> GetCountryList()
        {
            List<countrylist> location = new List<countrylist>();
            ResponseModel res = new ResponseModel();

            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select country_id, country_name from tbl_country", con))
                {
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            countrylist u = new countrylist();
                            u.country_name = string.IsNullOrWhiteSpace(reader["country_name"].ToString()) ? "" : reader["country_name"].ToString();
                            u.country_id = string.IsNullOrEmpty(reader["country_id"].ToString()) ? 0 : Convert.ToInt32(reader["country_id"]);
                            location.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting ContryList is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return location;
        }

        public List<statelist> GetStateList(int? id)
        {
            List<statelist> state = new List<statelist>();
            ResponseModel res = new ResponseModel();

            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetStatesByCountryId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_state_id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            statelist u = new statelist();
                            u.state_name = string.IsNullOrWhiteSpace(reader["state_name"].ToString()) ? "" : reader["state_name"].ToString();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            state.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting State List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return state;
        }

        public List<citylist> GetCityList(int? id)
        {
            List<citylist> city = new List<citylist>();
            ResponseModel res = new ResponseModel();

            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetCityByStateId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_state_id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            citylist u = new citylist();
                            u.city_name = string.IsNullOrWhiteSpace(reader["city_name"].ToString()) ? "" : reader["city_name"].ToString();
                            u.city_id = string.IsNullOrEmpty(reader["city_id"].ToString()) ? 0 : Convert.ToInt32(reader["city_id"]);
                            city.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting CityList is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return city;
        }

        public List<payrolllist> GetPayrollList()
        {
            List<payrolllist> payroll = new List<payrolllist>();
            ResponseModel res = new ResponseModel();

            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select id, Payroll from tbl_payroll_master", con))
                {
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            payrolllist u = new payrolllist();
                            u.Payroll = string.IsNullOrWhiteSpace(reader["Payroll"].ToString()) ? "" : reader["Payroll"].ToString();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            payroll.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting Payroll List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return payroll;
        }

        public List<experiencelist> GetExperienceList()
        {
            List<experiencelist> experience = new List<experiencelist>();
            ResponseModel res = new ResponseModel();

            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select id, experience from tbl_experience_master", con))
                {
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            experiencelist u = new experiencelist();
                            u.experience = string.IsNullOrWhiteSpace(reader["experience"].ToString()) ? "" : reader["experience"].ToString();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            experience.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting Experience List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return experience;
        }




        public ResponseModel AddPostandJob(PostUser user)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertPostandJobData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_position", user.position);
                    cmd.Parameters.AddWithValue("@p_country_name", user.country_name);
                    cmd.Parameters.AddWithValue("@p_state_name", user.state_name);
                    cmd.Parameters.AddWithValue("@p_city_name", user.city_name);
                    cmd.Parameters.AddWithValue("@p_experience", user.experience);
                    cmd.Parameters.AddWithValue("@p_joining_period", user.joining_period);
                    cmd.Parameters.AddWithValue("@p_mail_Id", user.mail_Id);
                    cmd.Parameters.AddWithValue("@p_number_of_position", user.number_of_position);
                    cmd.Parameters.AddWithValue("@p_Payroll", user.Payroll);
                    cmd.Parameters.AddWithValue("@p_valid_till", user.valid_till);
                    cmd.Parameters.AddWithValue("@p_phone_number", user.phone_number);
                    cmd.Parameters.AddWithValue("@p_budget", user.budget);
                    cmd.Parameters.AddWithValue("@p_description", user.description);

                    var Id = cmd.ExecuteNonQuery();

                    if (Id > 0)
                    {
                        res.status = true;
                        res.Message = "Data Saved Successfully!!";
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "Oops!! Error Occurred!!";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while adding new Post&Job is:" + ex);
            }
            finally
            {
                con.Close();
            }

            return res;
        }

        public List<PostUser> getPostAndJobsList()
        {
            List<PostUser> result = new List<PostUser>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetPostAndJobData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@job_id", null);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PostUser u = new PostUser();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.position = string.IsNullOrWhiteSpace(reader["position"].ToString()) ? "" : reader["position"].ToString();
                            u.country_name = string.IsNullOrWhiteSpace(reader["country_name"].ToString()) ? "" : reader["country_name"].ToString();
                            u.state_name = string.IsNullOrWhiteSpace(reader["state_name"].ToString()) ? "" : reader["state_name"].ToString();
                            u.city_name = string.IsNullOrWhiteSpace(reader["city_name"].ToString()) ? "" : reader["city_name"].ToString();


                            u.country_id = string.IsNullOrWhiteSpace(reader["country_id"].ToString()) ? "" : reader["country_id"].ToString();
                            u.state_id = string.IsNullOrWhiteSpace(reader["state_id"].ToString()) ? "" : reader["state_id"].ToString();
                            u.city_id = string.IsNullOrWhiteSpace(reader["city_id"].ToString()) ? "" : reader["city_id"].ToString();

                            u.expid = string.IsNullOrWhiteSpace(reader["expid"].ToString()) ? "" : reader["expid"].ToString();

                            u.experience = string.IsNullOrWhiteSpace(reader["experience"].ToString()) ? "" : reader["experience"].ToString();
                            u.joining_period = string.IsNullOrWhiteSpace(reader["joining_period"].ToString()) ? "" : reader["joining_period"].ToString();
                            u.mail_Id = string.IsNullOrWhiteSpace(reader["mail_Id"].ToString()) ? "" : reader["mail_Id"].ToString();
                            u.number_of_position = string.IsNullOrEmpty(reader["number_of_position"].ToString()) ? 0 : Convert.ToInt32(reader["number_of_position"]);

                            u.pay_id = string.IsNullOrWhiteSpace(reader["pay_id"].ToString()) ? "" : reader["pay_id"].ToString();

                            u.Payroll = string.IsNullOrWhiteSpace(reader["Payroll"].ToString()) ? "" : reader["Payroll"].ToString();
                            u.valid_till = string.IsNullOrWhiteSpace(reader["valid_till"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(reader["valid_till"]);
                            u.phone_number = string.IsNullOrWhiteSpace(reader["phone_number"].ToString()) ? "" : reader["phone_number"].ToString();
                            u.budget = string.IsNullOrWhiteSpace(reader["budget"].ToString()) ? "" : reader["budget"].ToString();
                          //  u.budget = string.IsNullOrEmpty(reader["budget"].ToString()) ? 0 : Convert.ToInt32(reader["budget"]);
                            u.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            // u.Active = (byte)reader["Active"];
                            //  u.Active = Convert.ToByte(reader["Active"]);
                            u.Active = Convert.ToByte(reader["Active"]) == 1;

                            result.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting All Post&Job List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public PostUser GetPostById(int id)
        {
            ResponseModel res = new ResponseModel();
            PostUser user = new PostUser();

            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetPostAndJobData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@job_id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            user.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            user.position = string.IsNullOrWhiteSpace(reader["position"].ToString()) ? "" : reader["position"].ToString();
                            user.country_name = string.IsNullOrWhiteSpace(reader["country_name"].ToString()) ? "" : reader["country_name"].ToString();
                            user.state_name = string.IsNullOrWhiteSpace(reader["state_name"].ToString()) ? "" : reader["state_name"].ToString();
                            user.city_name = string.IsNullOrWhiteSpace(reader["city_name"].ToString()) ? "" : reader["city_name"].ToString();


                            user.country_id = string.IsNullOrWhiteSpace(reader["country_id"].ToString()) ? "" : reader["country_id"].ToString();
                            user.state_id = string.IsNullOrWhiteSpace(reader["state_id"].ToString()) ? "" : reader["state_id"].ToString();
                            user.city_id = string.IsNullOrWhiteSpace(reader["city_id"].ToString()) ? "" : reader["city_id"].ToString();

                            user.expid = string.IsNullOrWhiteSpace(reader["expid"].ToString()) ? "" : reader["expid"].ToString();

                            user.experience = string.IsNullOrWhiteSpace(reader["experience"].ToString()) ? "" : reader["experience"].ToString();
                            user.joining_period = string.IsNullOrWhiteSpace(reader["joining_period"].ToString()) ? "" : reader["joining_period"].ToString();
                            user.mail_Id = string.IsNullOrWhiteSpace(reader["mail_Id"].ToString()) ? "" : reader["mail_Id"].ToString();
                            user.number_of_position = string.IsNullOrEmpty(reader["number_of_position"].ToString()) ? 0 : Convert.ToInt32(reader["number_of_position"]);

                            user.pay_id = string.IsNullOrWhiteSpace(reader["pay_id"].ToString()) ? "" : reader["pay_id"].ToString();

                            user.Payroll = string.IsNullOrWhiteSpace(reader["Payroll"].ToString()) ? "" : reader["Payroll"].ToString();
                            user.valid_till = string.IsNullOrWhiteSpace(reader["valid_till"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) : Convert.ToDateTime(reader["valid_till"]);
                            user.phone_number = string.IsNullOrWhiteSpace(reader["phone_number"].ToString()) ? "" : reader["phone_number"].ToString();
                            user.budget = string.IsNullOrWhiteSpace(reader["budget"].ToString()) ? "" : reader["budget"].ToString();
                            //  user.budget = string.IsNullOrEmpty(reader["budget"].ToString()) ? 0 : Convert.ToInt32(reader["budget"]);
                            user.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Errorr!!!";
                Helper.WriteLog("The error while getting PostandJob by Id is:" + ex);
            }
            finally
            {
                con.Close();
            }

            return user;

        }

        public ResponseModel UpdatePostandJob(PostUser user)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_UpdatePostandJobData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id", user.id);
                    cmd.Parameters.AddWithValue("@p_position", user.position);
                    cmd.Parameters.AddWithValue("@p_country_name", user.country_name);
                    cmd.Parameters.AddWithValue("@p_state_name", user.state_name);
                    cmd.Parameters.AddWithValue("@p_city_name", user.city_name);
                    cmd.Parameters.AddWithValue("@p_experience", user.experience);
                    cmd.Parameters.AddWithValue("@p_joining_period", user.joining_period);
                    cmd.Parameters.AddWithValue("@p_mail_Id", user.mail_Id);
                    cmd.Parameters.AddWithValue("@p_number_of_position", user.number_of_position);
                    cmd.Parameters.AddWithValue("@p_Payroll", user.Payroll);
                    cmd.Parameters.AddWithValue("@p_valid_till", user.valid_till);
                    cmd.Parameters.AddWithValue("@p_phone_number", user.phone_number);
                    cmd.Parameters.AddWithValue("@p_budget", user.budget);
                    cmd.Parameters.AddWithValue("@p_description", user.description);

                    var Id = cmd.ExecuteNonQuery();

                    if (Id > 0)
                    {
                        res.status = true;
                        res.Message = "Data Updated Successfully!!";
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "Oops!! Error Occurred!!";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while Updating Post and Job is:" + ex);
            }
            finally
            {
                con.Close();
            }

            return res;
        }

        public ResponseModel DisablePost(int id, bool Active)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DisablePostData", con))
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
                Helper.WriteLog("The error while disable PostandJob is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        public List<PostUser> getActiveJobsList()
        {
            List<PostUser> result = new List<PostUser>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetActiveJobData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@job_id", null);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PostUser u = new PostUser();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.position = string.IsNullOrWhiteSpace(reader["position"].ToString()) ? "" : reader["position"].ToString();
                            u.country_name = string.IsNullOrWhiteSpace(reader["country_name"].ToString()) ? "" : reader["country_name"].ToString();
                            u.state_name = string.IsNullOrWhiteSpace(reader["state_name"].ToString()) ? "" : reader["state_name"].ToString();
                            u.city_name = string.IsNullOrWhiteSpace(reader["city_name"].ToString()) ? "" : reader["city_name"].ToString();


                            u.country_id = string.IsNullOrWhiteSpace(reader["country_id"].ToString()) ? "" : reader["country_id"].ToString();
                            u.state_id = string.IsNullOrWhiteSpace(reader["state_id"].ToString()) ? "" : reader["state_id"].ToString();
                            u.city_id = string.IsNullOrWhiteSpace(reader["city_id"].ToString()) ? "" : reader["city_id"].ToString();

                            u.experience = string.IsNullOrWhiteSpace(reader["experience"].ToString()) ? "" : reader["experience"].ToString();
                            u.joining_period = string.IsNullOrWhiteSpace(reader["joining_period"].ToString()) ? "" : reader["joining_period"].ToString();
                            u.mail_Id = string.IsNullOrWhiteSpace(reader["mail_Id"].ToString()) ? "" : reader["mail_Id"].ToString();
                            u.number_of_position = string.IsNullOrEmpty(reader["number_of_position"].ToString()) ? 0 : Convert.ToInt32(reader["number_of_position"]);
                            u.Payroll = string.IsNullOrWhiteSpace(reader["Payroll"].ToString()) ? "" : reader["Payroll"].ToString();
                            u.valid_till = string.IsNullOrWhiteSpace(reader["valid_till"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(reader["valid_till"]);
                            u.phone_number = string.IsNullOrWhiteSpace(reader["phone_number"].ToString()) ? "" : reader["phone_number"].ToString();
                            u.budget = string.IsNullOrWhiteSpace(reader["budget"].ToString()) ? "" : reader["budget"].ToString();
                            // u.budget = string.IsNullOrEmpty(reader["budget"].ToString()) ? 0 : Convert.ToInt32(reader["budget"]);
                            u.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            u.Active = Convert.ToByte(reader["Active"]) == 1;

                            result.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting Active PostJob List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }






    }
}
