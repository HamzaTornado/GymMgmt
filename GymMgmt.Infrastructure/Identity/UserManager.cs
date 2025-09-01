using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Common.Results;
using GymMgmt.Application.Exceptions;
using GymMgmt.Infrastructure.Exceptions;
using GymMgmt.Infrastructure.Identity.Models;
using GymMgmt.Infrastructure.Identity.Security;
using Microsoft.AspNetCore.Identity;

namespace GymMgmt.Infrastructure.Identity
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDateTimeService _machineDateTimeService;

        public UserManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IDateTimeService machineDateTimeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _machineDateTimeService = machineDateTimeService;
        }

        public async Task<bool> SignInUserAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);

            return result.Succeeded;
        }
        public async Task<AppIdentityResult> CreateUserAsync(string firstName, string lastName, string password, string email, List<string> roles)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new IdentityValidationException(
                    result.Errors.Select(e => new AppIdentityError
                    {
                        Code = e.Code,
                        Description = e.Description
                    })
                    );
            }
            if (roles.Count <= 0)
            {
                roles.Add("User");
            }

            var addUserRole = await _userManager.AddToRolesAsync(user, roles);
            if (!addUserRole.Succeeded)
            {
                throw new IdentityValidationException(addUserRole.Errors.Select(e => new AppIdentityError
                {
                    Code = e.Code,
                    Description = e.Description
                }));
            }
            return AppIdentityResult.Success();
        }

        public async Task<string?> GetUserIdAsync(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);

            if(result ==null)
                throw new UserNotFoundException(userName);

            return result.Id;
        }

       

        public async Task<string?> GetUserNameAsync(string userId)
        {
            var result = await _userManager.FindByIdAsync(userId);

            if (result == null)
                throw new UserNotFoundException(userId: Guid.Parse(userId));

            return result.UserName;
        }

        public async Task<bool> IsUniqueUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName) == null;
        }

        public async Task<AppIdentityResult> UpdateRefreshTokenAsync(string userName, RefreshTokenInfo tokenInfo)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return AppIdentityResult.Failure();
            }
            // hashing the refresh token before saving it
            user.RefreshTokenHash = HashingService.Hash(tokenInfo.Token);
            user.IsRefreshTokenRevoked = tokenInfo.IsRevoked;
            user.RefreshTokenExpiryTime = tokenInfo.Expiry;

            var result = await _userManager.UpdateAsync(user);
            return AppIdentityResult.Success();
        }

        public Task<UserIdResult> GetUserIdByRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
