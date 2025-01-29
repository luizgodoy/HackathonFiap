using Hackathon.Data.Interfaces;
using Hackathon.Data.Repository;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Services;

namespace Hackathon.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAppointmentServices, AppointmentServices>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            return services;
        }
    }
}