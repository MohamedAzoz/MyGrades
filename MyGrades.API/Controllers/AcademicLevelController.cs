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

        /// <summary>
        /// Adds a new academic level.
        /// </summary>
        /// <param name="academicLevelDto">The academic level to add.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AcademicLevelDto academicLevelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await levelService.AddAsync(academicLevelDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Created();
        }

        /// <summary>
        /// Clears all academic levels that do not have any associated subjects.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var result = await levelService.ClearAsync(level => level.Subjects!.Count == 0);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return NoContent();
        }

        /// <summary>
        /// Retrieves all academic levels.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await levelService.GetAll();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves an academic level by its unique identifier.
        /// </summary>
        /// <param name="levelId">The unique identifier of the academic level to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("getById/{levelId}")]
        public async Task<IActionResult> GetById(int levelId)
        {
            var result = await levelService.GetLevelAsync(levelId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Deletes an academic level by its unique identifier.
        /// </summary>
        /// <param name="levelId">The unique identifier of the academic level to delete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("{levelId}")]
        public async Task<IActionResult> Delete(int levelId)
        {
            var result = await levelService.DeleteAsync(levelId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return NoContent();
        }

    }

}
