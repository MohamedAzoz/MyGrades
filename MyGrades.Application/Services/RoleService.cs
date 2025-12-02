using Microsoft.AspNetCore.Identity;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.User;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleService(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        public async Task<Result> AddRole(RoleDto roleDto)
        {
            IdentityRole role = new IdentityRole();
            role.Name = roleDto.Name;
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                string Error = "Role";
                foreach (var item in result.Errors)
                {
                    Error += item.Description;
                }
                return Result.Failure(Error);
            }
            return Result.Success();
        }

        public async Task<Result> DeleteRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return Result.Failure("Role not found");
            }

            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                string Error = "Role";
                foreach (var item in result.Errors)
                {
                    Error += item.Description;
                }
                return Result.Failure(Error);
            }
            return Result.Success();
        }

        public async Task<Result> UpdateRole(UpdateRoleDto roleDto)
        {
            var role = await roleManager.FindByNameAsync(roleDto.OldName);
            if (role == null)
            {
                return Result.Failure("Role not found");
            }

            role.Name = roleDto.NewName;
            var result = await roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                string Error = "Role";
                foreach (var item in result.Errors)
                {
                    Error += item.Description;
                }
                return Result.Failure(Error);
            }
            return Result.Success();
        }
    }
}
