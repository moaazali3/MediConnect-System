using FluentValidation;
using Hospital.Application.DTOs.Payment;

namespace Hospital.Application.Validators.Payment
{
    public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
    {
        private static readonly string[] ValidPaymentMethods = { "Visa", "Wallet", "Cash" };

        public CreatePaymentDtoValidator()
        {
            RuleFor(x => x.PaymentMethod)
                 .NotEmpty().WithMessage("Payment method is required")
                 .Must(g => ValidPaymentMethods.Contains(g))
                 .WithMessage("Invalid payment method. Allowed methods are: Visa, Wallet, Cash");
        }
    }
}
