using FluentValidation;
using MyGrades.Application.Contracts.DTOs.AcademicYear;

namespace MyGrades.Application.Contracts.Validations.AcademicYearValidator
{
    public class AcademicYearDtoValidator : AbstractValidator<AcademicLevelDto>
    {
        public AcademicYearDtoValidator()
        {
            RuleFor(x => x.LevelName)
                .NotEmpty().WithMessage("Year Name is required.")
                .MaximumLength(100).WithMessage("Year Name must not exceed 100 characters.");
        }
    }
}
