using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Helper
{
    public interface IExcelWriter
    {
        public MemoryStream WriteAssistantsTemplateToStream();
        public MemoryStream WriteGradesTemplateToStream(List<UserExcelWriterDto> students);
        public MemoryStream WriteStudentsTemplateToStream(List<UserExcelDto> students);
        public MemoryStream WriteGradesToStream(List<GradeExcelWriterDto> grades);
    }
}
