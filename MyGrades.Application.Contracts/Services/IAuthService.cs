using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Services
{
    public interface IAuthService
    {
        public Task<Result> Add();
        public Task<Result> AddRang();
        public Task<Result> Update();
        public Task<Result> UpdateRang();
        public Task<Result> Delete();
        public Task<Result> DeleteAllRang();
        public Task<Result> getAllUser();
        public Task<Result> Find();
        public Task<Result<AppUser>> Search();

    }
}
