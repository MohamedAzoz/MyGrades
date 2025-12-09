using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Contracts.Validations.User_Validator.Student
{
    public class StudentCreateModelValidator : AbstractValidator<StudentCreateModel>
    {
        public StudentCreateModelValidator()
        {
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(14).WithMessage("National ID must be exactly 14 characters long.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be a positive integer.");
            RuleFor(x => x.AcademicLevelId)
                .GreaterThan(0).WithMessage("Academic Level ID must be a positive integer.");
        }
    }
}
