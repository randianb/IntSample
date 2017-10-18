using Dev.Options;
using Int;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Dev.Controller
{
    public class TelegramMessageSender
    {
        private static string INSTANCE_ID = AppConfiguration.GetAppConfig("INSTANCE_ID");
        private static string CLIENT_ID = AppConfiguration.GetAppConfig("CLIENT_ID");
        private static string CLIENT_SECRET = "3db62fc897ef4322a607699ff20f249c"; 
        private static string API_URL = "http://api.whatsmate.net/v1/telegram/single/message/" + INSTANCE_ID;

        public TelegramMessageSender()
        {
            // CLIENT_SECRET = Int.Members.MessageClientToken;
        }

        public bool sendMessage(string number, string message)
        {

            bool success = true;

            try
            {

                using (WebClient client = new WebClient())
                {

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;

                    client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                    Payload payloadObj = new Payload() { number = number, message = message };

                    string postData = (new JavaScriptSerializer()).Serialize(payloadObj);



                    client.Encoding = Encoding.UTF8;

                    string response = client.UploadString(API_URL, postData);

                    Console.WriteLine(response);

                }
            }
            catch (WebException webEx)
            {
                CommonController.Log("[Message Code] " + ((HttpWebResponse)webEx.Response).StatusCode);

                Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);

                Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();

                StreamReader reader = new StreamReader(stream);

                String body = reader.ReadToEnd();

                Console.WriteLine(body);

                success = false;

            }

            return success;
        }
    }

    public class Payload
    {
        public string number { get; set; }
        public string message { get; set; }
    }
}
