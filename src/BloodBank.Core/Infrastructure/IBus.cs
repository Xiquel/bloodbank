using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Core.Infrastructure
{
    public interface IBus
    {
        Task Publish(IIntegrationMessage message);
    }
}
