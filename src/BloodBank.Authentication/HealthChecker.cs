using Dotnet.Microservice.Health;
using Dotnet.Microservice.Health.Checks;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Authentication
{
    public class HealthChecker
    {
        public static void AddHealthChecks(IConfiguration configuration)
        {
            HealthCheckRegistry.RegisterHealthCheck("Postgresql", () => PostgresqlHealthCheck.CheckHealth(configuration.GetConnectionString("Auth")));
            // return collection;
        }
    }
}
