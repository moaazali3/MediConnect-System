using FluentValidation;
using Hospital.Application.DTOs.DoctorSchedule;

namespace Hospital.Application.Validators.DoctorSchedule
{
    public class CreateDoctorScheduleDtoValidator : AbstractValidator<CreateDoctorScheduleDto>
    {
        public CreateDoctorScheduleDtoValidator()
        {
            RuleFor(x => x.DoctorSchedules)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one schedule is required");

            RuleForEach(x => x.DoctorSchedules)
                .SetValidator(new DoctorScheduleDtoValidator());

            RuleFor(x => x.DoctorSchedules)
                .Must(schedules => schedules.Count <= 7)
                .WithMessage("A doctor cannot have more than 7 schedules per week");

            RuleFor(x => x.DoctorSchedules)
                .Must(x => x.Count() == x.Select(d => d.DayOfWeek).Distinct().Count())
                .WithMessage("Duplicate days of the week are not allowed in the schedule");
        }
    }
}
