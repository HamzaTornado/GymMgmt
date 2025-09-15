using GymMgmt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Auth.Logout
{
    public sealed record LogoutCommand(string UserName) : ICommand<bool>;
}
