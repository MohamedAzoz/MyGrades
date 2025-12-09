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

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="model">The student creation model.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
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
            return Created();
        }

        /// <summary>
        /// Retrieves all students in a specific department.
        /// </summary>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("byDepartment/{departmentId:int}")]
        public async Task<IActionResult> GetStudentsByDepartment(int departmentId)
        { 
            var result = await studentService.FindAll(x => x.DepartmentId == departmentId,x=>x.AcademicLevel ,x => x.User);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
             
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves all students in a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("bySubject/{subjectId:int}")]
        public async Task<IActionResult> GetAllStudentsBySubject(int subjectId)
        { 
            var result = await studentService.FindAll(x => x.Department.Subjects.Any(s => s.Id == subjectId),
                x=>x.Department, x => x.User, x => x.Department.Subjects);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves all students.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        { 
            var result = await studentService.GetAll();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves a student by their ID.
        /// </summary>
        /// <param name="id">The student ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await studentService.Find(x=>x.Id==id , x => x.User);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves a student by their National ID.
        /// </summary>
        /// <param name="nationalId">The student's National ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("byNationalId/{nationalId:long}")]
        public async Task<IActionResult> GetStudentByNationalId(long nationalId)
        {
            var result = await studentService.Find(x=>x.User.NationalId==nationalId.ToString(), x => x.User);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            return Ok(result.Data);
        }

        /// <summary>
        /// Deletes a student by their ID.
        /// </summary>
        /// <param name="id">The student ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await studentService.Delete(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);

            return NoContent();
        }

        /// <summary>
        /// Deletes all students in a specific department.
        /// </summary>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("byDepartment/{departmentId:int}")]
        public async Task<IActionResult> DeleteStudentsByDepartment(int departmentId)
        {
            var result = await studentService.ClearAsync(x => x.DepartmentId == departmentId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return NoContent();
        }

        /// <summary>
        /// Retrieves the grades of a student.
        /// </summary>
        /// <param name="studentId">The student ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("GetStudentGradesAsync/{studentId:int}")]
        public async Task<IActionResult> GetStudentGradesAsync(int studentId)
        {
            var result = await studentService.GetStudentGradesAsync(studentId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves the subjects of a student.
        /// </summary>
        /// <param name="studentId">The student ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("GetStudentSubjectsAsync/{studentId:int}")]
        public async Task<IActionResult> GetStudentSubjectsAsync(int studentId)
        {
            var result = await studentService.GetStudentSubjectsAsync(studentId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            return Ok(result.Data);
        }

    }


}

