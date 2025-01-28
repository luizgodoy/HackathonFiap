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
            builder.Property(e => e.Name).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(e => e.Email).HasColumnType("VARCHAR(100)");
            builder.Property(e => e.CPF).HasColumnType("VARHCAR(11)");
            builder.Property(e => e.CRM).HasColumnType("VARCHAR(10)");
            builder.Property(e => e.Password).HasColumnType("VARCHAR(10)");
            builder.Property(e => e.Role).HasColumnType("VARCHAR(10)");
        }   
    }
}