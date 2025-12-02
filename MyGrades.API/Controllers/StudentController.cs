using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;
        public StudentController(IStudentService _studentService)
        {
            studentService = _studentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent( StudentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.Create(model);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Student created successfully.";
            return Ok(result);
        }

        [HttpGet("byDepartment/{departmentId:int}")]
        public async Task<IActionResult> GetStudentsByDepartment(int departmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.FindAll(x => x.DepartmentId == departmentId,x=>x.AcademicYear ,x => x.User);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Students retrieved successfully.";
            return Ok(result);
        }

        [HttpGet("bySubject/{subjectId:int}")]
        public async Task<IActionResult> GetAllStudentsBySubject(int subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.FindAll(x => x.Department.Subjects.Any(s => s.Id == subjectId),
                x=>x.Department, x => x.User, x => x.Department.Subjects);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Students retrieved successfully.";
            return Ok(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllStudents()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.GetAll();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Students retrieved successfully.";
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.Find(x=>x.Id==id , x => x.User);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Student retrieved successfully.";
            return Ok(result);
        }

        [HttpGet("byNationalId/{nationalId:long}")]
        public async Task<IActionResult> GetStudentByNationalId(long nationalId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.Find(x=>x.User.NationalId==nationalId.ToString(), x => x.User);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Student retrieved successfully.";
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.Delete(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Student deleted successfully.";
            return Ok(result);
        }

        [HttpDelete("byDepartment/{departmentId:int}")]
        public async Task<IActionResult> DeleteStudentsByDepartment(int departmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentService.ClearAsync(x => x.DepartmentId == departmentId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Students deleted successfully.";
            return Ok(result);
        }

    }
}

