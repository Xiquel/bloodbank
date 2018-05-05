using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Authentication.Models
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Vhost { get; set; }
        public List<string> Queues { get; set; }
    }
}
