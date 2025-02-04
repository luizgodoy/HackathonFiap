using Hackathon.Contract.Contracts;
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
                        cfg.Host("192.168.0.15", "/", h =>
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

//var message = new EmailNotificationMessage
//{
//    RecipientEmail = "mr.zampieri@live.com",
//    Subject = "Nova Consulta",
//    Body = "Você tem uma consulta agendada para amanhã às 14h."
//};

//await publishEndpoint.Publish(message);

//Console.WriteLine("📩 Mensagem publicada com sucesso!");


var appointment = new EditAppointmentMessage
{
    Id = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
    Title = "Consulta",
    Description = "Detalhes da consulta",
    DoctorId = Guid.NewGuid(),
    FinishAt = DateTime.Now,
    StartAt = DateTime.Now.AddMinutes(-60),
    PatientId = Guid.NewGuid(),
};

await publishEndpoint.Publish(appointment);

Console.WriteLine("📩 Mensagem publicada com sucesso!");

await host.StopAsync();