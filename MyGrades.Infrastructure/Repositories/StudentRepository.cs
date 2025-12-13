using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Domain.Entities;

namespace MyGrades.Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly AppDbContext context;

        public StudentRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Result<StudentModelData>> GetByNationalIdAsync(string nationalId)
        {
            var student = await context.Students
                .Where(s => s.User!.NationalId == nationalId)
                .Select(s => new StudentModelData
                {
                    Id = s.Id,
                    FullName = s.User.FullName,
                    NationalId = s.User.NationalId,
                    AppUserId = s.User.Id,
                    Department = s.Department!.Name,
                    AcademicLevel = s.AcademicLevel!.LevelName,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return Result<StudentModelData>.Failure("Student not found");
            }

            return Result<StudentModelData>.Success(student);
        }

        public async Task<Result<StudentGradesDto>> GetStudentGradesAsync(int studentId)
        {
            var student = await context.Students
                .Where(s => s.Id == studentId)
                .Select(s => new StudentGradesDto
                {
                    StudentName = s.User!.FullName,
                    Grades = s.StudentSubjects!
                        .Where(ss => ss.Grade != null)
                        .Select(ss => new GradeDetailDto
                        {
                            CourseName = ss.Subject.Name,
                            Attendance = ss.Grade!.Attendance,
                            Tasks = ss.Grade.Tasks,
                            Practical = ss.Grade.Practical,
                            TotalScore = ss.Grade.TotalScore
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
            if (student == null || student.Grades.Count == 0)
            {
                return Result<StudentGradesDto>.Failure("Student not found or no grades available", 404);
            }
           
            return Result<StudentGradesDto>.Success(student);
        }

        public async Task<Result<StudentModelData>> GetStudentWithDetailsAsync(int studentId)
        {
            var student = await context.Students
                .Where(s => s.Id == studentId)
                .Select(s => new StudentModelData
                {
                    Id = s.Id,
                    FullName = s.User!.FullName,
                    NationalId = s.User.NationalId,
                    AppUserId = s.User.Id,
                    Department = s.Department!.Name,
                    AcademicLevel = s.AcademicLevel!.LevelName,
                   
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return Result<StudentModelData>.Failure("Student not found");
            }

            return Result<StudentModelData>.Success(student);
        }
    }
}
