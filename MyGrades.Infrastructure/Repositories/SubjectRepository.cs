using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly AppDbContext context;

        public SubjectRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Result<List<SubjectModel>>> FindAllByAssistantIdAsync(int assistantId)
        {
            try
            {
                var subjects = await  context.Subjects.
                     Where(s => s.AssistantId == assistantId).
                    Select(s => new SubjectModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return Result<List<SubjectModel>>.Success(subjects);
            }
            catch (Exception ex)
            {
                return Result<List<SubjectModel>>.Failure($"An error occurred while retrieving subjects: {ex.Message}");
            }
        }

        public async Task<Result<List<SubjectModel>>> GetStudentSubjectsAsync(int studentId)
        {
            var subjects = await context.StudentSubjects
                .Where(s => s.StudentId == studentId)
                .Select(s => new SubjectModel
                {
                    Id = s.SubjectId,
                    Name = s.Subject.Name
                })
                .AsNoTracking()
                .ToListAsync();

            return Result<List<SubjectModel>>.Success(subjects);
        }

        public async Task<Result<List<SubjectModel>>> GetUserSubjectsAsync(Expression<Func<Subject, bool>> predicate)
        {
            var subjects = await context.Subjects
                    .Where(predicate)
                    .Select(s => new SubjectModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();

            return Result<List<SubjectModel>>.Success(subjects);
        }
    }
}
