using Microsoft.AspNetCore.Http;

namespace MyGrades.Application.Contracts.DTOs.Imports
{
    public class ImportDto
    {
        public IFormFile File { get; set; }
        public string DefaultPassword { get; set; }
    }
}
