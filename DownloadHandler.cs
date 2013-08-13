using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.IO;

namespace SimpleDownloadCounter
{
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileName = context.Request.QueryString["filename"].ToString();
            string filePath = context.Request.PhysicalApplicationPath + "\\files\\";
            //string filePath = "C:\\Users\\Rafael\\Downloads\\";
            FileInfo file = new System.IO.FileInfo(filePath + fileName);
            if (file.Exists)
            {
                try
                {
                    CountDownload(context.Request.ServerVariables, fileName);
                }
                catch (Exception)
                {
                    
                }
                //return file
                context.Response.Clear();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                context.Response.ContentType = "application/octet-stream";
                context.Response.WriteFile(file.FullName);
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
            }
        }
        public bool IsReusable
        {
            get { return true; }
        }



        public String CountDownload(NameValueCollection ServerVariables, String fileName)
        {
            int contador = 0;
            StringBuilder sql = new StringBuilder();

            sql.Append("INSERT INTO tb_downloads  VALUES ( ");
            sql.Append("NULL, ");
            sql.Append("NOW(), ");
            sql.Append("'" + fileName + "', ");
            sql.Append("'" + ServerVariables["REMOTE_ADDR"] + "', ");
            sql.Append("'" + ServerVariables["REMOTE_HOST"] + "', ");
            sql.Append("'" + ServerVariables["HTTP_ACCEPT_LANGUAGE"] + "', ");
            sql.Append("'" + ServerVariables["HTTP_HOST"] + "', ");
            sql.Append("'" + ServerVariables["HTTP_USER_AGENT"] + "' ");
            sql.Append(")");

            if (DataBase.ExecuteNonQuery(sql.ToString()) > 0)
            {
                contador = DataBase.ExecuteScalarInt("SELECT COUNT(id) FROM tb_downloads");
            }

            return Convert.ToString(contador, 2);
        }
    }
}