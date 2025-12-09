using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Application.Contracts.Validations.Grade;

namespace MyGrades.Application.Contracts.Validations.User_Validator.Student
{
    public class StudentGradesDtoValidator : AbstractValidator<StudentGradesDto>
        {
        public StudentGradesDtoValidator()
        {
            RuleFor(x => x.StudentName)
                .NotEmpty().WithMessage("Student name is required.")
                .MaximumLength(100).WithMessage("Student name cannot exceed 100 characters.");
            RuleForEach(x => x.Grades).SetValidator(new GradeDetailDtoValidator());
        }
    }
}
