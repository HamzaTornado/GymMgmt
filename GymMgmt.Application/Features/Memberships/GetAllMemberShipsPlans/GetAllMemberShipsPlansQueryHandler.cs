using AutoMapper;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;
using GymMgmt.Domain.Entities.Plans;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.GetAllMemberShipsPlans
{
    internal class GetAllMemberShipsPlansQueryHandler : IRequestHandler<GetAllMemberShipsPlansQuery,IEnumerable<ReadMemberShipPlanDto>>
    {
        private readonly IMemberShipPlanRepository _memberShipPlanRepository;
        private readonly IMapper _mapper;

        public GetAllMemberShipsPlansQueryHandler(
            IMemberShipPlanRepository memberShipPlanRepository,
            IMapper mapper)
        {
            _memberShipPlanRepository = memberShipPlanRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ReadMemberShipPlanDto>> Handle(GetAllMemberShipsPlansQuery query,CancellationToken ct)
        {
            var memmbershipsplans = await _memberShipPlanRepository.GetAllAsync(ct);

            if(memmbershipsplans == null)
            {
                return Enumerable.Empty<ReadMemberShipPlanDto>();
            }

            return _mapper.Map<IEnumerable<ReadMemberShipPlanDto>>(memmbershipsplans);
        }
    }
}
