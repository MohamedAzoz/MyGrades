using MyGrades.Application.Contracts.DTOs;

namespace MyGrades.Application.Contracts.Services
{
    public interface IStudentSubjectService
    {
        Task<Result> EnrollStudentsInSubjects(CreateYearDepartmentDto createYearDepartment);
        Task<Result> CreateStudentSubject(CreateStudentSubjectDto createStudentSubject);
        Task<Result> AddRangeStudentSubject(List<CreateStudentSubjectDto> createStudentSubjects);
        Task<Result> DeleteStudentSubject(CreateStudentSubjectDto createStudentSubject);
    }
}
