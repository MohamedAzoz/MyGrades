using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User;

namespace MyGrades.Application.Contracts.Validations.User_Validator
{
    public class RoleDtoValidator : AbstractValidator<RoleDto>
    {
        public RoleDtoValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .Length(2, 100).WithMessage("Role name must be between 2 and 100 characters.");
        }   
    }
}
