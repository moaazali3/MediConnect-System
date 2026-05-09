using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations
{
    public class MedicalRecordConfig : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasKey(mr => mr.MedicalRecordId);

            builder.HasOne(mr => mr.Appointment)
                   .WithOne(a => a.MedicalRecord)
                   .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
