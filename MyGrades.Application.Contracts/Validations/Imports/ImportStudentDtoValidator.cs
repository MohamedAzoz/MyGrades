using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Imports;

namespace MyGrades.Application.Contracts.Validations.Imports
{
    public class ImportStudentDtoValidator : AbstractValidator<ImportStudentDto>
    {
        public ImportStudentDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");
            RuleFor(x => x.DefaultPassword)
                .NotEmpty()
                .WithMessage("Default password is required.");
            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0)
                .WithMessage("Academic Year Id must be greater than zero.");
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Department Id must be greater than zero.");
        }
    }
}
