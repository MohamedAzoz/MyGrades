using MyGrades.Application.Contracts.DTOs.User;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Services
{
    public interface IAuthService
    {
        public Task<Result<AppUser>> AddAdminAsync(AdminDto adminDto);
        public Task<Result<AppUser>> AddAssistantAsync(AdminDto adminDto);
        public Task<Result<AppUser>> AddDoctorAsync(AdminDto adminDto);
        public Task<Result<AppUser>> AddStudentAsync(AdminDto adminDto);
        public Task<Result<AuthModelDto>> LoginAsync(UserLoginDto userLoginDto);
        public Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto changePassword);
        public Task<Result> DeleteUser(string userId);

    }
}
