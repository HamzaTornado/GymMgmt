using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Features.Members.Queries.GetMemberStatistics;
using GymMgmt.Application.Features.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GymMgmt.Api.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("Stats")]
        [ProducesResponseType(typeof(ApiResponse<MemberStatisticsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetStats()
        {
            var result = await _mediator.Send(new GetMemberStatisticsQuery());
            return Ok(ApiResponse<MemberStatisticsDto>.Success(result));
        }
    }
}
