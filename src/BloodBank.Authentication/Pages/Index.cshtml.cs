using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Microservice.Health;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Dotnet.Microservice.Health.HealthCheckRegistry;

namespace Bloodbank.Web.Status.Pages
{
    public class IndexModel : PageModel
    {
        public HealthStatus HealthStatus;
        public string OverallStatus => HealthStatus.IsHealthy ? "Healthy" : "Unhealthy";
        public void OnGet()
        {
           HealthStatus =  HealthCheckRegistry.GetStatus();
           
        }
    }
}
