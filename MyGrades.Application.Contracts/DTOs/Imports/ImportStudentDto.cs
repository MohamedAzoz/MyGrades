using Microsoft.AspNetCore.Http;

namespace MyGrades.Application.Contracts.DTOs.Imports
{
    public class ImportStudentDto
    {
        public IFormFile File { get; set; }
        public string DefaultPassword { get; set; }
        public int AcademicYearId { get; set; }
        public int DepartmentId { get; set; }
    }
}
