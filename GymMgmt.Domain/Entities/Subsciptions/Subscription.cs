using GymMgmt.Domain.Common;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Entities.Plans;
using GymMgmt.Domain.Exceptions;


namespace GymMgmt.Domain.Entities.Subsciptions
{
    public class Subscription : AuditableEntity<SubscriptionId>
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string PlanName { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public SubscriptionStatus Status { get; private set; }
        public bool WillNotRenew { get; private set; } = false;

        private Subscription() { }

        private Subscription(
        SubscriptionId id,
        DateTime startDate,
        DateTime endDate,
        MembershipPlan plan)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            PlanName = plan.Name;
            Price = plan.Price;
            Status = SubscriptionStatus.Active;
        }

        public static Subscription Create(
        SubscriptionId id,
        DateTime startDate,
        MembershipPlan plan)
        {
            // 1. Normalize start date: Nov 12, 2025 00:00:00
            var normalizedStartDate = startDate.Date;

            // 2. Calculate provisional end date using plan: Dec 11, 2025 00:00:00
            var provisionalEndDate = plan.CalculateEndDate(normalizedStartDate);

            // 3. Normalize end date to the end of that day: Dec 11, 2025 23:59:59...
            var normalizedEndDate = provisionalEndDate.Date.AddDays(1).AddTicks(-1);

            return new Subscription(id, normalizedStartDate, normalizedEndDate, plan);

        }
        public void Activate()
        {
            if (Status == SubscriptionStatus.Active)
                throw new SubscriptionAlreadyActiveException(Id.Value);

            if (EndDate < DateTime.Today)
                throw new SubscriptionExpiredException(Id.Value);

            Status = SubscriptionStatus.Active;
        }
        public void Revoke()
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value, "revoke");

            Status = SubscriptionStatus.Cancelled;

        }

        public (DateTime NewPeriodStart, DateTime NewPeriodEnd) Extend(MembershipPlan extensionPlan)
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value, "extend");

            var extensionStartDate = this.EndDate.Date.AddDays(1);
            var provisionalNewEndDate = extensionPlan.CalculateEndDate(extensionStartDate);
            var normalizedNewEndDate = provisionalNewEndDate.Date.AddDays(1).AddTicks(-1);

            // Set the new EndDate
            EndDate = normalizedNewEndDate;
            Price = extensionPlan.Price;
            PlanName = $"Extended: {extensionPlan.Name}";

            // Return the period we just calculated
            return (extensionStartDate, normalizedNewEndDate);
        }
        public void CancelAtPeriodEnd()
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value, "cancel at end of period");

            WillNotRenew = true;
        }
        /// <summary>
        /// Re-enables renewal for a subscription that was "soft cancelled".
        /// </summary>
        public void EnableRenewal()
        {
            if (!WillNotRenew)
                throw new SubscriptionRenewalAlreadyEnabledException(Id.Value);

            WillNotRenew = false;
        }
        public bool IsActiveOn(DateTime date) =>
            Status == SubscriptionStatus.Active &&
            StartDate <= date &&
            EndDate >= date;
        public bool IsInGracePeriod(int graceDays)
        {
            return Status == SubscriptionStatus.Active &&
                   DateTime.Today > EndDate &&
                   DateTime.Today <= EndDate.AddDays(graceDays);
        }
        public override string ToString() =>
            $"{PlanName}: {StartDate:yyyy-MM-dd} → {EndDate:yyyy-MM-dd} [{Status}]";
    }
}

