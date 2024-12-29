using Bookify.Application.Users.LoginUser;
using Bookify.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UserController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);
        }
    }
}