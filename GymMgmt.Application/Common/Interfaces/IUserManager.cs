

using GymMgmt.Application.Common.Results;

namespace GymMgmt.Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<bool> SignInUserAsync(string userName, string password);
        Task<AppIdentityResult> CreateUserAsync(string firstName, string lastName, string password, string email, List<string> roles);
        Task<string?> GetUserIdAsync(string userName);
        Task<string?> GetUserNameAsync(string userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<AppIdentityResult> UpdateRefreshTokenAsync(string userName, RefreshTokenInfo tokenInfo);
        Task<UserIdResult> GetUserIdByRefreshTokenAsync(string refreshToken);
    }
}
