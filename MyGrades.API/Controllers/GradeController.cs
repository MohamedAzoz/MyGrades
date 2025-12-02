using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;
using System.Threading.Tasks;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService gradeService;

        public GradeController(IGradeService _gradeService)
        {
            gradeService = _gradeService;
        }

        [HttpGet("gatAll{subjectId:int}")]
        public async Task<IActionResult> GatAll(int subjectId)
        {
            var result=await gradeService.GetAll(subjectId);
            if (! result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500 ,result.Message);
            }
            result.Message = "Grades retrieved successfully.";
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result=await gradeService.Delete(id);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500, result.Message);
            }
            result.Message = "Grade deleted successfully.";
            return Ok(result);
        }

        //[HttpPost("create")]
        //public async Task<IActionResult> Create([FromBody] Grade grade)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var result = await gradeService.Create(grade);
        //    if (!result.IsSuccess)
        //    {
        //        return StatusCode(result.StatusCode ?? 500, result.Message);
        //    }
        //    result.Message = "Grade created successfully.";
        //    return Ok(result);
        //}
        //[HttpPost("createRange")]
        //public async Task<IActionResult> CreateRange([FromBody] List<Grade> grades)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var result = await gradeService.CreateRang(grades);
        //    if (!result.IsSuccess)
        //    {
        //        return StatusCode(result.StatusCode ?? 500, result.Message);
        //    }
        //    result.Message = "Grades created successfully.";
        //    return Ok(result);
        //}
        //[HttpPut("update")]
        //public async Task<IActionResult> Update([FromBody] Grade grade)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var deleteResult = await gradeService.Delete(grade.Id);
        //    if (!deleteResult.IsSuccess)
        //    {
        //        return StatusCode(deleteResult.StatusCode ?? 500, deleteResult.Message);
        //    }
        //    var createResult = await gradeService.Create(grade);
        //    if (!createResult.IsSuccess)
        //    {
        //        return StatusCode(createResult.StatusCode ?? 500, createResult.Message);
        //    }
        //    createResult.Message = "Grade updated successfully.";
        //    return Ok(createResult);
        //}
        //[HttpPut("updateRange")]
        //public async Task<IActionResult> UpdateRange([FromBody] List<Grade> grades)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    foreach (var grade in grades)
        //    {
        //        var deleteResult = await gradeService.Delete(grade.Id);
        //        if (!deleteResult.IsSuccess)
        //        {
        //            return StatusCode(deleteResult.StatusCode ?? 500, deleteResult.Message);
        //        }
        //    }
        //    var createResult = await gradeService.CreateRang(grades);
        //    if (!createResult.IsSuccess)
        //    {
        //        return StatusCode(createResult.StatusCode ?? 500, createResult.Message);
        //    }
        //    createResult.Message = "Grades updated successfully.";
        //    return Ok(createResult);
        //}


    }
}
