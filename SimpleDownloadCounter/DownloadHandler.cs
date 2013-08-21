using System;
using System.Web;
using System.IO;
using System.Configuration;

namespace SimpleDownloadCounter
{
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileName = context.Request.QueryString["filename"].ToString();
            string filePath = Configuration.FilesPath;
            FileStream file = null;

            if (File.Exists(filePath + fileName))
            {
                file = new FileStream(filePath + fileName, FileMode.Open, FileAccess.Read);

                context.Response.Clear();
                context.Response.Buffer = false;
                context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                context.Response.ContentType = "application/octet-stream";
                context.Response.AppendHeader("Content-Length", file.Length.ToString());

                int offset = 0;
                int readCount;
                byte[] buffer = new byte[64 * 1024];
                while (context.Response.IsClientConnected && offset < file.Length)
                {

                    file.Seek(offset, SeekOrigin.Begin);
                    readCount = file.Read(buffer, 0, (int)Math.Min(file.Length - offset, buffer.Length));

                    context.Response.OutputStream.Write(buffer, 0, readCount);
                    offset += readCount;
                }

                try
                {
                    if (context.Response.IsClientConnected)
                    {
                        DownloadCount.AddDownload(context.Request.ServerVariables, fileName);
                    }
                }
                catch (Exception)
                {
                    //Can't record data
                }
                file.Dispose();
                file.Close();
            }
            else 
            {
                context.Response.StatusCode = 404;   
            }
            context.ApplicationInstance.CompleteRequest();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}