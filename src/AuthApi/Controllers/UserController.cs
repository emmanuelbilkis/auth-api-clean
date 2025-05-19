using AuthApi.Dtos;
using AuthApi.Services.Email;
using AuthApi.Services.Token;
using AuthApi.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;  
        public UserController(UserService userService){ _userService = userService;}

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync (UserRegisterDto newUser) 
        {
            var result = await _userService.Register(newUser);
            if (!result.IsSuccessful) return BadRequest(result.Error);
            return Ok(result.Value);    
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var result = await _userService.GetAll();
            if(!result.IsSuccessful) return BadRequest(result.Error);   
            return Ok(result.Value);
        }

        [HttpGet("activate-account")]
        public async Task<IActionResult> ActivateAccount([FromQuery] string token, [FromQuery] string email)
        {
            var result = await _userService.ActivateAccountAsync(email, token);
            if (!result.IsSuccessful) return BadRequest(result.Error);

            return Ok(new { message = "Cuenta activada correctamente", email });
        }
    }
}
