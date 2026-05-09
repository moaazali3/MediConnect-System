using FluentValidation;
using Hospital.Application.DTOs.Profile;

namespace Hospital.Application.Validators.Profile
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required");
        }
    }
}
