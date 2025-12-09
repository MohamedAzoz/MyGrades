using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        /// <summary>
        /// Adds a new subject.
        /// </summary>
        /// <param name="subjectDto">The subject information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddSubject([FromBody] SubjectDto subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
            var result = await subjectService.Create(subjectDto);
            if (result.IsSuccess)
            {
                return Created();
            }

            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        /// <summary>
        /// Retrieves a subject by its ID.
        /// </summary>
        /// <param name="id">The subject ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            var result = await subjectService.GetById(id);
            if (result.IsSuccess)
            { 
                return Ok(result.Data);
            }

            return StatusCode(result.StatusCode ?? 404, result.Message);
        }

        /// <summary>
        /// Retrieves all subjects in a specific department.
        /// </summary>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("byDepartment/{departmentId}")]
        public async Task<IActionResult> GetSubjectsByDepartment(int departmentId)
        {
            var result = await subjectService.FindAllAsync(x => x.DepartmentId == departmentId);
            if (result.IsSuccess)
            { 
                return Ok(result.Data);
            }

            return StatusCode(result.StatusCode ?? 404, result.Message);
        }

        /// <summary>
        /// Retrieves all subjects.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSubjects()
        {
            var result = await subjectService.GetAll();
            if (!result.IsSuccess)
            {
                 return StatusCode(result.StatusCode ?? 404, result.Message);
            } 
            return Ok(result.Data);
        }

        /// <summary>
        /// Deletes a subject.
        /// </summary>
        /// <param name="id">The subject ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await subjectService.Delete(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        /// <summary>
        /// Deletes all subjects in a specific department.
        /// </summary>
        /// <param name="departmentId">The department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("DeleteAll/{departmentId:int}")]
        public async Task<IActionResult> DeleteAllSubjectsbyDepartment(int departmentId)
        {
            var result = await subjectService.ClearAsync(x=>x.DepartmentId==departmentId);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        /// <summary>
        /// Updates a subject.
        /// </summary>
        /// <param name="subjectDto">The subject data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateSubject(UpdateSubjectDto subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
            var result = await subjectService.Update(subjectDto);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }
        
    }

}
