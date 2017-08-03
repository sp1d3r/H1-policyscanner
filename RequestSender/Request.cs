using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RequestSender
{
    class Request
    {
        public string Coockie { get; set; }
        public string Headers { get; set; }
        public string Req { get; set; }
        public string Url { get; set; }
        public string method { get; set; }
        public string respheaders { get; set; }

        public Request()
        {
            Coockie = "";
            Headers = "";
            Req = "";
            Url = "";
            method = "";
            respheaders = "";
        }
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public string Send()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Stream dataStream;
            WebResponse response;
            request.Method = method;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Referer = "https://hackerone.com/directory?query=bounties%3Ayes&sort=published_at%3Adescending&page=1";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            response = (HttpWebResponse)request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
    }
}
