using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Features.Memberships.CreateMembership;
using GymMgmt.Application.Features.Memberships.GetAllMemberShipsPlans;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;
using GymMgmt.Application.Features.Memberships.UpdateMembershipPlan;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GymMgmt.Api.Controllers.MemberShipsPlans
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberShipsPlansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MemberShipsPlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("AddMemberShipPlan")]
        [ProducesResponseType(typeof(ApiResponse<ReadMemberShipPlanDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateMemberShipPlan([FromBody] CreateMemberShipPlanCommand createMemberShipPlanCommand)
        {
            var result = await _mediator.Send(createMemberShipPlanCommand);


            return Ok(ApiResponse<ReadMemberShipPlanDto>.Success(result, "MemberShip Plan  added successful"));
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<ReadMemberShipPlanDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetMember(Guid id)
        {
            var result = await _mediator.Send(new GetMemberShipPlanByIdQuery(id));

            return Ok(ApiResponse<ReadMemberShipPlanDto>.Success(result));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReadMemberShipPlanDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetAllMembers()
        {
            var result = await _mediator.Send(new GetAllMemberShipsPlansQuery());
            return Ok(ApiResponse<IEnumerable<ReadMemberShipPlanDto>>.Success(result));
        }


        [AllowAnonymous]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReadMemberShipPlanDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateMemberShipPlan(Guid id, UpdateMemberShipPlanCommand updateMemberShipPlanComman)
        {
            if (updateMemberShipPlanComman.Id != id)
            {
                return BadRequest(ApiResponse<object>.Fail("Route ID and body ID mismatch"));
            }
            var result = await _mediator.Send(updateMemberShipPlanComman);

            return Ok(ApiResponse<ReadMemberShipPlanDto>.Success(result, "MemberShip Plan  Updated successful"));
        }

    }
}
