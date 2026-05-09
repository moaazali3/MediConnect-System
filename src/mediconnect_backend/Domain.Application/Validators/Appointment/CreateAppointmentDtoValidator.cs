using FluentValidation;
using Hospital.Application.DTOs.Appointment;

namespace Hospital.Application.Validators.Appointment
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {

        private static readonly string[] ValidDays =
        {
            "Sunday", "Monday", "Tuesday", "Wednesday",
            "Thursday", "Friday", "Saturday"
        };

        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.DayOfWeek)
                .NotEmpty()
                .Must(d => ValidDays.Contains(d))
                .WithMessage("Invalid day of week");
        }
    }
}
