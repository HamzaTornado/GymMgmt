using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Entities.Members;
using AutoMapper;
using GymMgmt.Application.Features.Members.DeleteMember;
using MediatR;


namespace AuthSystem.Application.Features.Memebers.DeleteMember
{
    public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, bool>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public DeleteMemberCommandHandler(IMemberRepository memberRepository,IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            var memberId = MemberId.FromValue(request.Id);
            var member = await _memberRepository.FindByIdAsync(memberId, cancellationToken)
                ?? throw new NotFoundException(nameof(Member), memberId);

             _memberRepository.Delete(member);

            return true;
        }

    }
}
