using System;
using System.Collections.Generic;
using System.Text;

namespace BloodBank.Core.Infrastructure
{
    public interface IIntegrationMessage
    {
        string QueueName { get; set; }
        string ExchangeName { get; set; }
        byte[] Data { get; set; }
    }
}
