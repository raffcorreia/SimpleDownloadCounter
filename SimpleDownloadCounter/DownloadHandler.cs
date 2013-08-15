using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Threading;

namespace SimpleDownloadCounter
{
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileName = context.Request.QueryString["filename"].ToString();
            string filePath = context.Request.PhysicalApplicationPath + "\\files\\";
            FileInfo file = new System.IO.FileInfo(filePath + fileName);
            if (file.Exists)
            {
                try
                {
                    DownloadCount.AddDownload(context.Request.ServerVariables, fileName);
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
    }
}