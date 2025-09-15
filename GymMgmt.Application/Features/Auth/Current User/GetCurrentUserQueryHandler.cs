using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using MediatR;
using System.Security.Claims;


namespace GymMgmt.Application.Features.Auth.Current_User
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserInfo>
    {
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public Task<UserInfo> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {

            var currentUser = new UserInfo
            (
                Id : _currentUserService.UserId,
                Email : _currentUserService.UserName,
                Roles : _currentUserService.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToArray()
            );

            return Task.FromResult(currentUser);
        }
    }
}
