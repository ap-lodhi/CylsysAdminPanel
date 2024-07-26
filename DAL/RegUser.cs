using AdminCylsys.Interface;
using AdminCylsys.Models;
using MySqlConnector;
using System;
using System.Configuration;
using System.Data;

namespace AdminCylsys.DAL
{

    public class RegUser : LoginInterface
    {
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        public ResponseModel loginUser(Register objmodel)
        {
            ResponseModel result = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_ValidateLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_email", objmodel.Email);
                    cmd.Parameters.AddWithValue("@p_password", objmodel.Password);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        result.status = true;
                        result.Message = "Login successful.";
                    }
                    else
                    {
                        result.status = false;
                        result.Message = "Invalid email or password.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.status = false;
                result.Message = "An error occurred. Please try again later.";
                Helper.WriteLog("The error while Login is:" + ex);              
            }
            finally
            {
                con.Close();             

            }
            return result;
        }


    }
}