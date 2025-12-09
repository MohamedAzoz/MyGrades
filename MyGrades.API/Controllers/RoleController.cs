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

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="roleDto">The role data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
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

        /// <summary>
        /// Deletes a role by its name.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
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

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="roleDto">The role data transfer object.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPut]
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
