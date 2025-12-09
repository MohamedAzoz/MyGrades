using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Application.Contracts.Projections_Models.Grades;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IGradeRepository : IGenericRepository<Grade>
    {
        public Task<Result<List<GradeModel>>> GetAllAsync();
        public Task<Result<List<GradeModel>>> GetAllBySubjectIdAsync(int subjectId);
        public Task<Result<List<GradeExcelWriterDto>>> GetAllStudentsGradesAsync(int subjectId);
        public Task<Result<List<UserExcelWriterDto>>> GetAllStudentsIdsAsync(int subjectId);

    }
}
