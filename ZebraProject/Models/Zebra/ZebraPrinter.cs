using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using System.Net;
using HtmlAgilityPack;

namespace ZebraProject.Models.Zebra
{
    public class ZebraPrinter
    {
        private string _ipAddress;
        private int _port;

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public string PreviewPath { get; set; }

        public string PreviewFilePath { get; set; }

        public TcpClient Client { get; private set; }

        public StreamWriter Writer { get; private set; }

        public string ErrMessage { get; private set; }

        public ZebraPrinter() { }

        public ZebraPrinter(string ip, int port)
            : this()
        {
            this._ipAddress = ip;
            this._port = port;
        }

        public void Open()
        {
            this.Client = new TcpClient();
            this.Client.Connect(this._ipAddress, this._port);
            this.Writer = new StreamWriter(this.Client.GetStream());
        }

        public bool Print(string zpl)
        {
            try
            {
                this.Writer.Write(zpl);
                this.Writer.Flush();

                return true;
            }
            catch (Exception ex)
            {
                this.ErrMessage = ex.Message;
                return false;
            }
        }

        public string GetPreviewImage(string zpl)
        {
            // Get preview image URL
            var imageURL = GetPreviewImageURL(zpl);
            // Get the image from the printer
            string base64Image = GetImageFromURL(imageURL);
            return base64Image;
        }

        //private string GetPreviewURL()
        //{
        //    return "http://" + _ipAddress + "/printer";
        //}

        private string GetPrinterURL()
        {
            return "http://" + _ipAddress;
        }

        private string GetPreviewImageURL(string zpl)
        {
            string response = null;
            string parameters = GetPreviewParameter(zpl);
            string url = GetPrinterURL() + this.PreviewPath;

            // Post to the printer
            //try
            //{
            response = HttpPost(url, parameters);
            //}
            //catch
            //{
            //url = GetPrinterURL() + "/zpl";
            //response = HttpPost(url, parameters);
            //}


            // Parse the response to get the image name.  This image name is stored for one retrieval only.
            string imageURL = ParseImageURL(response);

            string fullImageURL = GetPrinterURL() + this.PreviewFilePath + "/" + imageURL;
            // Return the image name.
            return fullImageURL;
        }

        private string ParseImageURL(string previewHTML)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(previewHTML);
            var imageNameXPath = "/html[1]/body[1]/div[1]/img[1]/@alt[1]";
            var imageURL = doc.DocumentNode.SelectSingleNode(imageNameXPath).GetAttributeValue("src", "");
            // Return the image name.
            return imageURL;
        }

        private static string GetPreviewParameter(string zpl)
        {
            // Setup the post parameters.
            string parameters = "data=" + HttpUtility.UrlEncode(zpl);
            parameters = parameters + "&" + "dev=R";
            parameters = parameters + "&" + "oname=UNKNOWN";
            parameters = parameters + "&" + "otype=ZPL";
            parameters = parameters + "&" + "prev=Preview Label";
            parameters = parameters + "&" + "pw=";
            return parameters;
        }


        private static string HttpPost(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Proxy = new System.Net.WebProxy();

            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            //We need to count how many bytes we're sending. 
            //Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
            req.ContentLength = bytes.Length;

            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();

            System.Net.WebResponse resp = req.GetResponse();

            if (resp == null) return null;
            System.IO.StreamReader sr =
                    new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        private string GetImageFromURL(string url)
        {
            var response = Http.Get(url);

            using (var ms = new MemoryStream(response))
            {
                string base64String = Convert.ToBase64String(ms.ToArray());
                return base64String;
            }
        }

        public void Close()
        {
            this.Writer.Close();
            this.Client.Close();
        }
    }

    public static class Http
    {
        public static byte[] Get(string uri)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.DownloadData(uri);
            }
            return response;
        }
    }
}