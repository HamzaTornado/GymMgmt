using GymMgmt.Application.Common.Interfaces;
using MediatR;


namespace GymMgmt.Application.Features.Auth.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand,bool>
    {
        private readonly ITokenService _tokenService;

        public LogoutCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var result = await _tokenService.RevokeRefreshTokenAsync(request.UserName,cancellationToken);

            return result;
        }
    }
}
