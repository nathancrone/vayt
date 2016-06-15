using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net;
using System.Web;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace vayt.s3.native
{
    class Program
    {
        static void Main(string[] args)
        {
            //ListBuckets();
            //GetBucketProperty();
            //GetBucketItems();
            UploadToBucket();
        }

        private static void ListBuckets()
        {
            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "GET\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "\n" +                                  //content-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/";                                    //resource    

            string awsPrivateKey = "YYY";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);

            //actual URL string
            //no parameters with this request
            string s3Url = "https://s3.amazonaws.com/";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "GET";
            req.Host = "s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;
            //AWS AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS XXX:" + encodedCanonical;

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }

            Console.WriteLine("S3 bucket list queried.");
            Console.WriteLine(Beautify(doc));
            Console.ReadLine();
        }

        private static void GetBucketProperty()
        {
            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "GET\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "\n" +                                  //conten-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/kd5t/?lifecycle";                        //resource    

            string awsPrivateKey = "XXX";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);

            //actual URL string
            //no parameters with this request, but bucketname in URL
            string s3Url = "https://kd5t.s3.amazonaws.com/?lifecycle";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "GET";
            req.Host = "kd5t.s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;
            //AWS AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS XXX:" + encodedCanonical;

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }

            Console.WriteLine("S3 bucket list queried.");
            Console.WriteLine(Beautify(doc));
            Console.ReadLine();
        }

        private static void GetBucketItems()
        {
            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "GET\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "\n" +                                  //conten-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/kd5t/";                        //resource    

            string awsPrivateKey = "XXX";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);

            //actual URL string
            //no parameters with this request, but bucketname in URL
            string s3Url = "https://kd5t.s3.amazonaws.com/";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "GET";
            req.Host = "kd5t.s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;
            //AWS AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS XXX:" + encodedCanonical;

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }

            Console.WriteLine("S3 bucket list queried.");
            Console.WriteLine(Beautify(doc));
            Console.ReadLine();
        }

        private static void UploadToBucket()
        {
            byte[] fileData = File.ReadAllBytes(@"demo.txt");

            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "PUT\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "text/plain\n" +                        //content-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/kd5t/demo.txt";             //resource    

            string awsPrivateKey = "YYY";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);

            //actual URL string
            //no parameters with this request, but bucketname in URL
            string s3Url = "https://kd5t.s3.amazonaws.com/demo.txt";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "PUT";
            req.Host = "kd5t.s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;
            req.ContentType = "text/plain";
            req.ContentLength = fileData.Length;
            //AWS AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS XXX:" + encodedCanonical;

            Stream reqStream = req.GetRequestStream();
            reqStream.Write(fileData, 0, fileData.Length);
            reqStream.Close();

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
            }

            Console.WriteLine("S3 object uploaded.");
            Console.ReadLine();
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
