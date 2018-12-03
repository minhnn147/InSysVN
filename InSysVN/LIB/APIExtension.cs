using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public class APIExtension
    {
        public static HttpResponseMessage PostKeyValue(Dictionary<string, string> dictionaryBody, string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            var content = new FormUrlEncodedContent(dictionaryBody);
            var result = client.PostAsync(url, content).Result;
            client.Dispose();
            return result;
        }
        public HttpResponseMessage PostJson(Dictionary<string, string> dictionary, string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            var content = new FormUrlEncodedContent(dictionary);
            return client.PostAsync(url, content).Result;
        }
        public static string GetValueAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
