using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations
{
    public class ReceptionistConfig : IEntityTypeConfiguration<Receptionist>
    {
        public void Configure(EntityTypeBuilder<Receptionist> builder)
        {
            builder.HasKey(r => r.UserId);

            builder.HasOne(r => r.AppUser)
                .WithOne(u => u.Receptionist)
                .HasForeignKey<Receptionist>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Doctor)
                .WithOne(d => d.Receptionist)
                .HasForeignKey<Receptionist>(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
