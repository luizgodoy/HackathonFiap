﻿using Hackathon.Core.Models;
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
            builder.Property(e => e.Title).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(e => e.Description).HasColumnType("VARCHAR(100)");
            builder.Property(e => e.StartAt).HasColumnType("DATE");
            builder.Property(e => e.FinishAt).HasColumnType("DATE");

            //builder.HasOne(a => a.Doctor).WithMany(u => u.Appointments).HasForeignKey(a => a.DoctorId);
            //builder.HasOne(a => a.Patient).WithMany(u => u.Appointments).HasForeignKey(a => a.PatientId);
        }   
    }
}