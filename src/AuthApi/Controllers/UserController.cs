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
        private readonly TokenService _tokenService;    
        public UserController(UserService userService, TokenService tokenService){
            _userService = userService;
            _tokenService = tokenService;   
        }

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

        [HttpGet("get-all-tokens")]
        public async Task<IActionResult> GetAllTokenAsync()
        {
            var result = await _tokenService.GetAll();  
            if (!result.IsSuccessful) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
