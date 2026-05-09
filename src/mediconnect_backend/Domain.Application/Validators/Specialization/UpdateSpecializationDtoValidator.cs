using FluentValidation;
using Hospital.Application.DTOs.Specialization;

namespace Hospital.Application.Validators.Specialization
{
    public class UpdateSpecializationDtoValidator : AbstractValidator<UpdateSpecializationDto>
    {
        public UpdateSpecializationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name must contain only letters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
        }
    }
}
