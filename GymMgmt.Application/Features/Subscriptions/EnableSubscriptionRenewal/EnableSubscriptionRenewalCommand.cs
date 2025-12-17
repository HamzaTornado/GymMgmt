using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.EnableSubscriptionRenewal
{
    public record EnableSubscriptionRenewalCommand(Guid MemberId) : IRequest<bool>;
}
