using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
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
                using (WebClient wc = new WebClient())
                {
                    return Encoding.Default.GetString(wc.UploadValues(URL, Values));
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

        public static List<Dictionary<object, object>> ConvertDeserialize(string jsonstr) {
            // デシリアライズしてDictionaryに戻します。
            var json = JsonConvert.DeserializeObject<List<Dictionary<object, object>>>(jsonstr);
            return json;
        }
    }
}
