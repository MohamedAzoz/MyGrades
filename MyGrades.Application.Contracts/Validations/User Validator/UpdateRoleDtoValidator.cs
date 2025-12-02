using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User;

namespace MyGrades.Application.Contracts.Validations.User_Validator
{
    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            RuleFor(x => x.OldName)
                .NotEmpty().WithMessage("Old role name is required.");

            RuleFor(x => x.NewName)
                .NotEmpty().WithMessage("New role name is required.");
        }
    }
}
