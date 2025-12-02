using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Assistant;

namespace MyGrades.Application.Contracts.Validations.User_Validator.Assistant
{
    public class AssistantModelValidator : AbstractValidator<AssistantModel>
    {
        public AssistantModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a positive integer.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .Length(2, 100).WithMessage("Full Name must be between 2 and 100 characters.");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(10, 10).WithMessage("National ID must be 10 characters long.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be a positive integer.");
        }
    }
}
