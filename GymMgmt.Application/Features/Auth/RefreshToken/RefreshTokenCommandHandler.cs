using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserManager _userManager;

        public RefreshTokenCommandHandler(ITokenService tokenService, IUserManager userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshResult = await  _tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

            var updateRefresh = new UpdateRefreshTokenRequest(
                refreshResult.UserName,
                refreshResult.RefreshToken,
                refreshResult.RefreshTokenExpiryTime
                );

            await _userManager.UpdateRefreshTokenAsync(updateRefresh);

            return new TokenResponse(
                refreshResult.AccessToken,
                refreshResult.RefreshToken,
                refreshResult.RefreshTokenExpiryTime
                );
        }
    }
}
