using AutoMapper;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Features.Account.GetUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Account.CreateUser
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, RegistreResponse>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        public CreateUserCommandHandler(IUserManager userManager, IMapper mapper) {
        
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<RegistreResponse> Handle(CreateUserCommand request,CancellationToken cancellationToken)
        {
            var createUserReq = new CreateUserRequest(
                request.FirstName,
                request.LastName,
                request.Password,
                request.ConfirmPassword,
                request.PhoneNumber,
                request.Email,
                request.Roles
                );

            await  _userManager.CreateUserAsync(createUserReq);

            return new RegistreResponse
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Roles = request.Roles,
                CreatedAt = default
            };

        }
    }
}
