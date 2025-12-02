using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
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
                    SubjectName = g.Subject.Name,
                    StudentFullName = g.Student.User.FullName,
                    StudentNationalId = g.Student.User.NationalId
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
            var grades = await context.Grades.Where(x=>x.SubjectId==subjectId)
               .Select(g => new GradeModel
               {
                   Grade = g,
                   SubjectName = g.Subject.Name,
                   StudentFullName = g.Student.User.FullName,
                   StudentNationalId = g.Student.User.NationalId
               })
               .AsNoTracking()
               .ToListAsync();

            if (grades == null || grades.Count == 0)
            {
                return Result<List<GradeModel>>.Failure("No grades found", 404);
            }

            return Result<List<GradeModel>>.Success(grades);
        }
    }
}
