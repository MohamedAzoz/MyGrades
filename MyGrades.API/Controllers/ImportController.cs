using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> ImportStudents(
             IFormFile file,
             string defaultPassword,
             int academicYearId,
            int departmentId)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (file == null || file.Length == 0)
                return BadRequest("must attach a file.");

            // التحقق من الامتداد
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = file.OpenReadStream();
            var result = await _studentService.ImportStudentsFromExcel(stream, academicYearId, defaultPassword, departmentId);
                if (!result.IsSuccess)
                    return StatusCode(result.StatusCode ?? 400, result.Message); // 400 Bad Request مع رسالة الخطأ
            
            return Ok("Students imported successfully.");
        }

        /// <summary>
        /// Imports assistants from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing assistant data.</param>
        /// <param name="defaultPassword">The default password for the imported assistants.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("assistants/import")]
        public async Task<IActionResult> ImportAssistantsFromExcel(IFormFile file, string defaultPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest("must attach a file.");
            }
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = file.OpenReadStream();
            var result = await _assistantService.ImportAssistantsFromExcel(stream, defaultPassword);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
             
            return Ok("Assistants imported successfully.");
        }

        /// <summary>
        /// Imports doctors from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing doctor data.</param>
        /// <param name="defaultPassword">The default password for the imported doctors.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("doctors/import")]
        public async Task<IActionResult> ImportDoctors(
             IFormFile file,
             string defaultPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (file == null || file.Length == 0)
                return BadRequest("must attach a file.");

            // التحقق من الامتداد
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = file.OpenReadStream();
            
                var result = await _doctorService.ImportDoctorsFromExcel(stream, defaultPassword);

                if (!result.IsSuccess)
                    return StatusCode(result.StatusCode ?? 400, result.Message); // 400 Bad Request مع رسالة الخطأ
             
            return Ok("Doctors imported successfully.");
        }

        /// <summary>
        /// Imports grades from an Excel file.
        /// </summary>
        /// <param name="file">The Excel file containing grade data.</param>
        /// <param name="subjectId">The subject ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("grades/import")]
        public async Task<IActionResult> ImportGrades(
            IFormFile file,
            int subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (file == null || file.Length == 0)
                return BadRequest("must attach a file.");

            // التحقق من الامتداد
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                return BadRequest("must be .xlsx or .xls or .csv");

            using var stream = file.OpenReadStream();
            
                var result = await gradeService.ImportGradesFromExcel(stream, subjectId);

                if (!result.IsSuccess)
                    return StatusCode(result.StatusCode ?? 400, result.Message); // 400 Bad Request مع رسالة الخطأ
             
            return Ok("Grades imported successfully.");
        }

    }

}
