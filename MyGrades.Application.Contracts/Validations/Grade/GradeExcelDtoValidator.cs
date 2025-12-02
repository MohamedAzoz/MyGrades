using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Grade;

namespace MyGrades.Application.Contracts.Validations.Grade
{
    public class GradeExcelDtoValidator : AbstractValidator<GradeExcelDto>
    {
        public GradeExcelDtoValidator()
        {
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("Student ID is required.");
            RuleFor(x => x.Attendance).InclusiveBetween(0, 100).WithMessage("Attendance must be between 0 and 100.");
            RuleFor(x => x.Tasks).InclusiveBetween(0, 100).WithMessage("Tasks must be between 0 and 100.");
            RuleFor(x => x.Practical).InclusiveBetween(0, 100).WithMessage("Practical must be between 0 and 100.");
        }
    }
}
