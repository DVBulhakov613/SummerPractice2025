using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Class_Lib.Services
{
    public class RouteService
    {
        //public static async Task Main(string[] args)
        //{
        //    double fromLatitude = 50.007526; // Example latitude 50.007526, 36.234823
        //    double fromLongitude = 36.234823; // Example longitude 49.838625, 36.558511
        //    double toLatitude = 49.838625; // Example latitude
        //    double toLongitude = 36.558511; // Example longitude
        //    await GetRouteInfoAsync(fromLatitude, fromLongitude, toLatitude, toLongitude);
        //    // TestGetRouteInfoAsync().Wait();
        //}

        private static readonly string ApiKey = "5b3ce3597851110001cf6248cdc645bc4dad474ab7023cecf0a12b46";
        private static readonly HttpClient Client = new HttpClient();

        public static async Task<string> GetRouteInfoAsync(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude)
        {
            string url = "https://api.openrouteservice.org/v2/directions/driving-hgv/json";


            var body = new
            {
                coordinates = new[]
                {
                // strict long, lati order
                new[] { fromLongitude, fromLatitude },
                new[] { toLongitude, toLatitude }
            }
            };

            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            Console.Write("Sending: " + jsonBody);

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", ApiKey);

            var response = await Client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(resultJson);

                var routes = result["routes"] as JArray;
                if (routes != null && routes.Count > 0)
                {
                    var summary = routes[0]["summary"];
                    if (summary != null)
                    {
                        double distance = summary["distance"]?.Value<double>() ?? 0;
                        double duration = summary["duration"]?.Value<double>() ?? 0;

                        return $"Відстань: {distance / 1000.0:F2} км | Розрахований час: {TimeSpan.FromSeconds(duration):hh\\:mm\\:ss}";
                    }
                }
            }

            return "Сервіс недоступний або помилка.";
        }
    }
}
