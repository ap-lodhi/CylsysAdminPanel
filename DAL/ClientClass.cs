using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace AdminCylsys.DAL
{
    public class ClientClass : ClientInterface
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        public ResponseModel DeleteClient(int clientId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DeleteClientData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_client_id", clientId);
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
                Helper.WriteLog("The error for Delete Client is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        public ResponseModel Addclient(clientModel cm, string imgname, string imgpath)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertClientData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id", cm.id);
                    cmd.Parameters.AddWithValue("@p_client_name", cm.client_name);
                    cmd.Parameters.AddWithValue("@p_website_url", cm.website_url);
                    cmd.Parameters.AddWithValue("@p_client_image", imgname);
                    cmd.Parameters.AddWithValue("@p_description", cm.description);
                    cmd.Parameters.AddWithValue("@p_path", imgpath);
                    //    cmd.Parameters.AddWithValue("@p_size", "null");
                    //      cmd.Parameters.AddWithValue("@p_alt_content", "null");
                    // cmd.Parameters.AddWithValue("@p_path", imgpath);
                    var Id = cmd.ExecuteNonQuery();
                    if (Id > 0)
                    {
                        res.status = true;
                        res.Message = "Data Saved Successfully!!";
                        Helper.WriteLog("Data Saved Successfully");
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "Oops!! Error Occurred!!";
                        Helper.WriteLog("Oops!! Error Occurred!");
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error for Add Client is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }
        public List<clientModel> getClientList(int? id)
        {
            ResponseModel res = new ResponseModel();
            List<clientModel> result = new List<clientModel>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetClientData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@client_id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clientModel u = new clientModel();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.client_name = string.IsNullOrWhiteSpace(reader["client_name"].ToString()) ? "" : reader["client_name"].ToString();
                            u.client_Image = string.IsNullOrWhiteSpace(reader["client_Image"].ToString()) ? "" : reader["client_Image"].ToString();
                            u.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            u.website_url = string.IsNullOrWhiteSpace(reader["website_url"].ToString()) ? "" : reader["website_url"].ToString();
                            u.path = string.IsNullOrWhiteSpace(reader["path"].ToString()) ? "" : reader["path"].ToString();
                            //  u.Active = (Boolean)reader["Active"];
                            result.Add(u);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error for Get All Client List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }


    }
}