using GymMgmt.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? UserName => 
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        public bool IsAuthenticated => 
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IEnumerable<Claim> Claims => 
            _httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();

        public ClaimsPrincipal? User => 
            _httpContextAccessor.HttpContext?.User;
    }
}
