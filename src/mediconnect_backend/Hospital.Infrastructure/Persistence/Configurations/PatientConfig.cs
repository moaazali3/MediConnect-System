using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.UserId);

            builder.HasOne(p => p.AppUser)
                   .WithOne(a => a.Patient)
                   .HasForeignKey<Patient>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.BloodType)
                   .IsRequired();

            builder.Property(p => p.Height)
                   .HasColumnType("decimal(5, 2)")
                   .IsRequired();

            builder.Property(p => p.Weight)
                   .HasColumnType("decimal(5, 2)")
                   .IsRequired();

            builder.Property(p => p.EmergencyContact)
                   .IsRequired();
        }
    }
}
