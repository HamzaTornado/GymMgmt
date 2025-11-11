using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Entities.Plans;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.GetMemberShipPlanById
{
    internal class GetMemberShipPlanByIdQueryHandler : IRequestHandler<GetMemberShipPlanByIdQuery,ReadMemberShipPlanDto>
    {
        private readonly IMemberShipPlanRepository _memberShipPlanRepository;
        private readonly IMapper _mapper;

        public GetMemberShipPlanByIdQueryHandler(
            IMemberShipPlanRepository memberShipPlanRepository,
            IMapper mapper)
        {
            _memberShipPlanRepository = memberShipPlanRepository;
            _mapper = mapper;
        }
        public async Task<ReadMemberShipPlanDto> Handle(GetMemberShipPlanByIdQuery query,CancellationToken ct)
        {

            var membership =await _memberShipPlanRepository.FindByIdAsync(MembershipPlanId.FromValue(query.MemberShipPlanId),ct) 
                ?? throw new NotFoundException();

            return _mapper.Map<ReadMemberShipPlanDto>(membership);


        }
    }
}
