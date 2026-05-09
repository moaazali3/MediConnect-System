using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public Appointment Appointment { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Amount { get; set; }
    }
}
