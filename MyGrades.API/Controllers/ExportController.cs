using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public ExportController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("export-template")]
        public async Task<IActionResult> ExportStudentsTemplate(int departmentId)
        {
            var result = await _studentService.ExportStudentsTemplateToExcel(departmentId);
            if (result.IsSuccess && result.Data != null)
            {
                var stream = result.Data;

                var fileName = $"Students_Template_Department_{departmentId}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            return BadRequest(result.Message);
        }
    }
}
