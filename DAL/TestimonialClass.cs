using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
namespace AdminCylsys.DAL

{
    public class TestimonialClass : TestimonialInterface
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        ResponseModel res = new ResponseModel();

        public TestimonialModel GetTestimonialById(int id)
        {
            ResponseModel res = new ResponseModel();
            TestimonialModel user = new TestimonialModel();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetTestimonialData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@testimonial_id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            user.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            user.client_name = string.IsNullOrWhiteSpace(reader["client_name"].ToString()) ? "" : reader["client_name"].ToString();
                            user.project_name = string.IsNullOrWhiteSpace(reader["project_name"].ToString()) ? "" : reader["project_name"].ToString();

                            user.client_id = string.IsNullOrWhiteSpace(reader["client_id"].ToString()) ? "" : reader["client_id"].ToString();
                            //user.project_id = string.IsNullOrWhiteSpace(reader["project_id"].ToString()) ? "" : reader["project_id"].ToString();

                            user.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            user.rating = string.IsNullOrEmpty(reader["rating"].ToString()) ? 0 : Convert.ToInt32(reader["rating"]);
                            user.testimonial_Image = string.IsNullOrWhiteSpace(reader["website_Url"].ToString()) ? "" : reader["website_Url"].ToString();
                            user.path = string.IsNullOrWhiteSpace(reader["path"].ToString()) ? "" : reader["path"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Errorr!!!";
                Helper.WriteLog("The error while getting Testimonial by Id is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return user;
        }
        public List<TestimonialModel> getTestimonialList()
        {
            ResponseModel res = new ResponseModel();
            List<TestimonialModel> result = new List<TestimonialModel>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetTestimonialData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@testimonial_id", null);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TestimonialModel u = new TestimonialModel();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.client_name = string.IsNullOrWhiteSpace(reader["client_name"].ToString()) ? "" : reader["client_name"].ToString();

                            u.client_id = string.IsNullOrWhiteSpace(reader["client_id"].ToString()) ? "" : reader["client_id"].ToString();
                            //u.project_id = string.IsNullOrWhiteSpace(reader["project_id"].ToString()) ? "" : reader["project_id"].ToString();

                            u.testimonial_Image = string.IsNullOrWhiteSpace(reader["website_Url"].ToString()) ? "" : reader["website_Url"].ToString();
                            u.project_name = string.IsNullOrWhiteSpace(reader["project_name"].ToString()) ? "" : reader["project_name"].ToString();
                            u.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            u.rating = string.IsNullOrEmpty(reader["rating"].ToString()) ? 0 : Convert.ToInt32(reader["rating"]);
                            //  u.rating = string.IsNullOrEmpty(reader["rating"].ToString()) ? 0 : Convert.ToInt32(reader["rating"]);
                            u.path = string.IsNullOrWhiteSpace(reader["path"].ToString()) ? "" : reader["path"].ToString();
                            result.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting testimonial List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        public ResponseModel Addtestimonial(TestimonialModel tm, string imgname, string imgpath)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertTestimonial", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id", tm.id);
                    cmd.Parameters.AddWithValue("@p_client_name", tm.client_name);
                    cmd.Parameters.AddWithValue("@p_project_name", tm.project_name);
                    cmd.Parameters.AddWithValue("@p_rating", tm.rating);
                    cmd.Parameters.AddWithValue("@p_description", tm.description);
                    cmd.Parameters.AddWithValue("@p_upload_image", imgname);
                    cmd.Parameters.AddWithValue("@p_path", imgpath);
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
                Helper.WriteLog("The error while Add new Testimonial is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }
        public ResponseModel DeleteTestimonial(int testimonialId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DeleteTestimonialData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_testimonial_id", testimonialId);
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
                Helper.WriteLog("The error while deleting testimonial is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }


        public List<TestimonialModel> GetClientList()

        {

            List<TestimonialModel> client = new List<TestimonialModel>();

            ResponseModel res = new ResponseModel();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("select id, client_name from tbl_add_clients", con))

                {

                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            TestimonialModel u = new TestimonialModel();

                            u.client_name = string.IsNullOrWhiteSpace(reader["client_name"].ToString()) ? "" : reader["client_name"].ToString();

                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            client.Add(u);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while getting testimonial List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return client;

        }

        public List<TestimonialModel> GetProjectList()

        {

            List<TestimonialModel> project = new List<TestimonialModel>();

            ResponseModel res = new ResponseModel();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("select id, project_names from tbl_project_name", con))

                {

                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            TestimonialModel u = new TestimonialModel();

                            u.project_name = string.IsNullOrWhiteSpace(reader["project_names"].ToString()) ? "" : reader["project_names"].ToString();

                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            project.Add(u);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error while getting Projet List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return project;

        }
    }

}