using System;
using System.IO;
using System.Net;
using System.Text;
using Plus;

namespace LicenseManager
{
    public static class LicenseManager
    {
        internal static void License()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.msforum.ml/validation.php");

            var postData = "sn=233-421-752-325";
            postData += "&thing2=world";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (responseString.Contains("602"))
            {
                Console.WriteLine("Hey");
                

                return;
            }
            else
            {
               // Program.StartEverything();
            }
        }
       
    }
}