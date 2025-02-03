using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testes;


var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host("192.168.0.15", 5672, "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });
                    });
                });
            })
            .Build();

await host.StartAsync();

var publishEndpoint = host.Services.GetRequiredService<IPublishEndpoint>();

Publisher.PublishEmail(publishEndpoint);

Publisher.PublishAppointment(publishEndpoint);

await host.StopAsync();