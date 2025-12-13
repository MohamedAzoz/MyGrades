using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Imports;

namespace MyGrades.Application.Contracts.Validations.Imports
{
    public class ImportStudentGradeDtoValidator : AbstractValidator<ImportStudentGradeDto>
    {
        public ImportStudentGradeDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");
            RuleFor(x => x.SubjectId)
                .GreaterThan(0)
                .WithMessage("SubjectId must be greater than zero.");
        }
    }
}
