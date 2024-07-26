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
    public class DalTeam : IdalTeam
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        public ResponseModel AddTeam(TeamModel objteam, string imgname, string imgpath)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertTeamData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_event_name", objteam.EventName);
                    cmd.Parameters.AddWithValue("@p_upload_team_image", imgname);
                    cmd.Parameters.AddWithValue("@p_description", objteam.Description);
                    cmd.Parameters.AddWithValue("@p_path", imgpath);
                    cmd.Parameters.AddWithValue("@p_created_date", objteam.CreatedDate);

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
                Helper.WriteLog("The error while adding new Team is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        public List<TeamModel> GetTeamList()
        {
            ResponseModel res = new ResponseModel();
            List<TeamModel> result = new List<TeamModel>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetTeamData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@team_id", null);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TeamModel u = new TeamModel();
                            u.Id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.EventName = string.IsNullOrWhiteSpace(reader["event_Name"].ToString()) ? "" : reader["event_Name"].ToString();
                            u.UploadTeamImage = string.IsNullOrWhiteSpace(reader["upload_team_image"].ToString()) ? "" : reader["upload_team_image"].ToString();
                            u.Description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            u.Path = string.IsNullOrWhiteSpace(reader["path"].ToString()) ? "" : reader["path"].ToString();
                            u.CreatedDate = reader["created_date"] == DBNull.Value || reader["created_date"] == null ? (DateTime?)null : Convert.ToDateTime(reader["created_date"]);
                            result.Add(u);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while getting Team List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;

        }


        public ResponseModel DeleteTeam(int teamId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DeleteTeamData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_team_id", teamId);

                    var affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        res.status = true;
                        res.Message = "Data deleted successfully";
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "Team not found or error occurred during deletion";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error during team deletion";
                Helper.WriteLog("The error while deleting Team is: " + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }


        public ResponseModel UpdateTeamData(TeamModel objTeam, string imgname, string imgpath)
        {
            ResponseModel res = new ResponseModel();
            con.Open();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_UpdateTeamData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_team_id", objTeam.Id);
                    cmd.Parameters.AddWithValue("@p_event_name", objTeam.EventName);
                    cmd.Parameters.AddWithValue("@p_upload_team_image", imgname);
                    cmd.Parameters.AddWithValue("@p_description", objTeam.Description);
                    cmd.Parameters.AddWithValue("@p_path", imgpath);
                    cmd.Parameters.AddWithValue("@p_created_date", objTeam.CreatedDate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        res.status = true;
                        res.Message = "Team data updated successfully.";
                    }
                    else
                    {
                        res.status = false;
                        res.Message = "No rows affected. Team data update failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.Message = "Error!!!";
                Helper.WriteLog("The error while updating Team is:" + ex);
            }
            finally
            {
                con.Close();
            }

            return res;
        }

        public TeamModel GetTeamById(int id)
        {
            TeamModel team = new TeamModel();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetTeamData", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@team_id", id);
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            team.Id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            team.EventName = string.IsNullOrWhiteSpace(reader["event_Name"].ToString()) ? "" : reader["event_Name"].ToString();
                            team.UploadTeamImage = string.IsNullOrWhiteSpace(reader["upload_team_image"].ToString()) ? "" : reader["upload_team_image"].ToString();
                            team.Description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            team.Path = string.IsNullOrWhiteSpace(reader["path"].ToString()) ? "" : reader["path"].ToString();
                            team.CreatedDate = reader["created_date"] == DBNull.Value || reader["created_date"] == null ? (DateTime?)null : Convert.ToDateTime(reader["created_date"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Error while getting TeamData by Id: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return team;
        }

    }
}