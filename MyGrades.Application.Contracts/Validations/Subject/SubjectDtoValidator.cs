using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Subject;

namespace MyGrades.Application.Contracts.Validations.Subject
{
    public class SubjectDtoValidator : AbstractValidator<SubjectDto>
    {
        // Validation logic for SubjectDto can be implemented here
        public SubjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("DepartmentId is required.");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("DoctorId is required.");

            RuleFor(x => x.AssistantId)
                .NotEmpty().WithMessage("AssistantId is required.");

            RuleFor(x => x.AcademicLevelId)
                .NotEmpty().WithMessage("AcademicYearId is required.");
        }
    }
}
