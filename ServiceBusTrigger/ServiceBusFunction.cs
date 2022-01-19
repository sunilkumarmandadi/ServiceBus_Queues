using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ServiceBusTrigger.Models;

namespace ServiceBusTrigger
{
    public static class ServiceBusFunction
    {
        [FunctionName("ServiceBusFunction")]
        public static void Run([ServiceBusTrigger("servicebusqueue", Connection = "servicebusconnection")]Product product, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {product.Name}");
        }
    }
}
