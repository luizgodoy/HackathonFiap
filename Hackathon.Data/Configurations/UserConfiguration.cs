using Hackathon.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "Hackathon");

           
            builder.HasKey(e => e.Id);

     
            builder.Property(e => e.Name)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(e => e.CPF)
                .HasColumnType("CHAR(11)") 
                .IsRequired();

            builder.Property(e => e.CRM)
                .HasColumnType("VARCHAR(10)")
                .IsRequired(false);

            builder.Property(e => e.Password)
                .HasColumnType("VARCHAR(255)") 
                .IsRequired();

            builder.Property(e => e.Role)
                .HasConversion(
                    role => role.ToString(),        
                    role => (Role)Enum.Parse(typeof(Role), role) 
                )
                .HasColumnType("VARCHAR(10)")
                .IsRequired();
        }   
    }
}