using Hackathon.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Data.Context
{
    public class DesignTimeDbContextConfiguration : IDesignTimeDbContextFactory<HackathonDbContext>
    {
        public HackathonDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()                
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Hackathon.API"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HackathonDbContext>();
            var connectionString = configuration.GetConnectionString("SqlConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new HackathonDbContext(optionsBuilder.Options);
        }
    }
}

