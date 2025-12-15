using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Helper
{
    public interface IExcelReader
    {
        public Result<List<UserExcelDto>> ReadUsersFromStream(Stream stream);
        public Result<List<AssistantExcelDto>> ReadAssistantFromStream(Stream stream);
        public Result<List<GradeExcelDto>> ReadGradesFromStream(Stream stream);
    }
}
