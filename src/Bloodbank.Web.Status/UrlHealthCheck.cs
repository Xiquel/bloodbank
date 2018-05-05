using Dotnet.Microservice.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bloodbank.Web.Status
{
    public class UrlHealthCheck
    {
        private  static HttpClient httpClient = new HttpClient();

        public static HealthResponse CheckHealth(string Url)
        {
            return Check(Url);
        }
        private static HealthResponse Check(string url)
        {
            try
            {
                var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return HealthResponse.Healthy();
            }
            catch (Exception ex)
            {

                return HealthResponse.Unhealthy($"Exception during check: {ex.GetType().FullName} for Url:{url}");
            }
        }
    }
}
