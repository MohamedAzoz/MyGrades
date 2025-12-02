using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.User;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService _roleService)
        {
            roleService = _roleService;
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await roleService.AddRole(roleDto);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Created();
        }
        [HttpDelete("{roleName:alpha}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await roleService.DeleteRole(roleName);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return NoContent();
        }
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(UpdateRoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await roleService.UpdateRole(roleDto);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return NoContent();
        }
    }
}
