using GymMgmt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.StartSubscription
{
    public sealed record StartSubscriptionCommand(
        Guid MemberId,
        Guid MembershipPlanId,
        DateTime? StartDate = null,
        string? PaymentReference = null) : ICommand<Guid>;
}
