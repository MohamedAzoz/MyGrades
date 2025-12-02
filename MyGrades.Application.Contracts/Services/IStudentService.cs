using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace MyGrades.Application.Contracts.Services
{
    public interface IStudentService
    {
        public Task<Result<StudentModel>> GetById(int id);
        public Task<Result> Delete(int id);
        public Task<Result> Create(StudentCreateModel student);
        public Task<Result<StudentModel>> Find(Expression<Func<Student, bool>> expression, params Expression<Func<Student, object>>[] includes);
        public Task<Result<List<StudentModel>>> FindAll(Expression<Func<Student, bool>> expression, params Expression<Func<Student, object>>[] includes);
        public Task<Result> CreateRang(List<StudentCreateModel> students);
        public Task<Result<bool>> ImportStudentsFromExcel(Stream fileStream, int academicYearId, string defaultPassword,
            int departmentId);
        public Task<Result<Stream>> ExportStudentsTemplateToExcel(int departmentId);
        public Task<Result> Update(StudentModel student);
        public Task<Result> UpdateRang(List<StudentModel> students);
        public Task<Result<List<StudentModel>>> GetAll();
        public Task<Result> ClearAsync(Expression<Func<Student, bool>> expression);
    }
}
