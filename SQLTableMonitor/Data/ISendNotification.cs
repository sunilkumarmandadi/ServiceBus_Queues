using ServiceBusTrigger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusTrigger.Data
{
    public interface ISendNotification
    {
       public Task SendNotificationToServiceBus(Product product);
    }
}
