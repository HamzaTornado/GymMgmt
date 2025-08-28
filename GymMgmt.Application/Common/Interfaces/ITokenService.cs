using GymMgmt.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResult> GenerateTokenAsync(string userName);
        Task<RefreshResult> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string userName, CancellationToken cancellationToken);
    }
}
