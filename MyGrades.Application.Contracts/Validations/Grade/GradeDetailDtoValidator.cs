using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Grade;

namespace MyGrades.Application.Contracts.Validations.Grade
{
    public class GradeDetailDtoValidator : AbstractValidator<GradeDetailDto>
    {
        public GradeDetailDtoValidator()
        {
            RuleFor(x => x.CourseName).NotEmpty().WithMessage("Course name is required.");
            RuleFor(x => x.Attendance).InclusiveBetween(0, 5).WithMessage("Attendance must be between 0 and 5.");
            RuleFor(x => x.Tasks).InclusiveBetween(0, 5).WithMessage("Tasks must be between 0 and 5.");
            RuleFor(x => x.Practical).InclusiveBetween(0, 5).WithMessage("Practical must be between 0 and 5.");
            RuleFor(x => x.TotalScore).InclusiveBetween(0, 20).WithMessage("Total Score must be between 0 and 20.");
        }
    }
}
