using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User.Doctor;

namespace MyGrades.Application.Contracts.Validations.User_Validator.Doctor
{
    public class DoctorCreateModelValidator : AbstractValidator<DoctorCreateModel>
    {
        public DoctorCreateModelValidator()
        {
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(14).WithMessage("National ID must be 14 characters long.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(150).WithMessage("Full Name must not exceed 150 characters.");
        }
    }
}
