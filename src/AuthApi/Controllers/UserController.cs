using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UserController : ControllerBase
    {
    
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser () 
        {
            return Ok();
        }
    }
}
