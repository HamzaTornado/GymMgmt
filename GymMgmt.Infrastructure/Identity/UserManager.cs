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
        public async Task<AppIdentityResult> CreateUserAsync(CreateUserRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.email,
                Email = request.email,
                FirstName = request.firstName,
                LastName = request.lastName
            };

            var result = await _userManager.CreateAsync(user, request.password);

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
            if (request.roles.Count <= 0)
            {
                request.roles.Add("User");
            }

            var addUserRole = await _userManager.AddToRolesAsync(user, request.roles);
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

        public async Task<bool> UpdateRefreshTokenAsync(UpdateRefreshTokenRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            // hashing the refresh token before saving it
            user.RefreshTokenHash = HashingService.Hash(request.Token);
            user.IsRefreshTokenRevoked = false;
            user.RefreshTokenExpiryTime = request.Expiry;

            var result = await _userManager.UpdateAsync(user);

            if (result.Errors.Any())
            {
                throw new IdentityValidationException(result.Errors.Select(er =>
                    new AppIdentityError
                    {
                        Code = er.Code,
                        Description = er.Description
                    }));
            };
            return result.Succeeded;
        }

        public Task<string> GetUserIdByRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
