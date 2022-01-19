using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using ServiceBusTrigger.Data;

namespace ServiceBusTrigger
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddUserSecrets(typeof(Program).Assembly, optional: true)
              .AddEnvironmentVariables()
              .AddCommandLine(args);

            var config = builder.Build();
            var host = Host.CreateDefaultBuilder()
               .ConfigureServices((context, services) => {
                   services.AddSingleton<IReceiveNotification, ReceiveNotification>();
                   services.AddSingleton<ISendNotification, SendNotification>();
                   services.AddHttpClient();
               }).Build();
            
            host.Services.GetService<IReceiveNotification>().ReceiveNotificationFromSQL();
        }
    }
}
