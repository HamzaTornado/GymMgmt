using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using GymMgmt.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Subscriptions.GetAllSubscriptions
{
    public sealed record GetAllSubscriptionsQuery : IQuery<PaginatedList<SubscriptionDto>>
    {
        public SubscriptionStatus? Status { get; init; }
        public DateTime? StartDateFrom { get; init; }
        public DateTime? StartDateTo { get; init; }
        public DateTime? EndDateFrom { get; init; }
        public DateTime? EndDateTo { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public string? PlanName { get; init; }
        public bool? WillNotRenew { get; init; }
        public bool? IsActive { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string? SortBy { get; init; }
        public bool SortDescending { get; init; } = false;
    }

    // Default filters for common scenarios
    public static class SubscriptionFilters
    {
        public static GetAllSubscriptionsQuery ActiveSubscriptions => new()
        {
            Status = SubscriptionStatus.Active,
            IsActive = true
        };

        public static GetAllSubscriptionsQuery ExpiringThisWeek => new()
        {
            EndDateFrom = DateTime.Today,
            EndDateTo = DateTime.Today.AddDays(7),
            Status = SubscriptionStatus.Active
        };

        public static GetAllSubscriptionsQuery NonRenewing => new()
        {
            WillNotRenew = true,
            Status = SubscriptionStatus.Active
        };

        public static GetAllSubscriptionsQuery CancelledSubscriptions => new()
        {
            Status = SubscriptionStatus.Cancelled
        };
    }
}
