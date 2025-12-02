using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Contracts.Validations.UserValidator
{
    public class StudentExcelDtoValidator : AbstractValidator<UserExcelDto>
    {
        public StudentExcelDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(150).WithMessage("Full Name must not exceed 150 characters.");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(14).WithMessage("National ID must be 14 characters long.");
        }
    }
}
