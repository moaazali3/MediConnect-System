using FluentValidation;
using Hospital.Application.DTOs.MedicalRecord;

namespace Hospital.Application.Validators.MedicalRecord
{
    public class CreateMedicalRecordDtoValidator : AbstractValidator<CreateMedicalRecordDto>
    {
        public CreateMedicalRecordDtoValidator()
        {
            RuleFor(mr => mr.Prescription)
                .NotEmpty().WithMessage("Prescription is required")
                .MaximumLength(500).WithMessage("Prescription cannot exceed 500 characters");

            RuleFor(mr => mr.Diagnosis)
                .NotEmpty().WithMessage("Diagnosis is required")
                .MaximumLength(500).WithMessage("Diagnosis cannot exceed 500 characters");
        }
    }
}
