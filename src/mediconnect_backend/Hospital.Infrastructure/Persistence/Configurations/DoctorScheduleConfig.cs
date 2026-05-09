using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations
{
    public class DoctorScheduleConfig : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(ds => ds.ScheduleId);

            builder.HasOne(ds => ds.Doctor)
                   .WithMany(d => d.Schedules)
                   .HasForeignKey(ds => ds.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ds => ds.DayOfWeek)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(ds => ds.IsAvailable)
                .HasDefaultValue(true);
        }
    }
}
