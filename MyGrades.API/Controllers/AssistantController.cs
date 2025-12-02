using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistantController : ControllerBase
    {
        private readonly IAssistantService assistantService;
        public AssistantController(IAssistantService _assistantService)
        {
            assistantService = _assistantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssistants()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await assistantService.GetAllAsync();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Assistants retrieved successfully.";
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAssistantById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await assistantService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Assistant retrieved successfully.";
            return Ok(result);
        }
       
        [HttpPost("add")]
        public async Task<IActionResult> AddAssistant(AssistantExcelDto assistant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await assistantService.AddAsync(assistant);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Assistant added successfully.";
            return Ok(result);
        }
        [HttpPost("addRange")]
        public async Task<IActionResult> AddAssistantsRange(List<AssistantExcelDto> assistants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await assistantService.AddRangeAsync(assistants);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Assistants added successfully.";
            return Ok(result);
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteAssistant(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await assistantService.DeleteAsync(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Assistant deleted successfully.";
            return Ok(result);
        }

        //[HttpPut("update")]
        //public async Task<IActionResult> UpdateAssistant()
        //{
        //    if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //    var result = await assistantService.UpdateAsync();
        //    if (!result.IsSuccess)
        //        return StatusCode(result.StatusCode ?? 400, result.Message);
        //    result.Message = "Assistant updated successfully.";
        //    return Ok(result);
        //}

        [HttpGet("byDepartment/{departmentId:int}")]
        public async Task<IActionResult> GetAssistantsByDepartment(int departmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await assistantService.FindAllAsync(departmentId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Assistants retrieved successfully.";
            return Ok(result);
        }

    }
}
