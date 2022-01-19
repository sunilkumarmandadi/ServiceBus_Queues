using ServiceBusTrigger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBusTrigger.ServiceBus
{
    public interface IMessagePush
    {
        Task SendMessage(Product product);
    }
}
