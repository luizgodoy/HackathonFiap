﻿using Hackathon.Application.AutoMapper;
using Hackathon.Application.Consumers;
using Hackathon.Core.DTO;
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentServices, AppointmentServices>();

            var emailSettings = hostContext.Configuration.GetSection("EmailMessage").Get<EmailMessageSettings>();
            services.AddSingleton(emailSettings);

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

                    cfg.ReceiveEndpoint("update-appointment", e =>
                    {
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                        e.ConfigureConsumer<EditAppointmentConsumer>(context);
                    });
                });
            });
        });
    }
}