using AutoMapper;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.Members;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.CreateMember
{
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Guid>
    {

        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public CreateMemberCommandHandler(IMemberRepository memberRepository, IMapper mapper) 
        {

            _memberRepository = memberRepository;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = Member.Create(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email,
                _mapper.Map<Address>(request.AddressDto)
            );

            await _memberRepository.AddAsync(member,cancellationToken);

            return member.Id.Value;
        }
    }
}
