using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Geocodificador
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    IConfiguration configuration = hostContext.Configuration;

                    AmqpInfo options = new AmqpInfo()
                    {
                        Username = configuration["Amqp.Username"], 
                        Password = configuration["Amqp.Password"],
                        VirtualHost = configuration["Amqp.Virtualhost"], 
                        HostName = configuration["Amqp.Hostname"], 
                        Uri = configuration["Amqp.Uri"]
                    };

                    services.AddSingleton(options);

                    services.AddHostedService<Worker>();

                });
    }
}
