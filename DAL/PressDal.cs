using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace AdminCylsys.DAL
{
    public class PressDal //: PressInterface
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());

        ///------------------------------------------------------------------Insert Press Details
        public ResponseModel AddPressDetails(PressModel pressModel, string filePath, string fileName)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertPress", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_FileName", fileName);


                    cmd.Parameters.AddWithValue("@p_ImagePath", filePath);

                    cmd.Parameters.AddWithValue("@p_Heading", pressModel.Heading);


                    cmd.Parameters.AddWithValue("@p_Url", pressModel.Url);

                    cmd.Parameters.AddWithValue("@p_IsActive", null);

                    //cmd.Parameters.AddWithValue("@p_modified_by", cm.modified_by);

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
                Helper.WriteLog("The error for Add new Press Details is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }


        //----------------------------------------------------------------------------Get All Press Details
        public List<PressModel> GetAllPress()
        {
            ResponseModel res = new ResponseModel();
            List<PressModel> result = new List<PressModel>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetAllPressDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", null);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PressModel u = new PressModel();
                                u.Id = string.IsNullOrEmpty(reader["Id"].ToString()) ? 0 : Convert.ToInt32(reader["Id"]);
                                u.Heading = string.IsNullOrWhiteSpace(reader["Heading"].ToString()) ? "" : reader["Heading"].ToString();
                                u.Url = string.IsNullOrWhiteSpace(reader["Url"].ToString()) ? "" : reader["Url"].ToString();
                                u.FileName = string.IsNullOrWhiteSpace(reader["FileName"].ToString()) ? "" : reader["FileName"].ToString();
                                u.CreatedDate = string.IsNullOrWhiteSpace(reader["CreatedDate"].ToString()) ? DateTime.MinValue : DateTime.Parse(reader["CreatedDate"].ToString());
                                bool isActive;
                                u.IsActive = bool.TryParse(reader["IsActive"].ToString(), out isActive) && isActive;
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
                Helper.WriteLog("The error for getting All Press List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        //------------------------------------------------------------------------------Disable Press
        public ResponseModel DisablePress(int id, bool Active)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DisablePressData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_Id", id);  // Adjust parameter name to match the stored procedure
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
                Helper.WriteLog("The error for disable Press is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }


        //-----------------------GET PRESS BY ID
        public PressModel GetPressById(int id)
        {
            ResponseModel res = new ResponseModel();
            PressModel user = new PressModel();

            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetPressDetailsById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_PressId", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            user.Heading = string.IsNullOrWhiteSpace(reader["Heading"].ToString()) ? "" : reader["Heading"].ToString();
                            user.Url = string.IsNullOrWhiteSpace(reader["Url"].ToString()) ? "" : reader["Url"].ToString();
                            user.FileName = string.IsNullOrWhiteSpace(reader["FileName"].ToString()) ? "" : reader["FileName"].ToString();
                            user.ImagePath = string.IsNullOrWhiteSpace(reader["ImagePath"].ToString()) ? "" : reader["ImagePath"].ToString();
                            //  user.IsActive = string.IsNullOrWhiteSpace(reader["state_name"].ToString()) ? "" : reader["state_name"].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Errorr!!!";
                Helper.WriteLog("The error for getting Press by Id is:" + ex);
            }
            finally
            {
                con.Close();
            }

            return user;
        }

        //----------------------------------------------------------UPDATE PRESS
        public ResponseModel UpdatePress(PressModel pressModel, string filePath, string fileName)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_UpdatePressDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_PressId", pressModel.Id);
                    cmd.Parameters.AddWithValue("@p_FileName", fileName);

                    cmd.Parameters.AddWithValue("@p_ImagePath", filePath);
                    cmd.Parameters.AddWithValue("@p_Heading", pressModel.Heading);
                    cmd.Parameters.AddWithValue("@p_Url", pressModel.Url);


                    var affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        res.status = true;
                        res.Message = "Press Data updated successfully";
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "Press Data not found or error occurred during the update process";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!";
                Helper.WriteLog("The error while Updating Press is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        //-------------------------------------------------Enable PRess
        public ResponseModel EnablePress(int id)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_EnablePressData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_Id", id);
                    var affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        res.status = true;
                        res.Message = "Data Enabled successfully";
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "Data not found or error occurred during the process";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!";
                Helper.WriteLog("The error for Enable Press is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }



        //------------------------------Get All Press API

        public List<PressModel> PressList()

        {

            ResponseModel res = new ResponseModel();

            List<PressModel> result = new List<PressModel>();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetAllPressDetails", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", null);

                    using (MySqlDataReader reader = cmd.ExecuteReader())

                    {

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {

                                PressModel u = new PressModel();

                                u.Id = string.IsNullOrEmpty(reader["Id"].ToString()) ? 0 : Convert.ToInt32(reader["Id"]);

                                u.Heading = string.IsNullOrWhiteSpace(reader["Heading"].ToString()) ? "" : reader["Heading"].ToString();

                                u.Url = string.IsNullOrWhiteSpace(reader["Url"].ToString()) ? "" : reader["Url"].ToString();

                                u.FileName = string.IsNullOrWhiteSpace(reader["FileName"].ToString()) ? "" : reader["FileName"].ToString();

                                u.ImagePath = string.IsNullOrWhiteSpace(reader["ImagePath"].ToString()) ? "" : reader["ImagePath"].ToString();

                                u.CreatedDate = string.IsNullOrWhiteSpace(reader["CreatedDate"].ToString()) ? DateTime.MinValue : DateTime.Parse(reader["CreatedDate"].ToString());

                                bool isActive;

                                u.IsActive = bool.TryParse(reader["IsActive"].ToString(), out isActive) && isActive;

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

                Helper.WriteLog("The error while getting ALL Press List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return result;

        }






    }






}