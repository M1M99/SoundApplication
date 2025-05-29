using Identity.Api.Dtos;
using Identity.Api.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var data = await _service.LoginAsync(dto);
            if(data is null)
            {
                return Unauthorized();
            }
            return Ok(new { token = data } );
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            var data = await _service.RegisterAsync(dto);
            if (!data.Succeeded)
            {
                return BadRequest("Wrong Information");
            }
            return Ok("Successfully Register");
        }
    }
}
