
using Hackathon.API.AutoMapper;
using Hackathon.API.Configurations;
using Hackathon.Contract.Contracts;
using Hackathon.Core.Models;
using Hackathon.Data.Context;
using Hackathon.Data.Interfaces;
using Hackathon.Data.Repository;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Services;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do banco de dados
            var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
            builder.Services.AddDbContext<HackathonDbContext>(options => options.UseSqlServer(connectionString));

            // Configuração do AutoMapper
            builder.Services.AddAutoMapper(typeof(MapperProfile), typeof(MapperProfile));

            // Add services to the container.
            builder.Services.ResolveDependencies();
            builder.Services.AddControllers();

            builder.Services.AddScoped<IAppointmentServices, AppointmentServices>();

            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Configuração do MassTransit com RabbitMQ
            /*
            var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq");
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    // Host do RabbitMQ definido no appsettings.json
                    //cfg.Host(rabbitMqSettings["Host"], "/", h =>
                   // {
                        //h.Username(rabbitMqSettings["Username"]);
                       // h.Password(rabbitMqSettings["Password"]);
                  //  });
                   
                    cfg.Message<EditAppointmentMessage>(p =>
                    {
                        p.SetEntityName("hackathon.direct");
                    });

                    cfg.Publish<EditAppointmentMessage>(p =>
                    {
                        p.ExchangeType = "direct";
                    });                    
                });
            });*/

            //Configurações Identity
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<HackathonDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication()
                .AddJwtBearer();

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();

            app.Run();
        }
    }
}
