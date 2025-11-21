using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Features.ClubSetup.AddClubSettings;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;
using GymMgmt.Application.Features.ClubSetup.UpdateClubSettings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GymMgmt.Api.Controllers.ClubSettings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClubSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClubSettingsController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        
        [HttpPost("AddClubSettings")]
        [ProducesResponseType(typeof(ApiResponse<ReadClubSettingsDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateClubSettings([FromBody]  CreateClubSettingsCommand createClubSettingsCommand)
        {
            var result = await _mediator.Send(createClubSettingsCommand);


            return Ok(ApiResponse<ReadClubSettingsDto>.Success(result, "Club Settings added successful"));
        }

        [HttpGet("GetClubSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<ReadClubSettingsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>),(int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetClubSetting()
        {
            var query = new GetClubSettingsQuery();
            var result = await _mediator.Send(query);

            return Ok(ApiResponse<ReadClubSettingsDto>.Success(result));
        }
        [HttpPut("UpdateClubSetting")]
        [ProducesResponseType(typeof(ApiResponse<ReadClubSettingsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateClubSetting([FromBody] UpdateClubSettingsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<ReadClubSettingsDto>.Success(result));
        }

    }
}
