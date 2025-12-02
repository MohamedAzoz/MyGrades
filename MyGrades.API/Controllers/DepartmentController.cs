using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.Department;
using MyGrades.Application.Contracts.Services;
using MyGrades.Application.Services;

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
            result.Message = "Department created successfully.";
            return Ok(result);
        }
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
            result.Message = "Departments created successfully.";
            return Ok(result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await departmentService.GetById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Department retrieved successfully.";
            return Ok(result);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await departmentService.GetAll();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Departments retrieved successfully.";
            return Ok(result);
        }


        //[HttpPost("update")]
        //public async Task<IActionResult> Update(DepartmentDto departmentDto)
        //{
        //if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }/
        //    var result = await departmentService.Update(departmentDto);
        //    if (!result.IsSuccess)
        //        return StatusCode(result.StatusCode ?? 400, result.Message);
        //    result.Message = "Department updated successfully.";
        //    return Ok(result);
        //}

        [HttpDelete("delete{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await departmentService.Delete(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode ?? 400, result.Message);
            result.Message = "Department deleted successfully.";
            return Ok(result);
        }

    }
}
