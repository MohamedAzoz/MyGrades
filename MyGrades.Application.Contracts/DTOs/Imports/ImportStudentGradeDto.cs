using Microsoft.AspNetCore.Http;

namespace MyGrades.Application.Contracts.DTOs.Imports
{
    public class ImportStudentGradeDto
    {
        public IFormFile File { get; set; }
        public int SubjectId { get; set; } 
    }
}
