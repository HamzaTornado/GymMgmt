using FluentValidation;

namespace GymMgmt.Application.Features.Subscriptions.StartSubscription
{
    internal class StartSubscriptionValidator : AbstractValidator<StartSubscriptionCommand>
    {
        public StartSubscriptionValidator()
        {
            RuleFor(v => v.MemberId)
                .NotEmpty().WithMessage("Member ID is required.");

            RuleFor(v => v.MembershipPlanId)
                .NotEmpty().WithMessage("Membership Plan ID is required.");
        }
    }
}
