using AutoMapper;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;
using GymMgmt.Domain.Entities.Plans;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.CreateMembership
{
    internal class CreateMemberShipPlanCommandHandler : IRequestHandler<CreateMemberShipPlanCommand,ReadMemberShipPlanDto>
    {
        private readonly IMemberShipPlanRepository _memberShipPlanRepository;
        private readonly IMapper _mapper;

        public CreateMemberShipPlanCommandHandler(
            IMemberShipPlanRepository memberShipPlanRepository,
            IMapper mapper
            )
        {
            _memberShipPlanRepository = memberShipPlanRepository;
            _mapper = mapper;  
        }

        public async Task<ReadMemberShipPlanDto> Handle(CreateMemberShipPlanCommand command,CancellationToken ct)
        {
            var membership = MembershipPlan.Create(
                    MembershipPlanId.New(),
                    command.Name,
                    command.DurationInMonths,
                    command.Price
                );

             await _memberShipPlanRepository.AddAsync( membership ,ct);

            return _mapper.Map<ReadMemberShipPlanDto>( membership );
        }
    }
}
