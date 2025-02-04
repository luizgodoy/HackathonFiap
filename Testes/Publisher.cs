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
        internal async static void PublishEmail(IPublishEndpoint publishEndpoint)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Erro {ex.Message}");
            }
        }

        internal async static  void PublishAppointment(IPublishEndpoint publishEndpoint)
        {

        }
    }
}
