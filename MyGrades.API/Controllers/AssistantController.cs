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
        private readonly ISubjectService subjectService;

        public AssistantController(IAssistantService _assistantService , ISubjectService subjectService)
        {
            assistantService = _assistantService;
            this.subjectService = subjectService;
        }

        /// <summary>
        /// Retrieves all assistants.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAssistants()
        {
            var result = await assistantService.GetAllAsync();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves an assistant by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the assistant to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAssistantById(int id)
        {
            var result = await assistantService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Assistant retrieved successfully.";
            return Ok(result);
        }

        /// <summary>
        /// Adds a new assistant.
        /// </summary>
        /// <param name="assistant">The assistant to add.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>   
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
            return Ok(result.Data);
        }

        /// <summary>
        /// Adds a range of assistants.
        /// </summary>
        /// <param name="assistants">The list of assistants to add.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
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
            return Ok(result.Data);
        }

        /// <summary>
        /// Deletes an assistant by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the assistant to delete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteAssistant(int id)
        {
            var result = await assistantService.DeleteAsync(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return NoContent();
        }

        /// <summary>
        /// Retrieves a list of assistants by their department ID.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department whose assistants are to be retrieved. Must be a valid department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns a 200 OK response with the
        /// list of assistants if successful; otherwise, returns an error response with the appropriate status code and
        /// message.</returns>
        [HttpGet("byDepartment/{departmentId:int}")]
        public async Task<IActionResult> GetAssistantsByDepartment(int departmentId)
        {
            var result = await assistantService.FindAllAsync(departmentId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves the list of subjects assigned to the specified assistant.
        /// </summary>
        /// <remarks>This endpoint is accessible via HTTP GET at <c>/[assistantId]/subjects</c>.  If the
        /// specified assistant does not exist or an error occurs during retrieval, an error response is
        /// returned.</remarks>
        /// <param name="assistantId">The unique identifier of the assistant whose subjects are to be retrieved. Must be a valid assistant ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Returns a 200 OK response with the
        /// list of subjects if successful; otherwise, returns an error response with the appropriate status code and
        /// message.</returns>
        [HttpGet("{assistantId:int}/subjects")]
        public async Task<IActionResult> GetAssistantSubjects(int assistantId)
        {
            var result = await subjectService.GetUserSubjectsAsync(s => s.AssistantId == assistantId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

    }


}
