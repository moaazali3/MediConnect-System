using FluentValidation;
using Hospital.Application.DTOs.Receptionist;

namespace Hospital.Application.Validators.Receptionist
{
    public class CreateReceptionistDtoValidator : AbstractValidator<CreateReceptionistDto>
    {
        private static readonly string[] ValidGender = { "Male", "Female" };

        public CreateReceptionistDtoValidator()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Doctor ID is required");

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
        }
    }
}
