using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;
using GymMgmt.Domain.Entities.Plans;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.UpdateMembershipPlan
{
    internal class UpdateMemberShipPlanCommandHandler : IRequestHandler<UpdateMemberShipPlanCommand,ReadMemberShipPlanDto>
    {
        private readonly IMemberShipPlanRepository _memberShipPlanRepository;
        private readonly IMapper _mapper;

        public UpdateMemberShipPlanCommandHandler(
            IMemberShipPlanRepository memberShipPlanRepository,
            IMapper mapper)
        {
            _memberShipPlanRepository = memberShipPlanRepository;
            _mapper = mapper;
        }

        public async Task<ReadMemberShipPlanDto> Handle(UpdateMemberShipPlanCommand command, CancellationToken ct)
        {
            var membership = await _memberShipPlanRepository.FindByIdAsync(MembershipPlanId.FromValue(command.Id), ct)
                ?? throw new NotFoundException();

            membership.Update(command.Name, command.DurationInDays, command.Price);

            return _mapper.Map<ReadMemberShipPlanDto>(membership);
        }

    }
}
