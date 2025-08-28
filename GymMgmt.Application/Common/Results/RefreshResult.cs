using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Results
{
    public record RefreshResult(string UserName, string AccessToken, string RefreshToken, DateTimeOffset? RefreshTokenExpiryTime);
}
