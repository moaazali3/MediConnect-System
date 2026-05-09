using FluentValidation;
using Hospital.Application.DTOs.Profile;

namespace Hospital.Application.Validators.Profile
{
    public class UpdatePatientProfileDtoValidator : AbstractValidator<UpdatePatientProfileDto>
    {
        private static readonly string[] ValidBloodTypes =
        {
            "A+","A-","B+","B-","AB+","AB-","O+","O-"
        };

        private static readonly string[] ValidGender = { "Male", "Female" };

        public UpdatePatientProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required")
                .LessThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Date of birth must be in the past");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required")
                .MaximumLength(200)
                .WithMessage("Address cannot exceed 200 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^(\+20|0)?1[0125][0-9]{8}$")
                .WithMessage("Invalid phone number");

            RuleFor(x => x.EmergencyContact)
                .NotEmpty().WithMessage("Emergency contact is required")
                .Matches(@"^(\+20|0)?1[0125][0-9]{8}$")
                .WithMessage("Invalid phone number");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(g => ValidGender.Contains(g))
                .WithMessage("Invalid Gender");

            RuleFor(x => x.BloodType)
                .NotEmpty().WithMessage("Blood type is required")
                .Must(bt => ValidBloodTypes.Contains(bt))
                .WithMessage("Invalid blood type");

            RuleFor(x => x.Height)
                .NotEmpty().WithMessage("Height is required")
                .GreaterThan(0).WithMessage("Height must be greater than 0");

            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage("Weight is required")
                .GreaterThan(0).WithMessage("Weight must be greater than 0");
        }
    }
}
