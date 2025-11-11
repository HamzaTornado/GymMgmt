using AutoMapper;
using GymMgmt.Application.Features.Members.GetMemberById;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.Members;
using MediatR;


namespace GymMgmt.Application.Features.Members.CreateMember
{
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, ReadMemberDto>
    {

        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public CreateMemberCommandHandler(IMemberRepository memberRepository, IMapper mapper) 
        {

            _memberRepository = memberRepository;
            _mapper = mapper;
        }
        public async Task<ReadMemberDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = Member.Create(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email,
                _mapper.Map<Address>(request.Address)
            );

            await _memberRepository.AddAsync(member,cancellationToken);

            return _mapper.Map<ReadMemberDto>(member);
        }
    }
}
