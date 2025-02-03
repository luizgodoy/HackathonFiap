using Hackathon.Contract.Contracts;
using Hackathon.Core.DTO;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testes
{
    internal static class Publisher
    {
        internal static async void PublishEmail(IPublishEndpoint publishEndpoint)
        {
            var message = new EmailNotificationMessage
            {
                RecipientEmail = "mr.zampieri@live.com",
                Subject = "Nova Consulta",
                Body = "Você tem uma consulta agendada para amanhã às 14h."
            };

            await publishEndpoint.Publish(message);

            Console.WriteLine("📩 Mensagem publicada com sucesso!");
        }

        internal static async void PublishAppointment(IPublishEndpoint publishEndpoint)
        {
            var appointment = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                Title = "Consulta",
                Description = "Detalhes da consulta",
                DoctorId = Guid.NewGuid(),
                FinishAt = DateTime.Now,
                StartAt = DateTime.Now.AddMinutes(-60),
                PatientId = Guid.NewGuid(),
            };

            await publishEndpoint.Publish(appointment);

            Console.WriteLine("📩 Mensagem publicada com sucesso!");
        }
    }
}
