using GymMgmt.Domain.Common.Enums;


namespace GymMgmt.Application.Features.Subscriptions.GetAllSubscriptions
{
    public sealed record SubscriptionDto
    {
        public Guid Id { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string PlanName { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public SubscriptionStatus Status { get; init; }
        public bool WillNotRenew { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public string? CreatedBy { get; init; }
    }
}
