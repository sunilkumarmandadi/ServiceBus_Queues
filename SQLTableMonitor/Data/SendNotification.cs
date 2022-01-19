using ServiceBusTrigger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceBusTrigger.Data
{
    public class SendNotification : ISendNotification
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public SendNotification(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task SendNotificationToServiceBus(Product product)
        {
            Console.WriteLine("Inside send");
            var serializedPayload = JsonSerializer.Serialize(product);

            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/api/QueueSender");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(serializedPayload);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            try
            {
                using (var response = await httpClient.SendAsync(request))
                {
                    Console.WriteLine("Success");
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unsuccessful {ex.Message}");
            }
        }
    }
}
