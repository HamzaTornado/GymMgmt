using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.CancelSubscriptionRenewal
{
    internal class CancelSubscriptionRenewalValidator : AbstractValidator<CancelSubscriptionRenewalCommand>
    {
        public CancelSubscriptionRenewalValidator()
        {
            RuleFor(v => v.MemberId)
                .NotEmpty().WithMessage("Member ID is required.");
        }
    }
}
