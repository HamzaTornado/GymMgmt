using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.RevokeSubscription
{
    public record RevokeSubscriptionCommand(Guid MemberId) : IRequest<bool>;
}
