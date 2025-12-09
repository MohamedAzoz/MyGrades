using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Contracts.Validations.User_Validator.Student
{
    public class UserExcelWriterDtoValidator : AbstractValidator<UserExcelWriterDto>
    {
        public UserExcelWriterDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");
        }
    }
}
