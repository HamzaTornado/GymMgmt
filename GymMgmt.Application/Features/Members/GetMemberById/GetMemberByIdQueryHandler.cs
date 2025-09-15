using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Entities.Members;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.GetMemberById
{
    public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, ReadMemberDto>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        public GetMemberByIdQueryHandler(IMemberRepository memberRepository, IMapper mapper) 
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }
        public async Task<ReadMemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {

            // Convert Guid to MemberId using MemberId.FromValue  
            var memberId = MemberId.FromValue(request.Id);
            var member = await _memberRepository.FindByIdAsync(memberId, cancellationToken);

            if (member == null)
            {
                throw new NotFoundException(nameof(member) ,request.Id);
            }

            return _mapper.Map<ReadMemberDto>(member);
        }
    }
}
