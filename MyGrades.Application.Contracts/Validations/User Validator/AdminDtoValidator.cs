using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User;

namespace MyGrades.Application.Contracts.Validations.UserValidator
{
    public class AdminDtoValidator: AbstractValidator<AdminDto>
    {
        public AdminDtoValidator() { 
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("يجب ادخال الاسم الكامل.")
                .MaximumLength(100).WithMessage("يجب ان لا يتجاوز الاسم الكامل 100 حرف.");
            RuleFor(x => x.NationalId)  
                .NotEmpty().WithMessage("يجب ادخال الرقم الوطني")
                .Length(14).WithMessage("يجب ان يكون الرقم الوطني مكون من 14 حرف.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("يجب ادخال كلمة المرور.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("يجب ان تكون كلمة المرور مكونة من 6 احرف على الاقل وتحتوي على حرف كبير واحد على الاقل، وحرف صغير واحد على الاقل، ورقم واحد على الاقل، ورمز خاص واحد على الاقل.");
        }
    }
}
