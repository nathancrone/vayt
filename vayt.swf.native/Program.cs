using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Web;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace vayt.swf.native
{
    class Program
    {
        static void Main(string[] args)
        {
            string bodystring = @"{""name"": ""WFMin""}";
            byte[] body = Encoding.UTF8.GetBytes(bodystring);

            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "POST\n" + 
            "/\n" + 
            "\n" +
            "host:swf.us-east-1.amazonaws.com\n" +
            "x-amz-date:" + timestamp + "\n" +
            "x-amz-target:SimpleWorkflowService.DescribeDomain\n" +
            "content-encoding:amz-1.0\n" + bodystring;


            string awsPrivateKey = "YYY";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);
            
            string s3Url = "http://swf.us-east-1.amazonaws.com/";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "POST";
            req.Host = "swf.us-east-1.amazonaws.com";
            req.Headers["x-amz-date"] = timestamp;
            req.Headers["x-amz-target"] = "SimpleWorkflowService.DescribeDomain";
            req.Headers["Content-Encoding"] = "amz-1.0";
            req.Headers["x-amzn-authorization"] = "AWS3 AWSAccessKeyId=XXX,Algorithm=HmacSHA1,SignedHeaders=Host;X-Amz-Date;X-Amz-Target;Content-Encoding,Signature=" + encodedCanonical;
            req.ContentType = "application/x-amz-json-1.0";
            req.ContentLength = body.Length;

            Stream reqStream = req.GetRequestStream();
            reqStream.Write(body, 0, body.Length);
            reqStream.Flush();
            reqStream.Close();

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                Console.Write(responseXml);
            }

            Console.WriteLine("S3 object uploaded.");
            Console.ReadLine();
        }

        //private static string CalculateTimestamp()
        //{
        //    string timestamp = Uri.EscapeUriString(string.Format("{0:s}", DateTime.UtcNow));
        //    timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        //    timestamp = HttpUtility.UrlEncode(timestamp).Replace("%3a", "%3A");

        //    return timestamp;
        //}

        //private static string Beautify(XmlDocument doc)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    XmlWriterSettings settings = new XmlWriterSettings
        //    {
        //        Indent = true,
        //        IndentChars = "  ",
        //        NewLineChars = "\r\n",
        //        NewLineHandling = NewLineHandling.Replace
        //    };
        //    using (XmlWriter writer = XmlWriter.Create(sb, settings))
        //    {
        //        doc.Save(writer);
        //    }
        //    return sb.ToString();
        //}
    }
}
