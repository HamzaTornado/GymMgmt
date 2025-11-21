using FluentValidation;


namespace GymMgmt.Application.Features.Subscriptions.ExtendSubscription
{
    internal class ExtendSubscriptionValidator : AbstractValidator<ExtendSubscriptionCommand>
    {
        public ExtendSubscriptionValidator()
        {
            RuleFor(v => v.MemberId)
                .NotEmpty().WithMessage("Member ID is required.");

            RuleFor(v => v.ExtensionPlanId)
                .NotEmpty().WithMessage("Extension Plan ID is required.");
        }
    }
}
