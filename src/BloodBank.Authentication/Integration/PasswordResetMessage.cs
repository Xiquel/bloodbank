using BloodBank.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Authentication.Integration
{
    public class PasswordResetMessage : IIntegrationMessage
    {
        public string QueueName { get; set; } = "password_reset";
        public string ExchangeName { get; set; } = string.Empty;
        public byte[] Data { get ; set; }
    }
}
