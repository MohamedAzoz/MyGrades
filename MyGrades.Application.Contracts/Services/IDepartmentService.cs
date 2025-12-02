using MyGrades.Application.Contracts.DTOs.Department;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Services
{
    public interface IDepartmentService
    {
        public Task<Result<Department>> GetById(int id);
        public Task<Result> Delete(int id);
        public Task<Result> Create(DepartmentDto department);
        public Task<Result> CreateRang(List<DepartmentDto> departments);
        public Task<Result<Department>> Update(DepartmentDto department);
        public Task<Result<List<Department>>> GetAll();
    }
}
