using Asp.Versioning;
using Bookify.Application.Users.LoggedInUser;
using Bookify.Application.Users.LoginUser;
using Bookify.Application.Users.RegisterUser;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users
{
    [ApiController]
    //[ApiVersion(ApiVersions.V1, Deprecated = true)]
    [ApiVersion(ApiVersions.V1)]
    [ApiVersion(ApiVersions.V2)]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController(ISender sender) : ControllerBase
    {
        [HttpGet("me")]
        [MapToApiVersion(ApiVersions.V1)]
        [HassPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetLoggedInUserV1(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();
            var result = await sender.Send(query, cancellationToken);
            
            return Ok(result.Value);
        } 
        
        [HttpGet("me")]
        //[Authorize(Roles = Roles.Registered)]
        [MapToApiVersion(ApiVersions.V2)]
        [HassPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetLoggedInUserV2(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();
            var result = await sender.Send(query, cancellationToken);
            
            return Ok(result.Value);
        }
        
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