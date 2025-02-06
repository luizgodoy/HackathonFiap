using Hackathon.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class UserConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.ToTable("Specialty", "Hackathon");
        builder.Property(e => e.MedicalSpecialty)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(e => e.User)
            .WithMany(x => x.Specialties)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}