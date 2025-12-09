using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Application.Contracts.Projections_Models.Grades;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Domain.Entities;

namespace MyGrades.Infrastructure.Repositories
{
    public class GradeRepository : GenericRepository<Grade>, IGradeRepository
    {
        private readonly AppDbContext context;
        public GradeRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<Result<List<GradeModel>>> GetAllAsync()
        {
            var grades = await context.Grades
                .Select(g => new GradeModel
                {
                    Grade = g,
                    SubjectName = g.StudentSubject.Subject.Name,
                    StudentFullName = g.StudentSubject.Student.User!.FullName,
                    StudentNationalId = g.StudentSubject.Student.User!.NationalId
                })
                .AsNoTracking()
                .ToListAsync();

            if (grades == null || grades.Count == 0)
            {
                return Result<List<GradeModel>>.Failure("No grades found", 404);
            }

            return Result<List<GradeModel>>.Success(grades);
        }

        public async Task<Result<List<GradeModel>>> GetAllBySubjectIdAsync(int subjectId)
        {
            var grades = await context.StudentSubjects.
                Where(x => x.Subject.Id == subjectId)
               .Select(g => new GradeModel
               {
                   Grade = g.Grade!,
                   SubjectName = g.Subject.Name,
                   StudentFullName = g.Student.User!.FullName,
                   StudentNationalId = g.Student.User!.NationalId
               })
               .AsNoTracking()
               .ToListAsync();

            if (grades == null || grades.Count == 0)
            {
                return Result<List<GradeModel>>.Failure("No grades found", 404);
            }

            return Result<List<GradeModel>>.Success(grades);
        }

        public async Task<Result<List<GradeExcelWriterDto>>> GetAllStudentsGradesAsync(int subjectId)
        {
            var grades = await context.Grades
                .Where(g => g.StudentSubject.SubjectId == subjectId)
                .Select(g => new GradeExcelWriterDto
                {
                    StudentName = g.StudentSubject.Student.User!.FullName,
                    Attendance = g.Attendance,
                    Tasks = g.Tasks,
                    Practical = g.Practical,
                    TotalScore = g.TotalScore
                })
                .AsNoTracking()
                .ToListAsync();

            if (grades == null || grades.Count == 0)
            {
                return Result<List<GradeExcelWriterDto>>.Failure("No grades found", 404);
            }

            return Result<List<GradeExcelWriterDto>>.Success(grades);
        }

        // xxxxxxxxxxxxxxxxxxxxxxxxxx
        public async Task<Result<List<UserExcelWriterDto>>> GetAllStudentsIdsAsync(int subjectId )
        {
            var subjectsExist = await context.Subjects.AnyAsync(s => s.Id == subjectId);
            
            if (!subjectsExist)
            {
                return Result<List<UserExcelWriterDto>>.Failure("Subject not found", 404);
            }
            var studentIds = await context.StudentSubjects
                .Where(g => g.SubjectId == subjectId)
                .Select(g =>
                        new UserExcelWriterDto
                        {
                            Id = g.Student.Id,
                            FullName = g.Student.User!.FullName
                        })
                         .AsNoTracking()
                        .ToListAsync();

            if (studentIds == null || studentIds.Count == 0)
            {
                return Result<List<UserExcelWriterDto>>.Failure("No students found", 404);
            }

            return Result<List<UserExcelWriterDto>>.Success(studentIds);
        }

    }
}
