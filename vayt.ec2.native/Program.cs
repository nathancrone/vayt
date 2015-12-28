using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Web;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace vayt.ec2.native
{
    class Program
    {
        static void Main(string[] args)
        {
            //timestamp
            string timestamp = CalculateTimestamp();

            //create string to sign -- must be alpha ordered
            string stringToConvert = "GET\n" +
            "ec2.amazonaws.com\n" +
            //"ec2.us-west-2.amazonaws.com\n" +
            "/\n" +
            "AWSAccessKeyId=XXX" +
            "&Action=DescribeInstances" +
            //"&Filter.1.Name=availability-zone" +
            //"&Filter.1.Value.1=us-west-2" +
            //"&Filter.1.Value.2=us-east-1c" +
            "&SignatureMethod=HmacSHA1" +
            "&SignatureVersion=2" +
            "&Timestamp=" + timestamp +
            "&Version=2011-12-15";

            string awsPrivateKey = "XXX";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);
            string urlEncodedCanonical = HttpUtility.UrlEncode(encodedCanonical).Replace("+", "%20").Replace("%3d", "%3D").Replace("%2f", "%2F").Replace("%2b", "%2B");

            //actual URL string
            string ec2Url = "https://ec2.amazonaws.com/?Action=DescribeInstances" +
            //string ec2Url = "https://ec2.us-west-2.amazonaws.com/?Action=DescribeInstances" +
            "&Version=2011-12-15" +
            //"&Filter.1.Name=availability-zone" +
            //"&Filter.1.Value.1=us-west-2" +
            //"&Filter.1.Value.2=us-east-1c" +
            "&Timestamp=" + timestamp +
            "&Signature=" + urlEncodedCanonical +
            "&SignatureVersion=2" +
            "&SignatureMethod=HmacSHA1" +
            "&AWSAccessKeyId=XXX";

            HttpWebRequest req = WebRequest.Create(ec2Url) as HttpWebRequest;
            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }
            
            Console.WriteLine(Beautify(doc));
            Console.ReadLine();
        }

        private static string CalculateTimestamp()
        {
            string timestamp = Uri.EscapeUriString(string.Format("{0:s}", DateTime.UtcNow));
            timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            timestamp = HttpUtility.UrlEncode(timestamp).Replace("%3a", "%3A");

            return timestamp;
        }

        private static string Beautify(XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }
    }
}
