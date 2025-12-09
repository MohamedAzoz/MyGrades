using FluentValidation;
using MyGrades.Application.Contracts.DTOs.User;

namespace MyGrades.Application.Contracts.Validations.UserValidator
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.NationalId)
               .NotEmpty().WithMessage("يجب ادخال الرقم الوطني")
               .Length(14).WithMessage("يجب ان يكون الرقم الوطني مكون من 14 حرف.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("يجب ادخال كلمة المرور الحالية.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("يجب ان تكون كلمة المرور الحالية مكونة من 6 احرف على الاقل وتحتوي على حرف كبير واحد على الاقل، وحرف صغير واحد على الاقل، ورقم واحد على الاقل، ورمز خاص واحد على الاقل.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("يجب ادخال كلمة المرور الجديدة.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("يجب ان تكون كلمة المرور الجديدة مكونة من 6 احرف على الاقل وتحتوي على حرف كبير واحد على الاقل، وحرف صغير واحد على الاقل، ورقم واحد على الاقل، ورمز خاص واحد على الاقل.");
        
        }
    }   
}
