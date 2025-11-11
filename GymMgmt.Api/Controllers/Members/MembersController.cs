using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Features.Members.CreateMember;
using GymMgmt.Application.Features.Members.GetAllMembers;
using GymMgmt.Application.Features.Members.GetMemberById;
using GymMgmt.Application.Features.Members.PayInsurrance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GymMgmt.Api.Controllers.Members
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("AddMember")]
        [ProducesResponseType(typeof(ApiResponse<ReadMemberDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateMember([FromBody] CreateMemberCommand createMemberCommand)
        {
            var result = await _mediator.Send(createMemberCommand);


            return Ok(ApiResponse<ReadMemberDto>.Success(result, "Member  added successful"));
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<ReadMemberDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetMember(Guid id)
        {
            var result = await _mediator.Send(new GetMemberByIdQuery(id));

            return Ok(ApiResponse<ReadMemberDto>.Success(result));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReadMemberDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetAllMembers()
        {
            var result = await _mediator.Send(new GetAllMembersQuery());
            return Ok(ApiResponse<IEnumerable<ReadMemberDto>>.Success(result));
        }

        [AllowAnonymous]
        [HttpPost("PayInsurance")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> PayInsurance([FromBody] CreateInsurancePaymentCommand createInsurancePayment)
        {
            var result = await _mediator.Send(createInsurancePayment);

            return Ok(ApiResponse<bool>.Success(result));
        }

    }
}
