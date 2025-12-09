using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.AcademicYear;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Application.Services 
{
    public class AcademicLevelService : IAcademicLevelService
    {
        private readonly IUnitOfWork unitOfWork;

        public AcademicLevelService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task<Result> AddAsync(AcademicLevelDto level)
        {
            if (level == null) {
                return Result.Failure("Academic level cannot be null.", 400);
            }
            AcademicLevel newLevel = new AcademicLevel
            {
                LevelName = level.LevelName
            };
            var result = await unitOfWork.AcademicLevels.AddAsync(newLevel);
            if (!result.IsSuccess)
                return Result.Failure("Failed to add academic level.", result.StatusCode ?? 400);

            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

   
        public async Task<Result> ClearAsync(Expression<Func<AcademicLevel, bool>> expression)
        {
            var result = await unitOfWork.AcademicLevels.ClearAsync(expression);
            if (!result.IsSuccess)
            {
                return Result.Failure("Failed to clear academic levels.", result.StatusCode ?? 400);
            }
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int levelId)
        {
            var existingLevelResult = await unitOfWork.AcademicLevels.GetByIdAsync(levelId);
            if (!existingLevelResult.IsSuccess || existingLevelResult.Data == null)
            {
                return Result.Failure("Academic level not found.", 404);
            }
            var deleteResult = await unitOfWork.AcademicLevels.DeleteAsync(existingLevelResult.Data);
            if (!deleteResult.IsSuccess)
            {
                return Result.Failure("Failed to delete academic level.", deleteResult.StatusCode ?? 400);
            }
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<List<AcademicLevel>>> GetAll()
        {
            var result = await unitOfWork.AcademicLevels.GetAllAsync();
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<List<AcademicLevel>>.Failure("Failed to retrieve academic levels.", result.StatusCode ?? 400);
            }
            var dtoList = result.Data.ToList();
            return Result<List<AcademicLevel>>.Success(dtoList);
        }

        public async Task<Result<AcademicLevel>> GetLevelAsync(int year)
        {
            var result = await unitOfWork.AcademicLevels.GetByIdAsync(year);
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<AcademicLevel>.Failure("Failed to retrieve academic level.", result.StatusCode ?? 400);
            }
            return Result<AcademicLevel>.Success(result.Data);
        }

        public async Task<Result> UpdateAsync(AcademicLevelDto level)
        {
            //var existingLevelResult = await unitOfWork.AcademicLevels.GetByIdAsync(level.LevelId);
            //if (!existingLevelResult.IsSuccess || existingLevelResult.Data == null)
            //{
            //    return Result.Failure("Academic level not found.", 404);
            //}
            //existingLevelResult.Data.LevelName = level.LevelName;
            //var updateResult = await unitOfWork.AcademicLevels.UpdateAsync(existingLevelResult.Data);
            //if (!updateResult.IsSuccess)
            //{
            //    return Result.Failure("Failed to update academic level.", updateResult.StatusCode ?? 400);
            //}
            //await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
