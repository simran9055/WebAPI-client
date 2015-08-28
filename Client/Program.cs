using System;
using System.Collections.Generic;
using System.IO;
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
        private static string POST = "http://localhost:59822/api/v1/login";
        private static string POSTCreateReservation = "http://localhost:59822/api/v1/systems/8008800003/createCallReservation";

        private static string GETMessages =
            "http://localhost:59822/api/v1/systems/8008800003/mailboxes/80/folders/New/messages?SortAsc=true";

        static void Main()
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            using (var httpClient = new HttpClient(handler))
            {
                var response = httpClient.PostAsJsonAsync(
                    POST,
                    new {Username = "@freedomvoice.com", Password = "" },
                    CancellationToken.None
                ).Result;
                response.EnsureSuccessStatusCode();
                bool success = response.IsSuccessStatusCode;
                Console.WriteLine("1: " + response.Headers.GetValues("Set-Cookie").FirstOrDefault());
                if (success)
                {
                    var response2 = httpClient.PostAsJsonAsync(
                         POSTCreateReservation,
                        new {
                            ExpectedCallerIdNumber = "7605769573",
                            DestinationPhoneNumber = "7608349196",
                            PresentationPhoneNumber = 8008800003
                        },
                        CancellationToken.None
                        );
                    Stream receiveStream = response2.Result.Content.ReadAsStreamAsync().Result;
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    Console.WriteLine(readStream.ReadToEnd());
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
