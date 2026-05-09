using Hospital.Application.DTOs.Payment;

namespace Hospital.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task CreatePayment(Guid appointmentId, CreatePaymentDto model);
        Task<List<PaymentDto>> GetPaymentsByAppointmentId(Guid appointmentId);
    }
}
