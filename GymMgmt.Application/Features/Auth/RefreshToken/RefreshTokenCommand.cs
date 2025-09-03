using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Auth.RefreshToken
{
    public record RefreshTokenCommand(
        string AccessToken,
        string RefreshToken
        ) : ICommand<TokenResponse>;
}
