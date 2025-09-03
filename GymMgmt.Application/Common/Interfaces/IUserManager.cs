

using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Common.Results;

namespace GymMgmt.Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<bool> SignInUserAsync(string userName, string password);
        Task<AppIdentityResult> CreateUserAsync(CreateUserRequest request);
        Task<string?> GetUserIdAsync(string userName);
        Task<string?> GetUserNameAsync(string userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<bool> UpdateRefreshTokenAsync(UpdateRefreshTokenRequest request);
        Task<string> GetUserIdByRefreshTokenAsync(string refreshToken);
    }
}
