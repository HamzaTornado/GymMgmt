using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Features.Members.GetMemberById;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.Members;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.UpdateMember
{
    public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, ReadMemberDto>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public UpdateMemberCommandHandler(IMemberRepository memberRepository, IMapper mapper)
        {

            _memberRepository = memberRepository;
            _mapper = mapper;
        }
        public async Task<ReadMemberDto> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            var memberid = MemberId.FromValue(request.MemberId);

            var member = await _memberRepository.FindByIdAsync(memberid, cancellationToken)
                    ?? throw new NotFoundException(nameof(Member), memberid);

            member.UpdateName(request.FirstName, request.LastName);
            member.UpdatePhoneNumber(request.PhoneNumber);
            member.UpdateAddress(_mapper.Map<Address>(request.AddressDto));
            member.UpdateEmail(request.Email);

            return _mapper.Map<ReadMemberDto>(member);
        }
    }
}
