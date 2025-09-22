using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Common.Interfaces;
using MediatR;

namespace GymMgmt.Application.Features.Account.GetUser
{
    internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery,ReadUserDto>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserManager userManager,IMapper mapper) {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ReadUserDto> Handle(GetUserByIdQuery requset,CancellationToken cancellationToken=default)
        {

            var user= await _userManager.GetUserByIdAsync(requset.Id);

            if (user == null)
            {
                throw new NotFoundException();
            }

            return user;
        }
    }
}
