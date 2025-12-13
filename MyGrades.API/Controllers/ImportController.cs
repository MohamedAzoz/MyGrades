using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.Imports;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        // 1. استخدام الـ Interface
        private readonly IStudentService _studentService;
        private readonly IAssistantService _assistantService;
        private readonly IGradeService gradeService;
        private readonly IDoctorService _doctorService;
        public ImportController(IStudentService studentService, IDoctorService doctorService,
            IAssistantService assistantService,IGradeService _gradeService)
        {
            _studentService = studentService;
            _assistantService = assistantService;
            gradeService = _gradeService;
            _doctorService = doctorService;

        }

        /// <summary>
        /// Imports students from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing student data.</param>
        /// <param name="defaultPassword">The default password for the imported students.</param>
        /// <param name="academicYearId">The academic year ID.</param>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("students/import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportStudents([FromForm] ImportStudentDto importStudent)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (importStudent.File == null || importStudent.File.Length == 0)
                return BadRequest("must attach a file.");

            // التحقق من الامتداد
            var extension = Path.GetExtension(importStudent.File.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = importStudent.File.OpenReadStream();
            var result = await _studentService.ImportStudentsFromExcel(stream, importStudent.AcademicYearId, importStudent.DefaultPassword, importStudent.DepartmentId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, new { Message = result.Message }); // 400 Bad Request مع رسالة الخطأ

            return Ok("Students imported successfully.");
        }

        /// <summary>
        /// Imports assistants from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing assistant data.</param>
        /// <param name="defaultPassword">The default password for the imported assistants.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("assistants/import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportAssistantsFromExcel([FromForm] ImportDto importDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (importDto.File == null || importDto.File.Length == 0)
            {
                return BadRequest("must attach a file.");
            }
            var extension = Path.GetExtension(importDto.File.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = importDto.File.OpenReadStream();
            var result = await _assistantService.ImportAssistantsFromExcel(stream, importDto.DefaultPassword);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, new { Message = result.Message });
             
            return Ok("Assistants imported successfully.");
        }

        /// <summary>
        /// Imports doctors from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing doctor data.</param>
        /// <param name="defaultPassword">The default password for the imported doctors.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("doctors/import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportDoctors([FromForm] ImportDto importDoctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (importDoctor.File == null || importDoctor.File.Length == 0)
                return BadRequest("must attach a file.");

            // التحقق من الامتداد
            var extension = Path.GetExtension(importDoctor.File.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = importDoctor.File.OpenReadStream();

            var result = await _doctorService.ImportDoctorsFromExcel(stream, importDoctor.DefaultPassword);

                if (!result.IsSuccess)
                    return StatusCode(result.StatusCode ?? 400, new { Message = result.Message }); // 400 Bad Request مع رسالة الخطأ
             
            return Ok("Doctors imported successfully.");
        }

        /// <summary>
        /// Imports grades from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing grade data.</param>
        /// <param name="subjectId">The subject ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("grades/import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportGrades([FromForm] ImportStudentGradeDto importDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (importDto.File == null || importDto.File.Length == 0)
                return BadRequest("must attach a file.");

            // التحقق من الامتداد
            var extension = Path.GetExtension(importDto.File.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = importDto.File.OpenReadStream();

            var result = await gradeService.ImportGradesFromExcel(stream, importDto.SubjectId);

                if (!result.IsSuccess)
                    return StatusCode(result.StatusCode ?? 400, new { Message = result.Message }); // 400 Bad Request مع رسالة الخطأ
             
            return Ok("Grades imported successfully.");
        }

    }

}
