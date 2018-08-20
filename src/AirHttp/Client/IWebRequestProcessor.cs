using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirHttp.Client
{
    internal interface IWebRequestProcessor
    {
        Task<Tuple<HttpWebResponse, string>> Process(HttpWebRequest httpWebRequest, Lazy<string> body, Encoding encoding, CancellationToken cancellationToken);
    }
}