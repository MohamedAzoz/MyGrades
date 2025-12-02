using Microsoft.AspNetCore.Http;
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

        [HttpPost("Add")]
        public async Task<IActionResult> AddSubject([FromBody] SubjectDto subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (subjectDto == null)
            {
                return BadRequest("Invalid subject data.");
            }
            var result = await subjectService.Create(subjectDto);
            if (result.IsSuccess)
            {
                return Created();
            }

            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            var result = await subjectService.GetById(id);
            if (result.IsSuccess)
            {
                result.Message = "Subject retrieved successfully.";
                return Ok(result);
            }

            return StatusCode(result.StatusCode ?? 404, result.Message);
        }

        [HttpGet("byDepartment/{departmentId}")]
        public async Task<IActionResult> GetSubjectsByDepartment(int departmentId)
        {
            var result = await subjectService.FindAllAsync(x => x.DepartmentId == departmentId);
            if (result.IsSuccess)
            {
                result.Message = "Subjects retrieved successfully.";
                return Ok(result);
            }

            return StatusCode(result.StatusCode ?? 404, result.Message);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var result = await subjectService.GetAll();
            if (!result.IsSuccess)
            {
                 return StatusCode(result.StatusCode ?? 404, result.Message);
            }
            result.Message = "Subjects retrieved successfully.";
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await subjectService.Delete(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        [HttpDelete("DeleteAll{departmentId:int}")]
        public async Task<IActionResult> DeleteAllSubjectsbyDepartment(int departmentId)
        {
            var result = await subjectService.ClearAsync(x=>x.DepartmentId==departmentId);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSubject(UpdateSubjectDto subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (subjectDto == null)
            {
                return BadRequest("Invalid subject data.");
            }
            var result = await subjectService.Update(subjectDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode ?? 500, result.Message);
        }
        

    }
}
