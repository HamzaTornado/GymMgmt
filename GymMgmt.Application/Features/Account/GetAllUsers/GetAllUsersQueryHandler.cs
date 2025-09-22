using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Account.GetUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Account.GetAllUsers
{
    internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery,IEnumerable<ReadUserDto>>
    {
        private readonly IUserManager _userManager;

        public GetAllUsersQueryHandler(IUserManager userManager) {

            _userManager = userManager;
        }

        public async Task<IEnumerable<ReadUserDto>> Handle(GetAllUsersQuery request,CancellationToken cancellationToken)
        {
            var users = await _userManager.GetAllUsersAsync(cancellationToken);

            return users;
        }
    }
}
