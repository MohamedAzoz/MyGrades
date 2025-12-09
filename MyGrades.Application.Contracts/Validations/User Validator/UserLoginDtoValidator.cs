using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User;

namespace MyGrades.Application.Contracts.Validations.UserValidator
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(14).WithMessage("National ID must be 14 characters long.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
