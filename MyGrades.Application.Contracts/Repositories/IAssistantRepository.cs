using MyGrades.Application.Contracts.Projections_Models.User.Assistants;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IAssistantRepository : IGenericRepository<Assistant>
    {
        public Task<Result<List<AssistantProjection>>> FindAllByDepartmentIdAsync(int departmentId);
    }
}
