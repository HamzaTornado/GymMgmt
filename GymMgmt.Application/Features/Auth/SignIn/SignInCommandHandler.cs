using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using MediatR;


namespace GymMgmt.Application.Features.Auth.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResponse>
    {
        private readonly IUserManager _userManager;
        private readonly ITokenService _tokenService;

        public SignInCommandHandler(IUserManager userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var signinResponse = new SignInResponse();

            var result = await _userManager.SignInUserAsync(request.Email, request.Password);

            if (!result)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var tokenResult =  await _tokenService.GenerateTokenAsync(request.Email);

            var updateUser = new UpdateRefreshTokenRequest(
                request.Email,
                tokenResult.RefreshToken,
                tokenResult.RefreshTokenExpiryTime);

            await _userManager.UpdateRefreshTokenAsync(updateUser);

            signinResponse.AccessToken = tokenResult.AccessToken;
            signinResponse.RefreshToken = tokenResult.RefreshToken;
            signinResponse.RefreshTokenExpiryTime = tokenResult.RefreshTokenExpiryTime;

            return signinResponse;
        }
    }
}
