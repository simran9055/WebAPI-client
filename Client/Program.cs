using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Client
{
    class Program
    {
        private static string POST = "http://localhost:59822/api/login";
        private static string GET = "http://localhost:59822/api/Messages";

        static void Main()
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            using (var httpClient = new HttpClient(handler))
            {
                var response = httpClient.PostAsJsonAsync(
                    POST,
                    new {Username = "User2", Password = "asdf" },
                    CancellationToken.None
                ).Result;
                response.EnsureSuccessStatusCode();
                bool success = response.IsSuccessStatusCode;
                Console.WriteLine("1: " + response.Headers.GetValues("Set-Cookie").FirstOrDefault());
                if (success)
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri(GET);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(request.Headers);
                    }
                    var secret = httpClient.GetStringAsync(GET);
                    var h= httpClient.GetAsync(GET);
                    Console.WriteLine(secret.Result);
                }
                else
                {
                    Console.WriteLine("Sorry you provided wrong credentials");
                }
            }

            Console.Read();
        }
    }
}
