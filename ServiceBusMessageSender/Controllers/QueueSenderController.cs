using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBusTrigger.Models;
using ServiceBusTrigger.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBusTrigger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueSenderController : ControllerBase
    {
        private readonly IMessagePush _messagePush;

        public QueueSenderController(IMessagePush messagePush)
        {
            _messagePush = messagePush;
        }
        [HttpPost]
        public async Task PushMessageToServiceBus(Product product)
        {
            await _messagePush.SendMessage(product);
        }
    }
}
