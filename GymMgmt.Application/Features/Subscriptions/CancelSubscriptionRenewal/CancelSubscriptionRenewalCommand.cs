using GymMgmt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.CancelSubscriptionRenewal
{
    public record CancelSubscriptionRenewalCommand(Guid MemberId) : ICommand<bool>;
}
