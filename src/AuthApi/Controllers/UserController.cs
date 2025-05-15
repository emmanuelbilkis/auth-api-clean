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
        private readonly EmailService _emailService;    
        private readonly TokenService _tokenService;
        public UserController(UserService userService, 
                              EmailService emailService,
                              TokenService tokenService)
        {
            _userService = userService;
            _emailService = emailService;
            _tokenService = tokenService;   
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync (UserRegisterDto newUser) 
        {
            var result = await _userService.Register(newUser);
            if (!result.IsSuccessful) return BadRequest(result.Error);

            var tokenResult = _tokenService.AddToken(); 
            if (!tokenResult.IsSuccessful) return BadRequest(tokenResult);

            _emailService.SendActivationEmail(result.Value.Name, result.Value.Email, tokenResult.Value);
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
        public IActionResult ActivateAccount([FromQuery] string token, [FromQuery] string email)
        {
            //validar 
            //llamar al metodo para activar la cuenta 

            return Ok("xd");   
        }
    }
}
