

using GymMgmt.Application.Common.Results;

namespace GymMgmt.Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateUserAsync(string firstName, string lastName, string password, string email, List<string> roles);
        Task<SignInResult> SignInUserAsync(string userName, string password);
        Task<string?> GetUserIdAsync(string userName);
        Task<string?> GetUserNameAsync(string userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<IdentityResult> UpdateRefreshTokenAsync(string userName, RefreshTokenInfo tokenInfo);
        Task<UserIdResult> GetUserIdByRefreshTokenAsync(string refreshToken);
    }
}
