using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Models;
using Task_Management.Services;

namespace Task_Management.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CognitoAuthService _authService;

        public UsersController(CognitoAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _ = new AmazonCognitoIdentityProviderClient();

            var token = await _authService.AuthenticateUser(request.Username, request.Password);
            if (token == null)
                return Unauthorized(new { Message = "Invalid credentials" });

            return Ok(new { Token = token });
        }
    }
}
