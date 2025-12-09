using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.Projections_Models.Grades;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Services
{
    public interface IGradeService
    {
        public Task<Result> Delete(int id);
        public Task<Result> Create(Grade grade);
        public Task<Result> CreateRang(List<Grade> grades);
        public Task<Result<Grade>> Update(Grade grade);
        public Task<Result<bool>> ImportGradesFromExcel(Stream fileStream, int subjectId);
        public Task<Result<Stream>> ExportGradesToExcel(int subjectId);
        public Task<Result<List<Grade>>> UpdateRang(List<Grade> grades);
        
        public Task<Result<List<GradeModel>>> GetAll(int subjectId);

        public Task<Result<Stream>> GetAllStudentsGrades(int subjectId);
        
    }
}
