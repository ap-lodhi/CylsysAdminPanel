using System;
using System.Configuration;
using System.IO;

namespace AdminCylsys
{
    public class Helper
    {
        public static void WriteLog(string message)
        {
            string errorLogDir = ConfigurationManager.AppSettings["errorlog"].ToString();

            if (!Directory.Exists(errorLogDir))
                Directory.CreateDirectory(errorLogDir);

            string logFilePath = Path.Combine(errorLogDir, "errorlog" + DateTime.Now.ToString("dd-MMM-yy") + ".txt");

            using (StreamWriter sr = new StreamWriter(logFilePath, true))
            {
                sr.WriteLine(DateTime.Now.ToString("DD-MM-yyyy HH-mm-ss") + "\t" + message);
            }
        }
    }

}