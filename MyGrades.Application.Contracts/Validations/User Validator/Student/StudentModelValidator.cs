using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Contracts.Validations.User_Validator.Student
{
    public class StudentModelValidator : AbstractValidator<StudentModel>
    {
        public StudentModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

            RuleFor(x => x.AppUserId)
                .NotEmpty().WithMessage("AppUserId is required.")
                .MaximumLength(100).WithMessage("AppUserId must not exceed 100 characters.");

            RuleFor(x => x.AcademicYearId)
                .NotEmpty().WithMessage("AcademicYearId is required.")
                .WithMessage("AcademicYearId must not exceed 100 characters.");
            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("DepartmentId is required.")
                .WithMessage("DepartmentId must not exceed 100 characters.");
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(14).WithMessage("National ID must be exactly 14 characters long.")
                .Matches("^[0-9]{14}$").WithMessage("National ID must contain only numeric characters.");

        }
    }
}
