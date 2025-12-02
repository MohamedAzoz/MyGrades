using MyGrades.Application.Contracts.Projections_Models.Departments;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>  
    {
        public Task<Result<List<DepartmentIds>>> getAllIdsAsync();
    }
}
