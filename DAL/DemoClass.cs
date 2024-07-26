using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace AdminCylsys.DAL
{

    public class DemoClass : DemoInterface

    {

        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());

        public List<DemoModel> getDemoList()

        {

            ResponseModel res = new ResponseModel();

            List<DemoModel> result = new List<DemoModel>();

            con.Open();

            try

            {

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_Get_DemoReq_Enq", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.AddWithValue("@client_id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {

                        while (reader.Read())

                        {

                            DemoModel u = new DemoModel();

                            u.demo_id = string.IsNullOrEmpty(reader["demo_id"].ToString()) ? 0 : Convert.ToInt32(reader["demo_id"]);

                            u.user_name = string.IsNullOrWhiteSpace(reader["user_name"].ToString()) ? "" : reader["user_name"].ToString();

                            u.application_id = string.IsNullOrWhiteSpace(reader["application_id"].ToString()) ? "" : reader["application_id"].ToString();

                            u.email_id = string.IsNullOrWhiteSpace(reader["email_id"].ToString()) ? "" : reader["email_id"].ToString();

                            u.mobile_no = string.IsNullOrWhiteSpace(reader["mobile_no"].ToString()) ? "" : reader["mobile_no"].ToString();

                            u.details = string.IsNullOrWhiteSpace(reader["details"].ToString()) ? "" : reader["details"].ToString();

                            u.country_id = string.IsNullOrWhiteSpace(reader["country_id"].ToString()) ? "" : reader["country_id"].ToString();

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

                Helper.WriteLog("The error while getting Demo List is:" + ex);

            }

            finally

            {

                con.Close();

            }

            return result;

        }



        public ResponseModel DeleteDemo(int demoId)

        {

            ResponseModel res = new ResponseModel();

            try

            {

                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_DeleteDemoData", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_demo_id", demoId);

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

                Helper.WriteLog("The error while deleting Demo is: " + ex);

            }

            finally

            {

                con.Close();

            }

            return res;

        }

    }
}