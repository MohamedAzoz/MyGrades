using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.Department;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="departmentDto">The department data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(DepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await departmentService.Create(departmentDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Created();
        }

        /// <summary>
        /// Creates a range of departments.
        /// </summary>
        /// <param name="departmentDtos">The list of department data transfer objects.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("addRange")]
        public async Task<IActionResult> CreateRange(List<DepartmentDto> departmentDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await departmentService.CreateRang(departmentDtos);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Created();
        }

        /// <summary>
        /// Retrieves a department by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await departmentService.GetById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await departmentService.GetAll();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return Ok(result.Data);
        }

        /// <summary>
        /// Deletes a department by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await departmentService.Delete(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message); 
            return NoContent();
        }

    }

}
