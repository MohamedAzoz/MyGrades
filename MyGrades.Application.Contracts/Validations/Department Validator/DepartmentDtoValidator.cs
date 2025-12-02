using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Department;

namespace MyGrades.Application.Contracts.Validations.DepartmentValidator
{
    public class DepartmentDtoValidator : AbstractValidator<DepartmentDto>
    {
        public DepartmentDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
