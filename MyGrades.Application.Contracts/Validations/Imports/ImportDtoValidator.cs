using FluentValidation;
using MyGrades.Application.Contracts.DTOs.Imports;

namespace MyGrades.Application.Contracts.Validations.Imports
{
    public class ImportDtoValidator : AbstractValidator<ImportDto>
    {
        public ImportDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");

            RuleFor(x => x.DefaultPassword)
                .NotEmpty()
                .WithMessage("Default password is required.");
        }
    }
}
