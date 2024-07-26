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
public class CertificateClass : CertificateInterface
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
        public ResponseModel Addcertificate(certificateModel cer, string imgname, string imgpath)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_InsertOrUpdateCertificate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id", cer.id);
                    cmd.Parameters.AddWithValue("@p_title", cer.title);
                    cmd.Parameters.AddWithValue("@p_link", cer.link);
                    cmd.Parameters.AddWithValue("@p_upload_certificate", imgname);
                    cmd.Parameters.AddWithValue("@p_description", cer.description);
                    cmd.Parameters.AddWithValue("@p_path", imgpath);
                    //    cmd.Parameters.AddWithValue("@p_size", "null");
                    //      cmd.Parameters.AddWithValue("@p_alt_content", "null");
                    // cmd.Parameters.AddWithValue("@p_path", imgpath);
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
                Helper.WriteLog("The error for Add Certificate is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return res;
        }
        public List<certificateModel> getcertificateList(int? id)
        {
            ResponseModel res = new ResponseModel();
            List<certificateModel> result = new List<certificateModel>();
            con.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Cylsys_Sp_GetCertificateData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@certificate_id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            certificateModel u = new certificateModel();
                            u.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            u.title = string.IsNullOrWhiteSpace(reader["title"].ToString()) ? "" : reader["title"].ToString();
                            u.link = string.IsNullOrWhiteSpace(reader["link"].ToString()) ? "" : reader["link"].ToString();
                            u.description = string.IsNullOrWhiteSpace(reader["description"].ToString()) ? "" : reader["description"].ToString();
                            u.upload_certificate = string.IsNullOrWhiteSpace(reader["upload_certificate"].ToString()) ? "" : reader["upload_certificate"].ToString();
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
                Helper.WriteLog("The error for Get Certificate List is:" + ex);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

    }
}