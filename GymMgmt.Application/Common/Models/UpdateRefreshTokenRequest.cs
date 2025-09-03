using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Models
{
    public record UpdateRefreshTokenRequest(string UserName, string Token, DateTimeOffset? Expiry);
}
