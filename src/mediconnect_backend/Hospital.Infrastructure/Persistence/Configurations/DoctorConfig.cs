using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.UserId);

            builder.HasOne(d => d.AppUser)
                   .WithOne(a => a.Doctor)
                   .HasForeignKey<Doctor>(d => d.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.specialization)
                   .WithMany(s => s.Doctors)
                   .HasForeignKey(d => d.SpecializationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.ExperienceYears)
                   .IsRequired();

            builder.Property(d => d.Biography)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(d => d.ConsultationFee)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(d => d.IsActive)
                   .HasDefaultValue(false);
        }
    }
}
