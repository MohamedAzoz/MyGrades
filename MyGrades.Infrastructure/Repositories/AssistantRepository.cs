using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.Projections_Models.User.Assistants;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Domain.Entities;

namespace MyGrades.Infrastructure.Repositories
{
    public class AssistantRepository : GenericRepository<Assistant>, IAssistantRepository
    {
        private readonly AppDbContext context;

        public AssistantRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Result<List<AssistantProjection>>> FindAllByDepartmentIdAsync(int departmentId)
        {
            var assistants = await context.Assistants
                .Where(a => a.DepartmentId == departmentId)
                .Select(a => new AssistantProjection
                {
                    Assistant = a,
                    NationalId = a.User.NationalId,
                    FullName = a.User.FullName
                })
                .AsNoTracking()
                .ToListAsync();
            if (assistants == null || !assistants.Any())
            {
                return Result<List<AssistantProjection>>.Failure("No assistants found.", 404);
            }

            return Result<List<AssistantProjection>>.Success(assistants);
        }
    }
}
