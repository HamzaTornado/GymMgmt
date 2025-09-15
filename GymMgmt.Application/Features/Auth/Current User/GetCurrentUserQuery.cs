using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymMgmt.Application.Features.Auth.Current_User
{
    public sealed record GetCurrentUserQuery : IRequest<UserInfo>;
}
