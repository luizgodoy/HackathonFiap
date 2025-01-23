using Hackathon.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointment", "Hackathon");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnType("NUMERIC").IsRequired();
            builder.Property(e => e.Title).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(e => e.Description).HasColumnType("VARCHAR(100)");
            builder.Property(e => e.Date).HasColumnType("DATE");
            builder.Property(e => e.DoctorId).HasColumnType("NUMERIC");
            builder.Property(e => e.Patient).HasColumnType("NUMERIC");

            builder.HasOne(a => a.Doctor).WithMany(u => u.Appointments).HasForeignKey(a => a.DoctorId);
            builder.HasOne(a => a.Patient).WithMany(u => u.Appointments).HasForeignKey(a => a.PatientId);
        }   
    }
}