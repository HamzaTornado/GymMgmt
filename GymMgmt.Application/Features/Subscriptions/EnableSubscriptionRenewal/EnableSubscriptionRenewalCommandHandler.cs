

using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using MediatR;

namespace GymMgmt.Application.Features.Subscriptions.EnableSubscriptionRenewal
{
    internal class EnableSubscriptionRenewalCommandHandler : IRequestHandler<EnableSubscriptionRenewalCommand, bool>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberShipPlanRepository _planRepository;

        public EnableSubscriptionRenewalCommandHandler(
            IMemberRepository memberRepository,
            IMemberShipPlanRepository planRepository)
        {
            _memberRepository = memberRepository;
            _planRepository = planRepository;
        }
        public async Task<bool> Handle(EnableSubscriptionRenewalCommand request, CancellationToken ct)
        {
            var member = await _memberRepository.FindByIdAsync(MemberId.FromValue(request.MemberId), ct) 
                ?? throw new NotFoundException($"Member with ID {request.MemberId} Not Found");
            // Explicitly calls the Domain Method we defined
            member.EnableCurrentSubscriptionRenewal();

            return true;
        }
    }
}
