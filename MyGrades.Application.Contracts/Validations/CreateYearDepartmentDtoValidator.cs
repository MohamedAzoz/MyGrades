using FluentValidation;
using MyGrades.Application.Contracts.DTOs;

namespace MyGrades.Application.Contracts.Validations
{
    public class CreateYearDepartmentDtoValidator : AbstractValidator<CreateYearDepartmentDto>  
    {
        public CreateYearDepartmentDtoValidator() { 
            RuleFor(x => x.YearId).NotEmpty().WithMessage("YearId is required.");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId is required.");
        }
    }
}
