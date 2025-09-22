using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Account.GetUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Account.GetAllUsers
{
    public sealed record GetAllUsersQuery() : ICommand<IEnumerable<ReadUserDto>>;
}
