using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Result<StudentGradesDto>> GetStudentGradesAsync(int studentId);
    }
}
