using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Features.Account.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GymMgmt.Api.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand registerUserCommande)
        {
            var result = await _mediator.Send(registerUserCommande);


            return Ok(ApiResponse<RegistreResponse>.Success(result, "User Created successful"));
        }

    }
}
