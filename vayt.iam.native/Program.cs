using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Web;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace vayt.iam.native
{
    class Program
    {
        static void Main(string[] args)
        {
            //timestamp
            string timestamp = CalculateTimestamp();

            //create string to sign -- must be alpha ordered
            //encode path directly
            string stringToConvert = "GET\n" +
            "iam.amazonaws.com\n" +
            "/\n" +
            "AWSAccessKeyId=XXX" +
            "&Action=CreateUser" +
            "&Path=%2FIT%2Farchitecture%2F" +
            "&SignatureMethod=HmacSHA1" +
            "&SignatureVersion=2" +
            "&Timestamp=" + timestamp +
            "&UserName=seroterNative" +
            "&Version=2010-05-08";

            string awsPrivateKey = "XXX";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);
            string urlEncodedCanonical = HttpUtility.UrlEncode(encodedCanonical).Replace("+", "%20").Replace("%3d", "%3D").Replace("%2f", "%2F").Replace("%2b", "%2B");
            
            //actual URL string
            //encode path
            string iamUrl = "https://iam.amazonaws.com/?Action=CreateUser" +
            "&Path=%2FIT%2Farchitecture%2F" +
            "&UserName=seroterNative" +
            "&Version=2010-05-08" +
            "&Timestamp=" + timestamp +
            "&Signature=" + urlEncodedCanonical +
            "&SignatureVersion=2" +
            "&SignatureMethod=HmacSHA1" +
            "&AWSAccessKeyId=XXX";

            HttpWebRequest req = WebRequest.Create(iamUrl) as HttpWebRequest;
            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }

            Console.WriteLine("User created.");
            Console.WriteLine(doc.OuterXml);
            Console.ReadLine();
        }

        private static string CalculateTimestamp()
        {
            string timestamp = Uri.EscapeUriString(string.Format("{0:s}", DateTime.UtcNow));
            timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            timestamp = HttpUtility.UrlEncode(timestamp).Replace("%3a", "%3A");

            return timestamp;
        }
    }
}
