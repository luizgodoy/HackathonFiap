using Hackathon.Core.DTO;
using Hackathon.NotificationService.AutoMapper;
using Hackathon.NotificationService.Consumers;
using Hackathon.NotificationService.Interfaces;
using Hackathon.NotificationService.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Reflection;

namespace Hackathon.NotificationService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();


            await builder.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                // Configuração de AutoMapper
                services.AddAutoMapper(typeof(MapperProfile), typeof(MapperProfile));

                // Injeção de dependências para serviços e repositórios
                services.AddScoped<ISmtpClient, SmtpClientWrapper>();
                services.AddScoped<IEmailServices, EmailServices>();

                var emailSettings = hostContext.Configuration.GetSection("EmailSettings").Get<EmailServerSettings>();
                services.AddSingleton(emailSettings);

                // Configuração do RabbitMQ com MassTransit
                var rabbitMqSettings = hostContext.Configuration.GetSection("RabbitMq");
                services.AddMassTransit(x =>
                {
                    x.AddConsumers(Assembly.GetEntryAssembly());

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        // Configurações do RabbitMQ lidas do appsettings.json
                        cfg.Host("192.168.0.15", "/", h =>
                        {
                            h.Username(rabbitMqSettings["Username"] ?? "guest");
                            h.Password(rabbitMqSettings["Password"] ?? "guest");
                        });

                        cfg.ReceiveEndpoint("email-notification", e =>
                        {
                            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                            //e.ConfigureConsumeTopology = false;

                            //e.Bind(exchangeName, s =>
                            //{
                            //    s.RoutingKey = "email-notification";
                            //    s.ExchangeType = ExchangeType.Direct;
                            //    s.Durable = true;
                            //});

                            e.ConfigureConsumer<EmailNotificationConsumer>(context);
                        });
                    });
                });
            });
    }
}