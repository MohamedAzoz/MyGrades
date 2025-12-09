using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;

        public ExportController(IStudentService studentService, IGradeService gradeService)
        {
            _studentService = studentService;
            _gradeService = gradeService;
        }

        /// <summary>
        /// Exports the students template to Excel.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("export-template-students")]
        public async Task<IActionResult> ExportStudentsTemplate(int departmentId)
        {
            var result = await _studentService.ExportStudentsTemplateToExcel(departmentId);
            if (result.IsSuccess && result.Data != null)
            {
                var stream = result.Data;

                var fileName = $"Students_Template_Department_{departmentId}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            return StatusCode(result.StatusCode ?? 400, result.Message);
        }

        /// <summary>
        /// Exports the students grades to Excel.
        /// </summary>
        /// <param name="subjectId">The unique identifier of the subject.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("export-students-grades")]
        public async Task<IActionResult> ExportStudentsGrades(int subjectId)
        {
            var result = await _gradeService.GetAllStudentsGrades(subjectId);
            if (result.IsSuccess && result.Data != null)
            {
                var stream = result.Data;

                var fileName = $"Students_Grades_Subject_{subjectId}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            return StatusCode(result.StatusCode ?? 400, result.Message);
        }

        /// <summary>
        /// Exports the grades template to Excel.
        /// </summary>
        /// <param name="subjectId">The unique identifier of the subject.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("export-grades-template")]
        public async Task<IActionResult> ExportGradesTemplate(int subjectId)
        {
            var result = await _gradeService.ExportGradesToExcel(subjectId);
            if (result.IsSuccess && result.Data != null)
            {
                var stream = result.Data;
                var fileName = $"Grades_Template_Subject_{subjectId}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            return StatusCode(result.StatusCode ?? 400, result.Message);
        }

    }

}
