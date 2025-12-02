using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.AcademicYear;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicLevelController : ControllerBase
    {
        private readonly IAcademicLevelService levelService;
        public AcademicLevelController(IAcademicLevelService _levelService)
        {
            levelService = _levelService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AcademicLevelDto academicLevelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await levelService.AddAsync(academicLevelDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Academic level added successfully.";
            return Ok(result);
        }

        [HttpPost("addRange")]
        public async Task<IActionResult> AddRange(List<AcademicLevelDto> academicLevelDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await levelService.AddRangeAsync(academicLevelDtos);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Academic levels added successfully.";
            return Ok(result);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var result = await levelService.ClearAsync(level => level.Subjects!.Count == 0);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Academic levels cleared successfully.";
            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await levelService.GetAll();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Academic levels retrieved successfully.";
            return Ok(result);
        }
        [HttpGet("getById/{levelId}")]
        public async Task<IActionResult> GetById(int levelId)
        {
            var result = await levelService.GetLevelAsync(levelId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Academic level retrieved successfully.";
            return Ok(result);
        }

        //[HttpPut("update")]
        //public async Task<IActionResult> Update(AcademicLevelDto academicLevelDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var result = await levelService.UpdateAsync(academicLevelDto);
        //    if (!result.IsSuccess)
        //        return StatusCode(result.StatusCode ?? 400, result.Message);
        //    result.Message = "Academic level updated successfully.";
        //    return Ok(result);
        //}


        [HttpDelete("delete/{levelId}")]
        public async Task<IActionResult> Delete(int levelId)
        {
            var result = await levelService.DeleteAsync(levelId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Academic level deleted successfully.";
            return Ok(result);
        }

    }
}
