using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AuthorizationAPI.Models;
using AuthorizationAPI.Services;

namespace AuthorizationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IConfiguration config, IUserService userService, IJwtService jwtService)
        {
            _config = config;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] LoginModel loginModel)
        {
            var user = _userService.GetUser(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateSecurityToken(loginModel.Username);

            return Ok(new { Token = token });
        }

        [HttpPost]
        [Route("signout")]
        public IActionResult SignOut()
        {
            // TODO: Implement sign-out functionality
            return Ok();
        }
    }
}
