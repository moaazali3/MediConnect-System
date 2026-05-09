using FluentValidation;
using Hospital.Application.DTOs.Payment;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Repositories.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreatePaymentDto> _createPaymentValidator;

        public PaymentService(IUnitOfWork unitOfWork, IValidator<CreatePaymentDto> createPaymentValidator)
        {
            _unitOfWork = unitOfWork;
            _createPaymentValidator = createPaymentValidator;
        }

        public async Task CreatePayment(Guid appointmentId, CreatePaymentDto model)
        {
            var result = _createPaymentValidator.Validate(model);

            if (!result.IsValid)
                throw new ValidationException(result.ToString(","));

            var appointment = await _unitOfWork.Appointments.FindByIdAsync(appointmentId);

            var ConsultationFee = await _unitOfWork.Appointments.GetAsync(
                filter: a => a.AppointmentId == appointmentId,
                selector: a => a.Doctor.ConsultationFee
                );

            if (appointment == null)
                throw new Exception("Appointment not found");

            var payment = new Payment
            {
                AppointmentId = appointmentId,
                PaymentMethod = Enum.Parse<PaymentMethod>(model.PaymentMethod),
                Amount = ConsultationFee,
            };

            if (payment.PaymentMethod == PaymentMethod.Visa || payment.PaymentMethod == PaymentMethod.Wallet)
            {
                payment.PaymentStatus = PaymentStatus.Completed;
            }
            else
            {
                payment.PaymentStatus = PaymentStatus.Pending;
            }

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<PaymentDto>> GetPaymentsByAppointmentId(Guid appointmentId)
        {
            var payments = await _unitOfWork.Payments
                .GetAllAsync(
                filter: p => p.AppointmentId == appointmentId,
                selector: p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    AppointmentId = p.AppointmentId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod.ToString(),
                    PaymentStatus = p.PaymentStatus.ToString(),
                    CreatedDate = p.CreatedDate
                }
                );

            return payments;
        }
    }
}
