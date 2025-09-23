using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Results;
using GymMgmt.Infrastructure.Exceptions;
using GymMgmt.Infrastructure.Identity.Models;
using GymMgmt.Infrastructure.Identity.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace GymMgmt.Infrastructure.Identity
{
    internal class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TokenService> _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSigningConfigurations _signingConfigurations;
        private readonly IDateTimeService _machineDateTimeService;

        public TokenService(
            UserManager<ApplicationUser> userManager,
            ILogger<TokenService> logger,
            IOptionsSnapshot<JwtSettings> jwtSettings,
            JwtSigningConfigurations signingConfigurations,
            IDateTimeService machineDateTimeService)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _signingConfigurations = signingConfigurations;
            _machineDateTimeService = machineDateTimeService;
        }

        public async Task<TokenResult> GenerateTokenAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new UserNotFoundException(userName: userName);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = GetClaims(user, roles);

            var token = GetAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokenHash = HashingService.Hash(refreshToken);
            user.RefreshTokenExpiryTime = _machineDateTimeService.Now.AddMinutes(_jwtSettings.RefreshTokenValidityInMinutes);

            await _userManager.UpdateAsync(user);

            return new TokenResult(
                token,
                refreshToken,
                user.RefreshTokenExpiryTime);
        }

        public async Task<RefreshResult> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                throw new InvalidTokenException(message:"Invalid access token or refresh token");
            }

            string? username = principal.Identity?.Name;

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidTokenException(message:"Invalid access token: no username claim found");
            }

            var user = await _userManager.FindByNameAsync(username);

            // Check if the user exists and the refresh token matches
            if (user == null)
            {
                throw new UserNotFoundException(userName:username);
            }

            if (user.IsRefreshTokenRevoked == true) // Explicitly check for true
            {
                _logger.LogWarning("Refresh token rejected: already revoked for user {UserId}.", user.Id);
                throw new InvalidTokenException();
            }

            if (user.RefreshTokenExpiryTime <= DateTimeOffset.UtcNow)
            {
                _logger.LogWarning("Refresh token rejected: expired for user {UserId}.", user.Id);
                throw new InvalidTokenException();
            }

            if (user.RefreshTokenHash is null || !HashingService.Verify(refreshToken, user.RefreshTokenHash))
            {
                _logger.LogWarning("Refresh token rejected: signature mismatch or missing for user {UserId}.", user.Id);
                throw new InvalidTokenException();
            }
            user.IsRefreshTokenRevoked = true;
            var revokeResult = await _userManager.UpdateAsync(user);
            if (!revokeResult.Succeeded)
            {
                _logger.LogError("Failed to revoke refresh token for user {UserId}", user.Id);
                throw new InvalidOperationException("Failed to revoke refresh token");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = GetClaims(user, roles);
            var newToken = GetAccessToken(claims);
            var newRefreshToken = GenerateRefreshToken();

            // Update the user's refresh token and expiry time
            var hashedRefreshToken = HashingService.Hash(newRefreshToken);
            user.RefreshTokenHash = hashedRefreshToken;
            user.RefreshTokenExpiryTime = _machineDateTimeService.Now.AddMinutes(_jwtSettings.RefreshTokenValidityInMinutes);
            await _userManager.UpdateAsync(user);

            return new RefreshResult(username,newToken,newRefreshToken,user.RefreshTokenExpiryTime);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName)
               ?? throw new UserNotFoundException(userName:userName);

            var now = _machineDateTimeService.Now;

            user.IsRefreshTokenRevoked = true;
            user.RefreshTokenHash = null;
            user.RefreshTokenExpiryTime = now.AddMinutes(-1);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to revoke refresh token for user {UserId}: {Errors}",
                                 user.Id,
                                 string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new InfrastructureException(errorCode:"REVOKE_REFRESH_TOKEN_FAILD",message: "Could not revoke refresh token");
            }

            _logger.LogInformation("Refresh token revoked for user {UserId}", user.Id);
            return true;
        }

        private static IEnumerable<Claim> GetClaims(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim> {
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? user.UserName ?? string.Empty), // Email might be more appropriate
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private static string GenerateRefreshToken()
        {
            Span<byte> randomBytes = stackalloc byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Attempted to parse an empty or null access token.");
                throw new InvalidTokenException(message:"Access token is missing or empty.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingConfigurations.SecurityKey,
                ValidateLifetime = false, // Important: allow expired tokens for refresh
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken)
                {
                    _logger.LogWarning("Token validation failed: token is not a JWT.");
                    throw new InvalidTokenException(message:"The token is not a valid JWT.");
                }

                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Invalid token algorithm: {Algorithm}", jwtSecurityToken.Header.Alg);
                    throw new InvalidTokenException(message: "Invalid token algorithm.");
                }

                return principal;
            }
            catch (SecurityTokenMalformedException ex)
            {
                _logger.LogWarning(ex, "Malformed JWT token.");
                throw new InvalidTokenException(message: "The token format is malformed.");
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning(ex, "Token has expired (which is expected in refresh).");
                return null; // You may choose to allow returning claims even if expired
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Token validation failed.");
                throw new InvalidTokenException(message: "Invalid token format.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while validating access token.");
                throw new InfrastructureException("TOKEN_VALIDATION_FAILED", "Error message", ex);
            }
        }
        private string GetAccessToken(IEnumerable<Claim> claims)
        {
            var securityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenValidityInMinutes),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: _signingConfigurations.SigningCredentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
