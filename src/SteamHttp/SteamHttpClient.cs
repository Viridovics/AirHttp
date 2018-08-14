using System;
using System.IO;
using System.Net;
using SteamHttp.Configuration;

namespace SteamHttp
{
    public class SteamHttpClient
    {
        private ISteamHttpConfiguration _configuration;
        public SteamHttpClient(ISteamHttpConfiguration configuration)
        {
            _configuration = configuration;
            System.Console.WriteLine("Create object");
        }

        public SteamHttpResponse<T> Get<T>(string url)
        {
            return QueryUrl<T>(url, "GET");
        }

        private SteamHttpResponse<T> QueryUrl<T>(string url, string method, string body = null)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = _configuration.ContentType;
                httpWebRequest.Method = method;

                if (body != null)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(body);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return SteamHttpResponse<T>.CreateSuccessResponse(httpResponse.StatusCode,
                                                                      _configuration.DeserializeObject<T>(streamReader.ReadToEnd()));
                }
            }
            catch(Exception e)
            {
                return SteamHttpResponse<T>.CreateFaultedResponse(e);
            }
        }
    }
}