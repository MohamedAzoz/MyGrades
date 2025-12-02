using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Grade;

namespace MyGrades.Application.Contracts.Validations.Grade
{
    public class GradeExcelWriterDtoValidator : AbstractValidator<GradeExcelWriterDto>
    {
        public GradeExcelWriterDtoValidator()
        {
            RuleFor(x => x.StudentName).NotEmpty().WithMessage("Student Name is required.");
            RuleFor(x => x.SubjectName).NotEmpty().WithMessage("Subject Name is required.");
            RuleFor(x => x.GradeValue).InclusiveBetween(0, 100).WithMessage("Grade must be between 0 and 100.");
        }
    }
}
