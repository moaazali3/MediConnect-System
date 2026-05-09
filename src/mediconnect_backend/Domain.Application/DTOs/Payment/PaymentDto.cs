namespace Hospital.Application.DTOs.Payment
{
    public class PaymentDto
    {
        public Guid PaymentId { get; set; }
        public Guid AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
