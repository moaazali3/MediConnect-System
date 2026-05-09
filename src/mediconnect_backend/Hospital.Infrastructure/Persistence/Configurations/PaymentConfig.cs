using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistence.Configurations
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.PaymentId);

            builder.HasOne(p => p.Appointment)
                   .WithOne(a => a.Payment)
                   .HasForeignKey<Payment>(p => p.AppointmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.PaymentMethod)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(p => p.PaymentStatus)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(p => p.CreatedDate)
                   .HasDefaultValueSql("GETDATE()")
                   .IsRequired();
        }
    }
}
