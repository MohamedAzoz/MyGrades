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
           
            result.Message = "Students imported successfully.";
            return Ok(result);
        }

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
            result.Message = "Assistants imported successfully.";
            return Ok(result);
        }

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
            
                result.Message = "Doctors imported successfully.";
            return Ok(result);
        }

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
            
                result.Message = "Grades imported successfully.";
            return Ok(result);
        }
    }
}
