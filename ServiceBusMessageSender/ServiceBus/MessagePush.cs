using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceBusTrigger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusTrigger.ServiceBus
{
    public class MessagePush : IMessagePush
    {
        private const int numOfMessages = 3;
        private readonly IConfiguration _configuration;
        public MessagePush(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMessage(Product product)
        {
            string connectionString = _configuration.GetConnectionString("ServiceBusConnection");

            string queueName = "servicebusqueue";
            ServiceBusClient client;
            ServiceBusSender sender;

            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);


            #region batchMessages 
            //using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            //foreach(Product product in products)
            //{
            //    string jsonString = JsonConvert.SerializeObject(product);
            //    byte[] byteArray = Encoding.ASCII.GetBytes(jsonString);

            //    // try adding a message to the batch
            //    if (!messageBatch.TryAddMessage(new ServiceBusMessage(byteArray)))
            //    {
            //        // if it is too large for the batch
            //        throw new Exception($"The message {jsonString} is too large to fit in the batch.");
            //    }
            //}
            #endregion batchMessages
            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
               // await sender.SendMessagesAsync(messageBatch);

                string jsonEntity = JsonConvert.SerializeObject(product);
                ServiceBusMessage serializedContents = new ServiceBusMessage(jsonEntity);
                await sender.SendMessageAsync(serializedContents);
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }

        //private async Task ProcessOrder(IEnumerable<PizzaOrder> orders)
        //{
        //    await using (ServiceBusClient client = new ServiceBusClient(ConnectionString))
        //    {
        //        ServiceBusSender sender = client.CreateSender(QueueName);

        //        foreach (var order in orders)
        //        {
        //            string jsonEntity = JsonSerializer.Serialize(order);
        //            ServiceBusMessage serializedContents = new ServiceBusMessage(jsonEntity);
        //            await sender.SendMessageAsync(serializedContents);
        //        }
        //    }
        //}
    }
}
