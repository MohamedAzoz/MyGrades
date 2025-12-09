using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyGrades.Application.Contracts.DTOs.User;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly SignInManager<AppUser> signInManager;

        public AuthController(IAuthService authService, SignInManager<AppUser> signInManager)
        {
            this.authService = authService;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userLoginDto">The user login information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.LoginAsync(userLoginDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 400, result.Message);
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Changes the password of a user.
        /// </summary>
        /// <param name="changePasswordDto">The change password information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.ChangePasswordAsync(changePasswordDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 400, result.Message);
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.</returns>
        [HttpDelete]
        [Route("DeleteUser/{userId:guid}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await authService.DeleteUser(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }


    }

}
