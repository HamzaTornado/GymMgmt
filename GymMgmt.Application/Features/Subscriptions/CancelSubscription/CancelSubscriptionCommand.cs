using GymMgmt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.CancelSubscription
{
    public record CancelSubscriptionCommand(Guid MemberId) : ICommand<bool>;
}
