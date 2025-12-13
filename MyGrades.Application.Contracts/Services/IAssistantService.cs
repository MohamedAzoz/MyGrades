using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using MyGrades.Application.Contracts.Projections_Models.User.Assistants;
using MyGrades.Domain.Entities;
using System.Linq.Expressions;

namespace MyGrades.Application.Contracts.Services
{
    public interface IAssistantService
    {
        public Task<Result<AssistantModel>> AddAsync(AssistantExcelDto assistant);
        public Task<Result<List<AssistantModel>>> AddRangeAsync(List<AssistantExcelDto> assistants);
        public Task<Result> UpdateAsync(AssistantModel assistantModel);
        public Task<Result> DeleteAsync(int Id);
        public Task<Result<List<AssistantProjection>>> FindAllAsync(int departmentId);
        public Task<Result<bool>> ImportAssistantsFromExcel(Stream fileStream, string defaultPassword);
        public Task<Result<AssistantModelData>> GetByIdAsync(int id);
        public Task<Result<List<AssistantModel>>> GetAllAsync();
        public Task<Result<AssistantModelData>> GetByNationalIdAsync(string nationalId);

        //public Task<Result<List<SubjectModel>>> GetAllSubjectsAsync(int assistantId);

    }
}
