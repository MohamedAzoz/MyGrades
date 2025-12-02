using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyGrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
            
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await _authService.Login(model);

        //    if (!result.IsAuthenticated)
        //        return BadRequest(result.Message);

        //    if (!string.IsNullOrEmpty(result.RefreshToken))
        //        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        //    return Ok(result);
        //}

        ///// <summary>
        ///// Initiates the password reset process by generating a reset token and sending a reset link to the specified
        ///// email address.
        ///// </summary>
        ///// <remarks>This method does not disclose whether the email address is registered in the system
        ///// for security reasons. If the email address is registered, a password reset link is sent to the provided
        ///// email address. The reset link includes a token and user ID, and its format depends on the client type (e.g.,
        ///// mobile or web).</remarks>
        ///// <param name="emailDTO">An object containing the email address of the user requesting the password reset, along with additional
        ///// client-specific information.</param>
        ///// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns: <list type="bullet"> <item>
        ///// <description><see cref="OkObjectResult"/> with a confirmation message if the email is registered or
        ///// unregistered.</description> </item> <item> <description><see cref="BadRequestObjectResult"/> if the provided
        ///// model state is invalid.</description> </item> </list></returns>
        //[HttpPost("forgotPassword")]
        //public async Task<IActionResult> ForgotPassword([FromBody] EmailDTO emailDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var result = await _authService.ForgotPassword(emailDTO);
        //    return Ok(result.Message);
        //}
    }
}
