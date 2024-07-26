using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace AdminCylsys.DAL

{

    public class CaseStudiesEdit : CaseStudiesEdit_Inter

    {



        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        public ResponseModel AddCaseStudies(CaseStudiesEdit_Mod cm, string fileName, string filePath)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertCaseStudyData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_case_studies_Name", cm.case_studies_Name);
                    cmd.Parameters.AddWithValue("@p_description", cm.description);
                    cmd.Parameters.AddWithValue("@p_upload_case_study", fileName);
                    cmd.Parameters.AddWithValue("@p_path", filePath);
                    cmd.Parameters.AddWithValue("@p_size ", cm.size);
                    cmd.Parameters.AddWithValue("@p_created_by", cm.created_by);
                    cmd.Parameters.AddWithValue("@p_modified_by", cm.modified_by);
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
                Helper.WriteLog("The error for Add CaseStudy is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        public List<CaseStudiesEdit_Mod> GetCaseStudies()
        {
            ResponseModel res = new ResponseModel();
            List<CaseStudiesEdit_Mod> result = new List<CaseStudiesEdit_Mod>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetCaseStudyData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@case_study_id", null);

                    using (MySqlDataReader reader = cmd.ExecuteReader())

                    {

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {

                                CaseStudiesEdit_Mod u = new CaseStudiesEdit_Mod();

                                u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                                u.case_studies_Name = string.IsNullOrWhiteSpace(reader["case_studies_Name"].ToString()) ? "" : reader["case_studies_Name"].ToString();

                                u.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();

                                u.path = string.IsNullOrWhiteSpace(reader["path"].ToString()) ? "" : reader["path"].ToString();

                                u.upload_case_study = string.IsNullOrWhiteSpace(reader["upload_case_study"].ToString()) ? "" : reader["upload_case_study"].ToString();
                                u.Active = Convert.ToByte(reader["Active"]) == 1;

                                result.Add(u);

                            }

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";

                Helper.WriteLog("The error for Get All CaseStudy is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return result;

        }

        //--------------------------------------------

        public CaseStudiesEdit_Mod GetCaseStudyById(int id)

        {

            ResponseModel res = new ResponseModel();

            CaseStudiesEdit_Mod user = new CaseStudiesEdit_Mod();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("GetCaseStudyById", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            user.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);

                            user.case_studies_Name = string.IsNullOrWhiteSpace(reader["case_studies_Name"].ToString()) ? "" : reader["case_studies_Name"].ToString();

                            user.upload_case_study = string.IsNullOrWhiteSpace(reader["upload_case_study"].ToString()) ? "" : reader["upload_case_study"].ToString();

                            user.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                res.status = false;

                res.Message = "Error!!!";
                Helper.WriteLog("The error for Edit CaseStudy by Id is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return user;

        }

        public ResponseModel UpdateCaseStudy(CaseStudiesEdit_Mod cm, string fileName, string imgpath)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("UpdateCaseStudy", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id", cm.id);
                    cmd.Parameters.AddWithValue("@p_case_studies_Name", cm.case_studies_Name);
                    cmd.Parameters.AddWithValue("@p_upload_case_study", fileName);
                    cmd.Parameters.AddWithValue("@p_description", cm.description);
                    cmd.Parameters.AddWithValue("@p_path", imgpath);  // Corrected parameter name

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        res.status = true;
                        res.Message = "Data Updated Successfully!";
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "No records updated. Case study not found or no changes made.";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error updating case study!";
                Helper.WriteLog("The error for Update caseStudy is:" + ex);
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
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DisableCaseStudies", con))
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
                Helper.WriteLog("The error for Disable CaseStudy is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }




    }

}