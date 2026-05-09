using FluentValidation;
using Hospital.Application.DTOs.Authentication;

namespace Hospital.Application.Validators.Authentication
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        private static readonly string[] ValidBloodTypes =
        {
            "A+","A-","B+","B-","AB+","AB-","O+","O-"
        };

        private static readonly string[] ValidGender = { "Male", "Female" };

        public UserRegisterDtoValidator()
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

            RuleFor(x => x.EmergencyContact)
                .NotEmpty().WithMessage("Emergency contact is required")
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

            RuleFor(x => x.Height)
                .NotEmpty().WithMessage("Height is required")
                .GreaterThan(0).WithMessage("Height must be greater than 0");

            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage("Weight is required")
                .GreaterThan(0).WithMessage("Weight must be greater than 0");

            RuleFor(x => x.BloodType)
                .NotEmpty().WithMessage("Blood type is required")
                .Must(bt => ValidBloodTypes.Contains(bt))
                .WithMessage("Invalid blood type");
        }
    }
}
