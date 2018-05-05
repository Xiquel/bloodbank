using RabbitMQ.Client;
using System.Threading.Tasks;

namespace BloodBank.Core.Infrastructure
{
    public class RabbitBus : IBus
    {
        private readonly IModel connection;

        public RabbitBus(IModel connection)
        {
            this.connection = connection;
        }
        public Task Publish(IIntegrationMessage message)
        {
           return Task.Run(()=> connection.BasicPublish(message.ExchangeName,message.QueueName,null,message.Data));
        }
    }
}
