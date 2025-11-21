using GymMgmt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.ExtendSubscription
{
    public record ExtendSubscriptionCommand(
        Guid MemberId,
        Guid ExtensionPlanId,
        string? PaymentReference = null) : ICommand<bool>;
}
