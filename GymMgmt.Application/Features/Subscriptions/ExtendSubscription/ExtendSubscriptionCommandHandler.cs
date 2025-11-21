using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using MediatR;


namespace GymMgmt.Application.Features.Subscriptions.ExtendSubscription
{
    internal class ExtendSubscriptionCommandHandler : IRequestHandler<ExtendSubscriptionCommand, bool>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberShipPlanRepository _planRepository;

        public ExtendSubscriptionCommandHandler(
            IMemberRepository memberRepository,
            IMemberShipPlanRepository planRepository)
        {
            _memberRepository = memberRepository;
            _planRepository = planRepository;
        }

        public async Task<bool> Handle(ExtendSubscriptionCommand request, CancellationToken cancellationToken)
        {
            // 1. Load the Member aggregate
            var member = await _memberRepository.FindByIdAsync(MemberId.FromValue(request.MemberId), cancellationToken);
            if (member == null)
            {
                throw new NotFoundException($"Member with ID {request.MemberId} Not Found");
            }

            // 2. Load the plan to extend with
            var plan = await _planRepository.FindByIdAsync(MembershipPlanId.FromValue(request.ExtensionPlanId), cancellationToken);
            if (plan == null)
            {
                throw new NotFoundException($"MembershipPlan with ID {request.ExtensionPlanId} Not Found");
            }

            // 3. Call the first domain logic: Extend the subscription
            // This will throw NoActiveSubscriptionException if none is active
            var(newPeriodStart, newPeriodEnd)= member.ExtendCurrentSubscription(plan);

            // 4. Find the just-extended subscription to pass to the payment method
            var activeSubscription = member.Subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active);

            // This should never happen if ExtendCurrentSubscription succeeded, but it's safe to check
            if (activeSubscription == null)
            {
                // This indicates a potential logic error if ExtendCurrentSubscription didn't throw
                throw new InvalidOperationException("Failed to find active subscription after extension.");
            }

            // 5. Call the second domain logic: Record the payment
            // We use the subscription's *new* price (set by the .Extend method)
            var paymentDate = DateTime.Now;
            member.RecordSubscriptionPayment(
                activeSubscription,
                paymentDate,
                newPeriodStart,
                newPeriodEnd,
                request.PaymentReference);

            // 6. Return
            // The UnitOfWorkBehavior will save the changes to the Subscription
            // and create the new Payment in one transaction.

            return true;
        }
    }
}
