using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CSELibrary
{
    public class Handler
    {
        //ポスト先のURL
        public static string URL { get; set; }
        public const string InsertAndUpdateURL = "http://www.tibaneet.com/SQL/InsertAndUpdate.php";
        public const string SelectURL = "http://www.tibaneet.com/SQL/Select.php";

        //コンストラクタ
        public Handler() { }

        public static string DoPost(NameValueCollection Values)
        {
            try
            {
               return DoPostEx(Values).Result;
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

        async public static Task<string> DoPostEx(NameValueCollection Values)
        {
            try
            {
                ////Create X509Certificate2 object from .cer file.
                //byte[] rawData = ReadFile($@"C:\Users\agi13\Downloads\server.csr");
                //X509Certificate2 certificate = new X509Certificate2(rawData);
                
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Clear();
                //handler.ClientCertificates.Add(certificate);
                handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls | SslProtocols.Tls13;
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback += ValidateServerCertificate;

                using (HttpClient httpClient = new HttpClient(handler))
                {
                    String requestEndPoint = URL;
                    HttpRequestMessage request = Handler.CreateRequest(HttpMethod.Post, requestEndPoint);
                    // こうした場合、Accept: multipart/form-data を指定となっていることが多いです。
                    request.Headers.Remove("Accept");
                    request.Headers.Add("Accept", "multipart/form-data");

                    var dic = new Dictionary<string, string>();
                    foreach (var item in Values)
                    {
                        var str = item.ToString();
                        dic.Add(str, Values[str]);
                    }
                    ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
                    System.Net.ServicePointManager.SecurityProtocol |=
                        SecurityProtocolType.Tls
                        | SecurityProtocolType.Tls11
                        | SecurityProtocolType.Tls12
                        ;
                    var content = new FormUrlEncodedContent(dic);
                    request.Content = content;
                    //var response = await httpClient.SendAsync(request);
                    var response = await httpClient.PostAsync(URL, content);
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
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

        internal static byte[] ReadFile(string fileName)
        {
            FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

        /// <summary>
        /// HTTPリクエストメッセージを生成する内部メソッドです。
        /// </summary>
        /// <param name="httpMethod">HTTPメソッドのオブジェクト</param>
        /// <param name="requestEndPoint">通信先のURL</param>
        /// <returns>HttpRequestMessage</returns>
        private static HttpRequestMessage CreateRequest(HttpMethod httpMethod, string requestEndPoint)
        {
            var request = new HttpRequestMessage(httpMethod, requestEndPoint);
            return Handler.AddHeaders(request);
        }

        /// <summary>
        /// HTTPリクエストにヘッダーを追加する内部メソッドです。
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>HttpRequestMessage</returns>
        private static HttpRequestMessage AddHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept-Charset", "utf-8");
            // 同じようにして、例えば認証通過後のトークンが "Authorization: Bearer {トークンの文字列}"
            // のように必要なら適宜追加していきます。
            return request;
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
