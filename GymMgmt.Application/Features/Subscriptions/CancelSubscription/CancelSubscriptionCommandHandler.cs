using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Entities.Members;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.CancelSubscription
{
    internal class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, bool>
    {
        private readonly IMemberRepository _memberRepository;

        public CancelSubscriptionCommandHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<bool> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            // 1. Load the Member aggregate
            var member = await _memberRepository.FindByIdAsync(MemberId.FromValue(request.MemberId), cancellationToken);
            if (member == null)
            {
                throw new NotFoundException($"Member with ID {request.MemberId} Not Found");
            }

            // 2. Call the domain logic
            // The NoActiveSubscriptionException is handled by the aggregate
            member.CancelCurrentSubscription();

            // 3. Return
            // The UnitOfWorkBehavior will save the change of status
            // on the member's Subscription entity.

            return true;
        }
    }
}
