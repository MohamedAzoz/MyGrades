using MyGrades.Application.Contracts.DTOs.User;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Services
{
    public interface IAuthService
    {
        public Task<Result<AppUser>> AddAdminAsync(string name, string nationalId, string Parameter);
        public Task<Result<AuthModelDto>> LoginAsync(UserLoginDto userLoginDto);
        public Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto changePassword);
        public Task<Result> DeleteUser(string userId);

    }
}
