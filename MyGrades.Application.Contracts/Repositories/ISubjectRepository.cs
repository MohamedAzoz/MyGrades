using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public Task<Result<List<SubjectModel>>> FindAllByAssistantIdAsync(int assistantId);

        public Task<Result<List<SubjectModel>>> GetStudentSubjectsAsync(int studentId);
        public Task<Result<List<SubjectModel>>> GetUserSubjectsAsync(Expression<Func<Subject, bool>> predicate);


    }
}
