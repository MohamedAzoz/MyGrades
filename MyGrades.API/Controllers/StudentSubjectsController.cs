using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubjectsController : ControllerBase
    {
        private readonly IStudentSubjectService studentSubjectService;
        public StudentSubjectsController(IStudentSubjectService studentSubjectService)
        {
            this.studentSubjectService = studentSubjectService;
        }

        /// <summary>
        /// Enrolls students in subjects.
        /// </summary>
        /// <param name="createYearDepartment">The year and department information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("EnrollStudentsInSubjects")]
        public async Task<IActionResult> EnrollStudentsInSubjects([FromBody] CreateYearDepartmentDto createYearDepartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentSubjectService.EnrollStudentsInSubjects(createYearDepartment);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        /// <summary>
        /// Adds a range of student subjects.
        /// </summary>
        /// <param name="createStudentSubjects">The list of student subjects to add.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("AddRangeStudentSubject")]
        public async Task<IActionResult> AddRangeStudentSubject([FromBody] List<CreateStudentSubjectDto> createStudentSubjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentSubjectService.AddRangeStudentSubject(createStudentSubjects);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        /// <summary>
        /// Creates a new student subject.
        /// </summary>
        /// <param name="createStudentSubject">The student subject information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("CreateStudentSubject")]
        public async Task<IActionResult> CreateStudentSubject([FromBody] CreateStudentSubjectDto createStudentSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentSubjectService.CreateStudentSubject(createStudentSubject);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        /// <summary>
        /// Deletes a student subject.
        /// </summary>
        /// <param name="createStudentSubject">The student subject information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("DeleteStudentSubject")]
        public async Task<IActionResult> DeleteStudentSubject([FromBody] CreateStudentSubjectDto createStudentSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await studentSubjectService.DeleteStudentSubject(createStudentSubject);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

    }

}
