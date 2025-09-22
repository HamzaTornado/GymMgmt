using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Features.Auth.Current_User;
using GymMgmt.Application.Features.Auth.Logout;
using GymMgmt.Application.Features.Auth.RefreshToken;
using GymMgmt.Application.Features.Auth.SignIn;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMgmt.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<SignInResult>>> Login(SignInCommand signInCommand)
        {
            var result = await _mediator.Send(signInCommand);
            return Ok(ApiResponse<SignInResponse>.Success(result, "User Connected successful"));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<TokenResponse>>> RefreshToken(RefreshTokenCommand refreshTokenCommand)
        {
            var result = await _mediator.Send(refreshTokenCommand);
            return Ok(ApiResponse<TokenResponse>.Success(result, "Refresh Token successful"));
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserInfo>> Me(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetCurrentUserQuery(), ct);

            return Ok(ApiResponse<UserInfo>.Success(result, "Current User Info"));
        }

        [HttpPost("Logout")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiResponse<string>>> Logout(LogoutCommand logoutCommand)
        {
            await _mediator.Send(logoutCommand);
            return NoContent();
        }
    }
}
