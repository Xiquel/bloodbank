using BloodBank.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using Dotnet.Microservice.Health;
using Dotnet.Microservice.Health.Checks;

namespace BloodBank.Authentication.Framework
{
    public static class ServiceCollectionExtensions
    {
       
        public static IServiceCollection AddRabbitMq(this IServiceCollection collection , RabbitMqSettings settings)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri($"amqp://{settings.UserName}:{settings.Password}@{settings.Host}:5672/{settings.Vhost}")
            };
            IConnection conn = factory.CreateConnection();
            var model = conn.CreateModel();
            model.ExchangeDeclare(exchange: "emails", type: "fanout");
            if (settings.Queues.Any())
            {
                foreach (var item in settings.Queues)
                {
                    model.QueueDeclare(item,exclusive:false, autoDelete:false);
                }
            }
            collection.AddSingleton(model);
            return collection;
        }
        public static IdentityBuilder AddPasswordlessLoginTotpTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var totpProvider = typeof(TopTLoginTotpTokenProvider);
            return builder.AddTokenProvider("PasswordlessLoginTotpProvider", totpProvider);
        }
    }
}
