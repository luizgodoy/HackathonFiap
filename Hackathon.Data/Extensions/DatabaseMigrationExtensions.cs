using Microsoft.AspNetCore.Builder;
using Hackathon.Data.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Data.Extensions
{
    public static class DatabaseMigrationExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<HackathonDbContext>();
                dbContext.MigrateAsync().GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
