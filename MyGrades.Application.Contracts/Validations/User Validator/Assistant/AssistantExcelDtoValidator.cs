using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGrades.Application.Contracts.Validations.User_Validator.Assistant
{
    public class AssistantExcelDtoValidator : AbstractValidator<AssistantExcelDto>
    {
        public AssistantExcelDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .Length(2, 100).WithMessage("Full Name must be between 2 and 100 characters.");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(10, 10).WithMessage("National ID must be 10 characters long.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.");
        }
    }
}
