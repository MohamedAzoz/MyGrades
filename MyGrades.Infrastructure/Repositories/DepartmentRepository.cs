using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.Projections_Models.Departments;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Domain.Entities;

namespace MyGrades.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly AppDbContext context;

        public DepartmentRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Result<List<DepartmentIds>>> getAllIdsAsync()
        {
            var DepartmentIds =await context.Departments.Select(d =>new DepartmentIds
            {
                Id=d.Id
            })
                .AsNoTracking()
                .ToListAsync();
            if (DepartmentIds==null||DepartmentIds.Count==0)
            {
                return Result<List<DepartmentIds>>.Failure("Not Found",404);
            }
            return Result<List<DepartmentIds>>.Success(DepartmentIds);
        }

       
    }
}
