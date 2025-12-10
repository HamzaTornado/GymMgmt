using GymMgmt.Application.Common.Models;
using GymMgmt.Domain.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.GetAllSubscriptions
{
    public record GetSubscriptionsQuery(
    string? SearchTerm,
    List<SubscriptionStatus>? Statuses,
    string? SortColumn,       // e.g. "Price"
    string? SortDirection,    // "ASC" or "DESC"
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<SubscriptionDto>>;
}
