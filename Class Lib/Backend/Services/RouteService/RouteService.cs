using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        public static async Task<RouteInfo> GetRouteInfoAsync(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude)
        {
            string url = "https://api.openrouteservice.org/v2/directions/driving-hgv/json";

            var body = new
            {
                coordinates = new[]
                {
            new[] { fromLongitude, fromLatitude },
            new[] { toLongitude, toLatitude }
        }
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", ApiKey);

            try
            {
                var response = await Client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new RouteInfo
                    {
                        IsSuccess = false,
                        ErrorMessage = $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
                    };
                }

                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(resultJson);

                var summary = result["routes"]?[0]?["summary"];
                if (summary != null)
                {
                    double distance = summary["distance"]?.Value<double>() ?? 0;
                    double duration = summary["duration"]?.Value<double>() ?? 0;

                    return new RouteInfo
                    {
                        DistanceKm = distance / 1000.0,
                        Duration = TimeSpan.FromSeconds(duration)
                    };
                }

                return new RouteInfo
                {
                    IsSuccess = false,
                    ErrorMessage = "Інформацію не знайдено."
                };
            }
            catch (Exception ex)
            {
                return new RouteInfo
                {
                    IsSuccess = false,
                    ErrorMessage = $"Помилка: {ex.Message}"
                };
            }
        }

        public static double CostEstimate(double distance)
        {
            double minCost = 20;
            double maxCost = 300;
            double k = 0.05; // steepness of the curve
            double x0 = 75;  // midpoint of the curve

            double logistic = 1 / (1 + Math.Exp(-k * (distance - x0)));
            return minCost + (maxCost - minCost) * logistic;
        }
    }
    public class RouteInfo
    {
        public double DistanceKm { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }
    }
}
