using BreweryAPI.Models.Auth;
using BreweryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost()]
        public virtual async Task<IActionResult> Login(UserDTO userDto)
        {
            var token = await _authService.Login(userDto);
            return !string.IsNullOrEmpty(token) ? Ok(token) : BadRequest();
        }

        [HttpPost()]
        public virtual async Task<IActionResult> Register(UserDTO userDto)
        {
            var registered = await _authService.Register(userDto);
            return registered ? Ok() : BadRequest();
        }
    }
}
