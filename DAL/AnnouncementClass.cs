using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace AdminCylsys.DAL
{
    public class AnnouncementClass : AnnouncementInterface
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        public ResponseModel AddAnnouncement(AnnouncementModel am, string imgname, string imgpath)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertAnnouncementData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id", am.id);
                    cmd.Parameters.AddWithValue("@p_announcement_Name", am.announcement_Name);
                    cmd.Parameters.AddWithValue("@p_announcement_image", imgname);
                    cmd.Parameters.AddWithValue("@p_image_path", imgpath);
                   // cmd.Parameters.AddWithValue("@p_IsActive", am.IsActive);
                    cmd.Parameters.AddWithValue("@p_start_Date", am.start_Date);
                    cmd.Parameters.AddWithValue("@p_end_Date", am.end_Date);
                    cmd.Parameters.AddWithValue("@p_description", am.description );
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
                        Helper.WriteLog("Oops!! Error Occurred!");
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error for Add announcement is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }
        public List<AnnouncementModel> getAnnouncementList(int? id)
        {
            ResponseModel res = new ResponseModel();
            List<AnnouncementModel> result = new List<AnnouncementModel>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetAnnouncementData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@announcement_id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            AnnouncementModel u = new AnnouncementModel();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.announcement_Name = string.IsNullOrWhiteSpace(reader["announcement_Name"].ToString()) ? "" : reader["announcement_Name"].ToString();
                            u.announcement_image = string.IsNullOrWhiteSpace(reader["announcement_image"].ToString()) ? "" : reader["announcement_image"].ToString();
                            u.image_path = string.IsNullOrWhiteSpace(reader["image_path"].ToString()) ? "" : reader["image_path"].ToString();
                            string start_DateString = reader["start_Date"].ToString();
                            u.start_Date = (DateTime)(string.IsNullOrWhiteSpace(start_DateString) ? (DateTime?)null : DateTime.Parse(start_DateString));
                            string end_DateString = reader["end_Date"].ToString();
                            u.end_Date = (DateTime)(string.IsNullOrWhiteSpace(end_DateString) ? (DateTime?)null : DateTime.Parse(end_DateString));
                            u.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
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
                Helper.WriteLog("The error whle getting Announcement List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public ResponseModel DeleteAnnouncement(int announcementId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_Delete_AnnouncementData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_announcementId", announcementId);
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
                res.Message = "Error during announcement deletion";
                Helper.WriteLog("The error during announcement deletion is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }
    }
}