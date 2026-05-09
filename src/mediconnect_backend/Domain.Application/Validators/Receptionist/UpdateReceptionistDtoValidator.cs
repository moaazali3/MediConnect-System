using FluentValidation;
using Hospital.Application.DTOs.Receptionist;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.Validators.Receptionist
{
    public class UpdateReceptionistDtoValidator : AbstractValidator<UpdateReceptionistDto>
    {
        private static readonly string[] ValidGender = { "Male", "Female" };

        public UpdateReceptionistDtoValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

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

            RuleFor(r => r.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
        }
    }
}
