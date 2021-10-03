using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NEETLibrary
{
    class Handler
    {
        //ポスト先のURL
        public static string URL { get; set; }

        //コンストラクタ
        public Handler() { }

        public static string DoPost(NameValueCollection Values)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    return Encoding.Default.GetString(webClient.UploadValues(URL, Values));
                }
            }
            catch (WebException e)
            {

                Dictionary<string, object> ERROR = new Dictionary<string, object>();
                HttpWebResponse response = (HttpWebResponse)e.Response;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        ERROR.Add("result", "net_error_not_found");
                        break;
                    case HttpStatusCode.RequestEntityTooLarge:
                        ERROR.Add("result", "net_error_request_entry_too_large");
                        break;
                    case HttpStatusCode.ServiceUnavailable:
                        ERROR.Add("result", "net_error_service_unavailable");
                        break;
                    case HttpStatusCode.Forbidden:
                        ERROR.Add("result", "net_error_forbidden");
                        break;
                    default:
                        ERROR.Add("result", "net_error_unknown" + Environment.NewLine + e.Message);
                        break;
                }
                return JsonConvert.SerializeObject(ERROR);
            }
        }

        //for testing purpose only, accept any dodgy certificate... 
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public static List<Dictionary<object, object>> ConvertDeserialize(string jsonstr) {
            // デシリアライズしてDictionaryに戻します。
            var json = JsonConvert.DeserializeObject<List<Dictionary<object, object>>>(jsonstr);
            return json;
        }
    }
}
