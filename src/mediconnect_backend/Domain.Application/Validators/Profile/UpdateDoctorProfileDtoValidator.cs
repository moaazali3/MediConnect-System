using FluentValidation;
using Hospital.Application.DTOs.Profile;

namespace Hospital.Application.Validators.Profile
{
    public class UpdateDoctorProfileDtoValidator : AbstractValidator<UpdateDoctorProfileDto>
    {
        private static readonly string[] ValidGender = { "Male", "Female" };

        public UpdateDoctorProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of birth must be in the past");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^(\+20|0)?1[0125][0-9]{8}$")
                .WithMessage("Invalid phone number");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(g => ValidGender.Contains(g))
                .WithMessage("Invalid Gender");

            RuleFor(x => x.Biography)
                .NotEmpty().WithMessage("Biography is required")
                .MaximumLength(1000).WithMessage("Biography cannot exceed 1000 characters");
        }
    }
}
