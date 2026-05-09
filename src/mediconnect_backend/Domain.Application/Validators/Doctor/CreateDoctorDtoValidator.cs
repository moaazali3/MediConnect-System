using FluentValidation;
using Hospital.Application.DTOs.Doctor;

namespace Hospital.Application.Validators.Doctor
{
    public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
    {
        private static readonly string[] ValidGender = { "Male", "Female" };

        public CreateDoctorDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MinimumLength(3).WithMessage("First Name must be at least 3 characters long");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MinimumLength(3).WithMessage("Last Name must be at least 3 characters long");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^(\+20|0)?1[0125][0-9]{8}$")
                .WithMessage("Invalid phone number");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(g => ValidGender.Contains(g))
                .WithMessage("Invalid Gender");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of birth must be in the past");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required");

            RuleFor(x => x.ExperienceYears)
                .NotEmpty().WithMessage("Experience years is required")
                .GreaterThanOrEqualTo(0).WithMessage("Experience years must be greater than or equal to 0");

            RuleFor(x => x.ConsultationFee)
                .NotEmpty().WithMessage("Consultation fee is required")
                .GreaterThanOrEqualTo(0).WithMessage("Consultation fee must be greater than or equal to 0");

            RuleFor(x => x.SpecializationId)
                .NotEmpty().WithMessage("Specialization is required")
                .GreaterThan(0).WithMessage("Specialization must be selected");
        }
    }
}
