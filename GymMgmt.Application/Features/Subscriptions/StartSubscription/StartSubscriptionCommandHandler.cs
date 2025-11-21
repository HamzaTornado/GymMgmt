using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using MediatR;

namespace GymMgmt.Application.Features.Subscriptions.StartSubscription
{
    internal class StartSubscriptionCommandHandler: IRequestHandler<StartSubscriptionCommand,Guid>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberShipPlanRepository _planRepository;
        private readonly IClubSettingsRepository _clubSettingsRepository;
        public StartSubscriptionCommandHandler(
            IMemberRepository memberRepository,
            IMemberShipPlanRepository planRepository,
            IClubSettingsRepository clubSettingsRepository)
        {
            _memberRepository = memberRepository;
            _planRepository = planRepository;
            _clubSettingsRepository = clubSettingsRepository;
        }
        public async Task<Guid> Handle(StartSubscriptionCommand request, CancellationToken cancellationToken)
        {
            // 1. Load all required entities
            var member = await _memberRepository.FindByIdAsync(MemberId.FromValue(request.MemberId), cancellationToken);
            if (member == null)
            {
                throw new NotFoundException($"Member with ID {request.MemberId} Not Found");
            }

            var plan = await _planRepository.FindByIdAsync(MembershipPlanId.FromValue(request.MembershipPlanId), cancellationToken);
            if (plan == null)
            {
                throw new NotFoundException($"MembershipPlan with ID {request.MembershipPlanId} Not Found");
            }

            var settings = await _clubSettingsRepository.GetSingleOrDefaultAsync(cancellationToken);
            if (settings == null)
            {
                throw new NotFoundException("Club settings not Configured yet");
            }

            // 2. Call the first domain logic: Create the subscription
            var subscription = member.StartSubscription(
                plan,
                settings.CurrentInsuranceFee,
                settings.IsInsuranceFeeRequired,
                request.StartDate);

            // 3. Call the second domain logic: Record the payment for it
            var paymentDate = request.StartDate ?? DateTime.Now;
            member.RecordSubscriptionPayment(
                subscription,
                paymentDate,
                subscription.StartDate,
                subscription.EndDate,
                request.PaymentReference);

            // 4. Return the new subscription's ID
            // The UnitOfWorkBehavior will save both the new Subscription 
            // and the new Payment in one transaction.

            return subscription.Id.Value;
        }
    }
}
