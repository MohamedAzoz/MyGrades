using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Subject;

namespace MyGrades.Application.Contracts.Validations.Subject
{
    public class UpdateSubjectDtoValidator : AbstractValidator<UpdateSubjectDto>
    {
        public UpdateSubjectDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId is required.");
            RuleFor(x => x.DoctorId).NotEmpty().WithMessage("DoctorId is required.");
            RuleFor(x => x.AssistantId).NotEmpty().WithMessage("AssistantId is required.");
            RuleFor(x => x.AcademicLevelId).NotEmpty().WithMessage("AcademicYearId is required.");
        }
    }
}
