using Hackathon.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hackathon.Data.Context
{
    public class HackathonDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }

        public HackathonDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HackathonDbContext).Assembly);            
        }
        public async Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            await Database.MigrateAsync(cancellationToken);
        }
    }
}