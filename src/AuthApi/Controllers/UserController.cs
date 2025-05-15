using AuthApi.Dtos;
using AuthApi.Services.Email;
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
        public UserController(UserService userService, EmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync (UserRegisterDto newUser) 
        {
            var result = await _userService.Register(newUser);
            if (!result.IsSuccessful) return BadRequest(result.Error);

            // generar token 
            _emailService.SendActivationEmail(result.Value.Name, result.Value.Email,"d5as675das67");
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
