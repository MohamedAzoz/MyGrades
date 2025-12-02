using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Application.Contracts.Services
{
    public interface ISubjectService
    {
        public Task<Result<Subject>> GetById(int id);
        public Task<Result> Delete(int id);
        public Task<Result> ClearAsync(Expression<Func<Subject, bool>> predicate);
        public Task<Result> Create(SubjectDto subject);
        public Task<Result> CreateRange(List<SubjectDto> subjects);
        public Task<Result<Subject>> Update(UpdateSubjectDto subject);
        public Task<Result<List<Subject>>> UpdateRange(List<UpdateSubjectDto> subjects);
        public Task<Result<List<UpdateSubjectDto>>> FindAllAsync(Expression<Func<Subject, bool>> predicate);

        public Task<Result<List<SubjectDto>>> GetAll();
    }
}
