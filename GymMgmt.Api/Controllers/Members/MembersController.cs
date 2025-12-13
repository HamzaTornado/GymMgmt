using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Features.Members;
using GymMgmt.Application.Features.Members.CreateMember;
using GymMgmt.Application.Features.Members.GetAllMembers;
using GymMgmt.Application.Features.Members.GetMemberById;
using GymMgmt.Application.Features.Members.PayInsurrance;
using GymMgmt.Application.Features.Members.Queries.GetmembersByStatus;
using GymMgmt.Application.Features.Members.Queries.GetMemberStatistics;
using GymMgmt.Application.Features.Members.Queries;
using GymMgmt.Application.Features.Members.UpdateMember;
using GymMgmt.Application.Features.Memberships.UpdateMembershipPlan;
using GymMgmt.Application.Features.Subscriptions.GetAllSubscriptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using GymMgmt.Application.Features.Members.DeleteMember;

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

         
        [HttpPost("AddMember")]
        [ProducesResponseType(typeof(ApiResponse<ReadMemberDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateMember([FromBody] CreateMemberCommand createMemberCommand)
        {
            var result = await _mediator.Send(createMemberCommand);


            return Ok(ApiResponse<ReadMemberDto>.Success(result, "Member  added successful"));
        }

         
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

         
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReadMemberDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetAllMembers()
        {
            var result = await _mediator.Send(new GetAllMembersQuery());
            return Ok(ApiResponse<IEnumerable<ReadMemberDto>>.Success(result));
        }
       

         
        [HttpGet("Memberslist/{filter}")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<MemberListDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetList(
            MemberStatusFilter filter,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetMembersByStatusQuery(filter, pageNumber, pageSize));
            
            return Ok(ApiResponse<PaginatedList<MemberListDto>>.Success(result));
        }

         
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponse<ReadMemberDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateMember(Guid id,UpdateMemberCommand updateMemberCommand)
        {
            if (updateMemberCommand.MemberId != id)
            {
                return BadRequest(ApiResponse<object>.Fail("Route ID and body ID mismatch"));
            }
            var result = await _mediator.Send(updateMemberCommand);
            return Ok(ApiResponse<ReadMemberDto>.Success(result));
        }

         
        [HttpPost("PayInsurance")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> PayInsurance([FromBody] CreateInsurancePaymentCommand createInsurancePayment)
        {
            var result = await _mediator.Send(createInsurancePayment);

            return Ok(ApiResponse<bool>.Success(result));
        }

        [HttpDelete("DeleteMember/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteMember(Guid id)
        {
            var deleteMemberCommand = new DeleteMemberCommand(id);
            var result = await _mediator.Send(deleteMemberCommand);

            return Ok(ApiResponse<bool>.Success(result));
        }



    }
}
