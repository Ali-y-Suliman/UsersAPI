using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Dtos.User;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<SignInResponseDto>>> Register([FromBody] RegisterRequestDto user)
        {
            var response = await _userService.register(user);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<ServiceResponse<SignInResponseDto>>> user_signIn([FromBody] SignInRquestDto user)
        {
            var response = await _userService.signIn(user);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<ServiceResponse<SignInResponseDto>>> getUser(string email)
        {
            var response = await _userService.getUser(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // [HttpPut("resetPassword")]
        // public async Task <ActionResult <ServiceResponse<Boolean>>> resetPassword (ChangePasswordDto user)
        // {            
        //     return Ok(await _userService.changePassword(user));
        // }

    }
}