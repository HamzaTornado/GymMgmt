

using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Common.Results;
using GymMgmt.Application.Features.Account.GetUser;
using System.Threading;

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
        Task<ReadUserDto?> GetUserByIdAsync(string userId,CancellationToken cancellationToken= default);
        Task<IEnumerable<ReadUserDto>> GetAllUsersAsync(CancellationToken cancellationToken=default);
    }
}
