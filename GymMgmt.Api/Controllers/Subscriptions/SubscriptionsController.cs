using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Features.Subscriptions.CancelSubscription;
using GymMgmt.Application.Features.Subscriptions.ExtendSubscription;
using GymMgmt.Application.Features.Subscriptions.StartSubscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GymMgmt.Api.Controllers.Subscriptions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous] // TODO: Secure this endpoint
        [HttpPost("Start")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> StartSubscription([FromBody] StartSubscriptionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<Guid>.Success(result, "Subscription started successfully"));
        }

        [AllowAnonymous] // TODO: Secure this endpoint
        [HttpPost("Extend")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ExtendSubscription([FromBody] ExtendSubscriptionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<bool>.Success(result, "Subscription extended successfully"));
        }

        [AllowAnonymous] // TODO: Secure this endpoint
        [HttpPost("Cancel")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CancelSubscription([FromBody] CancelSubscriptionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<bool>.Success(result, "Subscription cancelled successfully"));
        }
    }
}
