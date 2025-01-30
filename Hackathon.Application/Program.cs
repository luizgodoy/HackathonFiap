using Hackathon.Application.AutoMapper;
using Hackathon.Application.Consumers;
using Hackathon.Data.Context;
using Hackathon.Data.Interfaces;
using Hackathon.Data.Repository;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Reflection;

namespace Hackathon.Application
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            // Configuração de AutoMapper
            services.AddAutoMapper(typeof(MapperProfile), typeof(MapperProfile));

            // Injeção de dependências para serviços e repositórios
            services.AddScoped<IAppointmentServices, AppointmentServices>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            // Configuração do contexto de banco de dados
            var connectionString = hostContext.Configuration.GetConnectionString("SqlConnection");
            services.AddDbContext<HackathonDbContext>(options => options.UseSqlServer(connectionString));

            // Configuração do RabbitMQ com MassTransit
            var rabbitMqSettings = hostContext.Configuration.GetSection("RabbitMq");
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetEntryAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    // Configurações do RabbitMQ lidas do appsettings.json
                    cfg.Host(rabbitMqSettings["Host"], "/", h =>
                    {
                        h.Username(rabbitMqSettings["Username"] ?? "guest");
                        h.Password(rabbitMqSettings["Password"] ?? "guest");
                    });

                    const string exchangeName = "hackathon.direct";

                    cfg.ReceiveEndpoint("update-appointment", e =>
                    {
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.ConfigureConsumeTopology = false;

                        e.Bind(exchangeName, s =>
                        {
                            s.RoutingKey = "update.appointment";
                            s.ExchangeType = ExchangeType.Direct;
                            s.Durable = true;
                        });

                        e.ConfigureConsumer<EditAppointmentConsumer>(context);
                    });
                });
            });
        });
    }
}