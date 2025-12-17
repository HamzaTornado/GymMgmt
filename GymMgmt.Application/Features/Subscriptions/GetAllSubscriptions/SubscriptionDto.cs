using GymMgmt.Domain.Common.Enums;


namespace GymMgmt.Application.Features.Subscriptions.GetAllSubscriptions
{
    public sealed record SubscriptionDto
    {
        public Guid Id { get; init; }
        public Guid MemberId { get; init; }
        public string MemberFullName { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string PlanName { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public SubscriptionStatus Status { get; init; }
        public bool WillNotRenew { get; init; }
        public DateTimeOffset Created { get; init; }
        public string? CreatedBy { get; init; }
    }
}
