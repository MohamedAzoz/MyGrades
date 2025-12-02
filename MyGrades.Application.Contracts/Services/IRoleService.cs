using MyGrades.Application.Contracts.DTOs.User;

namespace MyGrades.Application.Contracts.Services
{
    public interface IRoleService
    {
        public Task<Result> AddRole(RoleDto roleDto);
        public Task<Result> DeleteRole(string roleName);
        public Task<Result> UpdateRole(UpdateRoleDto roleDto);
    }
}
