using Hackathon.Contract.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host("localhost", 5672, "/", h =>
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

var message = new EmailNotificationMessage
{
    RecipientEmail = "test@example.com",
    Subject = "Nova Consulta",
    Body = "Você tem uma consulta agendada para amanhã às 14h."
};

await publishEndpoint.Publish(message);

Console.WriteLine("📩 Mensagem publicada com sucesso!");

await host.StopAsync();