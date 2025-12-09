using FluentValidation;
using MyGrades.Application.Contracts.DTOs;

namespace MyGrades.Application.Contracts.Validations
{
    public class CreateStudentSubjectDtoValidator : AbstractValidator<CreateStudentSubjectDto>  
    {
        public CreateStudentSubjectDtoValidator() { 
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("StudentId is required.");
            RuleFor(x => x.SubjectId).NotEmpty().WithMessage("SubjectId is required.");
        }
    }
}
