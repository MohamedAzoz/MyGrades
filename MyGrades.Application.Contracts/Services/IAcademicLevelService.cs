using MyGrades.Application.Contracts.DTOs.AcademicYear;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Application.Contracts.Services
{
    public interface IAcademicLevelService
    {
        public Task<Result> AddAsync (AcademicLevelDto year);
        //public Task<Result<List<AcademicLevel>>> AddRangeAsync(List<AcademicLevelDto> yearList); 
       public Task<Result<AcademicLevel>> GetLevelAsync(int year);
        public Task<Result<List<AcademicLevel>>> GetAll();
        public Task<Result> UpdateAsync ( AcademicLevelDto year);
        public Task<Result> DeleteAsync ( int year);
        public Task<Result> ClearAsync(Expression<Func<AcademicLevel,bool>> expression);

    }
}
