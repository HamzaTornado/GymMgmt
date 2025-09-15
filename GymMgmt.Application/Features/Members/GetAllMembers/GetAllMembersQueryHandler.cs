using AutoMapper;
using GymMgmt.Application.Features.Members.GetMemberById;
using GymMgmt.Domain.Entities.Members;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.GetAllMembers
{
    public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, IEnumerable<ReadMemberDto>>
    {

        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public GetAllMembersQueryHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadMemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            var members = await _memberRepository.GetAllAsync(cancellationToken);

            if (members == null || !members.Any())
            {
                return Enumerable.Empty<ReadMemberDto>();
            }

            return _mapper.Map<IEnumerable<ReadMemberDto>>(members);
        }
    }
}
